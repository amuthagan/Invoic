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
using ProcessDesigner.Model;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmISIR.xaml
    /// </summary>
    public partial class frmISIR : Window
    {
        public frmISIR(UserInformation userInformation, ISIRModel im)
        {
            InitializeComponent();
            ISIRViewModel vm = new ISIRViewModel(userInformation, im);
            this.DataContext = vm;

            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
