using ProcessDesigner.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Expression.Interactivity;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmSecurity.xaml
    /// </summary>
    public partial class frmSecurity : UserControl
    {
        private WPF.MDI.MdiChild _mdiChild;
        public frmSecurity(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            InitializeComponent();
            Model.SecurityViewModel vm = new Model.SecurityViewModel(userInformation);
            this.DataContext = vm;
            _mdiChild = mdiChild;
            //_mdiChild.Closing += ;
            _mdiChild.Closing += vm.CloseMethod;
        }
    }
}
