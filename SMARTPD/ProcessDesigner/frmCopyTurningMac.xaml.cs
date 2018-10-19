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
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System.Data;
using ProcessDesigner.BLL;
using WPF.MDI;


namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmCopyTurningMachine.xaml
    /// </summary>
    public partial class frmCopyTurningMac : Window
    {
        public frmCopyTurningMac(UserInformation userInformation, string costCentCode)
        {
            InitializeComponent();
            CopyTurningMacViewModel vm = new CopyTurningMacViewModel(userInformation, costCentCode);
            this.DataContext = vm;
            this.Closing += vm.CloseMethodWindow;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
