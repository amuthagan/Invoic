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
using ProcessDesigner.Model;
using ProcessDesigner.ViewModel;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.BLL;
using WPF.MDI;


namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmOperationMain.xaml
    /// </summary>
    public partial class frmOperationMain : UserControl
    {
        private string _formName;
        public frmOperationMain(string formName, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            this._formName = formName;


            //  FocusManager.SetFocusedElement(this, txt_operDesc);
            OperationMasterViewModel vm = new OperationMasterViewModel(_formName);
            this.DataContext = vm;
            me.Closing += vm.CloseMethod;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
        }

        private void frmOperationMain_Loaded(object sender, RoutedEventArgs e)
        {
            //  cmbOperMaster.Focus();
            ((TextBox)cmbOperMaster.FindName("txtCombobox")).Focus();
        }



    }
}
