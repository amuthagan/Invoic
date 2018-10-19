using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for frmToolsQuickView.xaml
    /// </summary>
    public partial class frmToolsQuickView : Window
    {

        ToolsQuickViewViewModel vm;
        public frmToolsQuickView()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;

            InitializeComponent();
        }

        public frmToolsQuickView(UserInformation userInfo, string toolCode, DataView dvAddedToolCode = null)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;

            InitializeComponent();
            vm = new ToolsQuickViewViewModel(userInfo, dvAddedToolCode);
            this.DataContext = vm;
            vm.ToolsQuickView.TOOL_CD = toolCode;
            vm.DimensionsParameters = DimensionsParameters;
            vm.DimensionsPreviewImage = DimensionsImgPhoto;
            vm.RpdDataGrid = rpdDataGrid;
            vm.dgvToolsScheduleRev = dgvToolsScheduleRev;
            if (dvAddedToolCode != null)
            {
                if (dvAddedToolCode.Count == 0)
                {
                    rpdDataGrid.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                rpdDataGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            //vm.DrawingsParameters = DrawingsParameters;
            //vm.DrawingsPreviewImage = DrawingsImgPhoto;

            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
