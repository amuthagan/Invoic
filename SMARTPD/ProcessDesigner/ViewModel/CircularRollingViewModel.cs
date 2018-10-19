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
using System.Text.RegularExpressions;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class CircularRollingViewModel : ValidatableBindableBase
    {
        private DDCIR_THREAD_ROLL_MAC _ddcircularroll;
        private readonly ICommand cancelCommand;
        private readonly ICommand saveCommand;
        private readonly ICommand unloadCommand;
        private readonly ICommand textBoxValueChanged;
        private bool isSaved = false;
        private bool _closed = false;
        private UserInformation userinformation;
        private CircularRolling circularroll;
        private string costCenterCode;
        public Action CloseAction { get; set; }
        [ImportingConstructor]
        public CircularRollingViewModel(UserInformation user, string costCentCode)
        {
            // UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            costCenterCode = costCentCode;

            circularroll = new CircularRolling(userinformation);

            DDCIR_THREAD_ROLL_MAC ddcirroll = circularroll.getCircularRollings(costCenterCode);
            if (ddcirroll == null)
            {
                ddcircularroll = new DDCIR_THREAD_ROLL_MAC();
                ddcircularroll.COST_CENT_CODE = costCenterCode;
            }
            else
            {
                ddcircularroll = new DDCIR_THREAD_ROLL_MAC();
                ddcircularroll.COST_CENT_CODE = ddcirroll.COST_CENT_CODE;
                ddcircularroll.BACKLASH_ELIMINATOR = ddcirroll.BACKLASH_ELIMINATOR;
                ddcircularroll.DIE_MOVEMENT_BOTH = ddcirroll.DIE_MOVEMENT_BOTH;
                ddcircularroll.MAX_PROD_DIA = ddcirroll.MAX_PROD_DIA;
                ddcircularroll.MAX_PITCH = ddcirroll.MAX_PITCH;
                ddcircularroll.MOTOR_POWER = ddcirroll.MOTOR_POWER;
                ddcircularroll.MAX_ROLL_DIA = ddcirroll.MAX_ROLL_DIA;
                ddcircularroll.MAX_TILT_ANGLE = ddcirroll.MAX_TILT_ANGLE;
                ddcircularroll.MIN_PITCH = ddcirroll.MIN_PITCH;
                ddcircularroll.MIN_PROD_DIA = ddcirroll.MIN_PROD_DIA;
                ddcircularroll.OUTBOARD_BEARINGS = ddcirroll.OUTBOARD_BEARINGS;
                ddcircularroll.ROLL_PRESSURE = ddcirroll.ROLL_PRESSURE;
                ddcircularroll.THROUGH_FEED = ddcirroll.THROUGH_FEED;
                ddcircularroll.TYPE_WORKREST_BLADE = ddcirroll.TYPE_WORKREST_BLADE;

            }
            this.cancelCommand = new DelegateCommand(this.CancelCircularRolling);
            this.saveCommand = new DelegateCommand(this.SaveCircularRolling);
            this.unloadCommand = new DelegateCommand(this.UnloadCircularRolling);
            this.textBoxValueChanged = new DelegateCommand(this.TextChanged);
        }
        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {

                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                {
                    closingev.Cancel = true;
                    e = closingev;
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

        // ddcircularroll.COST_CENT_CODE =costCenterCode;

        public ICommand TextBoxValueChanged { get { return this.textBoxValueChanged; } }
        private void TextChanged()
        {
            //ddcircularroll.MIN_PROD_DIA

            Regex.IsMatch(Convert.ToString(ddcircularroll.MIN_PROD_DIA), @"[\d]{1,4}([.][\d]{1,2})?");

            //if (SelectedRowPart != null)
            //{
            //    DispMessage = false;
            //    viewPartNo = (string)(SelectedRowPart["PART_NO"].ToString());
            //    //CIREF_NO = (int)(SelectedRowPart["CIREF_NO_FK"]);
            //    GetCirNoFromPartNo(viewPartNo);
            //}

        }

        public DDCIR_THREAD_ROLL_MAC ddcircularroll
        {
            get
            {
                return this._ddcircularroll;
            }
            set
            {
                SetProperty(ref this._ddcircularroll, value);
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
        private void CancelCircularRolling()
        {
            try
            {

                MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);
                _closed = false;
                if (msgResult == MessageBoxResult.Yes)
                {                    
                    SaveCircularRolling();
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void UnloadCircularRolling()
        {
            _closed = true;
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Circular Rolling Machine?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    SaveCircularRolling();
                }
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
                        SaveCircularRolling();
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
        private void SaveCircularRolling()
        {
            try
            {
                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();
                //Save it somewhere
                isSaved = circularroll.saveCircularRolling(ddcircularroll);
                Progress.End();
                if (isSaved == true)
                {
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    //  _closed = true;
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
