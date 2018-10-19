using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using WPF.MDI;

namespace ProcessDesigner.ViewModel
{
    class ReportMISFeatureWiseViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "FEATURE WISE REPORT";
        private const string REPORT_NAME = "FEATURE_WISE_REPORT";
        private const string REPORT_TITLE = "Feature Wise Report";
        ReportMISFeatureWise bll = null;
        WPF.MDI.MdiChild mdiChild = null;
        //private readonly ICommand mousecombocommand;
        //public ICommand MouseComboCommand { get { return this.mousecombocommand; } }

        public ReportMISFeatureWiseViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, PCCS feature = null, PCCS feature1 = null, PCCS feature2 = null, PCCS specification = null, PRD_MAST productMaster = null, bool refreshOnLoad = false, string title = REPORT_TITLE)
        {
            _userInformation = userInformation;
            this.mdiChild = mdiChild;

            bll = new ReportMISFeatureWise(userInformation);
            MandatoryFields = new ReportMISFeatureWiseModel();
            MandatoryFields.GRID_TITLE = REPORT_TITLE;

            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            //this.mousecombocommand = new DelegateCommand(this.MouseDblClick);
            this.refreshCommand = new DelegateCommand(this.RefreshSubmitCommand);
            this.exportToExcelCommand = new DelegateCommand(this.ExportToExcelSubmitCommand);

            //FeatureDataSource = bll.GetFeature().ToDataTable<PCCS>().DefaultView;
            FeatureDataSource = bll.GetFeatureDataTable().DefaultView;

            this._featureSelectedItemChangedCommand = new DelegateCommand(this.FeatureChanged);
            FeatureDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "FEATURE", ColumnDesc = "Feature", ColumnWidth = "1*" },
                        };


            Feature1DataSource = FeatureDataSource.Table.Copy().DefaultView;
            this._feature1SelectedItemChangedCommand = new DelegateCommand(this.Feature1Changed);
            Feature1DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "FEATURE", ColumnDesc = "Feature", ColumnWidth = "1*" },
                        };

            Feature2DataSource = FeatureDataSource.Table.Copy().DefaultView;
            this._feature2SelectedItemChangedCommand = new DelegateCommand(this.Feature2Changed);
            Feature2DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "FEATURE", ColumnDesc = "Feature", ColumnWidth = "1*" },
                        };

        }


        //private void MouseDblClick()
        //{
        //    DataSet dsReport = DsReport;
        //    if (dsReport.Tables[0].Rows[0]["PART_NO"].ToString().IsNotNullOrEmpty())
        //    {
        //        MdiChild frmProductInformationChild = new MdiChild
        //        {
        //            Title = ApplicationTitle + " - Product Master",
        //            MaximizeBox = false,
        //            MinimizeBox = false
        //        };

        //        ProcessDesigner.frmProductInformation productInformation = new ProcessDesigner.frmProductInformation(_userInformation,
        //            frmProductInformationChild, -99999, OperationMode.Edit);
        //        frmProductInformationChild.Content = productInformation;
        //        frmProductInformationChild.Height = productInformation.Height + 50;
        //        frmProductInformationChild.Width = productInformation.Width + 20;
        //        MainMDI.Container.Children.Add(frmProductInformationChild);
        //    }
        //}

        string partno = "";
        public void DataGridResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRowView selecteditem = (DataRowView)((System.Windows.Controls.DataGrid)(sender)).CurrentItem;
                DataGridColumn column = ((System.Windows.Controls.DataGrid)(sender)).CurrentColumn;
                if (selecteditem != null && column != null)
                {
                    string columnName = column.SortMemberPath;
                    if (columnName == "PART_NO")
                    {
                        Progress.Start();
                        partno = selecteditem["PART_NO"].ToString().Trim();
                        MdiChild frmProductInformationChild = new MdiChild
                        {
                            Title = ApplicationTitle + " - Product Master",
                            MaximizeBox = false,
                            MinimizeBox = false
                        };

                        ProcessDesigner.frmProductInformation productInformation = new ProcessDesigner.frmProductInformation(_userInformation,
                            frmProductInformationChild, selecteditem["IDPK"].ToString().ToIntValue(), OperationMode.Edit);
                        frmProductInformationChild.Content = productInformation;
                        frmProductInformationChild.Height = productInformation.Height + 50;
                        frmProductInformationChild.Width = productInformation.Width + 20;
                        MainMDI.Container.Children.Add(frmProductInformationChild);
                        Progress.End();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (IsChangesMade())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        Save();
                //        return;
                //    }
                //}

                //WPF.MDI.ClosingEventArgs closingev;
                //closingev = (WPF.MDI.ClosingEventArgs)e;
                //if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                //{
                //    closingev.Cancel = true;
                //    e = closingev;
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private string _title = REPORT_TITLE;
        private string Title
        {
            get { return _title; }
            set
            {
                value = value.IsNotNullOrEmpty() ? value : REPORT_TITLE;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {
            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();

            PCCS feature = new PCCS() { FEATURE = MandatoryFields.FEATURE };
            PCCS feature1 = new PCCS() { FEATURE = MandatoryFields.FEATURE1 };
            PCCS feature2 = new PCCS() { FEATURE = MandatoryFields.FEATURE2 };
            PCCS specification = new PCCS() { SPEC_MIN = MandatoryFields.SPEC_MIN, SPEC_MAX = MandatoryFields.SPEC_MAX };
            PRD_MAST productMaster = new PRD_MAST() { PART_DESC = MandatoryFields.PART_DESC };

            DataSet dsReport = DsReport;
            if (!DsReport.IsNotNullOrEmpty() || !DsReport.Tables.IsNotNullOrEmpty() || DsReport.Tables.Count == 0)
                RefreshSubmitCommand();
            //if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            //{
            //    ShowInformationMessage(PDMsg.NoRecordsPrint);
            //    return;
            //}
            if (MandatoryFields.GridData.IsNotNullOrEmpty())
                if (MandatoryFields.GridData.Count <= 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                    return;
                }


            //dsReport.DataSetName = REPORT_NAME;
            //MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
            //MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";
            //DataRow row = dsReport.Tables[1].Rows.Add();

            //row["ReportTitle"] = REPORT_TITLE;
            //row.AcceptChanges();
            //dsReport.Tables[1].AcceptChanges();

            //dsReport.WriteXmlSchema("D:\\" + dsReport.DataSetName + ".xml");

            frmMISInputBox inp = new frmMISInputBox("Report Title", "Enter the Title of the Report");
            inp.ShowDialog();
            //inp.Txt_InputBox.Text;

            frmReportViewer reportViewer = new frmReportViewer(dsReport, REPORT_NAME, inp.Txt_InputBox.Text);
            Progress.End();
            if (!reportViewer.ReadyToShowReport) return;
            reportViewer.ShowDialog();
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshClickCommand { get { return this.refreshCommand; } }
        private void RefreshSubmitCommand()
        {
            try
            {
                Progress.ProcessingText = PDMsg.Search;
                Progress.Start();
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                PCCS feature = new PCCS() { FEATURE = MandatoryFields.FEATURE };
                PCCS feature1 = new PCCS() { FEATURE = MandatoryFields.FEATURE1 };
                PCCS feature2 = new PCCS() { FEATURE = MandatoryFields.FEATURE2 };
                PCCS specification = new PCCS() { SPEC_MIN = MandatoryFields.SPEC_MIN, SPEC_MAX = MandatoryFields.SPEC_MAX };
                PRD_MAST productMaster = new PRD_MAST() { PART_DESC = MandatoryFields.PART_DESC };

                DsReport = bll.GetAllFeatures(feature, feature1, feature2, specification, productMaster);
                DataSet dsReport = DsReport;

                MandatoryFields.GRID_TITLE = REPORT_TITLE;
                if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
                {
                    Mouse.OverrideCursor = null;
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                    return;
                }

                dsReport.DataSetName = REPORT_NAME;
                MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
                MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";
                DataRow row = dsReport.Tables[1].Rows.Add();

                row["ReportTitle"] = REPORT_TITLE;
                row.AcceptChanges();
                dsReport.Tables[1].AcceptChanges();
                Progress.End();
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                ex.LogException();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
            //dsReport.WriteXmlSchema("D:\\" + dsReport.DataSetName + ".xml");
        }

        private readonly ICommand exportToExcelCommand;
        public ICommand ExportToExcelClickCommand { get { return this.exportToExcelCommand; } }
        private void ExportToExcelSubmitCommand()
        {
            try
            {
                PCCS feature = new PCCS() { FEATURE = MandatoryFields.FEATURE };
                PCCS feature1 = new PCCS() { FEATURE = MandatoryFields.FEATURE1 };
                PCCS feature2 = new PCCS() { FEATURE = MandatoryFields.FEATURE2 };
                PCCS specification = new PCCS() { SPEC_MIN = MandatoryFields.SPEC_MIN, SPEC_MAX = MandatoryFields.SPEC_MAX };
                PRD_MAST productMaster = new PRD_MAST() { PART_DESC = MandatoryFields.PART_DESC };

                DataSet dsReport = DsReport;
                if (!DsReport.IsNotNullOrEmpty() || !DsReport.Tables.IsNotNullOrEmpty() || DsReport.Tables.Count == 0)
                    dsReport = bll.GetAllFeatures(feature, feature1, feature2, specification, productMaster);
                //MandatoryFields.GRID_TITLE = REPORT_TITLE;
                if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                    return;
                }

                //dsReport.DataSetName = REPORT_NAME;
                //MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
                //MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";
                //DataRow row = dsReport.Tables[1].Rows.Add();

                //row["ReportTitle"] = REPORT_TITLE;
                //row.AcceptChanges();
                //dsReport.Tables[1].AcceptChanges();

                //dsReport.WriteXmlSchema("D:\\" + dsReport.DataSetName + ".xml");

                //frmReportViewer reportViewer = new frmReportViewer(dsReport, REPORT_NAME, CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                //if (!reportViewer.ReadyToShowReport) return;
                string reportName = GetReportPath() + "FeatureWiseExport.rpt";
                if (BindReport(dsReport, reportName, CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook) == true) // If condition added by Jeyan
                {
                    ShowInformationMessage("Exported to Excel File Successfully.");
                }
                //reportViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowWarningMessage(ex.Message, MessageBoxButton.OK);
            }
        }

        private bool fileExists(string fileName)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                ShowInformationMessage("Report File '" + fileName + "' does not exists!");
            }
            return fileInfo.Exists;
        }

        private string GetReportPath()
        {
            string reportPath = "";
            string reportPathNew = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (Assembly.GetExecutingAssembly().IsDebug() || reportPathNew.Contains("\\bin\\Debug"))
            {
                DirectoryInfo d = new DirectoryInfo(reportPathNew);
                reportPathNew = d.Parent.Parent.FullName;
            }
            reportPath = reportPathNew + "\\Reports\\";
            return reportPath;
        }


        private bool BindReport(DataSet dsReport, string reportPath, ExportFormatType exportFormatType = ExportFormatType.NoFormat)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                if (exportFormatType != ExportFormatType.NoFormat)
                {
                    switch (exportFormatType)
                    {
                        case ExportFormatType.Excel:
                        case ExportFormatType.ExcelRecord:
                        case ExportFormatType.ExcelWorkbook:
                            string fileName = "";
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.AddExtension = true;
                            saveFileDialog.CheckPathExists = true;
                            saveFileDialog.DefaultExt = ".xlsx";
                            saveFileDialog.Filter = "Excel files (*.xls or .xlsx)|.xls;*.xlsx";
                            System.IO.DirectoryInfo di = new DirectoryInfo(GetReportPath() + "\\ExportToExcel");
                            if (!di.Exists)
                            {
                                try
                                {
                                    di.Create();
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                }
                            }

                            saveFileDialog.InitialDirectory = di.FullName;
                            saveFileDialog.Title = "Export to " + Enum.GetName(typeof(ExportFormatType), exportFormatType).ToString();
                            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return bReturnValue;
                            fileName = saveFileDialog.FileName;

                            if (!fileName.IsNotNullOrEmpty()) return bReturnValue;
                            ReportDocument reportDocument = new ReportDocument();
                            reportDocument.Load(reportPath);
                            if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                            {
                                reportDocument.SetDataSource(dsReport);
                            }
                            reportDocument.ExportToDisk(exportFormatType, fileName);
                            break;
                    }
                }
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return bReturnValue;
        }


        private DataSet _dsReport = null;
        public DataSet DsReport
        {
            get
            {
                return _dsReport;
            }
            set
            {
                _dsReport = value;
                NotifyPropertyChanged("DsReport");
            }
        }

        private ReportMISFeatureWiseModel _mandatoryFields = null;
        public ReportMISFeatureWiseModel MandatoryFields
        {
            get
            {
                return _mandatoryFields;
            }
            set
            {
                _mandatoryFields = value;
                NotifyPropertyChanged("MandatoryFields");
            }
        }


        private DataView _featureDataSource = null;
        public DataView FeatureDataSource
        {
            get
            {
                return _featureDataSource;
            }
            set
            {
                _featureDataSource = value;
                NotifyPropertyChanged("FeatureDataSource");
            }
        }

        private DataRowView _featureSelectedRow;
        public DataRowView FeatureSelectedRow
        {
            get
            {
                return _featureSelectedRow;
            }

            set
            {
                _featureSelectedRow = value;
            }
        }

        private readonly ICommand _featureSelectedItemChangedCommand;
        public ICommand FeatureSelectedItemChangedCommand { get { return this._featureSelectedItemChangedCommand; } }
        private void FeatureChanged()
        {
            if (_featureSelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.FEATURE = _featureSelectedRow.Row["FEATURE"].ToValueAsString();
            }
        }

        private Visibility _featureHasDropDownVisibility = Visibility.Visible;
        public Visibility FeatureHasDropDownVisibility
        {
            get { return _featureHasDropDownVisibility; }
            set
            {
                _featureHasDropDownVisibility = value;
                NotifyPropertyChanged("FeatureHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _featureDropDownItems;
        public ObservableCollection<DropdownColumns> FeatureDropDownItems
        {
            get
            {
                return _featureDropDownItems;
            }
            set
            {
                _featureDropDownItems = value;
                OnPropertyChanged("FeatureDropDownItems");
            }
        }

        private DataView _feature1DataSource = null;
        public DataView Feature1DataSource
        {
            get
            {
                return _feature1DataSource;
            }
            set
            {
                _feature1DataSource = value;
                NotifyPropertyChanged("Feature1DataSource");
            }
        }

        private DataRowView _feature1SelectedRow;
        public DataRowView Feature1SelectedRow
        {
            get
            {
                return _feature1SelectedRow;
            }

            set
            {
                _feature1SelectedRow = value;
            }
        }

        private readonly ICommand _feature1SelectedItemChangedCommand;
        public ICommand Feature1SelectedItemChangedCommand { get { return this._feature1SelectedItemChangedCommand; } }
        private void Feature1Changed()
        {
            if (_feature1SelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.FEATURE = _feature1SelectedRow.Row["FEATURE"].ToValueAsString();
            }
        }

        private Visibility _feature1HasDropDownVisibility = Visibility.Visible;
        public Visibility Feature1HasDropDownVisibility
        {
            get { return _feature1HasDropDownVisibility; }
            set
            {
                _feature1HasDropDownVisibility = value;
                NotifyPropertyChanged("Feature1HasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _feature1DropDownItems;
        public ObservableCollection<DropdownColumns> Feature1DropDownItems
        {
            get
            {
                return _feature1DropDownItems;
            }
            set
            {
                _feature1DropDownItems = value;
                OnPropertyChanged("Feature1DropDownItems");
            }
        }

        private DataView _feature2DataSource = null;
        public DataView Feature2DataSource
        {
            get
            {
                return _feature2DataSource;
            }
            set
            {
                _feature2DataSource = value;
                NotifyPropertyChanged("Feature2DataSource");
            }
        }

        private DataRowView _feature2SelectedRow;
        public DataRowView Feature2SelectedRow
        {
            get
            {
                return _feature2SelectedRow;
            }

            set
            {
                _feature2SelectedRow = value;
            }
        }

        private readonly ICommand _feature2SelectedItemChangedCommand;
        public ICommand Feature2SelectedItemChangedCommand { get { return this._feature2SelectedItemChangedCommand; } }
        private void Feature2Changed()
        {
            if (_feature2SelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.FEATURE = _feature2SelectedRow.Row["FEATURE"].ToValueAsString();
            }
        }

        private Visibility _feature2HasDropDownVisibility = Visibility.Visible;
        public Visibility Feature2HasDropDownVisibility
        {
            get { return _feature2HasDropDownVisibility; }
            set
            {
                _feature2HasDropDownVisibility = value;
                NotifyPropertyChanged("Feature2HasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _feature2DropDownItems;
        public ObservableCollection<DropdownColumns> Feature2DropDownItems
        {
            get
            {
                return _feature2DropDownItems;
            }
            set
            {
                _feature2DropDownItems = value;
                OnPropertyChanged("Feature2DropDownItems");
            }
        }


        public object CloseAction { get; set; }
    }
}
