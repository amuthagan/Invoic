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
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using ProcessDesigner.UICommon;
using ProcessDesigner.UserControls;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmProductSearch.xaml
    /// </summary>
    public partial class frmProductSearch : UserControl
    {
        ViewModel.ProductSearchViewModel psmvm;
        public frmProductSearch(UserInformation userInformation, WPF.MDI.MdiChild mdi)
        {
            try
            {

                InitializeComponent();
                ViewModel.ProductSearchViewModel psmvm = new ViewModel.ProductSearchViewModel(userInformation);
                this.DataContext = psmvm;
                mdi.Closing += psmvm.CloseMethod;

                if (psmvm.CloseAction == null)
                    psmvm.CloseAction = new Action(() => mdi.Close());
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
