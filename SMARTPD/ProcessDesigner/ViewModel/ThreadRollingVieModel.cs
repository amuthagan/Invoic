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
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class ThreadRollingVieModel : BindableBase
    {
        private ThreadRollingBll _threadRollingBll;
        private ThreadRollingModel _threadRolling;
        private readonly ICommand _onSubmitCommand;
        private readonly ICommand _onCacelCommand;
        private readonly ICommand _onUnLoadCommand;
        private bool _closed = false;
        public string ApplicationTitle = "SmartPD";

        private bool isSaved = false;

        public Action CloseAction { get; set; }

        public ThreadRollingVieModel(UserInformation userInformation, string costCentCode)
        {
            this.ThreadRolling = new ThreadRollingModel();
            ThreadRolling.COST_CENT_CODE = costCentCode;
            _threadRollingBll = new ThreadRollingBll(userInformation);
            _threadRollingBll.GetThreadRollingMachine(ThreadRolling);
            this._onSubmitCommand = new DelegateCommand(this.Submit);
            this._onCacelCommand = new DelegateCommand(this.Cancel);
            this._onUnLoadCommand = new DelegateCommand(this.onUnloaded);
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

        public ICommand OnSubmitCommand { get { return this._onSubmitCommand; } }
        private void Submit()
        {
            try
            {
                //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                //Progress.Start();

                _threadRollingBll.UpdateThreadRollingMachine(ThreadRolling);
                //Progress.End();
                isSaved = true;
                if (isSaved == true)
                {
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    //MessageBox.Show("Records saved successfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
        public ICommand OnCacelCommand { get { return this._onCacelCommand; } }

        public ThreadRollingModel ThreadRolling
        {
            get
            {
                return _threadRolling;
            }
            set
            {
                _threadRolling = value;
                OnPropertyChanged("ThreadRolling");
            }
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
        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        public ICommand OnUnLoadCommad { get { return this._onUnLoadCommand; } }
        private void onUnloaded()
        {
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Flat Thread Rolling Machine?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                _closed = true;
                if (result == MessageBoxResult.Yes)
                {
                    Submit();

                    // MessageBox.Show("Records saved successfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }
    }
}
