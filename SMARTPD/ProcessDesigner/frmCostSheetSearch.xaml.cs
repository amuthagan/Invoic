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
using ProcessDesigner.UserControls;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmCostSheetSearch.xaml
    /// </summary>
    public partial class frmCostSheetSearch : UserControl
    {
        //public frmCostSheetSearch()
        //{
        //    InitializeComponent();
        //}


        public int CostSheetSearch { get; set; }
        private string InputMessage { get; set; }
        //private WPF.MDI.MdiChild me;
        private WPF.MDI.MdiChild mdiChild;
        private string calledfromparentform = "MainWindow";
        public frmCostSheetSearch(UserInformation userInformation, WPF.MDI.MdiChild mdi)
        {

            InitializeComponent();
            //ltbRmCode.Focus();
            CostSheetSearchViewModel vm = new CostSheetSearchViewModel(userInformation, mdi, CostSheetSearch, OperationMode.View);
            this.DataContext = vm;
            mdi.Closing += vm.CloseMethod;

            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => mdi.Close());
            //this.Title = vm.ApplicationTitle + " - " + "Cost Sheet Search";
        }

        public frmCostSheetSearch(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string calledfromparentform = "MainWindow")
        {
            InitializeComponent();
            //ltbRmCode.Focus();
            this.mdiChild = mdiChild;
            this.calledfromparentform = calledfromparentform.IsNotNullOrEmpty() ? calledfromparentform : "MainWindow";

            CostSheetSearchViewModel vm = new CostSheetSearchViewModel(userInformation, mdiChild, entityPrimaryKey, operationMode, calledfromparentform);
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => mdiChild.Close());
            //this.Title = vm.ApplicationTitle + " - " + "Cost Sheet Search";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProductDesc.Focus();
        }

    }
}
