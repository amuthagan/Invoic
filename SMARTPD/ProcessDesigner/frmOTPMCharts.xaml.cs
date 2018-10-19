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
    /// Interaction logic for frmOTPMCharts.xaml
    /// </summary>
    public partial class frmOTPMCharts : UserControl
    {
        private string InputMessage { get; set; }
        ReportOTPMChartsViewModel vm = null;
        private WPF.MDI.MdiChild mdiChild;

        public frmOTPMCharts(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, string chartType = null, int? workingYear = null, int? pgCatogory = null, bool refreshOnLoad = false, string title = "OTPM Charts - Development Lead Time")
        {
            InitializeComponent();

            this.mdiChild = mdiChild;
            vm = new ReportOTPMChartsViewModel(userInformation, mdiChild, chartType, workingYear, pgCatogory, refreshOnLoad, title);
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            //if (vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
            //    vm.CloseAction = new Action(() => mdiChild.Close());


        }

    }
}
