using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for frmFRCS.xaml
    /// </summary>
    public partial class frmFRCS : UserControl
    {
        private string InputMessage { get; set; }
        FeasibleReportAndCostSheetViewModel vm = null;
        FeasibleReportAndCostSheet bll = null;
        private WPF.MDI.MdiChild mdiChild;
        public frmFRCS(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string title = "Feasibility Report and Cost Sheet")
        {

           // Progress.ProcessingText = PDMsg.Load;
            //Progress.Start();
            InitializeComponent();

            this.mdiChild = mdiChild;
            vm = new FeasibleReportAndCostSheetViewModel(userInformation, mdiChild, entityPrimaryKey, operationMode, title);
            //vm.SsCostDetails = ssCostDetails;
            //Progress.End();
            this.DataContext = vm;
            mdiChild.Closing += vm.CloseMethod;

            if (vm.CloseAction == null && mdiChild.IsNotNullOrEmpty())
                vm.CloseAction = new Action(() => mdiChild.Close());

            bll = new FeasibleReportAndCostSheet(userInformation);

            List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("DDCI_INFO");
            this.SetColumnLength<TextBox>(lstTableDescription);

        }

        public frmFRCS(UserInformation userInformation, System.Windows.Window window, int entityPrimaryKey,
    OperationMode operationMode, string title = "Feasibility Report and Cost Sheet")
        {
            InitializeComponent();

            vm = new FeasibleReportAndCostSheetViewModel(userInformation, mdiChild, entityPrimaryKey, operationMode, title);
            this.DataContext = vm;
            window.Closing += vm.CloseMethodWindow;
            if (vm.CloseAction == null && window.IsNotNullOrEmpty())
                vm.CloseAction = new Action(() => window.Close());

            bll = new FeasibleReportAndCostSheet(userInformation);

            List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = bll.GetTableColumnsSize("DDCI_INFO");
            this.SetColumnLength<TextBox>(lstTableDescription);
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // dtpDateRecd.SelectedDate = DateTime.Now;
            dtpDateRecd.Focus();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkFeasible_Click(object sender, RoutedEventArgs e)
        {
            chkFeasible.IsChecked = true;
        }

        private void ssCostDetails_KeyDown(object sender, KeyEventArgs e)
        {

            //try
            //{
            //    if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            //    {
            //        e.Handled = true;
            //        int columnDisplayIndex = ssCostDetails.CurrentCell.Column.DisplayIndex;
            //        if (columnDisplayIndex == 0)
            //        {

            //            ssCostDetails.SelectedIndex = ssCostDetails.SelectedIndex - 1;
            //            columnDisplayIndex = ssCostDetails.Columns.Count - 1;
            //        }
            //        else
            //        {
            //            if (columnDisplayIndex == 1)
            //            {
            //                columnDisplayIndex = 0;
            //            }
            //            else
            //            {
            //                columnDisplayIndex = columnDisplayIndex - 1;
            //            }
            //        }
            //        Microsoft.Windows.Controls.DataGridColumn nextColumn = ssCostDetails.ColumnFromDisplayIndex(columnDisplayIndex);

            //        // now telling the grid, that we handled the key down event
            //        //e.Handled = true;

            //        // setting the current cell (selected, focused)
            //        ssCostDetails.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(ssCostDetails.SelectedItem, nextColumn);

            //        // tell the grid to initialize edit mode for the current cell
            //        ssCostDetails.BeginEdit();
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void ssCostDetails_KeyUp(object sender, KeyEventArgs e)
        {

            //try
            //{
            //    if ((e.Key == Key.Tab && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))) || e.Key == Key.Enter)
            //    {

            //        int columnDisplayIndex = 0;
            //        Microsoft.Windows.Controls.DataGrid grid = (Microsoft.Windows.Controls.DataGrid)sender;
            //        //grid.SelectedIndex = 0;
            //        // get the selected row
            //        var selectedRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as Microsoft.Windows.Controls.DataGridRow;

            //        // selectedRow can be null due to virtualization
            //        if (selectedRow != null)
            //        {
            //            // there should always be a selected cell
            //            if (grid.SelectedCells.Count != 0)
            //            {
            //                columnDisplayIndex = grid.CurrentCell.Column.DisplayIndex;
            //                if (columnDisplayIndex == 0)
            //                {
            //                    columnDisplayIndex = 1;
            //                }
            //                DataRowView drv = (DataRowView)grid.SelectedItem;
            //                if (!(drv["SNO"]).ToString().IsNotNullOrEmpty())
            //                {
            //                    columnDisplayIndex = 0;
            //                }

            //                if (columnDisplayIndex < grid.Columns.Count)
            //                {
            //                    // get the DataGridColumn instance from the display index
            //                    Microsoft.Windows.Controls.DataGridColumn nextColumn = grid.ColumnFromDisplayIndex(columnDisplayIndex);

            //                    // now telling the grid, that we handled the key down event
            //                    //e.Handled = true;

            //                    // setting the current cell (selected, focused)
            //                    grid.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(grid.SelectedItem, nextColumn);

            //                    // tell the grid to initialize edit mode for the current cell
            //                    grid.BeginEdit();
            //                }
            //            }
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{
            //    ex.LogException();
            //}
        }

        private void ssCostDetails_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = ssCostDetails.CurrentCell.Column.DisplayIndex;
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 0)
                    {

                        ssCostDetails.SelectedIndex = ssCostDetails.SelectedIndex - 1;
                        columnDisplayIndex = ssCostDetails.Columns.Count - 1;
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
                    Microsoft.Windows.Controls.DataGridColumn nextColumn = ssCostDetails.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    ssCostDetails.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(ssCostDetails.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    ssCostDetails.BeginEdit();
                }
                else
                {
                    if (e.Key == Key.Tab || e.Key == Key.Right)
                    {

                        if (columnDisplayIndex == 8)
                        {
                            columnDisplayIndex = 0;
                            ssCostDetails.SelectedIndex = ssCostDetails.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = ssCostDetails.SelectedIndex;

                        Microsoft.Windows.Controls.DataGridColumn nextColumn = ssCostDetails.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        ssCostDetails.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(ssCostDetails.SelectedItem, nextColumn);
                        ssCostDetails.ScrollIntoView(ssCostDetails.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        ssCostDetails.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            //try
            //{
            //    if ((e.Key == Key.Tab && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))) || e.Key == Key.Enter)
            //    {

            //        int columnDisplayIndex = 0;
            //        Microsoft.Windows.Controls.DataGrid grid = (Microsoft.Windows.Controls.DataGrid)sender;
            //        //grid.SelectedIndex = 0;
            //        // get the selected row
            //        var selectedRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as Microsoft.Windows.Controls.DataGridRow;

            //        // selectedRow can be null due to virtualization
            //        if (selectedRow != null)
            //        {
            //            // there should always be a selected cell
            //            if (grid.SelectedCells.Count != 0)
            //            {
            //                columnDisplayIndex = grid.CurrentCell.Column.DisplayIndex;
            //                if (columnDisplayIndex == 0)
            //                {
            //                    columnDisplayIndex = 1;
            //                }
            //                DataRowView drv = (DataRowView)grid.SelectedItem;
            //                if (!(drv["SNO"]).ToString().IsNotNullOrEmpty())
            //                {
            //                    columnDisplayIndex = 0;
            //                }
            //                if (((drv["SNO"]).ToString().IsNotNullOrEmpty()) && !((drv["OPERATION_NO"]).ToString().IsNotNullOrEmpty()))
            //                {
            //                    columnDisplayIndex = 0;
            //                }
            //                if (columnDisplayIndex < grid.Columns.Count)
            //                {
            //                    // get the DataGridColumn instance from the display index
            //                    Microsoft.Windows.Controls.DataGridColumn nextColumn = grid.ColumnFromDisplayIndex(columnDisplayIndex);

            //                    // now telling the grid, that we handled the key down event
            //                    //e.Handled = true;

            //                    // setting the current cell (selected, focused)
            //                    grid.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(grid.SelectedItem, nextColumn);

            //                    // tell the grid to initialize edit mode for the current cell
            //                    grid.BeginEdit();
            //                }
            //            }
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{
            //    ex.LogException();
            //}

        }

        //private void ssCostDetails_CurrentCellChanged(object sender, EventArgs e)
        //{
        //    return;
        //    try
        //    {
        //        Microsoft.Windows.Controls.DataGrid grid = (Microsoft.Windows.Controls.DataGrid)sender;
        //        grid.SelectedIndex = 0;
        //        // get the selected row
        //        var selectedRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as Microsoft.Windows.Controls.DataGridRow;

        //        // selectedRow can be null due to virtualization
        //        if (selectedRow != null)
        //        {
        //            // there should always be a selected cell
        //            if (grid.SelectedCells.Count != 0)
        //            {
        //                // get the cell info
        //                Microsoft.Windows.Controls.DataGridCellInfo currentCell = grid.CurrentCell;

        //                // get the display index of the cell's column + 1 (for next column)
        //                //columnDisplayIndex = currentCell.Column.DisplayIndex++;
        //                //columnDisplayIndex = 4;
        //                columnDisplayIndex = grid.CurrentCell.Column.DisplayIndex;
        //                if (columnDisplayIndex == 0)
        //                {
        //                    columnDisplayIndex = 1;
        //                }
        //                else if (columnDisplayIndex == 1)
        //                {
        //                    //return;
        //                    columnDisplayIndex = 3;
        //                }
        //                else if (columnDisplayIndex == 3)
        //                {
        //                    columnDisplayIndex = 4;
        //                }
        //                else if (columnDisplayIndex == 4)
        //                {
        //                    ssCostDetails.Focus();
        //                    columnDisplayIndex = 5;
        //                }
        //                else if (columnDisplayIndex == 5)
        //                {
        //                    columnDisplayIndex = 6;
        //                }
        //                else if (columnDisplayIndex == 7)
        //                {
        //                    columnDisplayIndex = 8;
        //                }
        //                else if (columnDisplayIndex == 8)
        //                {
        //                    columnDisplayIndex = 9;
        //                }
        //                else
        //                {
        //                    //return;
        //                }

        //                //if (columnDisplayIndex == 2)
        //                //{
        //                //    columnDisplayIndex = 3;
        //                //}

        //                //columnDisplayIndex = grid.CurrentCell.Column.DisplayIndex + 1;

        //                // if display index is valid
        //                if (columnDisplayIndex < grid.Columns.Count)
        //                {
        //                    // get the DataGridColumn instance from the display index
        //                    Microsoft.Windows.Controls.DataGridColumn nextColumn = grid.ColumnFromDisplayIndex(columnDisplayIndex);

        //                    // now telling the grid, that we handled the key down event
        //                    //e.Handled = true;

        //                    // setting the current cell (selected, focused)
        //                    grid.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(grid.SelectedItem, nextColumn);

        //                    // tell the grid to initialize edit mode for the current cell
        //                    grid.BeginEdit();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.LogException();
        //    }
        //}

        //private void ssCostDetails_PreviewKeyDownOld(object sender, KeyEventArgs e)
        //{
        //    int a;
        //    a = 0;

        //}
        //int columnDisplayIndex;

        //private void ssCostDetails_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    return;
        //    Microsoft.Windows.Controls.DataGrid grid = (Microsoft.Windows.Controls.DataGrid)sender;
        //    try
        //    {
        //        if (e.Key == Key.Tab)
        //        {
        //            // get the selected row
        //            grid.SelectedIndex = 0;
        //            var selectedRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as Microsoft.Windows.Controls.DataGridRow;

        //            // selectedRow can be null due to virtualization
        //            if (selectedRow != null)
        //            {
        //                // there should always be a selected cell
        //                if (grid.SelectedCells.Count != 0)
        //                {
        //                    // get the cell info
        //                    Microsoft.Windows.Controls.DataGridCellInfo currentCell = grid.CurrentCell;

        //                    // get the display index of the cell's column + 1 (for next column)
        //                    //columnDisplayIndex = currentCell.Column.DisplayIndex++;
        //                    //columnDisplayIndex = 4;
        //                    if (columnDisplayIndex < 4)
        //                    {
        //                        return;
        //                    }
        //                    if (columnDisplayIndex == 4)
        //                    {
        //                        columnDisplayIndex = 4;
        //                    }
        //                    else if (columnDisplayIndex == 5)
        //                    {
        //                        columnDisplayIndex = 5;
        //                    }
        //                    else if (columnDisplayIndex == 8)
        //                    {
        //                        columnDisplayIndex = 9;
        //                    }
        //                    else
        //                    {
        //                        return;
        //                    }

        //                    //if (columnDisplayIndex == 2)
        //                    //{
        //                    //    columnDisplayIndex = 3;
        //                    //}

        //                    //columnDisplayIndex = grid.CurrentCell.Column.DisplayIndex + 1;

        //                    // if display index is valid
        //                    if (columnDisplayIndex < grid.Columns.Count)
        //                    {
        //                        // get the DataGridColumn instance from the display index
        //                        Microsoft.Windows.Controls.DataGridColumn nextColumn = grid.ColumnFromDisplayIndex(columnDisplayIndex);

        //                        // now telling the grid, that we handled the key down event
        //                        e.Handled = true;

        //                        // setting the current cell (selected, focused)
        //                        grid.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(grid.SelectedItem, nextColumn);

        //                        // tell the grid to initialize edit mode for the current cell
        //                        grid.BeginEdit();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.LogException();
        //    }
        //}

        //private void ssCostDetails_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    Microsoft.Windows.Controls.DataGrid grid = ssCostDetails;
        //    try
        //    {
        //        if (e.Key == Key.Tab)
        //        {

        //            //grid.SelectedIndex = 0;
        //            // get the selected row
        //            var selectedRow = grid.ItemContainerGenerator.ContainerFromItem(grid.Items[currentRowIndex]) as Microsoft.Windows.Controls.DataGridRow;

        //            // selectedRow can be null due to virtualization
        //            if (selectedRow != null)
        //            {
        //                // there should always be a selected cell
        //                if (grid.SelectedCells.Count != 0)
        //                {
        //                    // get the cell info
        //                    Microsoft.Windows.Controls.DataGridCellInfo currentCell = grid.CurrentCell;

        //                    // get the display index of the cell's column + 1 (for next column)
        //                    //columnDisplayIndex = currentCell.Column.DisplayIndex++;
        //                    //columnDisplayIndex = 4;
        //                    columnDisplayIndex = grid.CurrentCell.Column.DisplayIndex;
        //                    if (columnDisplayIndex == 0)
        //                    {
        //                        columnDisplayIndex = 1;
        //                    }
        //                    else if (columnDisplayIndex == 1)
        //                    {
        //                        //return;
        //                        columnDisplayIndex = 3;
        //                    }
        //                    else if (columnDisplayIndex == 3)
        //                    {
        //                        grid.CommitEdit();
        //                        columnDisplayIndex = 4;
        //                    }
        //                    else if (columnDisplayIndex == 4)
        //                    {
        //                        //ssCostDetails.Focus();
        //                        grid.CommitEdit();
        //                        columnDisplayIndex = 5;
        //                    }
        //                    else if (columnDisplayIndex == 5)
        //                    {
        //                        grid.CommitEdit();
        //                        columnDisplayIndex = 6;
        //                    }
        //                    else if (columnDisplayIndex == 6)
        //                    {
        //                        grid.CommitEdit();
        //                        columnDisplayIndex = 7;
        //                    }
        //                    else if (columnDisplayIndex == 7)
        //                    {
        //                        grid.CommitEdit();
        //                        columnDisplayIndex = 8;
        //                    }
        //                    else if (columnDisplayIndex == 8)
        //                    {
        //                        grid.CommitEdit();
        //                        columnDisplayIndex = 9;
        //                    }
        //                    else
        //                    {
        //                        //return;
        //                    }

        //                    //if (columnDisplayIndex == 2)
        //                    //{
        //                    //    columnDisplayIndex = 3;
        //                    //}

        //                    //columnDisplayIndex = grid.CurrentCell.Column.DisplayIndex + 1;

        //                    // if display index is valid
        //                    if (columnDisplayIndex < grid.Columns.Count)
        //                    {
        //                        // get the DataGridColumn instance from the display index
        //                        Microsoft.Windows.Controls.DataGridColumn nextColumn = grid.ColumnFromDisplayIndex(columnDisplayIndex);

        //                        // now telling the grid, that we handled the key down event
        //                        e.Handled = true;

        //                        // setting the current cell (selected, focused)
        //                        grid.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(grid.Items[currentRowIndex], nextColumn);

        //                        // tell the grid to initialize edit mode for the current cell
        //                        grid.BeginEdit();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.LogException();
        //    }
        //}
        //int currentRowIndex = 0;
        //private void ssCostDetails_BeginningEdit(object sender, Microsoft.Windows.Controls.DataGridBeginningEditEventArgs e)
        //{
        //    currentRowIndex = ssCostDetails.SelectedIndex;
        //}
    }
}
