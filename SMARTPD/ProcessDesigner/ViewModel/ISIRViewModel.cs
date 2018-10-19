using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Data;
using ProcessDesigner.Common;
using System.Windows;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class ISIRViewModel : ViewModelBase
    {
        private ISIRBll isir;
        public ISIRViewModel(UserInformation userInformation, ISIRModel im)
        {
            
            isir = new ISIRBll(userInformation);
            Isirmodel = new ISIRModel();
            Isirmodel.PART_NO = im.PART_NO;
            Isirmodel = im;
            isir.GetISIR(Isirmodel);
            Isirmodel.DATE = isir.ServerDateTime();
            Isirmodel.DATE1 = Isirmodel.DATE.ToFormattedDateAsString();
            Isirmodel.NOOFSAMPLE = Convert.ToString(10);
            #region Event Init
            this.iSIRPrintCommand = new DelegateCommand(this.ISIRPrintReport);
            this.iSIRCancelCommand = new DelegateCommand(this.ISIRCancelReport);
            #endregion
        }

        
        private ISIRModel _isirmodel;
        public ISIRModel Isirmodel
        {
            get 
            { 
                return _isirmodel; 
            }
            set 
            { 
                _isirmodel = value;
                NotifyPropertyChanged("Isirmodel");
            }
        }

        private readonly ICommand iSIRPrintCommand;
        public ICommand ISIRPrintCommand { get { return this.iSIRPrintCommand; } }
        private void ISIRPrintReport()
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            
            List<ISIRModel> li = new List<ISIRModel>();
            li.Add(Isirmodel);
            dt = li.ToDataTable<ISIRModel>();
            if (dt == null || dt.Rows.Count == 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
            }
            else
            {

                frmReportViewer rv = new frmReportViewer(dt, "ISIR");
                rv.ShowDialog();
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        public Action CloseAction { get; set; }

        private readonly ICommand iSIRCancelCommand;
        public ICommand ISIRCancelCommand { get { return this.iSIRCancelCommand; } }
        private void ISIRCancelReport()
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

        //public void TextBoxDateValidation(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        TextBox tb = (TextBox)sender;

        //        if (!string.IsNullOrEmpty(tb.Text.Trim()))
        //        {
        //            if (UserControls.DateValidation.CheckIsValidDate(tb.Text.ToString().Trim()) == false)
        //            {
        //                MessageBox.Show("Invalid Date", "smartPD", MessageBoxButton.OK, MessageBoxImage.Information);
        //                if (tb.Tag.ToString() == "DATE")
        //                {
        //                    Isirmodel.DATE = null;
        //                    tb.Text = string.Empty;
        //                }
        //            }
        //            else
        //            {
        //                tb.Text = tb.Text.ToString().ToDateAsString("DD/MM/YYYY");
        //                if (tb.Tag.ToString() == "DATE")
        //                {
        //                    Isirmodel.DATE = DateTime.Parse(tb.Text.ToString());
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

    }
}