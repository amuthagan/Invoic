using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class PartSubmissionWarrantViewModel : ViewModelBase
    {
        //private Model.PartSubmissionWarrantModel _model_PartSubmission;
        private readonly ICommand _onAvailChkCommand;
        public ICommand OnAvailChkCommand { get { return this._onAvailChkCommand; } }

        private PartSubmissionWarrantBll partSubmissionBll;
        public PartSubmissionWarrantViewModel(UserInformation userInformation, PartSubmissionWarrantModel pm)
        {
            PartSubmissionWarrantModel psw = new PartSubmissionWarrantModel();
            partSubmissionBll = new PartSubmissionWarrantBll(userInformation);
            PARTSUBMISSIONWARRANT = new PartSubmissionWarrantModel();
            PARTSUBMISSIONWARRANT.PART_NO = pm.PART_NO;
            PARTSUBMISSIONWARRANT = pm;
            partSubmissionBll.GetPSW(PARTSUBMISSIONWARRANT);
            partSubmissionBll.GetBuyerCode(PARTSUBMISSIONWARRANT);
            NAME = partSubmissionBll.GetNamePSW(PARTSUBMISSIONWARRANT);
            Application = partSubmissionBll.GetApplicationPSW(PARTSUBMISSIONWARRANT);
            SetDropDown();
            // PARTSUBMISSIONWARRANT = new PartSubmissionWarrantModel();
            //pm.NAME = partSubmissionBll.GetNamePSW(PARTSUBMISSIONWARRANT);
            this.PARTSUBMISSIONWARRANT = pm;
            //PARTSUBMISSIONWARRANT.DATE1 = DateTime.Today.ToString("dd-MM-yyyy");
            #region Event Init
            this.pswPreviewCommand = new DelegateCommand(this.PSWPreviewReport);
            this.pswCancelCommand = new DelegateCommand(this.PSWCancelReport);
            this.selectchangecombocommandtitle = new DelegateCommand(this.SelectDataRowTitle);
            this.selectchangecombocommandApplication = new DelegateCommand(this.SelectDataRowApplication);
            this._onAvailChkCommand = new DelegateCommand(this.AvailChkCommand);
            #endregion

        }

        private void AvailChkCommand()
        {
            if (PARTSUBMISSIONWARRANT.OTHER == false)
            {
                PARTSUBMISSIONWARRANT.OTHERTXT = "";
                IsReadonlyComp = true;
                NotifyPropertyChanged("PARTSUBMISSIONWARRANT");
            }
            else if (PARTSUBMISSIONWARRANT.OTHER == true)
            {
                IsReadonlyComp = false;
                NotifyPropertyChanged("PARTSUBMISSIONWARRANT");
            }


        }

        private bool _isReadonlyComp = true;
        public bool IsReadonlyComp
        {
            get { return _isReadonlyComp; }
            set
            {
                this._isReadonlyComp = value;
                NotifyPropertyChanged("IsReadonlyComp");
            }
        }

        private DataView _name;
        public DataView NAME
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("NAME");
            }
        }

        private DataView _application;
        public DataView Application
        {
            get { return _application; }
            set
            {
                _application = value;
                NotifyPropertyChanged("Application");
            }
        }

        private PartSubmissionWarrantModel _partsubmissionwarrant;
        public PartSubmissionWarrantModel PARTSUBMISSIONWARRANT
        {
            get
            {
                return _partsubmissionwarrant;
            }
            set
            {
                _partsubmissionwarrant = value;
                NotifyPropertyChanged("PARTSUBMISSIONWARRANT");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderApplication;
        public ObservableCollection<DropdownColumns> DropDownHeaderApplication
        {
            get
            {
                return _dropDownHeaderApplication;
            }
            set
            {
                this._dropDownHeaderApplication = value;
                NotifyPropertyChanged("DropDownHeaderApplication");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderName;
        public ObservableCollection<DropdownColumns> DropDownHeaderName
        {
            get
            {
                return _dropDownHeaderName;
            }
            set
            {
                this._dropDownHeaderName = value;
                NotifyPropertyChanged("DropDownHeaderName");
            }
        }
        private void SetDropDown()
        {
            try
            {
                DropDownHeaderName = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "NAME", ColumnDesc = "Name", ColumnWidth = "1*" }
                 };
                DropDownHeaderApplication = new ObservableCollection<DropdownColumns>
                {
                    new DropdownColumns { ColumnName = "NewApplication", ColumnDesc = "Application", ColumnWidth = "1*" }
                };
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



        private readonly ICommand pswPreviewCommand;
        public ICommand PSWPreviewCommand { get { return this.pswPreviewCommand; } }
        private void PSWPreviewReport()
        {
            //PartSubmissionWarrantModel pswm = new PartSubmissionWarrantModel();
            if (PARTSUBMISSIONWARRANT.CSQSRYES == true && PARTSUBMISSIONWARRANT.CSQSRNO == false)
            {
                //partSubmissionBll.GetBIFPSW(PARTSUBMISSIONWARRANT);
                DataSet ds = new DataSet();
                DataTable dt = null;
                List<PartSubmissionWarrantModel> li = new List<PartSubmissionWarrantModel>();
                li.Add(PARTSUBMISSIONWARRANT);
                dt = li.ToDataTableWithType<PartSubmissionWarrantModel>();
                if (dt != null)
                {

                    dt.Columns.Add("NAME");
                    dt.Columns.Add("Application");
                    dt.Rows[0]["NAME"] = PARTSUBMISSIONWARRANT.PUTNAME.ToString();
                    dt.Rows[0]["Application"] = PARTSUBMISSIONWARRANT.PUTAPPLICATION.ToString();
                    dt.AcceptChanges();
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {

                    frmReportViewer rv = new frmReportViewer(dt, "PartSubmissionWarrant");
                    rv.ShowDialog();
                }
            }
            else if (PARTSUBMISSIONWARRANT.CSQSRYES == false && PARTSUBMISSIONWARRANT.CSQSRNO == true)
            {
                ShowWarningMessage("Confirm verification of CSQSR for this Part? Please Check to proceed...", MessageBoxButton.OK);
            }
        }

        private DataRowView _selectedrowtitle;
        public DataRowView SelectedRowTitle
        {
            get
            {
                return _selectedrowtitle;
            }

            set
            {
                _selectedrowtitle = value;
                NotifyPropertyChanged("SelectedRowTitle");
            }
        }
        private DataRowView _selectedrowApplication;
        public DataRowView SelectedRowApplication
        {
            get
            {
                return _selectedrowApplication;
            }

            set
            {
                _selectedrowApplication = value;
                NotifyPropertyChanged("SelectedRowApplication");
            }
        }
        private readonly ICommand selectchangecombocommandtitle;
        public ICommand SelectChangeComboCommandTitle { get { return this.selectchangecombocommandtitle; } }
        private void SelectDataRowTitle()
        {
            if (SelectedRowTitle != null)
            {
                PARTSUBMISSIONWARRANT.TITLE = SelectedRowTitle["TITLE"].ToString();
            }
        }
        private readonly ICommand selectchangecombocommandApplication;
        public ICommand SelectChangeComboCommandApplication { get { return this.selectchangecombocommandApplication; } }
        private void SelectDataRowApplication()
        {
            if (SelectedRowApplication != null)
            {
                PARTSUBMISSIONWARRANT.PUTAPPLICATION = SelectedRowApplication["NewApplication"].ToString();
            }
        }
        private bool _categoryisfocused = false;
        public bool CategoryIsFocused
        {
            get { return _categoryisfocused; }
            set
            {
                _categoryisfocused = value;
                NotifyPropertyChanged("CategoryIsFocused");
            }
        }

        public Action CloseAction { get; set; }

        private readonly ICommand pswCancelCommand;
        public ICommand PSWCancelCommand { get { return this.pswCancelCommand; } }
        private void PSWCancelReport()
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
    }
}
