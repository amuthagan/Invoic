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
    public class QcpViewModel : ViewModelBase
    {
        private PCCSBll _pccsBll;
        private QcpBll qcpBll;
        public QcpViewModel(UserInformation userInformation)
        {
            QCPMODEL = new QcpModel();
            _pccsBll = new PCCSBll(userInformation);
            qcpBll = new QcpBll(userInformation);
            PccsModel = new PCCSModel();
            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            this.qcpReportClickCommand = new DelegateCommand(this.qcpReport);
            LoadCmbDatas();
            SetdropDownItems();
            //QCPMODEL.PartNo = qcpBll.GetPartNo(QCPMODEL).ToString();
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

        private QcpModel _qcpmodel;
        public QcpModel QCPMODEL
        {
            get
            {
                return _qcpmodel;
            }
            set
            {
                _qcpmodel = value;
                NotifyPropertyChanged("QCPMODEL");
            }
        }

        private void LoadCmbDatas()
        {
            _pccsBll.GetPartNoDetails(PccsModel);
        }

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
        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private readonly ICommand qcpReportClickCommand;
        public ICommand QcpReportClickCommand { get { return this.qcpReportClickCommand; } }
        private void qcpReport()
        {
            //SelectDataRowPart();

            if (!QCPMODEL.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                //ShowInformationMessage("Part No does not exists"); // Jeyan
                return;
            }
            else
            {
                DataTable processData;
                processData = qcpBll.GetQCP(PccsModel, QCPMODEL);
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "QCP");
                    rv.ShowDialog();
                }

            }
        }

        public Action CloseAction { get; set; }

        public PCCSModel _pccsModel { get; set; }
    }
}
