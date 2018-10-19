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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmProductCategoryMaster.xaml
    /// </summary>
    public partial class frmProductCategoryMaster : UserControl
    {
        public frmProductCategoryMaster(WPF.MDI.MdiChild me, UserInformation userInfo)
        {
            InitializeComponent();
            ProductCategoryViewModel vm = new ProductCategoryViewModel(userInfo);
            this.DataContext = vm;
            me.Closing += vm.CloseMethod;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtProductCode.Focus();
          //  ((TextBox)cmbPrdCategory.FindName("txtCombobox")).Focus();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StatusMessage.setStatus("Ready", "");
        }

                      
       
    }
}
