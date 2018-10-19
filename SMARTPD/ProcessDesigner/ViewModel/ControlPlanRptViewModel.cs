using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using Microsoft.Practices.Prism.Commands;
using System.Data;
using System.Windows.Input;
using ProcessDesigner.Common;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class ControlPlanRptViewModel : ViewModelBase
    {
        private ControlPlanRptBll _cpgbll;
        private LogViewBll _logviewBll;

        private readonly ICommand _onSaveCommand;
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }
        private readonly ICommand _onPrintCommand;
        public ICommand OnPrintCommand { get { return this._onPrintCommand; } }
        //new for export excel
        //private readonly ICommand _onExportCommand;
        //public ICommand OnExportCommand { get { return this._onExportCommand; } }
        //end
        private string seqNos = "";
        private readonly ICommand _onSelectedOperCommand;
        public ICommand OnSelectedOperCommand { get { return this._onSelectedOperCommand; } }

        public Action CloseAction { get; set; }

        private readonly ICommand rbtnProtoTypeCommand;
        public ICommand RbtnProtoTypeClickCommand { get { return this.rbtnProtoTypeCommand; } }
        private readonly ICommand rbtnPreLaunchCommand;
        public ICommand RbtnPreLaunchClickCommand { get { return this.rbtnPreLaunchCommand; } }
        private readonly ICommand rbtnProductionCommand;
        public ICommand RbtnProductionClickCommand { get { return this.rbtnProductionCommand; } }
        private readonly ICommand rbtnAllOperCommand;
        public ICommand RbtnAllOperClickCommand { get { return this.rbtnAllOperCommand; } }
        private readonly ICommand rbtnSelectOperCommand;
        public ICommand RbtnSelectOperClickCommand { get { return this.rbtnSelectOperCommand; } }

        public ControlPlanRptViewModel(UserInformation userInfo, string partNo, string processNo, string seqNo)
        {
            _onSaveCommand = new DelegateCommand(this.SaveCommand);
            _onPrintCommand = new DelegateCommand(this.PrintCommand);
            //new
            //_onExportCommand = new DelegateCommand(this.ExportCommand);
            //end

            _onSelectedOperCommand = new DelegateCommand(this.SelectedOperCommand);
            _controlPlanRptModel = new ControlPlanRptModel();
            _cpgbll = new ControlPlanRptBll(userInfo);
            this._logviewBll = new LogViewBll(userInfo);
            this.selectChangeComboCommandContactPerson = new DelegateCommand(this.SelectDataRowContactPerson);
            this.selectChangeComboCommandFax = new DelegateCommand(this.SelectDataRowFax);
            this.selectChangeComboCommandPhone = new DelegateCommand(this.SelectDataRowPhone);
            this.selectChangeComboCommandCtm1 = new DelegateCommand(this.SelectDataRowCtm1);
            this.selectChangeComboCommandCtm2 = new DelegateCommand(this.SelectDataRowCtm2);
            this.selectChangeComboCommandCtm3 = new DelegateCommand(this.SelectDataRowCtm3);
            this.selectChangeComboCommandCtm4 = new DelegateCommand(this.SelectDataRowCtm4);
            this.selectChangeComboCommandCtm5 = new DelegateCommand(this.SelectDataRowCtm5);
            this.selectChangeComboCommandCtm6 = new DelegateCommand(this.SelectDataRowCtm6);
            this.selectChangeComboCommandCtm7 = new DelegateCommand(this.SelectDataRowCtm7);
            this.rbtnProtoTypeCommand = new DelegateCommand<string>(this.RbtnProtoTypeCommand);
            this.rbtnPreLaunchCommand = new DelegateCommand<string>(this.RbtnPreLaunchCommand);
            this.rbtnProductionCommand = new DelegateCommand<string>(this.RbtnProductionCommand);
            this.rbtnAllOperCommand = new DelegateCommand<string>(this.RbtnAllOperCommand);
            this.rbtnSelectOperCommand = new DelegateCommand<string>(this.RbtnSelectOperCommand);

            SetdropDownItems();
            ControlPlanRptModel.PartNo = partNo;
            ControlPlanRptModel.RouteNo = processNo;
            ControlPlanRptModel.SeqNo = seqNo;
            if (ControlPlanRptModel.PartNo.IsNotNullOrEmpty() && ControlPlanRptModel.RouteNo.IsNotNullOrEmpty())
            {
                _cpgbll.GetCPRptCPM(ControlPlanRptModel);
                _cpgbll.GetCPRptLoadCurrentPartNo(ControlPlanRptModel);
                _cpgbll.GetCPRptLoadSeqNo(ControlPlanRptModel);
                if (ControlPlanRptModel.ControlPlanType == "")
                {
                    ControlPlanRptModel.ControlPlanType = "PRE LAUNCH";
                }
                if (ControlPlanRptModel.ControlPlanType == "PRE LAUNCH")
                {
                    RbtnProtoType = false;
                    RbtnPreLaunch = true;
                    RbtnProduction = false;
                }
                else if (ControlPlanRptModel.ControlPlanType == "PRODUCTION")
                {
                    RbtnProtoType = false;
                    RbtnPreLaunch = false;
                    RbtnProduction = true;
                }
                else if (ControlPlanRptModel.ControlPlanType == "PROTOTYPE")
                {
                    RbtnProtoType = true;
                    RbtnPreLaunch = false;
                    RbtnProduction = false;
                }
                else if (ControlPlanRptModel.ControlPlanType == "")
                {
                    RbtnProtoType = false;
                    RbtnPreLaunch = true;
                    RbtnProduction = false;
                }
                _cpgbll.GetCPRptIssueNoCurrentPartNo(ControlPlanRptModel);
            }
            else
            {
                PDMsg.NotEmpty("Part No");
            }

        }

        private ObservableCollection<string> _chk;
        public ObservableCollection<string> Chk
        {
            get { return _chk; }
            set
            {
                _chk = value;
                NotifyPropertyChanged("Chk");
            }
        }
        private bool _chk1 = false;
        public bool Chk1Checked
        {
            get { return _chk1; }
            set
            {
                _chk1 = value;
                NotifyPropertyChanged("Chk1Checked");
            }
        }

        private bool _chk2 = false;
        public bool Chk2Checked
        {
            get { return _chk2; }
            set
            {
                _chk2 = value;
                NotifyPropertyChanged("Chk2Checked");
            }
        }
        private bool _chk3 = false;
        public bool Chk3Checked
        {
            get { return _chk3; }
            set
            {
                _chk3 = value;
                NotifyPropertyChanged("Chk3Checked");
            }
        }
        private bool _chk4 = false;
        public bool Chk4Checked
        {
            get { return _chk4; }
            set
            {
                _chk4 = value;
                NotifyPropertyChanged("Chk4Checked");
            }
        }
        private bool _chk5 = false;
        public bool Chk5Checked
        {
            get { return _chk5; }
            set
            {
                _chk5 = value;
                NotifyPropertyChanged("Chk5Checked");
            }
        }
        private bool _chk6 = false;
        public bool Chk6Checked
        {
            get { return _chk6; }
            set
            {
                _chk6 = value;
                NotifyPropertyChanged("Chk6Checked");
            }
        }
        private bool _chk7 = false;
        public bool Chk7Checked
        {
            get { return _chk7; }
            set
            {
                _chk7 = value;
                NotifyPropertyChanged("Chk7Checked");
            }
        }
        private bool _chk8 = false;
        public bool Chk8Checked
        {
            get { return _chk8; }
            set
            {
                _chk8 = value;
                NotifyPropertyChanged("Chk8Checked");
            }
        }
        private bool _chk9 = false;
        public bool Chk9Checked
        {
            get { return _chk9; }
            set
            {
                _chk9 = value;
                NotifyPropertyChanged("Chk9Checked");
            }
        }
        private bool _chk10 = false;
        public bool Chk10Checked
        {
            get { return _chk10; }
            set
            {
                _chk10 = value;
                NotifyPropertyChanged("Chk10Checked");
            }
        }
        private bool _chk11 = false;
        public bool Chk11Checked
        {
            get { return _chk11; }
            set
            {
                _chk11 = value;
                NotifyPropertyChanged("Chk11Checked");
            }
        }
        private bool _chk12 = false;
        public bool Chk12Checked
        {
            get { return _chk12; }
            set
            {
                _chk12 = value;
                NotifyPropertyChanged("Chk12Checked");
            }
        }
        private bool _chk13 = false;
        public bool Chk13Checked
        {
            get { return _chk13; }
            set
            {
                _chk13 = value;
                NotifyPropertyChanged("Chk13Checked");
            }
        }
        private bool _chk14 = false;
        public bool Chk14Checked
        {
            get { return _chk14; }
            set
            {
                _chk14 = value;
                NotifyPropertyChanged("Chk14Checked");
            }
        }
        private bool _chk15 = false;
        public bool Chk15Checked
        {
            get { return _chk15; }
            set
            {
                _chk15 = value;
                NotifyPropertyChanged("Chk15Checked");
            }
        }
        private bool _chk16 = false;
        public bool Chk16Checked
        {
            get { return _chk16; }
            set
            {
                _chk16 = value;
                NotifyPropertyChanged("Chk16Checked");
            }
        }
        private bool _chk17 = false;
        public bool Chk17Checked
        {
            get { return _chk17; }
            set
            {
                _chk17 = value;
                NotifyPropertyChanged("Chk17Checked");
            }
        }
        private bool _chk18 = false;
        public bool Chk18Checked
        {
            get { return _chk18; }
            set
            {
                _chk18 = value;
                NotifyPropertyChanged("Chk18Checked");
            }
        }
        private bool _chk19 = false;
        public bool Chk19Checked
        {
            get { return _chk19; }
            set
            {
                _chk19 = value;
                NotifyPropertyChanged("Chk19Checked");
            }
        }
        private bool _chk20 = false;
        public bool Chk20Checked
        {
            get { return _chk20; }
            set
            {
                _chk20 = value;
                NotifyPropertyChanged("Chk20Checked");
            }
        }
        private ObservableCollection<Visibility> _chkVisibility;
        public ObservableCollection<Visibility> ChkVisibility
        {
            get { return _chkVisibility; }
            set
            {
                _chkVisibility = value;
                NotifyPropertyChanged("ChkVisibility");
            }
        }
        private Visibility _visibleSeqNumber = Visibility.Collapsed;
        public Visibility VisibleSeqNumber
        {
            get { return _visibleSeqNumber; }
            set
            {
                _visibleSeqNumber = value;
                NotifyPropertyChanged("VisibleSeqNumber");
            }
        }

        //private Visibility _chk1Visibility = Visibility.Collapsed;
        //public Visibility Chk1Visibility
        //{
        //    get { return _chk1Visibility; }
        //    set
        //    {
        //        _chk1Visibility = value;
        //        NotifyPropertyChanged("Chk1Visibility");
        //    }
        //}

        //private Visibility _chk2Visibility = Visibility.Collapsed;
        //public Visibility Chk2Visibility
        //{
        //    get { return _chk2Visibility; }
        //    set
        //    {
        //        _chk2Visibility = value;
        //        NotifyPropertyChanged("Chk2Visibility");
        //    }
        //}
        //private Visibility _chk3Visibility = Visibility.Collapsed;
        //public Visibility Chk3Visibility
        //{
        //    get { return _chk3Visibility; }
        //    set
        //    {
        //        _chk3Visibility = value;
        //        NotifyPropertyChanged("Chk3Visibility");
        //    }
        //} private Visibility _chk4Visibility = Visibility.Collapsed;
        //public Visibility Chk4Visibility
        //{
        //    get { return _chk4Visibility; }
        //    set
        //    {
        //        _chk4Visibility = value;
        //        NotifyPropertyChanged("Chk4Visibility");
        //    }
        //} private Visibility _chk5Visibility = Visibility.Collapsed;
        //public Visibility Chk5Visibility
        //{
        //    get { return _chk5Visibility; }
        //    set
        //    {
        //        _chk5Visibility = value;
        //        NotifyPropertyChanged("Chk5Visibility");
        //    }
        //} private Visibility _chk6Visibility = Visibility.Collapsed;
        //public Visibility Chk6Visibility
        //{
        //    get { return _chk6Visibility; }
        //    set
        //    {
        //        _chk6Visibility = value;
        //        NotifyPropertyChanged("Chk6Visibility");
        //    }
        //} private Visibility _chk7Visibility = Visibility.Collapsed;
        //public Visibility Chk7Visibility
        //{
        //    get { return _chk7Visibility; }
        //    set
        //    {
        //        _chk7Visibility = value;
        //        NotifyPropertyChanged("Chk7Visibility");
        //    }
        //} private Visibility _chk8Visibility = Visibility.Collapsed;
        //public Visibility Chk8Visibility
        //{
        //    get { return _chk8Visibility; }
        //    set
        //    {
        //        _chk8Visibility = value;
        //        NotifyPropertyChanged("Chk8Visibility");
        //    }
        //} private Visibility _chk9Visibility = Visibility.Collapsed;
        //public Visibility Chk9Visibility
        //{
        //    get { return _chk9Visibility; }
        //    set
        //    {
        //        _chk9Visibility = value;
        //        NotifyPropertyChanged("Chk9Visibility");
        //    }
        //} private Visibility _chk10Visibility = Visibility.Collapsed;
        //public Visibility Chk10Visibility
        //{
        //    get { return _chk10Visibility; }
        //    set
        //    {
        //        _chk10Visibility = value;
        //        NotifyPropertyChanged("Chk10Visibility");
        //    }
        //}

        //private Visibility _chk11Visibility = Visibility.Collapsed;
        //public Visibility Chk11Visibility
        //{
        //    get { return _chk11Visibility; }
        //    set
        //    {
        //        _chk11Visibility = value;
        //        NotifyPropertyChanged("Chk11Visibility");
        //    }
        //}

        //private Visibility _chk12Visibility = Visibility.Collapsed;
        //public Visibility Chk12Visibility
        //{
        //    get { return _chk12Visibility; }
        //    set
        //    {
        //        _chk12Visibility = value;
        //        NotifyPropertyChanged("Chk12Visibility");
        //    }
        //}
        //private Visibility _chk13Visibility = Visibility.Collapsed;
        //public Visibility Chk13Visibility
        //{
        //    get { return _chk13Visibility; }
        //    set
        //    {
        //        _chk13Visibility = value;
        //        NotifyPropertyChanged("Chk13Visibility");
        //    }
        //} private Visibility _chk14Visibility = Visibility.Collapsed;
        //public Visibility Chk14Visibility
        //{
        //    get { return _chk14Visibility; }
        //    set
        //    {
        //        _chk14Visibility = value;
        //        NotifyPropertyChanged("Chk14Visibility");
        //    }
        //} private Visibility _chk15Visibility = Visibility.Collapsed;
        //public Visibility Chk15Visibility
        //{
        //    get { return _chk15Visibility; }
        //    set
        //    {
        //        _chk15Visibility = value;
        //        NotifyPropertyChanged("Chk15Visibility");
        //    }
        //} private Visibility _chk16Visibility = Visibility.Collapsed;
        //public Visibility Chk16Visibility
        //{
        //    get { return _chk16Visibility; }
        //    set
        //    {
        //        _chk16Visibility = value;
        //        NotifyPropertyChanged("Chk16Visibility");
        //    }
        //} private Visibility _chk17Visibility = Visibility.Collapsed;
        //public Visibility Chk17Visibility
        //{
        //    get { return _chk17Visibility; }
        //    set
        //    {
        //        _chk17Visibility = value;
        //        NotifyPropertyChanged("Chk17Visibility");
        //    }
        //} private Visibility _chk18Visibility = Visibility.Collapsed;
        //public Visibility Chk18Visibility
        //{
        //    get { return _chk18Visibility; }
        //    set
        //    {
        //        _chk18Visibility = value;
        //        NotifyPropertyChanged("Chk18Visibility");
        //    }
        //} private Visibility _chk19Visibility = Visibility.Collapsed;
        //public Visibility Chk19Visibility
        //{
        //    get { return _chk19Visibility; }
        //    set
        //    {
        //        _chk19Visibility = value;
        //        NotifyPropertyChanged("Chk19Visibility");
        //    }
        //} private Visibility _chk20Visibility = Visibility.Collapsed;
        //public Visibility Chk20Visibility
        //{
        //    get { return _chk20Visibility; }
        //    set
        //    {
        //        _chk20Visibility = value;
        //        NotifyPropertyChanged("Chk20Visibility");
        //    }
        //}

        private void SelectedOperCommand()
        {
            throw new NotImplementedException();
        }

        private void RbtnProtoTypeCommand(string option)
        {
            ControlPlanRptModel.ControlPlanType = option;
        }
        private void RbtnPreLaunchCommand(string option)
        {
            ControlPlanRptModel.ControlPlanType = option;
        }
        private void RbtnProductionCommand(string option)
        {
            ControlPlanRptModel.ControlPlanType = option;
        }
        private void RbtnAllOperCommand(string option)
        {
            VisibleSeqNumber = Visibility.Collapsed;
        }
        private void RbtnSelectOperCommand(string grpOption)
        {
            try
            {

                Chk = new ObservableCollection<string>();
                ChkVisibility = new ObservableCollection<Visibility>();

                for (int i = 0; i < 20; i++)
                {
                    ChkVisibility.Add(Visibility.Hidden);
                }
                if (ControlPlanRptModel.DtSeqNumber.IsNotNullOrEmpty())
                {
                    int i = 0;
                    foreach (DataRowView seqList in ControlPlanRptModel.DtSeqNumber)
                    {
                        Chk.Add(seqList["seq_no"].ToValueAsString());
                        NotifyPropertyChanged("Chk");
                        ChkVisibility[i] = Visibility.Visible;
                        NotifyPropertyChanged("ChkVisibility");
                        i++;
                    }
                }
                VisibleSeqNumber = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsContactPerson;
        public ObservableCollection<DropdownColumns> DropDownItemsContactPerson
        {
            get
            {
                return _dropDownItemsContactPerson;
            }
            set
            {
                this._dropDownItemsContactPerson = value;
                NotifyPropertyChanged("DropDownItemsContactPerson");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsFax;
        public ObservableCollection<DropdownColumns> DropDownItemsFax
        {
            get
            {
                return _dropDownItemsFax;
            }
            set
            {
                this._dropDownItemsFax = value;
                NotifyPropertyChanged("DropDownItemsFax");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsPhone;
        public ObservableCollection<DropdownColumns> DropDownItemsPhone
        {
            get
            {
                return _dropDownItemsPhone;
            }
            set
            {
                this._dropDownItemsPhone = value;
                NotifyPropertyChanged("DropDownItemsPhone");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsCTM1;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM1
        {
            get
            {
                return _dropDownItemsCTM1;
            }
            set
            {
                this._dropDownItemsCTM1 = value;
                NotifyPropertyChanged("DropDownItemsCTM1");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM2;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM2
        {
            get
            {
                return _dropDownItemsCTM2;
            }
            set
            {
                this._dropDownItemsCTM2 = value;
                NotifyPropertyChanged("DropDownItemsCTM2");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM3;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM3
        {
            get
            {
                return _dropDownItemsCTM3;
            }
            set
            {
                this._dropDownItemsCTM3 = value;
                NotifyPropertyChanged("DropDownItemsCTM3");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM4;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM4
        {
            get
            {
                return _dropDownItemsCTM4;
            }
            set
            {
                this._dropDownItemsCTM4 = value;
                NotifyPropertyChanged("DropDownItemsCTM4");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsCTM5;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM5
        {
            get
            {
                return _dropDownItemsCTM5;
            }
            set
            {
                this._dropDownItemsCTM5 = value;
                NotifyPropertyChanged("DropDownItemsCTM5");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsCTM6;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM6
        {
            get
            {
                return _dropDownItemsCTM6;
            }
            set
            {
                this._dropDownItemsCTM6 = value;
                NotifyPropertyChanged("DropDownItemsCTM6");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM7;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM7
        {
            get
            {
                return _dropDownItemsCTM7;
            }
            set
            {
                this._dropDownItemsCTM7 = value;
                NotifyPropertyChanged("DropDownItemsCTM7");
            }
        }

        private bool _rbtnProtoType = true;
        public bool RbtnProtoType
        {
            get
            {
                return _rbtnProtoType;
            }
            set
            {
                this._rbtnProtoType = value;
                NotifyPropertyChanged("RbtnProtoType");
            }
        }

        private bool _rbtnAllOper = true;
        public bool RbtnAllOper
        {
            get
            {
                return _rbtnAllOper;
            }
            set
            {
                this._rbtnAllOper = value;
                NotifyPropertyChanged("RbtnAllOper");
            }
        }

        private bool _rbtnSelectOper = true;
        public bool RbtnSelectOper
        {
            get
            {
                return _rbtnSelectOper;
            }
            set
            {
                this._rbtnSelectOper = value;
                NotifyPropertyChanged("RbtnSelectOper");
            }
        }

        private bool _rbtnPreLaunch = false;
        public bool RbtnPreLaunch
        {
            get
            {
                return _rbtnPreLaunch;
            }
            set
            {
                this._rbtnPreLaunch = value;
                NotifyPropertyChanged("RbtnPreLaunch");
            }
        }
        private bool _rbtnProduction = false;
        public bool RbtnProduction
        {
            get
            {
                return _rbtnProduction;
            }
            set
            {
                this._rbtnProduction = value;
                NotifyPropertyChanged("RbtnProduction");
            }
        }

        private void SetdropDownItems()
        {
            try
            {

                DropDownItemsContactPerson = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "KEY_CONTACT_PERSON", ColumnDesc = "Contact Person", ColumnWidth = "1*" }
                        };
                DropDownItemsFax = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "KEY_CONTACT_FAXNO", ColumnDesc = "Fax No", ColumnWidth = "1*" }
                        };
                DropDownItemsPhone = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "KEY_CONTACT_PHONE", ColumnDesc = "Phone", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM1 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER1", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM2 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER2", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM3 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER3", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM4 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER4", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM5 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER5", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM6 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER6", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM7 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER7", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        // Key Contact Person
        private readonly ICommand selectChangeComboCommandContactPerson;
        public ICommand SelectChangeComboCommandContactPerson { get { return this.selectChangeComboCommandContactPerson; } }
        private void SelectDataRowContactPerson()
        {
            if (this.SelectedRowContactPerson != null)
            {
                ControlPlanRptModel.KeyContactPerson = SelectedRowContactPerson["key_contact_person"].ToString();
            }
            else
            {
                ControlPlanRptModel.KeyContactPerson = "";
            }
        }

        private DataRowView _selectedRowContactPerson;
        public DataRowView SelectedRowContactPerson
        {
            get
            {
                return _selectedRowContactPerson;
            }
            set
            {
                _selectedRowContactPerson = value;
            }
        }

        //End Key Contact Person

        // Key Fax
        private readonly ICommand selectChangeComboCommandFax;
        public ICommand SelectChangeComboCommandFax { get { return this.selectChangeComboCommandFax; } }
        private void SelectDataRowFax()
        {
            if (this.SelectedRowFax != null)
            {
                ControlPlanRptModel.Fax = SelectedRowFax["key_contact_faxno"].ToString();
            }
            else
            {
                ControlPlanRptModel.Fax = "";
            }
        }

        private DataRowView _selectedRowFax;
        public DataRowView SelectedRowFax
        {
            get
            {
                return _selectedRowFax;
            }

            set
            {
                _selectedRowFax = value;
            }
        }

        //End Key Fax

        // Key Phone number
        private readonly ICommand selectChangeComboCommandPhone;
        public ICommand SelectChangeComboCommandPhone { get { return this.selectChangeComboCommandPhone; } }
        private void SelectDataRowPhone()
        {
            if (this.SelectedRowPhone != null)
            {
                ControlPlanRptModel.Phone = SelectedRowPhone["key_contact_phone"].ToString();
            }
            else
            {
                ControlPlanRptModel.Phone = "";
            }
        }

        private DataRowView _selectedRowPhone;
        public DataRowView SelectedRowPhone
        {
            get
            {
                return _selectedRowPhone;
            }
            set
            {
                _selectedRowPhone = value;
            }
        }

        //End Key Phone number

        // Key Ctm1
        private readonly ICommand selectChangeComboCommandCtm1;
        public ICommand SelectChangeComboCommandCtm1 { get { return this.selectChangeComboCommandCtm1; } }
        private void SelectDataRowCtm1()
        {
            if (this.SelectedRowCtm1 != null)
            {
                ControlPlanRptModel.Ctm1 = SelectedRowCtm1["core_team_member1"].ToString();
            }
            else
            {
                ControlPlanRptModel.Ctm1 = "";
            }
        }

        private DataRowView _selectedRowCtm1;
        public DataRowView SelectedRowCtm1
        {
            get
            {
                return _selectedRowCtm1;
            }
            set
            {
                _selectedRowCtm1 = value;
            }
        }

        //End Key Ctm1

        // Key Ctm2
        private readonly ICommand selectChangeComboCommandCtm2;
        public ICommand SelectChangeComboCommandCtm2 { get { return this.selectChangeComboCommandCtm2; } }
        private void SelectDataRowCtm2()
        {
            if (this.SelectedRowCtm2 != null)
            {
                ControlPlanRptModel.Ctm2 = SelectedRowCtm2["core_team_member2"].ToString();
            }
            else
            {
                ControlPlanRptModel.Ctm2 = "";
            }
        }

        private DataRowView _selectedRowCtm2;
        public DataRowView SelectedRowCtm2
        {
            get
            {
                return _selectedRowCtm2;
            }
            set
            {
                _selectedRowCtm2 = value;
            }
        }

        //End Key Ctm2

        // Key Ctm3
        private readonly ICommand selectChangeComboCommandCtm3;
        public ICommand SelectChangeComboCommandCtm3 { get { return this.selectChangeComboCommandCtm3; } }
        private void SelectDataRowCtm3()
        {
            if (this.SelectedRowCtm3 != null)
            {
                ControlPlanRptModel.Ctm3 = SelectedRowCtm3["core_team_member3"].ToString();
            }
            else
            {
                ControlPlanRptModel.Ctm3 = "";
            }
        }

        private DataRowView _selectedRowCtm3;
        public DataRowView SelectedRowCtm3
        {
            get
            {
                return _selectedRowCtm3;
            }
            set
            {
                _selectedRowCtm3 = value;
            }
        }

        //End Key Ctm3

        // Key Ctm4
        private readonly ICommand selectChangeComboCommandCtm4;
        public ICommand SelectChangeComboCommandCtm4 { get { return this.selectChangeComboCommandCtm4; } }
        private void SelectDataRowCtm4()
        {
            if (this.SelectedRowCtm4 != null)
            {
                ControlPlanRptModel.Ctm4 = SelectedRowCtm4["core_team_member4"].ToString();
            }
            else
            {
                ControlPlanRptModel.Ctm4 = "";
            }
        }

        private DataRowView _selectedRowCtm4;
        public DataRowView SelectedRowCtm4
        {
            get
            {
                return _selectedRowCtm4;
            }
            set
            {
                _selectedRowCtm4 = value;
            }
        }

        //End Key Ctm4

        // Key Ctm5
        private readonly ICommand selectChangeComboCommandCtm5;
        public ICommand SelectChangeComboCommandCtm5 { get { return this.selectChangeComboCommandCtm5; } }
        private void SelectDataRowCtm5()
        {
            if (this.SelectedRowCtm5 != null)
            {
                ControlPlanRptModel.Ctm5 = SelectedRowCtm5["core_team_member5"].ToString();
            }
            else
            {
                ControlPlanRptModel.Ctm5 = "";
            }
        }

        private DataRowView _selectedRowCtm5;
        public DataRowView SelectedRowCtm5
        {
            get
            {
                return _selectedRowCtm5;
            }
            set
            {
                _selectedRowCtm5 = value;
            }
        }

        //End Key Ctm5

        // Key Ctm6
        private readonly ICommand selectChangeComboCommandCtm6;
        public ICommand SelectChangeComboCommandCtm6 { get { return this.selectChangeComboCommandCtm6; } }
        private void SelectDataRowCtm6()
        {
            if (this.SelectedRowCtm6 != null)
            {
                ControlPlanRptModel.Ctm6 = SelectedRowCtm6["core_team_member6"].ToString();
            }
            else
            {
                ControlPlanRptModel.Ctm6 = "";
            }
        }

        private DataRowView _selectedRowCtm6;
        public DataRowView SelectedRowCtm6
        {
            get
            {
                return _selectedRowCtm6;
            }
            set
            {
                _selectedRowCtm6 = value;
            }
        }

        //End Key Ctm6

        // Key Ctm7
        private readonly ICommand selectChangeComboCommandCtm7;
        public ICommand SelectChangeComboCommandCtm7 { get { return this.selectChangeComboCommandCtm7; } }
        private void SelectDataRowCtm7()
        {
            if (this.SelectedRowCtm7 != null)
            {
                ControlPlanRptModel.Ctm7 = SelectedRowCtm7["core_team_member7"].ToString();
            }
            else
            {
                ControlPlanRptModel.Ctm7 = "";
            }
        }

        private DataRowView _selectedRowCtm7;
        public DataRowView SelectedRowCtm7
        {
            get
            {
                return _selectedRowCtm7;
            }
            set
            {
                _selectedRowCtm7 = value;
            }
        }

        //End Key Ctm7

        private ControlPlanRptModel _controlPlanRptModel;
        public ControlPlanRptModel ControlPlanRptModel
        {
            get { return _controlPlanRptModel; }
            set
            {
                _controlPlanRptModel = value;
                NotifyPropertyChanged("ControlPlanRptModel");
            }
        }

        private void SaveCommand()
        {
            string typ = "";
            Progress.ProcessingText = PDMsg.ProgressUpdateText;
            Progress.Start();
            _cpgbll.SaveCPRptControlPlan(ControlPlanRptModel, ref typ);
            Progress.End();
            if (typ == "UPD")
            {
                ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                _logviewBll.SaveLog(ControlPlanRptModel.PartNo, "Control_plan members");
            }
            else
            {
                ShowInformationMessage(PDMsg.SavedSuccessfully);
                _logviewBll.SaveLog(ControlPlanRptModel.PartNo, "Control_plan members");
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
        private DataView _controlPlanRpt = null;
        public DataView ControlPlanRpt
        {
            get { return _controlPlanRpt; }
            set
            {
                _controlPlanRpt = value;
                NotifyPropertyChanged("ControlPlanRpt");
            }
        }
        private void PrintCommand()
        {
            if (VisibleSeqNumber == Visibility.Visible)
            {
                seqNos = ",";
                if (Chk1Checked == true) seqNos = seqNos + Chk[0].ToString();
                if (Chk2Checked == true) seqNos = seqNos + "," + Chk[1].ToString();
                if (Chk3Checked == true) seqNos = seqNos + "," + Chk[2].ToString();
                if (Chk4Checked == true) seqNos = seqNos + "," + Chk[3].ToString();
                if (Chk5Checked == true) seqNos = seqNos + "," + Chk[4].ToString();
                if (Chk6Checked == true) seqNos = seqNos + "," + Chk[5].ToString();
                if (Chk7Checked == true) seqNos = seqNos + "," + Chk[6].ToString();
                if (Chk8Checked == true) seqNos = seqNos + "," + Chk[7].ToString();
                if (Chk9Checked == true) seqNos = seqNos + "," + Chk[8].ToString();
                if (Chk10Checked == true) seqNos = seqNos + "," + Chk[9].ToString();
                if (Chk11Checked == true) seqNos = seqNos + "," + Chk[10].ToString();
                if (Chk12Checked == true) seqNos = seqNos + "," + Chk[11].ToString();
                if (Chk13Checked == true) seqNos = seqNos + "," + Chk[12].ToString();
                if (Chk14Checked == true) seqNos = seqNos + "," + Chk[13].ToString();
                if (Chk15Checked == true) seqNos = seqNos + "," + Chk[14].ToString();
                if (Chk16Checked == true) seqNos = seqNos + "," + Chk[15].ToString();
                if (Chk17Checked == true) seqNos = seqNos + "," + Chk[16].ToString();
                if (Chk18Checked == true) seqNos = seqNos + "," + Chk[17].ToString();
                if (Chk19Checked == true) seqNos = seqNos + "," + Chk[18].ToString();
                if (Chk20Checked == true) seqNos = seqNos + "," + Chk[19].ToString();
                seqNos = seqNos.ToString().TrimStart(',');
                seqNos = seqNos.Replace(",", "','");
            }
            else
            {
                seqNos = "";
            }
            Progress.Start();
            ControlPlanRpt = _cpgbll.PrintCPRpt(ControlPlanRptModel, seqNos);
            if (ControlPlanRpt != null && ControlPlanRpt.Count > 0)
            {
                frmReportViewer showControlPlanRpt = new frmReportViewer(ControlPlanRpt.ToTable(), "ControlPlanPrint");
                Progress.End();
                if (!showControlPlanRpt.ReadyToShowReport) return;
                showControlPlanRpt.ShowDialog();
            }
            else
            {
                Progress.End();
                ShowInformationMessage(PDMsg.NoRecordsPrint);
            }
            Progress.End();
            seqNos = "";
        }

        //new for export excel
        //private void ExportCommand()
        //{
        //    Progress.Start();
        //    ControlPlanRpt = _cpgbll.PrintCPRpt(ControlPlanRptModel, seqNos);
        //    if (ControlPlanRpt != null && ControlPlanRpt.Count > 0)
        //    {
        //        frmReportViewer showControlPlanRpt = new frmReportViewer(ControlPlanRpt.ToTable());
        //        Progress.End();
        //    }
        //}
    }
   
}
