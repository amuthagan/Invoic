using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;

namespace ProcessDesigner.BLL
{
    public class ToolAdminBll : Essential
    {
        public ToolAdminBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public ToolAdminBll()
        {
            // TODO: Complete member initialization
        }

        public bool GetToolFamily(ToolAdminModel tooladm)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.TOOL_FAMILY
                                  select new { o.FAMILY_CD, o.FAMILY_NAME }).ToList());
                if (dt != null)
                {

                    tooladm.DVToolFamily = dt.DefaultView;
                }
                else
                {
                    tooladm.DVToolFamily = null;
                }

                dt = ToDataTable((from o in DB.TOOL_PARAMETER
                                  where 1 == 2
                                  select o).ToList());
                if (dt != null)
                {

                    tooladm.DVToolParameter = dt.DefaultView;
                    tooladm.DVToolParameter.AddNew();
                    tooladm.DTDeletedRecords = dt.Clone();
                }
                else
                {
                    tooladm.DVToolParameter = null;
                }
                //GetImage(ToolAdm);
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetToolParameter(ToolAdminModel tooladm)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.TOOL_PARAMETER
                                  where o.FAMILY_CD == tooladm.FAMILY_CD
                                  select o).ToList());
                if (dt != null)
                {

                    tooladm.DVToolParameter = dt.DefaultView;
                    tooladm.DVToolParameter.AddNew();                  
                    tooladm.DTDeletedRecords = dt.Clone();
                }
                else
                {
                    tooladm.DVToolParameter = null;
                }

                dt = ToDataTable((from o in DB.TOOL_FAMILY
                                  where o.FAMILY_CD == tooladm.FAMILY_CD
                                  select new { o.PICTURE }).ToList());
                if (dt != null)
                {
                    tooladm.DVPicture = dt.DefaultView;
                    GetToolImage(tooladm);
                }
                else
                {
                    tooladm.DVPicture = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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


        public void GetToolImage(ToolAdminModel toolAdm)
        {

            int offset = 0;
            TOOL_FAMILY toolfamily = null;
            FileStream fileStream = null;
            byte[] photosource = null;
            System.IO.MemoryStream strm;
            try
            {
                toolfamily = (from c in DB.TOOL_FAMILY
                              where c.FAMILY_CD == toolAdm.FAMILY_CD
                              select c).SingleOrDefault<TOOL_FAMILY>();

                if (toolfamily.IsNotNullOrEmpty())
                {
                    if (toolfamily.PICTURE != null)
                    {
                        photosource = toolfamily.PICTURE.ToArray();
                        offset = GetImageBytesFromOLEField(photosource, toolAdm);
                        strm = new System.IO.MemoryStream();
                        strm.Write(photosource, offset, photosource.Length - offset);
                        toolAdm.MimeType = getMimeFromFile(photosource);
                        toolAdm.File_Name = System.AppDomain.CurrentDomain.BaseDirectory + "File" + toolfamily.FAMILY_CD + "." + toolAdm.FileType;
                        if (toolAdm.FileType == "vsd")
                        {
                            toolAdm.DisplayFile_Name = System.AppDomain.CurrentDomain.BaseDirectory + "File" + toolfamily.FAMILY_CD + ".bmp";
                        }
                        else
                        {
                            toolAdm.DisplayFile_Name = System.AppDomain.CurrentDomain.BaseDirectory + "File" + toolfamily.FAMILY_CD + "." + toolAdm.FileType;
                        }
                        //ToolAdm.File_Name = "E:\\File1.vsd";
                        fileStream = File.Create(toolAdm.File_Name);
                        toolAdm.picture = strm;
                        strm.Seek(0, SeekOrigin.Begin);
                        strm.CopyTo(fileStream);
                        fileStream.Close();
                        fileStream.Dispose();
                        strm.Close();
                        strm.Dispose();
                    }
                }
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

        private int GetImageBytesFromOLEField(byte[] oleFieldBytes, ToolAdminModel toolAdm)
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
            if (strVTemp.IndexOf(BITMAP_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK);
                toolAdm.FileType = "bmp";
            }
            else if (strVTemp.IndexOf(JPG_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(JPG_ID_BLOCK);
                toolAdm.FileType = "bmp";
            }
            else if (strVTemp.IndexOf(PNG_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(PNG_ID_BLOCK);
                toolAdm.FileType = "png";
            }
            else if (strVTemp.IndexOf(GIF_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(GIF_ID_BLOCK);
                toolAdm.FileType = "gif";
            }
            else if (strVTemp.IndexOf(TIFF_ID_BLOCK) != -1)
            {
                iPos = strVTemp.IndexOf(TIFF_ID_BLOCK);
                toolAdm.FileType = "tiff";
            }
            else if (strVTemp.IndexOf(VSD_ID_BLOCK) != -1)
            {
                toolAdm.FileType = "vsd";
                iPos = strVTemp.IndexOf(VSD_ID_BLOCK);
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

        public bool UpdateToolAdmin(ToolAdminModel tooladm)
        {

            bool _status = false;
            tooladm.Status = "";
            try
            {

                if (tooladm.Mode == OperationMode.AddNew)
                {

                    TOOL_FAMILY toolf = (from o in DB.TOOL_FAMILY
                                         where o.FAMILY_CD == tooladm.FAMILY_CD
                                         select o).FirstOrDefault<TOOL_FAMILY>();

                    if (toolf == null)
                    {
                        try
                        {
                            toolf = new TOOL_FAMILY();
                            toolf.FAMILY_CD = tooladm.FAMILY_CD;
                            toolf.FAMILY_NAME = tooladm.FAMILY_NAME;
                            toolf.TOOL_PATH = tooladm.File_Name;
                            if (tooladm.picture != null)
                            {
                                toolf.PICTURE = tooladm.picture.ToArray();
                            }

                            //toolf.ENTERED_BY = userInformation.UserName;
                            //toolf.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                            //toolf.ROWID = Guid.NewGuid();
                            DB.TOOL_FAMILY.InsertOnSubmit(toolf);
                            DB.SubmitChanges();
                            toolf = null;
                            tooladm.Status = PDMsg.SavedSuccessfully;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                            tooladm.Status = PDMsg.SavedSuccessfully;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.TOOL_FAMILY.DeleteOnSubmit(toolf);

                        }
                    }
                    else if (toolf != null)
                    {
                        try
                        {
                            toolf.FAMILY_CD = tooladm.FAMILY_CD;
                            toolf.FAMILY_NAME = tooladm.FAMILY_NAME;
                            if (tooladm.ImageChanged == true)
                            {
                                toolf.TOOL_PATH = tooladm.File_Name;
                                if (tooladm.picture != null)
                                {
                                    toolf.PICTURE = tooladm.picture.ToArray();
                                }
                            }
                            //toolf.ENTERED_BY = userInformation.UserName;
                            //toolf.ENTERED_DATE = userInformation.Dal.ServerDateTime;
                            //toolf.UPDATED_BY = null;
                            //toolf.UPDATED_DATE = null;

                            DB.SubmitChanges();
                            toolf = null;
                            tooladm.Status = PDMsg.SavedSuccessfully;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                            tooladm.Status = PDMsg.SavedSuccessfully;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.TOOL_FAMILY.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, toolf);

                        }
                    }

                    toolf = null;                  

                }
                else if (tooladm.Mode == OperationMode.Edit)
                {
                    TOOL_FAMILY toolf = (from o in DB.TOOL_FAMILY
                                         where o.FAMILY_CD == tooladm.FAMILY_CD
                                         select o).FirstOrDefault<TOOL_FAMILY>();

                    if (toolf != null)
                    {
                        try
                        {
                            if (tooladm.ImageChanged == true)
                            {
                                toolf.FAMILY_CD = tooladm.FAMILY_CD;
                                toolf.FAMILY_NAME = tooladm.FAMILY_NAME;
                                toolf.TOOL_PATH = tooladm.File_Name;
                                if (tooladm.picture != null)
                                {
                                    toolf.PICTURE = tooladm.picture.ToArray();
                                }
                                DB.SubmitChanges();
                                tooladm.Status = PDMsg.SavedSuccessfully;
                            }
                            //toolf.ENTERED_BY = userInformation.UserName;
                            //toolf.ENTERED_DATE = userInformation.Dal.ServerDateTime;
                            //toolf.UPDATED_BY = null;
                            //toolf.UPDATED_DATE = null;

                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                            tooladm.Status = PDMsg.SavedSuccessfully;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.TOOL_FAMILY.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, toolf);

                        }
                    }                    
                }

                foreach (DataRowView dr in tooladm.DVToolParameter)
                {
                    if (dr["PARAMETER_CD"].ToString() != "")
                    {

                        TOOL_PARAMETER toolp = (from o in DB.TOOL_PARAMETER
                                                where o.FAMILY_CD == tooladm.FAMILY_CD && o.PARAMETER_CD == dr["PARAMETER_CD"].ToString()
                                                select o).SingleOrDefault<TOOL_PARAMETER>();
                        if (toolp == null)
                        {
                            try
                            {
                                toolp = new TOOL_PARAMETER();
                                toolp.FAMILY_CD = tooladm.FAMILY_CD;
                                toolp.PARAMETER_CD = dr["PARAMETER_CD"].ToString();
                                toolp.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                                toolp.X_COORDINATE = dr["X_COORDINATE"].ToValueAsString().ToIntValue();
                                toolp.Y_COORDINATE = dr["Y_COORDINATE"].ToValueAsString().ToIntValue();
                                toolp.DATATYPE = dr["DATATYPE"].ToValueAsString().ToIntValue();
                                toolp.DEFAULT_VALUE = dr["DEFAULT_VALUE"].ToString();
                                toolp.DEFAULT_VISIBLE = dr["DEFAULT_VISIBLE"].ToValueAsString().ToIntValue();

                                //toolp.ENTERED_BY = userInformation.UserName;
                                //toolp.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                toolp.ROWID = Guid.NewGuid();
                                DB.TOOL_PARAMETER.InsertOnSubmit(toolp);
                                DB.SubmitChanges();
                                toolp = null;
                                //ToolAdm.Status = "Record inserted successfully.";
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.TOOL_PARAMETER.DeleteOnSubmit(toolp);

                            }
                        }
                        else if (toolp != null)
                        {
                            try
                            {
                                toolp.FAMILY_CD = tooladm.FAMILY_CD;
                                toolp.PARAMETER_CD = dr["PARAMETER_CD"].ToString();
                                toolp.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                                toolp.X_COORDINATE = dr["X_COORDINATE"].ToValueAsString().ToIntValue();
                                toolp.Y_COORDINATE = dr["Y_COORDINATE"].ToValueAsString().ToIntValue();
                                toolp.DATATYPE = dr["DATATYPE"].ToValueAsString().ToIntValue();
                                toolp.DEFAULT_VALUE = dr["DEFAULT_VALUE"].ToString();
                                toolp.DEFAULT_VISIBLE = dr["DEFAULT_VISIBLE"].ToValueAsString().ToIntValue();
                                //toolp.ENTERED_BY = userInformation.UserName;
                                //toolp.ENTERED_DATE = userInformation.Dal.ServerDateTime;
                                //toolp.UPDATED_BY = null;
                                //toolp.UPDATED_DATE = null;
                                DB.SubmitChanges();
                                toolp = null;
                                //ToolAdm.Status = "Record inserted successfully.";
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

                            }
                            catch (Exception ex)
                            {
                                DB.TOOL_PARAMETER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, toolp);
                                ex.LogException();
                            }
                        }
                        toolp = null;

                    }
                }

                DeleteParameters(tooladm.DTDeletedRecords, tooladm.FAMILY_CD);
                _status = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return _status;
        }

        public bool DeleteParameters(DataTable dvRecords, string family_cd)
        {

            bool _status = false;

            foreach (DataRow dr in dvRecords.Rows)
            {
                try
                {

                    if (family_cd.IsNotNullOrEmpty() && dr["PARAMETER_CD"].ToString() != "")
                    {
                        TOOL_PARAMETER toolp = (from o in DB.TOOL_PARAMETER
                                                where o.FAMILY_CD == family_cd && o.PARAMETER_CD == dr["PARAMETER_CD"].ToString()
                                                select o).SingleOrDefault<TOOL_PARAMETER>();

                        if (toolp != null)
                        {
                            DB.TOOL_PARAMETER.DeleteOnSubmit(toolp);
                            DB.SubmitChanges();
                        }
                        toolp = null;
                        _status = true;
                    }
                }
                catch (System.Data.Linq.ChangeConflictException)
                {
                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    _status = true;
                }
                catch (Exception ex)
                {
                    _status = false;
                    throw ex.LogException();
                }
            }
            return _status;
        }

        /*
        public void GetImage(ToolAdminModel ToolAdm)
        {

            TOOL_FAMILY ddDrawing = null;

            byte[] photosource = null;
            System.IO.MemoryStream strm;
            try
            {
                ddDrawing = (from c in DB.TOOL_FAMILY
                             where c.FAMILY_CD == "0002"
                             select c).SingleOrDefault<TOOL_FAMILY>();
                if (ddDrawing.IsNotNullOrEmpty())
                {
                    if (ddDrawing.PICTURE != null)
                    {
                        photosource = ddDrawing.PICTURE.ToArray();
                        var str = getMimeFromFile(photosource);
                        strm = new System.IO.MemoryStream();
                        strm.Write(photosource, 0, photosource.Length);
                        ToolAdm.picture = strm;
                    }
                    else
                    {
                        ToolAdm.picture = null;
                    }
                }
                else
                {
                    ToolAdm.picture = null;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool SaveToolImage(ToolAdminModel toolAdmin)
        {
            ///SaveDrawingSQL(costcentermaster);
            //return true;
            TOOL_FAMILY ddDrawing = null;
            try
            {
                ddDrawing = (from c in DB.TOOL_FAMILY 
                             where c.FAMILY_CD == toolAdmin.FAMILY_CD
                             select c).SingleOrDefault<TOOL_FAMILY>();
                if (ddDrawing.IsNotNullOrEmpty())
                {
                    if (toolAdmin.picture != null)
                    {
                        ddDrawing.FAMILY_CD = toolAdmin.FAMILY_CD;
                        ddDrawing.FAMILY_NAME = toolAdmin.FAMILY_NAME;
                        ddDrawing.PICTURE= toolAdmin.picture.ToArray();
                        ddDrawing.TOOL_PATH= toolAdmin.File_Name;
                    }
                }
                else
                {
                    ddDrawing = new TOOL_FAMILY();
                    ddDrawing.FAMILY_CD = toolAdmin.FAMILY_CD;
                    ddDrawing.FAMILY_NAME = toolAdmin.FAMILY_NAME;
                    ddDrawing.TOOL_PATH = toolAdmin.File_Name;
                    ddDrawing.PICTURE = toolAdmin.picture.ToArray();
                    DB.TOOL_FAMILY.InsertOnSubmit(ddDrawing);
                }
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
         */

        public int CheckDuplicateCode(string code)
        {
            int count = 0;
            try
            {
                //DDCOST_CENT_MAST costcentmast = DB.DDCOST_CENT_MAST.Where(c => c.COST_CENT_CODE == code).First();
                List<TOOL_FAMILY> lstfamily = (from c in DB.TOOL_FAMILY
                                               where c.FAMILY_CD.ToUpper() == code.ToUpper()
                                               orderby c.FAMILY_CD ascending
                                               select c).ToList<TOOL_FAMILY>();

                count = lstfamily.Count;
                return count;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return 0;
        }

        public int CheckDuplicateFamilyName(string familyname)
        {
            int count = 0;
            try
            {
                //DDCOST_CENT_MAST costcentmast = DB.DDCOST_CENT_MAST.Where(c => c.COST_CENT_CODE == code).First();
                List<TOOL_FAMILY> lstfamily = (from c in DB.TOOL_FAMILY
                                               where c.FAMILY_NAME.ToUpper().Trim() == familyname.ToUpper().Trim()
                                               orderby c.FAMILY_CD ascending
                                               select c).ToList<TOOL_FAMILY>();

                count = lstfamily.Count;
                return count;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return 0;
        }


        public bool CheckDuplicate(DataTable data, string columnname)
        {
            DataRow[] drRow;
            int ctr;
            string filter;
            try
            {
                for (ctr = 0; ctr <= data.Rows.Count - 1; ctr++)
                {
                    filter = "" + columnname + "='" + data.Rows[ctr][columnname.ToString().ToUpper()].ToString().Trim() + "'" + "";
                    drRow = data.Select(filter);
                    if (drRow.Length > 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


    }
}
