using System;
using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;
using ProcessDesigner.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmSapImport : UserControl
    {
        public frmSapImport(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            SapExportToPd fm = new SapExportToPd(userInformation);
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFileName.Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
