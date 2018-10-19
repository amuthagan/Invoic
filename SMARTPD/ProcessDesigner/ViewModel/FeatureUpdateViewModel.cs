using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.Common;
using ProcessDesigner.BLL;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using ProcessDesigner.Model;
using System.Data;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;

namespace ProcessDesigner.ViewModel
{
    [Export]
    public class FeatureUpdateViewModel : ViewModelBase
    {

        private string _newFeature;
        private string _existingFeatureUpdate;

        private readonly ICommand clearFeatureUpdateCommand;
        private readonly ICommand updateFeatureUpdateCommand;
        private readonly ICommand selectChangeComboCommand;

        private UserInformation userinformation;
        private BLL.FeatureUpdateBll featureUpdate;
        private DataView dvFeature = null;

        public List<PCCS> FeatureUpdateList { get; private set; }


        [ImportingConstructor]
        public FeatureUpdateViewModel(UserInformation userInformation)
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            featureUpdate = new FeatureUpdateBll(userinformation);
            this.DVFeature = featureUpdate.GetFeatureUpdateMaster();
            LoadFormData();
            this.clearFeatureUpdateCommand = new DelegateCommand(this.ClearFeature);
            this.updateFeatureUpdateCommand = new DelegateCommand(this.UpdateFeature);
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            ClearFeature();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Existing Feature Update is required")]
        public string ExistingFeatureUpdate
        {
            get
            {
                return this._existingFeatureUpdate;
            }
            set
            {
                _existingFeatureUpdate = value;
                NotifyPropertyChanged("ExistingFeatureUpdate");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "New Feature is required")]
        public string NewFeature
        {
            get
            {
                return this._newFeature;
            }
            set
            {
                _newFeature = value;
                NotifyPropertyChanged("NewFeature");
            }
        }

        public ICommand ClearFeatureUpdateCommand { get { return this.clearFeatureUpdateCommand; } }
        private void ClearFeature()
        {
            ExistingFeatureUpdate = "";
            NewFeature = "";

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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
        public ICommand UpdateFeatureUpdateCommand { get { return this.updateFeatureUpdateCommand; } }
        private void UpdateFeature()
        {
            if (String.IsNullOrEmpty(ExistingFeatureUpdate))
            {
                Message = PDMsg.NotEmpty("Feature Name");
            }
            else if (String.IsNullOrEmpty(NewFeature))
            {
                Message = PDMsg.NotEmpty("New Feature Name");
            }
            else
            {
                if (featureUpdate.FeatureAddDuplicate(NewFeature))
                {

                    bool val = featureUpdate.updateFeatureUpdateMaster(ExistingFeatureUpdate, NewFeature);
                    if (val)
                    {
                        Message = PDMsg.UpdatedSuccessfully;
                        ClearFeature();
                        this.DVFeature = featureUpdate.GetFeatureUpdateMaster();
                        LoadFormData();
                    }

                }
                else
                {
                    ShowInformationMessage(PDMsg.AlreadyExists("Feature Name"));
                }
            }
        }
        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            //EX_NO
            ExistingFeatureUpdate = SelectedRow["Feature"].ToString();
            //ShowInCaset = SelectedRow["SHOW_IN_COST"].ToString();
        }

        public void LoadFormData()
        {
            LabelCode = "Feature* :";
            ComboBoxMaxLength = 10;
            DescTextboxMaxLength = 50;
            Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "FEATURE", ColumnDesc = "Existing Feature Name", ColumnWidth = "1*" }
                        };

        }

        private string _labelCode;
        public String LabelCode
        {
            get { return _labelCode; }
            set
            {
                _labelCode = value;
                OnPropertyChanged("LabelCode");
            }

        }

        private string _showInCost;
        public string ShowInCaset
        {
            get
            {
                return this._showInCost;
            }
            set
            {
                //SetProperty(ref this._showInCost, value);
                _showInCost = value;
                NotifyPropertyChanged("ShowInCaset");
            }
        }

        private int _combomaxlength;
        public int ComboBoxMaxLength
        {
            get
            {
                return _combomaxlength;
            }
            set
            {
                _combomaxlength = value;
                OnPropertyChanged("ComboBoxMaxLength");
            }
        }

        private int _descTextboxMaxLength;
        public int DescTextboxMaxLength
        {
            get
            {
                return _descTextboxMaxLength;
            }
            set
            {
                _descTextboxMaxLength = value;
                NotifyPropertyChanged("DescTextboxMaxLength");
            }
        }

        private ObservableCollection<DropdownColumns> _columns;
        public ObservableCollection<DropdownColumns> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
                NotifyPropertyChanged("Columns");
            }
        }

        private DataRowView _selectedrow;
        public DataRowView SelectedRow
        {
            get
            {
                return _selectedrow;
            }

            set
            {
                _selectedrow = value;
            }
        }

        public DataView DVFeature
        {
            get
            {
                return this.dvFeature;
            }
            set
            {
                this.dvFeature = value;
                NotifyPropertyChanged("DVFeature");
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

        private string _message = string.Empty;
        private string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                ShowInformationMessage(_message);
                //MessageBox.Show(_message, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
