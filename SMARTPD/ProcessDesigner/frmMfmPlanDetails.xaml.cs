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
    /// Interaction logic for frmMfmPlanDetails.xaml
    /// </summary>
    public partial class frmMfmPlanDetails : UserControl 
    {
        public frmMfmPlanDetails(WPF.MDI.MdiChild me, UserInformation userInfo)
        {
            InitializeComponent();
            MFMPlanDetailsViewModel vm = new MFMPlanDetailsViewModel(userInfo);
            this.DataContext = vm;
            vm.dgrdMFMPlanDetails = dgrdMFMPlanDetails;
            me.Closing += vm.CloseMethod;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
        } 
    }
}
