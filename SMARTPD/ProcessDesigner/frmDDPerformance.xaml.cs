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
    /// Interaction logic for frmDDPerformance.xaml
    /// </summary>
    public partial class frmDDPerformance : UserControl
    {
        private WPF.MDI.MdiChild _mdiChild;
        public frmDDPerformance(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;

                InitializeComponent();
                ViewModel.DDPerformanceViewModel ddpvm = new ViewModel.DDPerformanceViewModel(userInformation, _mdiChild, dgDDPerformance);
                //ddpvm.dgDDPerformance = dgDDPerformance;
                this.DataContext = ddpvm;
                this._mdiChild = mdiChild;
                mdiChild.Closing += ddpvm.CloseMethod;              
                
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}
