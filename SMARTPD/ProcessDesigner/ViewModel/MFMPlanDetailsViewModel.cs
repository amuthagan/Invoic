using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using WPF.MDI;
using System.ComponentModel;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class MFMPlanDetailsViewModel : ViewModelBase
    {
        private UserInformation userInformation;
        private MFMPlanDetailsBll bll;
        public Action CloseAction { get; set; }
        public DataGrid dgrdMFMPlanDetails { get; set; }
        public MFMPlanDetailsViewModel(UserInformation userinfo)
        {
            userInformation = userinfo;
            mfmplandetails = new MFMPlanDetailsModel();
            bll = new MFMPlanDetailsBll(userinfo);
            this._windowLoadedCommand = new DelegateCommand(this.WindowLoaded);
            this.refreshCommand = new DelegateCommand(this.Refresh);
            this.editDataCommand = new DelegateCommand(this.EditData);
            this.printCommand = new DelegateCommand(this.Print);
            this.onCheckBoxClicked = new DelegateCommand<string>(this.CheckBoxClicked);
            this.onGroupBoxDoubleClicked = new DelegateCommand(this.GroupBoxDoubleClicked);
        }

        private readonly ICommand _windowLoadedCommand;
        public ICommand WindowLoadedCommand { get { return this._windowLoadedCommand; } }
        private void WindowLoaded()
        {
            try
            {
                DateTime dt = Convert.ToDateTime(bll.ServerDateTime());
                MFMPlanDetails.Month = dt.AddMonths(1).ToFormattedDateAsString("yyyyMM");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private MFMPlanDetailsModel mfmplandetails = null;
        public MFMPlanDetailsModel MFMPlanDetails
        {
            get { return mfmplandetails; }
            set
            {
                mfmplandetails = value;
                NotifyPropertyChanged("MFMPlanDetails");
            }
        }


        private Visibility _monthVisibility = Visibility.Hidden;
        public Visibility MonthVisibility
        {
            get { return _monthVisibility; }
            set
            {
                _monthVisibility = value;
                NotifyPropertyChanged("MonthVisibility");
            }
        }


        private readonly ICommand onCheckBoxClicked = null;
        public ICommand OnCheckBoxClicked { get { return this.onCheckBoxClicked; } }
        private void CheckBoxClicked(string name)
        {
            try
            {
                switch (name)
                {
                    case "Documentation":

                        if (MFMPlanDetails.IsDocumentation)
                        {
                            MFMPlanDetails.IsForging = false;
                            MFMPlanDetails.IsSecondary = false;
                            MFMPlanDetails.IsPPAP = false;
                            MFMPlanDetails.IsTools = false;

                        }
                        break;
                    case "Forging":

                        if (MFMPlanDetails.IsForging && MFMPlanDetails.GroupHeader == "Awaiting")
                        {
                            MFMPlanDetails.IsDocumentation = false;
                        }
                        break;
                    case "PPAP":

                        if (MFMPlanDetails.IsPPAP && MFMPlanDetails.GroupHeader == "Awaiting")
                        {
                            MFMPlanDetails.IsDocumentation = false;
                        }
                        break;
                    case "Tools":

                        if (MFMPlanDetails.IsTools && MFMPlanDetails.GroupHeader == "Awaiting")
                        {
                            MFMPlanDetails.IsDocumentation = false;
                        }
                        break;
                    case "Secondary":

                        if (MFMPlanDetails.IsSecondary && MFMPlanDetails.GroupHeader == "Awaiting")
                        {
                            MFMPlanDetails.IsDocumentation = false;
                        }
                        break;
                    case "Awaiting PSW":

                        if (MFMPlanDetails.IsAwaitingPSW && MFMPlanDetails.GroupHeader == "Awaiting")
                        {
                            MFMPlanDetails.IsApprovedPSW = false;
                        }
                        break;
                    case "Approved PSW":

                        if (MFMPlanDetails.IsApprovedPSW && MFMPlanDetails.GroupHeader == "Awaiting")
                        {
                            MFMPlanDetails.IsAwaitingPSW = false;
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand onGroupBoxDoubleClicked = null;
        public ICommand OnGroupBoxDoubleClicked { get { return this.onGroupBoxDoubleClicked; } }
        private void GroupBoxDoubleClicked()
        {
            try
            {
                if (MFMPlanDetails.GroupHeader == "Planned")
                {
                    MFMPlanDetails.GroupHeader = "Awaiting";
                    MonthVisibility = Visibility.Hidden;
                    MFMPlanDetails.IsDocumentation = false;
                    MFMPlanDetails.IsForging = false;
                    MFMPlanDetails.IsPPAP = false;
                    MFMPlanDetails.IsSecondary = false;
                    MFMPlanDetails.IsTools = false;
                    MFMPlanDetails.IsAwaitingPSW = false;
                    MFMPlanDetails.IsApprovedPSW = false;
                }
                else
                {
                    MFMPlanDetails.GroupHeader = "Planned";
                    MonthVisibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand refreshCommand = null;
        public ICommand RefreshCommand { get { return this.refreshCommand; } }
        private void Refresh()
        {
            try
            {
                MFMPlanDetails.DVMFMPlanDetails = null;
                MFMPlanDetails.ProductCount = "0";
                dgrdMFMPlanDetails.FrozenColumnCount = 0;
               
                if (MFMPlanDetails.IsDocumentation || MFMPlanDetails.IsForging || MFMPlanDetails.IsPPAP || MFMPlanDetails.IsSecondary || MFMPlanDetails.IsTools || MFMPlanDetails.IsApprovedPSW || MFMPlanDetails.IsAwaitingPSW)
                {
                    MFMPlanDetails.DVMFMPlanDetails = null;
                    bll.GetMFMPlanDetils(MFMPlanDetails);
                    if (MFMPlanDetails.DVMFMPlanDetails == null || MFMPlanDetails.DVMFMPlanDetails.Count == 0)
                    {
                        ShowInformationMessage(PDMsg.NoRecordFound);
                    }
                    if (MFMPlanDetails.GroupHeader == "Awaiting" && MFMPlanDetails.IsDocumentation)
                    {
                        int indexModule = dgrdMFMPlanDetails.Columns.Single(c => c.SortMemberPath == "MODULE").DisplayIndex;
                        dgrdMFMPlanDetails.ColumnFromDisplayIndex(indexModule).Visibility = Visibility.Collapsed;
                        int indexcc_abbr = dgrdMFMPlanDetails.Columns.Single(c => c.SortMemberPath == "CC_ABBR").DisplayIndex;
                        dgrdMFMPlanDetails.ColumnFromDisplayIndex(indexcc_abbr).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        int indexModule = dgrdMFMPlanDetails.Columns.Single(c => c.SortMemberPath == "MODULE").DisplayIndex;
                        dgrdMFMPlanDetails.ColumnFromDisplayIndex(indexModule).Visibility = Visibility.Visible;
                        int indexcc_abbr = dgrdMFMPlanDetails.Columns.Single(c => c.SortMemberPath == "CC_ABBR").DisplayIndex;
                        dgrdMFMPlanDetails.ColumnFromDisplayIndex(indexcc_abbr).Visibility = Visibility.Visible;
                    }
                    int index = dgrdMFMPlanDetails.Columns.Single(c => c.SortMemberPath == "PART_NO").DisplayIndex;
                    dgrdMFMPlanDetails.FrozenColumnCount = index + 1;
                }
                else
                {
                    MFMPlanDetails.ProductCount = "0";
                    ShowInformationMessage("Please select any one");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void OnBeginningEditMFMPlan(object sender, DataGridBeginningEditEventArgs e)
        {
            dgrdMFMPlanDetails.ColumnFromDisplayIndex(0).DisplayIndex = dgrdMFMPlanDetails.CurrentColumn.DisplayIndex;
            //for (int i = 0; i < dgrdMFMPlanDetails.CurrentColumn.DisplayIndex; i++)
            //{
            //    dgrdMFMPlanDetails.ColumnFromDisplayIndex(i + 1).DisplayIndex = i;
            //}

            //int index = dgrdMFMPlanDetails.Columns.Single(c => c.SortMemberPath == "PART_NO").DisplayIndex;
            //dgrdMFMPlanDetails.FrozenColumnCount = index + 1;

            e.Cancel = true;
        }

        public void OnColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            int index = dgrdMFMPlanDetails.Columns.Single(c => c.SortMemberPath == "PART_NO").DisplayIndex;
            dgrdMFMPlanDetails.FrozenColumnCount = index + 1;
        }


        private readonly ICommand editDataCommand = null;
        public ICommand EditDataCommand { get { return this.editDataCommand; } }
        private void EditData()
        {
            try
            {
                frmMFMPlan mfmplan = new frmMFMPlan(userInformation);
                mfmplan.ShowInTaskbar = true;
                mfmplan.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand printCommand = null;
        public ICommand PrintCommand { get { return this.printCommand; } }
        private void Print()
        {
            try
            {
                MdiChild mfmdev = new MdiChild();
                mfmdev.Title = ApplicationTitle + " - MFM Development Plan";
                ProcessDesigner.frmMfmDevelopment mfmdevelopment = new ProcessDesigner.frmMfmDevelopment(mfmdev, userInformation);
                mfmdev.Content = mfmdevelopment;
                mfmdev.Height = mfmdevelopment.Height + 40;
                mfmdev.Width = mfmdevelopment.Width + 20;                                
                mfmdev.Resizable = false;
                mfmdev.MinimizeBox = false;
                mfmdev.MaximizeBox = false;
                MainMDI.Container.Children.Add(mfmdev);                
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
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
    }
}
