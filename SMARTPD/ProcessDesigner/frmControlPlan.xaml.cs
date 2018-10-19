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
    /// Interaction logic for frmControlPlan.xaml
    /// </summary>
    public partial class frmControlPlan : Window
    {
        public frmControlPlan(UserInformation userInformation, string partNo, string processNo, string routeNo)
        {
            InitializeComponent();
            ControlPlanRptViewModel fm = new ControlPlanRptViewModel(userInformation, partNo, processNo, routeNo);
            this.DataContext = fm;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => this.Close());
        }
    }
}
