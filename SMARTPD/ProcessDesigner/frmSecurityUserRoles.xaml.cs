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

namespace ProcessDesigner
{

    /// <summary>
    /// Interaction logic for frmSecurityUserRoles.xaml
    /// </summary>
    public partial class frmSecurityUserRoles : Window
    {
        private Model.SecurityUserRolesViewModel survm;
        public frmSecurityUserRoles(UserInformation userInformation, string username)
        {
            try
            {
                InitializeComponent();
                survm = new Model.SecurityUserRolesViewModel(userInformation, username);
                this.DataContext = survm;
                this.Closing += survm.CloseMethodWindow;
                if (survm.CloseAction == null)
                    survm.CloseAction = new Action(() => this.Close());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SmartPD");
                throw ex.LogException();
            }
        }


    }
}
