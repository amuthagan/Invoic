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
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System.Data;
using ProcessDesigner.Model;
namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmControlPlanGeneration.xaml
    /// </summary>
    public partial class frmControlPlanGeneration : Window
    {
        public frmControlPlanGeneration(UserInformation userInformation, string partNo, decimal routeNo, decimal seqNo, PCCSModel dtPccsLocal)
        {
            InitializeComponent();
            frmCPGViewModel fm = new frmCPGViewModel(userInformation, partNo, routeNo, seqNo, dtPccsLocal);
            this.DataContext = fm;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => this.Close());
        }
    }
}
