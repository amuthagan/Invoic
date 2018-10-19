using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class LogViewModel : ViewModelBase
    {
        private PCCSBll _pccsBll;
        private LogViewBll _logBll;
        public LogViewModel(UserInformation userInformation)
        {
            _pccsBll = new PCCSBll(userInformation);
            _logBll = new LogViewBll(userInformation);
            PccsModel = new PCCSModel();
            logmodel = new LogModel();
            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            LoadCmbDatas();
            SetdropDownItems();
        }

        private DataRowView _selectedrowpart;
        public DataRowView SelectedRowPart
        {
            get
            {
                return _selectedrowpart;
            }

            set
            {
                _selectedrowpart = value;
            }
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

        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            if (SelectedRowPart != null)
            {
                PccsModel.PartNo = SelectedRowPart["PART_NO"].ToString();
                PccsModel.PartDesc = SelectedRowPart["PART_DESC"].ToString();
            }
            if (PccsModel.PartNo.IsNotNullOrEmpty())
            {
                _logBll.GetLogDetails(logmodel, PccsModel);
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
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void LoadCmbDatas()
        {
            _pccsBll.GetPartNoDetails(PccsModel);
        }

        private PCCSModel _pccsModel;
        public PCCSModel PccsModel
        {
            get
            {
                return _pccsModel;
            }
            set
            {
                this._pccsModel = value;
                NotifyPropertyChanged("PccsModel");
            }
        }

        private LogModel _log;
        public LogModel logmodel
        {
            get { return _log; }
            set 
            { 
                this._log = value;
                NotifyPropertyChanged("logmodel");
            }
        }
        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        public Action CloseAction { get; set; }
    }
}
