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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmSapSub.xaml
    /// </summary>
    public partial class frmSapSub : UserControl
    {
        public frmSapSub()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUnit.Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
