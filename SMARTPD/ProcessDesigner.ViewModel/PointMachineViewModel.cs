using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
    class PointMachineViewModel : BindableBase
    {
        private PointMachineModel _pointMachine;
        private PointMachineBll _pointMachBll;
        private readonly ICommand unLoadCommand;
        public ICommand UnLoadCommand { get { return this.unLoadCommand; } }

        private readonly ICommand submitCommand;
        public ICommand SubmitCommand { get { return this.submitCommand; } }

        private readonly ICommand cancelCommand;
        public ICommand CancelCommand { get { return this.cancelCommand; } }
        private bool isSaved = false;
        public Action CloseAction { get; set; }

        public PointMachineViewModel(UserInformation userInformation, string costCentCode)
        {
            this.PointMachine = new PointMachineModel();
            PointMachine.COST_CENT_CODE = costCentCode;
            _pointMachBll = new PointMachineBll(userInformation);
            _pointMachBll.GetPointMachine(PointMachine);
            
            this.submitCommand = new DelegateCommand(this.OnSubmit);
            this.cancelCommand = new DelegateCommand(this.OnCancel);
            this.unLoadCommand = new DelegateCommand(this.onUnloaded);
        }

        public PointMachineModel PointMachine
        {
            get
            {
                return _pointMachine;
            }
            set
            {
                _pointMachine = value;
            }
        }

        private void OnSubmit()
        {
            try
            {
                _pointMachBll.UpdatePointMachine(PointMachine);
                isSaved = true;
                CloseAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void onUnloaded()
        {
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Point Machine?", "Process Designer", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    OnSubmit();
                }
            }
        }

        private void OnCancel()
        {
            CloseAction();
        }


    }
}
