using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for frmAPQP.xaml
    /// </summary>
    public partial class frmAPQP : UserControl
    {
        private WPF.MDI.MdiChild _mdiChild;
        public frmAPQP(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                InitializeComponent();
                _mdiChild = mdiChild;
                ViewModel.APQPViewModel vmapqp = new ViewModel.APQPViewModel(userInformation, _mdiChild);
                this.DataContext = vmapqp;
                this._mdiChild = mdiChild;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //dgAPQP.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
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
