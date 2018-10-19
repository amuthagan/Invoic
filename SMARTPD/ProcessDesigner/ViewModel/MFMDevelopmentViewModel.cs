using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class MFMDevelopmentViewModel : ViewModelBase
    {
        private UserInformation userInformation;
        private MFMDevelopmentBll bll;
        public Action CloseAction { get; set; }

        public MFMDevelopmentViewModel(UserInformation userinfo)
        {
            userInformation = userinfo;
            mfmdevelopment = new MFMDevelopmentModel();
            bll = new MFMDevelopmentBll(userinfo);
            this._windowLoadedCommand = new DelegateCommand(this.WindowLoaded);
            this._optionButtonClickCommand = new DelegateCommand(this.OptionButton_Clicked);
            this._leadTimeRefreshCommand = new DelegateCommand(this.LeadTime_Refresh);
            this._pswRefreshCommand = new DelegateCommand(this.PSW_Refresh);
            this._pswPrintCommand = new DelegateCommand(this.PSW_Print);
            this._leadTimePrintCommand = new DelegateCommand(this.LeadTime_Print);
            this._printCommand = new DelegateCommand(this.Print);
            this._closeCommand = new DelegateCommand(this.Close);

            DropDownHeaderLoc = new ObservableCollection<DropdownColumns>
            {               
                new DropdownColumns { ColumnName = "LOC_CODE", ColumnDesc = "Code", ColumnWidth = 100 },
                new DropdownColumns { ColumnName = "LOCATION", ColumnDesc = "Location", ColumnWidth = "1*" }
            };

            DropDownHeaderCust = new ObservableCollection<DropdownColumns>
            {               
                new DropdownColumns { ColumnName = "CUST_CODE", ColumnDesc = "Code", ColumnWidth = 100 },
                new DropdownColumns { ColumnName = "CUST_NAME", ColumnDesc = "Customer Name", ColumnWidth = "1*" }
            };
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderLoc = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderLoc
        {
            get { return this._dropDownHeaderLoc; }
            set
            {
                this._dropDownHeaderLoc = value;
                NotifyPropertyChanged("DropDownHeaderLoc");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderCust = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderCust
        {
            get { return this._dropDownHeaderCust; }
            set
            {
                this._dropDownHeaderCust = value;
                NotifyPropertyChanged("DropDownHeaderCust");
            }
        }

        private Visibility _plantVisibility = Visibility.Visible;
        public Visibility PlantVisibility
        {
            get { return _plantVisibility; }
            set
            {
                _plantVisibility = value;
                NotifyPropertyChanged("PlantVisibility");
            }
        }

        private Visibility _dateRangeVisibility = Visibility.Hidden;
        public Visibility DateRangeVisibility
        {
            get { return _dateRangeVisibility; }
            set
            {
                _dateRangeVisibility = value;
                NotifyPropertyChanged("DateRangeVisibility");
            }
        }

        private Visibility _leadVisibility = Visibility.Hidden;
        public Visibility LeadVisibility
        {
            get { return _leadVisibility; }
            set
            {
                _leadVisibility = value;
                NotifyPropertyChanged("LeadVisibility");
            }
        }

        private Visibility _pswVisibility = Visibility.Hidden;
        public Visibility PSWVisibility
        {
            get { return _pswVisibility; }
            set
            {
                _pswVisibility = value;
                NotifyPropertyChanged("PSWVisibility");
            }
        }

        private String _pswHeading = "";
        public String PSWHeading
        {
            get { return _pswHeading; }
            set
            {
                _pswHeading = value;
                NotifyPropertyChanged("PSWHeading");
            }
        }

        private String _leadTimeText = "Lead Time For Parts Developed During ______ to ______ Qualifying _____ Days ";
        public String LeadTimeText
        {
            get { return _leadTimeText; }
            set
            {
                _leadTimeText = value;
                NotifyPropertyChanged("LeadTimeText");
            }
        }

        private String _pswText = "Parts Developed During ______ to ______ ";
        public String PSWText
        {
            get { return _pswText; }
            set
            {
                _pswText = value;
                NotifyPropertyChanged("PSWText");
            }
        }

        private List<String> _from_activity = null;
        public List<String> FROM_ACTIVITY
        {
            get { return this._from_activity; }
            set
            {
                this._from_activity = value;
                NotifyPropertyChanged("FROM_ACTIVITY");
            }
        }

        private List<String> _to_activity = null;
        public List<String> TO_ACTIVITY
        {
            get { return this._to_activity; }
            set
            {
                this._to_activity = value;
                NotifyPropertyChanged("TO_ACTIVITY");
            }
        }

        private readonly ICommand _windowLoadedCommand;
        public ICommand WindowLoadedCommand { get { return this._windowLoadedCommand; } }
        private void WindowLoaded()
        {
            try
            {
                bll.GetDropdownDetails(MFMDevelopment);

                FROM_ACTIVITY = new List<string>();
                FROM_ACTIVITY.Add("Allotted Date");
                FROM_ACTIVITY.Add("Document Date");
                FROM_ACTIVITY.Add("Tool Date");
                FROM_ACTIVITY.Add("Forging Date");
                FROM_ACTIVITY.Add("Secondary Date");
                FROM_ACTIVITY.Add("PPAP Date");

                MFMDevelopment.STAGE_START = "Allotted Date";

                TO_ACTIVITY = new List<string>();
                TO_ACTIVITY.Add("Document Date");
                TO_ACTIVITY.Add("Tool Date");
                TO_ACTIVITY.Add("Forging Date");
                TO_ACTIVITY.Add("Secondary Date");
                TO_ACTIVITY.Add("PPAP Date");
                TO_ACTIVITY.Add("PSW Date");

                MFMDevelopment.STAGE_END = "Document Date";
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private MFMDevelopmentModel mfmdevelopment = null;
        public MFMDevelopmentModel MFMDevelopment
        {
            get { return mfmdevelopment; }
            set
            {
                mfmdevelopment = value;
                NotifyPropertyChanged("MFMDevelopment");
            }
        }



        private readonly ICommand _optionButtonClickCommand;
        public ICommand OptionButtonClickCommand { get { return this._optionButtonClickCommand; } }
        private void OptionButton_Clicked()
        {
            try
            {
                if (MFMDevelopment.AwaitingDoc || MFMDevelopment.AwaitingTools || MFMDevelopment.AwaitingForging || MFMDevelopment.AwaitingSecondary || MFMDevelopment.AwaitingPPAP)
                {
                    DateRangeVisibility = Visibility.Hidden;
                    LeadVisibility = Visibility.Hidden;
                    PSWVisibility = Visibility.Hidden;
                    PlantVisibility = Visibility.Visible;
                }
                else if (MFMDevelopment.AwaitingPSWApproval)
                {
                    PSWHeading = "Product Awaiting PSW";
                    DateRangeVisibility = Visibility.Visible;
                    LeadVisibility = Visibility.Hidden;
                    PSWVisibility = Visibility.Visible;
                    PlantVisibility = Visibility.Visible;
                }
                else if (MFMDevelopment.PSWApproved)
                {
                    PSWHeading = "Product PSW Approved";
                    DateRangeVisibility = Visibility.Visible;
                    LeadVisibility = Visibility.Hidden;
                    PSWVisibility = Visibility.Visible;
                    PlantVisibility = Visibility.Visible;
                }
                else if (MFMDevelopment.NoOfShifts || MFMDevelopment.FirstTimeRight || MFMDevelopment.CustomerComp)
                {
                    DateRangeVisibility = Visibility.Visible;
                    LeadVisibility = Visibility.Hidden;
                    PSWVisibility = Visibility.Hidden;
                    PlantVisibility = Visibility.Hidden;
                }
                else if (MFMDevelopment.LeadTime)
                {
                    DateRangeVisibility = Visibility.Visible;
                    LeadVisibility = Visibility.Visible;
                    PSWVisibility = Visibility.Hidden;
                    PlantVisibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private readonly ICommand _leadTimeRefreshCommand;
        public ICommand LeadTimeRefreshCommand { get { return this._leadTimeRefreshCommand; } }
        private void LeadTime_Refresh()
        {
            try
            {
                if (MFMDevelopment.START_DATE == null)
                {
                    ShowInformationMessage("Please Fill the From Date");
                    return;
                }

                if (MFMDevelopment.END_DATE == null)
                {
                    ShowInformationMessage("Please Fill the To Date");
                    return;
                }

                if (MFMDevelopment.START_DATE > MFMDevelopment.END_DATE)
                {
                    ShowInformationMessage("End Date is Lesser than the Start Date");
                    return;
                }

                LeadTimeText = "Lead Time For Parts Developed During " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " to " + MFMDevelopment.END_DATE.ToFormattedDateAsString() + " Qualifying " + MFMDevelopment.TARGET_TIME.ToString() + " Days";

                bll.LoadTime_Refresh(MFMDevelopment);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _pswRefreshCommand;
        public ICommand PSWRefreshCommand { get { return this._pswRefreshCommand; } }
        private void PSW_Refresh()
        {
            try
            {
                if (MFMDevelopment.START_DATE == null)
                {
                    ShowInformationMessage("Please Fill the From Date");
                    return;
                }

                if (MFMDevelopment.END_DATE == null)
                {
                    ShowInformationMessage("Please Fill the To Date");
                    return;
                }

                if (MFMDevelopment.START_DATE > MFMDevelopment.END_DATE)
                {
                    ShowInformationMessage("End Date is Lesser than the Start Date");
                    return;
                }

                PSWText = "Parts Developed During " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " to " + MFMDevelopment.END_DATE.ToFormattedDateAsString();

                bll.PSW_Refresh(MFMDevelopment);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _pswPrintCommand;
        public ICommand PSWPrintCommand { get { return this._pswPrintCommand; } }
        private void PSW_Print()
        {
            try
            {
                String subheading = "";
                MFMDevelopment.DTPSWPrint = null;
                PSW_Refresh();
                if (MFMDevelopment.DTPSWPrint != null && MFMDevelopment.DTPSWPrint.Rows.Count > 0)
                {
                    //MFMDevelopment.DTPSWPrint.WriteXmlSchema(@"E:\PSW.xml");
                    if (MFMDevelopment.AwaitingPSWApproval)
                    {
                        subheading = "Parts PSW NOT Approved and Devloped During " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " to " + MFMDevelopment.END_DATE.ToFormattedDateAsString();
                        frmReportViewer rv = new frmReportViewer(MFMDevelopment.DTPSWPrint, "PSW", subheading);
                        rv.ShowDialog();
                    }
                    else
                    {
                        subheading = "Parts PSW Approved and Devloped During " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " to " + MFMDevelopment.END_DATE.ToFormattedDateAsString();
                        frmReportViewer rv = new frmReportViewer(MFMDevelopment.DTPSWPrint, "PSWApp", subheading);
                        rv.ShowDialog();
                    }

                }
                else
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _leadTimePrintCommand;
        public ICommand LeadTimePrintCommand { get { return this._leadTimePrintCommand; } }
        private void LeadTime_Print()
        {
            try
            {
                String subheading = "";
                MFMDevelopment.DTLeadTimePrint = null;
                LeadTime_Refresh();
                if (MFMDevelopment.DTLeadTimePrint != null && MFMDevelopment.DTLeadTimePrint.Rows.Count > 0)
                {
                    //MFMDevelopment.DTLeadTimePrint.WriteXmlSchema(@"E:\LeadTime.xml");
                    subheading = " Lead Time For Parts Devloped During " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " to " + MFMDevelopment.END_DATE.ToFormattedDateAsString()
                                 + " From Activity " + MFMDevelopment.STAGE_START + " To Activity " + MFMDevelopment.STAGE_END + " under " + MFMDevelopment.TARGET_TIME + " days ";
                    frmReportViewer rv = new frmReportViewer(MFMDevelopment.DTLeadTimePrint, "LeadTime", subheading);
                    rv.ShowDialog();
                }
                else
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _printCommand;
        public ICommand PrintCommand { get { return this._printCommand; } }
        private void Print()
        {
            try
            {
                String subheading = "";
                DataTable dtMFMDev = new DataTable();
                if (MFMDevelopment.AwaitingForging || MFMDevelopment.AwaitingPPAP)
                {
                    if (!MFMDevelopment.LOC_CODE.IsNotNullOrEmpty())
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Location"));
                        return;
                    }
                }

                if (MFMDevelopment.NoOfShifts || MFMDevelopment.FirstTimeRight || MFMDevelopment.CustomerComp)
                {
                    if (MFMDevelopment.START_DATE == null)
                    {
                        ShowInformationMessage("Please Fill the From Date");
                        return;
                    }

                    if (MFMDevelopment.END_DATE == null)
                    {
                        ShowInformationMessage("Please Fill the To Date");
                        return;
                    }

                    if (MFMDevelopment.START_DATE > MFMDevelopment.END_DATE)
                    {
                        ShowInformationMessage("End Date is Lesser than the Start Date");
                        return;
                    }
                }

                dtMFMDev = bll.GetMFMDevelopment(MFMDevelopment);
                if (dtMFMDev == null || dtMFMDev.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                    return;
                }

                if (MFMDevelopment.AwaitingDoc)
                {                   
                    subheading = "AWAITING DOCUMENTATION";
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "AwaitingDoc", subheading);
                    rv.ShowDialog();
                }
                else if (MFMDevelopment.AwaitingTools)
                {                   
                    subheading = "AWAITING TOOLS";
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "AwaitingTools", subheading);
                    rv.ShowDialog();
                }
                else if (MFMDevelopment.AwaitingForging)
                {
                    //dtMFMDev.WriteXmlSchema(@"E:\AwaitingTools.xml");
                    subheading = "AWAITING FORGING";
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "AwaitingForging", subheading);
                    rv.ShowDialog();
                }
                else if (MFMDevelopment.AwaitingSecondary)
                {                   
                    subheading = "AWAITING SECONDARY";
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "AwaitingSecondary", subheading);
                    rv.ShowDialog();
                }
                else if (MFMDevelopment.AwaitingPPAP)
                {
                    subheading = "AWAITING PPAP";
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "AwaitingPPAP", subheading);
                    rv.ShowDialog();
                }
                else if (MFMDevelopment.FirstTimeRight)
                {                   
                    subheading = "FIRST TIME RIGHT PRODUCTS BETWEEN " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " AND " + MFMDevelopment.END_DATE.ToFormattedDateAsString();
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "FirstTimeRight", subheading);
                    rv.ShowDialog();
                }
                else if (MFMDevelopment.CustomerComp)
                {                   
                    subheading = "CUSTOMER COMPLAINTS BETWEEN " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " AND " + MFMDevelopment.END_DATE.ToFormattedDateAsString();
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "CustomerComp", subheading);
                    rv.ShowDialog();
                }
                else if (MFMDevelopment.NoOfShifts)
                {
                    //dtMFMDev.WriteXmlSchema(@"E:\NoOfShifts.xml");
                    subheading = "No of Shifts Taken for development and No of items  for which tool change made during proving  Between " + MFMDevelopment.START_DATE.ToFormattedDateAsString() + " AND " + MFMDevelopment.END_DATE.ToFormattedDateAsString();
                    frmReportViewer rv = new frmReportViewer(dtMFMDev, "NoOfShifts", subheading);
                    rv.ShowDialog();
                }


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
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
