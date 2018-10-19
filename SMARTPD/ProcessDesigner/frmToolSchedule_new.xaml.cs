using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;
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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmToolSchedule_new.xaml
    /// </summary>
    public partial class frmToolSchedule_new : UserControl
    {

        private WPF.MDI.MdiChild me;
        ViewModel.ToolScheduleViewModel tsmvm;
        public frmToolSchedule_new(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                Thread.CurrentThread.CurrentCulture = ci;

                //CostCenterMasterDet costcenter = new CostCenterMasterDet(userInformation);
                InitializeComponent();
                tsmvm = new ViewModel.ToolScheduleViewModel(userInformation, mdiChild);
                this.DataContext = tsmvm;
                this.me = mdiChild;
                this.me.Closing += tsmvm.CloseMethod;
                tsmvm.CmbSubHeadingCombo = cmbSeqHeading;
                tsmvm.DgvToolSchedule = dgvToolSchedule;
                tsmvm.DgvAuxTools = dgvAuxTools;
                tsmvm.DgvToolsScheduleRev = dgvToolsScheduleRev;
                if (tsmvm.CloseAction == null)
                    tsmvm.CloseAction = new Action(() => mdiChild.Close());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public frmToolSchedule_new(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, string partNo)
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
                tsmvm = new ViewModel.ToolScheduleViewModel(userInformation, mdiChild);
                this.DataContext = tsmvm;
                this.me = mdiChild;
                this.me.Closing += tsmvm.CloseMethod;
                tsmvm.CmbSubHeadingCombo = cmbSeqHeading;
                tsmvm.DgvToolSchedule = dgvToolSchedule;
                tsmvm.DgvAuxTools = dgvAuxTools;
                tsmvm.DgvToolsScheduleRev = dgvToolsScheduleRev;
                Progress.End();
                if (tsmvm.CloseAction == null)
                    tsmvm.CloseAction = new Action(() => mdiChild.Close());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                tsmvm = null;
            }
            catch (Exception ex)
            {
                ex.LogException();

            }
        }


        private void cmbSeqHeading_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Insert)
            {
                tsmvm.InsertNewSubHeading();
                e.Handled = true;
                txtBotNote.Text = "";
                txtTopNote.Text = "";
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Delete)
            {
                tsmvm.DeleteNewSubHeading();
                e.Handled = true;
            }
        }

        public void EditSelectedPartNo(string partNo)
        {
            try
            {
                tsmvm.EditSelectedPartNo(partNo);
            }
            catch (Exception ex)
            {
                ex.LogException();

            }
        }

        private void dgvToolsScheduleRev_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                int columnDisplayIndex = dgvToolsScheduleRev.CurrentCell.Column.DisplayIndex;
                
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)) //original
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                {
                    //new
                    if (e.Key == Key.Left)
                    {
                        try
                        {
                            System.Windows.Controls.DataGridCellInfo dgt = dgvToolSchedule.CurrentCell;
                            var cellContent = (TextBox)dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            if (cellContent != null)
                            {
                                if (cellContent.SelectionLength != cellContent.Text.Length)
                                {
                                    return;
                                }
                                //new by me

                                //    TextBox 
                                //end new by me
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                    //end new

                    e.Handled = true;
                    columnDisplayIndex = dgvToolsScheduleRev.CurrentCell.Column.DisplayIndex;
                    if (columnDisplayIndex == 0)
                    {
                        if (dgvToolsScheduleRev.SelectedIndex != 0)
                        {
                            dgvToolsScheduleRev.SelectedIndex = dgvToolsScheduleRev.SelectedIndex - 1;
                            columnDisplayIndex = dgvToolsScheduleRev.Columns.Count - 1;
                        }
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgvToolsScheduleRev.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgvToolsScheduleRev.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvToolsScheduleRev.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgvToolsScheduleRev.BeginEdit();
                }
                
                //else if (e.Key == Key.Enter || e.Key == Key.Tab) //original
                else if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Right)
                {
                    if (e.Key == Key.Right)
                    {
                        try
                        {
                            System.Windows.Controls.DataGridCellInfo dgt = dgvToolSchedule.CurrentCell;
                            var cellContent = (TextBox)dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            if (cellContent != null)
                            {
                                if (cellContent.SelectionLength != cellContent.Text.Length)
                                {
                                    return;
                                }

                                //    TextBox 
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                    //end add by nandakumar
                    columnDisplayIndex = columnDisplayIndex + 1;
                    if (columnDisplayIndex > dgvToolsScheduleRev.Columns.Count - 1)
                    {
                        columnDisplayIndex = 0;
                        if (dgvToolsScheduleRev.SelectedIndex <= dgvToolsScheduleRev.Items.Count - 2)
                        {
                            dgvToolsScheduleRev.SelectedIndex = dgvToolsScheduleRev.SelectedIndex + 1;
                        }
                        else
                        {
                            dgvToolsScheduleRev.SelectedIndex = dgvToolsScheduleRev.Items.Count - 1;
                            dgvToolsScheduleRev.Focus();
                            dgvToolsScheduleRev.CommitEdit();
                            System.Windows.Controls.DataGridColumn nextColumn1 = dgvToolsScheduleRev.ColumnFromDisplayIndex(columnDisplayIndex);
                            dgvToolsScheduleRev.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvToolsScheduleRev.SelectedItem, nextColumn1);
                            return;
                        }
                    }
                    System.Windows.Controls.DataGridColumn nextColumn = dgvToolsScheduleRev.ColumnFromDisplayIndex(columnDisplayIndex);
                    // now telling the grid, that we handled the key down event
                    e.Handled = true;
                    // setting the current cell (selected, focused)
                    dgvToolsScheduleRev.Focus();
                    dgvToolsScheduleRev.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvToolsScheduleRev.SelectedItem, nextColumn);
                    // tell the grid to initialize edit mode for the current cell
                    dgvToolsScheduleRev.BeginEdit();
                    

                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void dgvToolSchedule_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {


                int columnDisplayIndex = dgvToolSchedule.CurrentCell.Column.DisplayIndex;

                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left) //new
                {
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)) //original
                    //new
                     if (e.Key == Key.Left)
                    {
                        try
                        {
                            System.Windows.Controls.DataGridCellInfo dgt = dgvToolSchedule.CurrentCell;
                            var cellContent = (TextBox)dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            if (cellContent != null)
                            {
                                if (cellContent.SelectionLength != cellContent.Text.Length)
                                {
                                    return;
                                }
                                    //new by me
                               
                                //    TextBox 
                                //end new by me
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                //end new
                    e.Handled = true;
                    columnDisplayIndex = dgvToolSchedule.CurrentCell.Column.DisplayIndex;
                    if (columnDisplayIndex == 0)
                    {
                        if (dgvToolSchedule.SelectedIndex != 0)
                        {
                            dgvToolSchedule.SelectedIndex = dgvToolSchedule.SelectedIndex - 1;
                            columnDisplayIndex = dgvToolSchedule.Columns.Count - 1;
                        }
                        else
                        {
                            dgvToolSchedule.CommitEdit();
                        }
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgvToolSchedule.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgvToolSchedule.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvToolSchedule.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgvToolSchedule.BeginEdit();
                    //dgvToolSchedule.Focus();
                }
               
                //else if (e.Key == Key.Enter || e.Key == Key.Tab) //original
                else if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Right) //new
                {
                    //added by nandakumar
                    if (e.Key == Key.Right)
                    {
                        try
                        {
                            System.Windows.Controls.DataGridCellInfo dgt = dgvToolSchedule.CurrentCell;
                            var cellContent = (TextBox)dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            if (cellContent != null)
                            {
                                if (cellContent.SelectionLength != cellContent.Text.Length)
                                {
                                    return;
                                }

                                //    TextBox 
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                    //end add by nandakumar
                    if (columnDisplayIndex == 6)
                    {
                        columnDisplayIndex = 0;
                        dgvToolSchedule.SelectedIndex = dgvToolSchedule.SelectedIndex + 1;
                    }
                    else
                    {
                        columnDisplayIndex = columnDisplayIndex + 1;
                    }
                    int selectedIndex = 0;
                    selectedIndex = dgvToolSchedule.SelectedIndex;
                    
                    System.Windows.Controls.DataGridColumn nextColumn = dgvToolSchedule.ColumnFromDisplayIndex(columnDisplayIndex);
                    // now telling the grid, that we handled the key down event
                    e.Handled = true;
                    // setting the current cell (selected, focused)
                    dgvToolSchedule.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvToolSchedule.SelectedItem, nextColumn);
                    dgvToolSchedule.ScrollIntoView(dgvToolSchedule.CurrentCell);
                    // tell the grid to initialize edit mode for the current cell
                    dgvToolSchedule.BeginEdit();

                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void dgvAuxTools_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                int columnDisplayIndex = dgvAuxTools.CurrentCell.Column.DisplayIndex;
                
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))  //original
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                {
                    //new
                    if (e.Key == Key.Left)
                    {
                        try
                        {
                            System.Windows.Controls.DataGridCellInfo dgt = dgvToolSchedule.CurrentCell;
                            var cellContent = (TextBox)dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            if (cellContent != null)
                            {
                                if (cellContent.SelectionLength != cellContent.Text.Length)
                                {
                                    return;
                                }
                                //new by me

                                //    TextBox 
                                //end new by me
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                    //end new
                    e.Handled = true;
                    columnDisplayIndex = dgvAuxTools.CurrentCell.Column.DisplayIndex;
                    if (columnDisplayIndex == 0)
                    {

                        dgvAuxTools.SelectedIndex = dgvAuxTools.SelectedIndex - 1;
                        columnDisplayIndex = dgvAuxTools.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgvAuxTools.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgvAuxTools.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvAuxTools.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgvAuxTools.BeginEdit();
                }
               
                //else if (e.Key == Key.Enter || e.Key == Key.Tab) //original
                else if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Right)
                {
                    if (e.Key == Key.Right)
                    {
                        try
                        {
                            System.Windows.Controls.DataGridCellInfo dgt = dgvToolSchedule.CurrentCell;
                            var cellContent = (TextBox)dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            if (cellContent != null)
                            {
                                if (cellContent.SelectionLength != cellContent.Text.Length)
                                {
                                    return;
                                }

                                //    TextBox 
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                    //end add by nandakumar
                    //System.Windows.Controls.DataGridColumn nextColumn = dgvAuxTools.ColumnFromDisplayIndex(columnDisplayIndex);

                    //// now telling the grid, that we handled the key down event
                    ////e.Handled = true;

                    //// setting the current cell (selected, focused)
                    //dgvAuxTools.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvAuxTools.SelectedItem, nextColumn);

                    //// tell the grid to initialize edit mode for the current cell
                    //dgvAuxTools.BeginEdit();

                    if (columnDisplayIndex == 6)
                    {
                        columnDisplayIndex = 0;
                        dgvAuxTools.SelectedIndex = dgvAuxTools.SelectedIndex + 1;
                    }
                    else
                    {
                        columnDisplayIndex = columnDisplayIndex + 1;
                    }
                    int selectedIndex = 0;
                    selectedIndex = dgvAuxTools.SelectedIndex;

                    System.Windows.Controls.DataGridColumn nextColumn = dgvAuxTools.ColumnFromDisplayIndex(columnDisplayIndex);
                    // now telling the grid, that we handled the key down event
                    e.Handled = true;
                    // setting the current cell (selected, focused)
                    dgvAuxTools.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvAuxTools.SelectedItem, nextColumn);
                    dgvAuxTools.ScrollIntoView(dgvAuxTools.CurrentCell);
                    // tell the grid to initialize edit mode for the current cell
                    dgvAuxTools.BeginEdit();

                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
