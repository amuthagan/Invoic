using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using System.Windows;
using System.Windows.Input;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;
namespace ProcessDesigner.ViewModel
{
    public class GrinmachineViewModel : ViewModelBase
    {
        private GrinmachineModel _grinMachine;
        public Action CloseAction { get; set; }
        private ICommand _onUnLoadCommand;
        private ICommand _onSubmitCommand;
        private ICommand _onCancelCommand;
        private bool isSaved = false;
        private bool _closed = false;
        private GrinmachineBll _grinMachineBll;

        public GrinmachineViewModel(UserInformation userInformation, string costCentCode)
        {
            this.GrinMachine = new GrinmachineModel();
            GrinMachine.COST_CENT_CODE = costCentCode;
            _grinMachineBll = new GrinmachineBll(userInformation);
            _grinMachineBll.GetGrinMachine(GrinMachine);
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
                        Submit();
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
        private MessageBoxResult ShowConfirmMessageYesNoCancel(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            return MessageBoxResult.None;
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
            _closed = true;
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Grinding Machine?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                //Progress.Start();
                _grinMachineBll.UpdateGrinMachine(GrinMachine);
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

        private void Cancel()
        {
            try
            {

                MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);
                _closed = false;
                if (msgResult == MessageBoxResult.Yes)
                {
                    Submit();
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
    }
}
