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
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmCopyStatus.xaml
    /// </summary>
    public partial class frmCopyStatus : Window
    {
        //public string VarProcess = string.Empty;
        //public string OldPartNo = string.Empty;
        //public string OldRouteNo = string.Empty;
        public frmCopyStatus(string varProcess, string oldPartNo, string oldRouteNo, string oldSeqNo, string oldCC, string oldSH)
        {
            InitializeComponent();
            // this.OldPartNo = OldPartNo;
            // OldRouteNo = OldRouteNo;
            // this._formName = formName;
            CopyStatusViewModel vm = new CopyStatusViewModel(varProcess, oldPartNo, oldRouteNo, oldSeqNo, oldCC, oldSH);
            this.Closing += vm.CloseMethodWindow;
            this.DataContext = vm;
            // PrivImg = Icon;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }

    }
}
