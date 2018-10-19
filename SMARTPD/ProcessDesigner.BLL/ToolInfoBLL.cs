using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using ProcessDesigner.Model;
using ProcessDesigner;
using System.IO;
using System.Runtime.InteropServices;
using ProcessDesigner.Common;
using System.IO;
using System.Reflection;

namespace ProcessDesigner.BLL
{
    public class ToolInfoBLL : Essential
    {
        public ToolInfoBLL(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;

        }

        //public DataView FilterDataDatable()
        //{
        //     DataTable  dataValue;
        //  dataValue = ToDataTable((from c in DB.TOOL_FAMILY.AsEnumerable()
        //                              where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
        //                             orderby c.FAMILY_CD ascending
        //                             ).ToList());

        //    return dataValue.DefaultView;
        //}


        public DataView GetSearchCriteria()
        {
            DataTable dataValue;
            dataValue = ToDataTable((from c in DB.TOOL_FAMILY.AsEnumerable()
                                     // where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                     orderby c.FAMILY_CD ascending
                                     select new { c.FAMILY_CD, c.FAMILY_NAME, }).ToList());

            return dataValue.DefaultView;
        }

        public void GetFamilyDrawings(string familycd, ToolAdminModel toolAdm)
        {

            //ToolAdminModel toolAdm = new ToolAdminModel();
            toolAdm.FAMILY_CD = familycd;
            ToolAdminBll toolAdminBll = new ToolAdminBll(this.userInformation);


            DataTable dt = new DataTable();
            dt = ToDataTable((from o in DB.TOOL_FAMILY
                              where o.FAMILY_CD == familycd
                              select new { o.PICTURE }).ToList());
            if (dt != null)
            {
                //  tooladm.DVPicture = dt.DefaultView;
                GetToolImage(toolAdm);


            }
            else
            {
                // tooladm.DVPicture = null;
            }



            // toolAdminBll.GetToolParameter(toolAdm);

            // return dataValue.DefaultView;
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


        public void GetToolImage(ToolAdminModel toolAdm)
        {

            int offset = 0;
            TOOL_FAMILY toolfamily = new TOOL_FAMILY();
            toolfamily = null;
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


        public DataView GetFamilyCode()
        {
            DataTable dataValue;
            dataValue = ToDataTable((from c in DB.TOOL_FAMILY.AsEnumerable()
                                     // where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                     orderby c.FAMILY_CD ascending
                                     select new { c.FAMILY_CD, c.FAMILY_NAME, }).ToList());

            return dataValue.DefaultView;
        }

        public DataView GetParameterCode(string familycd)
        {
            DataTable dataValue;
            dataValue = ToDataTable((from c in DB.TOOL_PARAMETER.AsEnumerable()
                                     where c.FAMILY_CD == familycd
                                     orderby c.PARAMETER_CD ascending
                                     select new { c.FAMILY_CD, c.PARAMETER_CD, c.PARAMETER_NAME, c.DATATYPE }).ToList());


            return dataValue.DefaultView;
        }

        //public string buildQuery()
        //{
        //    foreach (DataRow  row in DtParaMeterData.ToTable().Rows)
        //            {
        //}

        public bool CheckToolCodeEsxists(string toolcode)
        {
            try
            {

                TOOL_DIMENSION ddOperMast = (from c in DB.TOOL_DIMENSION
                                             where c.TOOL_CD == toolcode
                                             //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             select c).SingleOrDefault<TOOL_DIMENSION>();
                if (ddOperMast != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                ex.LogException();
                // throw ex.LogException();
                return false;
            }
        }

        public DataView GetFamilyGridToolCode(string familyCode, string tool_code)
        {
            try
            {
                TollInfoModel toolmodel = new TollInfoModel();

                string getQuery = "select * FROM  TOOL_DIMENSION WHERE FAMILY_CD = '" + familyCode + "' and TOOL_CD = '" + tool_code + "'";


                return ToDataTable(DB.ExecuteQuery<TollInfoModel>(getQuery).ToList()).DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        public DataView GetFamilyGridData(DataTable querytable, string familycd)
        {
            try
            {
                StringBuilder strQueryBuilder = new StringBuilder();
                string strQuery = string.Empty;

                foreach (DataRow row in querytable.Rows)
                {
                    // strQueryBuilder = strQueryBuilder.Append(row[2].ToString().ToUpper() + "= c." + row[1].ToString().ToUpper() + ", ");

                    if (row[1].ToString().ToUpper() != "FAMILY_CD")
                    {
                        strQueryBuilder = strQueryBuilder.Append(row[1].ToString().ToUpper() + ", ");
                    }


                }

                strQuery = strQueryBuilder.ToString();
                strQuery = strQuery.Substring(0, strQuery.Length - 2);

                DataView dtCIReference;
                //  dtCIReference =new DataView();
                TollInfoModel toolmodel = new TollInfoModel();



                //DataTable dataValue;
                //dtCIReference = ToDataTable((from c in DB.TOOL_PARAMETER.AsEnumerable()
                //                         where c.FAMILY_CD == FamilyCD
                //                         orderby c.PARAMETER_CD ascending
                //                             select new { c.PARAMETER_CD, strQuery }).ToList());



                //    return dtCIReference.DefaultView;
                string getQuery = "select TOOL_CD , " + strQuery + " FROM  TOOL_DIMENSION WHERE FAMILY_CD = '" + familycd + "'";

                //string getQuery = "select FAMILY_CD,TOOL_CD FROM  TOOL_DIMENSION WHERE FAMILY_CD = '" + FamilyCD + "'";

                return dtCIReference = ToDataTable(DB.ExecuteQuery<TollInfoModel>(getQuery).ToList()).DefaultView;
                //  var ci = ToDataTable(DB.ExecuteQuery<TollInfoModel>(getQuery).ToList()).DefaultView;

                //   ci.DeFa
                //  return null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public DataView GetFamilyData(string familycode, string sqlParameter, string orderbyfield = "")
        {
            DataView lstToolDimension;
            StringBuilder sbsql = new StringBuilder();
            try
            {
                string datatypefield = "";
                sbsql.Append("select *,0 as FINALFIELD FROM  TOOL_DIMENSION WHERE FAMILY_CD = '" + familycode + "' ");
                if (sqlParameter.ToValueAsString() != "")
                {
                    sbsql.Append(" and " + sqlParameter);
                }
                if (orderbyfield.ToValueAsString() != "")
                {
                    List<TOOL_PARAMETER> lstparameter = (from c in DB.TOOL_PARAMETER
                                                         where c.PARAMETER_CD == orderbyfield && c.FAMILY_CD.ToUpper() == familycode.ToUpper()
                                                         //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                         select c).ToList<TOOL_PARAMETER>();
                    if (lstparameter.Count > 0)
                    {
                        datatypefield = lstparameter[0].DATATYPE.ToValueAsString();
                        if (datatypefield == "1")
                        {
                            StringBuilder sbsql1 = new StringBuilder();
                            sbsql = new StringBuilder();
                            sbsql1.Append("select *,(case when isnumeric(" + orderbyfield + ") = 1 then  cast(left(" + orderbyfield + ",28) as decimal(38,10)) else (case when " + orderbyfield + " is null or  ltrim(" + orderbyfield + ")='' then -99999999999999999 else 99999999999 end) end) as FINALFIELD FROM  TOOL_DIMENSION WHERE FAMILY_CD = '" + familycode + "' ");
                            if (sqlParameter.ToValueAsString() != "")
                            {
                                sbsql1.Append(" and " + sqlParameter);
                            }
                            sbsql.Append("select * from (" + sbsql1.ToString() + ") A order by FINALFIELD ");
                        }
                        else
                        {
                            sbsql.Append(" ORDER BY " + orderbyfield);
                        }
                    }
                    else
                    {
                        sbsql.Append(" ORDER BY " + orderbyfield);
                    }
                }
                var collection = ToDataTableWithType(DB.ExecuteQuery<TollInfoModel>(sbsql.ToValueAsString().Trim()).ToList());
                return collection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool Save(DataTable dtSave, string familyCode)
        {
            bool blnAdd = false;
            List<TOOL_DIMENSION> lstDimension = (from c in DB.TOOL_DIMENSION
                                                 where c.FAMILY_CD == familyCode
                                                 //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                 select c).ToList<TOOL_DIMENSION>();
            TOOL_DIMENSION updateDimension = null;
            try
            {

                foreach (DataRow drRow in dtSave.Rows)
                {
                    blnAdd = false;
                    updateDimension = (from c in lstDimension
                                       where c.TOOL_CD.ToUpper().Trim() == drRow["TOOL_CD"].ToValueAsString().Trim().ToUpper()
                                       //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                       select c).FirstOrDefault<TOOL_DIMENSION>();
                    if (updateDimension == null)
                    {
                        updateDimension = new TOOL_DIMENSION();
                        blnAdd = true;
                        updateDimension.ROWID = Guid.NewGuid();
                    }
                    updateDimension.FAMILY_CD = familyCode;
                    updateDimension.DESCRIPTION = drRow["DESCRIPTION"].ToValueAsString().Trim();
                    updateDimension.TOOL_CD = drRow["TOOL_CD"].ToValueAsString().Trim();
                    updateDimension.P001 = drRow["P001"].ToValueAsString().Trim();
                    updateDimension.P002 = drRow["P002"].ToValueAsString().Trim();
                    updateDimension.P003 = drRow["P003"].ToValueAsString().Trim();
                    updateDimension.P004 = drRow["P004"].ToValueAsString().Trim();
                    updateDimension.P005 = drRow["P005"].ToValueAsString().Trim();
                    updateDimension.P006 = drRow["P006"].ToValueAsString().Trim();
                    updateDimension.P007 = drRow["P007"].ToValueAsString().Trim();
                    updateDimension.P008 = drRow["P008"].ToValueAsString().Trim();
                    updateDimension.P009 = drRow["P009"].ToValueAsString().Trim();
                    updateDimension.P010 = drRow["P010"].ToValueAsString().Trim();
                    updateDimension.P011 = drRow["P011"].ToValueAsString().Trim();
                    updateDimension.P012 = drRow["P012"].ToValueAsString().Trim();
                    updateDimension.P013 = drRow["P013"].ToValueAsString().Trim();
                    updateDimension.P014 = drRow["P014"].ToValueAsString().Trim();
                    updateDimension.P015 = drRow["P015"].ToValueAsString().Trim();
                    updateDimension.P016 = drRow["P016"].ToValueAsString().Trim();
                    updateDimension.P017 = drRow["P017"].ToValueAsString().Trim();
                    updateDimension.P018 = drRow["P018"].ToValueAsString().Trim();
                    updateDimension.P019 = drRow["P019"].ToValueAsString().Trim();
                    updateDimension.P020 = drRow["P020"].ToValueAsString().Trim();
                    updateDimension.P021 = drRow["P021"].ToValueAsString().Trim();
                    updateDimension.P022 = drRow["P022"].ToValueAsString().Trim();
                    updateDimension.P023 = drRow["P023"].ToValueAsString().Trim();
                    updateDimension.P024 = drRow["P024"].ToValueAsString().Trim();
                    updateDimension.P025 = drRow["P025"].ToValueAsString().Trim();
                    updateDimension.P026 = drRow["P026"].ToValueAsString().Trim();
                    updateDimension.P027 = drRow["P027"].ToValueAsString().Trim();
                    updateDimension.P028 = drRow["P028"].ToValueAsString().Trim();
                    updateDimension.P029 = drRow["P029"].ToValueAsString().Trim();
                    updateDimension.P030 = drRow["P030"].ToValueAsString().Trim();
                    updateDimension.P031 = drRow["P031"].ToValueAsString().Trim();
                    updateDimension.P032 = drRow["P032"].ToValueAsString().Trim();
                    updateDimension.P033 = drRow["P033"].ToValueAsString().Trim();
                    updateDimension.P034 = drRow["P034"].ToValueAsString().Trim();
                    updateDimension.P035 = drRow["P035"].ToValueAsString().Trim();
                    updateDimension.P036 = drRow["P036"].ToValueAsString().Trim();
                    updateDimension.P037 = drRow["P037"].ToValueAsString().Trim();
                    updateDimension.P038 = drRow["P038"].ToValueAsString().Trim();
                    updateDimension.P039 = drRow["P039"].ToValueAsString().Trim();
                    updateDimension.P040 = drRow["P040"].ToValueAsString().Trim();
                    updateDimension.P041 = drRow["P041"].ToValueAsString().Trim();
                    updateDimension.P042 = drRow["P042"].ToValueAsString().Trim();
                    updateDimension.P043 = drRow["P043"].ToValueAsString().Trim();
                    updateDimension.P044 = drRow["P044"].ToValueAsString().Trim();
                    updateDimension.P045 = drRow["P045"].ToValueAsString().Trim();
                    updateDimension.P046 = drRow["P046"].ToValueAsString().Trim();
                    updateDimension.P047 = drRow["P047"].ToValueAsString().Trim();
                    updateDimension.P048 = drRow["P048"].ToValueAsString().Trim();
                    updateDimension.P049 = drRow["P049"].ToValueAsString().Trim();
                    updateDimension.P050 = drRow["P050"].ToValueAsString().Trim();
                    updateDimension.S001 = drRow["S001"].ToValueAsString().Trim();
                    updateDimension.S002 = drRow["S002"].ToValueAsString().Trim();
                    updateDimension.S003 = drRow["S003"].ToValueAsString().Trim();
                    updateDimension.S004 = drRow["S004"].ToValueAsString().Trim();
                    updateDimension.S005 = drRow["S005"].ToValueAsString().Trim();
                    updateDimension.S006 = drRow["S006"].ToValueAsString().Trim();
                    updateDimension.S007 = drRow["S007"].ToValueAsString().Trim();
                    updateDimension.S008 = drRow["S008"].ToValueAsString().Trim();
                    updateDimension.S009 = drRow["S009"].ToValueAsString().Trim();
                    updateDimension.S010 = drRow["S010"].ToValueAsString().Trim();
                    if (blnAdd == true)
                    {
                        DB.TOOL_DIMENSION.InsertOnSubmit(updateDimension);
                    }
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool DeleteToolInfo(string family_cd, string tool_cd)
        {
            TOOL_DIMENSION toolDimension = null;
            try
            {
                toolDimension = (from c in DB.TOOL_DIMENSION
                                 where c.FAMILY_CD == family_cd && c.TOOL_CD == tool_cd
                                 select c).FirstOrDefault<TOOL_DIMENSION>();
                if (toolDimension != null)
                {
                    DB.TOOL_DIMENSION.DeleteOnSubmit(toolDimension);
                    DB.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private T GetItem<T>(DataRow dr)
        {
            try
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}

