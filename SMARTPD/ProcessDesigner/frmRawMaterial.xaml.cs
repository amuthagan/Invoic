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
using ProcessDesigner.UserControls;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmRawMaterial.xaml
    /// </summary>
    public partial class frmRawMaterial : UserControl
    {
        private RawMaterial bll = null;
        public int RawMaterialCode { get; set; }
        private string InputMessage { get; set; }

        public frmRawMaterial(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            InitializeComponent();
            ltbRmCode.Focus();

            RawMaterialCode = -99999;
            RawMaterialViewMode vm = new RawMaterialViewMode(userInformation, RawMaterialCode, OperationMode.AddNew);
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => mdiChild.Close());

            bll = new RawMaterial(userInformation);

            List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("DDRM_MAST");
            this.SetColumnLength<TextBox>(lstTableDescription);

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ltbRmCode.Focus();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {

            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)ltbRmCode.FindName("txtCombobox")).Focus();
                //List<V_TABLE_DESCRIPTION> cols = bllRawMaterial.GetTableColumnsSize("DDRM_MAST");
                //this.SetColumnLength<TextBox>(cols);

                //RolePermission permission = bllRawMaterial.GetUserRights(this.Tag.ToValueAsString());
                //this.SetUserRights(permission, AddNew, Edit, null, null, null);

                //ltbRmCode.DataSource = bllRawMaterial.GetRawMaterials().ToDataTable<DDRM_MAST>().DefaultView;
                //ssRawMaterial.ItemsSource = bllRawMaterial.GetRawMaterialsSize("");

                //        CenterForm Me, 6585, 5085
                //        fnLoadToolBar tbrAction, "Add", "Edit", "Delete", "Save", "Print", "Close"
                //        GridRefresh ddLocation, "Select loc_code,location from ddloc_mast"
                //        Set grdRmSize.Grid = ssRawMaterial

                //        Call fnCCSSetUserRights(Me, Me.Tag, gvarAdd, gvarEdit, gvarView, gvarDel, gvarPrint)

                //        If gvarAdd = True Then
                //          frmAdd
                //        End If

                //        If gvarAdd = False And gvarEdit = False Then
                //         frmEdit
                //        End If
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {

            }
        }
       
        //private void Close_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (mdiChild.IsNotNullOrEmpty())
        //        {
        //            mdiChild.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            Control cntrl = (Control)sender;
            string s = sender.GetType().ToString();

            //Button button = sender as Button;
            //if (button == null)
            //{
            //    return;
            //}

            MessageBox.Show(e.Source.ToString());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Control cntrl = (Control)sender;
                string s = sender.GetType().ToString();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ClearAll()
        {
            try
            {
                this.ClearAllControl<TextBox>();
                this.ClearAllControl<ComboBoxCus>();
                this.ClearAllControl<DataGrid>();

                //ltbRmCode.DataSource = dm.AsEnumerable<DDRM_MAST>();
                if (ssRawMaterial.DataContext == null)
                {

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Close_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ssRawMaterial_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void txtDomesticCost_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }

}
