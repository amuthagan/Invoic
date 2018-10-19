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
using ProcessDesigner.Common;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmProductWeight.xaml
    /// </summary>
    public partial class frmProductWeight : UserControl
    {
        public String WTOption = "";
        public Boolean IsSelected = false;
        public frmProductWeight(WPF.MDI.MdiChild me, UserInformation userInfo, string ciReference, string weightOption, OperationMode mode, int entityPrimaryKey)
        {
            InitializeComponent();
            WTOption = weightOption;           
            ProductWeightViewModel vm = new ProductWeightViewModel(userInfo, ciReference, weightOption, mode, entityPrimaryKey);
            this.DataContext = vm;
            vm.dgProductWeight = dgvWeightCalc;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            IsSelected = true;            
        }

    }
}
