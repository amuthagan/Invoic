using ProcessDesigner.Common;
using ProcessDesigner.BLL;
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
    /// Interaction logic for frmQcp.xaml
    /// </summary>
    public partial class frmQcp : UserControl
    {
        public frmQcp(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            QcpViewModel qcp = new QcpViewModel(userInformation);
            this.DataContext = qcp;
            if (qcp.CloseAction == null)
                qcp.CloseAction = new Action(() => me.Close());
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
