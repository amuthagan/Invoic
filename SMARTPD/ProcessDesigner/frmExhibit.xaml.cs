using ProcessDesigner.BLL;
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
    /// Interaction logic for frmExhibit.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmExhibit : UserControl
    {
        public frmExhibit(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            ExhibitViewModel fu = new ExhibitViewModel(userInformation);
            this.DataContext = fu;
            me.Closing += fu.CloseMethod;


            this.DataContext = new ExhibitViewModel(userInformation);
        }
        /// <summary>
        /// Sets the ViewModel.
        /// </summary>
        /// <remarks>
        /// This set-only property is annotated with the <see cref="ImportAttribute"/> so it is injected by MEF with
        /// the appropriate view model.
        /// </remarks>
        [Import]
        ExhibitViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        private void usrDocument_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
