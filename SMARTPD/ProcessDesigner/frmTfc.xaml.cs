using ProcessDesigner.BLL;
using ProcessDesigner.Common;
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
    /// Interaction logic for frmTfc.xaml
    /// </summary>
    public partial class frmTfc : UserControl
    {
        public frmTfc(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            TfcViewModel tfc = new TfcViewModel(userInfo);
            this.DataContext = tfc;
            me.Closing += tfc.CloseMethod;

            if (tfc.CloseAction == null)
                tfc.CloseAction = new Action(() => me.Close());
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
