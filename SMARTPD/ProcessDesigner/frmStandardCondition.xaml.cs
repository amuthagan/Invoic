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
    /// Interaction logic for frmStandardCondition.xaml
    /// </summary>
    public partial class frmStandardCondition : Window
    {
        public frmStandardCondition(string costcentercode, string categoryid)
        {
            InitializeComponent();
            //  CostCenterCode = "142411";
            // CategoryId = "1";
            StatndardConditionViewModel vm = new StatndardConditionViewModel(costcentercode, categoryid);
            this.DataContext = vm;
        }

    }
}
