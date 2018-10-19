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
    /// Interaction logic for frmCategory.xaml
    /// </summary>
    public partial class frmCategory : UserControl
    {
        public frmCategory(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            CategoryViewModel cv = new CategoryViewModel(userInfo);
            this.DataContext = cv;
            me.Closing += cv.CloseMethod;
            if (cv.CloseAction == null)
                cv.CloseAction = new Action(() => me.Close());
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
