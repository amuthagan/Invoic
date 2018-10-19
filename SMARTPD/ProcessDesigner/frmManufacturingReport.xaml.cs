using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;
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
using Xceed.Wpf.Toolkit;

namespace ProcessDesigner
{

    /// <summary>
    /// Interaction logic for frmManufacturingReport.xaml
    /// </summary>
    public partial class frmManufacturingReport : UserControl
    {
        public frmManufacturingReport(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;
            ManufacReportViewModel fm = new ManufacReportViewModel(userInformation, me);
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
        }
        public frmManufacturingReport(UserInformation userInformation, WPF.MDI.MdiChild me, string partNo)
        {

            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;
            ManufacReportViewModel fm = new ManufacReportViewModel(userInformation, me, partNo);
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            Progress.End();
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
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

        private void dgrdDesign_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdDesign.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 0)
                    {

                        dgrdDesign.SelectedIndex = dgrdDesign.SelectedIndex - 1;
                        columnDisplayIndex = dgrdDesign.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgrdDesign.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdDesign.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdDesign.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdDesign.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 3)
                        {
                            columnDisplayIndex = 0;
                            dgrdDesign.SelectedIndex = dgrdDesign.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgrdDesign.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgrdDesign.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdDesign.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdDesign.SelectedItem, nextColumn);
                        dgrdDesign.ScrollIntoView(dgrdDesign.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdDesign.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgrdDifficulties_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdDifficulties.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    if (columnDisplayIndex == 0)
                    {

                        dgrdDifficulties.SelectedIndex = dgrdDifficulties.SelectedIndex - 1;
                        columnDisplayIndex = dgrdDifficulties.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgrdDifficulties.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdDifficulties.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdDifficulties.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdDifficulties.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {
                        if (columnDisplayIndex == 3)
                        {
                            columnDisplayIndex = 0;
                            dgrdDifficulties.SelectedIndex = dgrdDifficulties.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgrdDifficulties.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgrdDifficulties.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdDifficulties.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdDifficulties.SelectedItem, nextColumn);
                        dgrdDifficulties.ScrollIntoView(dgrdDifficulties.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdDifficulties.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgrdPreQual_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdPreQual.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 0)
                    {

                        dgrdPreQual.SelectedIndex = dgrdPreQual.SelectedIndex - 1;
                        columnDisplayIndex = dgrdPreQual.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgrdPreQual.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdPreQual.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdPreQual.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdPreQual.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 1)
                        {
                            columnDisplayIndex = 0;
                            dgrdPreQual.SelectedIndex = dgrdPreQual.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgrdPreQual.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgrdPreQual.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdPreQual.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdPreQual.SelectedItem, nextColumn);
                        dgrdPreQual.ScrollIntoView(dgrdPreQual.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdPreQual.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgrdProcess_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdProcess.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 0)
                    {

                        dgrdProcess.SelectedIndex = dgrdProcess.SelectedIndex - 1;
                        columnDisplayIndex = dgrdProcess.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgrdProcess.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdProcess.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdProcess.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdProcess.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 5)
                        {
                            columnDisplayIndex = 0;
                            dgrdProcess.SelectedIndex = dgrdProcess.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgrdProcess.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgrdProcess.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdProcess.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdProcess.SelectedItem, nextColumn);
                        dgrdProcess.ScrollIntoView(dgrdProcess.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdProcess.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgrdOutput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgrdOutput.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 0)
                    {
                        dgrdOutput.SelectedIndex = dgrdOutput.SelectedIndex - 1;
                        columnDisplayIndex = dgrdOutput.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgrdOutput.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgrdOutput.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdOutput.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgrdOutput.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 5)
                        {
                            columnDisplayIndex = 0;
                            dgrdOutput.SelectedIndex = dgrdOutput.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgrdOutput.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgrdOutput.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgrdOutput.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgrdOutput.SelectedItem, nextColumn);
                        dgrdOutput.ScrollIntoView(dgrdOutput.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgrdOutput.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void DateTimePicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

    }
}




