using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
//using System.Runtime.InteropServices;

namespace ProcessDesigner.BLL
{
    public class CostCenterMasterDet : Essential
    {

        public CostCenterMasterDet(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCombo()
        {
            DataTable dttable;
            dttable = new DataTable();
            DataSet dsMaster = new DataSet();
            List<StringBuilder> sbSQL = new List<StringBuilder>();

            try
            {
                /*
                var lstcost1 = (from c in DB.DDCATE_MAST
                                orderby c.CATE_CODE ascending
                                select c);

                */

                dttable = ToDataTable((from c in DB.DDCATE_MAST.AsEnumerable()
                                       where c.CATE_CODE.ToValueAsString() != ""
                                       && c.CATEGORY.ToValueAsString().Trim() != ""
                                       orderby c.CATE_CODE ascending
                                       select new { c.CATE_CODE, c.CATEGORY }).ToList());
                dsMaster.Tables.Add(dttable.Copy());

                dttable = ToDataTable((from c in DB.DDLOC_MAST.AsEnumerable()
                                       where (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                                       orderby c.LOC_CODE ascending
                                       select new { c.LOC_CODE, c.LOCATION }).ToList());
                dsMaster.Tables.Add(dttable.Copy());

                dttable = ToDataTable((from c in DB.DDMODULE_MAST.AsEnumerable()
                                       where c.MODULE_CODE.ToValueAsString() != ""
                                       && c.MODULE_NAME.ToValueAsString() != ""
                                       orderby c.MODULE_CODE ascending
                                       select new { c.MODULE_CODE, c.MODULE_NAME }).ToList());
                dsMaster.Tables.Add(dttable.Copy());

                dttable = ToDataTable((from c in DB.DDCOST_CENT_MAST.AsEnumerable()
                                       where c.COST_CENT_CODE != null & c.COST_CENT_CODE.Trim() != "" && (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                                       orderby c.COST_CENT_CODE
                                       select new { c.COST_CENT_CODE, c.COST_CENT_DESC, c.LOC_CODE, c.EFFICIENCY, OPER_CODE_SORT = c.COST_CENT_CODE.ToDecimalValue() }).ToList());
                if (dttable.Rows.Count > 0)
                    dttable.DefaultView.Sort = "OPER_CODE_SORT asc";
                dsMaster.Tables.Add(dttable.Copy());

                dttable = ToDataTable((from c in DB.DDOPER_MAST.AsEnumerable()
                                       where c.OPER_CODE.ToValueAsString().Trim() != ""
                                       && c.OPER_DESC.ToValueAsString().Trim() != ""
                                        && (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                                       select new { OPN_CODE = c.OPER_CODE, c.OPER_DESC }).ToList());
                dsMaster.Tables.Add(dttable.Copy());

                dttable = ToDataTable((from c in DB.DDUNIT_MAST.AsEnumerable()
                                       where c.UNIT_CODE.ToValueAsString().Trim() != ""
                                       && c.UNIT_OF_MEAS.ToValueAsString().Trim() != "" && (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                                       select new { c.UNIT_CODE, c.UNIT_OF_MEAS }).ToList());
                dsMaster.Tables.Add(dttable.Copy());

                //sbSQL.Add(new StringBuilder("SELECT CATE_CODE,CATEGORY FROM DDCATE_MAST ORDER BY CATE_CODE")); // Category
                //sbSQL.Add(new StringBuilder("SELECT LOC_CODE,LOCATION FROM DDLOC_MAST"));   // Location 
                //sbSQL.Add(new StringBuilder("SELECT MODULE_CODE,MODULE_NAME FROM DDMODULE_MAST")); //Module
                //sbSQL.Add(new StringBuilder("SELECT COST_CENT_CODE,COST_CENT_DESC,LOC_CODE,EFFICIENCY FROM DDCOST_CENT_MAST")); //ddcost_cent_mast
                //sbSQL.Add(new StringBuilder("SELECT OPER_CODE AS OPN_CODE,OPER_DESC FROM  DDOPER_MAST")); //ddoper_mast
                //sbSQL.Add(new StringBuilder("SELECT UNIT_CODE,UNIT_OF_MEAS FROM  DDUNIT_MAST")); //Unit
                //dsMaster = Dal.GetDataSet(sbSQL, null);
                return dsMaster;
                //dsMaster=dalConnect.GetDataTable(sbSQL,)
            }
            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="cost_cent_code"></param>
        /// <returns></returns>
        public DataSet GetGridDetails(string cost_cent_code)
        {
            DataSet dsMaster = new DataSet();
            DataTable dtdatatable;
            List<StringBuilder> sbSQL = new List<StringBuilder>();
            try
            {

                dtdatatable = ToDataTable((from c in DB.DDCC_OUTPUT.AsEnumerable()
                                           where c.COST_CENT_CODE == cost_cent_code
                                           orderby c.IDPK
                                           select c).ToList());
                dsMaster.Tables.Add(dtdatatable.Copy());
                dsMaster.Tables[dsMaster.Tables.Count - 1].Namespace = "DDCC_OUTPUT";

                dtdatatable = ToDataTable((from c in DB.DDCC_OPER.AsEnumerable()
                                           where c.COST_CENT_CODE == cost_cent_code
                                           orderby c.IDPK
                                           select new { c.OPN_CODE, c.UNIT_CODE, c.FIX_COST, c.VAR_COST }).ToList());
                dsMaster.Tables.Add(dtdatatable.Copy());
                dsMaster.Tables[dsMaster.Tables.Count - 1].Namespace = "DDCC_OPER";

                /*
                sbSQL.Add(new StringBuilder("SELECT OUTPUT FROM DBO.DDCC_OUTPUT WHERE COST_CENT_CODE = " + ToDbParameter("pi_cost_cent_code"))); //output
                lstDbParameter.Clear();
                lstDbParameter.Add(DBParameter("pi_cost_cent_code", System.Data.DbType.String, cost_cent_code.ToValueAsString()));
                dtMaster = Dal.GetDataSet(sbSQL, lstDbParameter).Tables[0];
                dsMaster.Tables.Add(dtMaster.Copy());
                dsMaster.Tables[dsMaster.Tables.Count - 1].Namespace = "DDCC_OUTPUT";

                sbSQL = new List<StringBuilder>();
                sbSQL.Add(new StringBuilder("SELECT OPN_CODE  ,UNIT_CODE,FIX_COST,VAR_COST FROM DBO.DDCC_OPER WHERE COST_CENT_CODE=" + ToDbParameter("pi_cost_cent_code"))); //output
                lstDbParameter.Clear();
                lstDbParameter.Add(DBParameter("pi_cost_cent_code", cost_cent_code.ToValueAsString()));
                dtMaster = Dal.GetDataSet(sbSQL, lstDbParameter).Tables[0];
                dsMaster.Tables.Add(dtMaster.Copy());
                dsMaster.Tables[dsMaster.Tables.Count - 1].Namespace = "DDCC_OPER";
                */

                return dsMaster;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public string UpdateCCMasterVariableFixedCost(ref DataTable dtErrTable, string costCentCode, decimal opnCode, decimal varCost, decimal fixCost)
        {
            DDCC_OPER ccOper = null;
            if (!costCentCode.IsNotNullOrEmpty() & opnCode == 0)
            {
                dtErrTable.Rows.Add(costCentCode, opnCode);
                return "Blank Cost Centre & Operation Code";
            }
            else if (!costCentCode.IsNotNullOrEmpty())
            {
                dtErrTable.Rows.Add(costCentCode, opnCode);
                return "Blank Cost Centre";
            }
            else if (opnCode == 0)
            {
                dtErrTable.Rows.Add(costCentCode, opnCode);
                return "Blank Operation Code";
            }
            else if (varCost == 0 && fixCost == 0)
            {
                dtErrTable.Rows.Add(costCentCode, opnCode);
                return "Blank Fixed/Variable Cost";
            }
            try
            {
                ccOper = (from c in DB.DDCC_OPER
                          where c.COST_CENT_CODE == costCentCode
                          select c).FirstOrDefault<DDCC_OPER>();
                if (!ccOper.IsNotNullOrEmpty())
                {
                    dtErrTable.Rows.Add(costCentCode, opnCode);
                    return "Invalid Cost Centre";
                }
            }
            catch (Exception)
            {

            }
            try
            {
                ccOper = (from c in DB.DDCC_OPER
                          where c.OPN_CODE == opnCode
                          select c).FirstOrDefault<DDCC_OPER>();
                if (!ccOper.IsNotNullOrEmpty())
                {
                    dtErrTable.Rows.Add(costCentCode, opnCode);
                    return "Invalid Operation Code";
                }
            }
            catch (Exception)
            {

            }

            ccOper = null;
            ccOper = (from c in DB.DDCC_OPER
                      where c.COST_CENT_CODE == costCentCode && c.OPN_CODE == opnCode
                      select c).FirstOrDefault<DDCC_OPER>();
            try
            {
                if (ccOper.IsNotNullOrEmpty())
                {
                    ccOper.VAR_COST = varCost;
                    ccOper.FIX_COST = fixCost;
                    DB.SubmitChanges();
                    return "Cost updated Successfully";
                }
                else
                {
                    dtErrTable.Rows.Add(costCentCode, opnCode);
                    return "Invalid Cost Centre & Operation Code";
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.DDCC_OPER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ccOper);
                dtErrTable.Rows.Add(costCentCode, opnCode);
                return "Invalid Data. Refer path for details";
            }
            dtErrTable.Rows.Add(costCentCode, opnCode);
            return "Invalid Data. Refer path for details";
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="costCenterMaster"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool SaveCostCenterMaster(CostCenterMaster costCenterMaster, OperationMode opermode)
        {
            DDCOST_CENT_MAST ddCC = null;
            bool insert = false;
            bool update = false;
            bool submit = false;
            try
            {
                ddCC = new DDCOST_CENT_MAST();

                if (opermode == OperationMode.AddNew)
                {
                    ddCC = new DDCOST_CENT_MAST();
                }
                else
                {
                    ddCC = (from c in DB.DDCOST_CENT_MAST
                            where c.COST_CENT_CODE == costCenterMaster.CostCentCode
                            select c).Single<DDCOST_CENT_MAST>();
                }
                ddCC.COST_CENT_CODE = costCenterMaster.CostCentCode;
                ddCC.COST_CENT_DESC = costCenterMaster.CostCentDesc;
                ddCC.CATE_CODE = costCenterMaster.CateCode;
                ddCC.LOC_CODE = costCenterMaster.LocCode;
                ddCC.MODULE = costCenterMaster.Module;
                ddCC.EFFICIENCY = Convert.ToDecimal(costCenterMaster.Efficiency);
                ddCC.MACHINE_NAME = costCenterMaster.MachineName;
                ddCC.DELETE_FLAG = false;

                if (opermode == OperationMode.AddNew)
                {
                    ddCC.ENTERED_BY = userInformation.UserName;
                    ddCC.ENTERED_DATE = serverDateTime;
                    insert = true;
                    DB.DDCOST_CENT_MAST.InsertOnSubmit(ddCC);
                }
                else
                {
                    ddCC.UPDATED_BY = userInformation.UserName;
                    ddCC.UPDATED_DATE = serverDateTime;
                    update = true;
                }
                submit = true;
                DB.SubmitChanges();
                SaveCostDetails(costCenterMaster);
                return true;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (update == true)
                    {
                        DB.DDCOST_CENT_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddCC);
                    }
                    if (insert == true)
                    {
                        DB.DDCOST_CENT_MAST.DeleteOnSubmit(ddCC);
                    }
                }
                throw ex.LogException();
            }
        }

        public int CheckDuplicateCode(string code)
        {
            int count = 0;
            try
            {
                //DDCOST_CENT_MAST costcentmast = DB.DDCOST_CENT_MAST.Where(c => c.COST_CENT_CODE == code).First();
                List<DDCOST_CENT_MAST> lstcost = (from c in DB.DDCOST_CENT_MAST
                                                  where c.COST_CENT_CODE == code
                                                  orderby c.COST_CENT_CODE ascending
                                                  select c).ToList<DDCOST_CENT_MAST>();


                count = lstcost.Count;
                return count;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                throw ex.LogException();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return 0;
        }

        public int CheckDuplicateCode1(string code)
        {
            int count = 0;
            try
            {
                //DDCOST_CENT_MAST costcentmast = DB.DDCOST_CENT_MAST.Where(c => c.COST_CENT_CODE == code).First();
                List<DDCOST_CENT_MAST> lstcost = (from c in DB.DDCOST_CENT_MAST
                                                  where c.COST_CENT_CODE == code
                                                  orderby c.COST_CENT_CODE ascending
                                                  select c).ToList<DDCOST_CENT_MAST>();


                count = lstcost.Count;
                return count;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                throw ex.LogException();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return 0;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="costCenterMaster"></param>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        public bool GetCostCenterMaster(ref CostCenterMaster costCenterMaster)
        {
            try
            {
                string code;
                DDCOST_CENT_MAST ddCC;
                code = costCenterMaster.CostCentCode;
                /*
                costCenterMaster=(from c in DB.DDCOST_CENT_MAST
                                    where c.COST_CENT_CODE == code
                                    select costCenterMaster
                                    {
                                        CostCentCode = c.COST_CENT_CODE,
                                        CostCentDesc = c.COST_CENT_DESC,
                                        CateCode = c.CATE_CODE,
                                        LocCode = c.LOC_CODE,
                                        Module = c.MODULE,
                                        Efficiency = Convert.ToDouble(c.EFFICIENCY),
                                        MachineName = c.MACHINE_NAME
                                    }).SingleOrDefault<CostCenterMaster>();
                */
                ddCC = (from c in DB.DDCOST_CENT_MAST
                        where c.COST_CENT_CODE == code
                        select c).SingleOrDefault();


                if (costCenterMaster.IsNotNullOrEmpty() == false)
                {
                    costCenterMaster = new CostCenterMaster();
                }

                if (ddCC.IsNotNullOrEmpty())
                {
                    costCenterMaster.CostCentCode = ddCC.COST_CENT_CODE;
                    costCenterMaster.CostCentDesc = ddCC.COST_CENT_DESC;
                    costCenterMaster.CateCode = ddCC.CATE_CODE;
                    costCenterMaster.LocCode = ddCC.LOC_CODE;
                    costCenterMaster.Module = ddCC.MODULE;

                    if (ddCC.EFFICIENCY == null)
                    {
                        costCenterMaster.Efficiency = 0;
                    }
                    else
                    {
                        costCenterMaster.Efficiency = Convert.ToDouble(ddCC.EFFICIENCY >= 100 ? 0 : ddCC.EFFICIENCY);
                    }
                    costCenterMaster.MachineName = ddCC.MACHINE_NAME;
                    costCenterMaster.FilePath = "";
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool DeleteOutPut(string costcentcode)
        {
            try
            {
                var ddoutputs =
                        from details in DB.DDCC_OUTPUT
                        where details.COST_CENT_CODE == costcentcode
                        select details;
                DB.DDCC_OUTPUT.DeleteAllOnSubmit(ddoutputs);
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return false;
                throw ex.LogException();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private bool DeleteOperations(string costcentcode)
        {
            try
            {
                var ddopers =
                        from details in DB.DDCC_OPER
                        where details.COST_CENT_CODE == costcentcode
                        select details;
                DB.DDCC_OPER.DeleteAllOnSubmit(ddopers);
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return false;
                throw ex.LogException();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool SaveCostDetails(CostCenterMaster costcentermaster)
        {
            try
            {
                DeleteOutPut(costcentermaster.CostCentCode);
                DeleteOperations(costcentermaster.CostCentCode);
                //SaveOutput(costcentermaster);
                SaveOperations(costcentermaster);
                SaveOutPut(costcentermaster);
                if (costcentermaster.PhotoChanged == true)
                {
                    SaveDrawing(costcentermaster);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool SaveOperations(CostCenterMaster costcentermaster)
        {
            EnumerableRowCollection<DDCC_OPER> ddoperations = null;
            bool submit = false;
            try
            {
                ddoperations = from row in costcentermaster.Operation.AsEnumerable()
                               select new DDCC_OPER
                               {
                                   COST_CENT_CODE = costcentermaster.CostCentCode,
                                   OPN_CODE = row["opn_code"].ToString().ToDecimalValue(),
                                   UNIT_CODE = row["unit_code"].ToString(),
                                   FIX_COST = row["fix_cost"].ToString().ToDecimalValue(),
                                   VAR_COST = row["var_cost"].ToString().ToDecimalValue(),
                               };
                foreach (DDCC_OPER ddoper in ddoperations)
                {
                    if (ddoper.OPN_CODE.ToValueAsString() != "" && ddoper.UNIT_CODE.ToValueAsString() != "")
                    {
                        ddoper.ROWID = Guid.NewGuid();
                        DB.DDCC_OPER.InsertOnSubmit(ddoper);
                    }
                }
                submit = true;
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                ex.LogException();
                return false;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    DB.DDCC_OPER.DeleteAllOnSubmit(ddoperations);
                }
                throw ex.LogException();
            }
        }

        private bool SaveOutPut(CostCenterMaster costcentermaster)
        {
            EnumerableRowCollection<DDCC_OUTPUT> ddoutputs = null;
            bool submit = false;
            try
            {
                ddoutputs = from row in costcentermaster.Output.AsEnumerable()
                            select new DDCC_OUTPUT
                            {
                                COST_CENT_CODE = costcentermaster.CostCentCode,
                                OUTPUT = row["output"].ToString().ToDecimalValue()
                            };
                foreach (DDCC_OUTPUT ddout in ddoutputs)
                {
                    if (ddout.OUTPUT.ToValueAsString() != "")
                    {
                        ddout.ROWID = Guid.NewGuid();
                        DB.DDCC_OUTPUT.InsertOnSubmit(ddout);
                    }
                }
                submit = true;
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                ex.LogException();
                return false;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    DB.DDCC_OUTPUT.DeleteAllOnSubmit(ddoutputs);
                }
                throw ex.LogException();
            }
        }

        public System.IO.MemoryStream ShowImage(string costcode, int offset, ref string filepath)
        {

            DDCOST_CENT_DWG ddDrawing = null;

            byte[] photosource = null;
            System.IO.MemoryStream strm;
            try
            {
                ddDrawing = (from c in DB.DDCOST_CENT_DWG
                             where c.COST_CENT_CODE == costcode
                             select c).SingleOrDefault<DDCOST_CENT_DWG>();
                if (ddDrawing.IsNotNullOrEmpty())
                {
                    if (ddDrawing.PHOTO != null)
                    {
                        photosource = ddDrawing.PHOTO.ToArray();
                        strm = new System.IO.MemoryStream();
                        strm.Write(photosource, offset, photosource.Length - offset);
                        filepath = ddDrawing.FILE_PATH;
                        return strm;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                ex.LogException();
                return null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        ////[DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        ////private extern static System.UInt32 FindMimeFromData(
        ////    System.UInt32 pBC,
        ////    [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
        ////    [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
        ////    System.UInt32 cbSize,
        ////    [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
        ////    System.UInt32 dwMimeFlags,
        ////    out System.UInt32 ppwzMimeOut,
        ////    System.UInt32 dwReserverd
        ////);


        ////public string getMimeFromFile(byte[] byteArray)
        ////{

        ////    byte[] buffer = new byte[256];
        ////    using (System.IO.MemoryStream fs = new System.IO.MemoryStream(byteArray))
        ////    {
        ////        if (fs.Length >= 256)
        ////            fs.Read(buffer, 0, 256);
        ////        else
        ////            fs.Read(buffer, 0, (int)fs.Length);
        ////    }
        ////    try
        ////    {
        ////        System.UInt32 mimetype;
        ////        FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
        ////        System.IntPtr mimeTypePtr = new IntPtr(mimetype);
        ////        string mime = Marshal.PtrToStringUni(mimeTypePtr);
        ////        Marshal.FreeCoTaskMem(mimeTypePtr);
        ////        return mime;
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        return e.Message;
        ////    }
        ////}


        private bool SaveDrawing(CostCenterMaster costcentermaster)
        {
            ///SaveDrawingSQL(costcentermaster);
            //return true;
            DDCOST_CENT_DWG ddDrawing = null;
            bool submit = false;
            bool insert = false;
            bool update = false;
            try
            {
                ddDrawing = (from c in DB.DDCOST_CENT_DWG
                             where c.COST_CENT_CODE == costcentermaster.CostCentCode
                             select c).SingleOrDefault<DDCOST_CENT_DWG>();
                if (ddDrawing.IsNotNullOrEmpty())
                {
                    ddDrawing.UPDATED_BY = userInformation.UserName;
                    ddDrawing.UPDATED_DATE = serverDateTime;
                    ddDrawing.DELETE_FLAG = false;
                    if (costcentermaster.Photo != null)
                    {
                        //ddDrawing.PHOTO = System.IO.File.ReadAllBytes(costcentermaster.ImageByte);
                        ddDrawing.PHOTO = costcentermaster.ImageByte;
                        ddDrawing.FILE_PATH = costcentermaster.FilePath;
                    }
                    update = true;
                }
                else
                {
                    ddDrawing = new DDCOST_CENT_DWG();
                    ddDrawing.ENTERED_BY = userInformation.UserName;
                    ddDrawing.ENTERED_DATE = serverDate;
                    ddDrawing.DELETE_FLAG = false;
                    ddDrawing.COST_CENT_CODE = costcentermaster.CostCentCode;
                    ddDrawing.PHOTO = costcentermaster.ImageByte;
                    ddDrawing.FILE_PATH = costcentermaster.FilePath;
                    insert = true;
                    DB.DDCOST_CENT_DWG.InsertOnSubmit(ddDrawing);
                }
                submit = true;
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException ex)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                ex.LogException();
                return false;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (insert == true)
                    {
                        DB.DDCOST_CENT_DWG.DeleteOnSubmit(ddDrawing);
                    }
                    if (update == true)
                    {
                        DB.DDCOST_CENT_DWG.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddDrawing);
                    }
                }
                throw ex.LogException();
            }
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
