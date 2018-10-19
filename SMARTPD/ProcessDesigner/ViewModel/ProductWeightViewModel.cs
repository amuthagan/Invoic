using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using Microsoft.Practices.Prism.Commands;
using System.Data;
using System.Windows.Input;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;



namespace ProcessDesigner.ViewModel
{
    public class ProductWeightViewModel : ViewModelBase
    {
        ProductWeightModel _productWeight = null;
        ProductWeightBll _productWeightBll = null;
        private readonly ICommand _calculateCommand;
        public ICommand CalculateCommand { get { return this._calculateCommand; } }
        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return this._saveCommand; } }
        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }
        private readonly ICommand _deleteCommand;
        public ICommand DeleteCommand { get { return this._deleteCommand; } }
        //private readonly ICommand _cellEditEndingCommand;
        //public ICommand CellEditEndingCommand { get { return this._cellEditEndingCommand; } }
        private readonly ICommand _rowEditEndingCommand;
        public ICommand RowEditEndingCommand { get { return this._rowEditEndingCommand; } }
        public Action CloseAction { get; set; }
        //private readonly ICommand _selectionChanged;
        //public ICommand SelectionChanged { get { return this._selectionChanged; } }
        private OperationMode operaionmode = OperationMode.AddNew;

        private readonly ICommand _lostFocusCheeseWeightCommand;
        public ICommand LostFocusCheeseWeightCommand { get { return this._lostFocusCheeseWeightCommand; } }

        public Microsoft.Windows.Controls.DataGrid dgProductWeight { get; set; }

        public ProductWeightViewModel(UserInformation userInfo, string ciReference, string weightOption, OperationMode mode, int entityPrimaryKey)
        {
            _productWeightBll = new ProductWeightBll(userInfo);
            ProductWeight = new ProductWeightModel();
            this._saveCommand = new DelegateCommand(this.Save);
            this._calculateCommand = new DelegateCommand(this.Calculate);
            this._closeCommand = new DelegateCommand(this.Close);
            //this._cellEditEndingCommand = new DelegateCommand<object>(this.CellEditEnding);
            this._rowEditEndingCommand = new DelegateCommand<DataRowView>(this.RowEditEnding);
            this._deleteCommand = new DelegateCommand<DataRowView>(this.DeleteShapeDetail);
            this._lostFocusCheeseWeightCommand = new DelegateCommand(this.CheeseWeight_LostFocus);
            this._onShapeSelectionChanged = new DelegateCommand(this.ShapeCode_SelectionChanged);

            DropdownHeaders = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "SHAPE_CODE", ColumnDesc = "Shape Code", ColumnWidth = 120 },
                new DropdownColumns { ColumnName = "SHAPE_NAME", ColumnDesc = "Shape", ColumnWidth = "1*" }
                 };

            ProductWeight.CIREF_NO_FK = entityPrimaryKey;
            ProductWeight.CIreference = ciReference;
            ProductWeight.WeightOption = weightOption;
            operaionmode = mode;
            if (weightOption == "C")
            {
                WeightOptionText = "Cheese Weight : ";
            }
            else if (weightOption == "F")
            {
                WeightOptionText = "Finish Weight : ";
            }
            if (mode == OperationMode.Edit)
            {
                IsReadOnlyCheeseWeight = true;
                _productWeightBll.CopyProcess(ProductWeight);
            }


            _productWeightBll.GetShapeDetails(ProductWeight);

            if (ProductWeight.DVShapeDetails.Count > 0) ProductWeightSelectedItem = ProductWeight.DVShapeDetails[0];

            if (mode == OperationMode.Edit) Calculate();

        }

        public ProductWeightModel ProductWeight
        {
            get
            {
                return _productWeight;
            }
            set
            {
                _productWeight = value;
                NotifyPropertyChanged("ProductWeight");
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownHeaders = null;
        public ObservableCollection<DropdownColumns> DropdownHeaders
        {
            get { return this._dropdownHeaders; }
            set
            {
                this._dropdownHeaders = value;
                NotifyPropertyChanged("DropdownHeaders");
            }

        }

        private bool _cheesewtisfocus = false;
        public bool CheeseWtIsFocus
        {
            get { return _cheesewtisfocus; }
            set
            {
                _cheesewtisfocus = value;
                NotifyPropertyChanged("CheeseWtIsFocus");
            }
        }


        private string _weightoptiontext = "Cheese Weight : ";
        public string WeightOptionText
        {
            get { return _weightoptiontext; }
            set
            {
                _weightoptiontext = value;
                NotifyPropertyChanged("WeightOptionText");
            }
        }

        private bool _isReadOnlyCheeseWeight = false;
        public bool IsReadOnlyCheeseWeight
        {
            get { return _isReadOnlyCheeseWeight; }
            set
            {
                _isReadOnlyCheeseWeight = value;
                NotifyPropertyChanged("IsReadOnlyCheeseWeight");
            }
        }

        private DataRowView _selectedItem = null;
        public DataRowView ProductWeightSelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged("ProductWeightSelectedItem");
            }
        }


        private Microsoft.Windows.Controls.DataGridCell currentCell = null;
        public Microsoft.Windows.Controls.DataGridCell CurrentCell
        {
            get { return currentCell; }
            set
            {
                currentCell = value;
                NotifyPropertyChanged("CurrentCell");
            }
        }

        private DataRowView _shapeCode_SelectedItem = null;
        public DataRowView ShapeCode_SelectedItem
        {
            get { return this._shapeCode_SelectedItem; }
            set
            {
                this._shapeCode_SelectedItem = value;
                NotifyPropertyChanged("ShapeCode_SelectedItem");               
            }
        }

        private readonly ICommand _onShapeSelectionChanged;
        public ICommand OnShapeSelectionChanged { get { return this._onShapeSelectionChanged; } }
        private void ShapeCode_SelectionChanged()
        {
            try
            {
                if (ShapeCode_SelectedItem != null && ProductWeightSelectedItem != null)
                {

                    ProductWeightSelectedItem.BeginEdit();
                    DDSHAPE_MAST sm = _productWeightBll.GetShape(ShapeCode_SelectedItem["SHAPE_CODE"].ToString());
                    if (sm != null)
                    {
                        ProductWeightSelectedItem["HEAD1"] = sm.HEAD1.ToValueAsString();
                        ProductWeightSelectedItem["HEAD2"] = sm.HEAD2.ToValueAsString();
                        ProductWeightSelectedItem["HEAD3"] = sm.HEAD3.ToValueAsString();
                        if (!ProductWeightSelectedItem["SIGN"].IsNotNullOrEmpty()) ProductWeightSelectedItem["SIGN"] = "+";

                    }
                    ProductWeightSelectedItem.EndEdit();

                    //Microsoft.Windows.Controls.DataGridRow row = dgProductWeight.ItemContainerGenerator.ContainerFromIndex(dgProductWeight.SelectedIndex) as Microsoft.Windows.Controls.DataGridRow;
                    //if (row == null)
                    //{
                    //    row = dgProductWeight.ItemContainerGenerator.ContainerFromIndex(dgProductWeight.SelectedIndex) as Microsoft.Windows.Controls.DataGridRow;
                    //}
                    //if (row != null)
                    //{
                    //    Microsoft.Windows.Controls.DataGridCell cell = GetCell(dgProductWeight, row, 2);
                    //    if (cell != null)
                    //    {
                    //        cell.Focus();                            
                    //    }

                    //}

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public static Microsoft.Windows.Controls.DataGridCell GetCell(Microsoft.Windows.Controls.DataGrid dataGrid, Microsoft.Windows.Controls.DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method 
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    Microsoft.Windows.Controls.DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as Microsoft.Windows.Controls.DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as Microsoft.Windows.Controls.DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
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

        private void CheeseWeight_LostFocus()
        {
            try
            {
                if (IsReadOnlyCheeseWeight) return;

                if (ProductWeight.CheesWeight > 0)
                {
                    ProductWeight.Total = (ProductWeight.CheesWeight * 2.20462);
                }
                else
                {
                    ProductWeight.Total = 0;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Calculate()
        {
            try
            {
                if (ProductWeight.CIreference != "")
                {
                    CheeseWtIsFocus = true;
                    double varTotal = 0;
                    foreach (DataRowView drow in ProductWeight.DVShapeDetails)
                    {
                        if (drow["SIGN"].ToString() != "" && drow["VOLUME"].ToString() != "")
                        {
                            varTotal = varTotal + Convert.ToDouble(drow["VOLUME"]);
                        }
                    }

                    ProductWeight.Total = (varTotal * 0.000784393) * 2.20462;
                    ProductWeight.CheesWeight = varTotal * 0.000784393;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private double VolumeCalculation(string code, double v1, double v2, double v3)
        {
            double pvVOLUME, pi, r, c, angle, alpha, l;

            pvVOLUME = 0;
            pi = 3.141592654;
            try
            {

                if (code == "SP")
                {
                    pvVOLUME = (Math.Pow(v1, 2) * v2);
                }
                else if (code == "CD")
                {
                    pvVOLUME = (pi * Math.Pow(v1, 2) * v2 / 4);
                }
                else if (code == "HP")
                {
                    pvVOLUME = (3.464 * Math.Pow(v1, 2) * v2 / 4);
                }
                else if (code == "BHP")
                {
                    pvVOLUME = (3.314 * Math.Pow(v1, 2) * v2 / 4);
                }
                else if (code == "CF")
                {
                    pvVOLUME = (0.2618 * v3 * (Math.Pow(v1, 2) + (v1 * v2) + Math.Pow(v2, 2)));
                }
                else if (code == "SS")
                {
                    pvVOLUME = (pi * Math.Pow(v2, 2) * (v1 - (v2 / 3)));
                }
                else if (code == "DH")
                {
                    r = v1 / 2;
                    c = 2 * Math.Pow((v2 * (v1 - v2)), 0.5);
                    angle = (r - v2) / r;
                    alpha = 2 * Math.Atan(-angle / Math.Sqrt(-angle * angle + 1)) + 1.5708;
                    l = 0.01745 * r * alpha;
                    pvVOLUME = ((pi * Math.Pow(v1, 2) / 4) - (0.5 * ((r * l) - (c * (r - v2)))) * v3);
                }

                return pvVOLUME;
            }
            catch (Exception ex)
            {
                return pvVOLUME;
                throw ex.LogException();
            }
        }

        public void OnBeginningEdit(object sender, Microsoft.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            string code = ((System.Data.DataRowView)(e.Row.Item)).Row["SHAPE_CODE"].ToString();
            string column = e.Column.SortMemberPath;

            try
            {
                if (ProductWeightSelectedItem != null)
                {

                    switch (column)
                    {
                        case "VAL1":
                            if (!ProductWeightSelectedItem["HEAD1"].IsNotNullOrEmpty()) e.Cancel = true;
                            break;
                        case "VAL2":
                            if (!ProductWeightSelectedItem["HEAD2"].IsNotNullOrEmpty()) e.Cancel = true;
                            break;
                        case "VAL3":
                            if (!ProductWeightSelectedItem["HEAD3"].IsNotNullOrEmpty()) e.Cancel = true;
                            break;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void OnCellEditEnding(object sender, Microsoft.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                TextBox tb = e.EditingElement as TextBox;
                Microsoft.Windows.Controls.DataGrid dg = sender as Microsoft.Windows.Controls.DataGrid;
                string column = e.Column.SortMemberPath;

                if (ProductWeightSelectedItem != null)
                {


                    if (column == "SIGN")
                    {
                        if (!(tb.Text == "-" || tb.Text == "+"))
                        {
                            tb.Undo();
                        }
                    }

                    ProductWeightSelectedItem.BeginEdit();

                    if (column == "VAL1" || column == "VAL2" || column == "VAL3" || column == "SIGN")
                    {
                        ProductWeightSelectedItem[column] = tb.Text;
                    }

                    double pvVAL1 = 0, pvVAL2 = 0, pvVAL3 = 0;
                    if ((column == "VAL1" || column == "VAL1" || column == "VAL1" || column == "VOLUME") || ProductWeightSelectedItem["SHAPE_CODE"].ToString() != "")
                    {
                        if (ProductWeightSelectedItem["VAL1"].ToString() != "")
                        {
                            pvVAL1 = Convert.ToDouble(ProductWeightSelectedItem["VAL1"]);
                        }
                        if (ProductWeightSelectedItem["VAL2"].ToString() != "")
                        {
                            pvVAL2 = Convert.ToDouble(ProductWeightSelectedItem["VAL2"]);
                        }
                        if (ProductWeightSelectedItem["VAL3"].ToString() != "")
                        {
                            pvVAL3 = Convert.ToDouble(ProductWeightSelectedItem["VAL3"]);
                        }

                        if (ProductWeightSelectedItem["SHAPE_CODE"].ToString() == "DH" && (pvVAL1 > 0 || pvVAL2 > 0) && pvVAL1 == pvVAL2)
                        {
                            MessageBox.Show("Dimension-II Value should not be equals to Dimension-I value.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                            pvVAL2 = 0;
                            ProductWeightSelectedItem["VAL2"] = 0;
                            ProductWeightSelectedItem["VOLUME"] = 0;
                        }

                        if (ProductWeightSelectedItem["SHAPE_CODE"].ToString() == "DH" && pvVAL2 > pvVAL1)
                        {
                            MessageBox.Show("Dimension-II  Value should not be greater than Dimension-I value.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                            pvVAL2 = 0;
                            ProductWeightSelectedItem["VAL2"] = 0;
                            ProductWeightSelectedItem["VOLUME"] = 0;
                        }

                        if (pvVAL1 != 0 && pvVAL2 != 0)
                        {
                            ProductWeightSelectedItem["VOLUME"] = VolumeCalculation(ProductWeightSelectedItem["SHAPE_CODE"].ToString(), pvVAL1, pvVAL2, pvVAL3);
                        }
                        else
                        {
                            ProductWeightSelectedItem["VOLUME"] = 0;
                        }


                        if (ProductWeightSelectedItem["SIGN"].ToString() == "-")
                        {
                            ProductWeightSelectedItem["VOLUME"] = 0 - Math.Abs(Convert.ToDouble(ProductWeightSelectedItem["VOLUME"]));
                        }
                        else
                        {
                            ProductWeightSelectedItem["VOLUME"] = Math.Abs(Convert.ToDouble(ProductWeightSelectedItem["VOLUME"]));
                        }

                    }
                    ProductWeightSelectedItem.EndEdit();


                    if (column == "SHAPE_CODE" && ProductWeightSelectedItem["SHAPE_CODE"].ToString() != "")
                    {
                        if (ProductWeight.DVShapeDetails[ProductWeight.DVShapeDetails.Count - 1]["SHAPE_CODE"].ToString() != "")
                        {
                            ProductWeight.DVShapeDetails.AddNew();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void WeightCalc_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Windows.Controls.DataGridCellInfo cell = ((Microsoft.Windows.Controls.DataGrid)(sender)).CurrentCell;
                if (ProductWeightSelectedItem != null && cell.Column != null)
                {
                    string column = cell.Column.SortMemberPath;

                    ProductWeightSelectedItem.BeginEdit();

                    double pvVAL1 = 0, pvVAL2 = 0, pvVAL3 = 0;

                    if (ProductWeightSelectedItem["VAL1"].ToString() != "")
                    {
                        pvVAL1 = Convert.ToDouble(ProductWeightSelectedItem["VAL1"]);
                    }
                    if (ProductWeightSelectedItem["VAL2"].ToString() != "")
                    {
                        pvVAL2 = Convert.ToDouble(ProductWeightSelectedItem["VAL2"]);
                    }
                    if (ProductWeightSelectedItem["VAL3"].ToString() != "")
                    {
                        pvVAL3 = Convert.ToDouble(ProductWeightSelectedItem["VAL3"]);
                    }

                    if (pvVAL1 != 0 && pvVAL2 != 0)
                    {
                        ProductWeightSelectedItem["VOLUME"] = VolumeCalculation(ProductWeightSelectedItem["SHAPE_CODE"].ToString(), pvVAL1, pvVAL2, pvVAL3);
                    }
                    else
                    {
                        ProductWeightSelectedItem["VOLUME"] = 0;
                    }


                    if (ProductWeightSelectedItem["SIGN"].ToString() == "-")
                    {
                        ProductWeightSelectedItem["VOLUME"] = 0 - Math.Abs(Convert.ToDouble(ProductWeightSelectedItem["VOLUME"]));
                    }
                    else
                    {
                        ProductWeightSelectedItem["VOLUME"] = Math.Abs(Convert.ToDouble(ProductWeightSelectedItem["VOLUME"]));
                    }
                    ProductWeightSelectedItem.EndEdit();

                    if (column == "SHAPE_CODE" && ProductWeightSelectedItem["SHAPE_CODE"].ToString() != "")
                    {
                        if (ProductWeight.DVShapeDetails[ProductWeight.DVShapeDetails.Count - 1]["SHAPE_CODE"].ToString() != "")
                        {
                            ProductWeight.DVShapeDetails.AddNew();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditEnding(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    if (selecteditem["SHAPE_CODE"].ToString() != "")
                    {
                        if (ProductWeight.DVShapeDetails[ProductWeight.DVShapeDetails.Count - 1]["SHAPE_CODE"].ToString() != "")
                        {
                            ProductWeight.DVShapeDetails.AddNew();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void Save()
        {
            try
            {
                CheeseWtIsFocus = true;
                if (operaionmode == OperationMode.Edit) Calculate();

                ProductWeight.DVShapeDetails.Table.AcceptChanges();
                if (ProductWeight.DVShapeDetails != null)
                {
                    _productWeightBll.UpdateProductWeight(ProductWeight);

                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void DeleteShapeDetail(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {

                    MessageBoxResult msgResult = MessageBox.Show(PDMsg.BeforeDeleteRecord, "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (msgResult == MessageBoxResult.Yes)
                    {
                        if (selecteditem["ROWID"].IsNotNullOrEmpty())
                        {
                            ProductWeight.DTDeletedRecords.ImportRow(selecteditem.Row);
                        }

                        selecteditem.Delete();
                        MessageBox.Show(PDMsg.DeletedSuccessfully, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    if (ProductWeight.DVShapeDetails.Count == 0)
                    {
                        ProductWeight.DVShapeDetails.AddNew();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Close()
        {
            ProductWeight.CheesWeight = 0;
            if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
            {
                CloseAction();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

    }
}
