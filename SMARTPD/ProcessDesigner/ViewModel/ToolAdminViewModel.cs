using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Windows.Input;
using System.Data;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using ProcessDesigner.BLL;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PreviewHandlers;
using System.Windows.Media.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Threading;

namespace ProcessDesigner.ViewModel
{
    class ToolAdminViewModel : ViewModelBase
    {
        public PreviewContainer PreviewDrawing { get; set; }
        public Image PreviewImage { get; set; }

        private ToolAdminModel toolAdminModel;
        private ToolAdminBll toolAdminBll;
        private readonly ICommand _selectionChanged;
        public ICommand OnSelectionChanged { get { return this._selectionChanged; } }
        private readonly ICommand _addCommand;
        public ICommand AddCommand { get { return this._addCommand; } }
        private readonly ICommand _editCommand;
        public ICommand EditCommand { get { return this._editCommand; } }
        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return this._saveCommand; } }
        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }
        private readonly ICommand _deleteCommand;
        public ICommand DeleteCommand { get { return this._deleteCommand; } }
        private readonly ICommand _rowEditEndingCommand;
        public ICommand RowEditEndingCommand { get { return this._rowEditEndingCommand; } }
        private readonly ICommand _currentCellChangedCommand;
        public ICommand CurrentCellChangedCommand { get { return this._currentCellChangedCommand; } }
        private readonly ICommand _parametersClick;
        public ICommand ParametersClick { get { return this._parametersClick; } }
        private readonly ICommand _insertTemplateCommand;
        public ICommand InsertTemplateCommand { get { return this._insertTemplateCommand; } }


        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private Visibility _cmbButtonVisibility = Visibility.Visible;
        private bool _isReadOnlyFamilyCode = false;
        public Action CloseAction { get; set; }
        private ObservableCollection<DropdownColumns> _dropdownHeaders = null;
        Canvas cvCanvas = new Canvas();
        private bool isupdated = false;

        public ToolAdminViewModel(UserInformation userInformation)
        {
            toolAdminModel = new ToolAdminModel();
            toolAdminBll = new ToolAdminBll(userInformation);

            this._selectionChanged = new DelegateCommand<object>(this.SelectionChanged);
            this._parametersClick = new DelegateCommand<object>(this.ParameterMouseClick);
            this._addCommand = new DelegateCommand<object>(this.Add);
            this._editCommand = new DelegateCommand<object>(this.Edit);
            this._saveCommand = new DelegateCommand(this.Save);
            this._closeCommand = new DelegateCommand(this.Close);
            this._insertTemplateCommand = new DelegateCommand(this.ShowInsertTemplate);
            this._rowEditEndingCommand = new DelegateCommand<object>(this.RowEditEnding);
            this._currentCellChangedCommand = new DelegateCommand<object>(this.CurrentCellChanged);
            this._deleteCommand = new DelegateCommand<object>(this.DeleteParameter);
            toolAdminBll.GetToolFamily(ToolAdmin);
            GetRights();
            AddButtonIsEnable = true;
            EditButtonIsEnable = false;
            CmbButtonVisibility = Visibility.Visible;
            IsReadOnlyFamilyCode = true;
            ToolAdmin.FAMILY_CD = "";
            ToolAdmin.FAMILY_NAME = "";
            setRights();
            ToolAdmin.Mode = OperationMode.Edit;
            ToolAdmin.ImageChanged = false;
            DropdownHeaders = new ObservableCollection<DropdownColumns>
            {
                new DropdownColumns { ColumnName = "FAMILY_CD", ColumnDesc = "Family Code", ColumnWidth = "2*" },
                new DropdownColumns { ColumnName = "FAMILY_NAME", ColumnDesc = "Family Name", ColumnWidth = "3*", IsDefaultSearchColumn = true }
            };
        }

        public ToolAdminModel ToolAdmin
        {
            get { return this.toolAdminModel; }
            set
            {
                this.toolAdminModel = value;
                NotifyPropertyChanged("ToolAdmin");
            }
        }


        public bool AddButtonIsEnable
        {
            get { return _addButtonIsEnable; }
            set
            {
                this._addButtonIsEnable = value;
                NotifyPropertyChanged("AddButtonIsEnable");
            }
        }

        public bool EditButtonIsEnable
        {
            get { return _editButtonIsEnable; }
            set
            {
                this._editButtonIsEnable = value;
                NotifyPropertyChanged("EditButtonIsEnable");
            }
        }

        public bool SaveButtonIsEnable
        {
            get { return _saveButtonIsEnable; }
            set
            {
                this._saveButtonIsEnable = value;
                NotifyPropertyChanged("SaveButtonIsEnable");
            }
        }

        public Visibility CmbButtonVisibility
        {
            get { return _cmbButtonVisibility; }
            set
            {
                this._cmbButtonVisibility = value;
                NotifyPropertyChanged("CmbButtonVisibility");
            }
        }

        public bool IsReadOnlyFamilyCode
        {
            get { return _isReadOnlyFamilyCode; }
            set
            {
                this._isReadOnlyFamilyCode = value;
                NotifyPropertyChanged("IsReadOnlyFamilyCode");
            }
        }

        private bool _isFocusedFamilyCode = false;
        public bool IsFocusedFamilyCode
        {
            get { return _isFocusedFamilyCode; }
            set
            {
                this._isFocusedFamilyCode = value;
                NotifyPropertyChanged("IsFocusedFamilyCode");
            }
        }

        public ObservableCollection<DropdownColumns> DropdownHeaders
        {
            get { return this._dropdownHeaders; }
            set
            {
                this._dropdownHeaders = value;
                NotifyPropertyChanged("DropdownHeaders");
            }

        }

        private string _headertext = "List of Parameters";
        public string HeaderText
        {
            get { return _headertext; }
            set
            {
                _headertext = value;
                NotifyPropertyChanged("HeaderText");
            }
        }

        private void SelectionChanged(object parameters)
        {
            try
            {
                Progress.Start();

                if (ToolAdmin.DVToolFamily != null && ToolAdmin.SelectedFamily != null && ToolAdmin.DVToolFamily.Count > 0)
                {
                    ToolAdmin.FAMILY_CD = ToolAdmin.SelectedFamily["FAMILY_CD"].ToString();
                    ToolAdmin.picture = null;
                    PreviewImage.Source = null;
                    ToolAdmin.File_Name = "";
                    ToolAdmin.ImageChanged = false;
                    HeaderText = "List of Parameters for " + ToolAdmin.FAMILY_NAME;
                    toolAdminBll.GetToolParameter(ToolAdmin);
                    cvCanvas = (Canvas)parameters;
                    cvCanvas.Children.Clear();
                    if (ToolAdmin.DVToolParameter != null && ToolAdmin.DVToolParameter.Count > 0)
                    {
                        foreach (DataRowView drv in ToolAdmin.DVToolParameter)
                        {
                            if (drv["PARAMETER_CD"].IsNotNullOrEmpty())
                            {
                                TextBlock tb = new TextBlock();
                                tb.Name = drv["PARAMETER_CD"].ToString();
                                tb.Text = drv["PARAMETER_CD"].ToString();
                                tb.ToolTip = drv["PARAMETER_NAME"].ToString();
                                tb.Background = Brushes.LightGray;
                                tb.SetValue(Canvas.LeftProperty, ConvertTwipsToPixels(drv["X_COORDINATE"]));
                                tb.SetValue(Canvas.TopProperty, ConvertTwipsToPixels(drv["Y_COORDINATE"]));
                                cvCanvas.Children.Add(tb);
                            }
                        }
                        ToolAdmin.SelectedParameter = ToolAdmin.DVToolParameter[0];
                        if (ToolAdmin.picture != null)
                        {
                            //ToolAdmin.File_Name="E:\\File2.vsd";
                            //frmSaveVisio saveVisio = new frmSaveVisio();
                            //saveVisio.Show();
                            //saveVisio.SaveFile(ToolAdmin.File_Name, "E:\\File2.vsd");
                            if (SaveFile() == true)
                            {
                                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                                MemoryStream strm = new MemoryStream();
                                //strm = new FileStream(ToolAdmin.DisplayFile_Name, FileMode.Open, FileAccess.Read);

                                using (FileStream file = new FileStream(ToolAdmin.DisplayFile_Name, FileMode.Open, FileAccess.Read))
                                {
                                    byte[] bytes = new byte[file.Length];
                                    file.Read(bytes, 0, (int)file.Length);
                                    strm.Write(bytes, 0, (int)file.Length);
                                    file.Close();
                                    file.Dispose();
                                }
                                strm.Seek(0, SeekOrigin.Begin);
                                bitmap.BeginInit();
                                bitmap.StreamSource = strm;
                                bitmap.EndInit();
                                PreviewImage.Source = bitmap;
                                //strm.Close();
                            }
                            else
                            {
                                PreviewImage.Source = null;
                            }
                        }
                        else
                        {
                            PreviewImage.Source = null;
                        }

                    }
                }
                else
                {
                    HeaderText = "List of Parameters";
                }

                Progress.End();

            }
            catch (System.IO.FileFormatException)
            {
                MessageBox.Show("Unable to load image.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;
               
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                {
                    closingev.Cancel = true;
                    e = closingev;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private Boolean SaveFile()
        {
            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            try
            {
                if (ToolAdmin.FileType.ToUpper() == "VSD")
                {
                    if (CheckVisioIsInstalled() == false)
                    {
                        Progress.End();
                        ShowInformationMessage("Please install Visio and try again!");
                        return false;
                    }
                    if (ToolAdmin.MimeType.ToString().IndexOf("application") >= 0)
                    {
                        app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                        //if (app == null)
                        //{
                        //    ShowInformationMessage("Please install Visio and try again!");
                        //    return false;
                        //}
                        doc = app.Documents.Open(ToolAdmin.File_Name);
                        page = app.ActivePage;
                        page.Export(ToolAdmin.DisplayFile_Name);
                        app.Quit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    app.Quit();
                    return false;
                }
                catch (Exception ex1)
                {
                    return false;
                    throw ex1.LogException();
                }
                throw ex.LogException();
            }
        }

        private bool CheckVisioIsInstalled()
        {
            try
            {
                Type visioType = Type.GetTypeFromProgID("Visio.Application");
                if (visioType == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
            return true;
        }


        public bool DoesExtensionExist(ref string ext)
        {
            //open the HKEY_CLASSES_ROOT hive
            RegistryKey root = Registry.ClassesRoot;

            try
            {
                //open the subkey for this extension
                RegistryKey key = root.OpenSubKey(ext);

                //check if it's null, if no return false
                if (key == null)
                    return false;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
            return true;
        }

        private void ParameterMouseClick(object parameters)
        {
            try
            {

                if (parameters != null && ToolAdmin.SelectedParameter != null)
                {
                    Point mousePos = Mouse.GetPosition((IInputElement)parameters);
                    Canvas cv = (Canvas)parameters;

                    foreach (FrameworkElement parameter in cv.Children)
                    {
                        if (parameter is TextBlock)
                        {
                            TextBlock tb = (TextBlock)parameter;
                            if (tb.Name == ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString())
                            {
                                tb.SetValue(Canvas.LeftProperty, mousePos.X);
                                tb.SetValue(Canvas.TopProperty, mousePos.Y);
                                ToolAdmin.SelectedParameter["X_COORDINATE"] = ConvertPixelsToTwips(mousePos.X);
                                ToolAdmin.SelectedParameter["Y_COORDINATE"] = ConvertPixelsToTwips(mousePos.Y);
                            }
                        }
                    }
                }
                IsFocusedFamilyCode = true;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public static double ConvertTwipsToPixels(object twips)
        {
            int twip = 0;
            try
            {
                twip = Convert.ToInt32(twips);
            }
            catch (Exception)
            {
                twip = 0;
            }

            if (twip != 0)
            {
                return (twip / 15);
            }
            else return 0.0;
        }

        public static int ConvertPixelsToTwips(object pixels)
        {
            double twip = 0;
            try
            {
                twip = Convert.ToDouble(pixels);
            }
            catch (Exception)
            {
                twip = 0;
            }

            if (twip != 0)
            {
                return Convert.ToInt16(twip * 15);
            }
            else return 0;
        }

        private void Add(object parameters)
        {
            try
            {
                if (AddButtonIsEnable == false) return;

                if (!isupdated && (ToolAdmin.FAMILY_CD.IsNotNullOrEmpty() || ToolAdmin.FAMILY_NAME.IsNotNullOrEmpty()))
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        if (!isupdated) return;
                    }
                }

                isupdated = false;
                AddButtonIsEnable = false;
                EditButtonIsEnable = true;
                ToolAdmin.Mode = OperationMode.AddNew;
                CmbButtonVisibility = Visibility.Collapsed;
                IsReadOnlyFamilyCode = false;
                ToolAdmin.FAMILY_CD = "";
                ToolAdmin.FAMILY_NAME = "";
                ToolAdmin.picture = null;
                PreviewImage.Source = null;
                setRights();
                toolAdminBll.GetToolFamily(ToolAdmin);
                cvCanvas = (Canvas)parameters;
                cvCanvas.Children.Clear();
                IsFocusedFamilyCode = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Edit(object parameters)
        {
            try
            {
                if (EditButtonIsEnable == false) return;
                if (!isupdated && (ToolAdmin.FAMILY_CD.IsNotNullOrEmpty() || ToolAdmin.FAMILY_NAME.IsNotNullOrEmpty()))
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        if (!isupdated) return;
                    }                   
                }

                isupdated = false;
                AddButtonIsEnable = true;
                EditButtonIsEnable = false;
                CmbButtonVisibility = Visibility.Visible;
                IsReadOnlyFamilyCode = true;
                setRights();
                ToolAdmin.Mode = OperationMode.Edit;
                ToolAdmin.FAMILY_CD = "";
                ToolAdmin.FAMILY_NAME = "";
                toolAdminBll.GetToolFamily(ToolAdmin);
                Canvas cv = (Canvas)parameters;
                cv.Children.Clear();
                IsFocusedFamilyCode = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void Save()
        {
            try
            {

                if (SaveButtonIsEnable == false) return;

                IsFocusedFamilyCode = true;
                isupdated = false;

                if (ToolAdmin.FAMILY_CD.ToString() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Family code"));
                    IsFocusedFamilyCode = true;                    
                    return;
                }

                if (ToolAdmin.FAMILY_NAME.ToString() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Family name"));
                    IsFocusedFamilyCode = true;                   
                    return;
                }


                if (AddButtonIsEnable == false)
                {
                    if (CheckDuplicateCode() == true)
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Family code"));
                        IsFocusedFamilyCode = true;
                        return;
                    }

                    if (CheckDuplicateFamilyName() == true)
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Family name"));
                        IsFocusedFamilyCode = true;
                        return;
                    }

                }

                if (toolAdminBll.CheckDuplicate(ToolAdmin.DVToolParameter.ToTable(), "PARAMETER_CD"))
                {
                    MessageBox.Show("Duplicate parameter code should not be allowed!", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    IsFocusedFamilyCode = true;
                    return;
                }


                if (CheckCodeIsEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Parameter code"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    IsFocusedFamilyCode = true;
                    return;
                }

                if (CheckCodeIsValid() == false)
                {
                    MessageBox.Show("Parameter code is not valid", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    IsFocusedFamilyCode = true;
                    return;
                }


                if (ToolAdmin.FAMILY_CD.ToString() != "")
                {
                    bool result = false;
                    Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    Progress.Start();
                    
                    result = toolAdminBll.UpdateToolAdmin(ToolAdmin);
                    isupdated = true;
                    Progress.End();
                    if (ToolAdmin.Mode == OperationMode.AddNew)
                        ShowInformationMessage(PDMsg.SavedSuccessfully);
                    else
                        ShowInformationMessage(PDMsg.UpdatedSuccessfully);

                    AddButtonIsEnable = true;
                    Add(cvCanvas);
                }                

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckDuplicateCode()
        {
            try
            {
                if (toolAdminBll.CheckDuplicateCode(ToolAdmin.FAMILY_CD) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckDuplicateFamilyName()
        {
            try
            {
                if (toolAdminBll.CheckDuplicateFamilyName(ToolAdmin.FAMILY_NAME) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckCodeIsEmpty()
        {
            try
            {
                bool isempty = false;
                foreach (DataRowView drv in ToolAdmin.DVToolParameter)
                {
                    if (!drv["PARAMETER_CD"].IsNotNullOrEmpty() && (drv["PARAMETER_NAME"].IsNotNullOrEmpty() || drv["DATATYPE"].IsNotNullOrEmpty() || drv["DEFAULT_VISIBLE"].IsNotNullOrEmpty() || drv["DEFAULT_VALUE"].IsNotNullOrEmpty()))
                    {
                        isempty = true;
                        break;
                    }
                }

                return isempty;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckCodeIsValid()
        {
            try
            {
                bool isvalid = true;
                foreach (DataRowView drv in ToolAdmin.DVToolParameter)
                {
                    if (drv["PARAMETER_CD"].IsNotNullOrEmpty() && CodeIsValid(drv["PARAMETER_CD"].ToString()) == false)
                    {
                        isvalid = false;
                        break;
                    }
                }

                return isvalid;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CodeIsValid(string code)
        {
            try
            {
                if (!code.IsNotNullOrEmpty()) return true;
                bool isvalid = true;

                foreach (char c in code)
                {
                    string cd = c.ToString();

                    Match match = Regex.Match(cd, "[A-Za-z0-9]", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        isvalid = false;
                        break;
                    }

                }


                //Regex regex = new Regex("[a-z0-1]", RegexOptions.IgnoreCase); //regex that matches allowed text
                //if (!regex.IsMatch(code)) isvalid = false;

                return isvalid;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void RowEditEnding(object parameters)
        {
            try
            {
                if (ToolAdmin.SelectedParameter != null)
                {
                    if (!ToolAdmin.SelectedParameter["PARAMETER_CD"].IsNotNullOrEmpty() || ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString().Trim() == "" || !ToolAdmin.SelectedParameter["PARAMETER_NAME"].IsNotNullOrEmpty()) return;


                    if (ToolAdmin.SelectedParameter["DATATYPE"].ToString() == "")
                    {
                        ToolAdmin.SelectedParameter["DATATYPE"] = 0;
                    }

                    if (ToolAdmin.SelectedParameter["DEFAULT_VISIBLE"].ToString() == "")
                    {
                        ToolAdmin.SelectedParameter["DEFAULT_VISIBLE"] = 0;
                    }

                    Point mousePos = Mouse.GetPosition((IInputElement)parameters);
                    Canvas cv = (Canvas)parameters;
                    var children = cv.Children;
                    TextBlock obj = (from TextBlock child in children where child.Name == ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString() select child).FirstOrDefault();
                    if (obj == null)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Name = ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString();
                        tb.Text = ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString();
                        tb.ToolTip = ToolAdmin.SelectedParameter["PARAMETER_NAME"].ToString();
                        tb.Background = Brushes.LightGray;
                        tb.SetValue(Canvas.LeftProperty, mousePos.X);
                        tb.SetValue(Canvas.TopProperty, mousePos.Y);
                        cv.Children.Add(tb);
                        ToolAdmin.SelectedParameter["X_COORDINATE"] = ConvertPixelsToTwips(mousePos.X);
                        ToolAdmin.SelectedParameter["Y_COORDINATE"] = ConvertPixelsToTwips(mousePos.Y);
                    }

                    AddNewRecord();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void CurrentCellChanged(object parameters)
        {
            try
            {
                if (ToolAdmin.SelectedParameter != null)
                {
                    if (!ToolAdmin.SelectedParameter["PARAMETER_CD"].IsNotNullOrEmpty()) return;


                    Point mousePos = Mouse.GetPosition((IInputElement)parameters);
                    Canvas cv = (Canvas)parameters;
                    var children = cv.Children;
                    TextBlock obj = (from TextBlock child in children where child.Name == ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString() select child).FirstOrDefault();
                    if (obj == null)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Name = ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString();
                        tb.Text = ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString();
                        tb.ToolTip = ToolAdmin.SelectedParameter["PARAMETER_NAME"].ToString();
                        tb.Background = Brushes.LightGray;
                        tb.SetValue(Canvas.LeftProperty, mousePos.X);
                        tb.SetValue(Canvas.TopProperty, mousePos.Y);
                        cv.Children.Add(tb);
                        ToolAdmin.SelectedParameter["X_COORDINATE"] = ConvertPixelsToTwips(mousePos.X);
                        ToolAdmin.SelectedParameter["Y_COORDINATE"] = ConvertPixelsToTwips(mousePos.Y);
                    }
                    else if (obj != null)
                    {
                        obj.ToolTip = ToolAdmin.SelectedParameter["PARAMETER_NAME"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private RolePermission _actionPermission;
        public RolePermission ActionPermission
        {
            get { return _actionPermission; }
            set
            {
                _actionPermission = value;
                NotifyPropertyChanged("ActionPermission");
            }
        }

        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = toolAdminBll.GetUserRights("TOOL ADMINISTRATION MASTER");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
        }


        private void Close()
        {
            try
            {
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void ShowInsertTemplate()
        {
            string fileName = "";
            string ext = "";
            string showfilename = "";

            if (ToolAdmin.FAMILY_CD.ToValueAsString() == "")
            {
                //MessageBox.Show("Please enter the family code to insert the template!", "");
                //return;
            }

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            try
            {
                dlg.Filter = "Image Files (*.bmp, *.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Visio Files(*.vsd,*.vsdx)|*.vsd;*.vsdx";
                dlg.ShowDialog();
                if (dlg.FileName != "")
                {
                    fileName = dlg.FileName;
                    ext = Path.GetExtension(fileName);
                    ToolAdmin.File_Name = fileName;
                    if (ext.ToUpper() == ".VSDX" || ext.ToUpper() == ".VSD")
                    {
                        string visioInstall = "";
                        showfilename = CreateVSDFile(fileName, ref visioInstall);
                        if (visioInstall != "")
                        {
                            ShowInformationMessage(visioInstall);
                            return;
                        }
                        ShowImage(showfilename);
                        ToolAdmin.ImageChanged = true;
                    }
                    else
                    {
                        ShowImage(fileName);
                        ToolAdmin.picture = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(fileName));
                        //File.Copy(filename, System.AppDomain.CurrentDomain.BaseDirectory + "" + filewoext, true);
                        ToolAdmin.ImageChanged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                throw ex.LogException();
            }
        }

        private string CreateVSDFile(string filename, ref string visioInstall)
        {
            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            string extension = ".vsd";
            string filewoext = "";
            try
            {

                if (filename.Length > 0)
                {
                    visioInstall = "";
                    if (CheckVisioIsInstalled() == false)
                    {
                        //ShowInformationMessage("Please install Visio and try again!");
                        visioInstall = "Please install Visio and try again!";
                        return "";
                    }
                    filewoext = Path.GetFileNameWithoutExtension(filename);
                    ToolAdmin.picture = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(filename));
                    File.Copy(filename, System.AppDomain.CurrentDomain.BaseDirectory + "" + filewoext, true);
                    app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                    //if (app == null)
                    //{
                    //    visioInstall = "Please install Visio and try again!";
                    //    return "";
                    //}
                    doc = app.Documents.Open(System.AppDomain.CurrentDomain.BaseDirectory + "" + filewoext);
                    page = app.ActivePage;
                    page.Export(System.AppDomain.CurrentDomain.BaseDirectory + "" + filewoext + ".bmp");
                    app.Quit();
                }
                return System.AppDomain.CurrentDomain.BaseDirectory + "" + filewoext + ".bmp";
            }
            catch (Exception ex)
            {
                try
                {
                    if (app != null)
                    {
                        app.Quit();
                    }
                    throw ex.LogException();
                }
                catch (Exception ex1)
                {
                    return "";
                    throw ex1.LogException();
                }
                throw ex.LogException();
            }
        }

        private void ShowImage(string filename)
        {
            try
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                System.IO.Stream strm;
                strm = new FileStream(filename, FileMode.Open, FileAccess.Read);
                bitmap.BeginInit();
                bitmap.StreamSource = strm;
                bitmap.EndInit();
                PreviewImage.Source = bitmap;
                //strm.Close();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void DeleteParameter(object parameters)
        {
            try
            {

                if (ToolAdmin.SelectedParameter != null && ToolAdmin.SelectedParameter["PARAMETER_CD"].IsNotNullOrEmpty())
                {

                    MessageBoxResult msgResult = ShowWarningMessage(PDMsg.BeforeDelete("Parameter ") + ToolAdmin.SelectedParameter[1].ToString(), MessageBoxButton.YesNo);
                    //MessageBox.Show("Do you want to delete Parameter " + ToolAdmin.SelectedParameter[1].ToString(), "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (msgResult == MessageBoxResult.Yes)
                    {
                        string rowid = ToolAdmin.SelectedParameter["ROWID"].ToString();
                        if (rowid.IsNotNullOrEmpty())
                        {
                            ToolAdmin.DTDeletedRecords.ImportRow(ToolAdmin.SelectedParameter.Row);
                        }

                        if (ToolAdmin.SelectedParameter["PARAMETER_CD"].IsNotNullOrEmpty())
                        {
                            Canvas cv = (Canvas)parameters;
                            var children = cv.Children;
                            TextBlock obj = (from TextBlock child in children where child.Name == ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString() select child).FirstOrDefault();
                            if (obj != null)
                            {
                                cv.Children.Remove(obj);
                            }
                        }

                        ToolAdmin.SelectedParameter.Delete();
                        ShowInformationMessage(PDMsg.DeletedSuccessfully);

                        AddNewRecord();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddNewRecord()
        {
            if (ToolAdmin.DVToolParameter.Count == 0 || !HaveEmptyRow())
            {
                ToolAdmin.DVToolParameter.AddNew();
            }
        }

        private bool HaveEmptyRow()
        {

            foreach (DataRowView drv in ToolAdmin.DVToolParameter)
            {
                if (!drv["PARAMETER_CD"].IsNotNullOrEmpty()) return true;
            }
            return false;
        }


        private string oldParameterCD = "";
        public void OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

            if (e.Column.SortMemberPath == "PARAMETER_CD") oldParameterCD = ((System.Data.DataRowView)(e.Row.Item)).Row["PARAMETER_CD"].ToString();

        }


        public void OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            TextBox tb = e.EditingElement as TextBox;
            string columnName = e.Column.SortMemberPath;
            string rowid = ((System.Data.DataRowView)(e.Row.Item)).Row["ROWID"].ToString();
            string parCode = ((System.Data.DataRowView)(e.Row.Item)).Row["PARAMETER_CD"].ToString();
            string parName = ((System.Data.DataRowView)(e.Row.Item)).Row["PARAMETER_NAME"].ToString();

            if (columnName == "PARAMETER_CD")
            {
                if (rowid.IsNotNullOrEmpty() && !parCode.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Parameter code"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    tb.Text = oldParameterCD;
                    return;
                }

                if (parCode.IsNotNullOrEmpty() && parCode.Substring(0, 1).IsNumeric())
                {
                    MessageBox.Show("Parameter code should start with alphabet.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    tb.Text = oldParameterCD;
                    return;
                }

                if (parCode.IsNotNullOrEmpty() && CodeIsValid(parCode.ToString()) == false)
                {
                    MessageBox.Show("Parameter code is not valid.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    tb.Text = oldParameterCD;
                    return;
                }

                if (IsDuplicateParamCode(parCode))
                {
                    MessageBox.Show("Parameter Code should not be allow dublicate.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    tb.Text = oldParameterCD;
                    return;
                }
            }
        }

        public void FamilyName_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HeaderText = (ToolAdmin.FAMILY_NAME.IsNotNullOrEmpty()) ? "List of Parameters for " + ToolAdmin.FAMILY_NAME : "List of Parameters";
        }

        public bool IsDuplicateParamCode(string code)
        {

            if (code.Trim() == string.Empty) return false;


            DataView dv = ToolAdmin.DVToolParameter.ToTable().DefaultView;
            dv.RowFilter = "TRIM(PARAMETER_CD) = '" + code.Trim() + "'";
            if (dv.Count > 1) return true;

            return false;
        }
                
    }
}
