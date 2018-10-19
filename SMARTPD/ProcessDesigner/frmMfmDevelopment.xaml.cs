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
using System.Windows.Shapes;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmMfmDevelopment.xaml
    /// </summary>
    public partial class frmMfmDevelopment : UserControl
    {
        public frmMfmDevelopment()
        {
            InitializeComponent();
        }
        public frmMfmDevelopment(WPF.MDI.MdiChild me, UserInformation userInfo)
        {
            InitializeComponent();
            MFMDevelopmentViewModel vm = new MFMDevelopmentViewModel(userInfo);
            this.DataContext = vm;            
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)cmbPlant.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
