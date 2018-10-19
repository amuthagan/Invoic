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
using System.Data;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmRolesInfo.xaml
    /// </summary>
    public partial class frmRolesInfo : Window
    {
        public frmRolesInfo(UserInformation userInformation, DataRowView selectedItem, string mode)
        {
            InitializeComponent();
            ViewModel.RolesInfoViewModel rolesinfovm = new ViewModel.RolesInfoViewModel(userInformation, selectedItem, mode);
            this.DataContext = rolesinfovm;
            this.Closing += rolesinfovm.CloseMethodWindow;
            if (rolesinfovm.CloseAction == null)
                rolesinfovm.CloseAction = new Action(() => this.Close());

        }
    }
}
