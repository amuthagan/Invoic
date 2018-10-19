using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;
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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmForm2.xaml
    /// </summary>
    public partial class frmForm2 : Window
    {
        public frmForm2()
        {
            InitializeComponent();
        }
        public frmForm2(UserInformation userInfo)
        {
            InitializeComponent();
            AuditReportViewModel vm = new AuditReportViewModel(userInfo);
            this.DataContext = vm;
            this.Closing += vm.CloseMethodWindow;
        }
    }
}
