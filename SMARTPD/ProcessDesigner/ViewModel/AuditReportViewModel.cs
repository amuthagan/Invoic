using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProcessDesigner.ViewModel
{
    public class AuditReportViewModel : ViewModelBase
    {
        AuditReportBll bll = null;
        public AuditReportViewModel(UserInformation userinfo)
        {
            bll = new AuditReportBll(userinfo);
            _auditreport = new AuditReportModel();
            this.generateCommand = new DelegateCommand(this.Generate);
            this.printCommand = new DelegateCommand(this.Print);
            this.checkedchangedcommand = new DelegateCommand(this.CheckedChanged);
        }

        private AuditReportModel _auditreport = null;
        
        public AuditReportModel AuditReport
        {
            get { return this._auditreport; }
            set
            {
                this._auditreport = value;
                NotifyPropertyChanged("AuditReport");
            }
        }

        private Visibility _printButtonVisibility = Visibility.Hidden;
        public Visibility PrintButtonVisibility
        {
            get { return _printButtonVisibility; }
            set
            {
                this._printButtonVisibility = value;
                NotifyPropertyChanged("PrintButtonVisibility");
            }
        }

        private readonly ICommand generateCommand = null;
        public ICommand GenerateCommand { get { return this.generateCommand; } }
        private void Generate()
        {
            try
            {
                if (AuditReport.NineDigitPartNo == false && AuditReport.SixDigitPartNo == false)
                {
                    ShowInformationMessage("Please select Part Number Type.");
                    return;
                }

                bll.GeneratePrintDetails(AuditReport);
                PrintButtonVisibility = Visibility.Visible;
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
                DataTable dtAuditReport = AuditReport.DVAuditReport.Table.Copy();
                if (dtAuditReport == null || dtAuditReport.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                    return;
                }

                foreach (DataRow dr in dtAuditReport.Rows)
                {
                    if (dr["CUS_ISSUEDATE"].ToString() == " 7:01:01PM" || dr["CUS_ISSUEDATE"].ToString() == "12:00:00AM")
                    {
                        dr.BeginEdit();
                        dr["CUS_ISSUEDATE"] = "";
                        dr.EndEdit();
                    }
                }

                frmReportViewer rv = new frmReportViewer(dtAuditReport, "AuditReport");
                rv.ShowDialog();

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

        private readonly ICommand checkedchangedcommand = null;
        public ICommand CheckedChangedCommand { get { return this.checkedchangedcommand; } }
        private void CheckedChanged()
        {
            try
            {
                PrintButtonVisibility = Visibility.Hidden;
                AuditReport.DVAuditReport = null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        Boolean isclosed = false;
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

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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
