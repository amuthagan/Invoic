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
using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmForging.xaml
    /// </summary>
    public partial class frmForging : Window
    {
        private ForgingMachine bll;
        public frmForging(UserInformation userInformation, string costCentCode)
        {
            InitializeComponent();
            bll = new ForgingMachine(userInformation);
            ForgingMachineViewModel vm = new ForgingMachineViewModel(userInformation, costCentCode);
            this.DataContext = vm;
            this.Closing += vm.CloseMethodWindow;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
            List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("DDFORGING_MAC");
            this.SetColumnLength<TextBox>(lstTableDescription);

        }    


    }

}
