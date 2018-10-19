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
using ProcessDesigner.Common;
using ProcessDesigner.BLL;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmChangePassword.xaml
    /// </summary>
    public partial class frmChangePassword : Window
    {
        private ChangePassword clsChangePassword = null;
        public string ApplicationTitle = "SmartPD";
        private string userName { get; set; }
        bool changed = false;

        public bool IsUserVerified
        {
            get { return changed; }

        }

        public frmChangePassword(UserInformation userInformation)
        {
            InitializeComponent();
            clsChangePassword = new ChangePassword(userInformation);
            userName = userInformation.UserName;
            txtOldPassword.Focus();
            errOld.Visibility = Visibility.Visible;
            errNew.Visibility = Visibility.Visible;
            errVer.Visibility = Visibility.Visible;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            String message;


            message = "";
            try
            {

                if (txtOldPassword.Password.Trim().Length <= 0)
                {
                    MessageBox.Show("Existing Password should not be empty!", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtOldPassword.Focus();
                    errOld.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    errOld.Visibility = Visibility.Collapsed;

                }
                if (txtNewPassword.Password.Trim().Length <= 0)
                {
                    MessageBox.Show("New Password should not be empty!", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtNewPassword.Focus();
                    errNew.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    errNew.Visibility = Visibility.Collapsed;

                }
                if (txtVerifyPassword.Password.Trim().Length <= 0)
                {
                    MessageBox.Show("Verify Password should not be empty!", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtVerifyPassword.Focus();
                    errVer.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    errVer.Visibility = Visibility.Collapsed;

                }

                if (txtOldPassword.Password == txtNewPassword.Password)
                {
                    MessageBox.Show("Old & New Password should not be same!", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtVerifyPassword.Focus();
                    errVer.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    errVer.Visibility = Visibility.Collapsed;

                }

                //changed = clsChangePassword.ValidateAndUpdatePassword(userName, txtOldPassword.Password.ToString().EncryptPassword(), txtNewPassword.Password.ToString().EncryptPassword(), txtVerifyPassword.Password.ToString().EncryptPassword(), ref message);

                changed = clsChangePassword.ValidateAndUpdatePassword(userName, txtOldPassword.Password.ToString(),
                    txtNewPassword.Password.ToString(), txtVerifyPassword.Password.ToString(), ref message);

                if (changed == true)
                {
                    MessageBox.Show(message, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                    MessageBox.Show(message, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool isCancelClick = false;
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
            {
                isCancelClick = true;
                this.Close();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void txtOldPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtNewPassword.Focus();
            }
        }

        private void txtNewPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtVerifyPassword.Focus();
            }
        }

        private void txtVerifyPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOk.Focus();
            }
        }

        private void txtOldPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtOldPassword.Password.Trim().Length <= 0)
            {
                txtOldPassword.Focus();
                errOld.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                errOld.Visibility = Visibility.Collapsed;

            }
        }

        private void txtNewPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNewPassword.Password.Trim().Length <= 0)
            {
                txtNewPassword.Focus();
                errNew.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                errNew.Visibility = Visibility.Collapsed;

            }
        }

        private void txtVerifyPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtVerifyPassword.Password.Trim().Length <= 0)
            {
                txtVerifyPassword.Focus();
                errVer.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                errVer.Visibility = Visibility.Collapsed;

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!isCancelClick)
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
