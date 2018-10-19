using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace ProcessDesigner
{
    public partial class frmProductInformation : UserControl
    {       
        ProductInformationViewModel vm = null;
        FeasibleReportAndCostSheet bll = null;
        private WPF.MDI.MdiChild mdiChild;
        public frmProductInformation(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string title = "Product Master")
        {
            InitializeComponent();

            this.mdiChild = mdiChild;
            vm = new ProductInformationViewModel(userInformation, mdiChild, entityPrimaryKey, operationMode, title);
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            if (vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
                vm.CloseAction = new Action(() => mdiChild.Close());

            bll = new FeasibleReportAndCostSheet(userInformation);

            List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("PRD_MAST");
            this.SetColumnLength<TextBox>(lstTableDescription);
        }

    //    public frmProductInformation(UserInformation userInformation, System.Windows.Window window, int entityPrimaryKey,
    //OperationMode operationMode, string title = "Product Master")
    //    {
    //        InitializeComponent();

    //        vm = new ProductInformationViewModel(userInformation, mdiChild, entityPrimaryKey, operationMode, title);
    //        this.DataContext = vm;
    //        window.Closing += vm.CloseMethodWindow;
    //        if (vm.CloseAction == null && window.IsNotNullOrEmpty())
    //            vm.CloseAction = new Action(() => window.Close());

    //        bll = new FeasibleReportAndCostSheet(userInformation);

    //        List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("DDCI_INFO");
    //        this.SetColumnLength<TextBox>(lstTableDescription);
    //    }

     

        private void ProductInformation_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)ltbPartNo.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


    }
}


