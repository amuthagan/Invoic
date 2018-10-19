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
    public class CopyTurningMacViewModel : ViewModelBase
    {
        private CopyTurningMacModel _copyTurningModle;
        private CopyTurningMacBll _copyTurningBll;
        private readonly ICommand _onSubmitCommand;
        private readonly ICommand _onCacelCommand;
        private readonly ICommand _onUnLoadCommand;
        private bool isSaved = false;
        private bool _closed = false;
        public Action CloseAction { get; set; }

        public CopyTurningMacViewModel(UserInformation userInformation, string costCentCode)
        {
            this.CopyTurnMac = new CopyTurningMacModel();
            CopyTurnMac.COST_CENT_CODE = costCentCode;
            _copyTurningBll = new CopyTurningMacBll(userInformation);
            _copyTurningBll.GetCopyTurningMac(CopyTurnMac);
            this._onSubmitCommand = new DelegateCommand(this.Submit);
            this._onCacelCommand = new DelegateCommand(this.Cancel);
            this._onUnLoadCommand = new DelegateCommand(this.onUnloaded);

        }
        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
               
                //if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                //{
                //    e.Cancel = true;
                //}
               
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

        public CopyTurningMacModel CopyTurnMac
        {
            get
            {
                return _copyTurningModle;
            }
            set
            {
                _copyTurningModle = value;
                NotifyPropertyChanged("CopyTurnMac");
            }
        }

        public ICommand OnSubmitCommand { get { return this._onSubmitCommand; } }
        private void Submit()
        {
            try
            {
                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();
                _copyTurningBll.UpdateCopyTurningMac(CopyTurnMac);
                Progress.End();
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

        public ICommand OnUnLoadCommad { get { return this._onUnLoadCommand; } }
        private void onUnloaded()
        {
            _closed = true;
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Copy Turning Machine?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    Submit();
                    //   MessageBox.Show("Records saved successfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }
    }
}
