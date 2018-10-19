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
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class NylocmacViewModel : ViewModelBase
    {
        private NylocmacModel _nylocmachine;
        private ICommand _onUnLoadCommand;
        private ICommand _onSubmitCommand;
        private ICommand _onCancelCommand;
        private NylocmachineBll _nylocmachineBll;
        private bool isSaved = false;
        private bool _closed = false;
        public Action CloseAction { get; set; }

        public NylocmacViewModel(UserInformation userInformation, string costCentCode)
        {
            this.Nylocmachine = new NylocmacModel();
            Nylocmachine.COST_CENT_CODE = costCentCode;
            _nylocmachineBll = new NylocmachineBll(userInformation);
            _nylocmachineBll.GetNylocmachine(Nylocmachine);
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
            _closed = true;
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Nyloc Machine?", "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Information);

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
                _nylocmachineBll.UpdateNylocmachine(Nylocmachine);
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void Cancel()
        {
            try
            {
                //if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                //{
                //    CloseAction();
                //}
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

                //if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                //{
                //    CloseAction();
                //}
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
    }
}
