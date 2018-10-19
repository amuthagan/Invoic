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
    /// Interaction logic for frmTrimmac.xaml
    /// </summary>
    public partial class frmTrimmac : Window
    {
        public frmTrimmac(UserInformation userInformation, string costCentCode)
        {
            InitializeComponent();
            ViewModel.TrimmacViewModel vm = new ViewModel.TrimmacViewModel(userInformation, costCentCode);
            this.DataContext = vm;
            this.Closing += vm.CloseMethodWindow;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
