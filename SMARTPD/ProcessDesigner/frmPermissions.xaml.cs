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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmPermissions.xaml
    /// </summary>
    public partial class frmPermissions : Window
    {
        public frmPermissions(UserInformation userInformation, string userRole)
        {
            InitializeComponent();
            ViewModel.PermissionViewModel pvm = new ViewModel.PermissionViewModel(userInformation, userRole);
            this.DataContext = pvm;
            this.Closing += pvm.CloseMethodWindow;
            if (pvm.CloseAction == null)
                pvm.CloseAction = new Action(() => this.Close());
        }

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void grdPermission_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
