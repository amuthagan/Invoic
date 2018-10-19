using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using ProcessDesigner.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using System.Windows.Media.Imaging;
using System.IO;
using ProcessDesigner.DAL;

namespace ProcessDesigner.ViewModel
{
    public class ToolInfoViewModel : ViewModelBase
    {

        private BLL.ToolInfoBLL obllTollInfo;
        private Model.TollInfoModel modelToolInfo;
        private Model.ToolAdminModel toolAdm;
        private UserInformation userinformation;
        private readonly ICommand selectChangeComboCommand;
        private readonly ICommand addClickCommand;
        private readonly ICommand saveCommand;
        private readonly ICommand refreshClickCommand;
        private readonly ICommand showDrawingscommand;
        private readonly ICommand doubleClickCommand;
        private readonly ICommand copyToolCodeCommand;
        private readonly ICommand deleteToolCodeCommand;
        private readonly ICommand closeCommand;
        private TextBox tbInput;
        private string dataType = "0";
        string sort = "";
        private List<string> addedCode = new List<string>();
        private Dictionary<string, string> sortColumn = new Dictionary<string, string>();

        BHCustCtrl.DataGridNumericColumn numericColumn = new BHCustCtrl.DataGridNumericColumn();
        public ToolInfoViewModel()
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            obllTollInfo = new ToolInfoBLL(userinformation);
            toolAdm = new ToolAdminModel();
            modelToolInfo = new TollInfoModel();
            //this.DtDataview = obllTollInfo.GetFamilyCode();
            this.DtDataview = GetToolFamilyCode();
            this.refreshClickCommand = new DelegateCommand(this.SearchCriteria);
            this.addClickCommand = new DelegateCommand(this.AddSubmitCommand);
            this.showDrawingscommand = new DelegateCommand(this.ShowDrawings);
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            this.doubleClickCommand = new DelegateCommand<Object>(this.GridDoubleClickCommand);
            this.saveCommand = new DelegateCommand(this.Save);
            this.copyToolCodeCommand = new DelegateCommand(this.CopyToolCode);
            this.deleteToolCodeCommand = new DelegateCommand(this.DeleteToolCode);
            this.closeCommand = new DelegateCommand(this.Close);
            Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "FAMILY_CD", ColumnDesc = "Family Code", ColumnWidth = 100 },
                            new DropdownColumns() { ColumnName = "FAMILY_NAME", ColumnDesc = "Family Name", ColumnWidth = "1*" }
                        };
            SetdropDownItems();

            DataTable symbols = new DataTable();
            symbols.Columns.Add("Search");

            DataRow dr;
            dr = symbols.NewRow();
            dr[0] = " ";
            symbols.Rows.Add(dr);

            dr = symbols.NewRow();
            dr[0] = "<";
            symbols.Rows.Add(dr);

            dr = symbols.NewRow();
            dr[0] = ">";
            symbols.Rows.Add(dr);

            dr = symbols.NewRow();
            dr[0] = "<=";
            symbols.Rows.Add(dr);

            dr = symbols.NewRow();
            dr[0] = ">=";
            symbols.Rows.Add(dr);

            dr = symbols.NewRow();
            dr[0] = "<>";
            symbols.Rows.Add(dr);

            dr = symbols.NewRow();
            dr[0] = "Like";
            symbols.Rows.Add(dr);
            symbols.AcceptChanges();
            DtSymbols = symbols.DefaultView;
        }


        public Action CloseAction { get; set; }

        public System.IO.MemoryStream picture { get; set; }

        private string _displayFile_Name = "";
        public string DisplayFile_Name
        {
            get { return _displayFile_Name; }
            set
            {
                _displayFile_Name = value;
                NotifyPropertyChanged("DisplayFile_Name");
            }
        }

        //---------------
        public string Chk_Toolstr = string.Empty;
        public string Chk_Tool(string tool_code)
        {

            string pToolCode = null;
            int pLength = 0;
            string prefix = null;
            int start = 0;
            string toolcode;
            int length = 0;


            pToolCode = tool_code.Replace(" ", "");

            //PToolCode = Rem_Spc(Tool_Code);
            pLength = pToolCode.ToString().Length;
            if (pToolCode.Substring(0, 1) == "E")
            {
                toolcode = pToolCode.Substring(2);
                prefix = pToolCode.Substring(0, 2) + " ";
            }
            else
            {
                toolcode = pToolCode;
                prefix = "";
            }

            length = toolcode.ToString().Length;

            if (string.IsNullOrEmpty(prefix))
            {
                start = 0;
            }
            else
            {
                start = 1;
            }

            //int i = 0;        
            //string firstChar = string.Empty;
            //for (i = start; i < length; i++)
            //{
            //    firstChar = pToolCode.Substring(i, 1).ToString();

            //    int n;
            //    bool isNumeric = int.TryParse(firstChar, out n);


            //    if (isNumeric == false)
            //    {
            //        Chk_Toolstr = "";
            //        return Chk_Toolstr;
            //    }
            //}


            switch (length)
            {
                case 15:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 6) + " " + toolcode.Substring(6, 1) + " " + toolcode.Substring(7, 2) + " " + toolcode.Substring(9, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    //Chk_Tool =  Prefix & Left$(toolcode, 6) & " " & Mid$(toolcode, 7, 1) & " " & Mid$(toolcode, 8, 2) & " " & Mid$(toolcode, 10, 2) & " " & Right$(toolcode, 4)
                    break;
                case 12:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 3) + " " + toolcode.Substring(3, 1) + " " + toolcode.Substring(4, 2) + " " + toolcode.Substring(6, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    break;
                case 11:
                    if (toolcode.Substring(0, 1) == "0")
                    {
                        Chk_Toolstr = prefix + toolcode.Substring(0, 3) + " " + toolcode.Substring(3, 1) + " " + toolcode.Substring(4, 2) + " " + toolcode.Substring(6, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(3, toolcode.Length));
                        //    Chk_Toolstr = Prefix + Strings.Left(toolcode, 3) + " " + Strings.Mid(toolcode, 4, 1) + " " + Strings.Mid(toolcode, 5, 2) + " " + Strings.Mid(toolcode, 7, 2) + " " + Strings.Right(toolcode, 3);
                    }
                    else
                    {
                        Chk_Toolstr = prefix + toolcode.Substring(0, 2) + " " + toolcode.Substring(2, 1) + " " + toolcode.Substring(3, 2) + " " + toolcode.Substring(5, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                        //Chk_Toolstr = Prefix + Strings.Left(toolcode, 2) + " " + Strings.Mid(toolcode, 3, 1) + " " + Strings.Mid(toolcode, 4, 2) + " " + Strings.Mid(toolcode, 6, 2) + " " + Strings.Right(toolcode, 4);
                    }
                    break;
                case 10:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 3) + " " + toolcode.Substring(3, 1) + " " + toolcode.Substring(4, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    break;
                case 9:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 2) + " " + toolcode.Substring(2, 1) + " " + toolcode.Substring(3, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    break;
                case 8:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 2) + " " + toolcode.Substring(2, 1) + " " + toolcode.Substring(3, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(3, toolcode.Length));
                    break;
                default:
                    Chk_Toolstr = "";
                    break;
            }
            return Chk_Toolstr;
        }


        public Image PreviewImage { get; set; }

        public DataGrid DgToolInfo { get; set; }

        public ICommand ShowDrawingscommand { get { return this.showDrawingscommand; } }
        private void ShowDrawings()
        {
            //ShowInformationMessage(PDMsg.DoesNotExists("Tool Family"));
            //  return;

            try
            {

                if (FamilyCd == "")
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Tool Family"));
                    return;
                }

                Progress.ProcessingText = PDMsg.Load;
                Progress.Start();

                Mouse.OverrideCursor = Cursors.Wait;
                obllTollInfo.GetFamilyDrawings(FamilyCd, toolAdm);
                //toolAdm.picture = toolAdm.picture;
                if (toolAdm.picture != null)
                {
                    if (SaveFile() == true)
                    {
                        System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                        MemoryStream strm = new MemoryStream();
                        //strm = new FileStream(ToolAdmin.DisplayFile_Name, FileMode.Open, FileAccess.Read);

                        using (FileStream file = new FileStream(toolAdm.DisplayFile_Name, FileMode.Open, FileAccess.Read))
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
                        PreviewImage.Source = bitmap;
                        //strm.Close();
                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        ShowInformationMessage("Drawing not Available!");
                        PreviewImage.Source = null;
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    ShowInformationMessage("Drawing not Available!");
                    PreviewImage.Source = null;
                }

                Progress.End();
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                throw ex.LogException();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private Boolean SaveFile()
        {
            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            try
            {
                if (toolAdm.FileType.ToUpper() == "VSD")
                {
                    if (toolAdm.MimeType.ToString().IndexOf("application") >= 0)
                    {
                        app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                        doc = app.Documents.Open(toolAdm.File_Name);
                        page = app.ActivePage;
                        page.Export(toolAdm.DisplayFile_Name);
                        app.Quit();

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    app.Quit();
                    return false;
                }
                catch (Exception ex1)
                {
                    return false;
                    throw ex1.LogException();
                }
                throw ex.LogException();
            }
        }

        //public ICommand MouseDoubleClickCommand
        //{
        //    get
        //    {
        //        if (mouseDoubleClickCommand == null)
        //        {
        //            mouseDoubleClickCommand = new RelayCommand<SearchItem>(
        //                item =>
        //                {
        //                    var selectedetitem = item;
        //                }
        //                );
        //        }
        //        return mouseDoubleClickCommand;
        //    }
        //}




        public ICommand DoubleClickCommand { get { return this.doubleClickCommand; } }
        private void GridDoubleClickCommand(Object selecteditem)
        {

            this.SelectedToolRow = (DataRowView)selecteditem;
            string sFilter = "";
            DataView dvTable1 = GridDvDisplayData.ToTable().Copy().DefaultView;


            foreach (string scode in addedCode)
            {
                sFilter = sFilter + " '" + scode + "',";
            }

            if (sFilter.Length > 0)
            {
                sFilter = sFilter.Substring(0, sFilter.Length - 1);
                dvTable1.RowFilter = "TOOL_CD IN (" + sFilter + ")";
                obllTollInfo.Save(dvTable1.ToTable().Copy(), FamilyCd);
                addedCode.Clear();
            }
            else
            {
                dvTable1.RowFilter = "TOOL_CD = '1@!^&*()_+'";
            }




            DataView dvTable = GridDvDisplayData.ToTable().Copy().DefaultView;

            sFilter = " TOOL_CD = '" + SelectedToolRow["TOOL_CD"].ToValueAsString() + "'";
            dvTable.RowFilter = sFilter;

            if (SelectedToolRow == null || !SelectedToolRow["TOOL_CD"].IsNotNullOrEmpty()) return;
            frmToolsQuickView toolQuickView = new frmToolsQuickView(userinformation, SelectedToolRow["TOOL_CD"].ToString(), dvTable);
            toolQuickView.ShowDialog();
            SelectDataRow();

        }

        public ICommand AddClickCommand { get { return this.addClickCommand; } }
        private void AddSubmitCommand()
        {

            string toolcdString = string.Empty;
            try
            {
                if (FamilyCd == "")
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Tool Family"));
                    return;
                }

                frmInputBox inp = new frmInputBox();
                inp.Title = ApplicationTitle + " - New Tool Code";
                inp.ShowDialog();
                toolcdString = inp.Txt_InputBox.Text;
                if (inp.BlnOk == true)
                {
                    if (toolcdString == "")
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Tool Code"));
                        return;
                    }
                }
                if (toolcdString != "")
                {

                    if (Chk_Tool(toolcdString) == "")
                    {
                        ShowInformationMessage(PDMsg.Invalid("Tool Code has been entered"));
                        return;
                    }

                    if (obllTollInfo.CheckToolCodeEsxists(toolcdString) == true)
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Tool Code"));
                        return;
                    }
                    DataRowView drv = GridDvDisplayData.AddNew();
                    drv.BeginEdit();
                    drv["FAMILY_CD"] = FamilyCd;
                    drv["TOOL_CD"] = toolcdString;
                    drv.EndEdit();
                    addedCode.Add(toolcdString);
                    SelectedToolRow = drv;
                    DgToolInfo.ScrollIntoView(SelectedToolRow);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowQuestionMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }


        private StringBuilder _searchBuildQuery;
        public StringBuilder SearchBuildQuery
        {
            get
            {
                return _searchBuildQuery;
            }
            set
            {
                _searchBuildQuery = value;
                NotifyPropertyChanged("SearchBuildQuery");
            }
        }


        public ICommand RefreshClickCommand { get { return this.refreshClickCommand; } }

        private void SearchCriteria()
        {
            try
            {
                SearchCriteriaFinal("");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void SearchCriteriaFinal(string orderByClause = "", string ascOrDesc = "")
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                PartNumberIsFocused = true;
                string query = "";
                SearchBuildQuery = new StringBuilder();
                this.DtParaMeterData = obllTollInfo.GetParameterCode(FamilyCd);
                DtParaMeterData.ToTable().AcceptChanges();
                if (DtParaMeterData != null)
                {
                    DataTable dtData;
                    dtData = DtParaMeterData.ToTable().Copy();
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam1, SearchParam1, SearchText1, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam2, SearchParam2, SearchText2, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam3, SearchParam3, SearchText3, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam4, SearchParam4, SearchText4, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam5, SearchParam5, SearchText5, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam6, SearchParam6, SearchText6, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam7, SearchParam7, SearchText7, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam8, SearchParam8, SearchText8, dtData));
                    SearchBuildQuery.Append(BuildWhereClause(CodeParam9, SearchParam9, SearchText9, dtData));
                    if (ToolCodeLike.ToValueAsString().Trim() != "")
                    {
                        SearchBuildQuery.Append("TOOL_CD LIKE '%" + ToolCodeLike + "%' And ");
                    }
                    if (SearchBuildQuery.Length > 0)
                    {
                        query = SearchBuildQuery.ToString().Substring(0, SearchBuildQuery.ToString().Length - 4);
                    }

                }
                fillGridData(query, orderByClause + " " + ascOrDesc);
                Mouse.OverrideCursor = null;
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

        private string BuildWhereClause(string codeParam, string searchParam, string searchText, DataTable dtData)
        {
            string query = "";
            //DataTable dtData = new DataTable();
            DataRow[] drRow;
            try
            {
                if (codeParam.ToValueAsString().Trim() != "" && searchParam.ToValueAsString().Trim() != "" && searchText.ToValueAsString().Trim() != "")
                {
                    //dtData = DtParaMeterData.ToTable().Copy();
                    drRow = dtData.Select("PARAMETER_CD = '" + codeParam + "'");
                    if (drRow.Length > 0)
                    {
                        if (drRow[0]["DATATYPE"].ToValueAsString().Trim() == "1")
                        {
                            query = "ISNUMERIC(" + codeParam + ")=1 AND (case when ISNUMERIC(" + codeParam + ")=1 then convert(decimal(18,5)," + codeParam + ") else -1 end) " + searchParam + " " + searchText + " And ";
                        }
                        else
                        {
                            if (searchParam.ToValueAsString().ToUpper().Trim() == "LIKE")
                            {
                                query = codeParam + " " + searchParam + " " + "'%" + searchText + "%'" + " And ";
                            }
                            else
                            {
                                query = codeParam + " " + searchParam + " " + "'" + searchText + "'" + " And ";
                            }
                        }
                    }
                }
                else
                {
                    query = "";
                }
                return query;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private string _codeParam1 = "";
        public string CodeParam1
        {
            get
            {
                return _codeParam1;
            }
            set
            {
                _codeParam1 = value;
                NotifyPropertyChanged("CodeParam1");
            }
        }


        private string _codeParam2 = "";
        public string CodeParam2
        {
            get
            {
                return _codeParam2;
            }
            set
            {
                _codeParam2 = value;
                NotifyPropertyChanged("CodeParam2");
            }
        }

        private string _codeParam3 = "";
        public string CodeParam3
        {
            get
            {
                return _codeParam3;
            }
            set
            {
                _codeParam3 = value;
                NotifyPropertyChanged("CodeParam3");
            }
        }

        private string _codeParam4 = "";
        public string CodeParam4
        {
            get
            {
                return _codeParam4;
            }
            set
            {
                _codeParam4 = value;
                NotifyPropertyChanged("CodeParam4");
            }
        }

        private string _codeParam5 = "";
        public string CodeParam5
        {
            get
            {
                return _codeParam5;
            }
            set
            {
                _codeParam5 = value;
                NotifyPropertyChanged("CodeParam5");
            }
        }

        private string _codeParam6 = "";
        public string CodeParam6
        {
            get
            {
                return _codeParam6;
            }
            set
            {
                _codeParam6 = value;
                NotifyPropertyChanged("CodeParam6");
            }
        }

        private string _codeParam7 = "";
        public string CodeParam7
        {
            get
            {
                return _codeParam7;
            }
            set
            {
                _codeParam7 = value;
                NotifyPropertyChanged("CodeParam7");
            }
        }

        private string _codeParam8 = "";
        public string CodeParam8
        {
            get
            {
                return _codeParam8;
            }
            set
            {
                _codeParam8 = value;
                NotifyPropertyChanged("CodeParam8");
            }
        }

        private string _codeParam9 = "";
        public string CodeParam9
        {
            get
            {
                return _codeParam9;
            }
            set
            {
                _codeParam9 = value;
                NotifyPropertyChanged("CodeParam9");
            }
        }

        private string _nameParam1 = "";
        public string NameParam1
        {
            get
            {
                return _nameParam1;
            }
            set
            {
                _nameParam1 = value;
                NotifyPropertyChanged("NameParam1");
            }
        }

        private string _nameParam2 = "";
        public string NameParam2
        {
            get
            {
                return _nameParam2;
            }
            set
            {
                _nameParam2 = value;
                NotifyPropertyChanged("NameParam2");
            }
        }

        private string _nameParam3 = "";
        public string NameParam3
        {
            get
            {
                return _nameParam3;
            }
            set
            {
                _nameParam3 = value;
                NotifyPropertyChanged("NameParam3");
            }
        }

        private string _nameParam4 = "";
        public string NameParam4
        {
            get
            {
                return _nameParam4;
            }
            set
            {
                _nameParam4 = value;
                NotifyPropertyChanged("NameParam4");
            }
        }

        private string _nameParam5 = "";
        public string NameParam5
        {
            get
            {
                return _nameParam5;
            }
            set
            {
                _nameParam5 = value;
                NotifyPropertyChanged("NameParam5");
            }
        }

        private string _nameParam6 = "";
        public string NameParam6
        {
            get
            {
                return _nameParam6;
            }
            set
            {
                _nameParam6 = value;
                NotifyPropertyChanged("NameParam6");
            }
        }

        private string _nameParam7 = "";
        public string NameParam7
        {
            get
            {
                return _nameParam7;
            }
            set
            {
                _nameParam7 = value;
                NotifyPropertyChanged("NameParam7");
            }
        }

        private string _nameParam8 = "";
        public string NameParam8
        {
            get
            {
                return _nameParam8;
            }
            set
            {
                _nameParam8 = value;
                NotifyPropertyChanged("NameParam8");
            }
        }

        private string _nameParam9 = "";
        public string NameParam9
        {
            get
            {
                return _nameParam9;
            }
            set
            {
                _nameParam9 = value;
                NotifyPropertyChanged("NameParam9");
            }
        }


        private string _searchParam1 = "";
        public string SearchParam1
        {
            get
            {
                return _searchParam1;
            }
            set
            {
                _searchParam1 = value;
                NotifyPropertyChanged("SearchParam1");
            }
        }

        private string _searchParam2 = "";
        public string SearchParam2
        {
            get
            {
                return _searchParam2;
            }
            set
            {
                _searchParam2 = value;
                NotifyPropertyChanged("SearchParam2");
            }
        }

        private string _searchParam3 = "";
        public string SearchParam3
        {
            get
            {
                return _searchParam3;
            }
            set
            {
                _searchParam3 = value;
                NotifyPropertyChanged("SearchParam3");
            }
        }

        private string _searchParam4 = "";
        public string SearchParam4
        {
            get
            {
                return _searchParam4;
            }
            set
            {
                _searchParam4 = value;
                NotifyPropertyChanged("SearchParam4");
            }
        }

        private string _searchParam5 = "";
        public string SearchParam5
        {
            get
            {
                return _searchParam5;
            }
            set
            {
                _searchParam5 = value;
                NotifyPropertyChanged("SearchParam5");
            }
        }

        private string _searchParam6 = "";
        public string SearchParam6
        {
            get
            {
                return _searchParam6;
            }
            set
            {
                _searchParam6 = value;
                NotifyPropertyChanged("SearchParam6");
            }
        }

        private string _searchParam7 = "";
        public string SearchParam7
        {
            get
            {
                return _searchParam7;
            }
            set
            {
                _searchParam7 = value;
                NotifyPropertyChanged("SearchParam7");
            }
        }

        private string _searchParam8 = "";
        public string SearchParam8
        {
            get
            {
                return _searchParam8;
            }
            set
            {
                _searchParam8 = value;
                NotifyPropertyChanged("SearchParam8");
            }
        }

        private string _searchParam9 = "";
        public string SearchParam9
        {
            get
            {
                return _searchParam9;
            }
            set
            {
                _searchParam9 = value;
                NotifyPropertyChanged("SearchParam9");
            }
        }

        private string _searchText1 = "";
        public string SearchText1
        {
            get
            {
                return _searchText1;
            }
            set
            {
                _searchText1 = value;
                NotifyPropertyChanged("SearchText1");
            }
        }

        private string _searchText2 = "";
        public string SearchText2
        {
            get
            {
                return _searchText2;
            }
            set
            {
                _searchText2 = value;
                NotifyPropertyChanged("SearchText2");
            }
        }

        private string _searchText3 = "";
        public string SearchText3
        {
            get
            {
                return _searchText3;
            }
            set
            {
                _searchText3 = value;
                NotifyPropertyChanged("SearchText3");
            }
        }

        private string _searchText4 = "";
        public string SearchText4
        {
            get
            {
                return _searchText4;
            }
            set
            {
                _searchText4 = value;
                NotifyPropertyChanged("SearchText4");
            }
        }

        private string _searchText5 = "";
        public string SearchText5
        {
            get
            {
                return _searchText5;
            }
            set
            {
                _searchText5 = value;
                NotifyPropertyChanged("SearchText5");
            }
        }

        private string _searchText6 = "";
        public string SearchText6
        {
            get
            {
                return _searchText6;
            }
            set
            {
                _searchText6 = value;
                NotifyPropertyChanged("SearchText6");
            }
        }

        private string _searchText7 = "";
        public string SearchText7
        {
            get
            {
                return _searchText7;
            }
            set
            {
                _searchText7 = value;
                NotifyPropertyChanged("SearchText7");
            }
        }

        private string _searchText8 = "";
        public string SearchText8
        {
            get
            {
                return _searchText8;
            }
            set
            {
                _searchText8 = value;
                NotifyPropertyChanged("SearchText8");
            }
        }

        private string _searchText9 = "";
        public string SearchText9
        {
            get
            {
                return _searchText9;
            }
            set
            {
                _searchText9 = value;
                NotifyPropertyChanged("SearchText9");
            }
        }

        public DataTable _dtSymbols
        {
            get;
            private set;
        }


        public DataView DtSymbols
        {
            get
            {
                return this._dtSymbols.DefaultView;
            }
            set
            {
                this._dtSymbols = value.ToTable();
                NotifyPropertyChanged("DtSymbols");
            }
        }


        public DataTable SearchData
        {
            get;
            private set;
        }

        public DataView DtSearchCriteria
        {
            get
            {
                return this.SearchData.IsNotNullOrEmpty() ? this.SearchData.DefaultView : null;
            }
            set
            {
                this.SearchData = value.ToTable();
                NotifyPropertyChanged("DtSearchCriteria");
            }
        }




        public DataView DtSearchCriteria1
        {
            get
            {
                return this.SearchData.IsNotNullOrEmpty() ? this.SearchData.DefaultView : null;
            }
            set
            {
                this.SearchData = value.ToTable();
                NotifyPropertyChanged("DtSearchCriteria1");
            }
        }

        private ObservableCollection<DropdownColumns> _columns;
        public ObservableCollection<DropdownColumns> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
                NotifyPropertyChanged("Columns");
            }
        }

        public DataTable GridData
        {
            get;
            private set;
        }

        public DataView GridDtDataview
        {
            get
            {
                //return this.GridData.DefaultView;
                return this.GridData.IsNotNullOrEmpty() ? this.GridData.DefaultView : null;
            }
            set
            {
                this.GridData = value.ToTable();
                NotifyPropertyChanged("GridDtDataview");
            }
        }

        private DataView _gridDvDisplayData;
        public DataView GridDvDisplayData
        {
            get
            {
                return _gridDvDisplayData;
            }
            set
            {
                _gridDvDisplayData = value;
                NotifyPropertyChanged("GridDvDisplayData");
            }
        }

        private ObservableCollection<string> _hEADER;
        public ObservableCollection<string> HEADER
        {
            get { return _hEADER; }
            set
            {
                _hEADER = value;
                NotifyPropertyChanged("HEADER");
            }
        }

        private ObservableCollection<System.Windows.Visibility> _vISIBLE;
        public ObservableCollection<System.Windows.Visibility> VISIBLE
        {
            get { return _vISIBLE; }
            set
            {
                _vISIBLE = value;
                NotifyPropertyChanged("VISIBLE");
            }
        }


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

        public DataTable FamilyMaster
        {
            get;
            private set;
        }
        public DataView DtDataview
        {
            get
            {
                //return this.FamilyMaster.DefaultView;
                return this.FamilyMaster.IsNotNullOrEmpty() ? this.FamilyMaster.DefaultView : null;
            }
            set
            {
                this.FamilyMaster = value.ToTable();
                NotifyPropertyChanged("DtDataview");
            }
        }

        private bool _partNumberIsFocused = false;
        public bool PartNumberIsFocused
        {
            get { return _partNumberIsFocused; }
            set
            {
                _partNumberIsFocused = value;
                NotifyPropertyChanged("PartNumberIsFocused");
            }
        }




        private string _familyCd = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = " ")]
        public string FamilyCd
        {
            get
            {
                return this._familyCd;
            }
            set
            {
                this._familyCd = value;
                NotifyPropertyChanged("FamilyCd");
            }
        }
        private string _familyName = "";
        public string FamilyName
        {
            get
            {
                return this._familyName;
            }
            set
            {
                this._familyName = value;
                NotifyPropertyChanged("FamilyName");
            }
        }
        private DataRowView _selectedrow;
        public DataRowView SelectedRow
        {
            get
            {
                return _selectedrow;
            }

            set
            {
                _selectedrow = value;
            }
        }

        private DataRowView _selectedToolRow;
        public DataRowView SelectedToolRow
        {
            get
            {
                return _selectedToolRow;
            }
            set
            {
                _selectedToolRow = value;
                NotifyPropertyChanged("SelectedToolRow");
            }
        }

        private string _toolCodeLike;
        public string ToolCodeLike
        {
            get
            {
                return _toolCodeLike;
            }
            set
            {
                _toolCodeLike = value;
                NotifyPropertyChanged("ToolCodeLike");
            }
        }

        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            if (SelectedRow != null)
            {
                FamilyCd = SelectedRow["FAMILY_CD"].ToString();
                FamilyName = SelectedRow["FAMILY_NAME"].ToString();
                //fillGridData("");
                PreviewImage.Source = null;
                addedCode.Clear();
                ClearAll();
                SearchCriteria();
                LoadSearchCriteria();
            }
        }

        public void LoadSearchCriteria()
        {
            try
            {
                DataTable dtdearch1 = new DataTable();
                this.DtSearchCriteria = DtParaMeterData.ToTable().DefaultView;
                dtdearch1 = DtParaMeterData.ToTable().Copy();
                dtdearch1.Columns["PARAMETER_CD"].ColumnName = "PARAMETER_CD1";
                dtdearch1.Columns["PARAMETER_NAME"].ColumnName = "PARAMETER_NAME1";
                dtdearch1.AcceptChanges();
                this.DtSearchCriteria1 = dtdearch1.DefaultView;
                SetdropDownItems();
            }
            catch (Exception ex)
            {

            }
        }



        //public DataTable SearchDt
        //{
        //    get;
        //    private set;
        //}
        private DataTable _searchDatatable;
        public DataTable SearchDatatable
        {
            get
            {
                return this._searchDatatable;
            }
            set
            {
                this._searchDatatable = value;
                NotifyPropertyChanged("SearchDatatable");
            }
        }


        public void fillGridData(string whereClause, string orderByClause = "")
        {
            try
            {
                if (FamilyCd != "")
                {

                    //this.DtParaMeterData = obllTollInfo.GetParameterCode(FamilyCd);
                    //DtParaMeterData.ToTable().AcceptChanges();
                    //this.GridDtDataview = obllTollInfo.GetFamilyGridData(DtParaMeterData.ToTable(), FamilyCd);
                    orderByClause = orderByClause.Trim();
                    this.GridDvDisplayData = obllTollInfo.GetFamilyData(FamilyCd, whereClause, orderByClause);
                    SetGrid();
                    GridTotalCount = "Total Information " + GridDvDisplayData.Count.ToValueAsString().Trim() + " Entries Found";
                    if (GridDvDisplayData.Count > 0)
                    {
                        SelectedToolRow = GridDvDisplayData[0];
                    }
                    //HeaderNameText = "Sample";
                    //ChangeColumCaption(DtParaMeterData, GridDtDataview);
                }
            }
            catch (Exception ex)
            {
                //throw ex.ToString();
            }
        }



        public void ChangeColumCaption(DataView dtparameterdata, DataView griddtdataview)
        {
            try
            {

                DataTable griddtdisplaydata1 = new DataTable();
                griddtdisplaydata1 = griddtdataview.ToTable().Copy();
                SearchDatatable = griddtdataview.ToTable().Copy();
                griddtdisplaydata1.Columns["TOOL_CD"].ColumnName = "Tool Code";
                griddtdisplaydata1.AcceptChanges();

                foreach (DataRow row in dtparameterdata.ToTable().Rows)
                {
                    if (griddtdisplaydata1.Columns.Contains(row[1].ToString()))
                    {
                        string colname = "";
                        colname = row[1].ToString();
                        griddtdisplaydata1.Columns[colname].ColumnName = row[2].ToString();
                        griddtdisplaydata1.AcceptChanges();
                    }
                }


                for (int col = griddtdisplaydata1.Columns.Count - 1; col >= 0; col--)
                {
                    bool removeColumn = true;
                    foreach (DataRow row in griddtdisplaydata1.Rows)
                    {
                        if (!row.IsNull(col))
                        {
                            removeColumn = false;
                            break;
                        }
                    }
                    if (removeColumn)
                    {
                        griddtdisplaydata1.Columns.RemoveAt(col);
                        SearchDatatable.Columns.RemoveAt(col);
                    }
                }
            }
            catch
            {

            }
        }


        private string _gridTotalCount;
        public string GridTotalCount
        {
            get
            {
                return _gridTotalCount;
            }
            set
            {
                _gridTotalCount = value;
                NotifyPropertyChanged("GridTotalCount");
            }
        }


        public static bool AreAllCellsEmpty(DataRow row)
        {
            if (row == null) throw new ArgumentNullException("row");

            for (int i = row.Table.Columns.Count - 1; i >= 0; i--)
                if (!row.IsNull(i))
                    return false;

            return true;
        }

        public void rpdDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.DisplayIndex == 0)
            {
                e.Cancel = true;
            }
        }



        public ICommand SaveCommand { get { return this.saveCommand; } }
        private void Save()
        {
            try
            {
                if (FamilyCd == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Tool Family"));
                    return;
                }
                Mouse.OverrideCursor = Cursors.Wait;
                PartNumberIsFocused = true;
                obllTollInfo.Save(GridDvDisplayData.ToTable().Copy(), FamilyCd);
                ShowInformationMessage("Record Saved Successfully");
                ClearAll();
                FamilyCd = "";
                SelectedRow = null;
                GridDvDisplayData.Table.Clear();
                DgToolInfo.Columns.Clear();
                GridTotalCount = "Total Information " + 0 + " Entry Found";
                Mouse.OverrideCursor = null;
                addedCode.Clear();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                PreviewImage.Source = null;
                Mouse.OverrideCursor = null;
            }
        }

        public ICommand CopyToolCodeCommand { get { return this.copyToolCodeCommand; } }
        private void CopyToolCode()
        {
            string toolcdString = string.Empty;
            try
            {
                if (FamilyCd == "")
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Tool Family"));
                    return;
                }
                if (GridDvDisplayData.Table.Rows.Count == 0)
                {
                    ShowInformationMessage("No Data to Copy!");
                    return;
                }
                if (SelectedToolRow == null)
                {
                    ShowInformationMessage("Please select the required tool code to copy!");
                    return;
                }
                frmInputBox inp = new frmInputBox();
                inp.Title = ApplicationTitle + " - Copy Tool Code";
                inp.ShowDialog();
                toolcdString = inp.Txt_InputBox.Text;
                if (inp.BlnOk == true)
                {
                    if (toolcdString == "")
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Tool Code"));
                        return;
                    }
                }
                if (toolcdString != "")
                {
                    if (Chk_Tool(toolcdString) == "")
                    {
                        ShowInformationMessage(PDMsg.Invalid("Tool Code has been entered"));
                        return;
                    }
                    if (obllTollInfo.CheckToolCodeEsxists(toolcdString) == true)
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Tool Code"));
                        return;
                    }
                    DataRowView drv = GridDvDisplayData.AddNew();
                    drv.BeginEdit();
                    for (int ictr = 0; ictr < GridDvDisplayData.Count - 1; ictr++)
                    {
                        drv[ictr] = SelectedToolRow[ictr];
                    }
                    drv["FAMILY_CD"] = FamilyCd;
                    drv["TOOL_CD"] = toolcdString;
                    drv.EndEdit();
                    addedCode.Add(toolcdString);
                    SelectedToolRow = drv;
                    DgToolInfo.ScrollIntoView(SelectedToolRow);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public ICommand DeleteToolCodeCommand { get { return this.deleteToolCodeCommand; } }
        private void DeleteToolCode()
        {
            string toolcdString = string.Empty;
            int iSelectedIndex = -1;
            try
            {
                if (FamilyCd == "")
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Tool Family"));
                    return;
                }
                if (SelectedToolRow == null)
                {
                    ShowInformationMessage("Please select the required Tool Information!");
                    return;
                }
                if (ShowQuestionMessage("Do you want to Delete Tool Information of " + SelectedToolRow["TOOL_CD"].ToValueAsString().Trim() + " ?") == MessageBoxResult.OK)
                {
                    if (obllTollInfo.DeleteToolInfo(FamilyCd, SelectedToolRow["TOOL_CD"].ToValueAsString().Trim()) == true)
                    {
                        if (addedCode.IndexOf(SelectedToolRow["TOOL_CD"].ToValueAsString().Trim()) >= 0)
                        {
                            addedCode.Remove(SelectedToolRow["TOOL_CD"].ToValueAsString().Trim());
                        }
                        iSelectedIndex = DgToolInfo.SelectedIndex;
                        GridDvDisplayData.Delete(DgToolInfo.SelectedIndex);
                        iSelectedIndex = iSelectedIndex - 1;
                        if (iSelectedIndex > -1)
                        {
                            DgToolInfo.SelectedIndex = iSelectedIndex;
                        }
                        else
                        {
                            if (GridDvDisplayData.Count > 0)
                            {
                                DgToolInfo.SelectedIndex = 0;
                            }
                        }

                        ShowInformationMessage(PDMsg.DeletedSuccessfully);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public ICommand CloseCommand { get { return this.closeCommand; } }
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

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {

                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                {
                    closingev.Cancel = true;
                    e = closingev;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void SetGrid()
        {
            int ictr = 0;
            DataTable dtParameter;
            DataRow[] drRow;
            try
            {
                dtParameter = DtParaMeterData.ToTable().Copy();
                foreach (System.Windows.Controls.DataGridTextColumn dgColumn in DgToolInfo.Columns)
                {
                    if (dgColumn.SortMemberPath.ToValueAsString().ToUpper() != "TOOL_CD")
                    {
                        drRow = dtParameter.Select("PARAMETER_CD = '" + dgColumn.SortMemberPath + "'");
                        if (drRow.Length == 0)
                        {
                            dgColumn.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            dgColumn.Header = drRow[0]["PARAMETER_NAME"].ToValueAsString().Trim();
                            dgColumn.Visibility = Visibility.Visible;
                            //dgColumn.Width = 150;
                        }
                    }
                    else
                    {
                        dgColumn.Header = "Tool Code";
                        dgColumn.Width = 100;
                    }
                }
                DgToolInfo.FrozenColumnCount = 3;
                DgToolInfo.Columns[0].IsReadOnly = true;
                DgToolInfo.Columns[1].IsReadOnly = true;
                DgToolInfo.Columns[2].IsReadOnly = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ClearAll()
        {
            SearchParam1 = "";
            SearchParam2 = "";
            SearchParam3 = "";
            SearchParam4 = "";
            SearchParam5 = "";
            SearchParam6 = "";
            SearchParam7 = "";
            SearchParam8 = "";
            SearchParam9 = "";
            SearchText1 = "";
            SearchText2 = "";
            SearchText3 = "";
            SearchText4 = "";
            SearchText5 = "";
            SearchText6 = "";
            SearchText7 = "";
            SearchText8 = "";
            SearchText9 = "";
            CodeParam1 = "";
            CodeParam2 = "";
            CodeParam3 = "";
            CodeParam4 = "";
            CodeParam5 = "";
            CodeParam6 = "";
            CodeParam7 = "";
            CodeParam8 = "";
            CodeParam9 = "";
            NameParam1 = "";
            NameParam2 = "";
            NameParam3 = "";
            NameParam4 = "";
            NameParam5 = "";
            NameParam6 = "";
            NameParam7 = "";
            NameParam8 = "";
            NameParam9 = "";
            ToolCodeLike = "";
            addedCode.Clear();
        }


        public void rpdDataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
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

        public void txtVal1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam1);
        }

        public void txtVal2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam2);
        }

        public void txtVal3_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam3);
        }

        public void txtVal4_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam4);
        }

        public void txtVal5_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam5);
        }

        public void txtVal6_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam6);
        }

        public void txtVal7_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam7);
        }

        public void txtVal8_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam8);
        }

        public void txtVal9_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CheckDataTypeAndValidateForTextBox(sender, e, CodeParam9);
        }

        public void txtVal1_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText1 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam1);
        }

        public void txtVal2_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText2 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam2);
        }

        public void txtVal3_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText3 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam3);
        }

        public void txtVal4_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText4 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam4);
        }

        public void txtVal5_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText5 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam5);
        }

        public void txtVal6_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText6 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam6);
        }

        public void txtVal7_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText7 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam7);
        }

        public void txtVal8_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText8 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam8);
        }

        public void txtVal9_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchText9 = checkLostFocus(((TextBox)sender).Text.Trim(), CodeParam9);
        }


        public void rpdDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
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

        public void cmbCategory_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText1 = checkLostFocus(SearchText1, CodeParam1);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText2 = checkLostFocus(SearchText2, CodeParam2);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory2_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText3 = checkLostFocus(SearchText3, CodeParam3);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory3_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText4 = checkLostFocus(SearchText4, CodeParam4);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory4_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText5 = checkLostFocus(SearchText5, CodeParam5);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory5_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText6 = checkLostFocus(SearchText6, CodeParam6);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory6_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText7 = checkLostFocus(SearchText7, CodeParam7);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory7_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText8 = checkLostFocus(SearchText8, CodeParam8);
            }
            catch (Exception ex)
            {

            }
        }

        public void cmbCategory8_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SearchText9 = checkLostFocus(SearchText9, CodeParam9);
            }
            catch (Exception ex)
            {

            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter
        {
            get
            {
                return _dropDownItemsParameter;
            }
            set
            {
                this._dropDownItemsParameter = value;
                NotifyPropertyChanged("DropDownItemsParameter");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter1;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter1
        {
            get
            {
                return _dropDownItemsParameter1;
            }
            set
            {
                this._dropDownItemsParameter1 = value;
                NotifyPropertyChanged("DropDownItemsParameter1");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter2;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter2
        {
            get
            {
                return _dropDownItemsParameter2;
            }
            set
            {
                this._dropDownItemsParameter2 = value;
                NotifyPropertyChanged("DropDownItemsParameter2");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter3;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter3
        {
            get
            {
                return _dropDownItemsParameter3;
            }
            set
            {
                this._dropDownItemsParameter3 = value;
                NotifyPropertyChanged("DropDownItemsParameter3");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter4;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter4
        {
            get
            {
                return _dropDownItemsParameter4;
            }
            set
            {
                this._dropDownItemsParameter4 = value;
                NotifyPropertyChanged("DropDownItemsParameter4");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter5;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter5
        {
            get
            {
                return _dropDownItemsParameter5;
            }
            set
            {
                this._dropDownItemsParameter5 = value;
                NotifyPropertyChanged("DropDownItemsParameter5");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter6;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter6
        {
            get
            {
                return _dropDownItemsParameter6;
            }
            set
            {
                this._dropDownItemsParameter6 = value;
                NotifyPropertyChanged("DropDownItemsParameter6");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter7;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter7
        {
            get
            {
                return _dropDownItemsParameter7;
            }
            set
            {
                this._dropDownItemsParameter7 = value;
                NotifyPropertyChanged("DropDownItemsParameter7");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsParameter8;
        public ObservableCollection<DropdownColumns> DropDownItemsParameter8
        {
            get
            {
                return _dropDownItemsParameter8;
            }
            set
            {
                this._dropDownItemsParameter8 = value;
                NotifyPropertyChanged("DropDownItemsParameter8");
            }
        }







        private ObservableCollection<DropdownColumns> _dropDownItemsSymbol;
        public ObservableCollection<DropdownColumns> DropDownItemsSymbol
        {
            get
            {
                return _dropDownItemsSymbol;
            }
            set
            {
                this._dropDownItemsSymbol = value;
                NotifyPropertyChanged("DropDownItemsSymbol");
            }
        }


        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsParameter = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter1 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter2 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter3 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter4 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter5 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter6 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter7 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };
                DropDownItemsParameter8 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PARAMETER_CD1", ColumnDesc = "Parameter CD", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PARAMETER_NAME1", ColumnDesc = "Parameter Name", ColumnWidth = "1*" }
                        };









                DropDownItemsSymbol = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "Search", ColumnDesc = "Search", ColumnWidth = "1*" }
                        };


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void dgrdProcessIssue_Sorting(object sender, DataGridSortingEventArgs e)
        {
            int ictr = 0;
            int a = 0;
            a = 0;
            try
            {
                e.Handled = true;
                if (sortColumn.ContainsKey(e.Column.SortMemberPath) == false)
                {
                    sortColumn.Add(e.Column.SortMemberPath, "");
                }
                string sortType = sortColumn[e.Column.SortMemberPath];
                if (sortType == "")
                    sortType = "ASC";
                else if (sortType == "ASC")
                    sortType = "DESC";
                else
                    sortType = "ASC";
                sortColumn[e.Column.SortMemberPath] = sortType;

                SearchCriteriaFinal(e.Column.SortMemberPath, sortType);
                if (GridDvDisplayData.Table.Rows.Count > 0)
                {

                    SelectedToolRow = GridDvDisplayData[0];
                    var row1 = DgToolInfo.Items[0];
                    foreach (DataGridColumn col1 in DgToolInfo.Columns)
                    {
                        if (col1.SortMemberPath == e.Column.SortMemberPath)
                        {
                            DataGridColumn colScroll = DgToolInfo.Columns[ictr];
                            DgToolInfo.ScrollIntoView(row1, colScroll);
                            //DgToolInfo.BringIntoView(colScroll);
                            break;
                        }
                        ictr = ictr + 1;
                    }
                    //    DgToolInfo.Scroll
                    //SelectedToolRow =
                    //DgToolInfo.ScrollIntoView(SelectedToolRow);
                }




            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        //Jeyan
        private DataView GetToolFamilyCode()
        {
            DataView dv = null;
            try
            {
                DataTable dt = new DataTable();
               
                System.Resources.ResourceManager myManager;
                myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                string conStr = myManager.GetString("ConnectionString");

                DataAccessLayer dal = new DataAccessLayer(conStr);
                dt = dal.GetToolFamilyCode();
                if (dt != null)
                {
                    dv = dt.DefaultView;
                }
                else
                {
                    dv = null;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return dv;
        }
    }
}

