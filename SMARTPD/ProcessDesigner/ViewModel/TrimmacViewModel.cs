using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class TrimmacViewModel : ValidatableBindableBase
    {
        private DDTRIMMING_MAC _ddtrimmingmac;
        private readonly ICommand cancelCommand;
        private readonly ICommand saveCommand;
        private readonly ICommand unloadCommand;
        private bool isSaved = false;
        private UserInformation userinformation;
        private Trimmachine trimmac;
        private string costCenterCode;
        private bool _closed = false;
        public Action CloseAction { get; set; }
        [ImportingConstructor]
        public TrimmacViewModel(UserInformation user, string costCentCode)
        {
            // UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            costCenterCode = costCentCode;
            trimmac = new Trimmachine(userinformation);

            DDTRIMMING_MAC ddtap = trimmac.getTrimmachines(costCenterCode);
            if (ddtap == null)
            {
                ddtrimmingmac = new DDTRIMMING_MAC();
                ddtrimmingmac.COST_CENT_CODE = costCenterCode;
            }
            else
            {
                ddtrimmingmac = new DDTRIMMING_MAC();
                ddtrimmingmac.COST_CENT_CODE = ddtap.COST_CENT_CODE;
                ddtrimmingmac.FEED_TYPE = ddtap.FEED_TYPE;
                ddtrimmingmac.MAX_AF = ddtap.MAX_AF;
                ddtrimmingmac.MAX_PROD_DIA = ddtap.MAX_PROD_DIA;
                ddtrimmingmac.MAX_SHANK_LEN = ddtap.MAX_SHANK_LEN;
                ddtrimmingmac.MOTOR_POWER = ddtap.MOTOR_POWER;
                ddtrimmingmac.TRIM_DIE_LEN = ddtap.TRIM_DIE_LEN;
                ddtrimmingmac.TRIM_DIE_OD = ddtap.TRIM_DIE_OD;
                ddtrimmingmac.TRIM_PUNCH_LEN = ddtap.TRIM_PUNCH_LEN;
                ddtrimmingmac.TRIM_PUNCH_OD = ddtap.TRIM_PUNCH_OD;

            }
            this.cancelCommand = new DelegateCommand(this.CancelTrimmac);
            this.saveCommand = new DelegateCommand(this.SaveTrimmac);
            this.unloadCommand = new DelegateCommand(this.UnloadTrimmac);
        }
        public DDTRIMMING_MAC ddtrimmingmac
        {
            get
            {
                return this._ddtrimmingmac;
            }
            set
            {
                SetProperty(ref this._ddtrimmingmac, value);
            }
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
                        SaveTrimmac();
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
        private List<string> _errors;

        public List<string> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        public ICommand CancelTapmacCommand { get { return this.cancelCommand; } }
        public ICommand SaveTapmacCommand { get { return this.saveCommand; } }
        public ICommand UnLoadCommad { get { return this.unloadCommand; } }
        private void CancelTrimmac()
        {
            MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);
            _closed = false;
            if (msgResult == MessageBoxResult.Yes)
            {
                SaveTrimmac();
            }
            else if (msgResult == MessageBoxResult.No)
            {
                _closed = true;
                CloseAction();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void UnloadTrimmac()
        {
            _closed = true;
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Trimming Machine?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    SaveTrimmac();
                }
                else
                {
                    if (_closed == false)
                        CloseAction();
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

        private void SaveTrimmac()
        {
            try
            {

                //Save it somewhere
                //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                //Progress.Start();

                isSaved = trimmac.saveTrimmachine(ddtrimmingmac);
                //Progress.End();
                if (isSaved)
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

        private MessageBoxResult ShowConfirmMessageYesNoCancel(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            return MessageBoxResult.None;
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
