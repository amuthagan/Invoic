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
using System.Globalization;
namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmECNPCN.xaml
    /// </summary>
    public partial class frmECNPCN : Window
    {
        private string applicationTitle = "SmartPD - ";
        public frmECNPCN(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, string ecnorpcn, Nullable<DateTime> startdate, Nullable<DateTime> enddate)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                InitializeComponent();
                ViewModel.ECNPCNViewModel ddecnpcn = new ViewModel.ECNPCNViewModel(userInformation, mdiChild, ecnorpcn, startdate, enddate);
                this.DataContext = ddecnpcn;
                this.Title = applicationTitle + ecnorpcn;
            }
            catch (Exception ex)
            {

            }

        }
    }
}
