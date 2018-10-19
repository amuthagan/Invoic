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
using ProcessDesigner.ViewModel;
using System.Data;


namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmSecurityUsers.xaml
    /// </summary>
    public partial class frmSecurityUsers : Window
    {
        public frmSecurityUsers(UserInformation userInfo, DataRowView selectedItem, String mode)
        {
            InitializeComponent();
            SecurityUsersViewModel vm = new SecurityUsersViewModel(userInfo, selectedItem, mode);
            this.DataContext = vm;
            this.Closing += vm.CloseMethodWindow;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
           
        }
    }
}
