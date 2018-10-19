using ProcessDesigner.BLL;
using ProcessDesigner.Model;
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
    /// Interaction logic for frmPartSubmission.xaml
    /// </summary>
    public partial class frmPartSubmissionWarrant : Window
    {
        public frmPartSubmissionWarrant(UserInformation userInformation, PartSubmissionWarrantModel pm)
        {
            InitializeComponent();
            PartSubmissionWarrantViewModel pvm = new PartSubmissionWarrantViewModel(userInformation, pm);
            this.DataContext = pvm;           
            if (pvm.CloseAction == null)
                pvm.CloseAction = new Action(() => this.Close());
        }
    }
}
