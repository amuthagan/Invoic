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
//using System.Data.Entity;
//using System.Collections.ObjectModel;


namespace ProcessDesigner.ViewModel
{
    class OptimalCondtionViewModel : ViewModelBase 
    {
        private BLL.OptimalConditionBll optimalModel;
        private UserInformation userinformation;
        private readonly ICommand updateOperMastCommand;
        private readonly ICommand enterClickCommand;
      //  private Model.DDOPTIMAL_COND modelOptimal;
        private readonly ICommand alertCommand;
        private readonly ICommand rowEditEndingSubTypeCommand;
        public DataGrid AutoScroll;

        public OptimalCondtionViewModel(string cost_centercode)
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            CostCenterCode = cost_centercode;
            //   CostCenterCode = Cost_CenterCode;
            optimalModel = new OptimalConditionBll(userinformation);
         //   modelOptimal = new DDOPTIMAL_COND();
            this.updateOperMastCommand = new DelegateCommand(this.CommonFormValUpdtae);
            this.alertCommand = new DelegateCommand(this.KeyPressalertCommand);
            this.enterClickCommand = new DelegateCommand(this.EnterKeyPress);
            
             this.rowEditEndingSubTypeCommand = new DelegateCommand<Object>(this.RowEditEndingSubType);

            GridData = optimalModel.GetOptimalMaster(CostCenterCode);
            SelectedIndex = GridData.Rows.Count - 1;

            if (GridData == null || GridData.Rows.Count == 0)
            {
                DataRow newrow = GridData.NewRow();
                GridData.Rows.InsertAt(newrow, 0);
                GridData.Rows[0]["SER_NO"] = 1;
                GridData.AcceptChanges();
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

        public string _PART_REP_ADJ;
        public string PART_REP_ADJ
        {
            get { return this._PART_REP_ADJ; }
            set { this._PART_REP_ADJ = value; }
        
        }

        public bool _IsFirst = false;
        public bool IsFirst
        {
            get { return _IsFirst; }
            set { _IsFirst = value; }
        }
        public ICommand AlertCommand { get { return this.alertCommand; } }
        private void KeyPressalertCommand()
        {
            //DataRow DrRow = new DataRow();
            //DataRow//

         //   GridData.NewRow();

         //   DataRow row1 = GridData.NewRow();
         //   row1["SER_NO"] = GridData.Rows.Count + 2;
         //  // if (IsFirst == true)
         //  // {
         //       row1["PART_REP_ADJ"] = PART_REP_ADJ;

         //       GridData.Rows.Add(row1);
         //       GridData = GridData.Copy();
         //       GridData.AcceptChanges();
         ////   }

       

         //   int RowNum;
         //   SER_NO = GridData.Rows.Count.ToString();
         //   RowNum=Convert.ToInt32(SER_NO);
         //   RowNum = RowNum + 2;
         //   SER_NO = Convert.ToString(RowNum);

         //   //int RowNum;
         //   //SER_NO = GridData.Rows.Count.ToString();
         //   //RowNum=Convert.ToInt32(SER_NO);
         //   //RowNum = RowNum + 1;
         //   //SER_NO = Convert.ToString(RowNum);
         //   MessageBox.Show("sdfsdfsdfsdsdf");
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

        private int _selectedindex;
        public int SelectedIndex
        {
            get { return this._selectedindex; }
            set { this._selectedindex = value; NotifyPropertyChanged("SelectedIndex"); }
        }

        

     
        private string _ser_no;
        public string SER_NO
        {
            get
            {
                return this._ser_no;
            }
            set
            {
                this._ser_no = value;
                NotifyPropertyChanged("SER_NO");
            }
        }


        public ICommand EnterClickCommand { get { return this.enterClickCommand; } }
        public void EnterKeyPress()
        {
            try
            {
                SER_NO = GridData.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        //public event EventHandler RowChangedEvent;
        public void Execute(object parameter)
        {
            MessageBox.Show("Text Changed");
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private string _errMessage;
        public ICommand UpdateOperMastCommand { get { return this.updateOperMastCommand; } }
        public void CommonFormValUpdtae()
        {
            try
            {
                ChangeFocus = true;
                DataTable gridsaveddata = new DataTable();
                gridsaveddata = GridData.Copy();
                gridsaveddata.AcceptChanges();
                bool val = optimalModel.AddNewOptimalCondition(GridData, CostCenterCode, ref _errMessage);
                if (val)
                {
                    ShowInformationMessage(_errMessage);
                    //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    // ClearOperMaster();
                }
                if (val == false)
                {
                    if ((string)_errMessage != "")
                        ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }

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
        private DataTable _griddataview;
        public DataTable GridData
        {
            get
            {
                return this._griddataview;
            }
            set
            {
                this._griddataview = value;
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
                if (dr["PART_REP_ADJ"].ToString().Trim() != "" || dr["AREA"].ToString().Trim() != "" || dr["OPTIMAL_COND"].ToString().Trim() != "" || dr["RESP"].ToString().Trim() != "" || dr["FREQUENCY"].ToString().Trim() != "" || dr["NORMAL"].ToString().Trim() != "" || dr["COUNTER_MEAS"].ToString().Trim() != "")
                {
                    rowindex = GridData.Rows.Count;
                    DataRow newrow = GridData.NewRow();
                    bool retval = true;
                    if (retval == true)
                    {
                        newrow["SER_NO"] = GridData.Rows.Count + 1;
                        GridData.Rows.InsertAt(newrow, rowindex);
                    }
                }
            }
        }


        //private ICommand dgSelectionChanged;
        ///// <summary>
        ///// when the datagrid Selection Changes
        ///// </summary>f
        //public ICommand DgSelectionChanged
        //{
        //    get
        //{
        //    return dgSelectionChanged ??
        //    (dgSelectionChanged = new RelayCommand<DataGrid>(dg1 =>
        //    {
        //      // whatever you want when the Selection Changes
        //        SelectedIndex= d
        //        //select the item 
        //        if (dg1.SelectedIndex > -1)
        //        {
        //            dg1.Focus();
        //            dg1.CurrentCell = new DataGridCellInfo(dg1.Items[dg1.SelectedIndex], dg1.Columns[0]);
        //        }
        //    }));
        //}
        //}
    }
}
