using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using WPF.MDI;
using ProcessDesigner.UserControls;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProcessDesigner.ViewModel
{
    class APQPViewModel : ViewModelBase
    {
        WPF.MDI.MdiChild _mdiChild;
        UserInformation _userInformation;
        APQPBll _aPQPBll;
        public APQPViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                _aPQPBll = new APQPBll(userInformation);
                DvAPQP = _aPQPBll.GetApQpChart("").DefaultView;
                PartNoCombo = _aPQPBll.GetPartNoDetails();
                SetdropDownItems();
                this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
                this.saveCommand = new DelegateCommand(this.Save);
                this.printReportCommand = new DelegateCommand(this.PrintReport);
                PartNo = "";
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private DataView _dvAPQP;
        public DataView DvAPQP
        {
            get
            {
                return _dvAPQP;
            }
            set
            {
                _dvAPQP = value;
                NotifyPropertyChanged("DvAPQP");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsPart;
        public ObservableCollection<DropdownColumns> DropDownItemsPart
        {
            get
            {
                return _dropDownItemsPart;
            }
            set
            {
                this._dropDownItemsPart = value;
                NotifyPropertyChanged("DropDownItemsPart");
            }
        }

        private DataView _partNoCombo;
        public DataView PartNoCombo
        {
            get
            {
                return _partNoCombo;
            }
            set
            {
                _partNoCombo = value;
                NotifyPropertyChanged("PartNoCombo");
            }
        }

        private string _partNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part No is required")]
        public string PartNo
        {
            get
            {
                return _partNo;
            }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }

        private DataRowView _selectedrowpart;
        public DataRowView SelectedRowPart
        {
            get
            {
                return _selectedrowpart;
            }

            set
            {
                _selectedrowpart = value;
            }
        }

        private bool _partNumberIsFocused;
        public bool PartNumberIsFocused
        {
            get
            {
                return _partNumberIsFocused;
            }
            set
            {
                _partNumberIsFocused = value;
                NotifyPropertyChanged("PartNumberIsFocused");
            }

        }



        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            try
            {
                if (SelectedRowPart != null)
                {
                    DvAPQP = _aPQPBll.GetApQpChart(SelectedRowPart["PART_NO"].ToString().Trim()).DefaultView;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand saveCommand;
        public ICommand SaveCommand { get { return this.saveCommand; } }
        private void Save()
        {
            try
            {
                if (!PartNo.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                    return;
                }
                PartNumberIsFocused = true;
                Mouse.OverrideCursor = Cursors.Wait;
                DvAPQP.ToTable().AcceptChanges();
                if (_aPQPBll.Save(DvAPQP.ToTable().Copy(), PartNo) == true)
                {
                    Mouse.OverrideCursor = null;
                    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    // ClearAll();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "Part No", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "Description", ColumnWidth = "1*" }
                        };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void dgAPQP_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            int iCurrentRow;
            iCurrentRow = e.Row.GetIndex() + 1;
            DataRowView rowView = e.Row.Item as DataRowView;
            if (rowView["HIDDEN_SL_NO"].ToString() == "")
            {
                e.Cancel = true;
                return;
            }

            if (e.Column.Header.ToString().ToUpper() == "STATUS")
            {
                if (rowView["STATUS"].ToValueAsString().Trim() == "")
                {
                    rowView["STATUS"] = "Y";
                }
                else if (rowView["STATUS"].ToValueAsString().Trim() == "Y")
                {
                    rowView["STATUS"] = "N";
                }
                else if (rowView["STATUS"].ToValueAsString().Trim() == "N")
                {
                    rowView["STATUS"] = "";
                }
                if ((iCurrentRow == 4) || (iCurrentRow == 6) || (iCurrentRow == 7))
                {
                    rowView["COMMENT"] = "--";
                }
                else if ((iCurrentRow == 5) && rowView["STATUS"].ToValueAsString().Trim() == "N")
                {
                    rowView["COMMENT"] = "Generic FMEA";
                }
                else if ((iCurrentRow == 5) && rowView["STATUS"].ToValueAsString().Trim() == "Y")
                {
                    rowView["COMMENT"] = "--";
                }
                else if ((iCurrentRow == 5) && rowView["STATUS"].ToValueAsString().Trim() == "")
                {
                    rowView["COMMENT"] = "";
                }
                else if ((iCurrentRow == 8) && rowView["STATUS"].ToValueAsString().Trim() == "N")
                {
                    rowView["COMMENT"] = "Generic";
                }
                else if (iCurrentRow == 8 && rowView["STATUS"].ToValueAsString().Trim() != "N")
                {
                    rowView["COMMENT"] = "";
                }
                //e.Cancel = true;
                rowView.EndEdit();
                e.Cancel = true;
                return;
            }
        }

        public void dgAPQP_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                DataRowView rowView = e.Row.Item as DataRowView;
                if (rowView["HIDDEN_SL_NO"].ToString() == "")
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGray);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void dgAPQP_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dgAPQP = (DataGrid)sender;
                DataView dvTable = (System.Data.DataView)(dgAPQP.ItemsSource);
                DataTable dtTable = dvTable.ToTable();
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    DataGridRow row = (DataGridRow)dgAPQP.ItemContainerGenerator.ContainerFromIndex(i);
                    if (dtTable.Rows[i]["HIDDEN_SL_NO"].ToString() == "")
                    {
                        if (row != null)
                        {
                            row.Background = Brushes.LightGray;
                        }
                    }
                }
                dgAPQP.Columns[0].Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }
        private static T GetVisualChild<T>(DependencyObject visual) where T : DependencyObject
        {
            if (visual == null)
                return null;

            var count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < count; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(visual, i);

                var childOfTypeT = child as T ?? GetVisualChild<T>(child);
                if (childOfTypeT != null)
                    return childOfTypeT;
            }

            return null;
        }

        public void OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.GetType() == typeof(DataGridTemplateColumn))
            {
                var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                if (popup != null && popup.IsOpen)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        private void ClearAll()
        {
            try
            {
                PartNo = "";
                DvAPQP = _aPQPBll.GetApQpChart("").DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private ICommand printReportCommand;
        public ICommand PrintReportCommand { get { return this.printReportCommand; } }
        private void PrintReport()
        {
            try
            {
                DataTable dtData;
                string reportName = "APQPReport";
                if (!PartNo.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                    return;
                }
                dtData = DvAPQP.ToTable().Copy();
                //dtData.Rows.Clear();
                DataSet dsReport = new DataSet();
                dsReport.DataSetName = "DSAPQPDATA";
                //dtData.Rows.Clear();
                dsReport.Namespace = "APQP";
                dtData.TableName = "APQP";
                dsReport.Tables.Add(dtData);
                //dsReport.WriteXml("E:\\APQP.xml", XmlWriteMode.WriteSchema);
                Dictionary<string, string> dictFormula = new Dictionary<string, string>();
                dictFormula.Add("PartNo", "SFL Part No : " + PartNo);
                frmReportViewer reportViewer = new frmReportViewer(dsReport, reportName, CrystalDecisions.Shared.ExportFormatType.NoFormat, dictFormula);
                if (!reportViewer.ReadyToShowReport) return;
                reportViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}
