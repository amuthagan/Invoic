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
using System.Windows.Input;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
    public class CostSheetViewModel : ViewModelBase
    {
        private CostSheetBll bll;
        private UserInformation userInformation;
        private DataRowView partNoSelectedItem = null;

        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }

        private readonly ICommand _onPartNoSelectionChanged;
        public ICommand OnPartNoSelectionChanged { get { return this._onPartNoSelectionChanged; } }
        private readonly ICommand _onProcessNoSelectionChanged;
        public ICommand OnProcessNoSelectionChanged { get { return this._onProcessNoSelectionChanged; } }

        private readonly ICommand _onRMSelectionChanged;
        public ICommand OnRMSelectionChanged { get { return this._onRMSelectionChanged; } }

        public Action CloseAction { get; set; }

        public CostSheetViewModel(UserInformation userinfo, string partNo, int routeNo)
        {
            _costSheet = new CostSheetModel();
            userInformation = userinfo;
            bll = new CostSheetBll(userinfo);

            this._closeCommand = new DelegateCommand(this.Close);
            this._onPartNoSelectionChanged = new DelegateCommand(this.PartNoSelectionChanged);
            this._onProcessNoSelectionChanged = new DelegateCommand(this.ProcessNoSelectionChanged);
            this._onRMSelectionChanged = new DelegateCommand(this.RMSelectionChanged);
            this._printCommand = new DelegateCommand(this.Print);
            this._onCheckedChanged = new DelegateCommand(this.CheckedChanged);

            bll.GetProductMaster(CostSheet);
            bll.GetDropDownSource(CostSheet);
            CostSheet.PART_NO = partNo;
            PartNoSelectionChanged();
            CostSheet.ROUTE_NO = routeNo;
            ProcessNoSelectionChanged();

            DropdownHeaders = new ObservableCollection<DropdownColumns>
            {               
                new DropdownColumns { ColumnName = "PART_NO", ColumnDesc = "Part Number", ColumnWidth = "1*" },
                new DropdownColumns { ColumnName = "PART_DESC", ColumnDesc = "Part Description", ColumnWidth = "1*" }
            };

            RouteNoDropdownHeaders = new ObservableCollection<DropdownColumns>
            {               
                new DropdownColumns { ColumnName = "ROUTE_NO", ColumnDesc = "Process Number", ColumnWidth = "1*" }              
            };

            RMDropdownHeaders = new ObservableCollection<DropdownColumns>
            {               
                new DropdownColumns { ColumnName = "RM_CODE", ColumnDesc = "RM Code", ColumnWidth = 120 },
                new DropdownColumns { ColumnName = "RM_DESC", ColumnDesc = "RM Description", ColumnWidth = "1*" }
            };
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

        private ObservableCollection<DropdownColumns> _routeNoDropdownHeaders = null;
        public ObservableCollection<DropdownColumns> RouteNoDropdownHeaders
        {
            get { return this._routeNoDropdownHeaders; }
            set
            {
                this._routeNoDropdownHeaders = value;
                NotifyPropertyChanged("RouteNoDropdownHeaders");
            }

        }

        private ObservableCollection<DropdownColumns> _rmDropdownHeaders = null;
        public ObservableCollection<DropdownColumns> RMDropdownHeaders
        {
            get { return this._rmDropdownHeaders; }
            set
            {
                this._rmDropdownHeaders = value;
                NotifyPropertyChanged("RMDropdownHeaders");
            }

        }

        private CostSheetModel _costSheet;
        public CostSheetModel CostSheet
        {
            get { return this._costSheet; }
            set
            {
                this._costSheet = value;
                NotifyPropertyChanged("CostSheet");
            }
        }

        public DataRowView PartNoSelectedItem
        {
            get { return this.partNoSelectedItem; }
            set
            {
                this.partNoSelectedItem = value;
                NotifyPropertyChanged("PartNoSelectedItem");
            }
        }

        private DataRowView processNoSelectedItem = null;
        public DataRowView ProcessNoSelectedItem
        {
            get { return this.processNoSelectedItem; }
            set
            {
                this.processNoSelectedItem = value;
                NotifyPropertyChanged("ProcessNoSelectedItem");
            }
        }

        private DataRowView costSheetSelectedItem = null;
        public DataRowView CostSheetSelectedItem
        {
            get { return this.costSheetSelectedItem; }
            set
            {
                this.costSheetSelectedItem = value;
                NotifyPropertyChanged("CostSheetSelectedItem");
            }
        }


        private void PartNoSelectionChanged()
        {
            try
            {
                if (CostSheet.PART_NO.IsNotNullOrEmpty() && PartNoSelectedItem == null && CostSheet.DVProductMaster != null)
                {
                    DataView dv = CostSheet.DVProductMaster.ToTable().Copy().DefaultView;
                    dv.RowFilter = "PART_NO = '" + CostSheet.PART_NO + "'";

                    if (dv.Count > 0) PartNoSelectedItem = dv[0];
                }

                if (CostSheet.PART_NO.IsNotNullOrEmpty() && CostSheet.DVProductMaster != null && PartNoSelectedItem != null && CostSheet.DVProductMaster.Count > 0)
                {
                    CostSheet.PART_DESC = PartNoSelectedItem["PART_DESC"].ToString();
                    bll.GetProcessMain(CostSheet);
                }
                else
                {
                    CostSheet.PART_DESC = "";
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void ProcessNoSelectionChanged()
        {
            try
            {
                CostSheet.ExportIsClicked = false;
                bll.GetCostSheetDetails(CostSheet);
                RMSelectionChanged();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _onCheckedChanged;
        public ICommand OnCheckedChanged { get { return this._onCheckedChanged; } }
        private void CheckedChanged()
        {
            try
            {
                CostSheet.ExportIsClicked = true;
                bll.GetCostSheetDetails(CostSheet);
                RMSelectionChanged();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RMSelectionChanged()
        {
            try
            {
                if (CostSheet.PART_NO.IsNotNullOrEmpty() && CostSheet.WIRE_ROD_CD.IsNotNullOrEmpty())
                {
                    DDRM_MAST rmm = (from o in bll.DB.DDRM_MAST
                                     where o.RM_CODE == CostSheet.WIRE_ROD_CD
                                     select o).FirstOrDefault<DDRM_MAST>();

                    if (rmm != null)
                    {
                        if (CostSheet.EXPORT)
                        {
                            CostSheet.RMCOST = rmm.EXP_COST * (("1.05").ToDecimalValue() * CostSheet.CHEESE_WT);
                            CostSheet.RMMAST = rmm.EXP_COST;
                        }
                        else
                        {
                            CostSheet.RMCOST = rmm.LOC_COST * (("1.05").ToDecimalValue() * CostSheet.CHEESE_WT);
                            CostSheet.RMMAST = rmm.LOC_COST;
                        }
                    }
                    else
                    {
                        CostSheet.RMCOST = 0;
                        CostSheet.RMMAST = 0;
                    }
                }

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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private readonly ICommand _printCommand;
        public ICommand PrintCommand { get { return this._printCommand; } }
        private void Print()
        {
            if (!CostSheet.PART_NO.IsNotNullOrEmpty())
            {
                MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!CostSheet.ROUTE_NO.IsNotNullOrEmpty())
            {
                MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (CostSheet.DVCostSheet == null || CostSheet.DVCostSheet.Count == 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
            }
            else
            {
                DataSet processData = new DataSet();
                processData.Tables.Add(CostSheet.DVCostSheet.ToTable("CostSheet"));
                processData.Tables.Add(CostSheetDetails());
                //processData.WriteXmlSchema(@"E:\CostSheet.xml");
                frmReportViewer rv = new frmReportViewer(processData, "CostSheet");
                rv.ShowDialog();
            }

        }

        private DataTable CostSheetDetails()
        {
            DataTable dt = new DataTable("CostSheetMain");
            dt.Columns.Add(new DataColumn("PART_NO"));
            dt.Columns.Add(new DataColumn("PART_DESC"));
            dt.Columns.Add(new DataColumn("ROUTE_NO"));
            dt.Columns.Add(new DataColumn("WIRE_ROD_CD"));
            dt.Columns.Add(new DataColumn("CUSTOMER"));
            dt.Columns.Add(new DataColumn("RMCOST", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("RMMAST", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("TOTAL", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("COST", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("REAL", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("FINISH_WT", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("CHEESE_WT", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("RMWGT", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("CUSTCODE"));
            dt.Columns.Add(new DataColumn("FINISH_DESC"));
            dt.Columns.Add(new DataColumn("EXPORT"));

            DataRow dr = dt.NewRow();

            dr["PART_NO"] = CostSheet.PART_NO;
            dr["PART_DESC"] = CostSheet.PART_DESC;
            dr["ROUTE_NO"] = CostSheet.ROUTE_NO;
            dr["WIRE_ROD_CD"] = CostSheet.WIRE_ROD_CD;
            dr["CUSTOMER"] = CostSheet.CUSTOMER;
            dr["RMCOST"] = CostSheet.RMCOST;
            dr["RMMAST"] = CostSheet.RMMAST;
            dr["TOTAL"] = CostSheet.TOTAL;
            dr["COST"] = CostSheet.COST;
            dr["REAL"] = CostSheet.REAL;
            dr["FINISH_WT"] = CostSheet.FINISH_WT;
            dr["CHEESE_WT"] = CostSheet.CHEESE_WT;
            dr["RMWGT"] = CostSheet.CHEESE_WT * ("1.05").ToDecimalValue();
            dr["CUSTCODE"] = CostSheet.CUSTCODE;
            dr["FINISH_DESC"] = CostSheet.FINISH_DESC;
            dr["CUSTCODE"] = CostSheet.CUSTCODE;
            dr["EXPORT"] = (CostSheet.EXPORT) ? "Yes" : "No";

            dt.Rows.Add(dr);
            return dt;
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
