using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class OperatorQualityAssurance : Essential, IDataManipulation
    {
        public OperatorQualityAssurance(UserInformation userInformation)
        {
            try
            {
                this.userInformation = userInformation;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public List<PRD_MAST> GetProductMasterDetailsByPartNumber(PRD_MAST paramEntity = null)
        {

            List<PRD_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.IDPK == paramEntity.IDPK
                                 select row).ToList<PRD_MAST>();
                }
                else
                {

                    lstEntity = (from row in DB.PRD_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<PRD_MAST>();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<PROCESS_MAIN> GetProcessSheetMainDetailsByPartNumber(PRD_MAST paramEntity = null)
        {

            List<PROCESS_MAIN> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PROCESS_MAIN
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<PROCESS_MAIN>();
                }
                else
                {

                    lstEntity = (from row in DB.PROCESS_MAIN
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<PROCESS_MAIN>();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<PRD_DWG_ISSUE> GetDrawingIssueDetailsByPartNumber(PRD_DWG_ISSUE paramEntity = null)
        {

            List<PRD_DWG_ISSUE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_DWG_ISSUE
                                 where row.PART_NO == paramEntity.PART_NO && row.DWG_TYPE == paramEntity.DWG_TYPE
                                 orderby row.ISSUE_DATE descending, row.ISSUE_NO descending
                                 select row).ToList<PRD_DWG_ISSUE>();
                }
                else
                {
                    lstEntity = (from row in DB.PRD_DWG_ISSUE
                                 where row.DWG_TYPE == 1
                                 orderby row.ISSUE_DATE descending, row.ISSUE_NO descending
                                 select row).ToList<PRD_DWG_ISSUE>();


                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<WORK_ORDER_MAIN> GetWorkOrderDetailsByPartNumber(WORK_ORDER_MAIN paramEntity = null)
        {

            List<WORK_ORDER_MAIN> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.WORK_ORDER_MAIN
                                 where row.PART_NO == paramEntity.PART_NO
                                 //where row.PART_NO == paramEntity.PART_NO && row.WORK_ORDER_NO == paramEntity.WORK_ORDER_NO
                                 select row).ToList<WORK_ORDER_MAIN>();
                }
                else
                {
                    lstEntity = (from row in DB.WORK_ORDER_MAIN
                                 select row).ToList<WORK_ORDER_MAIN>();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }
        public List<WORK_ORDER_MAIN> GetWorkOrderDetailByPartNumberOrderNumber(WORK_ORDER_MAIN paramEntity = null)
        {

            List<WORK_ORDER_MAIN> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.WORK_ORDER_MAIN
                                 where row.PART_NO == paramEntity.PART_NO && row.WORK_ORDER_NO == paramEntity.WORK_ORDER_NO
                                 select row).ToList<WORK_ORDER_MAIN>();
                }
                else
                {
                    lstEntity = (from row in DB.WORK_ORDER_MAIN
                                 select row).ToList<WORK_ORDER_MAIN>();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<OQA_CCF> GetCCFDetailsByPartNumber(OQA_CCF paramEntity = null)
        {

            List<OQA_CCF> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.OQA_CCF
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<OQA_CCF>();
                }
                else
                {
                    lstEntity = (from row in DB.OQA_CCF
                                 select row).ToList<OQA_CCF>();


                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<PROCESS_SHEET> GetProcessSheetDetailsByPartNumber(PROCESS_SHEET paramEntity = null)
        {

            List<PROCESS_SHEET> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PROCESS_SHEET
                                 where row.PART_NO == paramEntity.PART_NO
                                 orderby row.SEQ_NO
                                 select row).ToList<PROCESS_SHEET>();
                }
                else
                {
                    lstEntity = (from row in DB.PROCESS_SHEET
                                 orderby row.SEQ_NO
                                 select row).ToList<PROCESS_SHEET>();
                }

                if (paramEntity.IsNotNullOrEmpty() && paramEntity.ROUTE_NO.IsNotNullOrEmpty() && lstEntity.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PART_NO == paramEntity.PART_NO && row.ROUTE_NO == paramEntity.ROUTE_NO
                                 orderby row.SEQ_NO
                                 select row).ToList<PROCESS_SHEET>();
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<PROCESS_CC> GetProcessSheetCCDetailsByPartNumber(PROCESS_CC paramEntity = null)
        {

            List<PROCESS_CC> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PROCESS_CC
                                 where row.PART_NO == paramEntity.PART_NO
                                 orderby row.SEQ_NO
                                 select row).ToList<PROCESS_CC>();
                }
                else
                {
                    lstEntity = (from row in DB.PROCESS_CC
                                 orderby row.SEQ_NO
                                 select row).ToList<PROCESS_CC>();
                }

                if (paramEntity.IsNotNullOrEmpty() && paramEntity.ROUTE_NO.IsNotNullOrEmpty() && paramEntity.SEQ_NO.IsNotNullOrEmpty() && lstEntity.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PART_NO == paramEntity.PART_NO && row.ROUTE_NO == paramEntity.ROUTE_NO && row.SEQ_NO == paramEntity.SEQ_NO
                                 orderby row.SEQ_NO
                                 select
                                 new PROCESS_CC()
                                 {
                                     PART_NO = row.PART_NO,
                                     ROUTE_NO = row.ROUTE_NO,
                                     SEQ_NO = row.SEQ_NO,
                                     CC_SNO = row.CC_SNO,
                                     CC_CODE = row.CC_CODE,
                                     WIRE_SIZE_MAX = row.WIRE_SIZE_MAX,
                                     WIRE_SIZE_MIN = row.WIRE_SIZE_MIN,
                                     TS_ISSUE_NO = row.TS_ISSUE_NO,
                                     TS_ISSUE_DATE = row.TS_ISSUE_DATE,
                                     TS_ISSUE_ALTER = (row.WIRE_SIZE_MIN != null && row.WIRE_SIZE_MAX != null ?
                                                       Convert.ToString(row.WIRE_SIZE_MIN).Trim() + "/" + Convert.ToString(row.WIRE_SIZE_MAX).Trim() :
                                                       Convert.ToString(Math.Max(Convert.ToDecimal(row.WIRE_SIZE_MIN), Convert.ToDecimal(row.WIRE_SIZE_MAX)))),
                                     TS_COMPILED_BY = row.TS_COMPILED_BY,
                                     TS_APPROVED_BY = row.TS_APPROVED_BY,
                                     OUTPUT = row.OUTPUT,
                                     ROWID = row.ROWID
                                 }).ToList<PROCESS_CC>();
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<PCCS> GetPCCSDetailsByPartNumber(PCCS paramEntity = null)
        {

            List<PCCS> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PCCS
                                 where row.PART_NO == paramEntity.PART_NO
                                 orderby row.SNO
                                 select row).ToList<PCCS>();
                }
                else
                {
                    lstEntity = (from row in DB.PCCS
                                 orderby row.SNO
                                 select row).ToList<PCCS>();
                }

                if (paramEntity.IsNotNullOrEmpty() && paramEntity.ROUTE_NO.IsNotNullOrEmpty() && paramEntity.SEQ_NO.IsNotNullOrEmpty() && lstEntity.IsNotNullOrEmpty())
                {
                    double sno = 1;
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PART_NO == paramEntity.PART_NO && row.ROUTE_NO == paramEntity.ROUTE_NO && row.SEQ_NO == paramEntity.SEQ_NO
                                 orderby row.SNO
                                 select new PCCS()
                                 {
                                     PART_NO = row.PART_NO,
                                     ROUTE_NO = row.ROUTE_NO,
                                     SEQ_NO = row.SEQ_NO,
                                     SNO = sno++,
                                     ISR_NO = row.ISR_NO,
                                     FEATURE = row.FEATURE,
                                     CLASS = row.CLASS,
                                     SPEC = row.SPEC,
                                     CONTROL_SPEC = row.CONTROL_SPEC,
                                     DEPT_RESP = row.DEPT_RESP,
                                     FREQ_OF_INSP = row.FREQ_OF_INSP,
                                     GAUGES_USED = row.GAUGES_USED,
                                     GAUGE_RR = row.GAUGE_RR,
                                     INPROCESS_CONTROL = row.INPROCESS_CONTROL,
                                     METHOD_OF_STUDY = row.METHOD_OF_STUDY,
                                     SAMPLE_SIZE = row.SAMPLE_SIZE,
                                     CP = row.CP,
                                     CPK = row.CPK,
                                     PRD_PROC_COMP = row.PRD_PROC_COMP,
                                     FREQ_OF_STUDY = row.FREQ_OF_STUDY,
                                     COMMENTS = row.COMMENTS,
                                     SPEC_MIN = row.SPEC_MIN,
                                     SPEC_MAX = row.SPEC_MAX,
                                     CTRL_SPEC_MIN = row.CTRL_SPEC_MIN,
                                     CTRL_SPEC_MAX = row.CTRL_SPEC_MAX,
                                     CONTROL_METHOD = row.CONTROL_METHOD,
                                     REACTION_PLAN = row.REACTION_PLAN,
                                     PROCESS_FEATURE = row.PROCESS_FEATURE,
                                     //SPEC_CHAR = row.SPEC_CHAR,
                                     SPEC_CHAR = (row.SPEC_CHAR.IsNotNullOrEmpty() && row.SPEC_CHAR.ToValueAsString().Length > 1) ? Convert.ToString(row.SPEC_CHAR).Trim().Substring(0, 1) : "",
                                     ROWID = row.ROWID,
                                     DELETE_FLAG = row.DELETE_FLAG,
                                     ENTERED_DATE = row.ENTERED_DATE,
                                     ENTERED_BY = row.ENTERED_BY,
                                     UPDATED_DATE = row.UPDATED_DATE,
                                     UPDATED_BY = row.UPDATED_BY,
                                 }).ToList<PCCS>();
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public List<DDCOST_CENT_MAST> GetCostCentreMasterDetailsByCode(DDCOST_CENT_MAST paramEntity = null)
        {

            List<DDCOST_CENT_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.COST_CENT_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDCOST_CENT_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 select row).ToList<DDCOST_CENT_MAST>();
                }
                else
                {

                    lstEntity = (from row in DB.DDCOST_CENT_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDCOST_CENT_MAST>();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstEntity;
        }

        public DateTime? ServerDateTime()
        {
            return serverDate;
        }

        /// <summary>
        /// Userd to get Raw Materials
        /// </summary>
        /// <param name="rawMaterialDescription">Code of the Raw Material</param>
        /// <returns>List of Raw Material</returns>
        public DDRM_MAST GetRawMaterialByCode(string rawMaterialCode = "")
        {
            List<DDRM_MAST> lstRawMaterial = null;
            DDRM_MAST ddrm_mast = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return ddrm_mast;
                if (rawMaterialCode.IsNotNullOrEmpty())
                {
                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false) && row.RM_CODE == rawMaterialCode
                                      select row).ToList<DDRM_MAST>();
                }
                else
                {

                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false)
                                      select row).ToList<DDRM_MAST>();
                }
                if (lstRawMaterial.IsNotNullOrEmpty() && lstRawMaterial.Count > 0)
                {
                    ddrm_mast = lstRawMaterial[0];
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return ddrm_mast;
        }

        public bool Insert<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as WORK_ORDER_MAIN).IsNotNullOrEmpty())
                {
                    WORK_ORDER_MAIN obj = entity as WORK_ORDER_MAIN;
                    try
                    {
                        if (obj.IDPK <= 0) obj.IDPK = GenerateNextNumber("WORK_ORDER_MAIN", "IDPK").ToIntValue();

                        obj.ROWID = Guid.NewGuid();
                        obj.DELETE_FLAG = false;
                        obj.ENTERED_BY = userName;
                        obj.ENTERED_DATE = serverDateTime;

                        DB.WORK_ORDER_MAIN.InsertOnSubmit(obj);
                        DB.SubmitChanges();

                        ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.WORK_ORDER_MAIN.DeleteOnSubmit(obj);

                    }

                }
                if ((entity as OQA_CCF).IsNotNullOrEmpty())
                {
                    OQA_CCF obj = entity as OQA_CCF;
                    try
                    {
                        if (obj.IDPK <= 0) obj.IDPK = GenerateNextNumber("OQA_CCF", "IDPK").ToIntValue();
                        DB.OQA_CCF.InsertOnSubmit(obj);
                        DB.SubmitChanges();

                        ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (InvalidOperationException)
                    {
                        DB.OQA_CCF.DeleteOnSubmit(obj);
                        DB.SubmitChanges();

                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.OQA_CCF.DeleteOnSubmit(obj);

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
                if ((entity as WORK_ORDER_MAIN).IsNotNullOrEmpty())
                {
                    WORK_ORDER_MAIN obj = null;
                    WORK_ORDER_MAIN activeEntity = (entity as WORK_ORDER_MAIN);

                    obj = (from row in DB.WORK_ORDER_MAIN
                           where row.IDPK == activeEntity.IDPK
                           select row).SingleOrDefault<WORK_ORDER_MAIN>();
                    if (obj.IsNotNullOrEmpty())
                    {
                        try
                        {
                            obj.WORK_ORDER_NO = activeEntity.WORK_ORDER_NO;
                            obj.PART_NO = activeEntity.PART_NO;
                            obj.TOTAL_QTY = activeEntity.TOTAL_QTY;
                            obj.PART_DESC = activeEntity.PART_DESC;
                            obj.CCF = activeEntity.CCF;
                            obj.ROWID = activeEntity.ROWID;

                            obj.DELETE_FLAG = false;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                        catch (ChangeConflictException)
                        {
                            foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(RefreshMode.KeepChanges);
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.WORK_ORDER_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                        }
                    }
                    else
                    {
                        returnValue = Delete<WORK_ORDER_MAIN>(new List<WORK_ORDER_MAIN> { activeEntity });
                        returnValue = Insert<WORK_ORDER_MAIN>(new List<WORK_ORDER_MAIN> { activeEntity });
                    }
                }
                if ((entity as OQA_CCF).IsNotNullOrEmpty())
                {
                    OQA_CCF objChild = null;
                    OQA_CCF activeEntityChild = (entity as OQA_CCF);

                    objChild = (from row in DB.OQA_CCF
                                where row.IDPK == activeEntityChild.IDPK
                                select row).SingleOrDefault<OQA_CCF>();

                    if (objChild.IsNotNullOrEmpty())
                    {
                        try
                        {
                            objChild.PART_NO = activeEntityChild.PART_NO;
                            objChild.CCF = activeEntityChild.CCF;
                            objChild.IDPK = activeEntityChild.IDPK;

                            objChild.DELETE_FLAG = false;
                            objChild.ENTERED_DATE = activeEntityChild.ENTERED_DATE;
                            objChild.ENTERED_BY = activeEntityChild.ENTERED_BY;
                            objChild.UPDATED_DATE = serverDateTime;
                            objChild.UPDATED_BY = userInformation.UserName;

                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                        catch (ChangeConflictException)
                        {
                            foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(RefreshMode.KeepChanges);
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.OQA_CCF.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objChild);

                        }
                        returnValue = true;
                    }
                    else
                    {
                        returnValue = Delete<OQA_CCF>(new List<OQA_CCF> { activeEntityChild });
                        returnValue = Insert<OQA_CCF>(new List<OQA_CCF> { activeEntityChild });
                    }
                    returnValue = true;
                }

            }
            returnValue = true;
            return returnValue;
        }

        public bool Delete<T>(List<T> entities)
        {
            bool returnValue = (!entities.IsNotNullOrEmpty() || (entities.IsNotNullOrEmpty() && entities.Count == 0 ? true : false));
            foreach (T entity in entities)
            {
                if ((entity as WORK_ORDER_MAIN).IsNotNullOrEmpty())
                {
                    WORK_ORDER_MAIN obj = null;
                    WORK_ORDER_MAIN activeEntity = (entity as WORK_ORDER_MAIN);
                    try
                    {
                        obj = (from row in DB.WORK_ORDER_MAIN
                               where row.IDPK == activeEntity.IDPK
                               select row).SingleOrDefault<WORK_ORDER_MAIN>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.DELETE_FLAG = true;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.WORK_ORDER_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                    }
                }
                if ((entity as OQA_CCF).IsNotNullOrEmpty())
                {
                    OQA_CCF objChild = null;
                    OQA_CCF activeEntityChild = (entity as OQA_CCF);
                    try
                    {
                        objChild = (from row in DB.OQA_CCF
                                    where row.IDPK == activeEntityChild.IDPK
                                    select row).SingleOrDefault<OQA_CCF>();

                        if (objChild.IsNotNullOrEmpty())
                        {
                            DB.OQA_CCF.DeleteOnSubmit(objChild);
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Deletes.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.OQA_CCF.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objChild);

                    }

                }
            }
            return returnValue;
        }
    }
}
