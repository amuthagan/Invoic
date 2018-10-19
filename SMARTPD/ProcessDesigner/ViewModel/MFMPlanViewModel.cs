using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
    public class MFMPlanViewModel : ViewModelBase
    {
        private UserInformation userInformation;
        private MFMPlanBll bll;
        public Action CloseAction { get; set; }

        public MFMPlanViewModel(UserInformation userinfo)
        {
            userInformation = userinfo;
            mfmplan = new MFMPlanModel();
            bll = new MFMPlanBll(userinfo);
            this._windowLoadedCommand = new DelegateCommand(this.WindowLoaded);
            this._onPartNoSelectionChanged = new DelegateCommand(this.PartNoSelectionChanged);
            this._saveCommand = new DelegateCommand(this.Save);
            this._closeCommand = new DelegateCommand(this.Close);

            DropdownHeaders = new ObservableCollection<DropdownColumns>
            {               
                new DropdownColumns { ColumnName = "PART_NO", ColumnDesc = "Part Number", ColumnWidth = "1*" },
                new DropdownColumns { ColumnName = "PART_DESC", ColumnDesc = "Part Description", ColumnWidth = "1*" }
            };

            DropDownItemsUsers = new ObservableCollection<DropdownColumns>
            {               
                new DropdownColumns { ColumnName = "LOGIN", ColumnDesc = "Login ID", ColumnWidth = "1*" },
                new DropdownColumns { ColumnName = "USER_FULL_NAME", ColumnDesc = "User Full Name", ColumnWidth = "1*" }
            };
        }

        private DataRowView partNoSelectedItem = null;
        public DataRowView PartNoSelectedItem
        {
            get { return this.partNoSelectedItem; }
            set
            {
                this.partNoSelectedItem = value;
                NotifyPropertyChanged("PartNoSelectedItem");
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownHeaders = null;
        public ObservableCollection<DropdownColumns> DropdownHeaders
        {
            get { return this._dropdownHeaders; }
            set
            {
                this._dropdownHeaders = value;
                NotifyPropertyChanged("DropdownHeaders");
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownItemsUsers = null;
        public ObservableCollection<DropdownColumns> DropDownItemsUsers
        {
            get { return this._dropdownItemsUsers; }
            set
            {
                this._dropdownItemsUsers = value;
                NotifyPropertyChanged("DropDownItemsUsers");
            }
        }

        private bool _isEnabledPlanGrid = false;
        public bool IsEnabledPlanGrid
        {
            get { return _isEnabledPlanGrid; }
            set
            {
                _isEnabledPlanGrid = value;
                NotifyPropertyChanged("IsEnabledPlanGrid");
            }
        }

        private Visibility _saveVisibility = Visibility.Hidden;
        public Visibility SaveVisibility
        {
            get { return _saveVisibility; }
            set
            {
                _saveVisibility = value;
                NotifyPropertyChanged("SaveVisibility");
            }
        }

        private readonly ICommand _windowLoadedCommand;
        public ICommand WindowLoadedCommand { get { return this._windowLoadedCommand; } }
        private void WindowLoaded()
        {
            try
            {
                bll.GetProductMaster(MFMPlan);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private MFMPlanModel mfmplan = null;
        public MFMPlanModel MFMPlan
        {
            get { return mfmplan; }
            set
            {
                mfmplan = value;
                NotifyPropertyChanged("MFMPlan");
            }
        }

        private readonly ICommand _onPartNoSelectionChanged;
        public ICommand OnPartNoSelectionChanged { get { return this._onPartNoSelectionChanged; } }
        private void PartNoSelectionChanged()
        {
            try
            {
                if (PartNoSelectedItem != null)
                {
                    MFMPlan.PART_DESC = PartNoSelectedItem["PART_DESC"].ToString();
                    bll.RetreieveCustomerName(MFMPlan);
                    bll.RetrieveMFMmast(MFMPlan);
                    if (MFMPlan.MFM_MASTER != null)
                    {
                        IsEnabledPlanGrid = true;
                        SaveVisibility = Visibility.Visible;
                        MFMPlan.DOC_REL_DT_PLAN = MFMPlan.MFM_MASTER.DOC_REL_DT_PLAN;
                        MFMPlan.DOC_REL_DT_ACTUAL = MFMPlan.MFM_MASTER.DOC_REL_DT_ACTUAL;
                        MFMPlan.TIME_BOGAUGE_PLAN = MFMPlan.MFM_MASTER.TIME_BOGAUGE_PLAN;
                        MFMPlan.TIME_BOGAUGE_ACTUAL = MFMPlan.MFM_MASTER.TIME_BOGAUGE_ACTUAL;
                        MFMPlan.TOOLS_READY_DT_PLAN = MFMPlan.MFM_MASTER.TOOLS_READY_DT_PLAN;
                        MFMPlan.TOOLS_READY_ACTUAL_DT = MFMPlan.MFM_MASTER.TOOLS_READY_ACTUAL_DT;
                        MFMPlan.FORGING_PLAN_DT = MFMPlan.MFM_MASTER.FORGING_PLAN_DT;
                        MFMPlan.FORGING_ACTUAL_DT = MFMPlan.MFM_MASTER.FORGING_ACTUAL_DT;
                        MFMPlan.SECONDARY_PLAN_DT = MFMPlan.MFM_MASTER.SECONDARY_PLAN_DT;
                        MFMPlan.SECONDARY_ACTUAL_DT = MFMPlan.MFM_MASTER.SECONDARY_ACTUAL_DT;
                        MFMPlan.HEAT_TREATMENT_PLAN_DT = MFMPlan.MFM_MASTER.HEAT_TREATMENT_PLAN_DT;
                        MFMPlan.HEAT_TREATMENT_ACTUAL = MFMPlan.MFM_MASTER.HEAT_TREATMENT_ACTUAL;
                        MFMPlan.ISSR_PLAN_DT = MFMPlan.MFM_MASTER.ISSR_PLAN_DT;
                        MFMPlan.ISSR_ACTUAL_DT = MFMPlan.MFM_MASTER.ISSR_ACTUAL_DT;
                        MFMPlan.PPAP_PLAN = MFMPlan.MFM_MASTER.PPAP_PLAN;
                        MFMPlan.PPAP_ACTUAL_DT = MFMPlan.MFM_MASTER.PPAP_ACTUAL_DT;
                        MFMPlan.SAMPLE_QTY = MFMPlan.MFM_MASTER.SAMPLE_QTY.ToString();
                        MFMPlan.PSW_DATE = MFMPlan.MFM_MASTER.PSW_DATE;
                        MFMPlan.RESP = MFMPlan.MFM_MASTER.RESP;
                        MFMPlan.USER_FULL_NAME = MFMPlan.MFM_MASTER.RESP;
                        if (MFMPlan.RESP.IsNotNullOrEmpty())
                        {
                            DataView dv = MFMPlan.DVUsers.Table.Copy().DefaultView;
                            dv.RowFilter = "LOGIN = '" + MFMPlan.RESP + "'";
                            if (dv.Count > 0)
                            {
                                MFMPlan.USER_FULL_NAME = dv[0]["USER_FULL_NAME"].ToString();
                            }
                        }
                        MFMPlan.REMARKS = MFMPlan.MFM_MASTER.REMARKS;
                    }
                    else
                    {
                        ShowInformationMessage("This Part No does not exist in the MFM Plan. Update Part No details (Product Master) to add it to the Plan");
                        IsEnabledPlanGrid = false;
                        SaveVisibility = Visibility.Hidden;
                    }
                }
                else
                {
                    MFMPlan.PART_DESC = "";
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return this._saveCommand; } }
        private void Save()
        {
            try
            {
                if (!MFMPlan.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);                  
                    return;
                }
                bll.UpdateMFMPlan(MFMPlan);
                
                ShowInformationMessage(PDMsg.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ClearValue()
        {
            IsEnabledPlanGrid = false;
            SaveVisibility = Visibility.Hidden;
            MFMPlan.DOC_REL_DT_PLAN = null;
            MFMPlan.DOC_REL_DT_ACTUAL = null;
            MFMPlan.TIME_BOGAUGE_PLAN = null;
            MFMPlan.TIME_BOGAUGE_ACTUAL = null;
            MFMPlan.TOOLS_READY_DT_PLAN = null;
            MFMPlan.TOOLS_READY_ACTUAL_DT = null;
            MFMPlan.FORGING_PLAN_DT = null;
            MFMPlan.FORGING_ACTUAL_DT = null;
            MFMPlan.SECONDARY_PLAN_DT = null;
            MFMPlan.SECONDARY_ACTUAL_DT = null;
            MFMPlan.HEAT_TREATMENT_PLAN_DT = null;
            MFMPlan.HEAT_TREATMENT_ACTUAL = null;
            MFMPlan.ISSR_PLAN_DT = null;
            MFMPlan.ISSR_ACTUAL_DT = null;
            MFMPlan.PPAP_PLAN = null;
            MFMPlan.PPAP_ACTUAL_DT = null;
            MFMPlan.SAMPLE_QTY = "";
            MFMPlan.PSW_DATE = null;
            MFMPlan.RESP = "";
            MFMPlan.USER_FULL_NAME = "";
            MFMPlan.REMARKS = "";
        }

        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }
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


    }
}
