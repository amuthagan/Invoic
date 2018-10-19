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
using ProcessDesigner.Model;
using ProcessDesigner.ViewModel;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.BLL;
using WPF.MDI;
using ProcessDesigner.UserControls;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmToolsInfo.xaml
    /// </summary>
    public partial class frmToolsInfo : UserControl
    {
        public frmToolsInfo(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {

            InitializeComponent();
            ToolInfoViewModel vm = new ToolInfoViewModel();
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            vm.PreviewImage = imgPhoto;
            vm.DgToolInfo = rpdDataGrid;

            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => mdiChild.Close());

        }

        private void rpdDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void rpdDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            //e.Handled = true;
            e.Column.SortMemberPath = "P002";
        }     
    }
}


