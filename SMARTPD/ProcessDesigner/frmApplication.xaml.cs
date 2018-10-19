using ProcessDesigner.BLL;
using ProcessDesigner.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmApplication.xaml
    /// </summary>
    public partial class frmApplication : UserControl
    {
        public frmApplication(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            ApplicationViewModel appView = new ApplicationViewModel(userInfo);
            this.DataContext = appView;
            me.Closing += appView.CloseMethod;
            if (appView.CloseAction == null)
                appView.CloseAction = new Action(() => me.Close());
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)cmbPswName.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
