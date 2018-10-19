using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows;

namespace ProcessDesigner.UICommon
{
    public class RowToIndexConverter : MarkupExtension, IValueConverter
    {
        static RowToIndexConverter converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Data.DataRowView drv = (System.Data.DataRowView)value;
            //Get the CollectionView from the DataGrid that is using the converter
            DataGrid dg = (DataGrid)Application.Current.MainWindow.FindName("MainGrid");
            CollectionView cv = (CollectionView)dg.Items;
            //Get the index of the DataRowView from the CollectionView
            int rowindex = cv.IndexOf(drv) + 1;

            return rowindex.ToString();
            //DataGridRow row = value as DataGridRow;
            //if (row != null)
            //    return row.GetIndex();
            //else
            //    return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null) converter = new RowToIndexConverter();
            return converter;
        }

        public RowToIndexConverter()
        {
        }
    }
}
