using ProcessDesigner.BLL;
using ProcessDesigner.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmTfcECN.xaml
    /// </summary>
    public partial class frmTfcECN : UserControl
    {
        public frmTfcECN(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            TfcECNViewModel ecn = new TfcECNViewModel(userInfo);
            this.DataContext = ecn;
            me.Closing += ecn.CloseMethod;
            if (ecn.CloseAction == null)
            {
                ecn.CloseAction = new Action(() => me.Close());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)cmbPartNo.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
