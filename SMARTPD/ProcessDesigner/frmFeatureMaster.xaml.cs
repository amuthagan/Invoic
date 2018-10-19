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
namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmCharacteristicMaster1.xaml
    /// </summary>
    public partial class frmFeatureMaster : UserControl
    {
        //WPF.MDI.MdiChild me;
        public frmFeatureMaster(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            Model.FeatureMasterViewModel fm = new Model.FeatureMasterViewModel(userInfo);
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
        }

        private void dgvCharacteristicsMast_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((TextBox)cmbOperationCode.FindName("txtCombobox")).Focus();
            ((TextBox)cmbFeatureCode.FindName("txtCombobox")).Background = Brushes.Linen;
            txtOperDesc.Background = Brushes.Linen;

        }
    }
}
