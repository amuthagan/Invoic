using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProcessDesigner.DAL;
using System.Data;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.ComponentModel.Composition;
using ProcessDesigner.UserControls;
using System.Resources;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    [Export]
    public partial class frmLogin : Window
    {
        Login loggedon = null;
        string conStr = "ENGGDB";
        public string ApplicationTitle = "SmartPD";
        private string sVersion = "Production V5.4";
        private ActiveUsersBLL bll;
        private Thread splashThread = null;

        public frmLogin()
        {
            splashThread = new Thread(new ThreadStart(threadStartingPoint));
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.IsBackground = true;
            splashThread.Start();
            Stopwatch stopwatch = Stopwatch.StartNew();

            InitializeComponent();
            ResourceManager myManager;
            string isConstrAvail = string.Empty;
            try
            {

                myManager = new ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                conStr = myManager.GetString("ConnectionString");
                isConstrAvail = System.Configuration.ConfigurationManager.ConnectionStrings[conStr].ToString();
            }
            catch (Exception ex)
            {
                ex.LogException();
                splashThread.Abort();
                if (string.IsNullOrEmpty(isConstrAvail))
                {
                    ShowErrorMessage(PDMsg.DBNotConnected);
                }
                btnClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            loggedon = new Login(conStr);
            if (!loggedon.Dal.IsNotNullOrEmpty())
            {
                splashThread.Abort();
                ShowErrorMessage(PDMsg.DBNotConnected);
                btnClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }

            int remaingtime = 5000 - stopwatch.ElapsedMilliseconds.ToString().ToIntValue();
            stopwatch.Stop();
            if (remaingtime > 0) Thread.Sleep(remaingtime);

            splashThread.Abort();

            //if (loggedon.IsValidVersionNo(sVersion) == false && Assembly.GetExecutingAssembly().IsRelease())
            //{
            //    ShowErrorMessage(PDMsg.VersionMisMatch(sVersion));
            //    this.Close();
            //}

            rbtDD.IsChecked = true;
            this.Topmost = true;
            this.Topmost = false;

            Txtusername.Focus();
            //Txtusername.Text = "jaya";
            //TxtPassword.Password = "jaya";
            //btnOk.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            //txtLastUpdate.Text = DateTime.Now.Date.ToString("dd.MM.yyyy") + " Ver.Release III /D&D/SFL";
            txtLastUpdate.Text = "SmartPD " + " 03/Oct/2018" + " Production V5.4/D&D/SFL";
            grdCapsLock.Visibility = Keyboard.IsKeyToggled(Key.CapsLock) ? Visibility.Visible : Visibility.Hidden;
        }

        private void threadStartingPoint()
        {
            SplashScreen splashscreen = new SplashScreen();
            splashscreen.txtProcess.Text = "Version : " + sVersion;
            splashscreen.ShowInTaskbar = false;
            splashscreen.ShowDialog();
            System.Windows.Threading.Dispatcher.Run();
        }

        string userName, password;
        MessageBoxResult result;
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                userName = Txtusername.Text;
                //password = TxtPassword.Password.ToString().EncryptPassword();
                password = TxtPassword.Password;

                var focusedControl = Keyboard.FocusedElement;

                if (userName.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("User name"));
                    Txtusername.Focus();
                    return;
                }

                if (TxtPassword.Password.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Password"));
                    TxtPassword.Focus();
                    return;
                }

                UserInformation _userInformation = new UserInformation();
                _userInformation.UserName = userName;
                _userInformation.Dal = loggedon.Dal;
                _userInformation.SFLPDDatabase = loggedon.DB;

                bool isForceToChangePassword;
                bool isValid = loggedon.IsValidUser(userName, password, out isForceToChangePassword);
                if (isValid)
                {
                    _userInformation.UserRole = loggedon.GetUserRole(userName);
                    _userInformation.Version = loggedon.GetVersion();
                    if (string.IsNullOrEmpty(_userInformation.UserRole))
                    {
                        result = ShowInformationMessage(PDMsg.RoleNotDefined);
                        return;
                    }
                    Application.Current.Properties.Add("userinfo", _userInformation);
                    isForceToChangePassword = false;
                    if (isForceToChangePassword)
                    {
                        ProcessDesigner.frmChangePassword changePassword = new ProcessDesigner.frmChangePassword(_userInformation);
                        changePassword.Owner = this;
                        changePassword.ShowInTaskbar = false;
                        changePassword.txtOldPassword.Focus();
                        changePassword.ShowDialog();

                        bool isPasswordChanged = changePassword.IsUserVerified;
                        if (!isPasswordChanged) App.Current.Shutdown();
                    }

                    ProcessDesigner.MainWindow mw = new ProcessDesigner.MainWindow(_userInformation);
                    bool isAdmin = loggedon.GetisAdmin(userName);
                    if (isAdmin)
                    {
                        mw.miSecurity.Visibility = Visibility.Visible;
                        mw.miPartNumberConfiguration.Visibility = Visibility.Visible;
                        mw.miExhibit.Visibility = Visibility.Visible;
                        mw.miLocationMaster.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        mw.miSecurity.Visibility = Visibility.Collapsed;
                        mw.miPartNumberConfiguration.Visibility = Visibility.Collapsed;
                        mw.miExhibit.Visibility = Visibility.Collapsed;
                        mw.miLocationMaster.Visibility = Visibility.Collapsed;
                    }

                    StatusMessage.StatusBarDetails = mw.stMain;
                    StatusMessage.UserName = userName;
                    StatusMessage.setStatus("Ready");
                    string hostname = Dns.GetHostName();
                    //  string ip = Dns.GetHostByName(hostname)[0].ToString();
                    string ip = Dns.GetHostByName(hostname).AddressList[0].ToString();
                    string username = _userInformation.UserName;
                    bll = new ActiveUsersBLL(_userInformation);
                    bll.LogIn(username, ip, hostname);
                    mw.Show();
                    this.Close();
                }
                else
                {
                    result = ShowInformationMessage(PDMsg.WrongUNamePassword);
                    return;
                }


            }
            catch (Exception ex)
            {
                ShowInformationMessage(PDMsg.WrongUNamePassword);
                ex.LogException();
                return;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblPasswater.Visibility = TxtPassword.Password.Length > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Txtusername_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TxtPassword.Focus();
            }

        }
        private void TxtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && result != MessageBoxResult.OK)
            {
                btnOk.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.Enter && result == MessageBoxResult.OK)
            {
                result = MessageBoxResult.None;
                return;
            }

        }

        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtPassword.Password = "";
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowErrorMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Error);
            return MessageBoxResult.None;
        }

        private void rbtOQA_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            UserInformation _userInformation = new UserInformation();
            string hostname = Dns.GetHostName();
            int maxLengthOfUserName = 15;
            _userInformation.UserName = hostname.IsNotNullOrEmpty() ? hostname.Substring(0, (hostname.Trim().Length > maxLengthOfUserName ? maxLengthOfUserName : hostname.Trim().Length)) : string.Empty;
            _userInformation.Dal = loggedon.Dal;
            _userInformation.SFLPDDatabase = loggedon.DB;
            _userInformation.UserRole = "OQA";
            _userInformation.Version = loggedon.GetVersion();

            string applicationTitle = "SmartPD - ";

            Window win = new Window();
            win.Title = Title = applicationTitle + "Operator Quality Assurance Chart";

            ProcessDesigner.frmOperatorQualityAssurance objRptOQA = new ProcessDesigner.frmOperatorQualityAssurance(_userInformation, win, -9999, OperationMode.AddNew, win.Title);
            System.Windows.Media.Imaging.IconBitmapDecoder ibd = new System.Windows.Media.Imaging.IconBitmapDecoder(new Uri(@"pack://application:,,/Images/logo.ico", UriKind.RelativeOrAbsolute), System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.Default);
            win.Icon = ibd.Frames[0];

            win.Content = objRptOQA;
            win.MinHeight = objRptOQA.Height + 50;
            win.MinWidth = objRptOQA.Width + 10;
            win.ShowInTaskbar = true;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.WindowState = WindowState.Maximized;
            win.ResizeMode = System.Windows.ResizeMode.CanResize;
            objRptOQA.Height = win.Height - 50;
            objRptOQA.Width = win.Width - 10;
            this.Close();
            win.Show();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            grdCapsLock.Visibility = Keyboard.IsKeyToggled(Key.CapsLock) ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
