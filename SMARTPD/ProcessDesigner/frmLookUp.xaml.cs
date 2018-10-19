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
using System.Windows.Shapes;
using System.Data;
using ProcessDesigner.Common;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for LookUpScreen.xaml
    /// </summary>
    public partial class frmLookUp : UserControl
    {
        DataView dv = new DataView();
        public frmLookUp()
        {
            InitializeComponent();
        }

        private DataTable _dataSource;
        public DataTable DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                if (value != null)
                {
                    dv = _dataSource.DefaultView;
                    setDataSource();
                }
            }
        }

        private DataRow _selectedItem;
        public DataRow SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
            }
        }

        public void setDataSource()
        {

            dgLookup.ItemsSource = dv;

            string[] columnNames = (from dc in _dataSource.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();

            cmbSearchIn.ItemsSource = columnNames;

            if (columnNames.Length > 0)
            {
                cmbSearchIn.SelectedIndex = 0;
            }
        }

        private void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //string dataType = _dataSource.Columns[cmbSearchIn.Text].DataType.Name.ToString();
                //if (dataType == "String")
                //{
                string sFilter = "Convert(" + cmbSearchIn.Text + ", 'System.String') LIKE ('%" + txtSearchValue.Text + "%')";
                dv.RowFilter = sFilter;
                //}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void cmbSearchIn_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                //string dataType = _dataSource.Columns[cmbSearchIn.Text].DataType.Name.ToString();
                //if (dataType == "String")
                //{
                string sFilter = "Convert(" + cmbSearchIn.Text + ", 'System.String') LIKE ('%" + txtSearchValue.Text + "%')";
                dv.RowFilter = sFilter;
                //}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void dgLookup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView drv = (DataRowView)dgLookup.SelectedItem;
            _selectedItem = drv.Row;
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataRowView drv = (DataRowView)dgLookup.SelectedItem;
                _selectedItem = drv.Row;
            }

        }



    }
}
