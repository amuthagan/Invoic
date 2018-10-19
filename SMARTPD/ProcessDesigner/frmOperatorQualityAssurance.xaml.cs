using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmOperatorQualityAssurance.xaml
    /// </summary>
    public partial class frmOperatorQualityAssurance : UserControl
    {
        private string InputMessage { get; set; }
        OperatorQualityAssuranceViewModel vm = null;
        OperatorQualityAssurance bll = null;
        private WPF.MDI.MdiChild mdiChild;

        public frmOperatorQualityAssurance(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string title = "Operator Quality Assurance Chart")
        {
            InitializeComponent();

            this.mdiChild = mdiChild;
            vm = new OperatorQualityAssuranceViewModel(userInformation, mdiChild, entityPrimaryKey, operationMode, title);
            this.DataContext = vm;
            if (vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
                vm.CloseAction = new Action(() => mdiChild.Close());

            bll = new OperatorQualityAssurance(userInformation);

        }

        public frmOperatorQualityAssurance(UserInformation userInformation, System.Windows.Window window, int entityPrimaryKey,
            OperationMode operationMode, string title = "Operator Quality Assurance Chart")
        {
            InitializeComponent();

            vm = new OperatorQualityAssuranceViewModel(userInformation, mdiChild, entityPrimaryKey, operationMode, title);
            this.DataContext = vm;
            if (vm.CloseAction == null && window.IsNotNullOrEmpty())
                vm.CloseAction = new Action(() => window.Close());

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void DataGridResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
               

    }
}
