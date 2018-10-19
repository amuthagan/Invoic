using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.ViewModel;
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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmrptPartNos.xaml
    /// </summary>
    public partial class frmrptPartNos : UserControl
    {
        private string InputMessage { get; set; }
        ReportMISProductInformationViewModel vm = null;
        private WPF.MDI.MdiChild mdiChild;

        public frmrptPartNos(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, PRD_MAST productMaster = null, ReportMISProductInformationModel productInformationModel = null, DDCUST_MAST customerMaster = null, List<DDLOC_MAST> lstLocation = null, bool refreshOnLoad = false, string title = "Customer Partno Wise Report")
        {
            InitializeComponent();

            this.mdiChild = mdiChild;
            vm = new ReportMISProductInformationViewModel(userInformation, mdiChild, productMaster, productInformationModel, customerMaster, lstLocation, refreshOnLoad, title);
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            //if (vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
            //    vm.CloseAction = new Action(() => mdiChild.Close());


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)ltbCustomer.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


    }
}
