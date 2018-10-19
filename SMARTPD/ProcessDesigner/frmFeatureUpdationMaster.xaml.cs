using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
    /// Interaction logic for frmFeatureUpdationMaster.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmFeatureUpdationMaster : UserControl
    {
        public frmFeatureUpdationMaster(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            FeatureUpdateViewModel fu = new FeatureUpdateViewModel(userInformation);
            this.DataContext = fu;
            me.Closing += fu.CloseMethod;
        }

        [Import]
        FeatureUpdateViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
