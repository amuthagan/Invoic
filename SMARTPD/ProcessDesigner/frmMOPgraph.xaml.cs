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
using System.Globalization;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmMOPgraph.xaml
    /// </summary>
    public partial class frmMOPgraph : Window
    {
        private string applicationTitle = "SmartPD - ";
        public frmMOPgraph(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                InitializeComponent();
                ViewModel.MOPGraphViewModel mopgvm = new ViewModel.MOPGraphViewModel(userInformation, mdiChild);
                this.DataContext = mopgvm;
                this.Title = applicationTitle + "Measure Of Performance";
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
