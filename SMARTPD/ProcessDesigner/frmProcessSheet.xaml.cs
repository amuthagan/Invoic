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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProcessDesigner.BLL;
using ProcessDesigner.ViewModel;
using ProcessDesigner.Common;
using System.ComponentModel;
using System.Data;
using ProcessDesigner.UserControls;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmProcessSheet.xaml
    /// </summary>
    public partial class frmProcessSheet : UserControl
    {
        ProcessSheetViewModel vm;
        public frmProcessSheet(WPF.MDI.MdiChild me, UserInformation userinfo)
        {

            InitializeComponent();
            vm = new ProcessSheetViewModel(userinfo, me);
            vm.DgrdProcessSheet = dgrdProcessSheet;
            vm.Sort = "asc";
            this.DataContext = vm;
            me.Closing += vm.CloseMethod;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());

        }

        public frmProcessSheet(WPF.MDI.MdiChild me, UserInformation userinfo, string productNo, string productDesc)
        {
            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            InitializeComponent();
            ProcessSheetViewModel vm = new ProcessSheetViewModel(userinfo, productNo, productDesc, me);
            this.DataContext = vm;
            vm.DgrdProcessSheet = dgrdProcessSheet;
            //cmbPartNo.ButtonVisibility = Visibility.Collapsed;
            me.Closing += vm.CloseMethod;
            Progress.End();
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => me.Close());

        }

        private void dgrdProcessSheet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgrdProcessSheet_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdProcessSheet.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)

                {
                   
                    if (columnDisplayIndex == 0)
                    {

                        dgrdProcessSheet.SelectedIndex = dgrdProcessSheet.SelectedIndex - 1;
                        columnDisplayIndex = dgrdProcessSheet.Columns.Count - 1;
                    }
                    else
                    {
                        if (columnDisplayIndex == 1)
                        {
                            columnDisplayIndex = 0;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex - 2;
                        }
                    }
                    Microsoft.Windows.Controls.DataGridColumn nextColumn = dgrdProcessSheet.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdProcessSheet.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(dgrdProcessSheet.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdProcessSheet.BeginEdit();

                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab || e.Key == Key.Right)
                    {

                        if (columnDisplayIndex == 7)
                        {
                            columnDisplayIndex = 0;
                            dgrdProcessSheet.SelectedIndex = dgrdProcessSheet.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;

                        selectedIndex = dgrdProcessSheet.SelectedIndex;

                        Microsoft.Windows.Controls.DataGridColumn nextColumn = dgrdProcessSheet.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdProcessSheet.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(dgrdProcessSheet.SelectedItem, nextColumn);
                        dgrdProcessSheet.ScrollIntoView(dgrdProcessSheet.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdProcessSheet.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void dgrdProcessSheet_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void dgrdProcessCC_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdProcessCC.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                    if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    if (columnDisplayIndex == 0)
                    {

                        dgrdProcessCC.SelectedIndex = dgrdProcessCC.SelectedIndex - 1;
                        columnDisplayIndex = dgrdProcessCC.Columns.Count - 1;
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
                    Microsoft.Windows.Controls.DataGridColumn nextColumn = dgrdProcessCC.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdProcessCC.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(dgrdProcessCC.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdProcessCC.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 3)
                        {
                            columnDisplayIndex = 0;
                            dgrdProcessCC.SelectedIndex = dgrdProcessCC.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgrdProcessCC.SelectedIndex;

                        Microsoft.Windows.Controls.DataGridColumn nextColumn = dgrdProcessCC.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdProcessCC.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(dgrdProcessCC.SelectedItem, nextColumn);
                        dgrdProcessCC.ScrollIntoView(dgrdProcessCC.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdProcessCC.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgrdProcessIssue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdProcessIssue.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    if (columnDisplayIndex == 0)
                    {

                        dgrdProcessIssue.SelectedIndex = dgrdProcessIssue.SelectedIndex - 1;
                        columnDisplayIndex = dgrdProcessIssue.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgrdProcessIssue.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdProcessIssue.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdProcessIssue.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdProcessIssue.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                        if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 3)
                        {
                            columnDisplayIndex = 0;
                            dgrdProcessIssue.SelectedIndex = dgrdProcessIssue.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgrdProcessIssue.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgrdProcessIssue.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdProcessIssue.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdProcessIssue.SelectedItem, nextColumn);
                        dgrdProcessIssue.ScrollIntoView(dgrdProcessIssue.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdProcessIssue.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //private void dgrdProcessIssue_Sorting(object sender, DataGridSortingEventArgs e)
        //{
        //    e.Handled = true;

        //    var dataView = CollectionViewSource.GetDefaultView(this.dgrdProcessIssue.ItemsSource);
        //    dataView.SortDescriptions.Clear();
        //    if (vm.Sort == "" || vm.Sort == "asc")
        //    {
        //        dataView.SortDescriptions.Add(new SortDescription("Issue_NoNumeric", ListSortDirection.Descending));
        //        vm.Sort = "desc";
        //    }
        //    else
        //    {
        //        vm.Sort = "asc";
        //        dataView.SortDescriptions.Add(new SortDescription("Issue_NoNumeric", ListSortDirection.Ascending));
        //    }
        //    // multi-level sort could add more sort descriptions here
        //    dataView.Refresh();
        //    //this.dgrdProcessIssue.ItemsSource = dataView;
        //}


    }
}
