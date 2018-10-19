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
using ProcessDesigner.Common;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmCPMMaster.xaml
    /// </summary>
    public partial class frmCPMMaster : UserControl
    {
        //WPF.MDI.MdiChild me;
        public frmCPMMaster(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            ViewModel.CPMMasterViewModel fm = new ViewModel.CPMMasterViewModel(userInformation);
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            //            ((TextBox)cmbDept.FindName("txtCombobox")).Focus();
            cmbDept.Focus();
        }


    }
}
