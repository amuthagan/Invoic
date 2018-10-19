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


namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmPCCS.xaml
    /// </summary>
    public partial class frmPCCS : UserControl
    {
        //WPF.MDI.MdiChild me;
        public frmPCCS(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
            PCCSViewModel fm = new PCCSViewModel(userInformation, me);
            fm.DgvPccs = dgvPccs;
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
        }
        public frmPCCS(UserInformation userInformation, WPF.MDI.MdiChild me, string partNo)
        {

            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
            // PCCSViewModel fm = new PCCSViewModel(userInformation);
            PCCSViewModel fm = new PCCSViewModel(userInformation, partNo, me);
            fm.DgvPccs = dgvPccs;
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

        private void dgvPccs_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                int columnDisplayIndex = dgvPccs.CurrentCell.Column.DisplayIndex;
                
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)) //original

                    if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                    
                {
                    if (e.Key == Key.Left)
                    {
                        try
                        {
                            Microsoft.Windows.Controls.DataGridCellInfo dgt = dgvPccs.CurrentCell;
                            var cellContent = dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            //if (cellContent != null)
                            //{
                            //    if (cellContent.SelectionLength != cellContent.Text.Length)
                            //    {
                            //        return;
                            //    }
                            //        //new by me
                               
                            //    //    TextBox 
                            //    //end new by me
                            //}
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                    e.Handled = true;

                    if (columnDisplayIndex == 0)
                    {

                        dgvPccs.SelectedIndex = dgvPccs.SelectedIndex - 1;
                        columnDisplayIndex = dgvPccs.Columns.Count - 1;
                    }
                    else
                    {
                        if (columnDisplayIndex == 1)
                        {
                            columnDisplayIndex = 0;
                        }
                        //new by me
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex - 1;
                        }
                        //new by me
                    }
                    Microsoft.Windows.Controls.DataGridColumn nextColumn = dgvPccs.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgvPccs.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(dgvPccs.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    dgvPccs.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab || (e.Key == Key.Right))
                    {
                       //added by nandakumar
                        if (e.Key == Key.Right)
                        {
                            try
                            {
                                Microsoft.Windows.Controls.DataGridCellInfo dgt = dgvPccs.CurrentCell;
                                var cellContent = dgt.Column.GetCellContent(dgt.Item);
                                //cellContent.SelectionLength
                                //if (cellContent != null)
                                //{
                                //    if (cellContent != null)
                                //    {
                                //        return;
                                //    }
                                   
                                //    //    TextBox 
                                //}
                            }
                            catch (Exception ex)
                            {
                                return;
                            }

                        }
                        //end add by nandakumar

                        //dgt.Column.
                        //int columnDisplayIndex = dgvPccsRevisions.CurrentCell.Column.DisplayIndex;
                        //if (dgvPccsRevisions.SelectedIndex == dgvPccsRevisions.Items.Count - 1)
                        //{
                        //    if (columnDisplayIndex == dgvPccsRevisions.Columns.Count - 1)
                        //    {
                        //        dgvPccsRevisions.Focus();
                        //    }
                        //}
                        if (columnDisplayIndex == 14)
                        {
                            columnDisplayIndex = 0;
                            dgvPccs.SelectedIndex = dgvPccs.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgvPccs.SelectedIndex;

                        Microsoft.Windows.Controls.DataGridColumn nextColumn = dgvPccs.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgvPccs.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(dgvPccs.SelectedItem, nextColumn);
                        dgvPccs.ScrollIntoView(dgvPccs.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgvPccs.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvPccsRevisions_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = dgvPccsRevisions.CurrentCell.Column.DisplayIndex;

                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                //new
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                //new end
                {
                    //new
                    if (e.Key == Key.Left)
                    {
                        try
                        {
                            Microsoft.Windows.Controls.DataGridCellInfo dgt = dgvPccs.CurrentCell;
                            var cellContent = (TextBox)dgt.Column.GetCellContent(dgt.Item);
                            //cellContent.SelectionLength
                            if (cellContent != null)
                            {
                                if (cellContent.SelectionLength != cellContent.Text.Length)
                                {
                                    return;
                                }
                               
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                    }
                    //end new
                    e.Handled = true;
                    //int columnDisplayIndex = dgvPccsRevisions.CurrentCell.Column.DisplayIndex;
                    if (columnDisplayIndex == 0)
                    {

                        dgvPccsRevisions.SelectedIndex = dgvPccsRevisions.SelectedIndex - 1;
                        columnDisplayIndex = dgvPccsRevisions.Columns.Count - 1;
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
                    System.Windows.Controls.DataGridColumn nextColumn = dgvPccsRevisions.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    dgvPccsRevisions.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvPccsRevisions.SelectedItem, nextColumn);
                    string str = dgvPccsRevisions.CurrentCell.Item.ToValueAsString();
                    // tell the grid to initialize edit mode for the current cell
                    dgvPccsRevisions.BeginEdit();
                }
                else
                {
                    if (e.Key == Key.Tab || e.Key == Key.Right)
                    //new
                    //if (e.Key == Key.Tab)
                    //new end
                    {
                        //int columnDisplayIndex = dgvPccsRevisions.CurrentCell.Column.DisplayIndex;
                        //if (dgvPccsRevisions.SelectedIndex == dgvPccsRevisions.Items.Count - 1)
                        //{
                        //    if (columnDisplayIndex == dgvPccsRevisions.Columns.Count - 1)
                        //    {
                        //        dgvPccsRevisions.Focus();
                        //    }
                        //}
                        //new
                        if (e.Key == Key.Right)
                        {
                            try
                            {
                                Microsoft.Windows.Controls.DataGridCellInfo dgt = dgvPccs.CurrentCell;
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
                            //end new
                        if (columnDisplayIndex == 3)
                        {
                            columnDisplayIndex = 0;
                            dgvPccsRevisions.SelectedIndex = dgvPccsRevisions.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = dgvPccsRevisions.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = dgvPccsRevisions.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        dgvPccsRevisions.CurrentCell = new System.Windows.Controls.DataGridCellInfo(dgvPccsRevisions.SelectedItem, nextColumn);
                        dgvPccsRevisions.ScrollIntoView(dgvPccsRevisions.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        dgvPccsRevisions.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //GotKeyboardFocus="SelectAddress" PreviewMouseLeftButtonDown="SelectivelyIgnoreMouseButton"
        //private void SelectAddress(object sender, RoutedEventArgs e)
        //{
        //    //TextBox tb = ((TextBox)grdcmbPrdCNo.FindName("txtCombobox"));
        //    ComboBoxWithSearch cmb = (sender as ComboBoxWithSearch);
        //    if (cmb != null)
        //    {
        //        cmb.Focus();
        //        cmb.Focusable = true;

        //        TextBox tb = ((TextBox)cmb.FindName("txtCombobox"));
        //        if (tb != null)
        //        {
        //            cmb.Focus();
        //            cmb.Focusable = true;
        //            e.Handled = true;
        //            tb.SelectAll();
        //            //cmb.Focus();
        //            //cmb.Focusable = true;
        //            //cmb.Focus();
        //            //cmb.Focusable = true;
        //        }
        //    }
        //}

        //private void SelectivelyIgnoreMouseButton(object sender,
        //    MouseButtonEventArgs e)
        //{
        //    //TextBox tb = ((TextBox)grdcmbPrdCNo.FindName("txtCombobox"));
        //    ComboBoxWithSearch cmb = (sender as ComboBoxWithSearch);
        //    if (cmb != null)
        //    {
        //        TextBox tb = ((TextBox)cmb.FindName("txtCombobox"));
        //        if (tb != null)
        //        {
        //            if (!tb.IsKeyboardFocusWithin)
        //            {
        //                e.Handled = true;
        //                tb.Focus();
        //            }
        //        }
        //    }
        //}

    }
}
