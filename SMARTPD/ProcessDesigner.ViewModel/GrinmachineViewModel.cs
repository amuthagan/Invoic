using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using System.Windows;
using System.Windows.Input;
using ProcessDesigner.Model;

namespace ProcessDesigner.ViewModel
{
    class GrinmachineViewModel : ViewModelBase
    {
        private GrinmachineModel _grinMachine;
        public Action CloseAction { get; set; }
        private ICommand _onUnLoadCommand;
        private ICommand _onSubmitCommand;
        private ICommand _onCancelCommand;
        private bool isSaved = false;
        private GrinmachineBll _grinMachineBll;

        public GrinmachineViewModel(UserInformation userInformation, string costCentCode)
        {
            this.GrinMachine = new GrinmachineModel();
            GrinMachine.COST_CENT_CODE = costCentCode;
            _grinMachineBll = new GrinmachineBll(userInformation);
            _grinMachineBll.GetGrinMachine(GrinMachine);
        }

        public GrinmachineModel GrinMachine
        {
            get
            {
                return _grinMachine;
            }
            set
            {
                _grinMachine = value;
                NotifyPropertyChanged("GrinMachine");
            }
        }

        public ICommand OnUnLoadCommad
        {
            get
            {
                if (_onUnLoadCommand == null)
                {
                    _onUnLoadCommand = new RelayCommand(param => this.onUnloaded(), null);
                }
                return _onUnLoadCommand;
            }
        }

        public ICommand OnSubmitCommand
        {
            get
            {
                if (_onSubmitCommand == null)
                {
                    _onSubmitCommand = new RelayCommand(param => this.Submit(), null);
                }
                return _onSubmitCommand;
            }
        }

        public ICommand OnCacelCommand
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

        private void onUnloaded()
        {
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Grin Machine?", "Process Designer", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    Submit();
                }
            }
        }


        private void Submit()
        {
            try
            {
                _grinMachineBll.UpdateGrinMachine(GrinMachine);
                isSaved = true;
                CloseAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cancel()
        {
            CloseAction();
        }
    }
}
