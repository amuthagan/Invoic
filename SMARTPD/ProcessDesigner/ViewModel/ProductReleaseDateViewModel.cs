using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessDesigner.ViewModel
{
    class ProductReleaseDateViewModel : ViewModelBase
    {
        ProductInformationModel _productInformationModel = null;

        public ProductInformationModel PRODUCT_INFORMATION_MODEL
        {
            get { return _productInformationModel; }
            set
            {
                _productInformationModel = value;
                NotifyPropertyChanged("PRODUCT_INFORMATION_MODEL");
            }
        }

        public ProductReleaseDateViewModel(UserInformation userInformation, System.Windows.Window window, ProductInformationModel productInformationModel,
    ProcessDesigner.Common.OperationMode operationMode, string title = "Document Release Date")
        {
            PRODUCT_INFORMATION_MODEL = (productInformationModel.IsNotNullOrEmpty() ? productInformationModel : new ProductInformationModel());
            if (!_productInformationModel.DOC_REL_DATE.IsNotNullOrEmpty())
            {
                FeasibleReportAndCostSheet bll = new FeasibleReportAndCostSheet(userInformation);
                _productInformationModel.DOC_REL_DATE = bll.ServerDateTime();
            }
            this.saveCommand = new DelegateCommand(this.SaveSubmitCommand);
        }

        public Action CloseAction { get; set; }
        private RelayCommand _onCancelCommand;
        private void CloseSubmitCommand()
        {
            try
            {
                if (CloseAction.IsNotNullOrEmpty())
                {
                    PRODUCT_INFORMATION_MODEL.DOC_REL_DATE = null;
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                    {
                        CloseAction();
                    }
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

        public ICommand CloseClickCommand
        {
            get
            {

                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.CloseSubmitCommand(), null);
                }
                return _onCancelCommand;
            }
        }

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }
        private void SaveSubmitCommand()
        {
            DateTime? doc_rel_date = PRODUCT_INFORMATION_MODEL.DOC_REL_DATE;
            //***** Start Commented by Jeyan
            //RelayCommand closeWindowCommand = new RelayCommand(param => this.CloseSubmitCommand(), null);
            //closeWindowCommand.Execute(null);
            //***** End Jeyan
            PRODUCT_INFORMATION_MODEL.PART_DESC = "OKButtonClicked";
            PRODUCT_INFORMATION_MODEL.DOC_REL_DATE = doc_rel_date;
            //if (CloseAction.IsNotNullOrEmpty())
             CloseAction(); // Uncommented by Jeyan
        }
    }
}
