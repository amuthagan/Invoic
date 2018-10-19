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
    /// Interaction logic for frmRptFeatureWise.xaml
    /// </summary>
    public partial class frmRptFeatureWise : UserControl
    {
        private string InputMessage { get; set; }
        ReportMISFeatureWiseViewModel vm = null;
        private WPF.MDI.MdiChild mdiChild;

        public frmRptFeatureWise(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, PCCS feature = null, PCCS feature1 = null, PCCS feature2 = null, PCCS specification = null, PRD_MAST productMaster = null, bool refreshOnLoad = false, string title = "Feature Wise Report")
        {
            InitializeComponent();

            this.mdiChild = mdiChild;
            vm = new ReportMISFeatureWiseViewModel(userInformation, mdiChild, feature, feature1, feature2, specification, productMaster, refreshOnLoad, title);
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => mdiChild.Close());


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)ltbFeature.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

       
        //public frmRptFeatureWise(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, PCCS feature = null, PCCS feature1 = null, PCCS feature2 = null, PCCS specification = null, PRD_MAST productMaster = null, bool refreshOnLoad = false, string title = "Feature Wise Report", string partno, string partdesc)
        //{
        //    InitializeComponent();

        //    this.mdiChild = mdiChild;
        //    vm = new ReportMISFeatureWiseViewModel(userInformation, mdiChild, feature, feature1, feature2, specification, productMaster, refreshOnLoad, title, partno, partdesc);
        //    this.DataContext = vm;
        //    mdiChild.Closing += vm.CloseMethod;
        //    if (vm.CloseAction == null)
        //        vm.CloseAction = new Action(() => mdiChild.Close());
        //}
    }
}
