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
using WPF.MDI;

namespace ProcessDesigner
{
    class ReportPendingPartNumberStatusViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "PENDING PARTS STATUS";
        private const string REPORT_NAME = "PENDING_PARTS_STATUS";
        private const string REPORT_TITLE = "Pending Parts Status Report";

        ReportPendingPartNumberStatus bll = null;
        WPF.MDI.MdiChild mdiChild = null;

        public ReportPendingPartNumberStatusViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, bool refreshOnLoad = false, string title = REPORT_TITLE)
        {
            _userInformation = userInformation;
            this.mdiChild = mdiChild;

            bll = new ReportPendingPartNumberStatus(userInformation);
            MandatoryFields = new ReportPendingPartNumberStatusModel();
            MandatoryFields.GRID_TITLE = REPORT_TITLE;

            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            this.refreshCommand = new DelegateCommand(this.RefreshSubmitCommand);
            this.closeCommand = new DelegateCommand(this.Close);

            this.mouseDoubleClickGrdMeasuringDetails = new DelegateCommand<DataRowView>(this.SelectDataRowMouseDoubleClickMeasuringDetails);
            isFocusedDocRel = true;
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

        private bool isFocusedDocRel = false;
        public bool IsFocusedDocRel
        {
            get { return isFocusedDocRel; }
            set
            {
                this.isFocusedDocRel = value;
                NotifyPropertyChanged("IsFocusedDocRel");
            }
        }

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {
            isFocusedDocRel = true;
            DataSet dsReport = new DataSet(REPORT_NAME);
            if (MandatoryFields.GridData.IsNotNullOrEmpty())
            {
                dsReport.Tables.Add(MandatoryFields.GridData.Table.Copy());
            }

            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            //foreach (DataTable dt in dsReport.Tables)
            //{
            //    dt.Rows.Clear();
            //    dt.AcceptChanges();
            //}
            //dsReport.WriteXmlSchema("d:\\" + dsReport.DataSetName + ".xml");

            frmReportViewer reportViewer = new frmReportViewer(dsReport, REPORT_NAME);
            if (!reportViewer.ReadyToShowReport) return;
            reportViewer.ShowDialog();
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshClickCommand { get { return this.refreshCommand; } }
        private void RefreshSubmitCommand()
        {
            isFocusedDocRel = true;
            if (!MandatoryFields.DOC_REL_DT_PLAN.IsNotNullOrEmpty()) return;
            DataTable dtMFM_MAST = bll.GetAllPendingPartNumber(Convert.ToDateTime(MandatoryFields.DOC_REL_DT_PLAN));

            DataSet dsReport = new DataSet(REPORT_NAME);
            if (dtMFM_MAST.IsNotNullOrEmpty())
            {
                MandatoryFields.GridData = dtMFM_MAST.Copy().DefaultView;
                dsReport.Tables.Add(MandatoryFields.GridData.Table);
            }

            MandatoryFields.GRID_TITLE = REPORT_TITLE;
            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count <= 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
            MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";

        }

        public Action CloseAction { get; set; }
        private readonly ICommand closeCommand;
        public ICommand CloseCommand { get { return this.closeCommand; } }
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private ReportPendingPartNumberStatusModel _mandatoryFields = null;
        public ReportPendingPartNumberStatusModel MandatoryFields
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

        private readonly ICommand mouseDoubleClickGrdMeasuringDetails;
        public ICommand MouseDoubleClickGrdMeasuringDetails { get { return this.mouseDoubleClickGrdMeasuringDetails; } }
        private void SelectDataRowMouseDoubleClickMeasuringDetails(DataRowView selecteditem)
        {
            if (selecteditem.IsNotNullOrEmpty() && MandatoryFields.GridData.IsNotNullOrEmpty())
            {
                int idpk = -99999;

                DataView dv = MandatoryFields.GridData.Table.Copy().DefaultView;
                dv.RowFilter = "PART_NO='" + selecteditem["PART_NO"] + "'";
                if (dv.Count > 0)
                {
                    idpk = dv[0]["IDPK"].ToValueAsString().ToIntValue();
                }
                //Window win = new Window();
                //win.Title = ApplicationTitle + " - Prodcut Master";

                //System.Windows.Media.Imaging.IconBitmapDecoder ibd = new System.Windows.Media.Imaging.IconBitmapDecoder(new Uri(@"pack://application:,,/Images/logo.ico", UriKind.RelativeOrAbsolute), System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.Default);

                //win.Icon = ibd.Frames[0];
                //win.ResizeMode = ResizeMode.NoResize;
                //ProcessDesigner.frmProductInformation userControl = new ProcessDesigner.frmProductInformation(_userInformation, win, idpk, OperationMode.Edit, win.Title);
                //win.Content = userControl;
                //win.Height = userControl.Height + 50;
                //win.Width = userControl.Width + 10;
                //win.ShowInTaskbar = false;
                //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //win.ShowDialog();

                MdiChild frmProductInformationChild = new MdiChild
                {
                    Title = ApplicationTitle + " - Product Master",
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                ProcessDesigner.frmProductInformation productInformation = new ProcessDesigner.frmProductInformation(_userInformation,
                    frmProductInformationChild, idpk, OperationMode.Edit);
                frmProductInformationChild.Content = productInformation;
                frmProductInformationChild.Height = productInformation.Height + 50;
                frmProductInformationChild.Width = productInformation.Width + 20;
                MainMDI.Container.Children.Add(frmProductInformationChild);
            }

        }

    }
}
