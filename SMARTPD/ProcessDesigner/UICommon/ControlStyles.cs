using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Windows.Media;
using Microsoft;
using BHCustCtrl;
using ProcessDesigner.UserControls;
using ProcessDesigner.Common;



namespace ProcessDesigner.UICommon
{
    partial class ControlStyles : ResourceDictionary
    {
        public ControlStyles()
        {
            // InitializeComponent();
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null)
            {
                GridColumnFastEdit(cell, e);
            }
            else
            {
                Microsoft.Windows.Controls.DataGridCell toolkitcell = sender as Microsoft.Windows.Controls.DataGridCell;
                ToolkitGridColumnFastEdit(toolkitcell, e);
            }

        }

        private void DataGridCell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null)
            {
                GridColumnFastEdit(cell, e);
            }
            else
            {
                Microsoft.Windows.Controls.DataGridCell toolkitcell = sender as Microsoft.Windows.Controls.DataGridCell;
                ToolkitGridColumnFastEdit(toolkitcell, e);
            }

        }

        private void DataGridCell_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null)
            {
                GridColumnFastEditOnFocus(cell, e);
            }
            else
            {
                Microsoft.Windows.Controls.DataGridCell toolkitcell = sender as Microsoft.Windows.Controls.DataGridCell;
                ToolkitGridColumnFastEditOnFocus(toolkitcell, e);
            }
        }

        private static void GridColumnFastEdit(DataGridCell cell, RoutedEventArgs e)
        {
            if (cell == null || cell.IsEditing || cell.IsReadOnly)
                return;

            DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
            if (dataGrid == null)
                return;

            dataGrid.UnselectAll();

            if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
            {
                if (!cell.IsSelected)
                    cell.IsSelected = true;
            }
            else
            {
                DataGridRow row = FindVisualParent<DataGridRow>(cell);
                if (row != null && !row.IsSelected)
                {
                    row.IsSelected = true;
                }
            }

            if (!cell.IsFocused)
            {
                cell.Focus();
            }

            String statusmsg = "";
            if (cell.Content is TextBlock)
            {
                TextBlock tb = cell.Content as TextBlock;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is TextBox)
            {
                TextBox tb = cell.Content as TextBox;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is ComboBox)
            {
                ComboBox cb = cell.Content as ComboBox;
                if (cb != null)
                {
                    statusmsg = cb.Text;
                    //DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                    cell.Dispatcher.Invoke(
                     DispatcherPriority.Background,
                     new Action(delegate { }));
                    cb.IsDropDownOpen = true;
                }
            }
            if (cell.Content is ContentPresenter)
            {
                TextBlock tb = FindVisualChild<TextBlock>(cell);
                if (tb != null) statusmsg = tb.Text;                
            }


            StatusMessage.setStatus(statusmsg);

        }

        private static void ToolkitGridColumnFastEdit(Microsoft.Windows.Controls.DataGridCell cell, RoutedEventArgs e)
        {
            if (cell == null || cell.IsEditing || cell.IsReadOnly)
                return;

            Microsoft.Windows.Controls.DataGrid dataGrid = FindVisualParent<Microsoft.Windows.Controls.DataGrid>(cell);
            if (dataGrid == null)
                return;

            dataGrid.UnselectAll();

            if (dataGrid.SelectionUnit != Microsoft.Windows.Controls.DataGridSelectionUnit.FullRow)
            {
                if (!cell.IsSelected)
                    cell.IsSelected = true;
            }
            else
            {
                Microsoft.Windows.Controls.DataGridRow row = FindVisualParent<Microsoft.Windows.Controls.DataGridRow>(cell);
                if (row != null && !row.IsSelected)
                {
                    row.IsSelected = true;
                }
            }

            if (!cell.IsFocused)
            {
                cell.Focus();
            }

            String statusmsg = "";
            if (cell.Content is TextBlock)
            {
                TextBlock tb = cell.Content as TextBlock;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is TextBox)
            {
                TextBox tb = cell.Content as TextBox;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is ComboBox)
            {
                ComboBox cb = cell.Content as ComboBox;
                if (cb != null)
                {
                    statusmsg = cb.Text;
                    //DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                    cell.Dispatcher.Invoke(
                     DispatcherPriority.Background,
                     new Action(delegate { }));
                    cb.IsDropDownOpen = true;
                }
            }
            if (cell.Content is ContentPresenter)
            {
                TextBlock tb = FindVisualChild<TextBlock>(cell);
                if (tb != null) statusmsg = tb.Text; 
            }


            StatusMessage.setStatus(statusmsg);
        }

        private static void GridColumnFastEditOnFocus(DataGridCell cell, KeyboardFocusChangedEventArgs e)
        {
            if (cell == null || cell.IsEditing || cell.IsReadOnly)
                return;

            DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
            if (dataGrid == null)
                return;

            dataGrid.UnselectAll();

            if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
            {
                if (!cell.IsSelected)
                    cell.IsSelected = true;
            }
            else
            {
                DataGridRow row = FindVisualParent<DataGridRow>(cell);
                if (row != null && !row.IsSelected)
                {
                    row.IsSelected = true;
                }
            }

            String statusmsg = "";
            if (cell.Content is TextBlock)
            {
                TextBlock tb = cell.Content as TextBlock;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is TextBox)
            {
                TextBox tb = cell.Content as TextBox;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is ComboBox)
            {
                ComboBox cb = cell.Content as ComboBox;
                if (cb != null)
                {
                    statusmsg = cb.Text;
                    //DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                    cell.Dispatcher.Invoke(
                     DispatcherPriority.Background,
                     new Action(delegate { }));
                    cb.IsDropDownOpen = true;
                }
            }
            if (cell.Content is ContentPresenter)
            {
                TextBlock tb = FindVisualChild<TextBlock>(cell);
                if (tb != null) statusmsg = tb.Text; 
            }

            StatusMessage.setStatus(statusmsg);

        }

        private static void ToolkitGridColumnFastEditOnFocus(Microsoft.Windows.Controls.DataGridCell cell, KeyboardFocusChangedEventArgs e)
        {
            if (cell == null || cell.IsEditing || cell.IsReadOnly)
                return;

            Microsoft.Windows.Controls.DataGrid dataGrid = FindVisualParent<Microsoft.Windows.Controls.DataGrid>(cell);
            if (dataGrid == null)
                return;

            dataGrid.UnselectAll();

            if (dataGrid.SelectionUnit != Microsoft.Windows.Controls.DataGridSelectionUnit.FullRow)
            {
                if (!cell.IsSelected)
                    cell.IsSelected = true;
            }
            else
            {
                Microsoft.Windows.Controls.DataGridRow row = FindVisualParent<Microsoft.Windows.Controls.DataGridRow>(cell);
                if (row != null && !row.IsSelected)
                {
                    row.IsSelected = true;
                }
            }

            String statusmsg = "";
            if (cell.Content is TextBlock)
            {
                TextBlock tb = cell.Content as TextBlock;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is TextBox)
            {
                TextBox tb = cell.Content as TextBox;
                if (tb != null) statusmsg = tb.Text;
            }
            else if (cell.Content is ComboBox)
            {
                ComboBox cb = cell.Content as ComboBox;
                if (cb != null)
                {
                    statusmsg = cb.Text;
                    //DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                    cell.Dispatcher.Invoke(
                     DispatcherPriority.Background,
                     new Action(delegate { }));
                    cb.IsDropDownOpen = true;
                }
            }
            if (cell.Content is ContentPresenter)
            {
                TextBlock tb = FindVisualChild<TextBlock>(cell);
                if (tb != null) statusmsg = tb.Text; 
            }

            StatusMessage.setStatus(statusmsg);
        }

        private void DataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Shapes.Rectangle rect = e.OriginalSource as System.Windows.Shapes.Rectangle;
            if (rect != null)
            {
                e.Handled = true;
            }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid == null)
                return;

            if (e.Key == Key.Enter || e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Right || e.Key == Key.Left)
            {
                if (dataGrid.ItemsSource != null && dataGrid.SelectedIndex == dataGrid.Items.Count - 2)
                {
                    var border = VisualTreeHelper.GetChild(dataGrid, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null) scroll.ScrollToEnd();
                    }
                }
            }

            if (e.Key == Key.Enter)
            {

                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;

                DataGridRow rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentItem);
                if (rowContainer != null)
                {
                    int columnIndex = dataGrid.Columns.IndexOf(dataGrid.CurrentColumn);
                    System.Windows.Controls.Primitives.DataGridCellsPresenter presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                    dataGrid.CommitEdit();
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    cell.MoveFocus(request);
                    dataGrid.SelectedItem = dataGrid.CurrentItem;
                    e.Handled = true;
                    dataGrid.UpdateLayout();
                    dataGrid.BeginEdit(e);
                }
            }
            else if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Right || e.Key == Key.Left)
            {
                int columnIndex = dataGrid.Columns.IndexOf(dataGrid.CurrentColumn);
                DataGridRow rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentItem);
                if (rowContainer != null)
                {
                    System.Windows.Controls.Primitives.DataGridCellsPresenter presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                    DataGridCell currentcell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

                    if (currentcell.Content is ContentPresenter)
                    {
                        DatePickerCus datepicker = FindVisualChild<DatePickerCus>(currentcell);
                        if (datepicker != null && datepicker.IsDropDownOpen)
                        {
                            e.Handled = true;
                            return;
                        }

                        ComboBoxWithSearch combo = FindVisualChild<ComboBoxWithSearch>(currentcell);
                        if (combo != null && combo.IsDropDownOpen)
                        {
                            e.Handled = true;
                            return;
                        }
                    }

                    if (e.Key == Key.Down)
                    {
                        dataGrid.CommitEdit();
                        dataGrid.SelectedIndex++;

                        rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                        if (rowContainer != null)
                        {
                            presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                            cell.Focus();
                            dataGrid.CurrentItem = dataGrid.SelectedItem;
                            e.Handled = true;
                            dataGrid.UpdateLayout();
                            dataGrid.BeginEdit(e);
                        }
                    }
                    else if (e.Key == Key.Up)
                    {
                        if (dataGrid.ItemsSource != null && dataGrid.SelectedIndex == 0) return;

                        dataGrid.CommitEdit();
                        dataGrid.SelectedIndex--;

                        rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                        if (rowContainer != null)
                        {
                            presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                            cell.Focus();
                            dataGrid.CurrentItem = dataGrid.SelectedItem;
                            e.Handled = true;
                            dataGrid.UpdateLayout();
                            dataGrid.BeginEdit(e);
                        }
                    }

                }
            }
        }

        private void ToolkitDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Microsoft.Windows.Controls.DataGrid dataGrid = sender as Microsoft.Windows.Controls.DataGrid;
            if (dataGrid == null)
                return;

            if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Right || e.Key == Key.Left)
            {
                if (dataGrid.ItemsSource != null && dataGrid.SelectedIndex == dataGrid.Items.Count - 2)
                {
                    var border = VisualTreeHelper.GetChild(dataGrid, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null) scroll.ScrollToEnd();
                    }
                }
            }


            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {

                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;

                Microsoft.Windows.Controls.DataGridRow rowContainer = (Microsoft.Windows.Controls.DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentItem);
                if (rowContainer != null)
                {
                    int columnIndex = dataGrid.Columns.IndexOf(dataGrid.CurrentColumn);
                    Microsoft.Windows.Controls.Primitives.DataGridCellsPresenter presenter = GetVisualChild<Microsoft.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                    dataGrid.CommitEdit();
                    Microsoft.Windows.Controls.DataGridCell cell = (Microsoft.Windows.Controls.DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    cell.MoveFocus(request);
                    dataGrid.SelectedItem = dataGrid.CurrentItem;
                    e.Handled = true;
                    dataGrid.UpdateLayout();
                    try
                    {
                        dataGrid.BeginEdit(e);
                    }
                    catch (System.InvalidOperationException)
                    {
                         //Ignore error if it is 'Recursive call to Automation Peer API is not valid.'
                    }
                }
            }
            else if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Right || e.Key == Key.Left)
            {
                int columnIndex = dataGrid.Columns.IndexOf(dataGrid.CurrentColumn);
                Microsoft.Windows.Controls.DataGridRow rowContainer = (Microsoft.Windows.Controls.DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentItem);
                if (rowContainer != null)
                {
                    Microsoft.Windows.Controls.Primitives.DataGridCellsPresenter presenter = GetVisualChild<Microsoft.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                    Microsoft.Windows.Controls.DataGridCell currentcell = (Microsoft.Windows.Controls.DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

                    if (currentcell.Content is ContentPresenter)
                    {
                        DatePickerCus datepicker = FindVisualChild<DatePickerCus>(currentcell);
                        if (datepicker != null && datepicker.IsDropDownOpen)
                        {
                            e.Handled = true;
                            return;
                        }

                        ComboBoxWithSearch combo = FindVisualChild<ComboBoxWithSearch>(currentcell);
                        if (combo != null && combo.IsDropDownOpen)
                        {
                            e.Handled = true;
                            return;
                        }
                    }

                    if (e.Key == Key.Down)
                    {
                        dataGrid.CommitEdit();
                        dataGrid.SelectedIndex++;

                        rowContainer = (Microsoft.Windows.Controls.DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                        if (rowContainer != null)
                        {
                            presenter = GetVisualChild<Microsoft.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                            Microsoft.Windows.Controls.DataGridCell cell = (Microsoft.Windows.Controls.DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                            cell.Focus();
                            dataGrid.CurrentItem = dataGrid.SelectedItem;
                            e.Handled = true;
                            dataGrid.UpdateLayout();
                            try
                            {
                                dataGrid.BeginEdit(e);
                            }
                            catch (System.InvalidOperationException)
                            {
                                //Ignore error if it is 'Recursive call to Automation Peer API is not valid.'
                            }
                        }
                    }
                    else if (e.Key == Key.Up)
                    {
                        if (dataGrid.ItemsSource != null && dataGrid.SelectedIndex == 0) return;

                        dataGrid.CommitEdit();
                        dataGrid.SelectedIndex--;

                        rowContainer = (Microsoft.Windows.Controls.DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                        if (rowContainer != null)
                        {
                            presenter = GetVisualChild<Microsoft.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);
                            Microsoft.Windows.Controls.DataGridCell cell = (Microsoft.Windows.Controls.DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                            cell.Focus();
                            dataGrid.CurrentItem = dataGrid.SelectedItem;
                            e.Handled = true;
                            dataGrid.UpdateLayout();
                            try
                            {
                                dataGrid.BeginEdit(e);
                            }
                            catch (System.InvalidOperationException)
                            {
                                //Ignore error if it is 'Recursive call to Automation Peer API is not valid.'
                            }
                        }
                    }

                }
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                if (tb.Tag.IsNotNullOrEmpty()) StatusMessage.setStatus(tb.Tag.ToString());
            }
            else if (sender is PasswordBox)
            {
                PasswordBox pwd = sender as PasswordBox;
                if (pwd.Tag.IsNotNullOrEmpty()) StatusMessage.setStatus(pwd.Tag.ToString());
            }
            else if (sender is ComboBox)
            {
                ComboBox cb = sender as ComboBox;
                if (cb.Tag.IsNotNullOrEmpty()) StatusMessage.setStatus(cb.Tag.ToString());
            }
        }


        private static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>
                    (v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }


    }
}
