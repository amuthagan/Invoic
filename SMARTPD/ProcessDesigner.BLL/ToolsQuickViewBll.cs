using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class ToolsQuickViewBll : Essential
    {
        public ToolsQuickViewBll(UserInformation userInformation)
        {
            if (userInformation == null) throw new ArgumentNullException("userInformation");

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;

            this.userInformation = userInformation;
        }

        public ToolsQuickViewBll()
        {
            // TODO: Complete member initialization
        }

        public bool GetToolFamily(ToolsQuickVModel toolQv)
        {
            try
            {
                var dt = ToDataTable((from o in DB.TOOL_FAMILY
                                      select new { o.FAMILY_CD, o.FAMILY_NAME }).ToList());
                toolQv.DVToolFamily = dt != null ? dt.DefaultView : null;

                //GetImage(ToolAdm);
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetToolDimension(ToolsQuickVModel toolQv)
        {
            try
            {
                toolQv.ToolDimension = (from o in DB.TOOL_DIMENSION
                                        where o.TOOL_CD == toolQv.TOOL_CD.Replace(" ", "")
                                        select o).FirstOrDefault<TOOL_DIMENSION>();

                toolQv.FAMILY_CD = toolQv.ToolDimension != null ? toolQv.ToolDimension.FAMILY_CD : "";

                var dt = ToDataTable((from o in DB.TOOL_ISSUES
                                      where o.TOOL_CD == toolQv.TOOL_CD.Replace(" ", "")
                                      select o).ToList());
                if (dt != null)
                {
                    toolQv.DVToolIssue = dt.DefaultView;
                    toolQv.DVToolIssue.AddNew();
                }
                else
                {
                    toolQv.DVToolIssue = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetToolIssues(ToolsQuickVModel toolQv)
        {
            try
            {
                var dt = (from o in DB.TOOL_ISSUES
                          where o.TOOL_CD == toolQv.TOOL_CD.Replace(" ", "")
                          select o).ToList().ToDataTable();
                if (dt != null)
                {
                    dt.ColumnChanged += new DataColumnChangeEventHandler(ColumnChanged_ToolIssues);
                    dt.RowChanged += new DataRowChangeEventHandler(RowChanged_ToolIssues);
                    AddNewRowIfNotExistNewRow(dt);
                    dt.AcceptChanges();
                    toolQv.DVRevisionToolIssue = dt.DefaultView;
                    //toolQv.DVRevisionToolIssue.AddNew();
                }
                else
                {
                    toolQv.DVRevisionToolIssue = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ColumnChanged_ToolIssues(object sender, DataColumnChangeEventArgs e)
        {
            AddNewRowIfNotExistNewRow((DataTable)sender);
        }

        private void RowChanged_ToolIssues(object sender, DataRowChangeEventArgs e)
        {
            AddNewRowIfNotExistNewRow((DataTable)sender);
        }

        private void AddNewRowIfNotExistNewRow(DataTable dt)
        {
            if (dt == null) return;
            DataView dv = dt.Copy().DefaultView;
            dv.RowFilter =
                "CONVERT(Isnull(ALTERATIONS,''), System.String) = ''"; // CONVERT(Isnull(ISSUE_NO,''), System.String) = '' AND  CONVERT(Isnull(INTL,''), System.String) = ''
            if (dv.Count == 0)
            {
                DataRow dr = dt.Rows.Add();
                dr.BeginEdit();

            }
            dv.RowFilter = null;
        }

        public bool GetToolParameter(ToolsQuickVModel toolQv)
        {
            try
            {
                var dt = ToDataTable((from o in DB.TOOL_PARAMETER
                                      where o.FAMILY_CD == toolQv.FAMILY_CD
                                      select o).ToList());
                if (dt != null)
                {

                    toolQv.DVToolParameter = dt.DefaultView;
                    toolQv.DVToolParameter.AddNew();
                }
                else
                {
                    toolQv.DVToolParameter = null;
                }

                GetToolImage(toolQv);

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static UInt32 FindMimeFromData(
            UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
            UInt32 dwMimeFlags,
            out UInt32 ppwzMimeOut,
            UInt32 dwReserverd);


        public string GetMimeFromFile(byte[] byteArray)
        {

            byte[] buffer = new byte[256];
            using (MemoryStream fs = new MemoryStream(byteArray))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = null;
                //mime = Marshal.PtrToStringUni(mimeTypePtr);
                //Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception e)
            {
                e.LogException();
                return e.Message;
            }
        }


        public void GetToolImage(ToolsQuickVModel toolQv)
        {
            try
            {
                var toolfamily = (from c in DB.TOOL_FAMILY
                                  where c.FAMILY_CD == toolQv.FAMILY_CD
                                  select c).SingleOrDefault<TOOL_FAMILY>();

                if (toolfamily == null || toolfamily.PICTURE == null) return;
                var photosource = toolfamily.PICTURE.ToArray();
                var offset = GetImageBytesFromOLEField(photosource, toolQv);
                var strm = new MemoryStream();
                strm.Write(photosource, offset, photosource.Length - offset);
                toolQv.MimeType = GetMimeFromFile(photosource);
                toolQv.File_Name = AppDomain.CurrentDomain.BaseDirectory + "File" + toolfamily.FAMILY_CD + "." + toolQv.FileType;
                toolQv.DisplayFile_Name = toolQv.FileType == "vsd"
                    ? AppDomain.CurrentDomain.BaseDirectory + "File" + toolfamily.FAMILY_CD + ".bmp"
                    : AppDomain.CurrentDomain.BaseDirectory + "File" + toolfamily.FAMILY_CD + "." + toolQv.FileType;

                var fileStream = File.Create(toolQv.File_Name);
                toolQv.picture = strm;
                strm.Seek(0, SeekOrigin.Begin);
                strm.CopyTo(fileStream);
                fileStream.Close();
                fileStream.Dispose();
                strm.Close();
                strm.Dispose();
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private int GetImageBytesFromOLEField(byte[] oleFieldBytes, ToolsQuickVModel toolQv)
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
            string strVTemp = strTemp.Substring(0, 300);

            // Search for the block
            int iPos = -1;
            if (strVTemp.IndexOf(BITMAP_ID_BLOCK, StringComparison.Ordinal) != -1)
            {
                iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK, StringComparison.Ordinal);
                toolQv.FileType = "bmp";
            }
            else if (strVTemp.IndexOf(JPG_ID_BLOCK, StringComparison.Ordinal) != -1)
            {
                iPos = strVTemp.IndexOf(JPG_ID_BLOCK, StringComparison.Ordinal);
                toolQv.FileType = "bmp";
            }
            else if (strVTemp.IndexOf(PNG_ID_BLOCK, StringComparison.Ordinal) != -1)
            {
                iPos = strVTemp.IndexOf(PNG_ID_BLOCK, StringComparison.Ordinal);
                toolQv.FileType = "png";
            }
            else if (strVTemp.IndexOf(GIF_ID_BLOCK, StringComparison.Ordinal) != -1)
            {
                iPos = strVTemp.IndexOf(GIF_ID_BLOCK, StringComparison.Ordinal);
                toolQv.FileType = "gif";
            }
            else if (strVTemp.IndexOf(TIFF_ID_BLOCK, StringComparison.Ordinal) != -1)
            {
                iPos = strVTemp.IndexOf(TIFF_ID_BLOCK, StringComparison.Ordinal);
                toolQv.FileType = "tiff";
            }
            else if (strVTemp.IndexOf(VSD_ID_BLOCK, StringComparison.Ordinal) != -1)
            {
                toolQv.FileType = "vsd";
                iPos = strVTemp.IndexOf(VSD_ID_BLOCK, StringComparison.Ordinal);
            }
            //if (iPos == -1)
            //    throw new Exception("Unable to determine header size for the OLE Object");
            /*
            imageBytes = new byte[oleFieldBytes.LongLength - iPos];
            MemoryStream ms = new MemoryStream();
            ms.Write(oleFieldBytes, iPos, oleFieldBytes.Length - iPos);
            imageBytes = ms.ToArray();
            ms.Close();
            ms.Dispose();
            */
            if (iPos == -1)
            {
                iPos = 0;
            }
            return iPos;
        }

        public DateTime? ServerDateTime()
        {
            return serverDate;
        }

        public bool Insert<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as TOOL_ISSUES).IsNotNullOrEmpty())
                {
                    TOOL_ISSUES obj = entity as TOOL_ISSUES;
                    try
                    {
                        if (!obj.TOOL_CD.IsNotNullOrEmpty()) obj.TOOL_CD = GenerateNextNumber("TOOL_ISSUES", "TOOL_CD");

                        DB.TOOL_ISSUES.InsertOnSubmit(obj);
                        DB.SubmitChanges();

                        System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.TOOL_ISSUES.DeleteOnSubmit(obj);
                    }

                }
            }

            returnValue = true;
            return returnValue;
        }

        public bool Update<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as TOOL_ISSUES).IsNotNullOrEmpty())
                {
                    TOOL_ISSUES obj = null;
                    TOOL_ISSUES activeEntity = (entity as TOOL_ISSUES);

                    obj = (from row in DB.TOOL_ISSUES
                           where row.TOOL_CD == activeEntity.TOOL_CD && row.ISSUE_NO == activeEntity.ISSUE_NO
                           select row).SingleOrDefault<TOOL_ISSUES>();
                    if (obj.IsNotNullOrEmpty())
                    {
                        try
                        {
                            obj.TOOL_CD = activeEntity.TOOL_CD;
                            obj.ISSUE_NO = activeEntity.ISSUE_NO;
                            obj.ISSUE_DATE = activeEntity.ISSUE_DATE;
                            obj.ALTERATIONS = activeEntity.ALTERATIONS;
                            obj.INTL = activeEntity.INTL;
                            DB.SubmitChanges();

                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.TOOL_ISSUES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                        }
                    }
                    else
                    {
                        returnValue = Delete<TOOL_ISSUES>(new List<TOOL_ISSUES> { activeEntity });
                        returnValue = Insert<TOOL_ISSUES>(new List<TOOL_ISSUES> { activeEntity });
                    }
                }
            }
            returnValue = true;
            return returnValue;
        }

        public bool Delete<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as TOOL_ISSUES).IsNotNullOrEmpty())
                {
                    TOOL_ISSUES obj = null;
                    TOOL_ISSUES activeEntity = (entity as TOOL_ISSUES);
                    try
                    {
                        obj = (from row in DB.TOOL_ISSUES
                               where row.TOOL_CD == activeEntity.TOOL_CD && row.ISSUE_NO == activeEntity.ISSUE_NO
                               select row).SingleOrDefault<TOOL_ISSUES>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            DB.TOOL_ISSUES.DeleteOnSubmit(obj);
                            DB.SubmitChanges();

                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.TOOL_ISSUES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                    }
                }
            }
            return returnValue;
        }

        public DataTable GetReportData(ToolsQuickVModel toolQv)
        {
            string qry = @"SELECT a.[TOOL_CD],
                               a.[PAGE_NO],
                               a.[DWG] as DWG,
                               a.[IDPK],
                               b.[ISSUE_NO],
                               b.[ISSUE_DATE],
                               b.[ALTERATIONS],
                               b.[INTL],
                               c.[FAMILY_CD],
                               c.[DESCRIPTION],
                               c.[MATERIAL_SIZE],
                               c.[HEAT_TREATMENT],
                               c.[PROGRAM_NO],
                               c.[P001],
                               c.[P002],
                               c.[P003],
                               c.[P004],
                               c.[P005],
                               c.[P006],
                               c.[P007],
                               c.[P008],
                               c.[P009],
                               c.[P010],
                               c.[P011],
                               c.[P012],
                               c.[P013],
                               c.[P014],
                               c.[P015],
                               c.[P016],
                               c.[P017],
                               c.[P018],
                               c.[P019],
                               c.[P020],
                               c.[P021],
                               c.[P022],
                               c.[P023],
                               c.[P024],
                               c.[P025],
                               c.[P026],
                               c.[P027],
                               c.[P028],
                               c.[P029],
                               c.[P030],
                               c.[P031],
                               c.[P032],
                               c.[P033],
                               c.[P034],
                               c.[P035],
                               c.[P036],
                               c.[P037],
                               c.[P038],
                               c.[P039],
                               c.[P040],
                               c.[P041],
                               c.[P042],
                               c.[P043],
                               c.[P044],
                               c.[P045],
                               c.[P046],
                               c.[P047],
                               c.[P048],
                               c.[P049],
                               c.[P050],
                               c.[S001],
                               c.[S002],
                               c.[S003],
                               c.[S004],
                               c.[S005],
                               c.[S006],
                               c.[S007],
                               c.[S008],
                               c.[S009],
                               c.[S010],
                               c.[ROWID]
                        FROM tool_drwg a
                        LEFT OUTER JOIN tool_issues b ON a.tool_cd = b.tool_cd
                        LEFT OUTER JOIN tool_dimension c ON a.tool_cd = c.tool_cd
						LEFT OUTER JOIN TOOL_FAMILY d on c.FAMILY_CD = d.FAMILY_CD
                        WHERE a.tool_cd = '" + toolQv.TOOL_CD + "'";
            DataTable dtProcessSheet = Dal.GetDataTable(qry);
            if (dtProcessSheet.IsNotNullOrEmpty())
            {

            }
            else
            {
                dtProcessSheet = new DataTable();

            }
            return dtProcessSheet;
        }
    }
}
