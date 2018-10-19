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
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class PointMachineViewModel : BindableBase
    {
        private PointMachineModel _pointMachine;
        private PointMachineBll _pointMachBll;
        private readonly ICommand unLoadCommand;
        public ICommand UnLoadCommand { get { return this.unLoadCommand; } }

        public string ApplicationTitle = "SmartPD";

        private readonly ICommand submitCommand;
        public ICommand SubmitCommand { get { return this.submitCommand; } }

        private readonly ICommand cancelCommand;
        public ICommand CancelCommand { get { return this.cancelCommand; } }
        private bool isSaved = false;
        private bool _closed = false;
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
                //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                //Progress.Start();
                
                _pointMachBll.UpdatePointMachine(PointMachine);
                //Progress.End();
                ShowInformationMessage(PDMsg.SavedSuccessfully);
                //MessageBox.Show("Records saved successfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                isSaved = true;
                if (_closed == false)
                {
                    _closed = true;
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void onUnloaded()
        {
            _closed = true;
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Point Machine?", "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    OnSubmit();
                }
            }
        }

        private void OnCancel()
        {
            try
            {
                MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);
                _closed = false;
                if (msgResult == MessageBoxResult.Yes)
                {
                    OnSubmit();
                }
                else if (msgResult == MessageBoxResult.No)
                {
                    _closed = true;
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!_closed)
                {
                    MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);
                    _closed = true;
                    if (msgResult == MessageBoxResult.Yes)
                    {
                        OnSubmit();
                    }
                    else if (msgResult == MessageBoxResult.Cancel)
                    {
                        _closed = false;
                        e.Cancel = true;
                    }
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

        private MessageBoxResult ShowConfirmMessageYesNoCancel(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
    }
}
