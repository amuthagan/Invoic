using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using System.Windows.Input;
using System.Data;
using ProcessDesigner.Common;
using System.Windows;
using System.ComponentModel;

namespace ProcessDesigner.ViewModel
{
    public class SeqDwgIssueViewmodel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public bool Closed = false;
        private RawMaterial locBll = null;
        private UserInformation _userinformation;
        private DrawingBll drwBll;

        public SeqDwgIssueViewmodel(UserInformation userInfo, string partno)
        {
            _userinformation = userInfo;
            drwBll = new DrawingBll(_userinformation);
            locBll = new RawMaterial(_userinformation);
            DV_PROD_DWG_ISSUE = drwBll.GetRevisionDetailsWithOutNewRow(partno, "1");
            //  LocationMaster = locBll.GetLocations().ToDataTable<DDLOC_MAST>().DefaultView;
        }
        private DataView _locationMaster;
        public DataView LocationMaster
        {
            get
            {
                return _locationMaster;
            }
            set
            {
                _locationMaster = value;
                NotifyPropertyChanged("LocationMaster");
            }
        }

        private DataView _dvProdDwgIssue;
        public DataView DV_PROD_DWG_ISSUE
        {
            get { return _dvProdDwgIssue; }
            set
            {
                _dvProdDwgIssue = value;
                NotifyPropertyChanged("DV_PROD_DWG_ISSUE");
            }
        }
        //private void Close()
        //{
        //    try
        //    {
        //        _closed = false;
        //        if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
        //        {
        //            _closed = true;
        //            CloseAction();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}
        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (Closed == false)
                {
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
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
    }
}
