using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessDesigner.ViewModel
{
    class ReportMISCustomerPartNoWiseViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "CUSTOMER PARTNO WISE REPORT";
        private const string REPORT_NAME = "CUSTOMER_PARTNO_WISE_REPORT";
        private const string REPORT_TITLE = "Customer Partno Wise Report";

        ReportMISCustomerPartNoWise bll = null;
        WPF.MDI.MdiChild mdiChild = null;

        public ReportMISCustomerPartNoWiseViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, PRD_MAST productMaster = null, DDCI_INFO customerInfo = null, DDCUST_MAST customerMaster = null, bool refreshOnLoad = false, string title = REPORT_TITLE)
        {
            _userInformation = userInformation;
            this.mdiChild = mdiChild;

            bll = new ReportMISCustomerPartNoWise(userInformation);
            MandatoryFields = new ReportMISCustomerPartNoWiseModel();
            MandatoryFields.GRID_TITLE = REPORT_TITLE;

            CustomersDataSource = bll.GetCustomerDetails().ToDataTable<DDCUST_MAST>().DefaultView;
            this.customerSelectedItemChangedCommand = new DelegateCommand(this.CustomerChanged);
            CustomerDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "Customer", ColumnWidth = "75*", IsDefaultSearchColumn = true }
                        };

            SFLPartNumbersDataSource = bll.GetPartNumber().ToDataTable<PRD_MAST>().DefaultView;
            this.sflPartNumberSelectedItemChangedCommand = new DelegateCommand(this.SFLPartNumberChanged);
            SFLPartNumberDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "SFL Part Number", ColumnWidth = "1*" },
                        };


            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            this.refreshCommand = new DelegateCommand(this.RefreshSubmitCommand);

            CustomerPartNumbersDataSource = bll.GetCustomerPartNumber().ToDataTable<DDCI_INFO>().DefaultView;
            this.customerPartNumberSelectedItemChangedCommand = new DelegateCommand(this.CustomerPartNumberChanged);
            CustomerPartNumberDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "CUST_DWG_NO", ColumnDesc = "Customer Part Number", ColumnWidth = "1*" },
                        };

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

        private string _title = REPORT_TITLE;
        private string Title
        {
            get { return _title; }
            set
            {
                value = value.IsNotNullOrEmpty() ? value : REPORT_TITLE;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {
            PRD_MAST productMaster = new PRD_MAST() { PART_NO = MandatoryFields.PART_NO };
            DDCI_INFO customerInfo = new DDCI_INFO() { CUST_DWG_NO = MandatoryFields.CUST_DWG_NO };

            DataSet dsReport = bll.GetAllCustomerPartNos(productMaster, customerInfo, CustomerSelectedRow);
            MandatoryFields.GRID_TITLE = REPORT_TITLE;
            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            dsReport.DataSetName = REPORT_NAME;
            MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
            MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";
            DataRow row = dsReport.Tables[1].Rows.Add();

            row["ReportTitle"] = REPORT_TITLE;
            row.AcceptChanges();
            dsReport.Tables[1].AcceptChanges();

            //dsReport.WriteXmlSchema("D:\\" + dsReport.DataSetName + ".xml");

            frmReportViewer reportViewer = new frmReportViewer(dsReport, REPORT_NAME);
            if (!reportViewer.ReadyToShowReport) return;
            reportViewer.ShowDialog();
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshClickCommand { get { return this.refreshCommand; } }
        private void RefreshSubmitCommand()
        {
            PRD_MAST productMaster = new PRD_MAST() { PART_NO = MandatoryFields.PART_NO };
            DDCI_INFO customerInfo = new DDCI_INFO() { CUST_DWG_NO = MandatoryFields.CUST_DWG_NO };

            DataSet dsReport = bll.GetAllCustomerPartNos(productMaster, customerInfo, CustomerSelectedRow);
            MandatoryFields.GRID_TITLE = REPORT_TITLE;
            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count <= 0)
            {
                MandatoryFields.GridData = null;
                return;
            }

            MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
            MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";

        }

        private ReportMISCustomerPartNoWiseModel _mandatoryFields = null;
        public ReportMISCustomerPartNoWiseModel MandatoryFields
        {
            get
            {
                return _mandatoryFields;
            }
            set
            {
                _mandatoryFields = value;
                NotifyPropertyChanged("MandatoryFields");
            }
        }

        private DataView _customersDataSource = null;
        public DataView CustomersDataSource
        {
            get
            {
                return _customersDataSource;
            }
            set
            {
                _customersDataSource = value;
                NotifyPropertyChanged("CustomersDataSource");
            }
        }

        private DataRowView _customerSelectedRow;
        public DataRowView CustomerSelectedRow
        {
            get
            {
                return _customerSelectedRow;
            }

            set
            {
                _customerSelectedRow = value;
            }
        }

        private readonly ICommand customerSelectedItemChangedCommand;
        public ICommand CustomerSelectedItemChangedCommand { get { return this.customerSelectedItemChangedCommand; } }
        private void CustomerChanged()
        {
            if (_customerSelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetCustomerDetails(new DDCUST_MAST() { CUST_CODE = -99999.00m }).ToDataTable<DDCUST_MAST>().Clone();
                dt.ImportRow(_customerSelectedRow.Row);

                List<DDCUST_MAST> lstEntity = (from row in dt.AsEnumerable()
                                               select new DDCUST_MAST()
                                               {
                                                   CUST_CODE = row.Field<string>("CUST_CODE").ToIntValue(),
                                                   CUST_NAME = row.Field<string>("CUST_NAME"),
                                                   ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                   DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                               }).ToList<DDCUST_MAST>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    MandatoryFields.CUST_NAME = lstEntity[0].CUST_NAME;
                    DDCUST_MAST = lstEntity[0];
                }
            }
        }

        private Visibility _customerHasDropDownVisibility = Visibility.Visible;
        public Visibility CustomerHasDropDownVisibility
        {
            get { return _customerHasDropDownVisibility; }
            set
            {
                _customerHasDropDownVisibility = value;
                NotifyPropertyChanged("CustomerHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _customerDropDownItems;
        public ObservableCollection<DropdownColumns> CustomerDropDownItems
        {
            get
            {
                return _customerDropDownItems;
            }
            set
            {
                _customerDropDownItems = value;
                OnPropertyChanged("CustomerDropDownItems");
            }
        }

        private DataView _sflPartNumbersDataSource = null;
        public DataView SFLPartNumbersDataSource
        {
            get
            {
                return _sflPartNumbersDataSource;
            }
            set
            {
                _sflPartNumbersDataSource = value;
                NotifyPropertyChanged("SFLPartNumbersDataSource");
            }
        }

        private DataRowView _sflPartNumberSelectedRow;
        public DataRowView SFLPartNumberSelectedRow
        {
            get
            {
                return _sflPartNumberSelectedRow;
            }

            set
            {
                _sflPartNumberSelectedRow = value;
            }
        }

        private readonly ICommand sflPartNumberSelectedItemChangedCommand;
        public ICommand SFLPartNumberSelectedItemChangedCommand { get { return this.sflPartNumberSelectedItemChangedCommand; } }
        private void SFLPartNumberChanged()
        {
            if (_sflPartNumberSelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.PART_NO = SFLPartNumberSelectedRow.Row["PART_NO"].ToValueAsString();
            }
        }

        private Visibility _sflPartNumberHasDropDownVisibility = Visibility.Visible;
        public Visibility SFLPartNumberHasDropDownVisibility
        {
            get { return _sflPartNumberHasDropDownVisibility; }
            set
            {
                _sflPartNumberHasDropDownVisibility = value;
                NotifyPropertyChanged("SFLPartNumberHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _sflPartNumberDropDownItems;
        public ObservableCollection<DropdownColumns> SFLPartNumberDropDownItems
        {
            get
            {
                return _sflPartNumberDropDownItems;
            }
            set
            {
                _sflPartNumberDropDownItems = value;
                OnPropertyChanged("SFLPartNumberDropDownItems");
            }
        }

        private DataView _customerPartNumbersDataSource = null;
        public DataView CustomerPartNumbersDataSource
        {
            get
            {
                return _customerPartNumbersDataSource;
            }
            set
            {
                _customerPartNumbersDataSource = value;
                NotifyPropertyChanged("CustomerPartNumbersDataSource");
            }
        }

        private DataRowView _customerPartNumberSelectedRow;
        public DataRowView CustomerPartNumberSelectedRow
        {
            get
            {
                return _customerPartNumberSelectedRow;
            }

            set
            {
                _customerPartNumberSelectedRow = value;
            }
        }

        private readonly ICommand customerPartNumberSelectedItemChangedCommand;
        public ICommand CustomerPartNumberSelectedItemChangedCommand { get { return this.customerPartNumberSelectedItemChangedCommand; } }
        private void CustomerPartNumberChanged()
        {
            if (_customerPartNumberSelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.CUST_DWG_NO = _customerPartNumberSelectedRow.Row["CUST_DWG_NO"].ToValueAsString();
            }
        }

        private Visibility _customerPartNumberHasDropDownVisibility = Visibility.Visible;
        public Visibility CustomerPartNumberHasDropDownVisibility
        {
            get { return _customerPartNumberHasDropDownVisibility; }
            set
            {
                _customerPartNumberHasDropDownVisibility = value;
                NotifyPropertyChanged("CustomerPartNumberHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _customerPartNumberDropDownItems;
        public ObservableCollection<DropdownColumns> CustomerPartNumberDropDownItems
        {
            get
            {
                return _customerPartNumberDropDownItems;
            }
            set
            {
                _customerPartNumberDropDownItems = value;
                OnPropertyChanged("CustomerPartNumberDropDownItems");
            }
        }

        DDCUST_MAST DDCUST_MAST { get; set; }

    }
}
