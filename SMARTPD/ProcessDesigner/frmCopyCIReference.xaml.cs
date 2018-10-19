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

using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for CopyCIReference.xaml
    /// </summary>
    public partial class frmCopyCIReference : UserControl
    {
        public CopyCIReferenceViewModel Vm = null;

        private string InputMessage { get; set; }
        public DDCI_INFO ActiveEntity = null;
        private FeasibleReportAndCostSheet bll = null;
        public frmCopyCIReference(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, DDCI_INFO activeentity, OperationMode operationMode)
        {
            InitializeComponent();
            bll = new FeasibleReportAndCostSheet(userInformation);
            this.ActiveEntity = activeentity;

            Vm = new CopyCIReferenceViewModel(userInformation, mdiChild, activeentity, operationMode);
            this.DataContext = Vm;
            if (Vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
                Vm.CloseAction = new Action(() => mdiChild.Close());

            bll = new FeasibleReportAndCostSheet(userInformation);

            List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("DDCI_INFO");
            this.SetColumnLength<TextBox>(lstTableDescription);

        }

        public frmCopyCIReference(UserInformation userInformation, Window mdiChild, DDCI_INFO activeentity, OperationMode operationMode)
        {
            InitializeComponent();
            bll = new FeasibleReportAndCostSheet(userInformation);
            this.ActiveEntity = activeentity;

            Vm = new CopyCIReferenceViewModel(userInformation, activeentity, operationMode);
            this.DataContext = Vm;
            if (Vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
                Vm.CloseAction = new Action(() => mdiChild.Close());

            bll = new FeasibleReportAndCostSheet(userInformation);

            List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("DDCI_INFO");
            this.SetColumnLength<TextBox>(lstTableDescription);

        }        
    }
}
