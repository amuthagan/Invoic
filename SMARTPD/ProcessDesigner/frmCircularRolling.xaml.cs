using ProcessDesigner.BLL;
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
    /// Interaction logic for frmCircularRolling.xaml
    /// </summary>
    public partial class frmCircularRolling : Window
    {
        public frmCircularRolling(UserInformation userInformation, string costCentCode)
        {
            InitializeComponent();
            ViewModel.CircularRollingViewModel vm = new ViewModel.CircularRollingViewModel(userInformation, costCentCode);
            this.DataContext = vm;
            this.Closing += vm.CloseMethodWindow;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
