using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using System.Data;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
    class FlxReportsViewModel : ViewModelBase
    {
        UserInformation _userInformation;
        WPF.MDI.MdiChild _mdiChild;
        FlxReportsBll _flxReportsBll;

        public FlxReportsViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, Nullable<DateTime> startdate, Nullable<DateTime> enddate)
        {
            try
            {
                _userInformation = userInformation;
                _mdiChild = mdiChild;
                _flxReportsBll = new FlxReportsBll(_userInformation);
                LoadUserGrid();
                LoadOptionCombo();
                ReportCode = "PPAP";
                this.refreshDataCommand = new DelegateCommand(this.RefreshData);
                this.exportToExcelCommand = new DelegateCommand(this.ExportToExcel);
                if (UserList.Count > 0)
                {
                    SelectedUser = UserList[0];
                    NotifyPropertyChanged("SelectedUser");
                    StartDate = startdate;
                    EndDate = enddate;
                    RefreshData();
                    //HeaderDetails = message + cnt.ToString() + (cnt > 0 ? " Entries" : " Entry") + " found ";

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        List<SEC_USER_MASTER> _userList;
        public List<SEC_USER_MASTER> UserList
        {
            get
            {
                return _userList;
            }
            set
            {
                _userList = value;
                NotifyPropertyChanged("UserList");
            }
        }

        private SEC_USER_MASTER _selectedUser;
        public SEC_USER_MASTER SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged("SelectedUser");
            }
        }

        private DataView _reportCombo;
        public DataView ReportCombo
        {
            get
            {
                return _reportCombo;
            }
            set
            {
                _reportCombo = value;
                NotifyPropertyChanged("ReportCombo");
            }
        }

        private DataView _reportData;
        public DataView ReportData
        {
            get
            {
                return _reportData;
            }
            set
            {
                _reportData = value;
                NotifyPropertyChanged("ReportData");
            }
        }



        string _reportCode;
        public string ReportCode
        {
            get
            {
                return _reportCode;
            }
            set
            {
                _reportCode = value;
                NotifyPropertyChanged("ReportCode");
            }
        }

        private Nullable<DateTime> _startDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start date is required")]
        public Nullable<DateTime> StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }

        private Nullable<DateTime> _endDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "End date is required")]
        public Nullable<DateTime> EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                NotifyPropertyChanged("EndDate");
            }
        }

        string _headerDetails;
        public string HeaderDetails
        {
            get
            {
                return _headerDetails;
            }
            set
            {
                _headerDetails = value;
                NotifyPropertyChanged("HeaderDetails");
            }
        }


        private void LoadUserGrid()
        {
            try
            {
                UserList = _flxReportsBll.GetUserList();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void LoadOptionCombo()
        {
            DataTable dtCombo = new DataTable();
            dtCombo.Columns.Add("Code");
            dtCombo.Columns.Add("Description");
            try
            {
                InsertCombo(dtCombo, "PPAP", "PPAP Submitted");
                InsertCombo(dtCombo, "DR", "Documents Released");
                InsertCombo(dtCombo, "PNA", "Part Nos Allotted");
                ReportCombo = dtCombo.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void InsertCombo(DataTable dtCombo, string code, string description)
        {
            DataRow drRow;
            try
            {
                drRow = dtCombo.NewRow();
                drRow["Description"] = description;
                drRow["Code"] = code;
                dtCombo.Rows.Add(drRow);
                dtCombo.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private readonly ICommand refreshDataCommand;
        public ICommand RefreshDataCommand
        {
            get { return this.refreshDataCommand; }
        }
        private void RefreshData()
        {
            try
            {
                int cnt = 0;
                if (SelectedUser == null)
                {
                    return;
                }
                if (ReportCode.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Report Option"));
                    return;
                }

                if (StartDate.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Start Date"));
                    return;
                }

                if (EndDate.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("End Date"));
                    return;
                }

                if (StartDate > EndDate)
                {
                    ShowInformationMessage("Start Date is Greater than End Date,Please Check it.!");
                    return;
                }

                //ReportData = _flxReportsBll
                if (ReportCode == "PPAP") //PPAP Submitted
                {
                    ReportData = _flxReportsBll.PPAPSubmitted(SelectedUser.USER_NAME.ToUpper().Trim(), Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                }
                else if (ReportCode == "DR") //Document Released
                {
                    ReportData = _flxReportsBll.DocumentReleased(SelectedUser.USER_NAME.ToUpper().Trim(), Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                }
                else if (ReportCode == "PNA") //part Nos Allotted
                {
                    ReportData = _flxReportsBll.PartNoAllotted(SelectedUser.USER_NAME.ToUpper().Trim(), Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                }
                cnt = ReportData.Count;
                HeaderDetails = "Designers - " + SelectedUser.FULL_NAME + " - " + cnt.ToString() + (cnt > 0 ? " Entries" : " Entry") + " found ";
                //if (ReportData.Count == 0)
                //{
                //    ShowInformationMessage("No Entries Found!");
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand exportToExcelCommand;
        public ICommand ExportToExcelCommand
        {
            get { return this.exportToExcelCommand; }
        }
        private void ExportToExcel()
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".xlsx";
                dlg.Filter = "Excel File (*.xls, *.xlsx)|*.xls;*.xlsx";
                dlg.ShowDialog();
                if (dlg.FileName != "")
                {
                    DataTable dtExport;
                    dtExport = ReportData.ToTable().Copy();
                    dtExport.Columns.Remove("COLUMN3");
                    dtExport.Columns.Remove("COLUMN4");
                    dtExport.Columns.Remove("COLUMN5");
                    dtExport.Columns["COLUMN0"].ColumnName = "Part No";
                    dtExport.Columns["COLUMN1"].ColumnName = "Description";
                    dtExport.Columns["COLUMN2"].ColumnName = "Location";

                    dtExport.ExportToExcel(dlg.FileName);
                    ShowInformationMessage("Exported to Excel File Succesfully.");
                }
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



    }
}
