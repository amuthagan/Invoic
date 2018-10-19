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
using ProcessDesigner.DAL;
using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmSNO.xaml
    /// </summary>
    public partial class frmSNO : Window
    {
        public frmSNO(UserInformation userInformation)
        {
            InitializeComponent();
        //    RawMaterialCode = string.Empty;
         //   RawMaterialViewMode vm = new RawMaterialViewMode(userInformation, RawMaterialCode, OperationMode.AddNew);
         //   this.DataContext = vm;
       //     if (vm.CloseAction == null)
           //     vm.CloseAction = new Action(() => mdiChild.Close());
            TempViewModel vm = new TempViewModel(userInformation);
           this.DataContext = vm;
         //   custComboBoxCol.ItemsSource = vm.DtData;
             
        }
    }
}
