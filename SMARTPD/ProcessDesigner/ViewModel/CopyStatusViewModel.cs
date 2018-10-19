using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using ProcessDesigner.Model;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProcessDesigner.ViewModel
{
    public class CopyStatusViewModel : ViewModelBase
    {
        private UserInformation userinformation;
        private readonly ICommand copyClickCommand;
        private BLL.CopyStatusBLL cpyBll;
        private CopyProcess cpyProcess;
        private ICommand _onCloseCommand;
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
        public CopyStatusViewModel(string _varProcess, string _oldPartNo, string _oldRouteNo, string _oldSeqNo = "", string _oldCC = "", string _oldSH = "")
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;

            OldPartNo = _oldPartNo;
            OldRouteNo = _oldRouteNo;
            oldSeqNo = _oldSeqNo;
            oldShNo = _oldSH;
            OldCCSno = _oldCC;

            // NewPartNo = _newPartNo;
            VarProcess = _varProcess;
            this.copyClickCommand = new DelegateCommand(this.CopyPartNo);
            this._onCloseCommand = new DelegateCommand(this.Close);
            cpyBll = new CopyStatusBLL(userinformation);
            cpyProcess = new CopyProcess(userinformation);
            LoadData();
            this.onCheckBoxClicked = new DelegateCommand(this.EnableDisableTextBoxs);
            this.selectChangeComboCommandNewPartNo = new DelegateCommand(this.SelectDataRowNewPart);
            this.selectChangeComboCommandNewRoute = new DelegateCommand(this.SelectDataRowNewRoute);
            this.selectChangeComboCommandNewSequence = new DelegateCommand(this.SelectDataRowNewSequence);
            //if (IschkControlPlan == true && IschkDrawings == false && ischkPrdMast == false && IschkProcessSheet == false && IschkToolSchedule == false)
            //{
            //    IsNewPartNoReadonly = true;
            //    ButtonVisibleNewPartNumber = Visibility.Visible;
            //    IsNewRouteNoReadonly = true;
            //    ButtonVisibleNewRouteNumber = Visibility.Visible;
            //    IsNewSeqNoReadonly = true;
            //    ButtonVisibleNewSeqNumber = Visibility.Visible;
            //    SetdropDownItems();
            //    NewPartNoDetails = cpyBll.GetPartNoDetails(NewPartNoDetails);
            //}
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsPart;
        public ObservableCollection<DropdownColumns> DropDownItemsPart
        {
            get
            {
                return _dropDownItemsPart;
            }
            set
            {
                this._dropDownItemsPart = value;
                NotifyPropertyChanged("DropDownItemsPart");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsSeq;
        public ObservableCollection<DropdownColumns> DropDownItemsSeq
        {
            get
            {
                return _dropDownItemsSeq;
            }
            set
            {
                this._dropDownItemsSeq = value;
                NotifyPropertyChanged("DropDownItemsSeq");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsRoute;
        public ObservableCollection<DropdownColumns> DropDownItemsRoute
        {
            get
            {
                return _dropDownItemsRoute;
            }
            set
            {
                this._dropDownItemsRoute = value;
                NotifyPropertyChanged("DropDownItemsRoute");
            }
        }
        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "1*" }
                        };
                DropDownItemsSeq = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SEQ_NO", ColumnDesc = "SEQ NO", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "OPN_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "1*" }
                        };
                DropDownItemsRoute = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "ROUTE_NO", ColumnDesc = "PROCESS NO", ColumnWidth = "1*" },
                            };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private DataRowView _selectedrowNewpart;
        public DataRowView SelectedRowNewPart
        {
            get
            {
                return _selectedrowNewpart;
            }

            set
            {
                _selectedrowNewpart = value;
            }
        }
        private readonly ICommand selectChangeComboCommandNewPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandNewPartNo; } }
        private void SelectDataRowNewPart()
        {
            if (this.SelectedRowNewPart != null)
            {
                if (SelectedRowNewPart.IsNotNullOrEmpty())
                {
                    NewPartNo = this.SelectedRowNewPart["PART_NO"].ToString();

                }
                if (NewPartNo.IsNotNullOrEmpty())
                {
                    NewRouteNoDetails = cpyBll.GetRouteNoDetailsByPartNo(NewRouteNoDetails, NewPartNo);
                }
            }
        }
        private DataRowView _selectedrowNewRoute;
        public DataRowView SelectedRowNewRoute
        {
            get
            {
                return _selectedrowNewRoute;
            }

            set
            {
                _selectedrowNewRoute = value;
            }
        }
        private readonly ICommand selectChangeComboCommandNewRoute;
        public ICommand SelectChangeComboCommanddNewRouteNo { get { return this.selectChangeComboCommandNewRoute; } }
        private void SelectDataRowNewRoute()
        {
            if (this.SelectedRowNewRoute != null)
            {
                NewRouteNo = this.SelectedRowNewRoute["ROUTE_NO"].ToString();
                if (NewRouteNo.IsNotNullOrEmpty())
                {
                    NewSeqNoDetails = cpyBll.GetSequenceNoDetailsByPartNoRouteNo(NewSeqNoDetails, newPartNo, NewRouteNo.ToDecimalValue());
                }
            }
        }

        private DataRowView _selectedrowNewSequence;
        public DataRowView SelectedRowNewSequence
        {
            get
            {
                return _selectedrowNewSequence;
            }

            set
            {
                _selectedrowNewSequence = value;
            }
        }
        private readonly ICommand selectChangeComboCommandNewSequence;
        public ICommand SelectChangeComboCommanddNewSequenceNo { get { return this.selectChangeComboCommandNewSequence; } }
        private void SelectDataRowNewSequence()
        {
            if (this.SelectedRowNewSequence != null)
            {
                NewSeqNo = this.SelectedRowNewSequence["SEQ_NO"].ToString();
            }
        }
        //private DataRowView _selectedrowNewSequence;
        //public DataRowView SelectedRowNewSequence
        //{
        //    get
        //    {
        //        return _selectedrowNewSequence;
        //    }

        //    set
        //    {
        //        _selectedrowNewSequence = value;
        //    }
        //}
        //private readonly ICommand selectChangeComboCommandNewRoute;
        //public ICommand SelectChangeComboCommanddNewRouteNo { get { return this.selectChangeComboCommandNewRoute; } }
        //private void SelectDataRowNewRoute()
        //{
        //    if (this.SelectedRowNewRoute != null)
        //    {

        //    }
        //}
        bool isclosed = false;
        private void Close()
        {
            try
            {
                isclosed = false;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    isclosed = true;
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!isclosed)
                {
                    isclosed = true;
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        isclosed = false;
                        e.Cancel = true;
                    }
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

        public Action CloseAction { get; set; }
        public ICommand CopyClickCommand { get { return this.copyClickCommand; } }

        private void CopyPartNo()
        {
            Progress.ProcessingText = PDMsg.Copy;
            Progress.Start();

            cpyBll = new CopyStatusBLL(userinformation);
            cpyProcess = new CopyProcess(userinformation);
            DataValidation();

            Progress.End();
        }

        public void DataValidation()
        {
            try
            {
                DataTable dtRecordCount;
                int estcount = 0;
                int actcount = 0;
                bool dataCopied = false;

                IsNewPartNoReadonly = false;
                ButtonVisibleNewPartNumber = Visibility.Collapsed;

                imgPrdMaster = Visibility.Hidden;
                ImgProcessSheetHeader = Visibility.Hidden;
                ImgProcessSheet = Visibility.Hidden;
                ImgProcessSheetCC = Visibility.Hidden;
                ImgToolScheduleHeader = Visibility.Hidden;
                ImgToolScheduleDetails = Visibility.Hidden;
                imgControlPlan = Visibility.Hidden;
                imgProductDrawing = Visibility.Hidden;
                imgSequenceDrawing = Visibility.Hidden;

                if (OldPartNo.Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Old PartNo"));
                    return;
                }
                else
                {
                    dtRecordCount = cpyBll.GetRecordCount("part_no", "prd_mast", OldPartNo);
                    if (dtRecordCount.Rows.Count == 0)
                    {
                        ShowInformationMessage(PDMsg.EnterValid("Old PartNo"));
                        return;
                    }
                }

                if (NewPartNo.Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("New PartNo"));
                    return;
                }
                else
                {
                    if (IschkPrdMast == false)
                    {
                        dtRecordCount = cpyBll.GetRecordCount("part_no", "prd_mast", NewPartNo);
                        if (dtRecordCount.Rows.Count == 0)
                        {
                            ShowInformationMessage(PDMsg.EnterValid("New PartNo"));
                            return;
                        }
                    }
                }

                //if (newPartNo.Trim() == oldPartNo.Trim())
                //{
                //    ShowInformationMessage("Old Part No. and New Part No. Should not be Same!");
                //    return;
                //}

                if (IschkProcessSheet == true)
                {
                    if (cpyProcess.RecordsCountProcessSheet(OldPartNo, OldRouteNo.ToIntValue(), OldSeqNo.ToDecimalValue(), OldCCSno.ToDecimalValue(), OldShNo.ToDecimalValue()) == 0)
                    {
                        ShowInformationMessage("Process sheet Data not available to Copy.");
                        return;
                    }
                }

                if (IschkToolSchedule == true)
                {
                    if (cpyProcess.RecordsCountToolSched(OldPartNo, OldRouteNo.ToIntValue(), OldSeqNo.ToDecimalValue(), OldCCSno.ToDecimalValue(), OldShNo.ToDecimalValue()) == 0)
                    {
                        IschkToolSchedule = false;
                        ShowInformationMessage("Tool schedule Data not available to Copy.");
                        //return;
                    }
                }

                if (IschkControlPlan == true)
                {
                    if (VarProcess == "ProductMaster" && NewRouteNo.Trim() == "" && OldRouteNo.Trim() == "")
                    {
                        newRouteNo = oldRouteNo = Convert.ToString(cpyProcess.GetCurrentProcessByPartNumber(new PROCESS_MAIN() { PART_NO = oldPartNo.Trim() }).ROUTE_NO);
                        if (NewRouteNo.Trim() == "" && OldRouteNo.Trim() == "" || (NewRouteNo.Trim() == "-999999" && OldRouteNo.Trim() == "-999999"))
                        {
                            ShowInformationMessage(PDMsg.DoesNotExists("Current Process"));
                            return;
                        }
                    }

                    if (NewRouteNo.Trim() == "" && OldRouteNo.Trim() == "")
                    {
                        ShowInformationMessage(PDMsg.NormalString("Enter New Process No and Old Process No"));
                        return;
                    }
                    if (NewRouteNo.Trim() == "" && OldRouteNo.Trim() != "")
                    {
                        ShowInformationMessage(PDMsg.NormalString("Enter New Process No"));
                        return;
                    }
                    if (NewRouteNo.Trim() != "" && OldRouteNo.Trim() == "")
                    {
                        ShowInformationMessage(PDMsg.NormalString("Enter Old Process No"));
                        return;
                    }
                }
                else
                {
                    if (NewRouteNo.Trim() == "" && OldRouteNo.Trim() != "")
                    {
                        ShowInformationMessage(PDMsg.NormalString("Enter New Process No or remove Old Process No"));
                        return;
                    }
                }
                if (NewSeqNo.Trim() == "" && OldSeqNo.Trim() != "")
                {
                    ShowInformationMessage(PDMsg.NormalString("Enter New Sequence No or remove Old Sequence No"));
                    return;
                }

                if (NewCCSno.Trim() == "" && OldCCSno.Trim() != "")
                {
                    ShowInformationMessage(PDMsg.NormalString("Enter New Cost Centre no or remove Old Cost Centre No"));
                    return;
                }

                if (NewCCSno.Trim() == "" && OldCCSno.Trim() != "")
                {
                    ShowInformationMessage(PDMsg.NormalString("Enter New Cost Centre no or remove Old Cost Centre No"));
                    return;
                }

                int x = 0;

                if (IschkPrdMast == true)
                {
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_prdMaster(OldPartNo, NewPartNo);
                    if (x == 1)
                    {
                        imgPrdMaster = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }

                }
                if (IschkProcessSheet == true)
                {
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_Process_main(OldPartNo, NewPartNo, OldRouteNo, NewRouteNo);
                    if (x == 1)
                    {
                        ImgProcessSheetHeader = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_Process_sheet(OldPartNo, NewPartNo, OldRouteNo, NewRouteNo, OldSeqNo, NewSeqNo);
                    if (x == 1)
                    {
                        ImgProcessSheet = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProcessCostCenter(OldPartNo, NewPartNo, OldRouteNo.ToIntValue(), NewRouteNo.ToIntValue(), OldSeqNo.ToDecimalValue(), NewSeqNo.ToDecimalValue(), OldCCSno.ToIntValue(), NewCCSno.ToIntValue());
                    if (x == 1)
                    {
                        ImgProcessSheetCC = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }
                }
                if (IschkToolSchedule == true)
                {
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_Tool_Sched_Main(oldPartNo, NewPartNo, OldRouteNo, NewRouteNo, OldSeqNo, NewSeqNo, OldCCSno, NewCCSno, OldShNo, NewShNo);
                    if (x == 1)
                    {
                        ImgToolScheduleHeader = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_Tool_Sched_Sub(oldPartNo, NewPartNo, OldRouteNo, NewRouteNo, OldSeqNo, NewSeqNo, OldCCSno, NewCCSno, OldShNo, NewShNo);
                    if (x == 1)
                    {
                        ImgToolScheduleDetails = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }
                }
                if (IschkControlPlan == true)
                {
                    //  x = cpyProcess.CopyProcessSheet(OldPartNo, NewPartNo, OldRouteNo.ToIntValue(), NewRouteNo.ToIntValue(), OldSeqNo.ToDecimalValue(), NewSeqNo.ToDecimalValue());
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_PCCS(OldPartNo, NewPartNo, OldRouteNo, NewRouteNo, OldSeqNo, NewSeqNo);
                    ButtonVisibleNewPartNumber = Visibility.Collapsed;
                    if (x == 1)
                    {
                        imgControlPlan = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }
                    else if (x == -1)
                    {
                        cpyProcess = new CopyProcess(userinformation);
                        //  ShowInformationMessage("Matching Sequence No is not available in Process Sheet");
                    }
                }

                if (ischkDrawings == true)
                {
                    // x = cpyProcess.CopyProductData_PROD_DRAWING(OldPartNo, NewPartNo);
                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_PRD_DRAWING(OldPartNo, NewPartNo, 0);
                    if (x == 1)
                    {
                        imgProductDrawing = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }

                    estcount = estcount + 1;
                    x = cpyProcess.CopyProductData_PRD_DRAWING(OldPartNo, NewPartNo, 1);
                    if (x == 1)
                    {
                        imgSequenceDrawing = Visibility.Visible;
                        dataCopied = true;
                        actcount = actcount + 1;
                    }
                }

                if (estcount > 0)
                {
                    if (estcount == actcount)
                    {
                        ShowInformationMessage("Data successfully copied!");
                    }
                    else if (actcount > 0)
                    {
                        ShowInformationMessage("Partial data successfully copied!");
                    }
                    else
                    {
                        ShowInformationMessage("Data not copied!");
                    }
                }
                else
                {
                    ShowInformationMessage("Data not copied!");
                }
                //if (dataCopied == true)
                //{
                //    ShowInformationMessage("Data successfully copied!");
                //}

                //         private PCCS _activeEntity = null;
                //public PCCS ActiveEntity
                //{
                //    get
                //    {
                //        return _activeEntity;
                //    }
                //    set
                //    {
                //        _activeEntity = value;
                //        NotifyPropertyChanged("ActiveEntity");
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool _isNewPartNoReadonly = false;
        public bool IsNewPartNoReadonly
        {
            get { return _isNewPartNoReadonly; }
            set
            {
                this._isNewPartNoReadonly = value;
                NotifyPropertyChanged("IsNewPartNoReadonly");
            }
        }

        private Visibility _buttonVisibleNewPartNumber = Visibility.Collapsed;
        public Visibility ButtonVisibleNewPartNumber
        {
            get { return _buttonVisibleNewPartNumber; }
            set
            {
                this._buttonVisibleNewPartNumber = value;
                NotifyPropertyChanged("ButtonVisibleNewPartNumber");

            }
        }

        private bool _isNewRouteNoReadonly = false;
        public bool IsNewRouteNoReadonly
        {
            get { return _isNewRouteNoReadonly; }
            set
            {
                this._isNewRouteNoReadonly = value;
                NotifyPropertyChanged("IsNewRouteNoReadonly");
            }
        }

        private Visibility _buttonVisibleNewRouteNumber = Visibility.Collapsed;
        public Visibility ButtonVisibleNewRouteNumber
        {
            get { return _buttonVisibleNewRouteNumber; }
            set
            {
                this._buttonVisibleNewRouteNumber = value;
                NotifyPropertyChanged("ButtonVisibleNewRouteNumber");

            }
        }

        private bool _isNewSeqNoReadonly = false;
        public bool IsNewSeqNoReadonly
        {
            get { return _isNewSeqNoReadonly; }
            set
            {
                this._isNewSeqNoReadonly = value;
                NotifyPropertyChanged("IsNewSeqNoReadonly");
            }
        }

        private Visibility _buttonVisibleNewSeqNumber = Visibility.Collapsed;
        public Visibility ButtonVisibleNewSeqNumber
        {
            get { return _buttonVisibleNewSeqNumber; }
            set
            {
                this._buttonVisibleNewSeqNumber = value;
                NotifyPropertyChanged("ButtonVisibleNewSeqNumber");

            }
        }

        private Visibility _imgPrdMaster = Visibility.Hidden;
        public Visibility imgPrdMaster
        {
            get { return _imgPrdMaster; }
            set
            {
                _imgPrdMaster = value;
                NotifyPropertyChanged("imgPrdMaster");
            }
        }

        private Visibility _imgControlPlan = Visibility.Hidden;
        public Visibility imgControlPlan
        {
            get { return _imgControlPlan; }
            set
            {
                _imgControlPlan = value;
                NotifyPropertyChanged("imgControlPlan");
            }
        }

        private Visibility _imgToolScheduleDetails = Visibility.Hidden;
        public Visibility ImgToolScheduleDetails
        {
            get { return _imgToolScheduleDetails; }
            set
            {
                _imgToolScheduleDetails = value;
                NotifyPropertyChanged("ImgToolScheduleDetails");
            }
        }

        private Visibility _imgToolScheduleHeader = Visibility.Hidden;
        public Visibility ImgToolScheduleHeader
        {
            get { return _imgToolScheduleHeader; }
            set
            {
                _imgToolScheduleHeader = value;
                NotifyPropertyChanged("ImgToolScheduleHeader");
            }
        }

        private Visibility _imgProcessSheetHeader = Visibility.Hidden;
        public Visibility ImgProcessSheetHeader
        {
            get { return _imgProcessSheetHeader; }
            set
            {
                _imgProcessSheetHeader = value;
                NotifyPropertyChanged("ImgProcessSheetHeader");
            }
        }

        private Visibility _imgProcessSheet = Visibility.Hidden;
        public Visibility ImgProcessSheet
        {
            get { return _imgProcessSheet; }
            set
            {
                _imgProcessSheet = value;
                NotifyPropertyChanged("ImgProcessSheet");
            }
        }

        private Visibility _imgProcessSheetCC = Visibility.Hidden;
        public Visibility ImgProcessSheetCC
        {
            get { return _imgProcessSheetCC; }
            set
            {
                _imgProcessSheetCC = value;
                NotifyPropertyChanged("ImgProcessSheetCC");
            }
        }

        private Visibility _imgProductDrawing = Visibility.Hidden;
        public Visibility imgProductDrawing
        {
            get { return _imgProductDrawing; }
            set
            {
                _imgProductDrawing = value;
                NotifyPropertyChanged("imgProductDrawing");
            }
        }

        private Visibility _imgSequenceDrawing = Visibility.Hidden;
        public Visibility imgSequenceDrawing
        {
            get { return _imgSequenceDrawing; }
            set
            {
                _imgSequenceDrawing = value;
                NotifyPropertyChanged("imgSequenceDrawing");
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }
        public void LoadData()
        {
            IschkPrdMast = false;
            IschkDrawings = false;
            IschkControlPlan = false;
            IschkProcessSheet = false;
            IschkToolSchedule = false;

            En_chkControlPlan = false;
            En_chkDrawings = false;
            En_chkPrdMast = false;
            En_chkProcessSheet = false;
            En_chkToolSchedule = false;
            En_chkPrdMast = false;
            OldPartNo = this.OldPartNo;

            switch (VarProcess)
            {
                case "ProductMaster":
                    En_chkPrdMast = true;
                    En_chkProcessSheet = true;
                    En_chkControlPlan = true;
                    En_chkDrawings = true;
                    En_chkToolSchedule = true;

                    IschkPrdMast = true;
                    IschkControlPlan = true;
                    IschkProcessSheet = true;
                    IschkToolSchedule = true;
                    IschkDrawings = false;
                    IschkPrdMast = true;
                    break;
                case "PCCS":
                    En_chkPrdMast = false;
                    En_chkProcessSheet = false;
                    En_chkToolSchedule = false;
                    En_chkDrawings = false;

                    IschkPrdMast = false;
                    IschkControlPlan = true;
                    IschkProcessSheet = false;
                    IschkToolSchedule = false;
                    ischkDrawings = false;
                    break;
                case "ToolSchedule":
                    // ischkToolSchedule =true;
                    en_chkPrdMast = false;
                    en_chkProcessSheet = false;
                    en_chkControlPlan = false;
                    en_chkDrawings = false;
                    IschkPrdMast = false;
                    IschkControlPlan = false;
                    IschkProcessSheet = false;
                    IschkToolSchedule = true;
                    IschkDrawings = false;
                    break;
                case "ProcessSheet":
                    IschkPrdMast = false;
                    IschkControlPlan = true;
                    IschkProcessSheet = true;
                    IschkToolSchedule = true;
                    IschkDrawings = false;

                    En_chkPrdMast = false;
                    En_chkDrawings = false;
                    En_chkProcessSheet = true;
                    En_chkToolSchedule = true;
                    En_chkControlPlan = true;
                    break;
                case "Drawings":
                    IschkPrdMast = false;
                    IschkControlPlan = false;
                    IschkProcessSheet = false;
                    IschkToolSchedule = false;
                    IschkDrawings = true;
                    En_chkPrdMast = true;
                    En_chkProcessSheet = true;
                    En_chkControlPlan = true;
                    En_chkDrawings = true;
                    En_chkToolSchedule = true;
                    break;
            }
            EnableDisableTextBoxs();
        }

        private readonly ICommand onCheckBoxClicked = null;
        public ICommand OnCheckBoxClicked { get { return this.onCheckBoxClicked; } }
        private void EnableDisableTextBoxs()
        {
            IsReadOnlyProcessNo = true;
            IsReadOnlySeqNo = true;
            IsReadOnlyCCSno = true;
            IsReadOnlySHNo = true;

            if (IschkProcessSheet)
            {
                IsReadOnlyProcessNo = false;
                IsReadOnlySeqNo = false;
                IsReadOnlyCCSno = false;
            }

            if (IschkControlPlan)
            {
                IsReadOnlyProcessNo = false;
                IsReadOnlySeqNo = false;
                //IsReadOnlyCCSno = false;
            }
            if (IschkToolSchedule)
            {
                IsReadOnlyProcessNo = false;
                IsReadOnlySeqNo = false;
                IsReadOnlyCCSno = false;
                IsReadOnlySHNo = false;
            }

            if (IsReadOnlyProcessNo)
            {
                OldRouteNo = "";
                NewRouteNo = "";
            }
            if (IsReadOnlySeqNo)
            {
                OldSeqNo = "";
                NewSeqNo = "";
            }
            if (IsReadOnlyCCSno)
            {
                OldCCSno = "";
                NewCCSno = "";
            }
            if (IsReadOnlySHNo)
            {
                OldShNo = "";
                NewShNo = "";
            }
        }

        private string oldPartNo = string.Empty;
        private string oldCCSno = string.Empty;
        private string oldSeqNo = string.Empty;
        private string oldShNo = string.Empty;
        private string oldRouteNo = string.Empty;

        public string OldPartNo
        {
            get { return oldPartNo; }
            set
            {
                oldPartNo = value;
                NotifyPropertyChanged("OldPartNo");
            }
        }

        public string OldCCSno
        {
            get { return oldCCSno; }
            set
            {
                oldCCSno = value;
                NotifyPropertyChanged("OldCCSno");
            }
        }

        public string OldSeqNo
        {
            get { return oldSeqNo; }
            set
            {
                oldSeqNo = value;
                NotifyPropertyChanged("OldSeqNo");
            }
        }

        public string OldShNo
        {
            get { return oldShNo; }
            set
            {
                oldShNo = value;
                NotifyPropertyChanged("OldShNo");
            }
        }

        public string OldRouteNo
        {
            get { return oldRouteNo; }
            set
            {
                oldRouteNo = value;
                NotifyPropertyChanged("OldRouteNo");
            }
        }

        private DataView _dtNewPartNoDetails;
        private DataView _dtNewRouteNoDetails;
        private DataView _dtNewSeqNoDetails;
        public DataView NewPartNoDetails
        {
            get { return _dtNewPartNoDetails; }
            set
            {
                _dtNewPartNoDetails = value;
                NotifyPropertyChanged("NewPartNoDetails");
            }
        }

        public DataView NewRouteNoDetails
        {
            get { return _dtNewRouteNoDetails; }
            set
            {
                _dtNewRouteNoDetails = value;
                NotifyPropertyChanged("NewRouteNoDetails");
            }
        }

        public DataView NewSeqNoDetails
        {
            get { return _dtNewSeqNoDetails; }
            set
            {
                _dtNewSeqNoDetails = value;
                NotifyPropertyChanged("NewSeqNoDetails");
            }
        }

        private string newPartNo = string.Empty;
        private string newCCSno = string.Empty;
        private string newSeqNo = string.Empty;
        private string newShNo = string.Empty;
        private string newRouteNo = string.Empty;

        public string NewPartNo
        {
            get { return newPartNo; }
            set
            {
                newPartNo = value;
                NotifyPropertyChanged("NewPartNo");
            }
        }

        public string NewCCSno
        {
            get { return newCCSno; }
            set
            {
                newCCSno = value;
                NotifyPropertyChanged("NewCCSno");
            }
        }

        public string NewSeqNo
        {
            get { return newSeqNo; }
            set
            {
                newSeqNo = value;
                NotifyPropertyChanged("NewSeqNo");
            }
        }

        public string NewShNo
        {
            get { return newShNo; }
            set
            {
                newShNo = value;
                NotifyPropertyChanged("NewShNo");
            }
        }

        public string NewRouteNo
        {
            get { return newRouteNo; }
            set
            {
                newRouteNo = value;
                NotifyPropertyChanged("NewRouteNo");
            }
        }


        private bool en_chkPrdMast = false;
        private bool en_chkDrawings = false;
        private bool en_chkControlPlan = false;
        private bool en_chkProcessSheet = false;
        private bool en_chkToolSchedule = false;

        public bool En_chkPrdMast
        {
            get { return en_chkPrdMast; }
            set
            {
                en_chkPrdMast = value;
                NotifyPropertyChanged("En_chkPrdMast");
            }
        }

        public bool En_chkDrawings
        {
            get { return en_chkDrawings; }
            set
            {
                en_chkDrawings = value;
                NotifyPropertyChanged("En_chkDrawings");
            }
        }

        public bool En_chkControlPlan
        {
            get { return en_chkControlPlan; }
            set
            {
                en_chkControlPlan = value;
                NotifyPropertyChanged("En_chkControlPlan");
            }
        }


        public bool En_chkProcessSheet
        {
            get { return en_chkProcessSheet; }
            set
            {
                en_chkProcessSheet = value;
                NotifyPropertyChanged("En_chkProcessSheet");
            }
        }

        public bool En_chkToolSchedule
        {
            get { return en_chkToolSchedule; }
            set
            {
                en_chkToolSchedule = value;
                NotifyPropertyChanged("En_chkToolSchedule");
            }
        }






        private bool isReadOnlyProcessNo = true;
        public bool IsReadOnlyProcessNo
        {
            get { return isReadOnlyProcessNo; }
            set
            {
                isReadOnlyProcessNo = value;
                NotifyPropertyChanged("IsReadOnlyProcessNo");
            }
        }

        private bool isReadOnlyCCSno = true;
        public bool IsReadOnlyCCSno
        {
            get { return isReadOnlyCCSno; }
            set
            {
                isReadOnlyCCSno = value;
                NotifyPropertyChanged("IsReadOnlyCCSno");
            }
        }

        private bool isReadOnlySeqNo = false;
        public bool IsReadOnlySeqNo
        {
            get { return isReadOnlySeqNo; }
            set
            {
                isReadOnlySeqNo = value;
                NotifyPropertyChanged("IsReadOnlySeqNo");
            }
        }


        private bool isReadOnlySHNo = false;
        public bool IsReadOnlySHNo
        {
            get { return isReadOnlySHNo; }
            set
            {
                isReadOnlySHNo = value;
                NotifyPropertyChanged("IsReadOnlySHNo");
            }
        }

        private bool ischkPrdMast = false;
        private bool ischkDrawings = false;
        private bool ischkControlPlan = false;
        private bool ischkProcessSheet = false;
        private bool ischkToolSchedule = false;

        public bool IschkPrdMast
        {
            get { return ischkPrdMast; }
            set
            {
                ischkPrdMast = value;
                NotifyPropertyChanged("IschkPrdMast");
            }
        }

        public bool IschkDrawings
        {
            get { return ischkDrawings; }
            set
            {
                ischkDrawings = value;
                NotifyPropertyChanged("IschkDrawings");
            }
        }

        public bool IschkControlPlan
        {
            get { return ischkControlPlan; }
            set
            {
                ischkControlPlan = value;
                NotifyPropertyChanged("IschkControlPlan");
            }
        }

        public bool IschkProcessSheet
        {
            get { return ischkProcessSheet; }
            set
            {
                ischkProcessSheet = value;
                NotifyPropertyChanged("IschkProcessSheet");
            }
        }

        public bool IschkToolSchedule
        {
            get { return ischkToolSchedule; }
            set
            {
                ischkToolSchedule = value;
                NotifyPropertyChanged("IschkToolSchedule");
            }
        }

        private string varProcess = string.Empty;
        public string VarProcess
        {
            get { return varProcess; }
            set
            {
                varProcess = value;
                NotifyPropertyChanged("VarProcess");
            }
        }


    }

}
