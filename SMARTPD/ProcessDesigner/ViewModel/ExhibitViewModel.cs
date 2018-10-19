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

namespace ProcessDesigner
{
    [Export]
    public class ExhibitViewModel : ViewModelBase
    {
        private string _docName;
        private string _exhibitNumber;
        private string _exhibitDetails;
        private string _showInCost;
        private readonly ICommand clearExhibitCommand;
        private readonly ICommand updateExhibitCommand;
        private readonly ICommand selectChangeComboCommand;

        private UserInformation userinformation;
        private BLL.ExhitbitMaster exhit;

        [ImportingConstructor]
        public ExhibitViewModel(UserInformation userInformation)
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            exhit = new ExhitbitMaster(userinformation);
            // this.ExhitbitDocs = exhit.getExhibitDocuments();
            this.DtDataview = exhit.GetExhibitMaster();

            LoadFormData();
            this.clearExhibitCommand = new DelegateCommand(this.ClearExhibit);
            this.updateExhibitCommand = new DelegateCommand(this.UpdateExhibit);
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            ExhibitDetails = "";
            ExhibitNumber = "";
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Exhibit Details is required")]
        public string ExhibitDetails
        {
            get
            {
                return this._exhibitDetails;
            }
            set
            {
                _exhibitDetails = value;
                NotifyPropertyChanged("ExhibitDetails");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Exhibit Number is required")]
        public string ExhibitNumber
        {
            get
            {
                return this._exhibitNumber;
            }
            set
            {
                _exhibitNumber = value;
                NotifyPropertyChanged("ExhibitNumber");
            }
        }

        public List<EXHIBIT_DOC> ExhitbitDocs { get; private set; }
        public ICommand ClearExhibitCommand { get { return this.clearExhibitCommand; } }
        public ICommand UpdateExhibitCommand { get { return this.updateExhibitCommand; } }

        public DataTable ExhitbitMaster
        {
            get;
            private set;
        }
        public DataView DtDataview
        {
            get
            {
                return this.ExhitbitMaster.DefaultView;
            }
            set
            {
                this.ExhitbitMaster = value.ToTable();
                NotifyPropertyChanged("DtDataview");
            }
        }

        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            //EX_NO
            ExhibitNumber = SelectedRow["EX_NO"].ToString();
            DocName = SelectedRow["DOC_NAME"].ToString();
            //ShowInCaset = SelectedRow["SHOW_IN_COST"].ToString();
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
        public void LoadFormData()
        {
            LabelCode = "Document Name* :";
            LabelDesc = "Exhibit Number* :";
            ComboBoxMaxLength = 10;
            DescTextboxMaxLength = 50;
            Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "DOC_NAME", ColumnDesc = "Document Name", ColumnWidth = 140 },
                            new DropdownColumns() { ColumnName = "EX_NO", ColumnDesc = "Exhibit Number", ColumnWidth = "1*" }
                        };

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

        private void ClearExhibit()
        {
            ExhibitDetails = "";
            ExhibitNumber = "";
        }

        private string _labelCode;
        public String LabelCode
        {
            get { return _labelCode; }
            set
            {
                _labelCode = value;
                NotifyPropertyChanged("LabelCode");
            }

        }

        private string _labelDesc;
        public String LabelDesc
        {
            get { return _labelDesc; }
            set
            {
                _labelDesc = value;
                NotifyPropertyChanged("LabelDesc");
            }

        }

        public string DocName
        {
            get
            {
                return this._docName;
            }
            set
            {
                _docName = value;
                NotifyPropertyChanged("DocName");
            }
        }

        public string ShowInCaset
        {
            get
            {
                return this._showInCost;
            }
            set
            {
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
                NotifyPropertyChanged("ComboBoxMaxLength");
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

        private void UpdateExhibit()
        {
            if (String.IsNullOrEmpty(ExhibitDetails))
            {
                //Message = "Please select Exhibit Document.";
                Message = PDMsg.NotEmpty("Exhibit Document");
            }
            else if (String.IsNullOrEmpty(ExhibitNumber.Trim()))
            {
                //Message = "Please enter Exhibit Number.";
                Message = PDMsg.NotEmpty("Exhibit Number");
            }
            else
            {
                bool val = exhit.updateExhitbitMaster(ExhibitNumber, ExhibitDetails);
                if (val)
                {
                    Message = PDMsg.UpdatedSuccessfully;
                    ClearExhibit();
                    this.DtDataview = exhit.GetExhibitMaster();
                    LoadFormData();
                }
            }
            //string query = "UPDATE [EXHIBIT_DOC] set [EX_NO]='" + ExhibitNumber + "' WHERE [DOC_NAME]='" + ExhibitDetails + "'";
            //IEnumerable<int> result = userinformation.Dal.SFLPDDatabase.ExecuteQuery<int>(query);
            //MessageBox.Show("Exhibit Updated");
            //Message = "Exhibit Updated";

            //using (SFLPD db = new SFLPD(userinformation.Dal.SFLPDDatabase.Connection))
            //{
            //    IEnumerable<int> result = db.ExecuteQuery<int>(query);
            //    //var exhibit = (from e in db.EXHIBIT_DOC where e.DOC_NAME == ExhibitDetails select e).FirstOrDefault();
            //    //exhibit.EX_NO = ExhibitNumber;
            //    //db.SubmitChanges();
            //    ExhibitDetails = "";
            //    ExhibitNumber = "";  
            //}                    
        }
    }
}
