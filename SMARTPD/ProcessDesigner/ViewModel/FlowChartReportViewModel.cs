using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Drawing;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class FlowChartReportViewModel : ViewModelBase
    {

        private PCCSBll _pccsBll;
        private UserInformation _userInformation;
        public FlowChartReportViewModel(UserInformation userInfo)
        {
            _userInformation = userInfo;
            this._pccsBll = new PCCSBll(userInfo);
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

        private int GetImageBytesFromOLEField(byte[] oleFieldBytes, ref string fileType)
        {
            const string BITMAP_ID_BLOCK = "BM";
            const string JPG_ID_BLOCK = "\u00FF\u00D8\u00FF";
            const string PNG_ID_BLOCK = "\u0089PNG\r\n\u001a\n";
            const string GIF_ID_BLOCK = "GIF8";
            const string TIFF_ID_BLOCK = "II*\u0000";
            const string VSD_ID_BLOCK = "ÐÏà¡±á";

            //byte[] imageBytes;

            // Get a UTF7 Encoded string version
            Encoding u8 = Encoding.UTF7;
            string strTemp = u8.GetString(oleFieldBytes);

            // Get the first 300 characters from the string
            string strVTemp;

            strVTemp = strTemp.Substring(0, (strTemp.Length >= 300 ? 300 : strTemp.Length));

            // Search for the block
            int iPos = -1;
            if (strVTemp.IndexOf(BITMAP_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK);
                fileType = "bmp";
            }
            else if (strVTemp.IndexOf(JPG_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(JPG_ID_BLOCK);
                fileType = "bmp";
            }
            else if (strVTemp.IndexOf(PNG_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(PNG_ID_BLOCK);
                fileType = "png";
            }
            else if (strVTemp.IndexOf(GIF_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(GIF_ID_BLOCK);
                fileType = "gif";
            }
            else if (strVTemp.IndexOf(TIFF_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(TIFF_ID_BLOCK);
                fileType = "tiff";
            }
            else if (strVTemp.IndexOf(VSD_ID_BLOCK) != -1)
            {
                fileType = "vsd";
                iPos = strVTemp.IndexOf(VSD_ID_BLOCK);
            }

            if (iPos == -1)
            {
                iPos = 0;
            }
            return iPos;
        }

        public string getMimeFromFile(byte[] byteArray)
        {

            byte[] buffer = new byte[256];
            using (System.IO.MemoryStream fs = new System.IO.MemoryStream(byteArray))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception e)
            {
                e.LogException();
                return e.Message;
            }
        }

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd);

        private Boolean SaveFile(string fileType, string mimeType, string file_Name, string displayFile_Name)
        {
            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            try
            {
                if (fileType.ToUpper() == "VSD")
                {
                    if (mimeType.ToString().IndexOf("application") >= 0)
                    {
                        app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                        doc = app.Documents.Open(file_Name);
                        page = app.ActivePage;
                        page.Export(displayFile_Name);
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

        public string GetFilePath()
        {
            string filePathNew = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (System.Reflection.Assembly.GetExecutingAssembly().IsDebug() || filePathNew.Contains("\\bin\\Debug"))
            {
                System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(filePathNew);
                filePathNew = d.Parent.Parent.FullName;
            }

            string filePath = filePathNew + "\\VSDFiles\\PCCS";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
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
            return filePath;

        }

        public void ShowFlowChart(string partNo, decimal routeNo)
        {
            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            Dictionary<string, string> dictFormulas = new Dictionary<string, string>();
            if (!_pccsBll.IsNotNullOrEmpty() || !_pccsBll.DB.IsNotNullOrEmpty()) return;

            if (!partNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }

            List<Model.PROC_FLOW_DGM> lstPROC_FLOW_DGM = (from row in _pccsBll.DB.PROC_FLOW_DGM
                                                          where row.PART_NO == partNo
                                                          && row.ROUTE_NO == routeNo
                                                          select row).ToList<Model.PROC_FLOW_DGM>();

            Model.PROC_FLOW_DGM proc_flow_dgm = null;

            System.IO.MemoryStream strm;
            byte[] bytes = null;
            bytes = new byte[10];

            if (lstPROC_FLOW_DGM.IsNotNullOrEmpty() && lstPROC_FLOW_DGM.Count > 0)
            {
                ////proc_flow_dgm = lstPROC_FLOW_DGM[0];
                ////byte[] photosource = proc_flow_dgm.DIAGRAM.ToArray();
                ////string fileType = null;
                ////int offset = GetImageBytesFromOLEField(photosource, ref fileType);

                ////strm = new System.IO.MemoryStream();
                ////strm.Write(photosource, offset, photosource.Length - offset);
                ////string mimeType = getMimeFromFile(photosource);

                ////string displayFile_Name = null;
                ////if (fileType == null && mimeType.Contains("wmf")) fileType = "wmf";
                ////if (fileType == null) fileType = "vsd";
                ////string file_Name = GetFilePath() + "\\TMP_FLOW_CHART_REPORT.VSD";

                ////if (fileType == "vsd")
                ////{
                ////    displayFile_Name = GetFilePath() + "\\File_FLOW_CHART_REPORT.bmp";
                ////}
                ////else
                ////{
                ////    displayFile_Name = GetFilePath() + "\\File_FLOW_CHART_REPORT." + fileType;
                ////}

                ////System.IO.FileStream fileStream = System.IO.File.Create((fileType != "vsd" ? displayFile_Name : file_Name));
                ////strm.Seek(0, System.IO.SeekOrigin.Begin);
                ////strm.CopyTo(fileStream);
                ////fileStream.Close();
                ////fileStream.Dispose();
                ////strm.Close();
                ////strm.Dispose();


                ////if (SaveFile(fileType, mimeType, file_Name, displayFile_Name) == true)
                ////{
                ////    strm = new System.IO.MemoryStream();
                ////    using (System.IO.FileStream file = new System.IO.FileStream(displayFile_Name, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                ////    {
                ////        bytes = new byte[file.Length];
                ////        file.Read(bytes, 0, (int)file.Length);
                ////        strm.Write(bytes, 0, (int)file.Length);
                ////        file.Close();
                ////        file.Dispose();
                ////    }
                ////}

            }

            DataTable dtPART_MASTER = null;

            DataTable dtPROC_FLOW_DGM = (from row in _pccsBll.DB.PROC_FLOW_DGM
                                         where row.PART_NO == partNo && row.ROUTE_NO == routeNo
                                         select row).ToList<Model.PROC_FLOW_DGM>().ToDataTable<Model.PROC_FLOW_DGM>();
            if (!dtPROC_FLOW_DGM.IsNotNullOrEmpty() || dtPROC_FLOW_DGM.Rows.Count == 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            dtPROC_FLOW_DGM.TableName = "PROC_FLOW_DGM";
            if (dtPROC_FLOW_DGM.Columns.Contains("DIAGRAM"))
            {
                dtPROC_FLOW_DGM.Columns.Remove("DIAGRAM");
                dtPROC_FLOW_DGM.Columns.Add("DIAGRAM", bytes.GetType());
            }

            if (dtPROC_FLOW_DGM.Columns.Contains("ROUTE_NO"))
            {
                dtPROC_FLOW_DGM.Columns.Remove("ROUTE_NO");
                dtPROC_FLOW_DGM.Columns.Add("ROUTE_NO", Type.GetType("System.Decimal"));
            }

            if (dtPROC_FLOW_DGM.Columns.Contains("PROCESS_MAIN"))
                dtPROC_FLOW_DGM.Columns.Remove("PROCESS_MAIN");

            if (dtPROC_FLOW_DGM.Rows.Count > 0)
            {
                GenerateProcessFlowDiagram(partNo, routeNo, ref dtPROC_FLOW_DGM);
                ////dtPROC_FLOW_DGM.Rows[0]["DIAGRAM"] = bytes;
                //foreach (DataRow row in dtPROC_FLOW_DGM.Rows)
                //{
                //    row["ROUTE_NO"] = "Process  No. : " + Convert.ToString(row["ROUTE_NO"]).Trim();
                //    row.EndEdit();
                //    dtPROC_FLOW_DGM.AcceptChanges();
                //}

                dtPART_MASTER = dtPROC_FLOW_DGM.Clone();
                dtPART_MASTER.ImportRow(dtPROC_FLOW_DGM.Rows[0]);

                dtPART_MASTER.TableName = "PART_MASTER";
                if (dtPART_MASTER.Columns.Contains("DIAGRAM"))
                {
                    dtPART_MASTER.Columns.Remove("DIAGRAM");
                }
            }

            List<PRD_MAST> lstPRD_MAST = (from pm in _pccsBll.DB.PRD_MAST
                                          where pm.PART_NO == partNo
                                          select pm).ToList<PRD_MAST>();


            DataTable dtPRD_MAST = new DataTable();
            if (lstPRD_MAST.IsNotNullOrEmpty())
            {
                dtPRD_MAST = lstPRD_MAST.ToDataTable<PRD_MAST>();
                if (dtPRD_MAST.Columns.Contains("PRD_DWG_ISSUE")) dtPRD_MAST.Columns.Remove("PRD_DWG_ISSUE");

            }

            if (!dtPRD_MAST.Columns.Contains("PART_NO")) dtPRD_MAST.Columns.Add("PART_NO");
            if (!dtPRD_MAST.Columns.Contains("LOC_CODE")) dtPRD_MAST.Columns.Add("LOC_CODE");

            if (dtPRD_MAST.Rows.Count == 0)
            {
                DataRow dataRow = dtPRD_MAST.Rows.Add();
                dataRow["PART_NO"] = partNo;
                dataRow["LOC_CODE"] = string.Empty;
            }

            List<DDLOC_MAST> lstDDLOC_MAST = (from lm in _pccsBll.DB.DDLOC_MAST
                                              join pm in _pccsBll.DB.PRD_MAST on lm.LOC_CODE equals pm.BIF_PROJ
                                              where pm.PART_NO == partNo
                                              select lm).ToList<DDLOC_MAST>();

            DataTable dtDDLOC_MAST = new DataTable();
            if (lstDDLOC_MAST.IsNotNullOrEmpty())
            {
                dtDDLOC_MAST = lstDDLOC_MAST.ToDataTable<DDLOC_MAST>();
            }

            if (!dtDDLOC_MAST.Columns.Contains("LOC_CODE")) dtDDLOC_MAST.Columns.Add("LOC_CODE");
            if (!dtDDLOC_MAST.Columns.Contains("LOCATION")) dtDDLOC_MAST.Columns.Add("LOCATION");
            if (dtDDLOC_MAST.Rows.Count == 0)
            {
                DataRow dataRow = dtPRD_MAST.Rows.Add();
                dataRow["LOC_CODE"] = string.Empty;
            }

            List<PRD_CIREF> lstPRD_CIREF = (from pm in _pccsBll.DB.PRD_CIREF
                                            where pm.PART_NO == partNo && pm.CURRENT_CIREF == true
                                            select pm).ToList<PRD_CIREF>();

            DataTable dtPRD_CIREF = new DataTable();
            if (lstPRD_CIREF.IsNotNullOrEmpty())
            {
                dtPRD_CIREF = lstPRD_CIREF.ToDataTable<PRD_CIREF>();
                if (dtPRD_CIREF.Columns.Contains("DDCI_INFO")) dtPRD_CIREF.Columns.Remove("DDCI_INFO");
            }

            if (!dtPRD_CIREF.Columns.Contains("PART_NO")) dtPRD_CIREF.Columns.Add("PART_NO");
            if (dtPRD_CIREF.Rows.Count == 0)
            {
                DataRow dataRow = dtPRD_CIREF.Rows.Add();
                dataRow["PART_NO"] = partNo;
                dataRow["CI_REF"] = string.Empty;
            }

            List<DDCI_INFO> lstDDCI_INFO = (from ci in _pccsBll.DB.DDCI_INFO
                                            join pc in _pccsBll.DB.PRD_CIREF on ci.CI_REFERENCE equals pc.CI_REF
                                            where pc.PART_NO == partNo && pc.CURRENT_CIREF == true
                                            select ci).ToList<DDCI_INFO>();

            DataTable dtDDCI_INFO = new DataTable();
            if (lstDDCI_INFO.IsNotNullOrEmpty())
            {
                dtDDCI_INFO = lstDDCI_INFO.ToDataTable<DDCI_INFO>();

                if (dtDDCI_INFO.Columns.Contains("PRD_CIREF")) dtDDCI_INFO.Columns.Remove("PRD_CIREF");
                if (dtDDCI_INFO.Columns.Contains("DDSHAPE_DETAILS")) dtDDCI_INFO.Columns.Remove("DDSHAPE_DETAILS");
                if (dtDDCI_INFO.Columns.Contains("DDCOST_PROCESS_DATA")) dtDDCI_INFO.Columns.Remove("DDCOST_PROCESS_DATA");

                foreach (DataRow row in dtDDCI_INFO.Rows)
                {
                    string dateString = Convert.ToString(row["CUST_STD_DATE"]).Trim();
                    if (dateString.IsNotNullOrEmpty())
                    {
                        if (dateString.Contains("{0:"))
                            dateString = " Dt. " + row["CUST_STD_DATE"].ToValueAsString().Substring(3, 10);
                        else
                            dateString = " Dt. " + row["CUST_STD_DATE"].ToValueAsString().Substring(0, 10);
                    }

                    row["CUST_DWG_NO_ISSUE"] = Convert.ToString(row["CUST_DWG_NO_ISSUE"]).Trim() +
                                              dateString;

                    //row["CUST_DWG_NO_ISSUE"] = Convert.ToString(row["CUST_DWG_NO_ISSUE"]).Trim() + " Dt. " +
                    //                       (row["CUST_STD_DATE"].ToValueAsString());

                    row.EndEdit();
                    dtDDCI_INFO.AcceptChanges();
                }

            }

            if (!dtDDCI_INFO.Columns.Contains("CI_REFERENCE")) dtDDCI_INFO.Columns.Add("CI_REFERENCE");
            if (!dtDDCI_INFO.Columns.Contains("CUST_CODE")) dtDDCI_INFO.Columns.Add("CUST_CODE");
            if (dtDDCI_INFO.Rows.Count == 0)
            {
                DataRow dataRow = dtDDCI_INFO.Rows.Add();
                string ci_reference = string.Empty;
                if (lstPRD_CIREF.IsNotNullOrEmpty() && lstPRD_CIREF.Count > 0)
                {
                    ci_reference = lstPRD_CIREF[0].CI_REF;
                }
                dataRow["CI_REFERENCE"] = ci_reference;
                dataRow["CUST_CODE"] = string.Empty;
                dataRow.EndEdit();
            }

            List<DDCUST_MAST> lstDDCUST_MAST = (from ci in _pccsBll.DB.DDCI_INFO
                                                join pc in _pccsBll.DB.PRD_CIREF on ci.CI_REFERENCE equals pc.CI_REF
                                                join cm in _pccsBll.DB.DDCUST_MAST on ci.CUST_CODE equals cm.CUST_CODE
                                                where pc.PART_NO == partNo && pc.CURRENT_CIREF == true
                                                select cm).ToList<DDCUST_MAST>();


            DataTable dtDDCUST_MAST = new DataTable();
            if (lstDDCUST_MAST.IsNotNullOrEmpty())
            {
                dtDDCUST_MAST = lstDDCUST_MAST.ToDataTable<DDCUST_MAST>();
            }

            if (!dtDDCUST_MAST.Columns.Contains("CUST_CODE")) dtDDCUST_MAST.Columns.Add("CUST_CODE");
            if (!dtDDCUST_MAST.Columns.Contains("CUST_NAME")) dtDDCUST_MAST.Columns.Add("CUST_NAME");

            if (dtDDCUST_MAST.Rows.Count == 0)
            {
                DataRow dataRow = dtDDCUST_MAST.Rows.Add();
                dataRow["CUST_CODE"] = string.Empty;
            }

            List<PROCESS_ISSUE> lstPROCESS_ISSUE = (from row in _pccsBll.DB.PROCESS_ISSUE
                                                    where row.PART_NO == partNo && row.ROUTE_NO == routeNo
                                                    select row).ToList<PROCESS_ISSUE>();
            string maxISSUE_NO = string.Empty;
            if (lstPROCESS_ISSUE.IsNotNullOrEmpty() && lstPROCESS_ISSUE.Count > 0)
            {
                maxISSUE_NO = lstPROCESS_ISSUE.AsEnumerable().Max(p => p.ISSUE_NO);
            }

            DataTable dtPROCESS_ISSUE = (from row in lstPROCESS_ISSUE.AsEnumerable()
                                         where row.PART_NO == partNo && row.ROUTE_NO == routeNo && row.ISSUE_NO == maxISSUE_NO
                                         select row).ToList<PROCESS_ISSUE>().ToDataTableWithType<PROCESS_ISSUE>();

            if (!dtPROCESS_ISSUE.IsNotNullOrEmpty()) dtPROCESS_ISSUE = new DataTable();

            if (!dtPROCESS_ISSUE.Columns.Contains("PART_NO")) dtPROCESS_ISSUE.Columns.Add("PART_NO");
            if (!dtPROCESS_ISSUE.Columns.Contains("ROUTE_NO")) dtPROCESS_ISSUE.Columns.Add("ROUTE_NO");

            //if (!dtPROCESS_ISSUE.Columns.Contains("LOCATION")) dtPROCESS_ISSUE.Columns.Add("LOCATION");

            //if (!dtPROCESS_ISSUE.Columns.Contains("CUST_CODE")) dtPROCESS_ISSUE.Columns.Add("CUST_CODE");
            //if (!dtPROCESS_ISSUE.Columns.Contains("CUST_NAME")) dtPROCESS_ISSUE.Columns.Add("CUST_NAME");
            //if (!dtPROCESS_ISSUE.Columns.Contains("CUST_STD_DATE"))
            //    dtPROCESS_ISSUE.Columns.Add("CUST_STD_DATE", DateTime.Now.Date.GetType());
            //if (!dtPROCESS_ISSUE.Columns.Contains("CUST_DWG_NO")) dtPROCESS_ISSUE.Columns.Add("CUST_DWG_NO");
            //if (!dtPROCESS_ISSUE.Columns.Contains("PART_DESCRIPTION")) dtPROCESS_ISSUE.Columns.Add("PART_DESCRIPTION");
            //if (!dtPROCESS_ISSUE.Columns.Contains("CUST_DWG_NO_ISSUE")) dtPROCESS_ISSUE.Columns.Add("CUST_DWG_NO_ISSUE");
            //if (!dtPROCESS_ISSUE.Columns.Contains("FR_CS_DATE"))
            //    dtPROCESS_ISSUE.Columns.Add("FR_CS_DATE", DateTime.Now.Date.GetType());

            if (dtPROCESS_ISSUE.Rows.Count == 0)
            {
                DataRow dataRow = dtPROCESS_ISSUE.Rows.Add();
                dataRow["PART_NO"] = partNo;
                dataRow["ROUTE_NO"] = routeNo;

                //dataRow["LOCATION"] = ddloc_mast.LOCATION;

                //dataRow["CUST_CODE"] = ddci_info.CUST_CODE;
                //dataRow["CUST_NAME"] = ddci_info.REMARKS;
                //dataRow["CUST_STD_DATE"] = Convert.ToDateTime(ddci_info.CUST_STD_DATE).Date;
                //dataRow["CUST_DWG_NO"] = ddci_info.CUST_DWG_NO;
                //dataRow["PART_DESCRIPTION"] = ddci_info.PROD_DESC;
                //dataRow["CUST_DWG_NO_ISSUE"] = ddci_info.CUST_DWG_NO_ISSUE;
                //dataRow["FR_CS_DATE"] = Convert.ToDateTime(ddci_info.FR_CS_DATE).Date;
                dataRow.EndEdit();
                dataRow.AcceptChanges();
                dtPROCESS_ISSUE.AcceptChanges();

            }

            foreach (DataRow row in dtPROCESS_ISSUE.Rows)
            {
                row["ISSUE_ALTER"] = (Convert.ToString(row["ISSUE_DATE"]).Trim().Length > 10 ?
                                           row["ISSUE_DATE"].ToValueAsString().Substring(3, 10) : "");
                row.EndEdit();
                dtPROCESS_ISSUE.AcceptChanges();
            }

            if (dtPROCESS_ISSUE.Columns.Contains("PROCESS_MAIN"))
                dtPROCESS_ISSUE.Columns.Remove("PROCESS_MAIN");

            dtPROCESS_ISSUE.TableName = "PROCESS_ISSUE";

            List<PROCESS_SHEET> lstPROCESS_SHEET = (from row in _pccsBll.DB.PROCESS_SHEET
                                                    where row.PART_NO == partNo && row.ROUTE_NO == routeNo
                                                    select row).ToList<PROCESS_SHEET>();

            List<PROCESS_SHEET> distinctPROCESS_SHEET = new List<PROCESS_SHEET>();
            if (lstPROCESS_SHEET.IsNotNullOrEmpty())
            {
                distinctPROCESS_SHEET = lstPROCESS_SHEET.AsEnumerable().GroupBy(p => new { p.SEQ_NO, p.OPN_DESC, p.FMEA_RISK }).Select(g => (g.First())).ToList();

            }

            DataTable dtPROCESS_SHEET = new DataTable();
            if (lstPROCESS_SHEET.IsNotNullOrEmpty())
            {
                  dtPROCESS_SHEET = (from row in distinctPROCESS_SHEET
                                  // where row.PART_NO == partNo && row.ROUTE_NO == routeNo
                                   select row).ToList<PROCESS_SHEET>().ToDataTable<PROCESS_SHEET>();
            }

            if (dtPROCESS_SHEET.Rows.Count > 0)
            {
                dtPROCESS_SHEET.Select("PART_NO =  '" + partNo + "'  AND ROUTE_NO =  " + routeNo);
            }
            if (!dtPROCESS_SHEET.IsNotNullOrEmpty())
                dtPROCESS_SHEET = new DataTable();

            if (!dtPROCESS_SHEET.Columns.Contains("PART_NO")) dtPROCESS_SHEET.Columns.Add("PART_NO");
            if (!dtPROCESS_SHEET.Columns.Contains("ROUTE_NO")) dtPROCESS_SHEET.Columns.Add("ROUTE_NO");

            if (dtPROCESS_SHEET.Rows.Count == 0)
            {
                DataRow dataRow = dtPROCESS_SHEET.Rows.Add();
                dataRow["PART_NO"] = partNo;
                dataRow["ROUTE_NO"] = routeNo;
                dataRow.EndEdit();
            }

            dtPROCESS_SHEET.TableName = "PROCESS_SHEET";

            if (dtPROCESS_SHEET.Columns.Contains("PCCS"))
                dtPROCESS_SHEET.Columns.Remove("PCCS");

            if (dtPROCESS_SHEET.Columns.Contains("PROCESS_CC"))
                dtPROCESS_SHEET.Columns.Remove("PROCESS_CC");

            if (dtPROCESS_SHEET.Columns.Contains("PROCESS_MAIN"))
                dtPROCESS_SHEET.Columns.Remove("PROCESS_MAIN");

            DataSet dsReport = new DataSet();
            dsReport.DataSetName = "FLOW_CHART_REPORT_DATA_SET";

            dsReport.Tables.Add(dtPART_MASTER);
            dsReport.Tables.Add(dtPROC_FLOW_DGM);
            dsReport.Tables.Add(dtPROCESS_ISSUE);
            dsReport.Tables.Add(dtPROCESS_SHEET);
            dsReport.Tables.Add(dtPRD_MAST);
            dsReport.Tables.Add(dtDDLOC_MAST);
            dsReport.Tables.Add(dtPRD_CIREF);
            dsReport.Tables.Add(dtDDCI_INFO);
            dsReport.Tables.Add(dtDDCUST_MAST);

            DataColumn pfdPrimaryKeyColumn = null;
            DataColumn lmPrimaryKeyColumn = null;
            DataColumn cmPrimaryKeyColumn = null;
            DataColumn ciPrimaryKeyColumn = null;

            DataColumn pfdChildColumn = null;
            ForeignKeyConstraint pfdForeignKeyConstraint = null;


            DataColumn piChildColumn = null;
            ForeignKeyConstraint piForeignKeyConstraint = null;

            DataColumn psChildColumn = null;
            ForeignKeyConstraint psForeignKeyConstraint = null;

            DataColumn pcChildColumnPN = null;
            ForeignKeyConstraint pcForeignKeyConstraintPN = null;
            DataColumn pcChildColumnCI = null;
            ForeignKeyConstraint pcForeignKeyConstraintCI = null;

            DataColumn pmChildColumnPN = null;
            ForeignKeyConstraint pmForeignKeyConstraintPN = null;
            DataColumn pmChildColumnLM = null;
            ForeignKeyConstraint pmForeignKeyConstraintLM = null;

            DataColumn ciChildColumnCM = null;
            ForeignKeyConstraint ciForeignKeyConstraintCM = null;

            //if (dtPART_MASTER.PrimaryKey.IsNotNullOrEmpty() && dtPART_MASTER.PrimaryKey.Length == 0)
            //{
            //    DataColumn[] primaryKeyColumns = new DataColumn[3];
            //    primaryKeyColumns[0] = dtPART_MASTER.Columns["PART_NO"];
            //    pfdPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtPART_MASTER.PrimaryKey = primaryKeyColumns;

            //    //DataColumn[] primaryKeyColumns = new DataColumn[3];
            //    //primaryKeyColumns[0] = dtPROC_FLOW_DGM.Columns["PART_NO"];
            //    //primaryKeyColumns[1] = dtPROC_FLOW_DGM.Columns["ROUTE_NO"];
            //    ////primaryKeyColumns[2] = dtPROC_FLOW_DGM.Columns["ENTERED_BY"];
            //    //pfdPrimaryKeyColumn = primaryKeyColumns[0];
            //    //dtPROC_FLOW_DGM.PrimaryKey = primaryKeyColumns;

            //    primaryKeyColumns = new DataColumn[1];
            //    primaryKeyColumns[0] = dtDDLOC_MAST.Columns["LOC_CODE"];
            //    lmPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtDDLOC_MAST.PrimaryKey = primaryKeyColumns;

            //    primaryKeyColumns = new DataColumn[1];
            //    primaryKeyColumns[0] = dtDDCUST_MAST.Columns["CUST_CODE"];
            //    cmPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtDDCUST_MAST.PrimaryKey = primaryKeyColumns;

            //    primaryKeyColumns = new DataColumn[1];
            //    primaryKeyColumns[0] = dtDDCI_INFO.Columns["CI_REFERENCE"];
            //    ciPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtDDCI_INFO.PrimaryKey = primaryKeyColumns;

            //    pfdChildColumn = dtPROC_FLOW_DGM.Columns["PART_NO"];
            //    pfdForeignKeyConstraint = new ForeignKeyConstraint("pfdForeignKeyConstraint", pfdPrimaryKeyColumn, pfdChildColumn);
            //    pfdForeignKeyConstraint.DeleteRule = Rule.SetNull;
            //    pfdForeignKeyConstraint.UpdateRule = Rule.Cascade;
            //    pfdForeignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PROC_FLOW_DGM"].Constraints.Add(pfdForeignKeyConstraint);

            //    piChildColumn = dtPROCESS_ISSUE.Columns["PART_NO"];
            //    piForeignKeyConstraint = new ForeignKeyConstraint("PIForeignKeyConstraint", pfdPrimaryKeyColumn, piChildColumn);
            //    piForeignKeyConstraint.DeleteRule = Rule.SetNull;
            //    piForeignKeyConstraint.UpdateRule = Rule.Cascade;
            //    piForeignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PROCESS_ISSUE"].Constraints.Add(piForeignKeyConstraint);

            //    psChildColumn = dtPROCESS_SHEET.Columns["PART_NO"];
            //    psForeignKeyConstraint = new ForeignKeyConstraint("PSForeignKeyConstraint", pfdPrimaryKeyColumn, psChildColumn);
            //    psForeignKeyConstraint.DeleteRule = Rule.SetNull;
            //    psForeignKeyConstraint.UpdateRule = Rule.Cascade;
            //    psForeignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PROCESS_SHEET"].Constraints.Add(psForeignKeyConstraint);

            //    pcChildColumnPN = dtPRD_CIREF.Columns["PART_NO"];
            //    pcForeignKeyConstraintPN = new ForeignKeyConstraint("pcForeignKeyConstraint_PART_NO", pfdPrimaryKeyColumn, pcChildColumnPN);
            //    pcForeignKeyConstraintPN.DeleteRule = Rule.SetNull;
            //    pcForeignKeyConstraintPN.UpdateRule = Rule.Cascade;
            //    pcForeignKeyConstraintPN.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_CIREF"].Constraints.Add(pcForeignKeyConstraintPN);

            //    pcChildColumnCI = dtPRD_CIREF.Columns["CI_REF"];
            //    pcForeignKeyConstraintCI = new ForeignKeyConstraint("pcForeignKeyConstraint_CI_REF", ciPrimaryKeyColumn, pcChildColumnCI);
            //    pcForeignKeyConstraintCI.DeleteRule = Rule.SetNull;
            //    pcForeignKeyConstraintCI.UpdateRule = Rule.Cascade;
            //    pcForeignKeyConstraintCI.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_CIREF"].Constraints.Add(pcForeignKeyConstraintCI);

            //    pmChildColumnPN = dtPRD_MAST.Columns["PART_NO"];
            //    pmForeignKeyConstraintPN = new ForeignKeyConstraint("PMForeignKeyConstraint_PART_NO", pfdPrimaryKeyColumn, pmChildColumnPN);
            //    pmForeignKeyConstraintPN.DeleteRule = Rule.SetNull;
            //    pmForeignKeyConstraintPN.UpdateRule = Rule.Cascade;
            //    pmForeignKeyConstraintPN.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_MAST"].Constraints.Add(pmForeignKeyConstraintPN);

            //    pmChildColumnLM = dtPRD_MAST.Columns["LOC_CODE"];
            //    pmForeignKeyConstraintLM = new ForeignKeyConstraint("PMForeignKeyConstraint_LOC_CODE", lmPrimaryKeyColumn, pmChildColumnLM);
            //    pmForeignKeyConstraintLM.DeleteRule = Rule.SetNull;
            //    pmForeignKeyConstraintLM.UpdateRule = Rule.Cascade;
            //    pmForeignKeyConstraintLM.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_MAST"].Constraints.Add(pmForeignKeyConstraintLM);

            //    ciChildColumnCM = dtDDCI_INFO.Columns["CUST_CODE"];
            //    ciForeignKeyConstraintCM = new ForeignKeyConstraint("ciForeignKeyConstraint_CUST_CODE", cmPrimaryKeyColumn, ciChildColumnCM);
            //    ciForeignKeyConstraintCM.DeleteRule = Rule.SetNull;
            //    ciForeignKeyConstraintCM.UpdateRule = Rule.Cascade;
            //    ciForeignKeyConstraintCM.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["DDCI_INFO"].Constraints.Add(ciForeignKeyConstraintCM);

            //    dsReport.EnforceConstraints = true;

            //}


            int ictr = 1;
            int igroup = 1;
            int maxSeqPerPage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxSeqPerPage"]);
            foreach (DataRow drRow in dsReport.Tables["PROCESS_SHEET"].Rows)
            {
                drRow["ROUTE_NO"] = igroup.ToString();
                if (ictr >= maxSeqPerPage) // Jeyan modified from 14 to 13
                {
                    ictr = 0;
                    igroup = igroup + 1;
                }
                ictr = ictr + 1;
            }

            //dsReport.Tables["PROCESS_SHEET"].Rows.Clear();
            //dsReport.Tables["PROCESS_ISSUE"].Rows.Clear();

            //dsReport.Tables["PRD_MAST"].Rows.Clear();
            //dsReport.Tables["PRD_CIREF"].Rows.Clear();
            //dsReport.Tables["DDCI_INFO"].Rows.Clear();
            //dsReport.Tables["DDCUST_MAST"].Rows.Clear();
            //dsReport.Tables["DDLOC_MAST"].Rows.Clear();
            //dsReport.Tables["PROC_FLOW_DGM"].Rows.Clear();
            //dsReport.Tables["PART_MASTER"].Rows.Clear();
            //dsReport.WriteXml("E:\\" + dsReport.DataSetName + ".xml", XmlWriteMode.WriteSchema);

            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }
            
            dictFormulas.Add("RouteNo", "Process No : " + routeNo.ToString());
            frmReportViewer reportViewer = new frmReportViewer(dsReport, "FlowChart", CrystalDecisions.Shared.ExportFormatType.NoFormat, dictFormulas);
            Progress.End();
            if (!reportViewer.ReadyToShowReport) return;
            reportViewer.ShowDialog();


        }

        private void GenerateProcessFlowDiagram(string partNo, decimal routeNo, ref DataTable dtPROC_FLOW_DGM)
        {
            string imageFilePath = "";
            string filePath = "";
            string wmfFilePath = "";
            List<PROCESS_SHEET> lstProcSheet;
            Microsoft.Office.Interop.Visio.Page visioPage;
            Microsoft.Office.Interop.Visio.InvisibleApp visApp;
            Microsoft.Office.Interop.Visio.Documents visioDocs;
            Microsoft.Office.Interop.Visio.Document visioStencil;
            Microsoft.Office.Interop.Visio.Shape visioStart = null;
            Microsoft.Office.Interop.Visio.Shape visioOperationShape;
            Microsoft.Office.Interop.Visio.Shape visioTransportationShape = null;
            Microsoft.Office.Interop.Visio.Shape visioTransportationShape1 = null;
            Microsoft.Office.Interop.Visio.Shape visioNextTransportationShape = null;
            Microsoft.Office.Interop.Visio.Shape visioOperationShape1;
            Microsoft.Office.Interop.Visio.Shape visioInspectionShape;
            Microsoft.Office.Interop.Visio.Shape visioPrev = null;
            Microsoft.Office.Interop.Visio.Master visioTransportationMaster;
            Microsoft.Office.Interop.Visio.Master visioOperation;
            Microsoft.Office.Interop.Visio.Master visioInspection;
            Microsoft.Office.Interop.Visio.Master visioSmall;
            Microsoft.Office.Interop.Visio.Cell celObj;
            Microsoft.Office.Interop.Visio.Shape vsoConnectorShape = null;
            //Dim celObj   As Visio.Cell 

            double iDot = -1.49;
            double dblTransport3 = -1.1;
            int minConnector = 0;
            int maxConnector = 0;
            int maxSeqPerPage = 13; // Jeyan - Value changed from 10 to 13
            int maxCircleCount = 7; // Jeyan - Value changed from 5 to 7
            int lineCount = 1;
            int circleCount = 0;
            double yPosition = 10.5;
            int itransport = 0;
            double dblsize = -.75;

            // Added by Jeyan - Start
            //double dblconnectDistance = 7; // Jeyan - Value changed from 4 to 7
            //double dblxConnectDistance = 8; // Jeyan - Value changed from 5 to 8

            maxSeqPerPage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxSeqPerPage"]);
            maxCircleCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxCircleCount"]);
            double dblconnectDistance = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["dblconnectDistance"]);
            double dblxConnectDistance = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["dblxConnectDistance"]);
            // Jeyan End
            double dblcircle = -.5;
            string lineColor = "=RGB(0,0,0)";
            string lineWeight = "=1.25 pt";
            string charSize = "=9 pt.";
            string charStyle = "1";
            System.Diagnostics.Process[] alreadyRunningProcesses = null;
            byte[] _diagram;
            try
            {
                filePath = GetFilePath();
                wmfFilePath = filePath + "\\temp.wmf";
                imageFilePath = filePath + "\\temp.bmp";

                lstProcSheet = (from row in _pccsBll.DB.PROCESS_SHEET
                                where row.PART_NO == partNo
                                    && row.ROUTE_NO == routeNo
                                select row).ToList<PROCESS_SHEET>();
                if (!lstProcSheet.IsNotNullOrEmpty()) return;

                alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
                foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
                {
                    process.Kill();
                    process.Close();
                }

                int no_of_loops = ((lstProcSheet.Count - 1) / maxSeqPerPage) + 1;
                //int no_of_loops = 0;
                maxConnector = maxSeqPerPage - 1;
                for (int iLoop = 0; iLoop <= no_of_loops; iLoop++)
                {
                    _diagram = null;
                    minConnector = iLoop * maxSeqPerPage;
                    //maxConnector = (iLoop + 1) * 13;

                    //if (iLoop == no_of_loops)

                    maxConnector = Math.Min(maxConnector, lstProcSheet.Count - 1);

                    visApp = new Microsoft.Office.Interop.Visio.InvisibleApp();

                    visApp.Documents.Add(@"");
                    visioDocs = visApp.Documents;

                    visioStencil = visioDocs.OpenEx("TQM Diagram Shapes.vss",
                    (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);


                    visioTransportationMaster = visioStencil.Masters.get_ItemU(@"Transportation"); //Arrow
                    visioOperation = visioStencil.Masters.get_ItemU(@"Operation"); //Circle
                    visioInspection = visioStencil.Masters.get_ItemU(@"Inspection/ measurement"); //Square
                    visioSmall = visioStencil.Masters.get_ItemU(@"Inspection/ measurement"); //Square
                    visioPage = visApp.ActivePage;
                    double xAxis = 0;
                    double y = 0;
                    visioNextTransportationShape = null;

                    lineCount = 1;
                    circleCount = 0;
                    yPosition = 10.5;

                    for (int ictr = minConnector; ictr <= maxConnector; ictr++)
                    {
                        double xPosition = ictr;

                        if (ictr == minConnector)
                        {
                            xPosition = 0;
                        }
                        itransport = lstProcSheet[ictr].TRANSPORT.ToValueAsString().ToIntValue();
                        if (lstProcSheet[ictr].OPN_CD == 2810 || lstProcSheet[ictr].OPN_CD == 2800 || lstProcSheet[ictr].OPN_CD == 2801)
                        {
                            visioOperationShape = visioPage.Drop(visioInspection, xPosition, yPosition);
                        }
                        else
                        {
                            visioOperationShape = visioPage.Drop(visioOperation, xPosition, yPosition);
                        }
                        visioOperationShape.Text = ((ictr + 1) * 10).ToString("D3"); // Jeyan - Format to 3 digit ex: 10->010, 20->020 like wise
                        visioOperationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblcircle, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                        visioOperationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblcircle, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                        visioOperationShape.Cells["LineColor"].FormulaForce = lineColor;
                        visioOperationShape.Cells["LineWeight"].FormulaForce = lineWeight;
                        celObj = visioOperationShape.Cells["Char.Size"];
                        celObj.Formula = charSize;
                        celObj = visioOperationShape.Cells["Char.Style"];
                        celObj.Formula = charStyle;
                        if (visioNextTransportationShape != null)
                        {
                            visioNextTransportationShape.Cells["LineColor"].FormulaForce = lineColor;
                            visioNextTransportationShape.Cells["LineWeight"].FormulaForce = lineWeight;
                            visioNextTransportationShape.CellsU["LineWeight"].FormulaForce = lineWeight;

                            if (lineCount % 2 != 0 && circleCount % maxCircleCount != 0)
                            {
                                visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                                xAxis = visioOperationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioOperationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                                //xAxis = visioNextTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                //visioNextTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                            }
                            else if (lineCount % 2 == 0 && circleCount % maxCircleCount == 0)
                            {
                                visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                y = visioOperationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioOperationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                            }
                            else if (lineCount % 2 != 0 && circleCount % maxCircleCount == 0)
                            {
                                visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                y = visioOperationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioOperationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                            }
                            else
                            {
                                visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                                xAxis = visioOperationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioOperationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance);
                                //visioNextTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblconnectDistance);
                            }

                        }
                        circleCount++;
                        if (circleCount % maxCircleCount == 0) lineCount++;
                        if (ictr != maxConnector)
                        {
                            if (lineCount % 2 != 0 && circleCount % maxCircleCount != 0)
                            {
                                if (itransport == 0)
                                {
                                    visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2), yPosition);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                }
                                else if (itransport == 1)
                                {
                                    visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2), yPosition);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 2)
                                {
                                    visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2), yPosition);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 3)
                                {
                                    visioTransportationShape1 = visioPage.Drop(visioTransportationMaster, (xPosition * 2), yPosition);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    visioOperationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                                    xAxis = visioTransportationShape1.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape1.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);

                                    visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * 2), yPosition);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    visioTransportationShape1.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                                    xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                                }
                                if (itransport == 0 || itransport == 1 || itransport == 2)
                                {
                                    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                                    xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                                }
                            }
                            else if (lineCount % 2 == 0 && circleCount % maxCircleCount == 0)
                            {
                                if (itransport == 0)
                                {
                                    visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 1)
                                {
                                    visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 2)
                                {
                                    visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 3)
                                {

                                    visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    visioTransportationShape1 = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();

                                    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    yPosition = yPosition - 1;
                                    y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance + 3);

                                    visioTransportationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    yPosition = yPosition - 1;
                                    y = visioTransportationShape1.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape1.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance + 3);

                                    visioTransportationShape = visioTransportationShape1;



                                    //visioTransportationShape1 = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    //visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    //visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    //visioTransportationShape1.Rotate90();
                                    //visioTransportationShape1.Rotate90();
                                    //visioTransportationShape1.Rotate90();
                                    //visioOperationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    //yPosition = yPosition - 1;
                                    //y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    //visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);


                                    //visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    //visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    //visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    //visioTransportationShape.Rotate90();
                                    //visioTransportationShape.Rotate90();
                                    //visioTransportationShape.Rotate90();
                                    //visioTransportationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    //yPosition = yPosition - 1;
                                    //y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    //visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);

                                }

                                if (itransport == 0 || itransport == 1 || itransport == 2)
                                {
                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    yPosition = yPosition - 1;
                                    y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                                }
                                //return;
                            }
                            else if (lineCount % 2 != 0 && circleCount % maxCircleCount == 0)
                            {
                                if (itransport == 0)
                                {
                                    visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 1)
                                {
                                    visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 2)
                                {
                                    visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 3)
                                {
                                    visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    visioTransportationShape1 = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    yPosition = yPosition - 1;
                                    y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);


                                    visioTransportationShape1.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    yPosition = yPosition - 1;
                                    y = visioTransportationShape1.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape1.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                                    visioTransportationShape = visioTransportationShape1;
                                }

                                if (itransport == 0 || itransport == 1 || itransport == 2)
                                {
                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                    yPosition = yPosition - 1;
                                    y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                                }
                            }
                            else
                            {
                                if (itransport == 0)
                                {
                                    visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * -2), yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 1)
                                {
                                    visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * -2), yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 2)
                                {
                                    visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * -2), yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                }
                                else if (itransport == 3)
                                {
                                    visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * -.5), yPosition - lineCount + 1);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                                    xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance + 2);

                                    visioTransportationShape1 = visioPage.Drop(visioSmall, (xPosition * -.5), yPosition - lineCount + 1);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                    visioTransportationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                                    xAxis = visioTransportationShape1.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape1.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance + 2);

                                    visioTransportationShape = visioTransportationShape1;
                                }

                                if (itransport == 0 || itransport == 1 || itransport == 2)
                                {
                                    visioTransportationShape.Rotate90();
                                    visioTransportationShape.Rotate90();
                                    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                                    xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                    visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance);
                                }
                                //visioOperationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblconnectDistance);
                                //double y = visioOperationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                //visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + 5);
                            }
                        }



                        visioNextTransportationShape = visioTransportationShape;
                    }

                    //                    visApp.ActiveDocument.SaveAs("E:\\File.VSD");
                    visioPage.Export(imageFilePath);
                    visioPage.Export(wmfFilePath);

                    //Image img = Image.FromFile(imageFilePath);
                    //imageFilePath = "d:\\abcd.bmp";
                    //Image img1 = ResizeImage(img.Height / 2, img);
                    //imageFilePath = "d:\\abcd.bmp";
                    //img1.Save(imageFilePath);

                    //visApp.Quit();
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(imageFilePath);
                    System.IO.MemoryStream strm = new System.IO.MemoryStream();

                    using (System.IO.FileStream file = new System.IO.FileStream(imageFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        _diagram = new byte[file.Length];
                        file.Read(_diagram, 0, (int)file.Length);
                        strm.Write(_diagram, 0, (int)file.Length);
                        file.Close();
                        file.Dispose();
                        //PhotoSource = bitmap;
                    }

                    DataRow row = null;
                    if (dtPROC_FLOW_DGM.Rows.Count == iLoop)
                        row = dtPROC_FLOW_DGM.Rows.Add();
                    else
                        row = dtPROC_FLOW_DGM.Rows[iLoop];

                    row["DIAGRAM"] = _diagram;
                    row["PART_NO"] = partNo;
                    row["ROUTE_NO"] = iLoop + 1;

                    row.AcceptChanges();
                    dtPROC_FLOW_DGM.AcceptChanges();
                    maxConnector += maxSeqPerPage;

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
            foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
            {
                process.Kill();
                process.Close();
            }
        }

        private Image ResizeImage(int newSize, Image originalImage)
        {
            if (originalImage.Width <= newSize)
                newSize = originalImage.Width;

            var newHeight = originalImage.Height * newSize / originalImage.Width;

            if (newHeight > newSize)
            {
                // Resize with height instead
                newSize = originalImage.Width * newSize / originalImage.Height;
                newHeight = newSize;
            }
            return originalImage.GetThumbnailImage(newSize, newHeight, null, IntPtr.Zero);
        }

    }
}
