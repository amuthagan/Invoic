using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Windows.Input;
using ProcessDesigner.BLL;
using System.Windows;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;


namespace ProcessDesigner.ViewModel
{
    class FeatureMasterViewModel : BindableBase
    {
        private FeatureMasterModel _featureMaster;
        private FeatureMasterBll _featureMasterbll;     

        private readonly ICommand _onAddCommand;
        public ICommand OnAddCommand { get { return this._onAddCommand; } }

        private readonly ICommand _onEditViewCommand;
        public ICommand OnEditViewCommand { get { return this._onEditViewCommand; } }

        private readonly ICommand _onSaveCommand;
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }

        private readonly ICommand _onCloseCommand;
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
      

        private bool isSaved = false;
        public Action CloseAction { get; set; }

        public FeatureMasterViewModel()
        {
            this.FeatureMaster = new FeatureMasterModel();
            _featureMasterbll = new FeatureMasterBll();
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            this.selectChangeComboCommandOper = new DelegateCommand(this.SelectDataRowOper);
            this._onAddCommand = new DelegateCommand(this.Add);
            this._onEditViewCommand = new DelegateCommand(this.Edit);
            this._onSaveCommand = new DelegateCommand(this.Save);
            this._onCloseCommand = new DelegateCommand(this.Close);

            ButtonEnable = Visibility.Collapsed;
            ButtonVisibleOper = Visibility.Visible;
            FeatureMaster.FeatureCode = _featureMasterbll.GenerateFeatuerCode();
            FeatureMaster.OperationCodeDetails = _featureMasterbll.GetOpertionMaster();
            AddEnable = false;
            EditEnable = true;            
        }

        public FeatureMasterModel FeatureMaster
        {
            get
            {
                return _featureMaster;
            }
            set
            {
                SetProperty(ref _featureMaster, value);               
            }
        }

        private bool _editOpertion = true;
        private bool _addOperation = false;
        public bool AddEnable
        {
            get { return _addOperation; }
            set
            {                
                SetProperty(ref _addOperation, value);     
            }
        }
        public bool EditEnable
        {
            get { return _editOpertion; }
            set
            {
                SetProperty(ref _editOpertion, value); 
            }
        }

        private readonly ICommand selectChangeComboCommand;
        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            FeatureMaster.Feature = FeatureMaster.SelectedRowFeat["FEATURE_DESC"].ToString();
            FeatureMaster.FeatureCode = FeatureMaster.SelectedRowFeat["FEATURE_CODE"].ToString();
        }
        private readonly ICommand selectChangeComboCommandOper;
        public  ICommand SelectChangeComboCommandOper { get { return this.selectChangeComboCommandOper; } }
        private void SelectDataRowOper()
        {
            FeatureMaster.Operations = FeatureMaster.SelectedRow["OPER_DESC"].ToString();
            FeatureMaster.OperationCode = Convert.ToInt16(FeatureMaster.SelectedRow["OPER_CODE"].ToString());
        }
        private Visibility _buttonVisible = Visibility.Collapsed;
        public Visibility ButtonEnable
        {
            get { return _buttonVisible; }
            set
            {                
                SetProperty(ref _buttonVisible, value); 
            }
        }
        private Visibility _buttonVisibleOper = Visibility.Collapsed;
        public Visibility ButtonVisibleOper
        {
            get { return _buttonVisibleOper; }
            set
            {
                              SetProperty(ref _buttonVisibleOper, value); 
            }
        }
        private void Add()
        {
            try
            {
               
                //FeatureMaster.Feature = string.Empty;
                //FeatureMaster.FeatureCode = string.Empty;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Edit()
        {
            try
            {
                //       _nylocmachineBll.UpdateNylocmachine(Nylocmachine);
                ButtonEnable = Visibility.Visible;
                ButtonVisibleOper = Visibility.Visible;
                FeatureMaster.FeatureCodeDetails = _featureMasterbll.GetFeatureMaster(FeatureMaster.OperationCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Save()
        {
            try
            {
                //       _nylocmachineBll.UpdateNylocmachine(Nylocmachine);
                isSaved = true;
                CloseAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Close()
        {
            CloseAction();
        }

    }
}
