using ProcessDesigner.Common;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
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
    /// Interaction logic for frmPendingPartStatus.xaml
    /// </summary>
    public partial class frmPendingPartStatus : UserControl
    {
        private string InputMessage { get; set; }
        ReportPendingPartNumberStatusViewModel vm = null;
        private WPF.MDI.MdiChild mdiChild;

        public frmPendingPartStatus(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, bool refreshOnLoad = false, string title = "Pending Parts Status Report")
        {
            InitializeComponent();
            this.mdiChild = mdiChild;

            vm = new ReportPendingPartNumberStatusViewModel(userInformation, mdiChild, refreshOnLoad, title);
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            if (vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
                vm.CloseAction = new Action(() => mdiChild.Close());

        }
    }
}
