using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProcessDesigner.UserControls
{
    //Example
    //<local : DataGridDateColumn DateFormat=”yyyy-MM-dd” Binding=”{Binding HireDate}”/>
    public class DataGridDateColumn : System.Windows.Controls.DataGridBoundColumn
    {
        //DatePicker dp = null;
        public string DateFormat { get; set; }
        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            DatePicker dp = editingElement as DatePicker;
            if (dp != null)
            {
                dp.SelectedDate = DateTime.Parse(uneditedValue.ToString());
            }
        }
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            Binding cellBinding = this.Binding as Binding;
            if (cellBinding != null)
            {
                //DatePicker dp = new DatePicker();
                Binding b = new Binding();
                b.Path = cellBinding.Path;
                b.Source = cellBinding.Source;

                if (DateFormat != null)
                {
                    //DateTimeConverter dtc = new DateTimeConverter();
                    DataGridCellDateTimeConverter dtc = new DataGridCellDateTimeConverter();
                    b.Converter = dtc;
                    b.ConverterParameter = DateFormat;

                }

            }

            ////DatePicker dp = new DatePicker();
            //Binding b = new Binding();
            //b.Path = this.Binding.Path;
            //b.Source = this.Binding.Source;

            //if (DateFormat != null)
            //{
            //    //DateTimeConverter dtc = new DateTimeConverter();
            //    DataGridCellDateTimeConverter dtc = new DataGridCellDateTimeConverter();
            //    b.Converter = dtc;
            //    b.ConverterParameter = DateFormat;

            //}
            DatePicker dp = new DatePicker();
            dp.SetBinding(DatePicker.SelectedDateProperty, this.Binding);
            return dp;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            TextBlock txt = new TextBlock();
            Binding b = new Binding();
            Binding cellBinding = this.Binding as Binding;
            if (cellBinding != null)
            {
                b.Path = cellBinding.Path;
                b.Source = cellBinding.Source;
                if (DateFormat != null)
                {
                    //DateTimeConverter dtc = new DateTimeConverter();
                    DataGridCellDateTimeConverter dtc = new DataGridCellDateTimeConverter();
                    b.Converter = dtc;
                    b.ConverterParameter = DateFormat;
                }
            }

            //TextBlock txt = new TextBlock();
            //Binding b = new Binding();
            //b.Path = this.Binding.Path;
            //b.Source = this.Binding.Source;
            //if (DateFormat != null)
            //{
            //    //DateTimeConverter dtc = new DateTimeConverter();
            //    DataGridCellDateTimeConverter dtc = new DataGridCellDateTimeConverter();
            //    b.Converter = dtc;
            //    b.ConverterParameter = DateFormat;
            //}
            txt.SetBinding(TextBlock.TextProperty, b);
            return txt;
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            DatePicker dp = editingElement as DatePicker;
            if (dp != null)
            {
                DateTime? dt = dp.SelectedDate;
                if (dt.HasValue)
                    return dt.Value;
            }
            return DateTime.Today;
        }

        protected override bool CommitCellEdit(System.Windows.FrameworkElement editingElement)
        {
            DatePicker dp = editingElement as DatePicker;
            if (dp == null) return true;

            dp.Text = "01/01/2001";

            //if (!isCellValueChanged) tb.Text = Convert.ToString(previouseCellValue);

            //if (MaxScale > 0 && FormatCellAfterCellEndEdit)
            //{
            //    if (Convert.ToString(tb.Text).Trim().Length == 0) tb.Text = "0";
            //    string decimalFormat = "0." + "".PadLeft(MaxScale, '0');
            //    tb.Text = Convert.ToDecimal(Convert.ToString(tb.Text)).ToString(decimalFormat);
            //}

            BindingExpression binding = editingElement.GetBindingExpression(DatePicker.TextProperty);
            if (binding != null)
                binding.UpdateSource();

            return true; //base.CommitCellEdit(editingElement);
        }

    }
}
