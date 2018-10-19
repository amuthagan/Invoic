using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data;

namespace ProcessDesigner.BLL
{
    public class ToolScheduleBll : Essential
    {
        public ToolScheduleBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        /// <summary>
        /// get the list of product master
        /// </summary>
        /// <returns></returns>
        public DataView GetPartNoDetails(ToolScheduleModel toolschedModel)
        {
            DataView dvTool = null;
            try
            {
                dvTool = ToDataTable((from o in DB.PRD_MAST
                                      where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select new { o.PART_NO, o.PART_DESC }).ToList()).DefaultView;
                if (dvTool != null)
                {
                    dvTool.AddNew();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dvTool;
        }

        public List<ToolSchedSubModel> GetToolScheduleSub(string partNo)
        {
            List<TOOL_SCHED_SUB> lstEntity = new List<TOOL_SCHED_SUB>();
            List<ToolSchedSubModel> lstCopy = new List<ToolSchedSubModel>();
            ToolSchedSubModel copy = new ToolSchedSubModel();

            //new
            //TOOL_SCHED_SUB entity = new TOOL_SCHED_SUB();
            //end new
            try
            {
                //original
                //if (!DB.IsNotNullOrEmpty()) return lstCopy;
                //lstEntity = (from row in DB.TOOL_SCHED_SUB
                //             where row.PART_NO.ToUpper().Trim() == partNo.ToUpper().Trim()
                //             && row.TOOL_CODE != ""
                //             orderby row.SNO //original


                //             select row).ToList<TOOL_SCHED_SUB>();
                //original end

                //new nandhini comment by me now
                if (!DB.IsNotNullOrEmpty()) return lstCopy;
                lstEntity = (from row in DB.TOOL_SCHED_SUB
                             where row.PART_NO.ToUpper().Trim() == partNo.ToUpper().Trim()
                             && row.TOOL_CODE != ""
                             //new new by nandakumar
                             //orderby row.UPDATED_DATE.HasValue descending,
                             //row.UPDATED_DATE 
                             //end new by nandakumar
                             orderby row.SUB_HEADING_NO ascending //new ny nandhu

                             //end new new

                             select row).ToList<TOOL_SCHED_SUB>();

                //end new nandhini new nandhini comment by me now


                foreach (TOOL_SCHED_SUB source in lstEntity)
                {
                    copy = new ToolSchedSubModel();
                    copy.CATEGORY = source.CATEGORY;
                    copy.CC_SNO = source.CC_SNO;
                    copy.IDPK = source.IDPK;
                    copy.PART_NO = source.PART_NO;
                    copy.QTY = source.QTY;
                    copy.REMARKS = source.REMARKS;
                    copy.ROUTE_NO = source.ROUTE_NO;
                    copy.ROWID = source.ROWID;
                    copy.SEQ_NO = source.SEQ_NO;
                    copy.SNO = source.SNO.Value.ToString("###.###");
                    copy.SUB_HEADING_NO = source.SUB_HEADING_NO;
                    copy.TOOL_CODE = source.TOOL_CODE;
                    copy.TOOL_CODE_END = source.TOOL_CODE_END;
                    copy.TOOL_DESC = source.TOOL_DESC;
                    lstCopy.Add(copy);
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
            return lstCopy;
        }

        /// <summary>
        /// Get the data from the table AUX_TOOL_SCHED.
        /// </summary>
        /// <param name="madeFor"></param>
        /// <returns></returns>
        public List<AUX_TOOL_SCHED> GetAuxToolSchedule(ToolScheduleModel toolschedModel, List<ToolSchedSubModel> toolscheduledetailsall)
        {
            List<AUX_TOOL_SCHED> lstEntity = new List<AUX_TOOL_SCHED>();
            List<AUX_TOOL_SCHED> lstCopy = new List<AUX_TOOL_SCHED>();
            AUX_TOOL_SCHED copy = null;
            try
            {
                var innerQuery = (from row in toolscheduledetailsall
                                  where row.TOOL_CODE != ""
                                  select row.TOOL_CODE);
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.AUX_TOOL_SCHED
                             where innerQuery.Contains(row.MADE_FOR)
                             select row).ToList<AUX_TOOL_SCHED>();

                foreach (AUX_TOOL_SCHED source in lstEntity)
                {
                    copy = new AUX_TOOL_SCHED();
                    copy.CATEGORY = source.CATEGORY;
                    copy.IDPK = source.IDPK;
                    copy.MADE_FOR = source.MADE_FOR;
                    copy.ROWID = source.ROWID;
                    copy.TEMPLATE_CD = source.TEMPLATE_CD;
                    copy.TOOL_CODE = source.TOOL_CODE;
                    copy.TOOL_DESC = source.TOOL_DESC;
                    lstCopy.Add(copy);
                }
                return lstCopy;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="routeNo"></param>
        /// <param name="seqNo"></param>
        /// <param name="costCentre"></param>
        /// <returns></returns>
        public List<TOOL_SCHED_ISSUE> GetAuxToolScheduleIssue(ToolScheduleModel toolSchedModel)
        {
            List<TOOL_SCHED_ISSUE> lstEntity = new List<TOOL_SCHED_ISSUE>();
            List<TOOL_SCHED_ISSUE> lstCopy = new List<TOOL_SCHED_ISSUE>();
            List<TOOL_SCHED_ISSUE> lstCopy1 = new List<TOOL_SCHED_ISSUE>();
            TOOL_SCHED_ISSUE entity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.TOOL_SCHED_ISSUE
                             where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                             select row).ToList<TOOL_SCHED_ISSUE>();
                foreach (TOOL_SCHED_ISSUE source in lstEntity)
                {
                    entity = new TOOL_SCHED_ISSUE();
                    entity.CC_SNO = source.CC_SNO;
                    entity.IDPK = source.IDPK;
                    entity.PART_NO = source.PART_NO;
                    entity.ROUTE_NO = source.ROUTE_NO;
                    entity.ROWID = source.ROWID;
                    entity.SEQ_NO = source.SEQ_NO;
                    entity.TS_APPROVED_BY = source.TS_APPROVED_BY;
                    entity.TS_COMPILED_BY = source.TS_COMPILED_BY;
                    entity.TS_ISSUE_ALTER = source.TS_ISSUE_ALTER;
                    entity.TS_ISSUE_DATE = source.TS_ISSUE_DATE;
                    entity.TS_ISSUE_NO = source.TS_ISSUE_NO;
                    int output;
                    if (int.TryParse(source.TS_ISSUE_NO.ToValueAsString(), out output) == true)
                    {
                        entity.ISSUE_NO_NEW = source.TS_ISSUE_NO.ToIntValue();
                    }
                    else
                    {
                        entity.ISSUE_NO_NEW = 0;
                    }


                    lstCopy1.Add(entity);
                }

                lstCopy = (from row in lstCopy1
                           orderby row.ISSUE_NO_NEW
                           select row).ToList<TOOL_SCHED_ISSUE>();
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstCopy;
        }

        /// <summary>
        /// Get process number for the selected part number
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public DataView GetProcessNo(ToolScheduleModel toolSchedModel)
        {
            DataView dvEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return dvEntity;
                dvEntity = ToDataTable((from row in DB.PROCESS_MAIN
                                        where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                                        //&& (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                        orderby row.ROUTE_NO
                                        select new { ROUTE_NO = Convert.ToString(row.ROUTE_NO) }).ToList()).DefaultView;
                return dvEntity;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dvEntity;
        }

        public String GetCurrentProcessNo(ToolScheduleModel toolSchedModel)
        {
            DataView dvEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return "";
                PROCESS_MAIN entity = (from row in DB.PROCESS_MAIN
                                       where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                                       && row.CURRENT_PROC == 1
                                       orderby row.ROUTE_NO
                                       select row).FirstOrDefault<PROCESS_MAIN>();
                if (entity == null)
                {
                    return "";
                }
                else
                {
                    return entity.ROUTE_NO.ToString();
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
            return "";
        }

        /// <summary>
        /// get the sequence number for the selected part number
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public DataView GetSeqNo(ToolScheduleModel toolSchedModel)
        {
            DataView dvEntity = null;

            try
            {
                if (!DB.IsNotNullOrEmpty()) return dvEntity;
                dvEntity = ToDataTable((from row in DB.PROCESS_SHEET
                                        where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                                            //&& (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                        && row.ROUTE_NO == toolSchedModel.RouteNo.ToDecimalValue()
                                        select new { SEQ_NO = Convert.ToString(row.SEQ_NO), row.OPN_DESC }).ToList()).DefaultView;
                return dvEntity;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dvEntity;
        }

        /// <summary>
        /// get the tool schedule for the selected part number
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public TOOL_SCHED_MAIN GetToolScheduleMain(ToolScheduleModel toolSchedModel)
        {
            TOOL_SCHED_MAIN lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.TOOL_SCHED_MAIN
                             where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                             //new
                             //orderby row.UPDATED_DATE descending
                             //end
                             select row).FirstOrDefault<TOOL_SCHED_MAIN>();
                if (lstEntity != null)
                {
                    toolSchedModel.RouteNo = lstEntity.ROUTE_NO.ToValueAsString();
                    toolSchedModel.SeqNo = lstEntity.SEQ_NO.ToValueAsString();
                    toolSchedModel.CCSno = lstEntity.CC_SNO.ToValueAsString();
                    toolSchedModel.SubHeadingNo = lstEntity.SUB_HEADING_NO.ToValueAsString();
                }
                else
                {
                    toolSchedModel.RouteNo = "";
                    toolSchedModel.SeqNo = "";
                    toolSchedModel.CCSno = "";
                    toolSchedModel.SubHeadingNo = "";
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
            return lstEntity;
        }

        /// <summary>
        /// get the tool schedule for the selected part number
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        /// 
        //new
        //public decimal Getsub_head_no;
        //public bool setsubheadingno(decimal sub_head_no)
        //{
        //    bool s = true;
        //    Getsub_head_no = sub_head_no;
        //    return s;
        //}
        //end new
        public DataView GetToolSubHeading(ToolScheduleModel toolSchedModel)
        {
            DataView dvEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                dvEntity = ToDataTable((from row in DB.TOOL_SCHED_MAIN
                                        where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                                        && row.ROUTE_NO == toolSchedModel.RouteNo.ToDecimalValue()
                                        && row.SEQ_NO == toolSchedModel.SeqNo.ToDecimalValue()
                                        && row.CC_SNO == toolSchedModel.CCSno.ToDecimalValue()  //original
                                        //new by nandhu
                                        //&& row.CC_SNO == Getcc_sno

                                        //end new
                                        select new { SUB_HEADING_NO = Convert.ToString(row.SUB_HEADING_NO), row.SUB_HEADING }).ToList()).DefaultView;
                return dvEntity;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dvEntity;
        }

        public DataView GetCostCentre(ToolScheduleModel toolSchedModel)
        {
            DataView dvEntity = null;
            try
            {
                //if (!DB.IsNotNullOrEmpty()) return dvEntity;
                dvEntity = ToDataTable((from row in DB.PROCESS_CC
                                        where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                                        && row.ROUTE_NO == toolSchedModel.RouteNo.ToDecimalValue()
                                        && row.SEQ_NO == toolSchedModel.SeqNo.ToDecimalValue()
                                        //new
                                        orderby row.UPDATED_DATE descending
                                        //end new
                                        select new { CC_SNO = Convert.ToString(row.CC_SNO), row.CC_CODE }).ToList()).DefaultView;
                return dvEntity;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dvEntity;
        }
        //new
        public decimal Getcc_sno;
        public bool setvalue(decimal cc_sno)
        {
            bool s = true;
            Getcc_sno = cc_sno;
            return s;
        }
        //
        public bool Save(List<ToolSchedSubModel> deleteToolSheduleDetailsAll, ToolScheduleModel toolSchedModel, List<AUX_TOOL_SCHED> auxToolScheduleDetailsAll, List<ToolSchedSubModel> toolScheduleDetailsAll, List<TOOL_SCHED_ISSUE> toolScheduleRevisionAll,
             List<AUX_TOOL_SCHED> deleteAuxToolScheduleDetailsAll, List<TOOL_SCHED_ISSUE> deleteToolScheduleIssueDetailsAll, List<TOOL_SCHED_MAIN> toolSchedMainAll)
        {
            List<TOOL_SCHED_SUB> lstsub = new List<TOOL_SCHED_SUB>();
            List<TOOL_SCHED_ISSUE> lstissue = new List<TOOL_SCHED_ISSUE>();
            try
            {
                DeleteToolSchedAux(deleteAuxToolScheduleDetailsAll);
                DeleteToolSchedIssue(deleteToolScheduleIssueDetailsAll);
                DeleteToolSchedSub(deleteToolSheduleDetailsAll);
                SaveToolSchedMainAll(toolSchedMainAll);

                SaveToolSchedSub(toolSchedModel, toolScheduleDetailsAll);

                SaveAuxToolSched(toolSchedModel, auxToolScheduleDetailsAll);
                SaveToolSchedIssue(toolSchedModel, toolScheduleRevisionAll);
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return true;
        }

        private void DeleteToolSchedSub(List<ToolSchedSubModel> deleteToolSheduleDetailsAll)
        {
            TOOL_SCHED_SUB entity;
            try
            {
                foreach (ToolSchedSubModel tssm in deleteToolSheduleDetailsAll)
                {
                    entity = (from row in DB.TOOL_SCHED_SUB
                              where row.TOOL_CODE == tssm.TOOL_CODE &&
                              row.IDPK == tssm.IDPK
                              select row).FirstOrDefault();
                    if (entity != null)
                    {
                        if (entity.IDPK > 0)
                        {
                            entity = (from row in DB.TOOL_SCHED_SUB
                                      where row.IDPK == entity.IDPK
                                      select row).FirstOrDefault();
                            if (entity != null)
                            {
                                DB.TOOL_SCHED_SUB.DeleteOnSubmit(entity);
                                DB.SubmitChanges();
                            }
                        }
                    }
                }
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
        }

        private void DeleteToolSchedAux(List<AUX_TOOL_SCHED> deleteAuxToolScheduleDetailsAll)
        {
            try
            {
                AUX_TOOL_SCHED entity;
                foreach (AUX_TOOL_SCHED ats in deleteAuxToolScheduleDetailsAll)
                {
                    if (ats.IDPK > 0)
                    {
                        entity = null;
                        entity = (from row in DB.AUX_TOOL_SCHED
                                  where row.IDPK == ats.IDPK
                                  select row).FirstOrDefault<AUX_TOOL_SCHED>();
                        if (entity != null)
                        {
                            DB.AUX_TOOL_SCHED.DeleteOnSubmit(entity);
                        }
                    }
                }
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
        }

        private void DeleteToolSchedIssue(List<TOOL_SCHED_ISSUE> deleteToolScheduleIssueDetailsAll)
        {
            try
            {
                TOOL_SCHED_ISSUE entity;
                foreach (TOOL_SCHED_ISSUE tsi in deleteToolScheduleIssueDetailsAll)
                {
                    entity = null;
                    entity = (from row in DB.TOOL_SCHED_ISSUE
                              where row.IDPK == tsi.IDPK
                              select row).FirstOrDefault<TOOL_SCHED_ISSUE>();
                    if (entity != null)
                    {
                        DB.TOOL_SCHED_ISSUE.DeleteOnSubmit(entity);
                    }
                }
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
        }
        //new
        public decimal Getsub_head_no;
        public bool setsubheadingno(decimal sub_head_no)
        {
            bool s = true;
            Getsub_head_no = sub_head_no;
            return s;
        }
        //end new
        private void SaveToolSchedSub(ToolScheduleModel toolSchedModel, List<ToolSchedSubModel> toolscheduledetailsall)
        {
            //List<ToolSchedSubModel> lstsub = new List<ToolSchedSubModel>();
            //List<ToolSchedSubModel> lstupdate = new List<ToolSchedSubModel>();
            //TOOL_SCHED_SUB newentity = new TOOL_SCHED_SUB();
            //bool submit = false;
            //try
            //{
            //    lstupdate = (from row in toolscheduledetailsall
            //                 where (row.IDPK > 0)
            //                 && row.TOOL_CODE != null && row.TOOL_CODE.Trim() != ""
            //                 orderby (row.SNO.ToDecimalValue() == 0 ? Int16.MaxValue : row.SNO.ToDecimalValue())
            //                 select row).ToList<ToolSchedSubModel>();


            //    foreach (ToolSchedSubModel oldentity in lstupdate)
            //    {
            //        newentity = (from row in DB.TOOL_SCHED_SUB
            //                     where row.IDPK == oldentity.IDPK
            //                     select row).FirstOrDefault();
            //        if (newentity != null)
            //        {
            //            newentity.CATEGORY = oldentity.CATEGORY;
            //            newentity.CC_SNO = oldentity.CC_SNO;
            //            newentity.PART_NO = oldentity.PART_NO;
            //            newentity.QTY = oldentity.QTY;
            //            newentity.REMARKS = oldentity.REMARKS;
            //            newentity.ROUTE_NO = oldentity.ROUTE_NO;
            //            newentity.SEQ_NO = oldentity.SEQ_NO;
            //            newentity.SNO = oldentity.SNO.ToDecimalValue();
            //            newentity.SUB_HEADING_NO = oldentity.SUB_HEADING_NO;
            //            newentity.TOOL_CODE = oldentity.TOOL_CODE;
            //            newentity.TOOL_CODE_END = oldentity.TOOL_CODE_END;
            //            newentity.TOOL_DESC = oldentity.TOOL_DESC;
            //        }
            //    }
            //    lstsub = (from row in toolscheduledetailsall
            //              where (row.IDPK <= 0)
            //              && row.TOOL_CODE != null && row.TOOL_CODE.Trim() != ""
            //              orderby (row.SNO.ToDecimalValue() == 0 ? Int16.MaxValue : row.SNO.ToDecimalValue())
            //              select row).ToList<ToolSchedSubModel>();
            //    //List<ToolSchedSubModel> lstupdate1 = new List<ToolSchedSubModel>();

            //    //lstupdate1 = (from row in toolscheduledetailsall
            //    //              where (row.IDPK > 0)
            //    //              && row.TOOL_CODE != null && row.TOOL_CODE.Trim() != ""
            //    //              select row).ToList<ToolSchedSubModel>();
            //    //foreach (ToolSchedSubModel tools in lstupdate1)
            //    //{
            //    //    DeleteToolSchedSub(lstupdate1);
            //    //    DB.SubmitChanges();
            //    //}

            //    foreach (ToolSchedSubModel tool in lstsub)
            //    {
            //        newentity = new TOOL_SCHED_SUB();
            //        newentity.CATEGORY = tool.CATEGORY;
            //        newentity.CC_SNO = tool.CC_SNO;
            //        newentity.PART_NO = tool.PART_NO;
            //        newentity.QTY = tool.QTY;
            //        newentity.REMARKS = tool.REMARKS;
            //        newentity.ROUTE_NO = tool.ROUTE_NO;
            //        newentity.SEQ_NO = tool.SEQ_NO;
            //        newentity.SNO = tool.SNO.ToDecimalValue();
            //        newentity.SUB_HEADING_NO = tool.SUB_HEADING_NO;
            //        newentity.TOOL_CODE = tool.TOOL_CODE;
            //        newentity.TOOL_CODE_END = tool.TOOL_CODE_END;
            //        newentity.TOOL_DESC = tool.TOOL_DESC;
            //        newentity.ROWID = tool.ROWID;

            //        if (tool.ROWID == System.Guid.Empty)
            //        {
            //            newentity.ROWID = Guid.NewGuid();
            //            submit = true;
            //            DB.TOOL_SCHED_SUB.InsertOnSubmit(newentity);
            //        }
            //        DB.SubmitChanges();
            //    }

            //    //new

            //    //      TOOL_SCHED_SUB lstEntity = new TOOL_SCHED_SUB();
            //    //ToolSchedSubModel lstCopy = new ToolSchedSubModel();
            //    //ToolSchedSubModel copy = new ToolSchedSubModel();

            //    //comment by me
            //    //lstEntity = (from row in DB.TOOL_SCHED_SUB
            //    //             where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
            //    //             //new
            //    //             && row.ROUTE_NO == toolSchedModel.RouteNo.ToDecimalValue()
            //    //            && row.SEQ_NO == toolSchedModel.SeqNo.ToDecimalValue()
            //    //                 //&& row.CC_SNO == tsm.CC_SNO
            //    //              && row.CC_SNO == Getcc_sno
            //    //              //end new
            //    //             && row.SUB_HEADING_NO == Getsub_head_no
            //    //             //&& row.TOOL_CODE != ""
            //    //             //orderby row.SNO
            //    //             select row).FirstOrDefault();
            //    //if (lstEntity != null)
            //    //{
            //    //    //pcc.UPDATED_DATE = serverDateTime;
            //    //    lstEntity.UPDATED_DATE = DateTime.Now;
            //    //}
            //    //lstEntity.UPDATED_DATE = null;
            //    //end new nandhini

            //    //end comment by me


            //    /*
            //    if (lstsub.Count > 0)
            //    {
            //        DB.TOOL_SCHED_SUB.InsertAllOnSubmit(lstsub);
            //    }
            //    */
            //}
            //catch (System.Data.Linq.ChangeConflictException)
            //{
            //    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            //    DB.TOOL_SCHED_SUB.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, newentity);
            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
            List<ToolSchedSubModel> lstsub = new List<ToolSchedSubModel>();
            List<ToolSchedSubModel> lstupdate = new List<ToolSchedSubModel>();
            TOOL_SCHED_SUB newentity = new TOOL_SCHED_SUB();
            try
            {
                lstupdate = (from row in toolscheduledetailsall
                             where (row.IDPK > 0)
                             && row.TOOL_CODE != null && row.TOOL_CODE.Trim() != ""
                             orderby (row.SNO.ToDecimalValue() == 0 ? Int16.MaxValue : row.SNO.ToDecimalValue())
                             select row).ToList<ToolSchedSubModel>();

                foreach (ToolSchedSubModel oldentity in lstupdate)
                {
                    newentity = (from row in DB.TOOL_SCHED_SUB
                                 where row.IDPK == oldentity.IDPK
                                 select row).FirstOrDefault();
                    if (newentity != null)
                    {
                        newentity.CATEGORY = oldentity.CATEGORY;
                        newentity.CC_SNO = oldentity.CC_SNO;
                        newentity.PART_NO = oldentity.PART_NO;
                        newentity.QTY = oldentity.QTY;
                        newentity.REMARKS = oldentity.REMARKS;
                        newentity.ROUTE_NO = oldentity.ROUTE_NO;
                        newentity.SEQ_NO = oldentity.SEQ_NO;
                        newentity.SNO = oldentity.SNO.ToDecimalValue();
                        newentity.SUB_HEADING_NO = oldentity.SUB_HEADING_NO;
                        newentity.TOOL_CODE = oldentity.TOOL_CODE;
                        newentity.TOOL_CODE_END = oldentity.TOOL_CODE_END;
                        newentity.TOOL_DESC = oldentity.TOOL_DESC;
                    }
                }
                lstsub = (from row in toolscheduledetailsall
                          where (row.IDPK <= 0)
                          && row.TOOL_CODE != null && row.TOOL_CODE.Trim() != ""
                          orderby (row.SNO.ToDecimalValue() == 0 ? Int16.MaxValue : row.SNO.ToDecimalValue())
                          select row).ToList<ToolSchedSubModel>();
                foreach (ToolSchedSubModel tool in lstsub)
                {
                    newentity = new TOOL_SCHED_SUB();
                    newentity.CATEGORY = tool.CATEGORY;
                    newentity.CC_SNO = tool.CC_SNO;
                    newentity.PART_NO = tool.PART_NO;
                    newentity.QTY = tool.QTY;
                    newentity.REMARKS = tool.REMARKS;
                    newentity.ROUTE_NO = tool.ROUTE_NO;
                    newentity.SEQ_NO = tool.SEQ_NO;
                    newentity.SNO = tool.SNO.ToDecimalValue();
                    newentity.SUB_HEADING_NO = tool.SUB_HEADING_NO;
                    newentity.TOOL_CODE = tool.TOOL_CODE;
                    newentity.TOOL_CODE_END = tool.TOOL_CODE_END;
                    newentity.TOOL_DESC = tool.TOOL_DESC;
                    newentity.ROWID = Guid.NewGuid();
                    TOOL_SCHED_SUB entity;
                    //DeleteToolSchedSub(lstsub);
                    entity = (from row in DB.TOOL_SCHED_SUB
                              where row.TOOL_CODE == newentity.TOOL_CODE
                              select row).FirstOrDefault();
                    if (entity != null)
                    {
                        DB.TOOL_SCHED_SUB.DeleteOnSubmit(entity);
                        DB.SubmitChanges();
                    }
                    DB.TOOL_SCHED_SUB.InsertOnSubmit(newentity);
                }

                //new

                //      TOOL_SCHED_SUB lstEntity = new TOOL_SCHED_SUB();
                //ToolSchedSubModel lstCopy = new ToolSchedSubModel();
                //ToolSchedSubModel copy = new ToolSchedSubModel();

                //comment by me
                //lstEntity = (from row in DB.TOOL_SCHED_SUB
                //             where row.PART_NO.ToUpper().Trim() == toolSchedModel.PartNo.ToUpper().Trim()
                //             //new
                //             && row.ROUTE_NO == toolSchedModel.RouteNo.ToDecimalValue()
                //            && row.SEQ_NO == toolSchedModel.SeqNo.ToDecimalValue()
                //                 //&& row.CC_SNO == tsm.CC_SNO
                //              && row.CC_SNO == Getcc_sno
                //              //end new
                //             && row.SUB_HEADING_NO == Getsub_head_no
                //             //&& row.TOOL_CODE != ""
                //             //orderby row.SNO
                //             select row).FirstOrDefault();
                //if (lstEntity != null)
                //{
                //    //pcc.UPDATED_DATE = serverDateTime;
                //    lstEntity.UPDATED_DATE = DateTime.Now;
                //}
                //lstEntity.UPDATED_DATE = null;
                //end new nandhini

                //end comment by me


                /*
                if (lstsub.Count > 0)
                {
                    DB.TOOL_SCHED_SUB.InsertAllOnSubmit(lstsub);
                }
                */
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


        private void SaveToolSchedMainAll(List<TOOL_SCHED_MAIN> lstToolSchedMain)
        {
            try
            {
                foreach (TOOL_SCHED_MAIN tsm in lstToolSchedMain)
                {
                    SaveToolSchedMain(tsm);
                    // DB.TOOL_SCHED_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstToolSchedMain);
                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void SaveToolSchedMain(TOOL_SCHED_MAIN tsm)
        {
            TOOL_SCHED_MAIN entity = new TOOL_SCHED_MAIN();
            bool insert = false;
            bool update = false;
            bool submit = false;
            try
            {
                entity = (from row in DB.TOOL_SCHED_MAIN
                          where
                          row.PART_NO == tsm.PART_NO
                          && row.ROUTE_NO == tsm.ROUTE_NO
                          && row.SEQ_NO == tsm.SEQ_NO
                          && row.CC_SNO == tsm.CC_SNO
                          && row.SUB_HEADING_NO == tsm.SUB_HEADING_NO
                          orderby row.ROWID
                          select row).FirstOrDefault();
                DB.SubmitChanges();
                if (entity != null)
                {
                    entity.TOP_NOTE = tsm.TOP_NOTE;
                    entity.BOT_NOTE = tsm.BOT_NOTE;
                    entity.UPDATED_BY = userName;
                    entity.UPDATED_DATE = serverDateTime;
                    entity.SUB_HEADING_NO = tsm.SUB_HEADING_NO;
                    entity.SUB_HEADING = tsm.SUB_HEADING;
                    //new
                    //entity.DELETE_FLAG = true;
                    //end new
                    update = true;
                    //}

                    //new by nandhini

                    PROCESS_CC pcc = (from row in DB.PROCESS_CC

                                      where
                                      row.PART_NO == tsm.PART_NO
                                      && row.ROUTE_NO == tsm.ROUTE_NO
                                      && row.SEQ_NO == tsm.SEQ_NO
                                          //&& row.CC_SNO == tsm.CC_SNO
                                        && row.CC_SNO == Getcc_sno
                                      select row).FirstOrDefault();
                    if (pcc != null)
                    {
                        //pcc.UPDATED_DATE = serverDateTime;
                        pcc.UPDATED_DATE = DateTime.Now;
                    }

                    //end new nandhini
                }
                else
                {
                    entity = new TOOL_SCHED_MAIN();
                    entity.ROWID = Guid.NewGuid();
                    entity.TOP_NOTE = tsm.TOP_NOTE;
                    entity.BOT_NOTE = tsm.BOT_NOTE;
                    entity.CC_SNO = tsm.CC_SNO;
                    entity.ENTERED_BY = userInformation.UserName;
                    entity.PART_NO = tsm.PART_NO;
                    entity.ROUTE_NO = tsm.ROUTE_NO;
                    entity.SEQ_NO = tsm.SEQ_NO;
                    entity.CC_SNO = tsm.CC_SNO;
                    entity.SUB_HEADING_NO = tsm.SUB_HEADING_NO;
                    entity.SUB_HEADING = tsm.SUB_HEADING;
                    entity.ENTERED_BY = userName;
                    entity.ENTERED_DATE = serverDateTime;
                    insert = true;
                    DB.TOOL_SCHED_MAIN.InsertOnSubmit(entity);
                }
                submit = true;
                DB.SubmitChanges();
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (insert == true)
                    {
                        DB.TOOL_SCHED_MAIN.DeleteOnSubmit(entity);
                    }
                    if (update == true)
                    {
                        DB.TOOL_SCHED_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, entity);
                    }
                }
            }
        }


        private void SaveToolSchedIssue(ToolScheduleModel toolSchedModel, List<TOOL_SCHED_ISSUE> toolschedulerevisionall)
        {
            List<TOOL_SCHED_ISSUE> lstissue = new List<TOOL_SCHED_ISSUE>();
            TOOL_SCHED_ISSUE newentity = new TOOL_SCHED_ISSUE();
            try
            {

                lstissue = (from row in toolschedulerevisionall
                            where (row.IDPK > 0)
                            && row.TS_ISSUE_NO != null && row.TS_ISSUE_NO.Trim() != ""
                            select row).ToList<TOOL_SCHED_ISSUE>();

                foreach (TOOL_SCHED_ISSUE oldentity in lstissue)
                {
                    newentity = (from row in DB.TOOL_SCHED_ISSUE
                                 where (row.IDPK == oldentity.IDPK)
                                 select row).FirstOrDefault<TOOL_SCHED_ISSUE>();
                    if (newentity != null)
                    {
                        newentity.CC_SNO = oldentity.CC_SNO;
                        newentity.PART_NO = oldentity.PART_NO;
                        newentity.ROUTE_NO = oldentity.ROUTE_NO;
                        newentity.SEQ_NO = oldentity.SEQ_NO;
                        newentity.TS_APPROVED_BY = oldentity.TS_APPROVED_BY;
                        newentity.TS_COMPILED_BY = oldentity.TS_COMPILED_BY;
                        newentity.TS_ISSUE_ALTER = oldentity.TS_ISSUE_ALTER;
                        newentity.TS_ISSUE_DATE = oldentity.TS_ISSUE_DATE;
                        newentity.TS_ISSUE_NO = oldentity.TS_ISSUE_NO;
                    }
                }

                lstissue = new List<TOOL_SCHED_ISSUE>();
                lstissue = (from row in toolschedulerevisionall
                            where (row.IDPK == 0 || row.IDPK == -1)
                            && row.TS_ISSUE_NO != null && row.TS_ISSUE_NO.Trim() != ""
                            select row).ToList<TOOL_SCHED_ISSUE>();
                foreach (TOOL_SCHED_ISSUE tool in lstissue)
                {
                    tool.ROWID = Guid.NewGuid();
                }
                if (lstissue.Count > 0)
                {
                    DB.TOOL_SCHED_ISSUE.InsertAllOnSubmit(lstissue);
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

        private void SaveAuxToolSched(ToolScheduleModel toolSchedModel, List<AUX_TOOL_SCHED> auxtoolscheduledetailsall)
        {
            List<AUX_TOOL_SCHED> lstissue = new List<AUX_TOOL_SCHED>();
            AUX_TOOL_SCHED newentity = new AUX_TOOL_SCHED();
            try
            {


                lstissue = new List<AUX_TOOL_SCHED>();

                lstissue = (from row in auxtoolscheduledetailsall
                            where (row.IDPK > 0)
                            && (row.TOOL_CODE.ToValueAsString().Trim() != ""
                            || row.CATEGORY.ToValueAsString() != ""
                            || row.TEMPLATE_CD.ToValueAsString() != ""
                            || row.TOOL_DESC.ToValueAsString() != "")
                            select row).ToList<AUX_TOOL_SCHED>();

                foreach (AUX_TOOL_SCHED oldentity in lstissue)
                {
                    newentity = new AUX_TOOL_SCHED();
                    newentity = (from row in DB.AUX_TOOL_SCHED
                                 where row.IDPK == oldentity.IDPK
                                 select row).FirstOrDefault<AUX_TOOL_SCHED>();
                    newentity.CATEGORY = oldentity.CATEGORY;
                    newentity.MADE_FOR = oldentity.MADE_FOR;
                    newentity.TEMPLATE_CD = oldentity.TEMPLATE_CD;
                    newentity.TOOL_CODE = oldentity.TOOL_CODE;
                    newentity.TOOL_DESC = oldentity.TOOL_DESC;
                }

                lstissue = new List<AUX_TOOL_SCHED>();
                lstissue = (from row in auxtoolscheduledetailsall
                            where (row.IDPK == 0 || row.IDPK == -1)
                            && (row.TOOL_CODE.ToValueAsString().Trim() != ""
                            || row.CATEGORY.ToValueAsString() != ""
                            || row.TEMPLATE_CD.ToValueAsString() != ""
                            || row.TOOL_DESC.ToValueAsString() != "")
                            select row).ToList<AUX_TOOL_SCHED>();
                foreach (AUX_TOOL_SCHED tool in lstissue)
                {
                    tool.ROWID = Guid.NewGuid();
                }
                if (lstissue.Count > 0)
                {
                    DB.AUX_TOOL_SCHED.InsertAllOnSubmit(lstissue);
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

        public List<TOOL_SCHED_MAIN> GetToolScheduleMainList(ToolScheduleModel toolSchedModel)
        {
            List<TOOL_SCHED_MAIN> lstentity;
            TOOL_SCHED_MAIN entity;
            try
            {
                lstentity = (from row in DB.TOOL_SCHED_MAIN
                             where
                             row.PART_NO.Trim() == toolSchedModel.PartNo.Trim()
                             && (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                             //orderby row.UPDATED_DATE descending cooment by nandhu
                             select row).ToList<TOOL_SCHED_MAIN>();
                ////if (lstentity.Count > 0)
                ////{
                ////    entity = lstentity[0];
                ////    toolSchedModel.RouteNo = entity.ROUTE_NO.ToValueAsString();
                ////    //GetSeqNo(toolSchedModel);
                ////    toolSchedModel.SeqNo = entity.SEQ_NO.ToValueAsString();
                ////    //GetCostCentre(toolSchedModel);
                ////    toolSchedModel.CCSno = entity.CC_SNO.ToValueAsString();
                ////    //GetToolSubHeading(toolSchedModel);
                ////    toolSchedModel.SubHeadingNo = entity.SUB_HEADING_NO.ToValueAsString();
                ////    toolSchedModel.TopNote = entity.TOP_NOTE;
                ////    toolSchedModel.BotNote = entity.BOT_NOTE;
                ////}
                ////else
                ////{
                ////    toolSchedModel.RouteNo = "";
                ////    toolSchedModel.SeqNo = "";
                ////    toolSchedModel.CCSno = "";
                ////    toolSchedModel.SubHeadingNo = "";
                ////    toolSchedModel.TopNote = "";
                ////    toolSchedModel.BotNote = "";
                ////}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstentity;
        }

        public int ValidateToolInfo(string toolCode)
        {
            TOOL_DIMENSION entity = null;
            try
            {
                //if (!DB.IsNotNullOrEmpty()) return dvEntity;
                entity = (from row in DB.TOOL_DIMENSION
                          where row.TOOL_CD == toolCode.Trim().Replace(" ", "")
                          select row).FirstOrDefault<TOOL_DIMENSION>();
                if (entity == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
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
            return 0;
        }

        public DataTable GetToolScheduleReport(string partNo, string seqNo, string routeNo, string ccSno)
        {
            DataTable dtData;

            try
            {
                if (seqNo.Trim() == "") seqNo = "0";
                if (routeNo.Trim() == "") routeNo = "0";
                if (ccSno.Trim() == "") ccSno = "0";

                StringBuilder sbsql = new StringBuilder();
                sbsql.Append("select a.part_no,cast(a.seq_no as varchar) as seq_no, ");
                sbsql.Append("(cast(a.wire_size_max as varchar) + '/' + cast(a.wire_size_min as varchar)) wire,cast(d.sno as varchar) sno,d.category, ");
                sbsql.Append("cast(b.opn_cd as varchar) opn_cd,d.tool_code,tool_code_end,cast(d.cc_sno as varchar) cc_sno,cast(d.qty as varchar) qty,upper(d.tool_desc) tool_desc1,");
                sbsql.Append("upper(d.remarks) remark1, a.cc_code,e.part_desc,b.opn_desc,upper(f.sub_heading) sub_head, ");
                sbsql.Append("upper(f.top_note) TN, upper(f.bot_note) BN, c.cost_cent_desc,upper(f.sub_heading_no) sub_head_no,cast(a.route_no as varchar) route_no from process_cc a,");
                sbsql.Append("process_sheet b,ddcost_cent_mast c, tool_sched_sub d,prd_mast e,tool_sched_main f ");
                sbsql.Append("where a.part_no = b.part_no and a.part_no = d.part_no and a.part_no = f.part_no and ");
                sbsql.Append("a.route_no = b.route_no and a.route_no = d.route_no and ");
                sbsql.Append("a.route_no = f.route_no and a.seq_no = b.seq_no and a.seq_no = d.seq_no and ");
                sbsql.Append("a.seq_no = f.seq_no  and a.part_no = e.part_no and ");
                sbsql.Append("a.cc_sno = f.cc_sno and a.cc_sno = d.cc_sno and a.cc_code  COLLATE Latin1_General_CI_AS = c.cost_cent_code and ");
                sbsql.Append("d.sub_heading_no = f.sub_heading_no and a.part_no = '" + partNo + "' and a.seq_no = " + seqNo + " and a.route_no = " + routeNo + " and a.cc_sno =  " + ccSno);
                sbsql.Append("order by d.sub_heading_no, d.sno ");

                dtData = ToDataTable(DB.ExecuteQuery<ToolScheduleReportModel>(sbsql.ToString()).ToList());

                if (dtData.Rows.Count == 0)
                {
                    sbsql = new StringBuilder();
                    sbsql.Append("select a.part_no,cast(a.seq_no as varchar) as seq_no, ");
                    sbsql.Append("(cast(a.wire_size_max as varchar) + '/' + cast(a.wire_size_min as varchar)) wire,'' as sno,'' as category,");
                    sbsql.Append("cast(b.opn_cd as varchar) as opn_cd,'' as tool_code,'' as tool_code_end,cast(a.cc_sno as varchar),'' as qty, '' as tool_desc1,");
                    sbsql.Append("'' as remark1,a.cc_code,e.part_desc,b.opn_desc,upper(f.sub_heading) sub_head,");
                    sbsql.Append(" upper(f.top_note) TN, upper(f.bot_note) BN,c.cost_cent_desc,upper(f.sub_heading_no) sub_head_no,cast(a.route_no as varchar) route_no from process_cc a, ");
                    sbsql.Append("process_sheet b,ddcost_cent_mast c, prd_mast e,tool_sched_main f ");
                    sbsql.Append("where a.part_no = b.part_no   ");
                    sbsql.Append("and a.part_no = f.part_no and a.route_no = b.route_no  and ");
                    sbsql.Append("a.route_no = f.route_no and a.seq_no = b.seq_no   and ");
                    sbsql.Append("a.seq_no = f.seq_no  and a.part_no = e.part_no and a.cc_sno = f.cc_sno  and ");
                    sbsql.Append("a.cc_code collate SQL_Latin1_General_CP1_CS_AS = c.cost_cent_code  and a.part_no = '" + sqlEncode(partNo) + "' and a.seq_no = " + seqNo + " and ");
                    sbsql.Append("a.route_no = " + routeNo + " and a.cc_sno =  " + ccSno + " order by f.sub_heading_no  ");
                    dtData = ToDataTable(DB.ExecuteQuery<ToolScheduleReportModel>(sbsql.ToString()).ToList());
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dtData;
        }

        public DataTable GetAuxToolScheduleReport(string partNo, string seqNo, string routeNo, string ccSno)
        {
            DataTable dtData;
            PROCESS_CC entity;
            string ccCode = "";

            try
            {

                if (seqNo.Trim() == "") seqNo = "0";
                if (routeNo.Trim() == "") routeNo = "0";
                if (ccSno.Trim() == "") ccSno = "0";

                StringBuilder sbsql = new StringBuilder();

                entity = (from row in DB.PROCESS_CC
                          where row.PART_NO == partNo && row.ROUTE_NO == Convert.ToDecimal(routeNo) &&
                          row.SEQ_NO == Convert.ToDecimal(seqNo) && row.CC_SNO == Convert.ToDecimal(ccSno)
                          select row).FirstOrDefault();

                if (entity != null)
                {
                    ccCode = entity.CC_CODE;
                }

                sbsql.Append(" select '" + sqlEncode(partNo) + "' as  PART_NO, '" + sqlEncode(ccCode) + "' as CC_CODE ,a.tool_code,a.tool_desc,a.made_for,a.template_cd from aux_tool_sched a, ");
                sbsql.Append(" tool_sched_sub b where a.made_for = b.tool_code and part_no = '" + sqlEncode(partNo) + "'");
                sbsql.Append(" and  B.route_no = '" + routeNo + "' and B.seq_no = '" + seqNo + "'");
                sbsql.Append("  and B.cc_sno = '" + ccSno + "'");
                dtData = ToDataTable(DB.ExecuteQuery<AuxToolScheduleReportModel>(sbsql.ToString()).ToList());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dtData;
        }

        private string sqlEncode(string sqlValue)
        {
            return sqlValue.Replace("'", "''").Trim();
        }

        public void CopyToolScheduleMain(ToolScheduleModel tsm, decimal oldHeadingNo, decimal newHeadingNo)
        {
            TOOL_SCHED_MAIN oldentity = new TOOL_SCHED_MAIN();
            TOOL_SCHED_MAIN newentity = new TOOL_SCHED_MAIN();
            try
            {
                oldentity = (from row in DB.TOOL_SCHED_MAIN
                             where
                             row.PART_NO == tsm.PartNo.ToValueAsString().Trim()
                             && row.ROUTE_NO == tsm.RouteNo.ToDecimalValue()
                             && row.SEQ_NO == tsm.SeqNo.ToDecimalValue()
                             && row.CC_SNO == tsm.CCSno.ToDecimalValue()
                             && row.SUB_HEADING_NO == oldHeadingNo
                             select row).FirstOrDefault();
                if (oldentity != null)
                {
                    newentity.PART_NO = oldentity.PART_NO;
                    newentity.ROUTE_NO = oldentity.ROUTE_NO;
                    newentity.SEQ_NO = oldentity.SEQ_NO;
                    newentity.CC_SNO = oldentity.CC_SNO;
                    newentity.SUB_HEADING_NO = newHeadingNo;
                    newentity.SUB_HEADING = oldentity.SUB_HEADING;
                    newentity.BOT_NOTE = oldentity.BOT_NOTE;
                    newentity.TOP_NOTE = oldentity.TOP_NOTE;
                    newentity.ROWID = Guid.NewGuid();
                    newentity.ENTERED_BY = userName;
                    newentity.ENTERED_DATE = serverDateTime;
                    SaveToolSchedMain(newentity);
                }
                else
                {
                    newentity.PART_NO = tsm.PartNo.ToValueAsString().Trim();
                    newentity.ROUTE_NO = tsm.RouteNo.ToDecimalValue();
                    newentity.SEQ_NO = tsm.SeqNo.ToDecimalValue();
                    newentity.CC_SNO = tsm.CCSno.ToDecimalValue();
                    newentity.SUB_HEADING_NO = newHeadingNo;
                    newentity.SUB_HEADING = "";
                    newentity.BOT_NOTE = tsm.BotNote.ToValueAsString().Trim();
                    newentity.TOP_NOTE = tsm.TopNote.ToValueAsString().Trim();
                    newentity.ROWID = Guid.NewGuid();
                    newentity.ENTERED_BY = userName;
                    newentity.ENTERED_DATE = serverDateTime;
                    SaveToolSchedMain(newentity);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool CopyToolScheduleSub(ToolScheduleModel tsm, decimal oldHeadingNo, decimal newHeadingNo)
        {
            List<TOOL_SCHED_SUB> lsttool = new List<TOOL_SCHED_SUB>();
            bool submit = false;
            bool save = false;
            try
            {
                lsttool = (from row in DB.TOOL_SCHED_SUB
                           where row.PART_NO.ToUpper().Trim() == tsm.PartNo.ToValueAsString().Trim()
                                      && row.ROUTE_NO == tsm.RouteNo.ToDecimalValue()
                                      && row.SEQ_NO == tsm.SeqNo.ToDecimalValue()
                                      && row.CC_SNO == tsm.CCSno.ToDecimalValue()
                                      && row.SUB_HEADING_NO == oldHeadingNo
                           select row).ToList<TOOL_SCHED_SUB>();
                //newHeadingNo = tsm.SubHeadingNo.ToDecimalValue() + 1;
                foreach (TOOL_SCHED_SUB entity in lsttool)
                {
                    entity.SUB_HEADING_NO = newHeadingNo;
                }
                submit = true;
                DB.SubmitChanges();
                save = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (lsttool.Count > 0)
                    {
                        DB.TOOL_SCHED_SUB.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lsttool);
                    }
                }
                throw ex.LogException();
            }
            return save;
        }

        public bool DeleteToolScheduleMain(ToolScheduleModel tsm, decimal oldSubHeadingNo)
        {
            TOOL_SCHED_MAIN entity = null;
            bool submit = false;
            bool save = false;
            try
            {
                entity = (from row in DB.TOOL_SCHED_MAIN
                          where row.PART_NO.ToUpper().Trim() == tsm.PartNo.ToValueAsString().Trim()
                                     && row.ROUTE_NO == tsm.RouteNo.ToDecimalValue()
                                     && row.SEQ_NO == tsm.SeqNo.ToDecimalValue()
                                     && row.CC_SNO == tsm.CCSno.ToDecimalValue()
                                     && row.SUB_HEADING_NO == oldSubHeadingNo
                          select row).FirstOrDefault<TOOL_SCHED_MAIN>();
                if (entity != null)
                {
                    DB.TOOL_SCHED_MAIN.DeleteOnSubmit(entity);
                }
                submit = true;
                DB.SubmitChanges();
                save = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (entity != null)
                    {
                        DB.TOOL_SCHED_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, entity);
                    }
                }
                throw ex.LogException();
            }
            return save;
        }
    }
}
