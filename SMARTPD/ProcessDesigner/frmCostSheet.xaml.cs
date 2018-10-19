using ProcessDesigner.BLL;
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
    /// Interaction logic for frmCostSheet.xaml
    /// </summary>
    public partial class frmCostSheet : Window
    {       
        public frmCostSheet(UserInformation userinfo, string partNo, int routeNo)
        {
            InitializeComponent();
            CostSheetViewModel vm = new CostSheetViewModel(userinfo, partNo, routeNo);
            this.DataContext = vm;
          
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());

        }
    }
}
