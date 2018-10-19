using ProcessDesigner.Common;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using System.Data;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;

namespace ProcessDesigner.ViewModel
{
    public class ForgingMachineViewModel : ViewModelBase
    {
        private ForgingMachine bll;

        private System.Windows.Controls.ListBox lstMachineType;

        private int _selectedIndexType;
        public int SelectedIndexType
        {
            get
            {
                return _selectedIndexType;
            }
            set
            {
                _selectedIndexType = value;
            }
        }

        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {

                if (!isSaved && !isCancelSaved)
                {
                    MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);
                    isSaved = true;
                    if (msgResult == MessageBoxResult.Yes)
                    {
                        Submit(true);
                    }
                    else if (msgResult == MessageBoxResult.Cancel)
                    {
                        isSaved = false;
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
        public ForgingMachineViewModel(UserInformation userInformation, string entityPrimaryKey)
        {
            bll = new ForgingMachine(userInformation);
            ActiveEntity = bll.GetEntityByCode(new DDFORGING_MAC() { COST_CENT_CODE = entityPrimaryKey });
            if (!ActiveEntity.IsNotNullOrEmpty())
            {
                ActiveEntity = new DDFORGING_MAC() { COST_CENT_CODE = entityPrimaryKey };
            }

            MachineTypes = bll.GetForgingMachineTypesByID();
            if (ActiveEntity.MACHINE_TYPE.ToValueAsString().Trim() != "")
            {
                SelectedRow = new ForgingMachineTypes();
                SelectedRow.DESCRIPTION = ActiveEntity.MACHINE_TYPE;
                for (int ictr = 0; ictr <= MachineTypes.Count - 1; ictr++)
                {
                    if (ActiveEntity.MACHINE_TYPE.Trim() == MachineTypes[ictr].DESCRIPTION.Trim())
                    {
                        SelectedIndexType = ictr;
                        NotifyPropertyChanged("SelectedIndexType");
                    }
                }
            }
            else
            {
                SelectedIndexType = -1;
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

        private ICommand _onSubmitCommand;
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

        private bool isSaved = false;
        private bool isCancelSaved = false;
        private void Submit(bool isUnload = false)
        {
            try
            {
                if (SelectedRow.IsNotNullOrEmpty())
                    ActiveEntity.MACHINE_TYPE = SelectedRow.DESCRIPTION;
                //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                //Progress.Start();
                isSaved = bll.Update<DDFORGING_MAC>(new List<DDFORGING_MAC>() { ActiveEntity });
                //Progress.End();
                ShowInformationMessage(PDMsg.SavedSuccessfully);
                //MessageBox.Show("Records saved successfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                if (!isUnload) CloseAction();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public Action CloseAction { get; set; }
        private void Cancel()
        {
            try
            {
                MessageBoxResult msgResult = ShowConfirmMessageYesNoCancel(PDMsg.BeforeCloseForm);

                if (msgResult == MessageBoxResult.Yes)
                {
                    Submit();
                }
                else if (msgResult == MessageBoxResult.No)
                {
                    isCancelSaved = true;
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

        private void onUnloaded()
        {
            if (isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save Forging Machine?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    Submit(true);
                }
            }
        }

        private ICommand _onCancelCommand;
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

        private ICommand _onUnLoadCommand;
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

        private DDFORGING_MAC _activeEntity = null;
        public DDFORGING_MAC ActiveEntity
        {
            get
            {
                return _activeEntity;
            }
            set
            {
                _activeEntity = value;
                NotifyPropertyChanged("ActiveEntity");
            }
        }

        private ObservableCollection<ForgingMachineTypes> _machineTypes = null;
        public ObservableCollection<ForgingMachineTypes> MachineTypes
        {
            get
            {
                return _machineTypes;
            }
            set
            {
                _machineTypes = value;
                NotifyPropertyChanged("MachineTypes");
            }
        }

        private ForgingMachineTypes _selectedRow;
        public ForgingMachineTypes SelectedRow
        {
            get
            {
                return _selectedRow;
            }

            set
            {
                _selectedRow = value;
            }
        }

        public void LoadMethod(object sender, EventArgs e)
        {
            try
            {
                lstMachineType = ((System.Windows.Controls.ListBox)((System.Windows.Window)sender).FindName("lstRoles"));
                lstMachineType.ScrollIntoView(lstMachineType.SelectedItem);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
