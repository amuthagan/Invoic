/*  
 * CustComboBox implements combobox with popup DataGrid control.
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
using System.ComponentModel;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using Microsoft.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Windows.Controls.Primitives;


namespace BHCustCtrl
{
    /// <summary>
    /// CustComboBox implements combobox with popup DataGrid control.
    /// Usage:
    ///     <local:CustComboBox Margin="121,0,251,159" VerticalAlignment="Bottom" Height="29" 
	///	                        ItemsSource="{Binding Collection}" DisplayMemberPath="Property1" IsEditable="True" >
    ///	                        
	///		                <toolKit:DataGridTextColumn Header="HeaderTitle1" Binding="{Binding Property1, Mode=Default}" />
    ///		                <toolKit:DataGridTextColumn Header="HeaderTitle2" Binding="{Binding Property2, Mode=Default}"/>
    ///		                <toolKit:DataGridTextColumn Header="HeaderTitle3" Binding="{Binding Property3, Mode=Default}"/>
    ///		                
	///	    </local:CustComboBox>
    ///
    /// </summary>
    [DefaultProperty("Columns")]
    [ContentProperty("Columns")]
    public class CustComboBox : ComboBox
    {
        const string partPopupDataGrid = "PART_PopupDataGrid";
        //Columns of DataGrid
        private ObservableCollection<DataGridTextColumn> columns;
        //Attached DataGrid control
        private DataGrid popupDataGrid;

        static CustComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustComboBox), new FrameworkPropertyMetadata(typeof(CustComboBox)));
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

        //Apply theme and attach columns to DataGrid popupo control
        public override void OnApplyTemplate()
        {
            if (popupDataGrid == null)
            {
                
                popupDataGrid = this.Template.FindName(partPopupDataGrid, this) as DataGrid;
                if (popupDataGrid != null && columns != null)
                {
                    //Add columns to DataGrid columns
                    for (int i = 0; i < columns.Count; i++)
                        popupDataGrid.Columns.Add(columns[i]);
                    
                    //Add event handler for DataGrid popup
                    popupDataGrid.MouseDown += new MouseButtonEventHandler(popupDataGrid_MouseDown);
                    popupDataGrid.SelectionChanged += new SelectionChangedEventHandler(popupDataGrid_SelectionChanged);

                }
            }
            //Call base class method
            base.OnApplyTemplate();
        }

        //Synchronize selection between Combo and DataGrid popup
        void popupDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //When open in Blend prevent raising exception 
          if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataGrid dg = sender as DataGrid;
                if (dg != null)
                {
                    this.SelectedItem = dg.SelectedItem;
                    this.SelectedValue = dg.SelectedValue;
                    this.SelectedIndex = dg.SelectedIndex;
                    this.SelectedValuePath = dg.SelectedValuePath;
                  //  base.CommandBindings();
                  //  ((DataRowView)editingElement.DataContext).Row[this.SelectedValuePath] = comboBox.SelectedValue;
                }
            }
        }
        //Event for DataGrid popup MouseDown
        void popupDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;

                // iteratively traverse the visual tree and stop when dep is one of ..
                while ((dep != null) &&
                        !(dep is DataGridCell) &&
                        !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                if (dep == null)
                    return;

                if (dep is DataGridColumnHeader)
                {
                   // do something
                }
                //When user clicks to DataGrid cell, popup have to be closed
                if (dep is DataGridCell)
                {
                    this.IsDropDownOpen = false;
                }
            }
        }
        //-----------------------------------------------------------------------
        //// Added by karthik for Set Max Length for Combobox
        //public static int GetMaxLength(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(MaxLengthProperty);
        //}

        //public static void SetMaxLength(DependencyObject obj, int value)
        //{
        //    obj.SetValue(MaxLengthProperty, value);
        //}

        //// Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached("MaxLength", typeof(int), typeof(CustComboBox), new UIPropertyMetadata(OnMaxLenghtChanged));

        //private static void OnMaxLenghtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        //{
        //    var comboBox = obj as ComboBox;
        //    if (comboBox == null) return;

        //    comboBox.Loaded +=
        //        (s, e) =>
        //        {
        //        //    var textBox = popupDataGrid.Children.GetType(TextBox);

        //            var textBox = comboBox.FindChild(typeof(TextBox), "PART_EditableTextBox");
        //            if (textBox == null) return;

        //            textBox.SetValue(TextBox.MaxLengthProperty, args.NewValue);
        //        };
        //}


        //public static DependencyObject FindChild(this DependencyObject reference, string childName, Type childType)
        //{
        //    DependencyObject foundChild = null;
        //    if (reference != null)
        //    {
        //        int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
        //        for (int i = 0; i < childrenCount; i++)
        //        {
        //            var child = VisualTreeHelper.GetChild(reference, i);
        //            // If the child is not of the request child type child
        //            if (child.GetType() != childType)
        //            {
        //                // recursively drill down the tree
        //                foundChild = FindChild(child, childName, childType);
        //            }
        //            else if (!string.IsNullOrEmpty(childName))
        //            {
        //                var frameworkElement = child as FrameworkElement;
        //                // If the child's name is set for search
        //                if (frameworkElement != null && frameworkElement.Name == childName)
        //                {
        //                    // if the child's name is of the request name
        //                    foundChild = child;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                // child element found.
        //                foundChild = child;
        //                break;
        //            }
        //        }
        //    }
        //    return foundChild;
        //}

        ////----------------------------------------------------

        public static readonly DependencyProperty MaxLengthProperty =
           DependencyProperty.Register(
               "MaxLength",
               typeof(int),
               typeof(CustComboBox),
               new UIPropertyMetadata(int.MaxValue));

        [Description("Maximum length of the text in the EditableTextBox.")]
        [Category("Editable ComboBox")]
        [DefaultValue(int.MaxValue)]
        public int MaxLength
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return (int)this.GetValue(MaxLengthProperty);
            }

            [System.Diagnostics.DebuggerStepThrough]
            set
            {
                this.SetValue(MaxLengthProperty, value);
                this.ApplyMaxLength();
            }
        }

        protected TextBox EditableTextBox
        {
            get
            {
               // TextBox TxtBox = (TextBox) this.Template.FindName("PART_EditableTextBox", this);
                return this.GetTemplateChild("PART_EditableTextBox") as TextBox;
            }
        }
 
        private void ApplyMaxLength()
        {
           // this.EditableTextBox.MaxLength = 5;
            if (this.EditableTextBox != null)
            {
                this.EditableTextBox.MaxLength = this.MaxLength;
                this.EditableTextBox.MaxLength = 5;
            }
        }


        //public static readonly DependencyProperty CustomMaxLengthProperty = DependencyProperty.RegisterAttached("MaxLengthValue", typeof(int), typeof(CustComboBox), new UIPropertyMetadata(OnMaxLenghtChanged));
        //private static void OnMaxLenghtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        //{
        //    var comboBox = obj as ComboBox ;
        //    if (comboBox == null) return;

        //    comboBox.Loaded +=
        //        (s, e) =>
        //        {
                    

        //            var textBox = this.FindChild(typeof(TextBox), "PART_EditableTextBox",);
        //            if (textBox == null) return;

        //            textBox.SetValue(TextBox.MaxLengthProperty, args.NewValue);
        //        };
        //}

        //public static readonly DependencyProperty MaxLengthProperty = 
       // DependencyProperty.RegisterAttached("MaxLength", typeof(int), typeof(CustComboBox), new UIPropertyMetadata(OnMaxLenghtChanged));



        //When selection changed in combobox (pressing  arrow key down or up) must be synchronized with opened DataGrid popup
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (popupDataGrid == null)
                return;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (IsDropDownOpen)
                {
                    popupDataGrid.SelectedItem = this.SelectedItem;
                    if (popupDataGrid.SelectedItem != null)
                        popupDataGrid.ScrollIntoView(popupDataGrid.SelectedItem);
                }
            }
        }
        protected override void OnDropDownOpened(EventArgs e)
        { 
            popupDataGrid.SelectedItem = this.SelectedItem;
            
            base.OnDropDownOpened(e);

            if (popupDataGrid.SelectedItem != null)
                popupDataGrid.ScrollIntoView(popupDataGrid.SelectedItem);
        }
         
    }
}
