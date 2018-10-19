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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmLocationMasterRange.xaml
    /// </summary>
    public partial class frmLocationMasterRange : UserControl 
    {
        public frmLocationMasterRange(WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            LocationMasterViewModel vm = new LocationMasterViewModel();
            this.DataContext = vm;
            me.Closing += vm.CloseMethod;

            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
           
        }
        private void frmLocationMasterRange_Loaded(object sender, RoutedEventArgs e)
        {

            ((TextBox)cmbLocMaster.FindName("txtCombobox")).Focus();
        }
    }
}
