using ProcessDesigner.BLL;
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
using ProcessDesigner.Model;
using ProcessDesigner.Common;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for CIReferenceCopy.xaml
    /// </summary>
    public partial class CIReferenceCopy : UserControl
    {
        private string InputMessage { get; set; }
        public DDCI_INFO ActiveEntity = null;
        public FeasibleReportAndCostSheet bll = null;
        public CIReferenceCopy(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, DDCI_INFO ActiveEntity)
        {
            InitializeComponent();
            bll = new FeasibleReportAndCostSheet(userInformation);
            this.ActiveEntity = ActiveEntity;
        }

        private void Controls_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement ctrl = sender as FrameworkElement;
                if (ctrl.IsNotNullOrEmpty())
                {
                    InputMessage = ctrl.ToolTip.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
