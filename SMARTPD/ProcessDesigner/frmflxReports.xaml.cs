using ProcessDesigner.BLL;
using System;
using System.Collections.Generic;
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
using ProcessDesigner.Common;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmflxReports.xaml
    /// </summary>
    public partial class frmflxReports : UserControl
    {
        private WPF.MDI.MdiChild _mdiChild;
        public frmflxReports(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, Nullable<DateTime> startdate, Nullable<DateTime> enddate)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                InitializeComponent();
                ViewModel.FlxReportsViewModel frvm = new ViewModel.FlxReportsViewModel(userInformation, mdiChild, startdate, enddate);
                this.DataContext = frvm;
                this._mdiChild = mdiChild;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
