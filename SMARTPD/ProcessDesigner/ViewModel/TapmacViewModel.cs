using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;

namespace ProcessDesigner
{
    [Export]
    public class TapmacViewModel : ViewModelBase
    {
        private DDTAPPING_MAC _ddtappingmac;
        private readonly ICommand cancelTapmacCommand;
        private readonly ICommand saveTapmacCommand;
        private readonly ICommand unloadCommand;
        private bool isSaved = false;
        private bool _closed = false;
        private UserInformation userinformation;
        private Tapmachine tapmac;
        private string costCenterCode;
        public Action CloseAction { get; set; }
        [ImportingConstructor]
        public TapmacViewModel(UserInformation user, string costCentCode)
        {
            // UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            costCenterCode = costCentCode;
            tapmac = new Tapmachine(userinformation);

            DDTAPPING_MAC ddtap = tapmac.getTapmachines(costCenterCode);
            if (ddtap == null)
            {
                ddtappingmac = new DDTAPPING_MAC();
                ddtappingmac.COST_CENT_CODE = costCenterCode;
                ddtappingmac.MAX_TAP_SIZE = 0;
                ddtappingmac.MIN_TAP_SIZE = 0;
                ddtappingmac.MOTOR_POWER = 0;
                ddtappingmac.NO_OF_SPINDLES = 0;
                ddtappingmac.PUSH_STORKE_SHAFT_SPEED = 0;
            }
            else
            {
                ddtappingmac = new DDTAPPING_MAC();
                ddtappingmac.COST_CENT_CODE = ddtap.COST_CENT_CODE;
                ddtappingmac.MAX_TAP_SIZE = ddtap.MAX_TAP_SIZE;
                ddtappingmac.MIN_TAP_SIZE = ddtap.MIN_TAP_SIZE;
                ddtappingmac.MOTOR_POWER = ddtap.MOTOR_POWER;
                ddtappingmac.NO_OF_SPINDLES = ddtap.NO_OF_SPINDLES;
                ddtappingmac.PUSH_STORKE_SHAFT_SPEED = ddtap.PUSH_STORKE_SHAFT_SPEED;
            }
            this.cancelTapmacCommand = new DelegateCommand(this.CancelTapmac);
            this.saveTapmacCommand = new DelegateCommand(this.SaveTapmac);
            this.unloadCommand = new DelegateCommand(this.UnloadTapmac);
        }
        //public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    try
        //    {

        //        if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
        //        {
        //            e.Cancel = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}
        public DDTAPPING_MAC ddtappingmac
        {
            get
            {
                return this._ddtappingmac;
            }
            set
            {
                this._ddtappingmac = value;
                NotifyPropertyChanged("ddtappingmac");
                //OnPropertyChanged(() => ddtappingmac);
                //OnPropertyChanged(() => ddtappingmac.MAX_TAP_SIZE);
                //OnPropertyChanged(() => ddtappingmac.MIN_TAP_SIZE);
                //OnPropertyChanged(() => ddtappingmac.MOTOR_POWER);
                //OnPropertyChanged(() => ddtappingmac.NO_OF_SPINDLES);
                //OnPropertyChanged(() => ddtappingmac.PUSH_STORKE_SHAFT_SPEED);

            }
        }
        private List<string> _errors;

        public List<string> Errors
        {
            get { return _errors; }
            set
            {
                this._errors = value;
                NotifyPropertyChanged("Errors");
            }
        }
        public ICommand CancelTapmacCommand { get { return this.cancelTapmacCommand; } }
        public ICommand SaveTapmacCommand { get { return this.saveTapmacCommand; } }
        public ICommand UnLoadCommad { get { return this.unloadCommand; } }
        private void CancelTapmac()
        {
            try
            {
                MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);
                _closed = false;
                if (msgResult == MessageBoxResult.Yes)
                {
                    SaveTapmac();
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
                        SaveTapmac();
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void UnloadTapmac()
        {
            _closed = true;
            if (isSaved == false)
            {

                MessageBoxResult result = MessageBox.Show("Do you want to save Tapping Machine?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    SaveTapmac();
                }
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

        private void SaveTapmac()
        {
            try
            {
                if (ddtappingmac.IsValid)
                {
                    //Save it somewhere
                    //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    //Progress.Start();

                    tapmac.saveTapmachine(ddtappingmac);
                    //Progress.End();
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    //MessageBox.Show("Tapmac Saved Successfully", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    isSaved = true;
                    if (_closed == false)
                    {
                        _closed = true;
                        CloseAction();
                    }
                }

                //ValidateProperties();
                //Errors = FlattenErrors();
                //if (!HasErrors)
                //{
                //    // Save it somewhere
                //    //tapmac.saveTapmachine(ddtappingmac);
                //    CloseAction();
                //}


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }
        private List<string> FlattenErrors()
        {
            List<string> errors = new List<string>();
            Dictionary<string, List<string>> allErrors = GetAllErrors();
            foreach (string propertyName in allErrors.Keys)
            {
                foreach (var errorString in allErrors[propertyName])
                {
                    errors.Add(propertyName + ": " + errorString);
                }
            }
            return errors;
        }
    }
}
