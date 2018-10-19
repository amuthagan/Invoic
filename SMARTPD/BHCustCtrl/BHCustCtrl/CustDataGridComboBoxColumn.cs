/*  
 * CustDataGridComboBoxColumn implements data grid column combobox with popup DataGrid control.
 * Bahrudin Hrnjica, bhrnjica@hotmail.com
 * First Release Oct, 2009
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Controls;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Reflection;

namespace BHCustCtrl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:BHCustCtrl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:BHCustCtrl;assembly=BHCustCtrl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustDataGridComboBoxColumn/>
    ///
    /// </summary>
    [DefaultProperty("Columns")]
    [ContentProperty("Columns")]
    public class CustDataGridComboBoxColumn : DataGridComboBoxColumn
    {
        //Columns of DataGrid
        private ObservableCollection<DataGridTextColumn> columns;
        //Cust Combobox  cell edit
        private CustComboBox comboBox;

        public CustDataGridComboBoxColumn()
        {
            comboBox = new CustComboBox();
            comboBox.MaxLength = 2;
            comboBox.IsEditable = true;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.Property == CustDataGridComboBoxColumn.ItemsSourceProperty)
                {
                    comboBox.ItemsSource = ItemsSource;


                }
                else if (e.Property == CustDataGridComboBoxColumn.SelectedValuePathProperty)
                {

                    comboBox.SelectedValuePath = SelectedValuePath;


                }
                else if (e.Property == CustDataGridComboBoxColumn.DisplayMemberPathProperty)
                {
                    comboBox.DisplayMemberPath = DisplayMemberPath;
                }

                base.OnPropertyChanged(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //The property is default and Content property for CustComboBox
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<DataGridTextColumn> Columns
        {
            get
            {
                if (this.columns == null)
                {
                    this.columns = new ObservableCollection<DataGridTextColumn>();
                }
                return this.columns;
            }
        }
        /// <summary>
        ///     Creates the visual tree for text based cells.
        /// </summary>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            if (comboBox.Columns.Count == 0)
            {
                //Add columns to DataGrid columns
                for (int i = 0; i < columns.Count; i++)
                    comboBox.Columns.Add(columns[i]);
            }
            return comboBox;
        }


        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            try
            {
                DataGridCell cell = null; //= editingEventArgs.Source as DataGridCell;

                if (cell != null)
                {
                    //For Typed DataSet
                    object obj = ((DataRowView)editingElement.DataContext).Row[this.SelectedValuePath];
                    comboBox.SelectedValue = obj;
                    //  comboBox.Focus();
                }
                else if (cell == null)
                {
                    object obj = ((DataRowView)editingElement.DataContext).Row[this.SelectedValuePath];
                    comboBox.SelectedValue = obj;
                    // comboBox.Focus();
                }
                comboBox.Focus();
                return comboBox.SelectedItem;
            }
            catch (Exception)
            {
                return null;
            }
        }


        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            try
            {


                if (comboBox.Text != "")
                {
                    comboBox.SelectedValue = comboBox.Text;
                }
                if (comboBox.SelectedValue == null)
                {
                    ((DataRowView)editingElement.DataContext).Row[this.SelectedValuePath] = "";
                    if (comboBox.Text != "")
                    {
                        string messageString = string.Empty;

                        messageString = SelectMessage(this.SelectedValuePath);
                        MessageBox.Show("Invalid " + messageString + " Code", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);

                        comboBox.Text = "";
                    }

                }
                else
                {
                    ((DataRowView)editingElement.DataContext).Row[this.SelectedValuePath] = comboBox.SelectedValue;
                }
                //  comboBox.SelectedValue = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string SelectMessage(string ColName)
        {
            try
            {
                string str = string.Empty;
                switch (ColName)
                {
                    case "OPN_CODE":
                        str = "Operation";
                        break;
                    case "UNIT_CODE":
                        str = "Unit";
                        break;
                    case "LOC_CODE":
                        str = "Location";
                        break;

                }
                return str;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        //--- Added by karthik


    }
}
