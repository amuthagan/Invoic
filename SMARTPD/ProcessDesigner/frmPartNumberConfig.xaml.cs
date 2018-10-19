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

using ProcessDesigner.Common;
using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmPartNumberConfig.xaml
    /// </summary>
    public partial class frmPartNumberConfig : UserControl
    {
        private PartNumberConfiguration bll = null;
        public int PartNumberCode { get; set; }
        public frmPartNumberConfig(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                InitializeComponent();
                CustPartNoDescription.Focus();

                PartNumberCode = 0;
                PartNumberConfigViewModel vm = new PartNumberConfigViewModel(userInformation, PartNumberCode, OperationMode.AddNew);
                this.DataContext = vm;
                mdiChild.Closing += vm.CloseMethod;
                if (vm.CloseAction == null)
                    vm.CloseAction = new Action(() => mdiChild.Close());

                bll = new PartNumberConfiguration(userInformation);

                List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("PartNumberConfig");
                this.SetColumnLength<TextBox>(lstTableDescription);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        public frmPartNumberConfig(UserInformation userInformation, System.Windows.Window window, int entityPrimaryKey,
    OperationMode operationMode, string title = "Part Number Configuration")
        {
            try
            {
                InitializeComponent();
                CustPartNoDescription.Focus();

                PartNumberCode = entityPrimaryKey;
                PartNumberConfigViewModel vm = new PartNumberConfigViewModel(userInformation, PartNumberCode, operationMode);
                this.DataContext = vm;
                window.Closing += vm.CloseMethodWindow;
                if (vm.CloseAction == null && window.IsNotNullOrEmpty())
                    vm.CloseAction = new Action(() => window.Close());

                bll = new PartNumberConfiguration(userInformation);

                List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("PartNumberConfig");
                this.SetColumnLength<TextBox>(lstTableDescription);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            object ctrl = CustPartNoDescription.FindName("CustPartNoDescription");

            if (ctrl.IsNotNullOrEmpty())
            {
                UserControl usrCtrl = ctrl as UserControl;
                if (usrCtrl.IsNotNullOrEmpty()) usrCtrl.Focus();
            }
        }

    }
}
