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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmPointmac.xaml
    /// </summary>
    public partial class frmPointmac : Window
    {
        public frmPointmac(UserInformation userInformation, string costCentCode)
        {
            InitializeComponent();
            PointMachineViewModel vm = new PointMachineViewModel(userInformation, costCentCode);
            this.DataContext = vm;
            this.Closing += vm.CloseMethodWindow;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
