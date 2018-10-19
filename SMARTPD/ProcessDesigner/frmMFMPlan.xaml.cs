using ProcessDesigner.BLL;
using ProcessDesigner.Common;
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
    /// Interaction logic for frmMFMPlan.xaml
    /// </summary>
    public partial class frmMFMPlan : Window
    {
        public frmMFMPlan(UserInformation userInfo)
        {
            InitializeComponent();
            MFMPlanViewModel vm = new MFMPlanViewModel(userInfo);
            this.DataContext = vm;            
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
