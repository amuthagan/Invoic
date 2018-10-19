using ProcessDesigner.Common;
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
    /// Interaction logic for frmProductReleaseDate.xaml
    /// </summary>
    public partial class frmProductReleaseDate : UserControl
    {
        ProductReleaseDateViewModel vm = null;
        public frmProductReleaseDate(UserInformation userInformation, System.Windows.Window window, ProductInformationModel productInformationModel,
    ProcessDesigner.Common.OperationMode operationMode, string title = "Document Release Date")
        {
            InitializeComponent();

            vm = new ProductReleaseDateViewModel(userInformation, window, productInformationModel, operationMode, title);
            this.DataContext = vm;
            if (vm.CloseAction == null && window.IsNotNullOrEmpty())
                vm.CloseAction = new Action(() => window.Close());
        }
    }
}
