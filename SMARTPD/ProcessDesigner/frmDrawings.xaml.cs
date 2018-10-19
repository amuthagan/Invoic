using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmDrawings.xaml
    /// </summary>
    public partial class frmDrawings : UserControl
    {
        public int DrawingsMaster { get; set; }
        private string InputMessage { get; set; }
        //private WPF.MDI.MdiChild me;
        private WPF.MDI.MdiChild mdiChild;
        private DrawingViewModel vm = null;
        public frmDrawings(WPF.MDI.MdiChild me, UserInformation userInformation)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                Thread.CurrentThread.CurrentCulture = ci;
                InitializeComponent();
                vm = new DrawingViewModel(userInformation, me, DrawingsMaster, OperationMode.View);
                vm.DgvProdDwgMast = dgvProdDwgMast;
                this.DataContext = vm;
                mdiChild = me;
                this.mdiChild.Closing += vm.CloseMethod;
                if (vm.CloseAction == null)
                    vm.CloseAction = new Action(() => me.Close());
                //me.Position = new Point(0, 0);
                dgvProdDwgMast.Columns[0].Visibility = (vm.DrwModel.DWG_TYPE_DESC.ToValueAsString() == "Sequence Drawing") ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public frmDrawings(WPF.MDI.MdiChild me, UserInformation userInformation, string partNo)
        {
            try
            {
                Progress.ProcessingText = PDMsg.Load;
                Progress.Start();
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                Thread.CurrentThread.CurrentCulture = ci;
                InitializeComponent();
                vm = new DrawingViewModel(userInformation, me, DrawingsMaster, OperationMode.View);
                vm.DgvProdDwgMast = dgvProdDwgMast;
                this.DataContext = vm;
                if (vm.CloseAction == null)
                    vm.CloseAction = new Action(() => me.Close());
                //me.Position = new Point(0, 0);
                mdiChild = me;
                this.mdiChild.Closing += vm.CloseMethod;
                vm.EditSelectedPartNo(partNo);
                dgvProdDwgMast.Columns[0].Visibility = (vm.DrwModel.DWG_TYPE_DESC.ToValueAsString() == "Sequence Drawing") ? Visibility.Visible : Visibility.Collapsed;
                Progress.End();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void cmbDrwType_SelectedItemChanged(object sender, EventArgs e)
        {
            dgvProdDwgMast.Columns[0].Visibility = (vm.DrwModel.DWG_TYPE_DESC.ToValueAsString() == "Sequence Drawing") ? Visibility.Visible : Visibility.Collapsed;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)cmbPartNo.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void btnECN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgvProdDwgMast_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgvProdDwgMast.CurrentCell.Column.DisplayIndex;

                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    int leftDisplayIndex = 0;

                    if (vm.DrwModel.DWG_TYPE_DESC.ToValueAsString() == "Sequence Drawing")
                        leftDisplayIndex = 0;
                    else
                        leftDisplayIndex = 1;

                    if (columnDisplayIndex == leftDisplayIndex)
                    {

                        dgvProdDwgMast.SelectedIndex = dgvProdDwgMast.SelectedIndex - 1;
                        columnDisplayIndex = dgvProdDwgMast.Columns.Count - 1;
                    }
                    else
                    {
                        if (columnDisplayIndex == 1)
                        {
                            columnDisplayIndex = 0;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex - 1;
                        }
                    }
                    System.Windows.Controls.DataGridColumn nextColumn = dgvProdDwgMast.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgvProdDwgMast.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvProdDwgMast.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgvProdDwgMast.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {
                        int displayIndex = 0;

                        if (vm.DrwModel.DWG_TYPE_DESC.ToValueAsString() == "Sequence Drawing")
                            displayIndex = 0;
                        else
                            displayIndex = 1;

                        if (columnDisplayIndex == 4)
                        {
                            columnDisplayIndex = displayIndex;
                            dgvProdDwgMast.SelectedIndex = dgvProdDwgMast.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgvProdDwgMast.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgvProdDwgMast.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgvProdDwgMast.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvProdDwgMast.SelectedItem, nextColumn);
                        dgvProdDwgMast.ScrollIntoView(dgvProdDwgMast.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgvProdDwgMast.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
