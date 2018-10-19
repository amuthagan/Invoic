using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.Windows.Media;
using Microsoft.Office.Interop.Visio;
using Path = System.IO.Path;
using System.Globalization;
using System.Threading;

namespace ProcessDesigner.ViewModel
{
    class ToolsQuickViewViewModel : ViewModelBase
    {
        private string dataType = "0";
        public Image DimensionsPreviewImage { get; set; }
        public Canvas DimensionsParameters { get; set; }
        public Microsoft.Windows.Controls.DataGrid RpdDataGrid { get; set; }
        public Microsoft.Windows.Controls.DataGrid dgvToolsScheduleRev { get; set; }

        //public Image DrawingsPreviewImage { get; set; }
        //public Canvas DrawingsParameters { get; set; }

        private ToolsQuickVModel _toolsQuickView = null;
        private ToolsQuickViewBll _toolQuickViewBll = null;
        private BLL.ToolInfoBLL _obllTollInfo;
        private readonly ICommand _selectionChanged;
        public ICommand OnSelectionChanged { get { return _selectionChanged; } }
        private UserInformation _userinfo = null;
        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return _closeCommand; } }

        private readonly ICommand _rowEditEndingCommand;
        public ICommand RowEditEndingCommand { get { return _rowEditEndingCommand; } }

        public Action CloseAction { get; set; }
        private ObservableCollection<DropdownColumns> _dropdownHeaders = null;
        BHCustCtrl.DataGridNumericColumn numericColumn = new BHCustCtrl.DataGridNumericColumn();
        public DataTable ParaMeterData
        {
            get;
            private set;
        }


        public DataView DtParaMeterData
        {
            get
            {
                //return this.ParaMeterData.DefaultView;
                return this.ParaMeterData.IsNotNullOrEmpty() ? this.ParaMeterData.DefaultView : null;
            }
            set
            {
                this.ParaMeterData = value.ToTable();
                NotifyPropertyChanged("DtParaMeterData");
            }
        }

        private TextBox tbInput;

        public ToolsQuickViewViewModel(UserInformation userInformation, DataView dvAddedToolCode = null)
        {
            if (userInformation == null) throw new ArgumentNullException("userInformation");

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;

            _toolsQuickView = new ToolsQuickVModel();
            _userinfo = userInformation;
            _toolQuickViewBll = new ToolsQuickViewBll(_userinfo);
            _obllTollInfo = new ToolInfoBLL(_userinfo);
            _selectionChanged = new DelegateCommand(SelectionChanged);
            _windowLoadedCommand = new DelegateCommand(WindowLoaded);
            _closeCommand = new DelegateCommand(Close);
            _rowEditEndingCommand = new DelegateCommand<object>(RowEditEnding);
            ToolsQuickView.ImageChanged = false;
            DropdownHeaders = new ObservableCollection<DropdownColumns>
            {
                new DropdownColumns { ColumnName = "FAMILY_CD", ColumnDesc = "Family Code", ColumnWidth = "2*" },
                new DropdownColumns { ColumnName = "FAMILY_NAME", ColumnDesc = "Family Name", ColumnWidth = "3*", IsDefaultSearchColumn = true }
            };

            _showRevisionsIsCheckedCommand = new DelegateCommand(ShowRevisionsIsChecked);
            this.rowEditEndingToolScheduleIssueCommand = new DelegateCommand<Object>(this.RowEditEndingToolScheduleIssue);
            this.multipleSelectionChangedToolScheduleIssueCommand = new DelegateCommand<object>(this.MultipleSelectionChangedToolScheduleIssue);
            //this.deleteToolScheduleIssueCommand = new DelegateCommand(this.MultipleDeleteToolScheduleIssue);
            this.deleteToolScheduleIssueCommand = new DelegateCommand(this.DeleteToolScheduleIssue);
            GridDvAddedToolCode = dvAddedToolCode;
            //SetGrid();
        }

        private readonly ICommand _windowLoadedCommand;
        public ICommand WindowLoadedCommand { get { return _windowLoadedCommand; } }
        private void WindowLoaded()
        {
            try
            {
                _toolQuickViewBll.GetToolFamily(ToolsQuickView);
                _toolQuickViewBll.GetToolDimension(ToolsQuickView);
                _toolQuickViewBll.GetToolIssues(ToolsQuickView);
                SelectionChanged();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public ToolsQuickVModel ToolsQuickView
        {
            get { return _toolsQuickView; }
            set
            {
                _toolsQuickView = value;
                NotifyPropertyChanged("ToolsQuickView");
            }
        }

        private bool _isFocusedFamilyCode = false;
        public bool IsFocusedFamilyCode
        {
            get { return _isFocusedFamilyCode; }
            set
            {
                _isFocusedFamilyCode = value;
                NotifyPropertyChanged("IsFocusedFamilyCode");
            }
        }

        public ObservableCollection<DropdownColumns> DropdownHeaders
        {
            get { return _dropdownHeaders; }
            set
            {
                _dropdownHeaders = value;
                NotifyPropertyChanged("DropdownHeaders");
            }

        }

        private DataView _gridDvAddedToolCode;
        public DataView GridDvAddedToolCode
        {
            get
            {
                return _gridDvAddedToolCode;
            }
            set
            {
                _gridDvAddedToolCode = value;
                NotifyPropertyChanged("GridDvAddedToolCode");
            }
        }



        private void SelectionChanged()
        {
            try
            {
                if (ToolsQuickView.DVToolFamily == null || !ToolsQuickView.FAMILY_CD.IsNotNullOrEmpty()) return;
                ToolsQuickView.picture = null;

                DimensionsPreviewImage.Source = null;
                //DrawingsPreviewImage.Source = null;

                ToolsQuickView.File_Name = "";
                ToolsQuickView.ImageChanged = false;

                _toolQuickViewBll.GetToolParameter(ToolsQuickView);

                DimensionsParameters.Children.Clear();
                //DrawingsParameters.Children.Clear();

                if (ToolsQuickView.DVToolParameter == null || ToolsQuickView.DVToolParameter.Count <= 0) return;
                foreach (DataRowView drv in ToolsQuickView.DVToolParameter)
                {
                    if (!drv["PARAMETER_CD"].IsNotNullOrEmpty() || !drv["X_COORDINATE"].IsNotNullOrEmpty()) continue;
                    TextBox tb = new TextBox
                    {
                        Name = Convert.ToString(drv["PARAMETER_CD"]),
                        ToolTip = Convert.ToString(drv["PARAMETER_NAME"]),
                        Width = ConvertTwipsToPixels(Convert.ToString(drv["DATATYPE"]) == "2" ? 1400 : 700),
                        Text = Convert.ToString(ToolsQuickView.ToolDimension.GetFieldValue(Convert.ToString(drv["PARAMETER_CD"]))),
                        Background = Brushes.LightGray,
                        IsReadOnly = true
                    };


                    tb.SetValue(Canvas.LeftProperty, ConvertTwipsToPixels(drv["X_COORDINATE"]));
                    tb.SetValue(Canvas.TopProperty, ConvertTwipsToPixels(drv["Y_COORDINATE"]));
                    if (_userinfo.UserName.ToUpper() != "DESIGNER" && _userinfo.UserName.ToUpper() != "ADMIN" && _userinfo.UserName.ToUpper() != "MANAGER")
                    {
                        tb.IsReadOnly = true;
                    }

                    DimensionsParameters.Children.Add(tb);

                    //TextBox tb1 = new TextBox
                    //{
                    //    Name = drv["PARAMETER_CD"].ToString(),
                    //    ToolTip = drv["PARAMETER_NAME"].ToString(),
                    //    Width = ConvertTwipsToPixels(drv["DATATYPE"].ToString() == "2" ? 1400 : 700),
                    //    Text = ToolsQuickView.ToolDimension.GetFieldValue(drv["PARAMETER_CD"].ToString()).ToString(),
                    //    Background = Brushes.LightGray
                    //};


                    //tb1.SetValue(Canvas.LeftProperty, ConvertTwipsToPixels(drv["X_COORDINATE"]));
                    //tb1.SetValue(Canvas.TopProperty, ConvertTwipsToPixels(drv["Y_COORDINATE"]));
                    //if (_userinfo.UserName.ToUpper() != "DESIGNER" && _userinfo.UserName.ToUpper() != "ADMIN" && _userinfo.UserName.ToUpper() != "MANAGER")
                    //{
                    //    tb1.IsReadOnly = true;
                    //}

                    //DrawingsParameters.Children.Add(tb1);
                }

                //DimensionsPreviewImage.Source = null;
                ////DrawingsPreviewImage.Source = null;

                //if (ToolsQuickView.picture == null) return;
                //if (SaveFile() != true) return;

                //System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                //MemoryStream strm = new MemoryStream();

                //using (FileStream file = new FileStream(ToolsQuickView.DisplayFile_Name, FileMode.Open, FileAccess.Read))
                //{
                //    byte[] bytes = new byte[file.Length];
                //    file.Read(bytes, 0, (int)file.Length);
                //    strm.Write(bytes, 0, (int)file.Length);
                //    file.Close();
                //    file.Dispose();
                //}
                //bitmap.BeginInit();
                //bitmap.StreamSource = strm;
                //bitmap.EndInit();
                //DimensionsPreviewImage.Source = bitmap;
                ////DrawingsPreviewImage.Source = bitmap;
                //GridDvAddedToolCode = _obllTollInfo.GetFamilyGridToolCode(ToolsQuickView.FAMILY_CD, ToolsQuickView.TOOL_CD);
                SetGrid();

            }
            catch (FileFormatException)
            {
                MessageBox.Show("Unable to load image.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GenerateBitmapImage()
        {
            try
            {
                if (ToolsQuickView.DVToolFamily == null || !ToolsQuickView.FAMILY_CD.IsNotNullOrEmpty()) return;
                //ToolsQuickView.picture = null;

                Progress.ProcessingText = PDMsg.Load;
                Progress.Start();
                DimensionsPreviewImage.Source = null;
                //DrawingsPreviewImage.Source = null;

                //ToolsQuickView.File_Name = "";
                ToolsQuickView.ImageChanged = false;

                //_toolQuickViewBll.GetToolParameter(ToolsQuickView);

                //DimensionsParameters.Children.Clear();
                ////DrawingsParameters.Children.Clear();

                //if (ToolsQuickView.DVToolParameter == null || ToolsQuickView.DVToolParameter.Count <= 0) return;
                //foreach (DataRowView drv in ToolsQuickView.DVToolParameter)
                //{
                //    if (!drv["PARAMETER_CD"].IsNotNullOrEmpty() || !drv["X_COORDINATE"].IsNotNullOrEmpty()) continue;
                //    TextBox tb = new TextBox
                //    {
                //        Name = Convert.ToString(drv["PARAMETER_CD"]),
                //        ToolTip = Convert.ToString(drv["PARAMETER_NAME"]),
                //        Width = ConvertTwipsToPixels(Convert.ToString(drv["DATATYPE"]) == "2" ? 1400 : 700),
                //        Text = Convert.ToString(ToolsQuickView.ToolDimension.GetFieldValue(Convert.ToString(drv["PARAMETER_CD"]))),
                //        Background = Brushes.LightGray
                //    };


                //    tb.SetValue(Canvas.LeftProperty, ConvertTwipsToPixels(drv["X_COORDINATE"]));
                //    tb.SetValue(Canvas.TopProperty, ConvertTwipsToPixels(drv["Y_COORDINATE"]));
                //    if (_userinfo.UserName.ToUpper() != "DESIGNER" && _userinfo.UserName.ToUpper() != "ADMIN" && _userinfo.UserName.ToUpper() != "MANAGER")
                //    {
                //        tb.IsReadOnly = true;
                //    }

                //    DimensionsParameters.Children.Add(tb);

                //    //TextBox tb1 = new TextBox
                //    //{
                //    //    Name = drv["PARAMETER_CD"].ToString(),
                //    //    ToolTip = drv["PARAMETER_NAME"].ToString(),
                //    //    Width = ConvertTwipsToPixels(drv["DATATYPE"].ToString() == "2" ? 1400 : 700),
                //    //    Text = ToolsQuickView.ToolDimension.GetFieldValue(drv["PARAMETER_CD"].ToString()).ToString(),
                //    //    Background = Brushes.LightGray
                //    //};


                //    //tb1.SetValue(Canvas.LeftProperty, ConvertTwipsToPixels(drv["X_COORDINATE"]));
                //    //tb1.SetValue(Canvas.TopProperty, ConvertTwipsToPixels(drv["Y_COORDINATE"]));
                //    //if (_userinfo.UserName.ToUpper() != "DESIGNER" && _userinfo.UserName.ToUpper() != "ADMIN" && _userinfo.UserName.ToUpper() != "MANAGER")
                //    //{
                //    //    tb1.IsReadOnly = true;
                //    //}

                //    //DrawingsParameters.Children.Add(tb1);
                //}

                DimensionsPreviewImage.Source = null;
                //DrawingsPreviewImage.Source = null;

                if (ToolsQuickView.picture == null) return;
                if (SaveFile() != true) return;

                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                MemoryStream strm = new MemoryStream();

                using (FileStream file = new FileStream(ToolsQuickView.DisplayFile_Name, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    strm.Write(bytes, 0, (int)file.Length);
                    file.Close();
                    file.Dispose();
                }
                bitmap.BeginInit();
                bitmap.StreamSource = strm;
                bitmap.EndInit();
                DimensionsPreviewImage.Source = bitmap;
                Progress.End();
                //DrawingsPreviewImage.Source = bitmap;
            }
            catch (FileFormatException)
            {
                MessageBox.Show("Unable to load image.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private Boolean SaveFile()
        {
            Document doc = null;
            InvisibleApp app = null;
            try
            {
                if (ToolsQuickView.FileType.ToUpper() != "VSD") return true;
                //if (ToolsQuickView.MimeType.ToString().IndexOf("application", StringComparison.Ordinal) < 0)
                //    return true;
                app = new InvisibleApp();
                doc = app.Documents.Open(ToolsQuickView.File_Name);
                var page = app.ActivePage;
                page.Export(ToolsQuickView.DisplayFile_Name);
                app.Quit();
                return true;
            }
            catch (Exception ex)
            {
                if (app != null) app.Quit();
                ex.LogException();
                return false;
            }
        }


        public static double ConvertTwipsToPixels(object twips)
        {
            int twip = 0;
            try
            {
                twip = Convert.ToInt32(twips);
            }
            catch (Exception)
            {
                twip = 0;
            }

            // ReSharper disable once PossibleLossOfFraction
            return twip != 0 ? twip / 15 : 0.0;
        }

        public static int ConvertPixelsToTwips(object pixels)
        {
            double twip = 0;
            try
            {
                twip = Convert.ToDouble(pixels);
            }
            catch (Exception)
            {
                twip = 0;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return twip != 0 ? Convert.ToInt16(twip * 15) : 0;
        }


        private MessageBoxResult ShowInformationMessage(string showMessage)
        {
            if (showMessage == null) return MessageBoxResult.None;
            return showMessage.IsNotNullOrEmpty() ? MessageBox.Show(showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information) : MessageBoxResult.None;
        }

        private MessageBoxResult ShowWarningMessage(string showMessage, MessageBoxButton messageBoxButton)
        {
            if (showMessage == null) return MessageBoxResult.None;
            return showMessage.IsNotNullOrEmpty() ? MessageBox.Show(showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question) : MessageBoxResult.None;
        }


        private void RowEditEnding(object parameters)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void Close()
        {
            try
            {
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string showMessage)
        {
            if (showMessage == null) return MessageBoxResult.None;
            return showMessage.IsNotNullOrEmpty() ? MessageBox.Show(showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) : MessageBoxResult.None;
        }

        private string CreateVsdFile(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            InvisibleApp app = null;
            string filewoext = "";
            try
            {
                if (filename.Length <= 0) return AppDomain.CurrentDomain.BaseDirectory + "" + filewoext + ".bmp";
                filewoext = Path.GetFileNameWithoutExtension(filename);
                ToolsQuickView.picture = new MemoryStream(File.ReadAllBytes(filename));
                File.Copy(filename, AppDomain.CurrentDomain.BaseDirectory + "" + filewoext, true);
                app = new InvisibleApp();
                var doc = app.Documents.Open(AppDomain.CurrentDomain.BaseDirectory + "" + filewoext);

                var page = app.ActivePage;
                page.Export(AppDomain.CurrentDomain.BaseDirectory + "" + filewoext + ".bmp");
                app.Quit();
                return AppDomain.CurrentDomain.BaseDirectory + "" + filewoext + ".bmp";
            }
            catch (Exception ex)
            {
                ex.LogException();
                if (app != null) app.Quit();
                return "";
            }
        }

        private void ShowImage(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            try
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                Stream strm = new FileStream(filename, FileMode.Open, FileAccess.Read);
                bitmap.BeginInit();
                bitmap.StreamSource = strm;
                bitmap.EndInit();
                DimensionsPreviewImage.Source = bitmap;
                //DrawingsPreviewImage.Source = bitmap;
                //strm.Close();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private string _oldParameterCd = "";
        public void OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }


        public void OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            TextBox tb = e.EditingElement as TextBox;
            string columnName = e.Column.SortMemberPath;
            string rowid = ((DataRowView)(e.Row.Item)).Row["ROWID"].ToString();

        }


        private readonly ICommand _showRevisionsIsCheckedCommand;
        public ICommand ShowRevisionsIsCheckedCommand { get { return _showRevisionsIsCheckedCommand; } }
        private void ShowRevisionsIsChecked()
        {
            switch (ToolsQuickView.ShowRevisions)
            {
                case true: RevisionsVisibility = Visibility.Visible;
                    break;
                case false: RevisionsVisibility = Visibility.Hidden; break;
            }

        }


        private ICommand _onSubmitCommand;
        public ICommand OnSubmitCommand
        {
            get { return _onSubmitCommand ?? (_onSubmitCommand = new RelayCommand(param => Submit(), null)); }
        }

        private void Submit(bool isUnload = false)
        {
            try
            {
                if (!ToolsQuickView.IsNotNullOrEmpty() || !ToolsQuickView.FAMILY_CD.IsNotNullOrEmpty())
                {
                    ShowInformationMessage("Tool Family Code should be entered"); return;
                }

                if (!ToolsQuickView.TOOL_CD.IsNotNullOrEmpty())
                {
                    ShowInformationMessage("Tool Code should be entered"); return;
                }

                RpdDataGrid.CommitEdit();
                GridDvAddedToolCode.Table.AcceptChanges();
                _obllTollInfo.Save(GridDvAddedToolCode.ToTable().Copy(), ToolsQuickView.FAMILY_CD);

                if (!ToolsQuickView.ToolDimension.IsNotNullOrEmpty() || !ToolsQuickView.ToolDimension.TOOL_CD.IsNotNullOrEmpty())
                {
                    ShowInformationMessage("Tool Code does not exists"); return;
                }
                dgvToolsScheduleRev.CommitEdit(); // Jeyan added - to refresh table
                ToolsQuickView.DVRevisionToolIssue.Table.AcceptChanges(); // Jeyan added- to refresh table
                List<TOOL_ISSUES> tool_issues = null;

                tool_issues = (from row in ToolsQuickView.DVRevisionToolIssue.ToTable().AsEnumerable()
                               //where String.IsNullOrEmpty(row.Field<string>("ISSUE_NO").ToValueAsString()) == true
                               // && (String.IsNullOrEmpty(row.Field<string>("ISSUE_DATE").ToValueAsString()) != true || String.IsNullOrEmpty(row.Field<string>("ALTERATIONS").ToValueAsString()) != true ||
                               // String.IsNullOrEmpty(row.Field<string>("INTL").ToValueAsString()) != true)
                               select new TOOL_ISSUES()
                               {
                                   TOOL_CD = Convert.ToString(ToolsQuickView.TOOL_CD),
                                   ISSUE_NO = Convert.ToDecimal(row.Field<string>("ISSUE_NO")),
                                   ALTERATIONS = Convert.ToString(row.Field<string>("ALTERATIONS")),
                                   INTL = Convert.ToString(row.Field<string>("INTL")),
                                   ROWID = Guid.NewGuid()
                               }).ToList<TOOL_ISSUES>();

                decimal maxIssueNo = tool_issues.Max(row => row.ISSUE_NO);
                tool_issues = tool_issues.Where(row => row.ISSUE_NO == 0 && (row.ISSUE_DATE.IsNotNullOrEmpty() || row.ALTERATIONS.IsNotNullOrEmpty() || row.INTL.IsNotNullOrEmpty())).ToList();

                if (tool_issues != null && tool_issues.Count > 1)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Issue Number")); return;
                }

                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.DateSeparator = "/";
                Thread.CurrentThread.CurrentCulture = ci;

                tool_issues = (from row in ToolsQuickView.DVRevisionToolIssue.ToTable().AsEnumerable()
                               select new TOOL_ISSUES()
                                    {
                                        TOOL_CD = Convert.ToString(ToolsQuickView.TOOL_CD),
                                        ISSUE_NO = (String.IsNullOrEmpty(row.Field<string>("ISSUE_NO").ToValueAsString()) ? ++maxIssueNo : Convert.ToDecimal(row.Field<string>("ISSUE_NO"))),
                                        ISSUE_DATE = !String.IsNullOrEmpty(row.Field<string>("ISSUE_DATE").ToValueAsString()) && row.Field<string>("ISSUE_DATE").ToValueAsString().ToDateTimeValue() != null ? row.Field<string>("ISSUE_DATE").ToValueAsString().ToDateTimeValue() : (DateTime?)null,
                                        ALTERATIONS = Convert.ToString(row.Field<string>("ALTERATIONS")),
                                        INTL = Convert.ToString(row.Field<string>("INTL")),
                                        ROWID = Guid.NewGuid()
                                    }).ToList<TOOL_ISSUES>();

                tool_issues = tool_issues.Where(o => (Convert.ToDateTime(o.ISSUE_DATE).Year != 1 || o.ISSUE_DATE == null)
                    && (o.ALTERATIONS != null && o.INTL != null)).ToList<TOOL_ISSUES>();
                if (_toolQuickViewBll.IsNotNullOrEmpty())
                {
                    if (_toolQuickViewBll.Update(tool_issues))
                    {
                        ShowInformationMessage(PDMsg.SavedSuccessfully);
                        _toolQuickViewBll.GetToolIssues(ToolsQuickView);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private ICommand _onPrintCommand;
        public ICommand OnPrintCommand
        {
            get { return _onPrintCommand ?? (_onPrintCommand = new RelayCommand(param => Print(), null)); }
        }

        private void Print(bool isUnload = false)
        {
            try
            {

                DataTable dt = _toolQuickViewBll.GetReportData(ToolsQuickView);
                dt.TableName = "DT_TOOL_DRWG_ISSUES_DIMENSION";

                DataSet dsReport = new DataSet();
                dsReport.DataSetName = "DS_TOOL_DRWG_ISSUES_DIMENSION";

                dsReport.Tables.Add(dt.Copy());

                //dt.WriteXmlSchema("D:\\" + dsReport.DataSetName + ".xml");

                if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count <= 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                    return;
                }

                frmReportViewer reportViewer = new frmReportViewer(dsReport, "TOOL_DRWG_ISSUES_DIMENSION");
                if (!reportViewer.ReadyToShowReport) return;
                reportViewer.ShowDialog();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private Visibility _imageVisibility = Visibility.Hidden;
        public Visibility ImageVisibility
        {
            get
            {
                return _imageVisibility;
            }
            set
            {
                _imageVisibility = value;
                NotifyPropertyChanged("ImageVisibility");
            }
        }

        private Visibility _canvasVisibility = Visibility.Visible;
        public Visibility CanvasVisibility
        {
            get
            {
                return _canvasVisibility;
            }
            set
            {
                _canvasVisibility = value;
                NotifyPropertyChanged("CanvasVisibility");
            }
        }

        private int _tabitemindex = 0;
        public int TabItemIndex
        {
            get { return _tabitemindex; }
            set
            {
                _tabitemindex = value;
                CanvasVisibility = Visibility.Visible;
                switch (_tabitemindex)
                {
                    case 0: ImageVisibility = Visibility.Hidden; break;
                    case 1: ImageVisibility = Visibility.Visible; GenerateBitmapImage(); break;
                }
                NotifyPropertyChanged("TabItemIndex");
            }
        }

        private Visibility _revisionsVisibility = Visibility.Hidden;
        public Visibility RevisionsVisibility
        {
            get
            {
                return _revisionsVisibility;
            }
            set
            {
                _revisionsVisibility = value;
                NotifyPropertyChanged("RevisionsVisibility");
            }
        }

        public void ToolsQuickViewViewModel_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                //if (e.Column.GetType() == typeof(DataGridTemplateColumn))
                //{
                //    if (e.Column.Header.ToString().Trim().ToUpper() == "DATE")
                //    {
                //        var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                //        if (popup != null && popup.IsOpen)
                //        {
                //            e.Cancel = true;
                //        }
                //    }
                //}

                //TextBox tb = e.EditingElement as TextBox;
                //string columnName = e.Column.SortMemberPath;

                //if (columnName == "ISSUE_NO")
                //{
                //    if (!CheckDuplicateIssueNo())
                //    {
                //        tb.Text = "";
                //        return;
                //    }
                //}
            }
            catch (Exception)
            {

            }
        }


        private static T GetVisualChild<T>(DependencyObject visual)
            where T : DependencyObject
        {
            if (visual == null)
                return null;

            var count = VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(visual, i);

                var childOfTypeT = child as T ?? GetVisualChild<T>(child);
                if (childOfTypeT != null)
                    return childOfTypeT;
            }

            return null;
        }

        private readonly ICommand multipleSelectionChangedToolScheduleIssueCommand;
        public ICommand MultipleSelectionChangedToolScheduleIssueCommand { get { return this.multipleSelectionChangedToolScheduleIssueCommand; } }
        private void MultipleSelectionChangedToolScheduleIssue(object selectedItems)
        {
            try
            {
                //_selectedItemsToolSchedSubModel = new List<ToolSchedSubModel>();
                //ToolsQuickView.ToolIssuesSelectedRow = (DataRowView)selectedItems;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand rowEditEndingToolScheduleIssueCommand;
        public ICommand RowEditEndingToolScheduleIssueCommand { get { return this.rowEditEndingToolScheduleIssueCommand; } }
        private void RowEditEndingToolScheduleIssue(Object selecteditem)
        {
            try
            {
                //TOOL_SCHED_ISSUE toolsubissue = new TOOL_SCHED_ISSUE();
                //toolsubissue = (TOOL_SCHED_ISSUE)selecteditem;
                //if (toolsubissue.IDPK == 0 && (toolsubissue.TS_ISSUE_NO.ToValueAsString().Trim() != ""
                //            || toolsubissue.TS_ISSUE_DATE.ToValueAsString() != ""
                //            || toolsubissue.TS_ISSUE_ALTER.ToValueAsString() != ""
                //            || toolsubissue.TS_COMPILED_BY.ToValueAsString() != ""))
                //{
                //    toolsubissue.IDPK = -1;
                //    FilterToolScheduleIssue();
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand deleteToolScheduleIssueCommand;
        public ICommand DeleteToolScheduleIssueCommand { get { return this.deleteToolScheduleIssueCommand; } }
        private void DeleteToolScheduleIssue()
        {
            try
            {
                if (ToolsQuickView.ToolIssuesSelectedRow != null)
                {

                    DataRowView drv = ToolsQuickView.ToolIssuesSelectedRow;
                    TOOL_ISSUES toolIssue = new TOOL_ISSUES
                    {
                        TOOL_CD = drv["TOOL_CD"].ToValueAsString(),
                        ISSUE_NO = drv["ISSUE_NO"].ToValueAsString().ToDecimalValue(),
                        ISSUE_DATE = !String.IsNullOrEmpty(drv["ISSUE_DATE"].ToValueAsString()) && drv["ISSUE_DATE"].ToValueAsString().ToDateTimeValue() != null ? drv["ISSUE_DATE"].ToValueAsString().ToDateTimeValue() : (DateTime?)null,
                        ALTERATIONS = drv["ALTERATIONS"].ToValueAsString(),
                        INTL = drv["INTL"].ToValueAsString(),
                        ROWID = Guid.NewGuid()
                    };

                    List<TOOL_ISSUES> tool_issues = new List<TOOL_ISSUES> { toolIssue };
                    if (_toolQuickViewBll.IsNotNullOrEmpty())
                    {
                        if (_toolQuickViewBll.Delete(tool_issues))
                        {
                            _toolQuickViewBll.GetToolIssues(ToolsQuickView);
                            ShowInformationMessage(PDMsg.DeletedSuccessfully);
                            return;
                        }
                    }

                    ////Bug Id : 693
                    //if (ToolsQuickView.ToolIssuesSelectedRow.TS_ISSUE_NO.ToValueAsString().Trim() == ""
                    //&& SelectedToolSchedIssue.TS_ISSUE_DATE.ToValueAsString() == ""
                    //&& SelectedToolSchedIssue.TS_ISSUE_ALTER.ToValueAsString() == ""
                    //&& SelectedToolSchedIssue.TS_COMPILED_BY.ToValueAsString() == "")
                    //{
                    //    if (SelectedToolSchedIssue.IDPK == ToolScheduleRevision[ToolScheduleRevision.Count - 1].IDPK)
                    //    {
                    //        return;
                    //    }
                    //}

                    //if (ShowQuestionMessage(PDMsg.SelectDeleteRecord) == MessageBoxResult.OK)
                    //{
                    //    if (SelectedToolSchedIssue.IDPK > 0)
                    //    {
                    //        DeleteToolScheduleIssueAll.Add(SelectedToolSchedIssue);
                    //    }
                    //    ToolScheduleRevisionAll.Remove(SelectedToolSchedIssue);
                    //    FilterToolScheduleIssue();
                    //    if (ToolScheduleRevision != null)
                    //    {
                    //        if (ToolScheduleRevision.Count > 0)
                    //        {
                    //            SelectedToolSchedIssue = ToolScheduleRevision[0];
                    //            NotifyPropertyChanged("SelectedToolSchedIssue");
                    //        }
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void MultipleDeleteToolScheduleIssue()
        {
            try
            {
                List<TOOL_SCHED_ISSUE> lstTsi;
                int label = 0;
                //if (_selectedItemsToolScheduleIssue != null)
                //{
                //    //Bug Id : 707
                //    lstTsi = new List<TOOL_SCHED_ISSUE>() { };
                //    System.Collections.IList items = (System.Collections.IList)_selectedItemsToolScheduleIssue;
                //    IEnumerable<TOOL_SCHED_ISSUE> collection = items.Cast<TOOL_SCHED_ISSUE>();
                //    lstTsi = collection.ToList<TOOL_SCHED_ISSUE>();
                //    if (lstTsi.Count == 0) { return; }
                //    if (ShowQuestionMessage(PDMsg.SelectDeleteRecord) == MessageBoxResult.Cancel)
                //    {
                //        return;
                //    }

                //    foreach (TOOL_SCHED_ISSUE tsi in lstTsi)
                //    {
                //        if (tsi.TS_ISSUE_NO.ToValueAsString().Trim() == ""
                //        && tsi.TS_ISSUE_DATE.ToValueAsString() == ""
                //        && tsi.TS_ISSUE_ALTER.ToValueAsString() == ""
                //        && tsi.TS_COMPILED_BY.ToValueAsString() == "")
                //        {
                //            if (tsi.IDPK == ToolScheduleRevision[ToolScheduleRevision.Count - 1].IDPK)
                //            {
                //                goto NextRecord;
                //            }
                //        }

                //        if (tsi.IDPK > 0)
                //        {
                //            DeleteToolScheduleIssueAll.Add(tsi);
                //        }
                //        ToolScheduleRevisionAll.Remove(tsi);
                //    NextRecord:
                //        label = label + 1;
                //    }
                //    FilterToolScheduleIssue();
                //    if (ToolScheduleRevision != null)
                //    {
                //        if (ToolScheduleRevision.Count > 0)
                //        {
                //            SelectedToolSchedIssue = ToolScheduleRevision[0];
                //            NotifyPropertyChanged("SelectedToolSchedIssue");
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void SetGrid()
        {
            int ictr = 0;
            DataTable dtParameter;
            DataRow[] drRow;
            try
            {
                //return;
                if (GridDvAddedToolCode != null)
                {

                    this.DtParaMeterData = _obllTollInfo.GetParameterCode(ToolsQuickView.FAMILY_CD);
                    dtParameter = DtParaMeterData.ToTable().Copy();
                    foreach (Microsoft.Windows.Controls.DataGridTextColumn dgColumn in RpdDataGrid.Columns)
                    {
                        if (dgColumn.SortMemberPath.ToUpper() != "TOOL_CD")
                        {
                            drRow = dtParameter.Select("PARAMETER_CD = '" + dgColumn.SortMemberPath + "'");
                            if (drRow.Length == 0)
                            {
                                dgColumn.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                dgColumn.Header = drRow[0]["PARAMETER_NAME"].ToString().Trim();
                                dgColumn.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            dgColumn.Header = "Tool Code";
                            dgColumn.Width = 100;
                        }
                    }
                    RpdDataGrid.FrozenColumnCount = 3;
                    RpdDataGrid.Columns[0].IsReadOnly = true;
                    RpdDataGrid.Columns[1].IsReadOnly = true;
                    RpdDataGrid.Columns[2].IsReadOnly = true;
                }
                else
                {
                    RpdDataGrid.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                //throw ex.LogException();
            }
        }

        public void rpdDataGrid_PreparingCellForEdit(object sender, Microsoft.Windows.Controls.DataGridPreparingCellForEditEventArgs e)
        {
            try
            {
                tbInput = e.EditingElement as System.Windows.Controls.TextBox;
                tbInput.MaxLength = 22;
                CheckDataType(e.Column.SortMemberPath);
                if (tbInput != null)
                {
                }
            }
            catch (Exception ex)
            {

            }
        }


        public void rpdDataGrid_CellEditEnding(object sender, Microsoft.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                CheckDataType(e.Column.SortMemberPath);
                if (dataType == "1")
                {
                    TextBox txtBox = (TextBox)e.EditingElement;
                    string columnName = "";
                    ////System.Windows.Controls.DataGridTextColumn dgColumn;
                    ////int columnIndex = -1;
                    ////columnIndex = DgToolInfo.CurrentCell.Column.DisplayIndex;
                    ////dgColumn = (System.Windows.Controls.DataGridTextColumn)DgToolInfo.Columns[columnIndex];
                    ////columnName = dgColumn.SortMemberPath;
                    ////columnIndex = DgToolInfo.CurrentCell.Column.DisplayIndex;
                    ////dgColumn = (System.Windows.Controls.DataGridTextColumn)DgToolInfo.Columns[columnIndex];
                    columnName = e.Column.SortMemberPath;
                    txtBox.Text = checkLostFocus(txtBox.Text.Trim(), columnName);
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void CheckDataType(string sortMemberPath)
        {
            try
            {
                DataRow[] drRow;
                string columnName = "";
                System.Windows.Controls.DataGridTextColumn dgColumn;
                //int columnIndex = -1;
                //columnIndex = DgToolInfo.CurrentCell.Column.DisplayIndex;
                //dgColumn = (System.Windows.Controls.DataGridTextColumn)DgToolInfo.Columns[columnIndex];
                columnName = sortMemberPath;
                drRow = DtParaMeterData.ToTable().Select("PARAMETER_CD = '" + columnName + "'");
                if (drRow.Length > 0)
                {
                    dataType = drRow[0]["DATATYPE"].ToValueAsString();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void CheckDataTypeAndValidateForTextBox(object sender, KeyEventArgs e, string columnName)
        {
            try
            {
                string localDataType = "0";
                DataRow[] drRow;
                if (DtParaMeterData != null)
                {
                    drRow = DtParaMeterData.ToTable().Select("PARAMETER_CD = '" + columnName + "'");
                    if (drRow.Length > 0)
                    {
                        localDataType = drRow[0]["DATATYPE"].ToValueAsString();
                    }
                    if (localDataType == "1")
                    {
                        validateNumericValue(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void validateNumericValue(object sender, KeyEventArgs e)
        {
            Key keypressed;
            try
            {
                if (((TextBox)sender).Text.ToString().Trim().IndexOf(".") > -1 && (e.Key == Key.OemPeriod || e.Key == Key.Decimal))
                {
                    e.Handled = true;
                    return;
                }
                if ((Keyboard.Modifiers == ModifierKeys.Shift && e.Key >= Key.D0 && e.Key <= Key.D9))
                {
                    e.Handled = true;
                    return;
                }
                if (e.Key >= Key.D0 && e.Key <= Key.D9)
                {
                    keypressed = e.Key;
                }
                else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) { keypressed = e.Key; } // it`s number
                else if (e.Key == Key.Escape || e.Key == Key.Tab || e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.LeftCtrl ||
                    e.Key == Key.LWin || e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.RightCtrl || e.Key == Key.RightShift ||
                    e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.Return || e.Key == Key.Delete ||
                    e.Key == Key.System || e.Key == Key.Back)
                {
                    keypressed = e.Key;
                }
                else if (e.Key == Key.OemPeriod || e.Key == Key.Decimal || e.Key == Key.Delete || e.Key == Key.Home || e.Key == Key.End)
                {
                    keypressed = e.Key;
                }
                else
                {
                    keypressed = e.Key;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private string checkLostFocus(string value, string codeParam)
        {
            try
            {
                string localDataType = "0";
                DataRow[] drRow;
                drRow = DtParaMeterData.ToTable().Select("PARAMETER_CD = '" + codeParam + "'");
                if (drRow.Length > 0)
                {
                    localDataType = drRow[0]["DATATYPE"].ToValueAsString();
                }
                if (localDataType == "1")
                {
                    if (value.Length > 0)
                    {
                        if (value.Length == value.IndexOf(".") + 1 && value.Length > 1)
                        {
                            value = value + "00";
                        }
                        if (numericColumn.IsNumeric(value))
                        {

                        }
                        else
                        {
                            ShowInformationMessage("Invalid Number!");
                            value = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return value;
        }
        public void rpdDataGrid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (dataType == "1")
            {
                if (numericColumn.IsNumeric(tbInput.Text.Trim()))
                {

                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        public void rpdDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key keypressed;
            try
            {
                if (dataType == "1")
                {
                    if (tbInput.Text.ToString().Trim().IndexOf(".") > -1 && (e.Key == Key.OemPeriod || e.Key == Key.Decimal))
                    {
                        e.Handled = true;
                        return;
                    }
                    if ((Keyboard.Modifiers == ModifierKeys.Shift && e.Key >= Key.D0 && e.Key <= Key.D9))
                    {
                        e.Handled = true;
                        return;
                    }
                    if (e.Key >= Key.D0 && e.Key <= Key.D9)
                    {
                        keypressed = e.Key;
                    }
                    else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) { keypressed = e.Key; } // it`s number
                    else if (e.Key == Key.Escape || e.Key == Key.Tab || e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.LeftCtrl ||
                        e.Key == Key.LWin || e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.RightCtrl || e.Key == Key.RightShift ||
                        e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.Return || e.Key == Key.Delete ||
                        e.Key == Key.System || e.Key == Key.Back)
                    {
                        keypressed = e.Key;
                    }
                    else if (e.Key == Key.OemPeriod || e.Key == Key.Decimal || e.Key == Key.Delete || e.Key == Key.Home || e.Key == Key.End)
                    {
                        keypressed = e.Key;
                    }
                    else
                    {
                        keypressed = e.Key;
                        e.Handled = true;
                    }
                }
                //SearchText1 = e.Key.ToString();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
