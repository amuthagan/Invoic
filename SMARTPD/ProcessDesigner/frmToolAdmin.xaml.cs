using ProcessDesigner.BLL;
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
using PreviewHandlers;
using ProcessDesigner.Common;


namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmToolAdministration.xaml
    /// </summary>
    public partial class frmToolAdmin : UserControl
    {
        public frmToolAdmin(WPF.MDI.MdiChild me, UserInformation userInfo)
        {
            InitializeComponent();
            ToolAdminViewModel vm = new ToolAdminViewModel(userInfo);
            this.DataContext = vm;
            me.Closing += vm.CloseMethod;
            //vm.PreviewDrawing = imgToolAdmin;
            vm.PreviewImage = imgPhoto;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //canParameters.Children.Add(imgHost);
                ((TextBox)cmbFamilyCode.FindName("txtCombobox")).Focus();
                //this.imgToolAdmin.DisplayType = DisplayType.DisplayContent;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }


            //imgToolAdmin.Controls.Add(new System.Windows.Forms.TextBox());
            //this.imgToolAdmin.CreateLink("E:\\ConeNew.vsd");
            //this.imgToolAdmin.SendToBack();
            //this.imgToolAdmin.CreateLink("E:\\Drawing1.dwg");

            ////        this.previewContainer.DisplayType =
            ////            this.displayTypeContentRadioButton.Checked
            ////            ? DisplayType.DisplayContent
            ////: DisplayType.DisplayIcon;

            ////        this.previewContainer.CreateLink(pathToDocumentTextBox.Text);

        }


    }
}
