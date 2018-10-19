using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System.Globalization;
using System.Threading;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmCustomerReference.xaml
    /// </summary>
    public partial class frmCustomerReference : UserControl
    {
        private CustomerReferenceViewModel crvm;
        private WPF.MDI.MdiChild _mdiChild;
        /// <summary>
        /// screen type="ECN" - CUSTOMER  , type="PCN"   SFL
        /// </summary>
        /// <param name="userInformation"></param>
        /// <param name="screenType"></param>
        public frmCustomerReference(UserInformation userInformation, string partNo, string screenType, WPF.MDI.MdiChild mdiChild, string partDesc)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                _mdiChild = mdiChild;
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                Thread.CurrentThread.CurrentCulture = ci;
                InitializeComponent();
                crvm = new CustomerReferenceViewModel(userInformation, partNo, screenType, partDesc);
                this.DataContext = crvm;
                this._mdiChild.Closing += crvm.CloseMethod;

                if (screenType == "ECN")
                    _mdiChild.Title = crvm.ApplicationTitle + " - " + "ECN";
                else
                    _mdiChild.Title = crvm.ApplicationTitle + " - " + "PCN";
                if (crvm.CloseAction == null)
                    crvm.CloseAction = new Action(() => _mdiChild.Close());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            //DrawingViewModel vm = new DrawingViewModel(userInformation, null, DrawingsMaster, OperationMode.View);
            //this.DataContext = vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)cmbPartNo.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }
    }
}
