using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Globalization;
using System.Windows.Controls;
using ProcessDesigner.Model;
using System.ComponentModel.DataAnnotations;
using log4net;
using log4net.Config;




namespace ProcessDesigner.ViewModel
{
    public class RPDViewModel : ViewModelBase
    {
        private BLL.RPPBLL rpdmodel;
        private Model.RPDModel _model_rpd;
        private UserInformation userinformation;
        private readonly ICommand selectChangePartNo;
        private readonly ICommand selectChangeCIRNo;
        private readonly ICommand lostFocusCirNo;

        private readonly Common.LogWriter log;

        private readonly ICommand selectCustomerCode;
        private readonly ICommand updateCommand;
        private readonly ICommand printCommand;
        private readonly ICommand rowEditEndingSubTypeCommand;
        private readonly ICommand standardClickCommand;
        private readonly ICommand speicalClickCommand;
        private readonly ICommand speicalGridClickCommandYes;
        private readonly ICommand speicalGridClickCommandNo;
        private readonly ICommand gridLostFocus;

        private readonly ICommand gridDeleteCommand;
        private bool dispmessage = false;

        private static readonly ILog _LOG = LogManager.GetLogger("MyLogger");

        public RPDViewModel()
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            log4net.Config.XmlConfigurator.Configure();

            log = new LogWriter();

            rpdmodel = new RPPBLL(userinformation);
            Model_Rpd = new RPDModel();

            ClearFields();


            DtCIReference = rpdmodel.GetCIReferrence();
            DtPartNo = rpdmodel.GetPartNo();
            DtCustomer = rpdmodel.GetCustomerCode(0);
            this.selectChangePartNo = new DelegateCommand(this.SelectDataRow);
            this.selectChangeCIRNo = new DelegateCommand(this.SelectCIRDataRow);
            this.lostFocusCirNo = new DelegateCommand(this.LostFocusCirNoDataRow);
            this.selectCustomerCode = new DelegateCommand(this.SelectCustomerDataRow);
            this.updateCommand = new DelegateCommand(this.SaveRecord);
            this.printCommand = new DelegateCommand(this.PrintData);
            this.standardClickCommand = new DelegateCommand(this.standardClick);
            this.speicalClickCommand = new DelegateCommand(this.SpeicalClick);
            this.speicalGridClickCommandYes = new DelegateCommand(this.GridEnable);
            this.speicalGridClickCommandNo = new DelegateCommand(this.GridDisable);
            this.rowEditEndingSubTypeCommand = new DelegateCommand<Object>(this.RowEditEndingSubType);
            //     this.gridLostFocus = new DelegateCommand<object>(this.GridDataDuplicate);
            this.gridDeleteCommand = new DelegateCommand<object>(this.GridRowDelete);
            Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CI_REFERENCE", ColumnDesc = "CI REFERENCE", ColumnWidth = 130 },
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "CUST PART NO ", ColumnWidth = 130 },
                            new DropdownColumns() { ColumnName = "SFLPART_NO", ColumnDesc = "SFL PART NO ", ColumnWidth = "1*" },
                           // new DropdownColumns() { ColumnName = "IDPK", ColumnDesc = "IDPK", ColumnWidth = 200 },
                           //  new DropdownColumns() { ColumnName = "CIREF_NO_FK", ColumnDesc = "CIREF_NO_FK ", ColumnWidth = "1*" }
                        };
            PartColumns = new ObservableCollection<DropdownColumns>()
                            {
                             new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = 200 },
                             new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "PART DESC", ColumnWidth = "1*" }
                             };
            PartCustomerColumns = new ObservableCollection<DropdownColumns>()
                             {
                              new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "CUST CODE", ColumnWidth = 200 },
                              new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "CUST NAME", ColumnWidth = "1*" }
                             };

            ActionPermission = rpdmodel.GetUserRights("PRODUCT DEVELOPMENT");
            if (ActionPermission.Edit == true || ActionPermission.AddNew == true)
            {
                SaveEnable = true;
            }
            else
            {
                SaveEnable = false;
            }

            if (ActionPermission.View == true)
            {
                PrintEnable = true;
            }
            else
            {
                PrintEnable = false;
            }





        }

        //private ProgressBar progressBar;
        //public ProgressBar progressBar()
        //{
        //    get
        //}


        private Visibility txtPacking;
        public Visibility TxtPacking
        {
            get
            {
                return txtPacking;
            }
            set
            {
                txtPacking = value;
                NotifyPropertyChanged("TxtPacking");
            }
        }

        private Visibility lblPacking;
        public Visibility LblPacking
        {
            get
            {
                return lblPacking;
            }
            set
            {
                lblPacking = value;
                NotifyPropertyChanged("LblPacking");
            }
        }

        //public ICommand GridLostFocus { get { return this.gridLostFocus; } }
        //public void GridDataDuplicate(Object selecteditem)
        //{
        //    bool retval = true;
        //    int rowindex;
        //    Model_Rpd.SelectedItem = (DataRowView)selecteditem;
        //    //CostDetailsSelectedRow.EndEdit();
        //    Model_Rpd.SelectedItem.EndEdit();
        //    if (Model_Rpd.SelectedItem != null)
        //    {
        //        rowindex = Convert.ToInt16(Model_Rpd.SelectedItem.Row["SLNO"].ToString());

        //        //if (dr["SLNO"].ToString().Trim() != "" || dr["CHARACTERISTIC"].ToString().Trim() != "" || dr["SEVERITY"].ToString().Trim() != "")
        //        Model_Rpd.GridData.Table.Rows[rowindex - 1]["CHARACTERISTIC"] = Model_Rpd.SelectedItem.Row["CHARACTERISTIC"].ToString();
        //        Model_Rpd.GridData.Table.Rows[rowindex - 1]["SEVERITY"] = Model_Rpd.SelectedItem.Row["SEVERITY"].ToString();
        //        Model_Rpd.GridData.Table.Rows[rowindex - 1]["CUSTOMER_EXP"] = Model_Rpd.SelectedItem.Row["CUSTOMER_EXP"].ToString();
        //        Model_Rpd.GridData.Table.AcceptChanges();
        //        retval = CheckGridDuplicate(rowindex);
        //        if (retval == false)
        //        {
        //            return;
        //            //MessageBox.Show("Duplicate Characteristics has been Entered ", "smartPD", MessageBoxButton.OK, MessageBoxImage.Information);
        //            //// MessageBox.Show("Duplicate Entry");
        //            //Model_Rpd.GridData.Table.Rows[rowindex - 1]["CHARACTERISTIC"] = "";
        //            //Model_Rpd.GridData.Table.Rows[rowindex - 1]["SEVERITY"] = "";
        //            //Model_Rpd.GridData.Table.Rows[rowindex - 1]["CUSTOMER_EXP"] = "";
        //        }
        //    }
        //}

        public ICommand SpeicalClickCommand { get { return this.speicalClickCommand; } }
        public void SpeicalClick()
        {
            if (Model_Rpd.Opt_Special == true)
            {
                TxtPacking = Visibility.Visible;
                LblPacking = Visibility.Visible;
            }
        }
        public ICommand StandardClickCommand { get { return this.standardClickCommand; } }
        public void standardClick()
        {
            if (Model_Rpd.Opt_Stand == true)
            {
                if (Model_Rpd.NATURE_PACKING == null)
                {
                    TxtPacking = Visibility.Collapsed;
                    LblPacking = Visibility.Collapsed;
                    return;
                }

                if (Model_Rpd.NATURE_PACKING.ToString().Trim() != "")
                {
                    Model_Rpd.Opt_Stand = false;
                    Model_Rpd.Opt_Special = true;
                    ShowInformationMessage(PDMsg.NormalString("Nature of Packing should be made empty before choosing Standard Option"));
                    return;
                }
                else
                {
                    TxtPacking = Visibility.Collapsed;
                    LblPacking = Visibility.Collapsed;
                }
            }
        }

        private Visibility gridVisible;
        public Visibility GridVisible
        {
            get
            {
                return gridVisible;
            }
            set
            {
                gridVisible = value;
                NotifyPropertyChanged("GridVisible");
            }
        }

        public ICommand GridDeleteCommand { get { return this.gridDeleteCommand; } }
        public void GridRowDelete(Object selecteditem)
        {
            Model_Rpd.SelectedItem = (DataRowView)selecteditem;
            if (Model_Rpd.SelectedItem == null)
            {
                if (Model_Rpd.CI_REFERENCE.ToString().Trim() == "")
                {
                    return;
                }
                int currentrow;
                currentrow = Model_Rpd.GridData.Table.Rows.Count;
                if (currentrow == 0)
                {
                    DataRow newrow = Model_Rpd.GridData.Table.NewRow();
                    Model_Rpd.GridData.Table.Rows.InsertAt(newrow, 0);
                    Model_Rpd.GridData.Table.Rows[0]["SLNO"] = 1;
                    Model_Rpd.GridData.Table.AcceptChanges();
                }
                return;
            }
            else if (Model_Rpd.SelectedItem != null)
            {
                string deleteRow;
                Model_Rpd.SelectedItem = (DataRowView)selecteditem;
                deleteRow = Model_Rpd.SelectedItem.Row["SLNO"].ToString();

                if (deleteRow != null && deleteRow.ToString().Trim() != "")
                {
                    Model_Rpd.GridData.Table.Rows[Convert.ToInt16(deleteRow) - 1].Delete();
                    Model_Rpd.GridData.Table.AcceptChanges();
                }
            }

            if (Model_Rpd.GridData.Table.Rows.Count > 0)
            {
                int j = 1;
                for (int i = 0; i < Model_Rpd.GridData.Table.Rows.Count; i++)
                {
                    Model_Rpd.GridData.Table.Rows[i]["SLNO"] = j;
                    j = j + 1;
                }
            }
            else
            {
                int currentrow;
                currentrow = Model_Rpd.GridData.Table.Rows.Count;
                if (currentrow == 0)
                {
                    DataRow newrow = Model_Rpd.GridData.Table.NewRow();
                    Model_Rpd.GridData.Table.Rows.InsertAt(newrow, 0);
                    Model_Rpd.GridData.Table.Rows[0]["SLNO"] = 1;
                    Model_Rpd.GridData.Table.AcceptChanges();
                }
            }



        }

        public ICommand SpeicalGridClickCommandYes { get { return this.speicalGridClickCommandYes; } }
        public void GridEnable()
        {
            if (Model_Rpd.Opt_Special_Yes == true)
            {
                GridVisible = Visibility.Visible;
                Model_Rpd.Opt_Special_No = false;

                if (Model_Rpd.CI_REFERENCE.ToString().Trim() != "" && Model_Rpd.CI_REFERENCE != null)
                {
                    int currentrow;
                    currentrow = Model_Rpd.GridData.Table.Rows.Count;
                    if (currentrow == 0)
                    {
                        DataRow newrow = Model_Rpd.GridData.Table.NewRow();
                        Model_Rpd.GridData.Table.Rows.InsertAt(newrow, 0);
                        Model_Rpd.GridData.Table.Rows[0]["SLNO"] = 1;
                        Model_Rpd.GridData.Table.AcceptChanges();
                    }
                }

            }

        }

        public ICommand SpeicalGridClickCommandNo { get { return this.speicalGridClickCommandNo; } }
        public void GridDisable()
        {

            if (Model_Rpd.Opt_Special_No == true)
            {
                if (Model_Rpd.CI_REFERENCE == null || Model_Rpd.CI_REFERENCE.ToString().Trim() == "")
                {
                    GridVisible = Visibility.Collapsed;
                    return;
                }
                if (Model_Rpd.GridData.Table.Rows.Count > 0)
                {
                    if (Model_Rpd.GridData.Table.Rows.Count == 1)
                    {
                        if (Model_Rpd.GridData.Table.Rows[0]["CHARACTERISTIC"].ToString().Trim() == "" || Model_Rpd.GridData.Table.Rows[0]["CUSTOMER_EXP"].ToString().Trim() == "" || Convert.ToString(Model_Rpd.GridData.Table.Rows[0]["SEVERITY"].ToString()).Trim() == "")
                        {
                            GridVisible = Visibility.Collapsed;
                            return;
                        }
                    }
                    ShowInformationMessage(PDMsg.NormalString("Special Characteristics has to be Deleted before choosing No option."));
                    Model_Rpd.Opt_Special_Yes = true;
                    return;
                }
                else if (Model_Rpd.GridData.Table == null || Model_Rpd.GridData.Table.Rows.Count == 0)
                {
                    GridVisible = Visibility.Collapsed;
                }

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
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Warning);
            return MessageBoxResult.None;
        }



        public ICommand PrintCommand { get { return this.printCommand; } }
        private void PrintData()
        {
            try
            {
                if ((Model_Rpd.CI_REFERENCE.Trim() == "") && (Model_Rpd.IDPK == 0))
                {
                    ShowInformationMessage(PDMsg.ToPrint("CI Reference No"));
                    //MessageBox.Show("Please Enter CI Reference No to Print ", "smartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Progress.ProcessingText = PDMsg.Load;
                Progress.Start();
                //int i = 0;
                DataSet reportdata = new DataSet();
                DataTable dtSearchReport = new DataTable();
                DataTable dtGridData = new DataTable();
                dtSearchReport = rpdmodel.GetReportData(Model_Rpd);
                dtSearchReport.AcceptChanges();
                if (dtSearchReport.Rows.Count > 0)
                {
                    dtSearchReport.Columns.Add("CUST_NAME");
                    dtSearchReport.Rows[0]["CUST_NAME"] = Model_Rpd.CUST_NAME.ToString();
                    dtSearchReport.Columns.Add("PART_NO");

                    if (Model_Rpd.PART_NO != null)
                    {
                        dtSearchReport.Rows[0]["PART_NO"] = Model_Rpd.PART_NO.ToString();
                    }
                    else
                    {
                        dtSearchReport.Rows[0]["PART_NO"] = "";
                    }


                    dtSearchReport.AcceptChanges();

                    if (dtSearchReport.Rows[0]["AUTOPART"] == DBNull.Value || dtSearchReport.Rows[0]["AUTOPART"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["AUTOPART"] = "Y";
                    }
                    if (dtSearchReport.Rows[0]["SAFTYPART"] == DBNull.Value || dtSearchReport.Rows[0]["SAFTYPART"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["SAFTYPART"] = "Y";
                    }

                    if (dtSearchReport.Rows[0]["STATUS"] == DBNull.Value || dtSearchReport.Rows[0]["STATUS"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["STATUS"] = "0";
                    }
                    if (dtSearchReport.Rows[0]["DEVL_METHOD"] == DBNull.Value || dtSearchReport.Rows[0]["DEVL_METHOD"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["DEVL_METHOD"] = "0";
                    }
                    if (dtSearchReport.Rows[0]["PACKING"] == DBNull.Value || dtSearchReport.Rows[0]["PACKING"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PACKING"] = "1";
                    }

                    if (dtSearchReport.Rows[0]["PPAP_LEVEL"] == DBNull.Value || dtSearchReport.Rows[0]["PPAP_LEVEL"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PPAP_LEVEL"] = "1";
                    }

                    if (dtSearchReport.Rows[0]["PPAP_FORGING"] == DBNull.Value || dtSearchReport.Rows[0]["PPAP_FORGING"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PPAP_FORGING"] = "0";
                    }
                    if (dtSearchReport.Rows[0]["PPAP_SAMPLE"] == DBNull.Value || dtSearchReport.Rows[0]["PPAP_SAMPLE"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PPAP_SAMPLE"] = "0";
                    }

                    if (dtSearchReport.Rows[0]["PPAP_SAMPLE"] == DBNull.Value || dtSearchReport.Rows[0]["PPAP_SAMPLE"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PPAP_SAMPLE"] = "0";
                    }

                    if (dtSearchReport.Rows[0]["SPL_CHAR"] == DBNull.Value || dtSearchReport.Rows[0]["SPL_CHAR"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["SPL_CHAR"] = "1";
                    }

                    if (dtSearchReport.Rows[0]["PPAP_LEVEL"] == DBNull.Value || dtSearchReport.Rows[0]["PPAP_LEVEL"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PPAP_LEVEL"] = "1";
                    }

                    if (dtSearchReport.Rows[0]["PPAP_FORGING"] == DBNull.Value || dtSearchReport.Rows[0]["PPAP_FORGING"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PPAP_FORGING"] = "0";
                    }

                    if (dtSearchReport.Rows[0]["PPAP_SAMPLE"] == DBNull.Value || dtSearchReport.Rows[0]["PPAP_SAMPLE"].ToString() == "")
                    {
                        dtSearchReport.Rows[0]["PPAP_SAMPLE"] = "0";
                    }

                    if (dtSearchReport.Rows[0]["CUST_STD_DATE"] != DBNull.Value)
                    {
                        dtSearchReport.Rows[0]["CUST_STD_DATE"] = Model_Rpd.CUST_STD_DATE.ToFormattedDateAsString();
                    }

                    if (dtSearchReport.Rows[0]["MKTG_COMMITED_DT"] != DBNull.Value)
                    {
                        dtSearchReport.Rows[0]["MKTG_COMMITED_DT"] = Model_Rpd.MKTG_COMMITED_DT.ToFormattedDateAsString();
                    }
                    if (dtSearchReport.Rows[0]["ATP_DATE"] != DBNull.Value)
                    {
                        dtSearchReport.Rows[0]["ATP_DATE"] = Model_Rpd.ATP_DATE.ToFormattedDateAsString();
                    }
                    if (dtSearchReport.Rows[0]["CUSTOMER_NEED_DT"] != DBNull.Value)
                    {
                        dtSearchReport.Rows[0]["CUSTOMER_NEED_DT"] = Model_Rpd.CUSTOMER_NEED_DT.ToFormattedDateAsString();
                    }


                    dtSearchReport.AcceptChanges();

                    //  dtSearchReport.WriteXmlSchema("E:\\RPD.XML");
                    //  Model_Rpd.GridData.Table.WriteXmlSchema("E:\\RPD1.XML");
                    reportdata.Tables.Add(dtSearchReport);
                    dtGridData = Model_Rpd.GridData.Table.Copy();
                    dtGridData.AcceptChanges();
                    reportdata.Tables.Add(dtGridData);

                    //if (reportdata.Tables[0].Rows[0])

                    Progress.End();
                    try
                    {
                        ProcessDesigner.frmReportViewer frv = new ProcessDesigner.frmReportViewer(reportdata, "RPD");
                        if (!frv.ReadyToShowReport) return;
                        frv.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        throw ex.LogException();
                    }
                    //System.Windows.MessageBox.Show(Window owner, String messageBoxText, String caption, MessageBoxButton button, MessageBoxImage icon)

                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }


        //Model_Rpd.GridData

        private bool _printenable = false;
        public bool PrintEnable
        {
            get { return _printenable; }
            set
            {
                _printenable = value;
                NotifyPropertyChanged("PrintEnable");
            }
        }

        private bool _saveoperation = false;
        public bool SaveEnable
        {
            get { return _saveoperation; }
            set
            {
                _saveoperation = value;
                NotifyPropertyChanged("SaveEnable");
            }
        }
        public int CurrentStrRow = 0;
        public void GridDataAddRows(Object selecteditem)
        {
            DataRow dr;


            Model_Rpd.SelectedItem = (DataRowView)selecteditem;




            //Model_Rpd.SelectedItem.DataView.Table.Rows.i

            // DataRow DR=new DataRow();
            int currentrow;
            if (Model_Rpd.GridData.Table.Rows.Count == 0)
            {
                currentrow = 0;
            }
            else
            {
                currentrow = Convert.ToInt16(Model_Rpd.SelectedItem.Row["SLNO"].ToString());
            }

            isSave = true;

            // currentrow = Model_Rpd.GridData.Table.Rows.Count;

            if (currentrow == 0)
            {
                DataRow newrow = Model_Rpd.GridData.Table.NewRow();
                Model_Rpd.GridData.Table.Rows.InsertAt(newrow, 0);
                Model_Rpd.GridData.Table.Rows[0]["SLNO"] = 1;
                Model_Rpd.GridData.Table.AcceptChanges();
            }
            else
            {
                dr = Model_Rpd.GridData.Table.Rows[currentrow - 1];
                if (dr["SLNO"].ToString().Trim() != "" || dr["CHARACTERISTIC"].ToString().Trim() != "" || dr["SEVERITY"].ToString().Trim() != "")
                {
                    if (dr["CHARACTERISTIC"].ToString().Trim() != "")
                    {

                        bool retval = true;
                        retval = CheckGridDuplicate(currentrow);
                        if (retval == true)
                        {
                            int rowindex;
                            rowindex = Model_Rpd.GridData.Table.Rows.Count;

                            if (Model_Rpd.GridData.Table.Rows[rowindex - 1]["CHARACTERISTIC"].ToString().Trim() != "" || Model_Rpd.GridData.Table.Rows[rowindex - 1]["SEVERITY"].ToString().Trim() != "" || Model_Rpd.GridData.Table.Rows[rowindex - 1]["CUSTOMER_EXP"].ToString().Trim() != "")
                            {
                                DataRow newrow = Model_Rpd.GridData.Table.NewRow();
                                newrow["SLNO"] = Model_Rpd.GridData.Table.Rows.Count + 1;
                                Model_Rpd.GridData.Table.Rows.InsertAt(newrow, rowindex);
                            }
                        }
                    }
                    else
                    {
                        int rowindex;
                        rowindex = Model_Rpd.GridData.Table.Rows.Count;
                        if (rowindex > 0)
                            if (Model_Rpd.GridData.Table.Rows[rowindex - 1]["CHARACTERISTIC"].ToString().Trim() == "" || Model_Rpd.GridData.Table.Rows[rowindex - 1]["SEVERITY"].ToString().Trim() == "" || Model_Rpd.GridData.Table.Rows[rowindex - 1]["CUSTOMER_EXP"].ToString().Trim() == "")
                            {
                                //Model_Rpd.GridData.Table.Rows[rowindex].Delete();
                                //Model_Rpd.GridData.Table.AcceptChanges();
                                int j = 1;
                                for (int i = 0; i < Model_Rpd.GridData.Table.Rows.Count; i++)
                                {
                                    Model_Rpd.GridData.Table.Rows[i]["SLNO"] = j;
                                    j = j + 1;
                                }
                            }

                        isSave = false;
                        return;
                    }
                }
                else
                {
                    int j = 1;
                    for (int i = 0; i < Model_Rpd.GridData.Table.Rows.Count; i++)
                    {
                        Model_Rpd.GridData.Table.Rows[i]["SLNO"] = j;
                        j = j + 1;
                    }
                    Model_Rpd.GridData.Table.AcceptChanges();
                }
            }
            firstCell = true;

        }
        bool isSave = true;
        public bool CheckGridDuplicate(int rowindex)
        {
            try
            {
                string findstr = "";
                if (Model_Rpd.GridData.Table.Rows.Count > 0)
                {
                    findstr = Model_Rpd.GridData.Table.Rows[rowindex - 1]["CHARACTERISTIC"].ToString().Trim();

                    //CostDetails.ToTable().Columns.Contains(fieldName + "_SEQUENCE"))
                    //DataRow DR =new DataRow 
                    //DR=Model_Rpd.GridData.Table.Select("CHARACTERISTIC" =) 



                    //Model_Rpd.GridData.RowFilter = "CHARACTERISTIC='" + FindStr + "'";
                    //if (Model_Rpd.GridData.Count > 1)
                    //{
                    //    MessageBox.Show("Duplicate Entry");
                    //    Model_Rpd.GridData.RowFilter = null;
                    //    return false;
                    //}
                    //Model_Rpd.GridData.RowFilter = null;

                    for (int i = 0; i <= Model_Rpd.GridData.Table.Rows.Count - 1; i++)
                    {
                        if (i != rowindex - 1)
                        {
                            int c = string.Compare(findstr.ToUpper(), Model_Rpd.GridData.Table.Rows[i]["CHARACTERISTIC"].ToString().ToUpper());
                            if (c == 0)
                            {
                                MessageBox.Show("Duplicate Characteristics has been Entered ", "smartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                                // MessageBox.Show("Duplicate Entry");
                                Model_Rpd.GridData.Table.Rows[rowindex - 1]["CHARACTERISTIC"] = "";
                                Model_Rpd.GridData.Table.Rows[rowindex - 1]["SEVERITY"] = "";
                                Model_Rpd.GridData.Table.Rows[rowindex - 1]["CUSTOMER_EXP"] = "";
                                isSave = false;
                                return false;
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private RolePermission _actionPermission;
        public RolePermission ActionPermission
        {
            get { return _actionPermission; }
            set
            {
                _actionPermission = value;
                NotifyPropertyChanged("ActionPermission");
            }
        }

        // private int RowCount;
        private void RowEditEndingSubType(Object selecteditem)
        {
            try
            {


                GridDataAddRows(selecteditem);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private int _maxlen = 1;
        public int MaxLen
        {
            get
            {
                return _maxlen;
            }
            set
            {
                _maxlen = value;
                NotifyPropertyChanged("MaxLen");
            }

        }



        public RPDModel Model_Rpd
        {
            get
            {
                return _model_rpd;
            }
            set
            {
                _model_rpd = value;
                NotifyPropertyChanged("Model_Rpd");
            }
        }


        //private FocusManager.se  

        private bool _firstCell = true;
        public bool firstCell
        {
            get
            {
                return _firstCell;
            }
            set
            {
                _firstCell = value;
                NotifyPropertyChanged("firstCell");
            }
        }
        public void FillFormData()
        {
            try
            {

                rpdmodel.GetRPDData(Model_Rpd);

                if (Model_Rpd.Opt_Special == true)
                {
                    TxtPacking = Visibility.Visible;
                    LblPacking = Visibility.Visible;
                }
                else if (Model_Rpd.Opt_Stand == true)
                {
                    TxtPacking = Visibility.Collapsed;
                    LblPacking = Visibility.Collapsed;
                }

                if (Model_Rpd.Opt_Special_Yes == true)
                {
                    GridVisible = Visibility.Visible;
                }
                else
                {
                    GridVisible = Visibility.Collapsed;
                }

                Model_Rpd.GridData = rpdmodel.GetGridData(Model_Rpd);
                if (Model_Rpd.GridData.Table.Rows.Count == 0)
                {
                    DataRow newrow = Model_Rpd.GridData.Table.NewRow();
                    newrow["SLNO"] = 1;
                    Model_Rpd.GridData.Table.Rows.InsertAt(newrow, 0);
                }
                Model_Rpd.CUST_CODE = Convert.ToDecimal(Model_Rpd.CUST_CODE);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void GetCirNoFromPartNo(string partno)
        {
            try
            {
                Model_Rpd.CI_REFERENCE = rpdmodel.GetCirRefFromPartNo(partno);
                Model_Rpd.IDPK = rpdmodel.GetCirRefIDPK(Model_Rpd.CI_REFERENCE);
                if (String.IsNullOrEmpty(Convert.ToString(Model_Rpd.CI_REFERENCE)) || Model_Rpd.CI_REFERENCE.Length == 0)
                {
                    if (dispmessage == false)
                    {
                        MessageBox.Show("Customer Cost Sheet is not attached to this Part No", "smartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        dispmessage = true;
                        ClearFields();
                    }


                }
                else
                {
                    FillFormData();
                }


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void ClearFields()
        {
            Model_Rpd = null;
            Model_Rpd = new RPDModel();
            Model_Rpd.CI_REFERENCE = " ";
            Model_Rpd.PROD_DESC = "";
            Model_Rpd.CUST_NAME = "";
            Model_Rpd.CUST_DWG_NO_ISSUE = " ";
            Model_Rpd.POTENTIAL = "";
            Model_Rpd.MONTHLY = "";
            Model_Rpd.APPLICATION = "";
            Model_Rpd.CUST_STD_DATE = null;
            Model_Rpd.CUST_DWG_NO = "";
            Model_Rpd.CUST_STD_NO = "";
            Model_Rpd.EXPORT = "Domestic";
            Model_Rpd.PPAP_LEVEL = "1";
            Model_Rpd.AutoPart_Yes = true;
            Model_Rpd.Safety_Yes = true;
            Model_Rpd.Opt_Prototype = true;
            Model_Rpd.Opt_Special = true;
            Model_Rpd.Opt_Special_Yes = true;
            // Model_Rpd.Opt_Special_No = true;
            Model_Rpd.PPAP_FORGING = "";
            Model_Rpd.PPAP_SAMPLE = "";
            Model_Rpd.Opt_Devlp_Proto = true;
            Model_Rpd.NATURE_PACKING = "";
        }

        public ICommand LostFocusCirNo { get { return this.lostFocusCirNo; } }
        private void LostFocusCirNoDataRow()
        {
            if (Model_Rpd.CI_REFERENCE.ToString().Trim() == "")
                return;
            if (rpdmodel.CheckCIRefIsthere(Model_Rpd.CI_REFERENCE.ToString()) == false)
            {
                MessageBox.Show("Invalid CI Reference No", "smartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                Model_Rpd.CI_REFERENCE = "";
                return;
            }
        }

        public string VIEWCIRREF_NO;
        public ICommand SelectChangeCIRNo { get { return this.selectChangeCIRNo; } }
        private void SelectCIRDataRow()
        {
            try
            {
                if (SelectedRow != null)
                {
                    ClearFields();
                    Model_Rpd.CI_REFERENCE = (string)(SelectedRow["CI_REFERENCE"].ToString());
                    Model_Rpd.IDPK = Convert.ToInt32(SelectedRow["IDPK"]);
                    if (!String.IsNullOrEmpty(Convert.ToString(Model_Rpd.CI_REFERENCE)) || Model_Rpd.CI_REFERENCE.Length > 0)
                    {
                        FillFormData();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public string VIEWPARTNO;
        public int CIREF_NO;
        public ICommand SelectChangePartNo { get { return this.selectChangePartNo; } }
        private void SelectDataRow()
        {
            if (SelectedRowPart != null)
            {
                dispmessage = false;
                VIEWPARTNO = (string)(SelectedRowPart["PART_NO"].ToString());
                //CIREF_NO = (int)(SelectedRowPart["CIREF_NO_FK"]);
                GetCirNoFromPartNo(VIEWPARTNO);
            }

        }





        public ICommand RowEditEndingSubTypeCommand { get { return this.rowEditEndingSubTypeCommand; } }

        public ICommand SelectCustomerCode { get { return this.selectCustomerCode; } }
        private void SelectCustomerDataRow()
        {
            if (SelectedRowCustomer != null)
            {
                Model_Rpd.CUST_CODE = Convert.ToDecimal((SelectedRowCustomer["CUST_CODE"].ToString()));
                Model_Rpd.CUST_NAME = (SelectedRowCustomer["CUST_NAME"].ToString());

            }

        }



        private ObservableCollection<DropdownColumns> _partcustomercolumns;
        public ObservableCollection<DropdownColumns> PartCustomerColumns
        {
            get
            {
                return _partcustomercolumns;
            }
            set
            {
                _partcustomercolumns = value;
                NotifyPropertyChanged("PartCustomerColumns");
            }
        }

        private ObservableCollection<DropdownColumns> _columns;
        public ObservableCollection<DropdownColumns> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
                NotifyPropertyChanged("Columns");
            }
        }

        private ObservableCollection<DropdownColumns> _partcolumns;
        public ObservableCollection<DropdownColumns> PartColumns
        {
            get
            {
                return _partcolumns;
            }
            set
            {
                _partcolumns = value;
                NotifyPropertyChanged("PartColumns");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the CI Reference No")]
        private DataView _dtcireference;
        public DataView DtCIReference
        {
            get
            {
                return this._dtcireference;
            }
            set
            {
                this._dtcireference = value;
                NotifyPropertyChanged("DtCIReference");
            }
        }




        private DataView _dtcustomer;
        public DataView DtCustomer
        {
            get
            {
                return this._dtcustomer;
            }
            set
            {
                this._dtcustomer = value;
                NotifyPropertyChanged("DtCustomer");
            }
        }


        private DataView _dtpartno;
        public DataView DtPartNo
        {
            get
            {
                return this._dtpartno;
            }
            set
            {
                this._dtpartno = value;
                NotifyPropertyChanged("DtPartNo");
            }
        }

        private DataView _dtsflpartno;
        public DataView DtSFLPartNo
        {
            get
            {
                return this._dtsflpartno;
            }
            set
            {
                this._dtsflpartno = value;
                NotifyPropertyChanged("DtSFLPartNo");
            }
        }

        private DataRowView _selectedrowcustomer;
        public DataRowView SelectedRowCustomer
        {
            get
            {
                return _selectedrowcustomer;
            }

            set
            {
                _selectedrowcustomer = value;
                NotifyPropertyChanged("SelectedRowCustomer");
            }
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

        private DataRowView _selectedrow;
        public DataRowView SelectedRow
        {
            get
            {
                return _selectedrow;
            }

            set
            {
                _selectedrow = value;
            }
        }
        private void Cancel()
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

        public Action CloseAction { get; set; }

        private bool _changefocus = false;
        public bool ChangeFocus
        {
            get { return _changefocus; }
            set
            {
                _changefocus = value;
                NotifyPropertyChanged("ChangeFocus");
            }
        }


        public void TextBoxDateValidation_LostFocus(object sender, RoutedEventArgs e)
        {

            try
            {
                TextBox tb = (TextBox)sender;

                if (!string.IsNullOrEmpty(tb.Text.Trim()))
                {
                    if (UserControls.DateValidation.CheckIsValidDate(tb.Text.ToString().Trim()) == false)
                    {
                        MessageBox.Show("InValid Date", "smartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (tb.Tag.ToString() == "ATP_DATE")
                        {
                            this.Model_Rpd.ATP_DATE = null;
                            tb.Text = string.Empty;
                        }
                        else if (tb.Tag.ToString() == "CUST_STD_DATE")
                        {
                            this.Model_Rpd.CUST_STD_DATE = null;
                            tb.Text = string.Empty;
                            this.Focus_CUST_STD_DATE = true;
                        }
                        else if (tb.Tag.ToString() == "CUSTOMER_NEED_DT")
                        {
                            this.Model_Rpd.CUSTOMER_NEED_DT = null;
                            tb.Text = string.Empty;
                        }
                        else if (tb.Tag.ToString() == "MKTG_COMMITED_DT")
                        {
                            this.Model_Rpd.MKTG_COMMITED_DT = null;
                            tb.Text = string.Empty;
                        }
                        return;
                    }
                    else
                    {
                        //tb.Text = tb.Text.ToString().ToDateAsString("DD/MM/YYYY");
                        // tb.Text= DateTime.p  Text
                        tb.Text = tb.Text.ToString();
                        // tb.Text = "30.12.1988";
                        string str = tb.Text;
                        if (tb.Tag.ToString() == "ATP_DATE")
                        {
                            this.Model_Rpd.ATP_DATE = DateTime.Parse(tb.Text.ToString());
                        }
                        else if (tb.Tag.ToString() == "CUST_STD_DATE")
                        {

                            string pattern = "dd/MM/yyyy";
                            DateTime parsedDate;

                            DateTime date = DateTime.Parse(tb.Text);

                            if (DateTime.TryParseExact(date.ToShortDateString().ToString(), pattern, null,
                                                    DateTimeStyles.None, out parsedDate))
                            {
                                MessageBox.Show("Converted '{0}' to {1:d}.+," + parsedDate);
                                this.Model_Rpd.CUST_STD_DATE = parsedDate;
                                return;
                            }
                            else
                            {
                                this.Model_Rpd.CUST_STD_DATE = date;
                            }

                        }
                        else if (tb.Tag.ToString() == "CUSTOMER_NEED_DT")
                        {
                            this.Model_Rpd.CUSTOMER_NEED_DT = DateTime.Parse(tb.Text.ToString());
                        }
                        else if (tb.Tag.ToString() == "MKTG_COMMITED_DT")
                        {
                            this.Model_Rpd.CUSTOMER_NEED_DT = DateTime.Parse(tb.Text.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }


        }


        private bool _focus_ci_reference = false;
        public bool Focus_CI_REFERENCE
        {
            get { return _focus_ci_reference; }
            set
            {
                _focus_ci_reference = value;
                NotifyPropertyChanged("Focus_CI_REFERENCE");
            }
        }

        private bool _focus_cust_dwg_no = false;
        public bool Focus_CUST_DWG_NO
        {
            get { return _focus_cust_dwg_no; }
            set
            {
                _focus_cust_dwg_no = value;
                NotifyPropertyChanged("Focus_CUST_DWG_NO");
            }
        }

        private bool _focus_cust_std_no = false;
        public bool Focus_CUST_STD_NO
        {
            get { return _focus_cust_std_no; }
            set
            {
                _focus_cust_std_no = value;
                NotifyPropertyChanged("Focus_CUST_STD_NO");
            }
        }

        private bool _focus_cust_std_date = false;
        public bool Focus_CUST_STD_DATE
        {
            get { return _focus_cust_std_date; }
            set
            {
                _focus_cust_std_date = value;
                NotifyPropertyChanged("Focus_CUST_STD_DATE");
            }
        }

        private bool _focus_similar_part_no = false;
        public bool Focus_SIMILAR_PART_NO
        {
            get { return _focus_similar_part_no; }
            set
            {
                _focus_similar_part_no = value;
                NotifyPropertyChanged("Focus_SIMILAR_PART_NO");
            }
        }

        private bool _focus_cust_name = false;
        public bool Focus_CUST_NAME
        {
            get { return _focus_cust_name; }
            set
            {
                _focus_cust_name = value;
                NotifyPropertyChanged("Focus_CUST_NAME");
            }
        }

        private bool _focus_prod_desc = false;
        public bool Focus_PROD_DESC
        {
            get { return _focus_prod_desc; }
            set
            {
                _focus_prod_desc = value;
                NotifyPropertyChanged("Focus_PROD_DESC");
            }
        }

        private bool _focus_atp_date = false;
        public bool Focus_ATP_DATE
        {
            get { return _focus_atp_date; }
            set
            {
                _focus_atp_date = value;
                NotifyPropertyChanged("Focus_ATP_DATE");
            }
        }

        private bool _focus_ppap_sample = false;
        public bool Focus_PPAP_SAMPLE
        {
            get { return _focus_ppap_sample; }
            set
            {
                _focus_ppap_sample = value;
                NotifyPropertyChanged("Focus_PPAP_SAMPLE");
            }
        }

        private bool _focus_ppap_forging = false;
        public bool Focus_PPAP_FORGING
        {
            get { return _focus_ppap_forging; }
            set
            {
                _focus_ppap_forging = value;
                NotifyPropertyChanged("Focus_PPAP_FORGING");
            }
        }

        private bool _focus_ppap_level = false;
        public bool Focus_PPAP_LEVEL
        {
            get { return _focus_ppap_level; }
            set
            {
                _focus_ppap_level = value;
                NotifyPropertyChanged("Focus_PPAP_LEVEL");
            }
        }

        private bool _focus_mktg_commited_dt = false;
        public bool Focus_MKTG_COMMITED_DT
        {
            get { return _focus_mktg_commited_dt; }
            set
            {
                _focus_mktg_commited_dt = value;
                NotifyPropertyChanged("Focus_MKTG_COMMITED_DT");
            }
        }

        private bool _focus_customer_need_dt = false;
        public bool Focus_CUSTOMER_NEED_DT
        {
            get { return _focus_customer_need_dt; }
            set
            {
                _focus_customer_need_dt = value;
                NotifyPropertyChanged("Focus_CUSTOMER_NEED_DT");

            }
        }
        private bool _focus_potential = false;
        public bool Focus_POTENTIAL
        {
            get { return _focus_potential; }
            set
            {
                _focus_potential = value;
                NotifyPropertyChanged("Focus_POTENTIAL");
            }
        }

        private bool _focus_monthly = false;
        public bool Focus_MONTHLY
        {
            get { return _focus_monthly; }
            set
            {
                _focus_monthly = value;
                NotifyPropertyChanged("Focus_MONTHLY");
            }
        }
        private bool _focus_cust_dwg_no_issue = false;
        public bool Focus_CUST_DWG_NO_ISSUE
        {
            get { return _focus_cust_dwg_no_issue; }
            set
            {
                _focus_cust_dwg_no_issue = value;
                NotifyPropertyChanged("Focus_CUST_DWG_NO_ISSUE");
            }
        }

        private bool _focus_application = false;
        public bool Focus_APPLICATION
        {
            get { return _focus_application; }
            set
            {
                _focus_application = value;
                NotifyPropertyChanged("Focus_APPLICATION");
            }
        }

        private bool _focus_NATURE_PACKING = false;
        public bool Focus_NATURE_PACKING
        {
            get { return _focus_NATURE_PACKING; }
            set
            {
                _focus_NATURE_PACKING = value;
                NotifyPropertyChanged("Focus_NATURE_PACKING");
            }
        }




        private string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
        public ICommand UpdateCommand { get { return this.updateCommand; } }
        public void SaveRecord()
        {
            try
            {
                Focus_CUST_DWG_NO = true;




                if (String.IsNullOrEmpty(Model_Rpd.CI_REFERENCE) || Model_Rpd.CI_REFERENCE.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("CI Reference No"));
                    Focus_CI_REFERENCE = true;

                    return;
                }
                if (String.IsNullOrEmpty(Model_Rpd.CUST_DWG_NO) || Model_Rpd.CUST_DWG_NO.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Customer Part No"));
                    Focus_CUST_DWG_NO = true;
                    return;
                }
                if (String.IsNullOrEmpty(Model_Rpd.CUST_STD_NO) || Model_Rpd.CUST_STD_NO.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Customer DWG No"));
                    Focus_CUST_STD_NO = true;
                    return;
                }
                if (String.IsNullOrEmpty(Model_Rpd.PROD_DESC) || Model_Rpd.PROD_DESC.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part Description"));
                    Focus_PROD_DESC = true;
                    return;
                }

                if (String.IsNullOrEmpty(Model_Rpd.CUST_NAME) || Model_Rpd.CUST_NAME.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Customer Name"));
                    Focus_CUST_NAME = true;
                    return;
                }
                if (Model_Rpd.CUST_STD_DATE == null)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Issue Date"));
                    Focus_CUST_STD_DATE = true;
                    return;
                }


                if (Model_Rpd.ATP_DATE == null)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("ATP Date"));
                    Focus_ATP_DATE = true;
                    return;
                }

                if (String.IsNullOrEmpty(Model_Rpd.CUST_DWG_NO_ISSUE) || Model_Rpd.CUST_DWG_NO_ISSUE.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Issue No"));
                    Focus_CUST_DWG_NO_ISSUE = true;
                    return;
                }

                if (!Model_Rpd.POTENTIAL.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Annual Req"));
                    Focus_POTENTIAL = true;
                    return;
                }
                if (!Model_Rpd.MONTHLY.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Monthly Req"));
                    Focus_MONTHLY = true;
                    return;
                }
                if (String.IsNullOrEmpty(Model_Rpd.APPLICATION) || Model_Rpd.APPLICATION.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Application of Product"));
                    Focus_APPLICATION = true;
                    return;
                }
                if (Model_Rpd.CUSTOMER_NEED_DT == null)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Customer Needed Date"));
                    Focus_CUSTOMER_NEED_DT = true;
                    return;
                }

                if (Model_Rpd.MKTG_COMMITED_DT == null)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Marketing Committed Date"));
                    Focus_MKTG_COMMITED_DT = true;
                    return;
                }


                if (String.IsNullOrEmpty(Model_Rpd.PPAP_LEVEL) || Model_Rpd.PPAP_LEVEL.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("PPAP Level"));
                    Focus_PPAP_LEVEL = true;
                    return;
                }
                if (!Model_Rpd.PPAP_FORGING.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("PPAP Forging Quanity"));
                    Focus_PPAP_FORGING = true;
                    return;
                }
                if (!Model_Rpd.PPAP_SAMPLE.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("PPAP Sample Quantity"));
                    Focus_PPAP_SAMPLE = true;
                    return;
                }

                if (Model_Rpd.AutoPart_Yes == true)
                    Model_Rpd.AUTOPART = "Y";
                else if (Model_Rpd.AutoPart_No == true)
                    Model_Rpd.AUTOPART = "N";

                if (Model_Rpd.Safety_Yes == true)
                    Model_Rpd.SAFTYPART = "Y";
                else if (Model_Rpd.Safety_No == true)
                    Model_Rpd.SAFTYPART = "N";

                if (Model_Rpd.Opt_Prototype == true)
                    Model_Rpd.STATUS = 0;
                else if (Model_Rpd.Opt_PreLaunch == true)
                    Model_Rpd.STATUS = 1;
                else if (Model_Rpd.Opt_Production == true)
                    Model_Rpd.STATUS = 2;

                if (Model_Rpd.Opt_Devlp_Proto == true)
                    Model_Rpd.DEVL_METHOD = 0;
                else if (Model_Rpd.Opt_Devlp_Prelaunch == true)
                    Model_Rpd.DEVL_METHOD = 1;

                if (string.IsNullOrEmpty(Model_Rpd.EXPORT) || ((Model_Rpd.EXPORT) == "Domestic"))
                {
                    Model_Rpd.EXPORT = "0";
                }
                else if (Model_Rpd.EXPORT == "Export")
                {
                    Model_Rpd.EXPORT = "1";
                }
                else if (Model_Rpd.EXPORT == "Retail")
                {
                    Model_Rpd.EXPORT = "2";
                }
                else if (Model_Rpd.EXPORT == "Wef")
                {
                    Model_Rpd.EXPORT = "3";
                }
                else
                {
                    Model_Rpd.EXPORT = "0";
                }

                if (Model_Rpd.Opt_Special == true)
                    Model_Rpd.PACKING = 1;
                else if (Model_Rpd.Opt_Stand == true)
                    Model_Rpd.PACKING = 0;

                if (Model_Rpd.Opt_Special_Yes == true)
                    Model_Rpd.SPL_CHAR = 1;
                else if (Model_Rpd.Opt_Special_No == true)
                    Model_Rpd.SPL_CHAR = 0;

                if (Model_Rpd.Opt_Special == true)
                {
                    if (!Model_Rpd.NATURE_PACKING.IsNotNullOrEmpty())
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Nature of Packing"));
                        Focus_NATURE_PACKING = true;
                        return;
                    }
                }



                List<DDCI_CHAR> lstStandardNotes = null;

                lstStandardNotes = (from row in Model_Rpd.GridData.ToTable().AsEnumerable()
                                    select new DDCI_CHAR()
                                    {
                                        SLNO = Convert.ToDecimal(row.Field<string>("SLNO")),
                                        CHARACTERISTIC = row.Field<string>("CHARACTERISTIC"),
                                        CUSTOMER_EXP = row.Field<string>("CUSTOMER_EXP"),
                                        ROWID = Guid.NewGuid(),
                                    }).ToList<DDCI_CHAR>();

                if (!lstStandardNotes.IsNotNullOrEmpty() || lstStandardNotes.Count == 0) return;

                var lstRecordCount = (from row in lstStandardNotes
                                      group row by row.CHARACTERISTIC into grpStandardNotes
                                      where grpStandardNotes.Count() > 1
                                      select new { Key = grpStandardNotes.Key, Count = grpStandardNotes.Count() }).ToList<object>();

                if (lstRecordCount.IsNotNullOrEmpty())
                {
                    foreach (var item in lstRecordCount)
                    {
                        ShowInformationMessage("Duplicate Characteristics has been Entered ");
                        isSave = false;
                        return;
                    }
                }

                if (isSave == false)
                {
                    isSave = true;
                    return;
                }
                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();

                if (rpdmodel.SaveRPDData(Model_Rpd) == true)
                {
                    Progress.End();
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    ClearFields();
                }
                else
                {
                    Progress.End();
                }

            }
            catch (Exception ex)
            {
                log.LogWrite(ex.ToString());
                throw ex.LogException();
            }
        }

    }
}
