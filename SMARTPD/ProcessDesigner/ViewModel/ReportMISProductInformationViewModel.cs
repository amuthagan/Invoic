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

namespace ProcessDesigner
{
    class ReportMISProductInformationViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "PRODUCT INFORMATION REPORT";
        private const string REPORT_NAME = "PRODUCT_INFORMATION_REPORT";
        private const string REPORT_TITLE = "Product Information Report";

        ReportMISProductInformationWise bll = null;
        WPF.MDI.MdiChild mdiChild = null;

        public ReportMISProductInformationViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, PRD_MAST productMaster = null, ReportMISProductInformationModel productInformationModel = null, DDCUST_MAST customerMaster = null, List<DDLOC_MAST> lstLocation = null, bool refreshOnLoad = false, string title = REPORT_TITLE)
        {
            _userInformation = userInformation;
            this.mdiChild = mdiChild;

            bll = new ReportMISProductInformationWise(userInformation);
            MandatoryFields = new ReportMISProductInformationModel();
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

            Items = new Dictionary<string, object>();
            SelectedItems = new Dictionary<string, object>();
            SelectedItems.Add("All", "All");

            foreach (DDLOC_MAST locationMaster in bll.GetLocationDetails())
            {
                Items.Add(locationMaster.LOCATION + "(" + locationMaster.LOC_CODE + ")", locationMaster.LOC_CODE);
                SelectedItems.Add(locationMaster.LOCATION + "(" + locationMaster.LOC_CODE + ")", locationMaster.LOC_CODE);
            }

            DataTable dtPGCategory = bll.GetProductPGCategoryByPrimaryKey().ToDataTable<PG_CATEGORY>();
            if (dtPGCategory.Columns.Contains("PG_CAT_DESC"))
                dtPGCategory.Columns["PG_CAT_DESC"].ColumnName = "PG_CATEGORY";
            ProductPGCategoryDataSource = dtPGCategory.DefaultView;



            ProductPGCategoryDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PG_CAT", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "PG_CATEGORY", ColumnDesc = " Product Group Category", ColumnWidth = "75*" }
                        };



            this.productPGCategorySelectedItemChangedCommand = new DelegateCommand(this.productPGCategoryChanged);
            this.productPGCategoryEndEditCommand = new DelegateCommand(this.productPGCategoryEndEdit);

            //Items.Add("Padi", "M");
            //Items.Add("KPM", "K");
            //Items.Add("Pondy", "Y");

            //SelectedItems.Add("All", "All");
            //SelectedItems.Add("Padi", "M");
            //SelectedItems.Add("KPM", "K");
            //SelectedItems.Add("Pondy", "Y");

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
            DataSet dsReport = null;

            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            frmReportViewer reportViewer = new frmReportViewer(dsReport, "FRCS");
            if (!reportViewer.ReadyToShowReport) return;
            reportViewer.ShowDialog();
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshClickCommand { get { return this.refreshCommand; } }
        private void RefreshSubmitCommand()
        {
            string pgCategory = "";
            ProductPGCategoryDataSource.RowFilter = "PG_CATEGORY='" + MandatoryFields.PG_CATEGORY + "'";
            if (ProductPGCategoryDataSource.Count > 0)
            {
                pgCategory = ProductPGCategoryDataSource[0]["PG_CAT"].ToValueAsString();
            }
            ProductPGCategoryDataSource.RowFilter = null;

            PRD_MAST productMaster = new PRD_MAST()
            {
                PART_NO = MandatoryFields.PART_NO,
                PART_DESC = MandatoryFields.PART_DESC,
                QUALITY = MandatoryFields.QUALITY,
                PG_CATEGORY = pgCategory,
                PSW_ST = pwsStatus
            };

            DDCUST_MAST customerMaster = new DDCUST_MAST() { CUST_CODE = (selectedCustomer.IsNotNullOrEmpty() ? selectedCustomer.CUST_CODE : -999999) };

            List<DDLOC_MAST> lstLocation = null;
            if (SelectedItems.IsNotNullOrEmpty())
            {
                lstLocation = new List<DDLOC_MAST>();
                foreach (object loc in SelectedItems.Values)
                {
                    lstLocation.Add(new DDLOC_MAST() { LOC_CODE = loc.ToValueAsString() });
                }
            }

            DataSet dsReport = bll.GetAllPartNos(productMaster, customerMaster, lstLocation);
            MandatoryFields.GRID_TITLE = REPORT_TITLE;
            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count <= 0)
            {
                return;
            }

            MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
            MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";

        }

        private ReportMISProductInformationModel _mandatoryFields = null;
        public ReportMISProductInformationModel MandatoryFields
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
                    selectedCustomer = lstEntity[0];
                }
            }
        }

        private DDCUST_MAST selectedCustomer { get; set; }
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

        public enum RadioButtons { YES, NO, None, All }

        private string pwsStatus = "All";

        private RadioButtons _selectedRadioButton = RadioButtons.All;
        public RadioButtons SelectedRadioButton
        {
            get
            {
                return _selectedRadioButton;
            }
            set
            {
                try
                {
                    if (Enum.GetName(value.GetType(), value).IsNotNullOrEmpty())
                        pwsStatus = Enum.GetName(value.GetType(), value);
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                _selectedRadioButton = value;
                OnPropertyChanged("SelectedRadioButton");
            }
        }


        private Dictionary<string, object> _items;
        public Dictionary<string, object> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
            }
        }

        private Dictionary<string, object> _selectedItems;
        public Dictionary<string, object> SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                _selectedItems = value;
                NotifyPropertyChanged("SelectedItems");
            }
        }

        #region Product PGCategory
        private DataView _productPGCategory = null;
        public DataView ProductPGCategoryDataSource
        {
            get
            {
                return _productPGCategory;
            }
            set
            {
                _productPGCategory = value;
                NotifyPropertyChanged("ProductPGCategoryDataSource");
            }
        }

        private DataRowView _productPGCategorySelectedRow;
        public DataRowView ProductPGCategorySelectedRow
        {
            get
            {
                return _productPGCategorySelectedRow;
            }

            set
            {
                _productPGCategorySelectedRow = value;
            }
        }

        private Visibility _productPGCategoryHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductPGCategoryHasDropDownVisibility
        {
            get { return _productPGCategoryHasDropDownVisibility; }
            set
            {
                _productPGCategoryHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductPGCategoryHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productPGCategoryDropDownItems;
        public ObservableCollection<DropdownColumns> ProductPGCategoryDropDownItems
        {
            get
            {
                return _productPGCategoryDropDownItems;
            }
            set
            {
                _productPGCategoryDropDownItems = value;
                OnPropertyChanged("ProductPGCategoryDropDownItems");
            }
        }

        private readonly ICommand productPGCategorySelectedItemChangedCommand;
        public ICommand ProductPGCategorySelectedItemChangedCommand { get { return this.productPGCategorySelectedItemChangedCommand; } }
        private void productPGCategoryChanged()
        {
            //CopyMandatoryFieldsToEntity(ProductPGCategoryDataSource, ref _productPGCategorySelectedRow, "PG_CAT", "PG_CATEGORY", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productPGCategoryEndEditCommand;
        public ICommand ProductPGCategoryEndEditCommand { get { return this.productPGCategoryEndEditCommand; } }
        private void productPGCategoryEndEdit()
        {
            //productPGCategoryChanged();
            //CopyMandatoryFieldsToEntity(ProductPGCategoryDataSource, ProductPGCategorySelectedRow, "PG_CAT", "PG_CATEGORY", MandatoryFields, ActiveEntity);
        }
        #endregion

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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
    }
}
