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
    public class CopyCIReferenceViewModel : ViewModelBase
    {
        private FeasibleReportAndCostSheet bll = null;
        public CopyCIReferenceViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, DDCI_INFO oldActiveEntity,
            OperationMode operationMode)
        {

            bll = new FeasibleReportAndCostSheet(userInformation);

            EnquiryReceivedOn = bll.ServerDateTime();

            OldCIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
            CIReferenceZoneDataSource = bll.GetZoneDetails().ToDataTable<CI_REFERENCE_ZONE>().DefaultView;

            OldActiveEntity = oldActiveEntity;

            if (!OldActiveEntity.IsNotNullOrEmpty())
                OldActiveEntity = new DDCI_INFO();
            EntityPrimaryKey = oldActiveEntity.IDPK;

            #region DropdownColumns Settins
            CiReferenceZoneDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CODE", ColumnDesc = "Zone Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "DESCRIPTION", ColumnDesc = "Zone", ColumnWidth = "75*" }
                        };

            OldCIReferenceDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CI_REFERENCE", ColumnDesc = "CI Reference", ColumnWidth = "90" },
                            new DropdownColumns() { ColumnName = "FRCS_DATE", ColumnDesc = "FRCS Date", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO", ColumnDesc = "Drawing No.", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "FINISH_CODE", ColumnDesc = "Finish Code", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO_ISSUE", ColumnDesc = "Customer Drawing Issue No.", ColumnWidth = "175" },
                            new DropdownColumns() { ColumnName = "CUST_STD_DATE", ColumnDesc = "Customer STD Date ", ColumnWidth = "150" }
                        };

            #endregion

            this.ciReferenceEndEditCommand = new DelegateCommand(this.ciReferenceEndEdit);
            this.oldReferenceSelectedItemChangedCommand = new DelegateCommand(this.OldCIReferenceChanged);

            this.enquiryReceivedOnChangedCommand = new DelegateCommand(this.EnquiryReceivedOnChanged);
            this.saveCommand = new DelegateCommand(this.SaveSubmitCommand);
            this.ciReferenceZoneSelectedItemChangedCommand = new DelegateCommand(this.CIReferenceZoneChanged);

            ActionPermission = bll.GetUserRights("CIReferenceCopy");
            ActionPermission.AddNew = true;
            ActionPermission.Edit = true;
            ActionPermission.Save = true;
            ActionPermission.Close = true;
            ActionPermission.Print = true;

            ActionMode = operationMode;
        }

        public CopyCIReferenceViewModel(UserInformation userInformation, DDCI_INFO oldActiveEntity,
    OperationMode operationMode)
        {
            bll = new FeasibleReportAndCostSheet(userInformation);

            EnquiryReceivedOn = bll.ServerDateTime();

            OldCIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
            CIReferenceZoneDataSource = bll.GetZoneDetails().ToDataTable<CI_REFERENCE_ZONE>().DefaultView;

            OldActiveEntity = oldActiveEntity;

            if (!OldActiveEntity.IsNotNullOrEmpty())
                OldActiveEntity = new DDCI_INFO();
            EntityPrimaryKey = oldActiveEntity.IDPK;

            #region DropdownColumns Settins
            CiReferenceZoneDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CODE", ColumnDesc = "Zone Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "DESCRIPTION", ColumnDesc = "Zone", ColumnWidth = "75*" }
                        };

            OldCIReferenceDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CI_REFERENCE", ColumnDesc = "CI Reference", ColumnWidth = "90" },
                            new DropdownColumns() { ColumnName = "FRCS_DATE", ColumnDesc = "FRCS Date", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO", ColumnDesc = "Drawing No.", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "FINISH_CODE", ColumnDesc = "Finish Code", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO_ISSUE", ColumnDesc = "Customer Drawing Issue No.", ColumnWidth = "175" },
                            new DropdownColumns() { ColumnName = "CUST_STD_DATE", ColumnDesc = "Customer STD Date ", ColumnWidth = "150" }
                        };

            #endregion

            this.ciReferenceEndEditCommand = new DelegateCommand(this.ciReferenceEndEdit);
            this.oldReferenceSelectedItemChangedCommand = new DelegateCommand(this.OldCIReferenceChanged);

            this.enquiryReceivedOnChangedCommand = new DelegateCommand(this.EnquiryReceivedOnChanged);
            this.saveCommand = new DelegateCommand(this.SaveSubmitCommand);
            this.ciReferenceZoneSelectedItemChangedCommand = new DelegateCommand(this.CIReferenceZoneChanged);

            ActionPermission = bll.GetUserRights("CIReferenceCopy");
            ActionPermission.AddNew = true;
            ActionPermission.Edit = true;
            ActionPermission.Save = true;
            ActionPermission.Close = true;
            ActionPermission.Print = true;

            ActionMode = operationMode;
        }

        public bool Reload = false;

        private DateTime? _enquiry_received_on;
        private DateTime? EnquiryReceivedOn
        {
            get { return _enquiry_received_on; }
            set
            {
                _enquiry_received_on = value;
                NotifyPropertyChanged("EnquiryReceivedOn");
            }
        }

        private void ChangeRights()
        {
            if (!ActionPermission.AddNew) ActionMode = OperationMode.Edit;
            //if (!ActionPermission.Edit) ActionMode = OperationMode.View;
            //if (!ActionPermission.View) ActionMode = OperationMode.Close;
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

        private DataView _oldCIReference = null;
        public DataView OldCIReferenceDataSource
        {
            get
            {
                return _oldCIReference;
            }
            set
            {
                _oldCIReference = value;
                NotifyPropertyChanged("OldCIReferenceDataSource");
            }
        }

        private Visibility _oldCIReferenceHasDropDownVisibility = Visibility.Visible;
        public Visibility OldCIReferenceHasDropDownVisibility
        {
            get { return _oldCIReferenceHasDropDownVisibility; }
            set
            {
                _oldCIReferenceHasDropDownVisibility = value;
                NotifyPropertyChanged("OldCIReferenceHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _oldCIReferenceDropDownItems;
        public ObservableCollection<DropdownColumns> OldCIReferenceDropDownItems
        {
            get
            {
                return _oldCIReferenceDropDownItems;
            }
            set
            {
                _oldCIReferenceDropDownItems = value;
                OnPropertyChanged("OldCIReferenceDropDownItems");
            }
        }

        private DataRowView _oldCIReferenceSelectedRow;
        public DataRowView OldCIReferenceSelectedRow
        {
            get
            {
                return _oldCIReferenceSelectedRow;
            }

            set
            {
                _oldCIReferenceSelectedRow = value;
            }
        }

        private readonly ICommand oldReferenceSelectedItemChangedCommand;
        public ICommand OldCIReferenceSelectedItemChangedCommand { get { return this.oldReferenceSelectedItemChangedCommand; } }
        private void OldCIReferenceChanged()
        {
            if (!_oldCIReferenceSelectedRow.IsNotNullOrEmpty())
            {
                return;
            }

            DataTable dt = bll.GetCIReferenceNumber(new DDCI_INFO() { IDPK = -99999 }).ToDataTable<V_CI_REFERENCE_NUMBER>().Clone();
            dt.ImportRow(_oldCIReferenceSelectedRow.Row);

            List<V_CI_REFERENCE_NUMBER> lstEntity = (from row in dt.AsEnumerable()
                                                     select new V_CI_REFERENCE_NUMBER()
                                                     {
                                                         CI_REFERENCE = row.Field<string>("CI_REFERENCE"),
                                                         FRCS_DATE = row.Field<string>("FRCS_DATE"),
                                                         CUST_DWG_NO = row.Field<string>("CUST_DWG_NO"),
                                                         CUST_CODE = row.Field<string>("CUST_CODE").ToDecimalValue(),
                                                         FINISH_CODE = row.Field<string>("FINISH_CODE"),
                                                         CUST_DWG_NO_ISSUE = row.Field<string>("CUST_DWG_NO_ISSUE"),
                                                         CUST_STD_DATE = row.Field<string>("CUST_STD_DATE"),
                                                         IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                     }).ToList<V_CI_REFERENCE_NUMBER>();
            if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
            {
                V_CI_REFERENCE_NUMBER currentEntity = lstEntity[0];
                List<DDCI_INFO> lstActiveEntity = bll.GetEntitiesByPrimaryKey(new DDCI_INFO() { IDPK = currentEntity.IDPK });

                if (lstActiveEntity.IsNotNullOrEmpty() && lstActiveEntity.Count > 0)
                {
                    OldActiveEntity = lstActiveEntity[0].DeepCopy<DDCI_INFO>();
                    OldActiveEntity.ZONE_CODE = null;

                    CIReferenceZoneDataSource.RowFilter = "CODE in('" + lstActiveEntity[0].ZONE_CODE.ToValueAsString() + "','"
                        + (lstActiveEntity[0].CI_REFERENCE.IsNotNullOrEmpty() ? lstActiveEntity[0].CI_REFERENCE.ToValueAsString().Substring(0, 1) : "") + "')";
                    if (CIReferenceZoneDataSource.Count > 0)
                    {
                        OldActiveEntity.ZONE_CODE = CIReferenceZoneDataSource[0].Row["CODE"].ToValueAsString();
                    }
                    CIReferenceZoneDataSource.RowFilter = null;
                }

            }
        }


        private DataView _newCIReference = null;
        public DataView NewCIReferenceDataSource
        {
            get
            {
                return _newCIReference;
            }
            set
            {
                _newCIReference = value;
                NotifyPropertyChanged("NewCIReferenceDataSource");
            }
        }

        private Visibility _newCIReferenceHasDropDownVisibility = Visibility.Visible;
        public Visibility NewCIReferenceHasDropDownVisibility
        {
            get { return _newCIReferenceHasDropDownVisibility; }
            set
            {
                _newCIReferenceHasDropDownVisibility = value;
                NotifyPropertyChanged("NewCIReferenceHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _newCIReferenceDropDownItems;
        public ObservableCollection<DropdownColumns> NewCIReferenceDropDownItems
        {
            get
            {
                return _newCIReferenceDropDownItems;
            }
            set
            {
                _newCIReferenceDropDownItems = value;
                OnPropertyChanged("NewCIReferenceDropDownItems");
            }
        }

        private DataRowView _newCIReferenceSelectedRow;
        public DataRowView NewCIReferenceSelectedRow
        {
            get
            {
                return _newCIReferenceSelectedRow;
            }

            set
            {
                _newCIReferenceSelectedRow = value;
            }
        }

        private readonly ICommand newCIReferenceSelectedItemChangedCommand;
        public ICommand NewCIReferenceSelectedItemChangedCommand { get { return this.newCIReferenceSelectedItemChangedCommand; } }
        private void NewCIReferenceChanged()
        {
            if (!_newCIReferenceSelectedRow.IsNotNullOrEmpty())
            {
                return;
            }

            DataTable dt = bll.GetCIReferenceNumber(new DDCI_INFO() { IDPK = -99999 }).ToDataTable<V_CI_REFERENCE_NUMBER>().Clone();
            dt.ImportRow(_newCIReferenceSelectedRow.Row);

            List<V_CI_REFERENCE_NUMBER> lstEntity = (from row in dt.AsEnumerable()
                                                     select new V_CI_REFERENCE_NUMBER()
                                                     {
                                                         CI_REFERENCE = row.Field<string>("CI_REFERENCE"),
                                                         FRCS_DATE = row.Field<string>("FRCS_DATE"),
                                                         CUST_DWG_NO = row.Field<string>("CUST_DWG_NO"),
                                                         CUST_CODE = row.Field<string>("CUST_CODE").ToDecimalValue(),
                                                         FINISH_CODE = row.Field<string>("FINISH_CODE"),
                                                         CUST_DWG_NO_ISSUE = row.Field<string>("CUST_DWG_NO_ISSUE"),
                                                         CUST_STD_DATE = row.Field<string>("CUST_STD_DATE"),
                                                         IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                     }).ToList<V_CI_REFERENCE_NUMBER>();
            if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
            {
                V_CI_REFERENCE_NUMBER currentEntity = lstEntity[0];
                List<DDCI_INFO> lstActiveEntity = bll.GetEntitiesByPrimaryKey(new DDCI_INFO() { IDPK = currentEntity.IDPK });

                if (lstActiveEntity.IsNotNullOrEmpty() && lstActiveEntity.Count > 0)
                {
                    NewActiveEntity = lstActiveEntity[0].DeepCopy<DDCI_INFO>();
                    NewActiveEntity.ZONE_CODE = null;

                    CIReferenceZoneDataSource.RowFilter = "CODE in('" + lstActiveEntity[0].ZONE_CODE.ToValueAsString() + "','"
                        + (lstActiveEntity[0].CI_REFERENCE.IsNotNullOrEmpty() ? lstActiveEntity[0].CI_REFERENCE.ToValueAsString().Substring(0, 1) : "") + "')";
                    if (CIReferenceZoneDataSource.Count > 0)
                    {
                        NewActiveEntity.ZONE_CODE = CIReferenceZoneDataSource[0].Row["CODE"].ToValueAsString();
                    }
                    CIReferenceZoneDataSource.RowFilter = null;
                }

            }
        }


        private DataView _ciReferenceZone = null;
        public DataView CIReferenceZoneDataSource
        {
            get
            {
                return _ciReferenceZone;
            }
            set
            {
                _ciReferenceZone = value;
                NotifyPropertyChanged("CIReferenceZoneDataSource");
            }
        }

        private readonly ICommand ciReferenceEndEditCommand;
        public ICommand CIReferenceEndEditCommand { get { return this.ciReferenceEndEditCommand; } }
        private void ciReferenceEndEdit()
        {
            if (!OldActiveEntity.IsNotNullOrEmpty()) return;
            if (!OldActiveEntity.CI_REFERENCE.IsNotNullOrEmpty()) return;
            string message;
            if (!bll.IsValidCIReferenceNumber(OldActiveEntity, ActionMode, out message) && message.IsNotNullOrEmpty() &&
                !(message.IndexOf("already exists") >= 0)) return;
            if (!message.IsNotNullOrEmpty())
            {
                OldActiveEntity.IDPK = -99999;
                string tmpCI = OldActiveEntity.CI_REFERENCE;
                //ClearAll();
                OldActiveEntity.CI_REFERENCE = tmpCI;
                OldCIReferenceSelectedRow = null;
                ShowInformationMessage(PDMsg.DoesNotExists("CI Reference"));
                return;
            }

        }

        private Visibility _ciReferenceZoneHasDropDownVisibility = Visibility.Visible;
        public Visibility CIReferenceZoneHasDropDownVisibility
        {
            get { return Visibility.Visible; }
            set
            {
                _ciReferenceZoneHasDropDownVisibility = value;
                NotifyPropertyChanged("CIReferenceZoneHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _ciReferenceZoneDropDownItems;
        public ObservableCollection<DropdownColumns> CiReferenceZoneDropDownItems
        {
            get
            {
                return _ciReferenceZoneDropDownItems;
            }
            set
            {
                _ciReferenceZoneDropDownItems = value;
                OnPropertyChanged("CiReferenceZoneDropDownItems");
            }
        }

        private DataRowView _ciReferenceZoneSelectedRow;
        public DataRowView CiReferenceZoneSelectedRow
        {
            get
            {
                return _ciReferenceZoneSelectedRow;
            }

            set
            {
                _ciReferenceZoneSelectedRow = value;
            }
        }

        private Visibility _zoneVisibility = Visibility.Visible;
        public Visibility ZoneVisibility
        {
            get
            {
                //return _zoneVisibility;
                return Visibility.Visible;
            }
            set
            {
                _zoneVisibility = value;
                NotifyPropertyChanged("ZoneVisibility");
            }
        }


        private readonly ICommand ciReferenceZoneSelectedItemChangedCommand;
        public ICommand CIReferenceZoneSelectedItemChangedCommand { get { return this.ciReferenceZoneSelectedItemChangedCommand; } }
        private void CIReferenceZoneChanged()
        {
            if (_ciReferenceZoneSelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetZoneDetails(new CI_REFERENCE_ZONE() { IDPK = -99999 }).ToDataTable<CI_REFERENCE_ZONE>().Clone();
                dt.ImportRow(_ciReferenceZoneSelectedRow.Row);

                List<CI_REFERENCE_ZONE> lstEntity = (from row in dt.AsEnumerable()
                                                     select new CI_REFERENCE_ZONE()
                                                     {
                                                         IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                         CODE = row.Field<string>("CODE"),
                                                         DESCRIPTION = row.Field<string>("DESCRIPTION"),
                                                     }).ToList<CI_REFERENCE_ZONE>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    NewActiveEntity.ZONE_CODE = lstEntity[0].CODE;
                    switch (ActionMode)
                    {
                        case OperationMode.AddNew:
                            NewActiveEntity.CI_REFERENCE = bll.CreateCIReferenceNumber(NewActiveEntity);
                            break;
                    }

                }
            }
        }

        private readonly ICommand enquiryReceivedOnChangedCommand;
        public ICommand EnquiryReceivedOnChangedCommand { get { return this.enquiryReceivedOnChangedCommand; } }
        private void EnquiryReceivedOnChanged()
        {
            switch (ActionMode)
            {
                case OperationMode.AddNew:
                    NewActiveEntity.FR_CS_DATE = NewActiveEntity.ENQU_RECD_ON;
                    NewActiveEntity.CI_REFERENCE = bll.CreateCIReferenceNumber(NewActiveEntity);
                    break;
            }
        }

        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {
                _actionMode = value;
                switch (_actionMode)
                {
                    case OperationMode.AddNew:
                        ZoneVisibility = Visibility.Visible;
                        OldCIReferenceHasDropDownVisibility = Visibility.Visible;

                        if (!OldActiveEntity.IsNotNullOrEmpty())
                        {
                            OldActiveEntity = new DDCI_INFO();
                            OldActiveEntity.IDPK = -99999;
                            OldActiveEntity.ENQU_RECD_ON = EnquiryReceivedOn;

                            OldActiveEntity.FR_CS_DATE = null;
                            OldActiveEntity.CUST_STD_DATE = null;
                        }
                        else
                        {
                            OldCIReferenceDataSource.RowFilter = "IDPK='" + OldActiveEntity.IDPK + "'";
                            if (OldCIReferenceDataSource.Count > 0)
                            {
                                OldCIReferenceSelectedRow = OldCIReferenceDataSource[0];
                            }
                            OldCIReferenceDataSource.RowFilter = null;
                        }

                        NewCIReferenceHasDropDownVisibility = Visibility.Collapsed;
                        if (!NewActiveEntity.IsNotNullOrEmpty())
                        {
                            NewActiveEntity = new DDCI_INFO();
                            NewActiveEntity.IDPK = -99999;
                            NewActiveEntity.ENQU_RECD_ON = EnquiryReceivedOn;
                        }
                        NewActiveEntity.FR_CS_DATE = null;
                        NewActiveEntity.CUST_STD_DATE = null;

                        break;
                    case OperationMode.Edit:
                        //ClearAll();
                        //ZoneVisibility = Visibility.Collapsed;
                        //CIReferenceHasDropDownVisibility = Visibility.Visible;

                        //List<DDCI_INFO> lstEntity = bll.GetEntitiesByPrimaryKey(new DDCI_INFO() { IDPK = EntityPrimaryKey });
                        //if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                        //{
                        //    ActiveEntity = lstEntity[0];
                        //}
                        //else
                        //{
                        //    ActiveEntity = new DDCI_INFO();
                        //    ActiveEntity.IDPK = EntityPrimaryKey;
                        //    ActiveEntity.ENQU_RECD_ON = EnquiryReceivedOn.ToDateTimeValue();
                        //    ActiveEntity.FR_CS_DATE = null;
                        //    ActiveEntity.CUST_STD_DATE = null;
                        //}
                        //EntityPrimaryKey = -99999;
                        //if (CIReferenceDataSource.IsNotNullOrEmpty())
                        //{
                        //    CIReferenceDataSource.RowFilter = "IDPK = " + ActiveEntity.IDPK;

                        //    if (CIReferenceDataSource.Count > 0)
                        //    {
                        //        CIReferenceSelectedRow = CIReferenceDataSource[0];
                        //    }
                        //    else
                        //    {
                        //        CIReferenceSelectedRow = null;
                        //    }
                        //    CIReferenceDataSource.RowFilter = null;
                        //}
                        //else
                        //{
                        //    CIReferenceSelectedRow = null;
                        //}
                        //ChangeRights();
                        break;
                    default: ; break;
                }

                NotifyPropertyChanged("ActionMode");
            }
        }

        private DDCI_INFO _oldactiveEntity = null;
        public DDCI_INFO OldActiveEntity
        {
            get
            {
                return _oldactiveEntity;
            }
            set
            {
                _oldactiveEntity = value;
                NotifyPropertyChanged("OldActiveEntity");
            }
        }

        private DDCI_INFO _newactiveEntity = null;
        public DDCI_INFO NewActiveEntity
        {
            get
            {
                return _newactiveEntity;
            }
            set
            {
                _newactiveEntity = value;
                NotifyPropertyChanged("NewActiveEntity");
            }
        }


        private void ClearAll(DDCI_INFO entity)
        {
            try
            {
                if (entity.IsNotNullOrEmpty())
                {
                    entity.CI_REFERENCE = null;
                    //ActiveEntity.ENQU_RECD_ON = EnquiryReceivedOn();
                    entity.FR_CS_DATE = null;
                    entity.PROD_DESC = string.Empty;
                    entity.CUST_CODE = null;
                    entity.CUST_DWG_NO = string.Empty;
                    entity.CUST_DWG_NO_ISSUE = string.Empty;
                    entity.EXPORT = string.Empty;
                    entity.NUMBER_OFF = 0.0m;
                    entity.POTENTIAL = 0.0m;
                    entity.SFL_SHARE = 0.0m;
                    entity.REMARKS = string.Empty;
                    entity.RESPONSIBILITY = null;
                    entity.PENDING = string.Empty;
                    entity.FEASIBILITY = string.Empty;
                    entity.REJECT_REASON = string.Empty;
                    entity.LOC_CODE = null;
                    entity.CHEESE_WT = 0.0m;
                    entity.FINISH_WT = 0.0m;
                    entity.FINISH_CODE = null;
                    entity.COATING_CODE = null;
                    entity.SUGGESTED_RM = null;
                    entity.RM_COST = 0.0m;
                    entity.FINAL_COST = 0.0m;
                    entity.COST_NOTES = string.Empty;
                    entity.PROCESSED_BY = null;
                    entity.ORDER_DT = null;
                    entity.PRINT = string.Empty;
                    entity.ALLOT_PART_NO = 0.0m;
                    entity.PART_NO_REQ_DATE = null;
                    entity.CUST_STD_NO = string.Empty;
                    entity.CUST_STD_DATE = null;
                    entity.AUTOPART = string.Empty;
                    entity.SAFTYPART = string.Empty;
                    entity.APPLICATION = string.Empty;
                    entity.STATUS = 0.0m;
                    entity.CUSTOMER_NEED_DT = null;
                    entity.MKTG_COMMITED_DT = null;
                    entity.PPAP_LEVEL = string.Empty;
                    entity.DEVL_METHOD = 0.0m;
                    entity.PPAP_FORGING = 0.0m;
                    entity.PPAP_SAMPLE = 0.0m;
                    entity.PACKING = 0.0m;
                    entity.NATURE_PACKING = string.Empty;
                    entity.SPL_CHAR = 0.0m;
                    entity.OTHER_CUST_REQ = string.Empty;
                    entity.ATP_DATE = null;
                    entity.SIMILAR_PART_NO = string.Empty;
                    entity.GENERAL_REMARKS = string.Empty;
                    entity.MONTHLY = 0.0m;
                    entity.MKTG_COMMITED_DATE = null;
                }
                //if (CIReferenceDataSource.IsNotNullOrEmpty()) CIReferenceDataSource.RowFilter = null;
                //if (CIReferenceZoneDataSource.IsNotNullOrEmpty()) CIReferenceZoneDataSource.RowFilter = null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private int _entityPrimaryKey = 0;
        private int EntityPrimaryKey
        {
            get { return _entityPrimaryKey; }
            set
            {
                _entityPrimaryKey = value;
                NotifyPropertyChanged("EntityPrimaryKey");
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        #region Close Button Action
        public Action CloseAction { get; set; }
        private RelayCommand _onCancelCommand;
        private void CloseSubmitCommand()
        {
            try
            {
                if (!ActionPermission.Close) return;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    CloseAction();
                }
                ActionMode = OperationMode.AddNew;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public ICommand CloseClickCommand
        {
            get
            {


                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.CloseSubmitCommand(), null);
                }
                return _onCancelCommand;
            }
        }

        #endregion

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }
        private void SaveSubmitCommand()
        {

            if (!OldActiveEntity.IsNotNullOrEmpty()) return;
            if (!NewActiveEntity.IsNotNullOrEmpty()) return;

            if (!OldActiveEntity.CI_REFERENCE.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Old CI Reference Number"));
                return;
            }

            if (!NewActiveEntity.CI_REFERENCE.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("New CI Reference Number"));
                return;
            }

            if (OldActiveEntity.CI_REFERENCE.Trim() == NewActiveEntity.CI_REFERENCE.Trim())
            {
                ShowInformationMessage("CI Reference Numbers should not be same");
                return;
            }

            string message;
            List<DDCI_INFO> lstResult = null;
            if (!bll.IsValidCIReferenceNumber(OldActiveEntity, OperationMode.Edit, out message) &&
                message.IsNotNullOrEmpty() && !(message.IndexOf("already exists") >= 0))
            {
                //ShowInformationMessage(message);
                //return;

                lstResult = (from row in bll.GetEntitiesByCIReferenceNumber(OldActiveEntity)
                             select row).ToList<DDCI_INFO>();

                if (lstResult.IsNotNullOrEmpty() && lstResult.Count == 0)
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Old CI Reference"));
                    return;
                }
            }

            lstResult = (from row in bll.GetEntitiesByCIReferenceNumber(OldActiveEntity)
                         select row).ToList<DDCI_INFO>();

            if (lstResult.IsNotNullOrEmpty() && lstResult.Count == 0)
            {
                ShowInformationMessage(PDMsg.DoesNotExists("Old CI Reference"));
                return;
            }

            lstResult = (from row in bll.GetEntitiesByCIReferenceNumber(NewActiveEntity)
                         select row).ToList<DDCI_INFO>();

            if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
            {
                message = PDMsg.AlreadyExists("CI Reference Number");
                message = message.IndexOf("already exists") >= 0 ?
                            "CI Reference Number " + NewActiveEntity.CI_REFERENCE + " already exists.\r\nEnter/Select another CI Reference Number" :
                            message;
                ShowInformationMessage(message);
                return;
            }

            string outMessage;
            if (!bll.IsValidCIReferenceNumber(NewActiveEntity, ActionMode, out outMessage) && outMessage.IsNotNullOrEmpty())
            {
                ShowInformationMessage(outMessage);
                return;
            }

            if (OldActiveEntity.RM_COST > 9999999999.99m)
            {
                ShowInformationMessage(PDMsg.NotExceeds("Cost for 100 Pcs", "9999999999.99"));
                return;
            }

            if (OldActiveEntity.FINAL_COST > 9999999999.99m)
            {
                ShowInformationMessage(PDMsg.NotExceeds("Cost for " + OldActiveEntity.NO_OF_PCS + " Pcs", "9999999999.99"));
                return;
            }

            if (OldActiveEntity.REALISATION > 99999999.99m)
            {
                ShowInformationMessage(PDMsg.NotExceeds("Realisation", "99999999.99"));
                return;
            }

            try
            {
                //List<DDCI_INFO> lstResult = null;
                List<DDCOST_PROCESS_DATA> lstAssociationEntity = null;

                bool isExecuted = false;
                switch (ActionMode)
                {
                    case OperationMode.AddNew:
                        #region Add Operation

                        foreach (DDCOST_PROCESS_DATA associationEntity in OldActiveEntity.DDCOST_PROCESS_DATA)
                        {
                            OldActiveEntity.DDCOST_PROCESS_DATA.Remove(associationEntity);
                        }

                        string newCIReferenceNumber = NewActiveEntity.CI_REFERENCE;
                        DateTime? newENQU_RECD_ON = NewActiveEntity.ENQU_RECD_ON;
                        string newZONE_CODE = NewActiveEntity.ZONE_CODE;

                        DDCI_INFO activeEntity = OldActiveEntity.DeepCopy<DDCI_INFO>();
                        activeEntity.IDPK = -99999;
                        activeEntity.CI_REFERENCE = newCIReferenceNumber;
                        activeEntity.ENQU_RECD_ON = newENQU_RECD_ON;
                        activeEntity.ZONE_CODE = newZONE_CODE;

                        activeEntity.CUST_DWG_NO = null;
                        activeEntity.CUST_DWG_NO_ISSUE = null;
                        activeEntity.FR_CS_DATE = bll.ServerDateTime();

                        string syear = newCIReferenceNumber.Substring(1, 2).ToIntValue() == 0 ?
                                       "20" + newCIReferenceNumber.Substring(1, 2) : newCIReferenceNumber.Substring(1, 2);

                        string smonth = newCIReferenceNumber.Substring(3, 2);
                        string sday = newCIReferenceNumber.Substring(5, 2);

                        try
                        {
                            DateTime receivedOn = new DateTime(syear.ToIntValue(), smonth.ToIntValue(), sday.ToIntValue());
                            activeEntity.ENQU_RECD_ON = receivedOn;
                        }
                        catch (Exception ex)
                        {
                            throw ex.LogException();
                        }

                        foreach (DDCOST_PROCESS_DATA associationEntity in activeEntity.DDCOST_PROCESS_DATA)
                        {
                            activeEntity.DDCOST_PROCESS_DATA.Remove(associationEntity);
                        }

                        isExecuted = bll.Update<DDCI_INFO>(new List<DDCI_INFO>() { activeEntity });

                        if (isExecuted)
                        {
                            activeEntity = (from row in bll.GetEntitiesByCIReferenceNumber(activeEntity)
                                            select row).SingleOrDefault<DDCI_INFO>();
                            if (activeEntity.IsNotNullOrEmpty())
                            {
                                lstAssociationEntity = (from row in bll.GetCostDetails(OldActiveEntity)
                                                        select new DDCOST_PROCESS_DATA()
                                                        {
                                                            CI_REFERENCE = newCIReferenceNumber,
                                                            SNO = Convert.ToDecimal(row.SNO),
                                                            OPERATION_NO = Convert.ToDecimal(row.OPERATION_NO),
                                                            OPERATION = row.OPERATION,
                                                            COST_CENT_CODE = row.COST_CENT_CODE,
                                                            OUTPUT = row.OUTPUT,
                                                            VAR_COST = row.VAR_COST,
                                                            FIX_COST = row.FIX_COST,
                                                            SPL_COST = row.SPL_COST,
                                                            UNIT_OF_MEASURE = row.UNIT_OF_MEASURE,
                                                            TOTAL_COST = row.TOTAL_COST,
                                                            IDPK = -99999,
                                                            CI_INFO_FK = activeEntity.IDPK,
                                                            ROWID = Guid.NewGuid(),
                                                            PROCESS_CODE = Convert.ToDecimal(row.PROCESS_CODE),
                                                        }).ToList<DDCOST_PROCESS_DATA>();
                                if (lstAssociationEntity.IsNotNullOrEmpty() && lstAssociationEntity.Count == 0)
                                {
                                    ShowInformationMessage("No Process Data to Copy");
                                }
                                else
                                {
                                    isExecuted = bll.Update<DDCOST_PROCESS_DATA>(lstAssociationEntity);
                                }

                                List<DDSHAPE_DETAILS> lstFinishWeightCalculation = null;
                                lstFinishWeightCalculation = (from row in bll.GetShapeDetails(OldActiveEntity)
                                                              select new DDSHAPE_DETAILS()
                                                              {
                                                                  CI_REFERENCE = newCIReferenceNumber,
                                                                  SHAPE_CODE = row.SHAPE_CODE,
                                                                  WEIGHT_OPTION = row.WEIGHT_OPTION,
                                                                  HEAD1 = row.HEAD1,
                                                                  VAL1 = row.VAL1,
                                                                  HEAD2 = row.HEAD2,
                                                                  VAL2 = row.VAL2,
                                                                  HEAD3 = row.HEAD3,
                                                                  VAL3 = row.VAL3,
                                                                  VOLUME = row.VOLUME,
                                                                  SIGN = row.SIGN,
                                                                  SNO = row.SNO,
                                                                  ROWID = row.ROWID,
                                                                  IDPK = -99999,
                                                                  CIREF_NO_FK = activeEntity.IDPK,
                                                              }).ToList<DDSHAPE_DETAILS>();
                                if (lstFinishWeightCalculation.IsNotNullOrEmpty() && lstFinishWeightCalculation.Count == 0)
                                {
                                    ShowInformationMessage("No Weight Calculation Data to Copy");
                                }
                                else
                                {
                                    Progress.ProcessingText = PDMsg.ProgressUpdateText;
                                    Progress.Start();
                                    isExecuted = bll.Update<DDSHAPE_DETAILS>(lstFinishWeightCalculation);
                                    Progress.End();
                                }
                                if (isExecuted)
                                {
                                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                                    NewActiveEntity = activeEntity.DeepCopy<DDCI_INFO>();
                                    NewActiveEntity.CI_REFERENCE = string.Empty;
                                    NewActiveEntity.ENQU_RECD_ON = newENQU_RECD_ON;
                                    NewActiveEntity.ZONE_CODE = newZONE_CODE;
                                }

                            }
                        }
                        #endregion
                        break;
                    case OperationMode.Edit:
                        //#region Update Operation
                        //lstResult = (from row in bll.DB.DDCI_INFO
                        //             where row.CUST_DWG_NO == ActiveEntity.CUST_DWG_NO
                        //             select row).ToList<DDCI_INFO>();

                        //if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 1)
                        //{
                        //    ShowInformationMessage("Customer Drawing Number already exists");
                        //    return;
                        //}

                        //lstStandardNotes = (from row in StandardNotes.ToTable().AsEnumerable()
                        //                    select new DDSTD_NOTES()
                        //                    {
                        //                        SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                        //                        STD_NOTES = row.Field<string>("STD_NOTES"),
                        //                        ROWID = Guid.NewGuid(),
                        //                    }
                        //    ).ToList<DDSTD_NOTES>();

                        //isExecuted = bll.Update<DDSTD_NOTES>(lstStandardNotes);

                        //foreach (DDCOST_PROCESS_DATA associationEntity in ActiveEntity.DDCOST_PROCESS_DATA)
                        //{
                        //    ActiveEntity.DDCOST_PROCESS_DATA.Remove(associationEntity);
                        //}
                        //lstAssociationEntity = (from row in CostDetails.ToTable().AsEnumerable()
                        //                        select new DDCOST_PROCESS_DATA()
                        //                        {
                        //                            CI_REFERENCE = ActiveEntity.CI_REFERENCE,
                        //                            SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                        //                            OPERATION_NO = Convert.ToDecimal(row.Field<string>("OPERATION_NO")),
                        //                            OPERATION = row.Field<string>("OPERATION"),
                        //                            COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                        //                            OUTPUT = Convert.ToDecimal(row.Field<string>("OUTPUT")),
                        //                            VAR_COST = Convert.ToDecimal(row.Field<string>("VAR_COST")),
                        //                            FIX_COST = Convert.ToDecimal(row.Field<string>("FIX_COST")),
                        //                            SPL_COST = Convert.ToDecimal(row.Field<string>("SPL_COST")),
                        //                            UNIT_OF_MEASURE = row.Field<string>("UNIT_OF_MEASURE"),
                        //                            TOTAL_COST = Convert.ToDecimal(row.Field<string>("TOTAL_COST")),
                        //                            IDPK = Convert.ToInt32(row.Field<string>("IDPK")),
                        //                            CI_INFO_FK = ActiveEntity.IDPK,
                        //                            ROWID = row.Field<string>("ROWID").ToGuidValue(),
                        //                        }).ToList<DDCOST_PROCESS_DATA>();

                        //isExecuted = bll.Update<DDCOST_PROCESS_DATA>(lstAssociationEntity);

                        //if (isExecuted)
                        //{

                        //    isExecuted = bll.Update<DDCI_INFO>(new List<DDCI_INFO>() { ActiveEntity });
                        //    if (isExecuted)
                        //    {
                        //        ShowInformationMessage("Records saved successfully");
                        //    }
                        //}
                        //#endregion
                        break;
                    case OperationMode.Delete:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            ActionMode = OperationMode.AddNew;
            //CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
            ChangeRights();
            Reload = true;
            CloseAction();
        }


    }
}
