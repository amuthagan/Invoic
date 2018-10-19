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

namespace ProcessDesigner.ViewModel
{
    class NylocmacViewModel : ViewModelBase 
    {
        private NylocmacModel _nylocmachine;
        private ICommand _onUnLoadCommand;
        private ICommand _onSubmitCommand;
        private ICommand _onCancelCommand;
        private NylocmachineBll _nylocmachineBll;
        private bool isSaved = false;
        public Action CloseAction { get; set; }

        public NylocmacViewModel(UserInformation userInformation, string costCentCode)
        {
            this.Nylocmachine = new NylocmacModel();
            Nylocmachine.COST_CENT_CODE = costCentCode;
            _nylocmachineBll = new NylocmachineBll(userInformation);
            _nylocmachineBll.GetNylocmachine(Nylocmachine);
        }


        public NylocmacModel Nylocmachine
        {
            get
            {
                return _nylocmachine;
            }
            set
            {
                _nylocmachine = value;
                NotifyPropertyChanged("Nylocmachine");
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
                MessageBoxResult result = MessageBox.Show("Do you want to save Nynoc Machine?", "Process Designer", MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                _nylocmachineBll.UpdateNylocmachine(Nylocmachine);
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
