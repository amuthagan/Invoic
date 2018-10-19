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
using ProcessDesigner.Model;
using System.Windows.Controls;
using System.Globalization;

namespace ProcessDesigner.ViewModel
{
    public class StatndardConditionViewModel : ViewModelBase
    {
        private BLL.StandardConditionBll stdcondition;
        //private UserControls.DateValidation isvalid;

        private UserInformation userinformation;
        private readonly ICommand updateOperMastCommand;
        //private readonly ICommand enterClickCommand;
        private readonly ICommand rowEditEndingSubTypeCommand;

        //private readonly ICommand textBoxValueChanged;
        //private readonly ICommand textBoxValueChanged1;
        //private readonly ICommand textBoxValueChanged2;
        //  private Model.DDOPTIMAL_COND modelOptimal;
        public DataGrid AutoScroll;

        public StatndardConditionViewModel(string cost_centercode, string category_id)
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            CostCenterCode = cost_centercode;
            CategoryId = category_id;
            if (category_id == "1") Standard = "Cleaning";
            if (category_id == "2") Standard = "Inspection";
            if (category_id == "3") Standard = "Lubrication";


            stdcondition = new StandardConditionBll(userinformation);
            this.updateOperMastCommand = new DelegateCommand(this.CommonFormValUpdtae);
            this.rowEditEndingSubTypeCommand = new DelegateCommand<Object>(this.RowEditEndingSubType);

            // this.textBoxValueChanged = new DelegateCommand<Object>(this.TextChanged);
            // this.textBoxValueChanged1 = new DelegateCommand<Object>(this.TextChanged1);
            // this.textBoxValueChanged2 = new DelegateCommand<Object>(this.TextChanged2);


            GridMainData = stdcondition.GetStandard_Main(CostCenterCode, category_id);
            GridData = stdcondition.GetStandSubMaster(CostCenterCode, category_id);

            string sysUIFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            if (GridData == null || GridData.Rows.Count == 0)
            {
                DataRow newrow = GridData.NewRow();
                GridData.Rows.InsertAt(newrow, 0);
                GridData.Rows[0]["SER_NO"] = 1;
                GridData.AcceptChanges();
            }

            if (GridMainData.Rows.Count > 0)
            {
                CATEGORY = GridMainData.Rows[0]["CATEGORY"].ToString();
                if (GridMainData.Rows[0]["DATE_MADE"] != DBNull.Value)
                {
                    DateTime d;
                    string dstr = Convert.ToString(GridMainData.Rows[0]["DATE_MADE"]);
                    DateTime.TryParse(dstr, out d);
                    if (d != null)
                    {
                        DATE_MADE = d.Date;
                    }
                }
                if (GridMainData.Rows[0]["VALID_UPTO"] != DBNull.Value)
                {
                    DateTime d;
                    string dstr = Convert.ToString(GridMainData.Rows[0]["VALID_UPTO"]);
                    DateTime.TryParse(dstr, out d);
                    if (d != null)
                    {
                        VALID_UPTO = d.Date;
                    }
                }

                if (GridMainData.Rows[0]["REVISION_DATE"] != DBNull.Value)
                {
                    DateTime d;
                    string dstr = Convert.ToString(GridMainData.Rows[0]["REVISION_DATE"]);
                    DateTime.TryParse(dstr, out d);
                    if (d != null)
                    {
                        REVISION_DATE = d.Date;
                    }
                }
                REVISION_NO = GridMainData.Rows[0]["REVISION_NO"].ToString();
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

        //public ICommand TextBoxValueChanged2 { get { return this.textBoxValueChanged2; } }
        //private void TextChanged2(Object DataValue)
        //{
        //    if (UserControls.DateValidation.CheckIsValidDate(Convert.ToString(DataValue.ToString())) == false)
        //    {
        //        MessageBox.Show("InValid Date", "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
        //        return;
        //    }

        //}

        //public ICommand TextBoxValueChanged1 { get { return this.textBoxValueChanged1; } }
        //private void TextChanged1(Object DataValue)
        //{
        //    if (UserControls.DateValidation.CheckIsValidDate(Convert.ToString(DataValue.ToString())) == false)
        //    {
        //        MessageBox.Show("InValid Date", "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
        //        return;
        //    }

        //}

        //public ICommand TextBoxValueChanged { get { return this.textBoxValueChanged; } }
        //private void TextChanged(Object DataValue)
        //{
        //    if (UserControls.DateValidation.CheckIsValidDate(Convert.ToString(DataValue.ToString())) == false)
        //    {
        //        MessageBox.Show("InValid Date", "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
        //        return;
        //    }

        //}


        public void TextBoxDateValidation_LostFocus(object sender, RoutedEventArgs e)
        {

            try
            {
                TextBox tb = (TextBox)sender;

                //if (tb.Text.Trim() !=null || tb.Text.Trim() != string.Empty || tb.Text.Trim() =="" || tb.Text.Trim().Length >0)
                if (!string.IsNullOrEmpty(tb.Text.Trim()))
                {
                    if (UserControls.DateValidation.CheckIsValidDate(tb.Text.ToString().Trim()) == false)
                    {
                        MessageBox.Show("Invalid Date", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                        if (tb.Tag.ToString() == "VALID_UPTO")
                        {

                            this.VALID_UPTO = null;
                            tb.Text = string.Empty;
                        }
                        else if (tb.Tag.ToString() == "DATE_MADE")
                        {
                            this.DATE_MADE = null;
                            tb.Text = string.Empty;
                        }
                        else if (tb.Tag.ToString() == "REVISION_DATE")
                        {
                            this.REVISION_DATE = null;
                            tb.Text = string.Empty;
                        }
                        return;
                    }
                    else
                    {
                        tb.Text = tb.Text.ToString().ToDateAsString("DD/MM/YYYY");
                        if (tb.Tag.ToString() == "VALID_UPTO")
                        {
                            this.VALID_UPTO = DateTime.Parse(tb.Text.ToString());
                        }
                        else if (tb.Tag.ToString() == "DATE_MADE")
                        {
                            this.DATE_MADE = DateTime.Parse(tb.Text.ToString());
                        }
                        else if (tb.Tag.ToString() == "REVISION_DATE")
                        {
                            this.REVISION_DATE = DateTime.Parse(tb.Text.ToString());
                        }
                    }
                }
                //else if (tb.Text.Trim() == string.Empty)
                //{
                //    if (tb.Tag.ToString() == "VALID_UPTO")
                //    {
                //        this.VALID_UPTO = null;
                //    }
                //    else if (tb.Tag.ToString() == "DATE_MADE")
                //    {
                //        this.DATE_MADE = null;
                //    }
                //    else if (tb.Tag.ToString() == "REVISION_DATE")
                //    {
                //        this.REVISION_DATE = null;
                //    }
                //}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }


        }




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


        private string _errMessage;
        public ICommand UpdateOperMastCommand { get { return this.updateOperMastCommand; } }
        public void CommonFormValUpdtae()
        {
            try
            {
                ChangeFocus = true;
                DataTable savegriddata = new DataTable();
                savegriddata = GridData.Copy();
                savegriddata.AcceptChanges();
                bool val = stdcondition.AddNewDDStandMain(DATE_MADE, VALID_UPTO, REVISION_DATE, REVISION_NO, CostCenterCode, CategoryId, ref _errMessage);
                if (val == false)
                {
                    //  if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                val = stdcondition.AddNewOptimalCondition(GridData, CostCenterCode, CategoryId, ref _errMessage);
                if (val)
                {
                    ShowInformationMessage(_errMessage);
                    //MessageBox.Show(_errMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    // ClearOperMaster();
                }
                if (val == false)
                {
                    if ((string)_errMessage != "")
                        ShowInformationMessage(_errMessage);
                    //MessageBox.Show(_errMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public Action CloseAction { get; set; }
        private RelayCommand _onCancelCommand;
        public ICommand OnCloseCommand
        {
            get
            {
                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.Cancel(), null);
                }
                return _onCancelCommand;
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

        private string _categoryid;
        public string CategoryId
        {
            get { return this._categoryid; }
            set
            {
                this._categoryid = value;
                NotifyPropertyChanged("CategoryId");
            }
        }

        private string _category;
        public string CATEGORY
        {
            get { return this._category; }
            set
            {
                this._category = value;
                NotifyPropertyChanged("CATEGORY");
            }
        }
        private string _standard;
        public string Standard
        {
            get { return this._standard; }
            set
            {
                this._standard = value;
                NotifyPropertyChanged("Standard");
            }
        }

        private string _revision_no;
        public string REVISION_NO
        {
            get { return this._revision_no; }
            set
            {
                this._revision_no = value;
                NotifyPropertyChanged("REVISION_NO");
            }
        }

        private DateTime? _valid_upto;
        public DateTime? VALID_UPTO
        {
            get { return this._valid_upto; }
            set
            {
                this._valid_upto = value;
                NotifyPropertyChanged("VALID_UPTO");
            }
        }
        private DateTime? _revision_date;
        public DateTime? REVISION_DATE
        {
            get { return this._revision_date; }
            set
            {
                this._revision_date = value;
                NotifyPropertyChanged("REVISION_DATE");
            }
        }

        private DateTime? _date_made;
        public DateTime? DATE_MADE
        {
            get { return this._date_made; }
            set
            {
                this._date_made = value;
                NotifyPropertyChanged("DATE_MADE");
            }
        }

        private string _costCenterCode = string.Empty;
        public string CostCenterCode
        {
            get
            {
                return this._costCenterCode;
            }
            set
            {
                this._costCenterCode = value;
                NotifyPropertyChanged("CostCenterCode");
            }
        }

        private DataTable _gridmaindata;
        public DataTable GridMainData
        {
            get
            {
                return this._gridmaindata;
            }
            set
            {
                this._gridmaindata = value;
                NotifyPropertyChanged("GridMainData");
            }
        }

        private DataTable _griddata;
        public DataTable GridData
        {
            get
            {
                return this._griddata;
            }
            set
            {
                this._griddata = value;
                NotifyPropertyChanged("GridData");
            }
        }

        public ICommand RowEditEndingSubTypeCommand { get { return this.rowEditEndingSubTypeCommand; } }
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

        public DataRowView SelectedItem { get; set; }

        public int CurrentStrRow = 0;
        public void GridDataAddRows(Object selecteditem)
        {
            DataRow dr;


            selecteditem = (DataRowView)selecteditem;


            // DataRow DR=new DataRow();
            int currentrow;
            currentrow = GridData.Rows.Count;


            int rowindex;
            if (currentrow == 0)
            {
                DataRow newrow = GridData.NewRow();
                GridData.Rows.InsertAt(newrow, 0);
                GridData.Rows[0]["SER_NO"] = 1;
                GridData.AcceptChanges();
            }
            else
            {
                dr = GridData.Rows[currentrow - 1];
                if (dr["ILLUSTRATION"].ToString().Trim() != "" || dr["AREA"].ToString().Trim() != "" || dr["REQD_COND"].ToString().Trim() != "" || dr["METHOD"].ToString().Trim() != "" || dr["RESP"].ToString().Trim() != "" || dr["FREQUENCY"].ToString().Trim() != "" || dr["TIME_ALLOWED"].ToString().Trim() != "")
                {
                    // if (DR["CHARACTERISTIC"].ToString().Trim() != "")
                    //{
                    rowindex = GridData.Rows.Count;
                    DataRow newrow = GridData.NewRow();
                    bool retval = true;
                    // retval = CheckGridDuplicate(RowIndex);
                    if (retval == true)
                    {
                        newrow["SER_NO"] = GridData.Rows.Count + 1;
                        GridData.Rows.InsertAt(newrow, rowindex);
                    }
                    //}
                }
            }
            //   firstCell = true;

        }
    }
}
