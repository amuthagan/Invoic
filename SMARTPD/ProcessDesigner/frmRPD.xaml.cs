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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmRPD.xaml
    /// </summary>
    public partial class frmRPD : UserControl
    {
        public frmRPD(WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            RPDViewModel vm = new RPDViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
        }
        private void frmRPD_Loaded(object sender, RoutedEventArgs e)
        {

            ((TextBox)cmbCIRefference.FindName("txtCombobox")).Focus();
        }

        private void cmbCustomer_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
