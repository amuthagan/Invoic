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
using System.Data;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Globalization;
namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmDevelopmentReportSummary.xaml
    /// </summary>
    public partial class frmDDPerformanceSummary : Window
    {
        private WPF.MDI.MdiChild _mdiChild;
        public frmDDPerformanceSummary(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, string appTitle)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;

                InitializeComponent();
                ViewModel.DDPerformanceSummaryViewModel ddpsvm = new ViewModel.DDPerformanceSummaryViewModel(userInformation, _mdiChild, dgDDPerformance);
                //ddpvm.dgDDPerformance = dgDDPerformance;
                this.DataContext = ddpsvm;
                this.Title = appTitle + " - " + "D&D Performance As of " + DateTime.Now.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
