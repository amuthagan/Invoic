using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;

namespace ProcessDesigner.ViewModel
{
    public class CopyProcess : Essential
    {
        public CopyProcess(UserInformation userinfo)
        {
            this.userInformation = userinfo;
        }

        public string ApplicationTitle = "SmartPD";

        public int CopyProcessSheet(string oldPartNo, string newPartNo, int oldRouteNo, int newRouteNo, decimal oldSeqNo, decimal newSeqNo, bool isReNumber = false)
        {

            bool varOverWrite = false;
            bool varAppend = false;

            List<PROCESS_SHEET> oldPS = (from o in DB.PROCESS_SHEET
                                         where o.PART_NO == oldPartNo && o.ROUTE_NO == ((oldRouteNo != 0) ? oldRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo != 0) ? oldSeqNo : o.SEQ_NO)
                                         select o).ToList<PROCESS_SHEET>();

            List<PROCESS_SHEET> newPS = (from o in DB.PROCESS_SHEET
                                         where o.PART_NO == newPartNo && o.ROUTE_NO == ((newRouteNo != 0) ? newRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo != 0) ? newSeqNo : o.SEQ_NO)
                                         select o).ToList<PROCESS_SHEET>();

            if (oldPS.Count == 0) return 0;

            foreach (PROCESS_SHEET nps in newPS)
            {
                foreach (PROCESS_SHEET ops in oldPS)
                {
                    if (nps.ROUTE_NO == ops.ROUTE_NO && nps.SEQ_NO == ops.SEQ_NO)
                    {
                        varOverWrite = true;
                        break;
                    }
                    else
                    {
                        varAppend = true;
                    }
                }


                if (varOverWrite == true)
                {
                    varAppend = false;
                    break;
                }
            }

            if (newPS.Count != 0)
            {
                if (varAppend == true)
                {
                    if (isReNumber != true && MessageBox.Show(newPS.Count + PDMsg.AlreadyExists("Process Sheet details") + ". \nDo you want to Append? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return 0;
                    }
                }
                else if (varOverWrite == true)
                {
                    if (isReNumber != true && MessageBox.Show(newPS.Count + PDMsg.AlreadyExists("Process Sheet details") + ". \nDo you want to Overwrite? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        PROCESS_CC pcc = new PROCESS_CC();
                        PROCESS_SHEET ps = new PROCESS_SHEET();

                        if (!string.IsNullOrEmpty(newPartNo) && newRouteNo != 0 && newSeqNo == 0)
                        {
                            pcc = (from o in DB.PROCESS_CC
                                   where o.PART_NO == newPartNo && o.ROUTE_NO == newRouteNo
                                   select o).FirstOrDefault<PROCESS_CC>();

                            ps = (from o in DB.PROCESS_SHEET
                                  where o.PART_NO == newPartNo && o.ROUTE_NO == newRouteNo
                                  select o).FirstOrDefault<PROCESS_SHEET>();

                        }
                        else if (!string.IsNullOrEmpty(newPartNo) && newRouteNo != 0 && newSeqNo != 0)
                        {
                            pcc = (from o in DB.PROCESS_CC
                                   where o.PART_NO == newPartNo && o.ROUTE_NO == newRouteNo && o.SEQ_NO == newSeqNo
                                   select o).FirstOrDefault<PROCESS_CC>();

                            ps = (from o in DB.PROCESS_SHEET
                                  where o.PART_NO == newPartNo && o.ROUTE_NO == newRouteNo && o.SEQ_NO == newSeqNo
                                  select o).FirstOrDefault<PROCESS_SHEET>();

                        }
                        else if (!string.IsNullOrEmpty(newPartNo) && newRouteNo == 0)
                        {
                            pcc = (from o in DB.PROCESS_CC
                                   where o.PART_NO == newPartNo
                                   select o).FirstOrDefault<PROCESS_CC>();

                            ps = (from o in DB.PROCESS_SHEET
                                  where o.PART_NO == newPartNo
                                  select o).FirstOrDefault<PROCESS_SHEET>();

                        }

                        if (pcc != null)
                        {
                            DB.PROCESS_CC.DeleteOnSubmit(pcc);
                            DB.SubmitChanges();
                        }
                        pcc = null;

                        if (ps != null)
                        {
                            DB.PROCESS_SHEET.DeleteOnSubmit(ps);
                            DB.SubmitChanges();
                        }
                        ps = null;
                    }
                    else
                    {
                        return 2;
                    }
                }

            }

            foreach (PROCESS_SHEET ops in oldPS)
            {
                PROCESS_SHEET ps = new PROCESS_SHEET();
                try
                {

                    ps.PART_NO = newPartNo;
                    ps.ROUTE_NO = ops.ROUTE_NO;
                    ps.SEQ_NO = ops.SEQ_NO;

                    if (oldRouteNo != 0 & newRouteNo != 0)
                    {
                        ps.ROUTE_NO = newRouteNo;
                        if (oldSeqNo != 0 & newSeqNo != 0)
                        {
                            ps.SEQ_NO = newSeqNo;
                        }
                    }

                    ps.OPN_CD = ops.OPN_CD;
                    ps.OPN_DESC = ops.OPN_DESC;
                    ps.SETTING_TIME = ops.SETTING_TIME;
                    ps.EFFICIENCY = ops.EFFICIENCY;
                    ps.STAGE_WT = ops.STAGE_WT;
                    ps.TRANSPORT = ops.TRANSPORT;
                    ps.OUTPUT = ops.OUTPUT;
                    ps.FMEA_RISK = ops.FMEA_RISK;
                    ps.UNIT_OF_MEASURE = ops.UNIT_OF_MEASURE;
                    ps.GROSS_WEIGHT = ops.GROSS_WEIGHT;
                    ps.NET_WEIGHT = ops.NET_WEIGHT;
                    ps.ENTERED_DATE = serverDateTime;
                    ps.ENTERED_BY = userName;
                    ps.ROWID = Guid.NewGuid();


                    PROCESS_SHEET psExist = (from o in DB.PROCESS_SHEET
                                             where o.PART_NO == ps.PART_NO && o.ROUTE_NO == ps.ROUTE_NO && o.SEQ_NO == ps.SEQ_NO
                                             select o).FirstOrDefault<PROCESS_SHEET>();
                    if (psExist == null)
                    {
                        DB.PROCESS_SHEET.InsertOnSubmit(ps);
                        DB.SubmitChanges();
                    }

                    ps = null;
                }
                catch (System.Data.Linq.ChangeConflictException)
                {
                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                }
                catch (Exception ex)
                {
                    DB.PROCESS_SHEET.DeleteOnSubmit(ps);
                    throw ex.LogException();
                }
            }

            return 1;
        }

        public int RecordsCountProcessSheet(string oldPartNo, int oldRouteNo = 0, decimal oldSeqNo = 0, decimal oldCCSno = 0, decimal oldSH = 0)
        {
            try
            {
                int count = 0;

                List<PROCESS_SHEET> oldPS = (from o in DB.PROCESS_SHEET
                                             where o.PART_NO == oldPartNo && o.ROUTE_NO == ((oldRouteNo != 0) ? oldRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo != 0) ? oldSeqNo : o.SEQ_NO)
                                             select o).ToList<PROCESS_SHEET>();
                count = oldPS.Count;

                if (count > 0 && oldCCSno != 0)
                {
                    List<PROCESS_CC> oldPCC = (from o in DB.PROCESS_CC
                                               where o.PART_NO == oldPartNo && o.ROUTE_NO == ((oldRouteNo != 0) ? oldRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo != 0) ? oldSeqNo : o.SEQ_NO) && o.CC_SNO == ((oldCCSno != 0) ? oldCCSno : o.CC_SNO)
                                               select o).ToList<PROCESS_CC>();
                    count = oldPCC.Count;
                }

                return count;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public int RecordsCountToolSched(string oldPartNo, int oldRouteNo = 0, decimal oldSeqNo = 0, decimal oldCCSno = 0, decimal oldSH = 0)
        {
            try
            {
                int count = 0;

                List<TOOL_SCHED_MAIN> oldTSM = (from o in DB.TOOL_SCHED_MAIN
                                                where o.PART_NO == oldPartNo.ToString() && o.ROUTE_NO == ((oldRouteNo != 0) ? oldRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo != 0) ? oldSeqNo : o.SEQ_NO) && o.CC_SNO == (oldCCSno != 0 ? oldCCSno : o.CC_SNO) && o.SUB_HEADING_NO == ((oldSH != 0) ? oldSH : o.SUB_HEADING_NO)
                                                select o).ToList<TOOL_SCHED_MAIN>();
                count = oldTSM.Count;

                return count;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public int CopyProcessSheet_PCCS(string oldPartNo, string newPartNo, int oldRouteNo, int newRouteNo, decimal oldSeqNo, decimal newSeqNo)
        {

            bool varAppend = false;

            List<PCCS> oldPCCS = (from o in DB.PCCS
                                  where o.PART_NO == oldPartNo && o.ROUTE_NO == ((oldRouteNo != 0) ? oldRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo != 0) ? oldSeqNo : o.SEQ_NO)
                                  select o).ToList<PCCS>();

            List<PCCS> newPCCS = (from o in DB.PCCS
                                  where o.PART_NO == newPartNo && o.ROUTE_NO == ((newRouteNo != 0) ? newRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo != 0) ? newSeqNo : o.SEQ_NO)
                                  select o).ToList<PCCS>();

            if (oldPCCS.Count == 0) return 0;

            foreach (PCCS nps in newPCCS)
            {
                foreach (PCCS ops in oldPCCS)
                {
                    if (nps.ROUTE_NO == ops.ROUTE_NO && nps.FEATURE == ops.FEATURE)
                    {
                        PCCS pc = new PCCS();
                        pc = (from o in DB.PCCS
                              where o.PART_NO == newPartNo && o.ROUTE_NO == nps.ROUTE_NO && o.FEATURE == nps.FEATURE
                              select o).FirstOrDefault<PCCS>();

                        if (pc != null)
                        {
                            DB.PCCS.DeleteOnSubmit(pc);
                            DB.SubmitChanges();
                        }
                        pc = null;

                        break;
                    }
                    else
                    {
                        varAppend = true;
                    }
                }

            }

            if (newPCCS.Count != 0)
            {
                if (varAppend == true)
                {
                    if (MessageBox.Show(newPCCS.Count + PDMsg.AlreadyExists("Control Plan details") + ". \nDo you want to Append? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return 0;
                    }
                }

            }

            foreach (PCCS opccs in oldPCCS)
            {
                PCCS pccs = new PCCS();
                try
                {

                    pccs.PART_NO = newPartNo;
                    pccs.ROUTE_NO = opccs.ROUTE_NO;
                    pccs.SEQ_NO = opccs.SEQ_NO;

                    if (oldRouteNo != 0 & newRouteNo != 0)
                    {
                        pccs.ROUTE_NO = newRouteNo;
                        if (oldSeqNo != 0 & newSeqNo != 0)
                        {
                            pccs.SEQ_NO = newSeqNo;
                        }
                    }

                    pccs.SNO = opccs.SNO;
                    pccs.ISR_NO = opccs.ISR_NO;
                    pccs.FEATURE = opccs.FEATURE;
                    pccs.CLASS = opccs.CLASS;
                    pccs.SPEC = opccs.SPEC;
                    pccs.CONTROL_SPEC = opccs.CONTROL_SPEC;
                    pccs.DEPT_RESP = opccs.DEPT_RESP;
                    pccs.FREQ_OF_INSP = opccs.FREQ_OF_INSP;
                    pccs.GAUGES_USED = opccs.GAUGES_USED;
                    pccs.GAUGE_RR = opccs.GAUGE_RR;
                    pccs.INPROCESS_CONTROL = opccs.INPROCESS_CONTROL;
                    pccs.METHOD_OF_STUDY = opccs.METHOD_OF_STUDY;
                    pccs.SAMPLE_SIZE = opccs.SAMPLE_SIZE;
                    pccs.CP = opccs.CP;
                    pccs.CPK = opccs.CPK;
                    pccs.PRD_PROC_COMP = opccs.PRD_PROC_COMP;
                    pccs.FREQ_OF_STUDY = opccs.FREQ_OF_STUDY;
                    pccs.COMMENTS = opccs.COMMENTS;
                    pccs.SPEC_MIN = opccs.SPEC_MIN;
                    pccs.SPEC_MAX = opccs.SPEC_MAX;
                    pccs.CTRL_SPEC_MIN = opccs.CTRL_SPEC_MIN;
                    pccs.CTRL_SPEC_MAX = opccs.CTRL_SPEC_MAX;
                    pccs.CONTROL_METHOD = opccs.CONTROL_METHOD;
                    pccs.REACTION_PLAN = opccs.REACTION_PLAN;
                    pccs.PROCESS_FEATURE = opccs.PROCESS_FEATURE;
                    pccs.SPEC_CHAR = opccs.SPEC_CHAR;

                    pccs.ENTERED_DATE = serverDateTime;
                    pccs.ENTERED_BY = userName;

                    pccs.ROWID = Guid.NewGuid();

                    PCCS pccsExists = (from o in DB.PCCS
                                       where o.PART_NO == pccs.PART_NO && o.ROUTE_NO == pccs.ROUTE_NO && o.SEQ_NO == pccs.SEQ_NO
                                       select o).FirstOrDefault<PCCS>();
                    if (pccsExists == null)
                    {
                        DB.PCCS.InsertOnSubmit(pccs);
                        DB.SubmitChanges();
                    }

                    pccs = null;
                }
                catch (System.Data.Linq.ChangeConflictException)
                {
                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                }
                catch (Exception ex)
                {
                    DB.PCCS.DeleteOnSubmit(pccs);
                    throw ex.LogException();
                }
            }

            return 1;
        }

        public int CopyProcessCostCenter(string oldPartNo, string newPartNo, int oldRouteNo, int newRouteNo, decimal oldSeqNo, decimal newSeqNo, int oldCC = 0, int newCC = 0)
        {
            bool varOverWrite = false;
            bool varAppend = false;

            List<PROCESS_CC> oldPCC = (from o in DB.PROCESS_CC
                                       where o.PART_NO == oldPartNo && o.ROUTE_NO == ((oldRouteNo != 0) ? oldRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo != 0) ? oldSeqNo : o.SEQ_NO) && o.CC_SNO == ((oldCC != 0) ? oldCC : o.CC_SNO)
                                       select o).ToList<PROCESS_CC>();

            List<PROCESS_CC> newPCC = (from o in DB.PROCESS_CC
                                       where o.PART_NO == newPartNo && o.ROUTE_NO == ((newRouteNo != 0) ? newRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo != 0) ? newSeqNo : o.SEQ_NO) && o.CC_SNO == ((newCC != 0) ? newCC : o.CC_SNO)
                                       select o).ToList<PROCESS_CC>();

            if (oldPCC.Count == 0) return 0;

            foreach (PROCESS_CC npcc in newPCC)
            {
                foreach (PROCESS_CC opcc in oldPCC)
                {
                    if (npcc.ROUTE_NO == opcc.ROUTE_NO && npcc.SEQ_NO == opcc.SEQ_NO)
                    {
                        varOverWrite = true;
                        return 2;
                    }
                    else
                    {
                        varAppend = true;
                    }
                }


                if (varOverWrite == true)
                {
                    return 2;
                }
            }


            foreach (PROCESS_CC opcc in oldPCC)
            {
                PROCESS_CC pcc = new PROCESS_CC();
                try
                {

                    pcc.PART_NO = newPartNo;
                    pcc.ROUTE_NO = opcc.ROUTE_NO;
                    pcc.SEQ_NO = opcc.SEQ_NO;
                    pcc.CC_SNO = opcc.CC_SNO;

                    if (oldRouteNo != 0 & newRouteNo != 0)
                    {
                        pcc.ROUTE_NO = newRouteNo;
                        if (oldSeqNo != 0 & newSeqNo != 0)
                        {
                            pcc.SEQ_NO = newSeqNo;
                            if (oldCC != 0 & newCC != 0)
                            {
                                pcc.CC_SNO = newCC;
                            }
                        }
                    }

                    pcc.CC_CODE = opcc.CC_CODE;
                    pcc.WIRE_SIZE_MAX = opcc.WIRE_SIZE_MAX;
                    pcc.WIRE_SIZE_MIN = opcc.WIRE_SIZE_MIN;
                    pcc.TS_ISSUE_NO = opcc.TS_ISSUE_NO;
                    pcc.TS_ISSUE_DATE = opcc.TS_ISSUE_DATE;
                    pcc.TS_ISSUE_ALTER = opcc.TS_ISSUE_ALTER;
                    pcc.TS_COMPILED_BY = opcc.TS_COMPILED_BY;
                    pcc.TS_APPROVED_BY = opcc.TS_APPROVED_BY;
                    pcc.OUTPUT = opcc.OUTPUT;
                    //pcc.ENTERED_DATE = serverDateTime;
                    //pcc.ENTERED_BY = userName;

                    pcc.ROWID = Guid.NewGuid();

                    PROCESS_CC pccExists = (from o in DB.PROCESS_CC
                                            where o.PART_NO == pcc.PART_NO && o.ROUTE_NO == pcc.ROUTE_NO && o.SEQ_NO == pcc.SEQ_NO && o.CC_SNO == pcc.CC_SNO
                                            select o).FirstOrDefault<PROCESS_CC>();


                    PROCESS_SHEET psExists = (from o in DB.PROCESS_SHEET
                                              where o.PART_NO == pcc.PART_NO && o.ROUTE_NO == pcc.ROUTE_NO && o.SEQ_NO == pcc.SEQ_NO
                                              select o).FirstOrDefault<PROCESS_SHEET>();

                    if (pccExists == null && psExists != null)
                    {
                        DB.PROCESS_CC.InsertOnSubmit(pcc);
                        DB.SubmitChanges();
                    }
                    pcc = null;
                }
                catch (System.Data.Linq.ChangeConflictException)
                {
                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                }
                catch (Exception ex)
                {
                    DB.PROCESS_CC.DeleteOnSubmit(pcc);
                    throw ex.LogException();
                }
                pcc = null;
            }

            return 1;
        }

        public int CopyProductData_prdMaster(string oldPartNo, string newPartNo)
        {

            try
            {
                int varOverWrite;
                int copyProductData;

                //DataTable olddataValue;

                List<PRD_MAST> olddataValue = (from o in DB.PRD_MAST
                                               where o.PART_NO == oldPartNo.ToString()
                                               select o).ToList<PRD_MAST>();

                List<PRD_MAST> newdataValue = (from o in DB.PRD_MAST
                                               where o.PART_NO == newPartNo.ToString()
                                               select o).ToList<PRD_MAST>();



                if (newdataValue.Count > 0)
                {
                    if (MessageBox.Show("Product Master details already exists. \nDo you want to Overwrite? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (newdataValue != null)
                        {
                            PRD_MAST pm = new PRD_MAST();
                            pm = (from o in DB.PRD_MAST
                                  where o.PART_NO == newPartNo
                                  select o).FirstOrDefault<PRD_MAST>();

                            if (pm.PRD_DWG_ISSUE.Count > 0) pm.PRD_DWG_ISSUE.Clear();

                            DB.PRD_MAST.DeleteOnSubmit(pm);
                            DB.SubmitChanges();
                        }
                        varOverWrite = 1;

                    }
                    else
                    {
                        copyProductData = 2;
                        return copyProductData;
                    }
                }
                foreach (PRD_MAST ops in olddataValue)
                {
                    PRD_MAST ps = new PRD_MAST();
                    try
                    {
                        ps.PART_NO = newPartNo;
                        ps.PART_DESC = ops.PART_DESC;
                        ps.SIM_TO_STD_CD = ops.SIM_TO_STD_CD;
                        ps.PRD_CLASS_CD = ops.PRD_CLASS_CD;
                        ps.MFG_STD = ops.MFG_STD;
                        ps.ED_CD = ops.ED_CD;
                        ps.THREAD_CD = ops.THREAD_CD;
                        ps.DIA_CD = ops.DIA_CD;
                        ps.QUALITY = ops.QUALITY;
                        ps.BIF_PROJ = ops.BIF_PROJ;
                        ps.FINISH_WT = ops.FINISH_WT;
                        ps.FINISH_WT_EST = ops.FINISH_WT_EST;
                        ps.HEAT_TREATMENT_CD = ops.HEAT_TREATMENT_CD;
                        ps.HEAT_TREATMENT_DESC = ops.HEAT_TREATMENT_DESC;
                        ps.PRD_GRP_CD = ops.PRD_GRP_CD;
                        ps.MACHINE_CD = ops.MACHINE_CD;
                        ps.DOC_REL_DATE = null; //ops.DOC_REL_DATE;
                        ps.DOC_REL_REMARKS = ops.DOC_REL_REMARKS;
                        ps.FAMILY = ops.FAMILY;
                        ps.HEAD_STYLE = ops.HEAD_STYLE;
                        ps.TYPE = ops.TYPE;
                        ps.APPLICATION = ops.APPLICATION;
                        ps.USER_CD = ops.USER_CD;
                        ps.THREAD_CLASS = ops.THREAD_CLASS;
                        ps.THREAD_STD = ops.THREAD_STD;
                        ps.PG_CATEGORY = ops.PG_CATEGORY;
                        ps.NOS_PER_PACK = ops.NOS_PER_PACK;
                        ps.SAMP_SUBMIT_DATE = ops.SAMP_SUBMIT_DATE;
                        ps.PSW_ST = ops.PSW_ST;
                        ps.DOC_REL_TYPE = ops.DOC_REL_TYPE;
                        ps.HOLD_ME = ops.HOLD_ME;
                        ps.ADDL_FEATURE = ops.ADDL_FEATURE;
                        ps.KEYWORDS = ops.KEYWORDS;
                        ps.CANCEL_STATUS = ops.CANCEL_STATUS;

                        ps.ALLOT_DATE = userInformation.Dal.ServerDateTime;
                        ps.ALLOTTED_BY = userInformation.UserName;

                        ps.ROWID = Guid.NewGuid();
                        DB.PRD_MAST.InsertOnSubmit(ps);
                        DB.SubmitChanges();
                        ps = null;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        DB.PRD_MAST.DeleteOnSubmit(ps);
                        throw ex.LogException();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public int CopyProductData_Process_main(string oldPartNo, string newPartNo, string oldRouteNo, string newRouteNo)
        {

            try
            {
                int varOverWrite;
                int copyProductData;

                //DataTable olddataValue;

                List<PROCESS_MAIN> olddataValue = (from o in DB.PROCESS_MAIN
                                                   where o.PART_NO == oldPartNo.ToString() && o.ROUTE_NO == ((oldRouteNo.ToIntValue() != 0) ? oldRouteNo.ToIntValue() : o.ROUTE_NO)
                                                   select o).ToList<PROCESS_MAIN>();

                List<PROCESS_MAIN> newdataValue = (from o in DB.PROCESS_MAIN
                                                   where o.PART_NO == newPartNo.ToString() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO)
                                                   select o).ToList<PROCESS_MAIN>();



                if (newdataValue.Count > 0)
                {
                    if (MessageBox.Show("Process Sheet Header details already exists. \nDo you want to Overwrite? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (newdataValue != null)
                        {
                            PROCESS_MAIN pm = new PROCESS_MAIN();
                            pm = (from o in DB.PROCESS_MAIN
                                  where o.PART_NO == newPartNo.ToString() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO)
                                  select o).FirstOrDefault<PROCESS_MAIN>();

                            DB.PROCESS_MAIN.DeleteOnSubmit(pm);
                            DB.SubmitChanges();
                        }
                        varOverWrite = 1;

                    }
                    else
                    {
                        copyProductData = 2;
                        return copyProductData;
                    }
                }
                PROCESS_MAIN pro_main;
                foreach (PROCESS_MAIN opm in olddataValue)
                {
                    pro_main = new PROCESS_MAIN();
                    try
                    {
                        pro_main.PART_NO = newPartNo;
                        pro_main.ROUTE_NO = opm.ROUTE_NO;

                        if (oldRouteNo.ToIntValue() != 0 & newRouteNo.ToIntValue() != 0)
                        {
                            pro_main.ROUTE_NO = newRouteNo.ToDecimalValue();
                        }

                        pro_main.CURRENT_PROC = 0;
                        pro_main.RM_CD = opm.RM_CD;
                        pro_main.ALT_RM_CD = opm.ALT_RM_CD;
                        pro_main.ALT_RM_CD1 = opm.ALT_RM_CD1;
                        pro_main.RM_WT = opm.RM_WT;
                        pro_main.CHEESE_WT = opm.CHEESE_WT;
                        pro_main.CHEESE_WT_EST = opm.CHEESE_WT_EST;
                        pro_main.WIRE_ROD_CD = opm.WIRE_ROD_CD;
                        pro_main.ALT_WIRE_ROD_CD = opm.ALT_WIRE_ROD_CD;
                        pro_main.ALT_WIRE_ROD_CD1 = opm.ALT_WIRE_ROD_CD1;
                        pro_main.TKO_CD = opm.TKO_CD;
                        pro_main.AJAX_CD = opm.AJAX_CD;
                        pro_main.ACTIVE_PART = opm.ACTIVE_PART;

                        List<PROCESS_MAIN> validatePMain = (from o in DB.PROCESS_MAIN
                                                            where o.PART_NO == pro_main.PART_NO && o.ROUTE_NO == pro_main.ROUTE_NO
                                                            select o).ToList<PROCESS_MAIN>();
                        if (validatePMain.Count == 0)
                        {
                            pro_main.ROWID = Guid.NewGuid();
                            DB.PROCESS_MAIN.InsertOnSubmit(pro_main);
                            DB.SubmitChanges();
                        }
                        pro_main = null;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        DB.PROCESS_MAIN.DeleteOnSubmit(pro_main);
                        throw ex.LogException();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public int CopyProductData_Process_sheet(string oldPartNo, string newPartNo, string oldRouteNo, string newRouteNo, string oldSeqNo, string newSeqNo)
        {
            try
            {
                return CopyProcessSheet(oldPartNo, newPartNo, oldRouteNo.ToIntValue(), newRouteNo.ToIntValue(), oldSeqNo.ToDecimalValue(), newSeqNo.ToDecimalValue(), false);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public int CopyProductData_Tool_Sched_Sub(string oldPartNo, string newPartNo, string oldRouteNo, string newRouteNo, string oldSeqNo, string newSeqNo, string oldCC = "0", string newCC = "0", string oldSH = "", string newSH = "")
        {
            bool varOverWrite = false;
            bool varAppend = false;
            bool submit = false;
            bool insert = false;
            int dataCopied = 0;
            TOOL_SCHED_SUB newnts = new TOOL_SCHED_SUB();
            try
            {
                newSH = newSH.ToValueAsString().Trim();
                oldSH = oldSH.ToValueAsString().Trim();
                List<TOOL_SCHED_SUB> olddataValue = (from o in DB.TOOL_SCHED_SUB
                                                     where o.PART_NO == oldPartNo.ToString() && o.ROUTE_NO == ((oldRouteNo.ToIntValue() != 0) ? oldRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo.ToDecimalValue() != 0) ? oldSeqNo.ToDecimalValue() : o.SEQ_NO) && o.CC_SNO == ((oldCC.ToIntValue() != 0) ? oldCC.ToIntValue() : o.CC_SNO) && o.SUB_HEADING_NO == ((oldSH.Trim() != "") ? oldSH.ToIntValue() : o.SUB_HEADING_NO)
                                                     orderby o.SNO
                                                     select o).ToList<TOOL_SCHED_SUB>();

                List<TOOL_SCHED_SUB> newdataValue = (from o in DB.TOOL_SCHED_SUB
                                                     where o.PART_NO == newPartNo.ToString() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo.ToDecimalValue() != 0) ? newSeqNo.ToDecimalValue() : o.SEQ_NO) && o.CC_SNO == ((newCC.ToIntValue() != 0) ? newCC.ToIntValue() : o.CC_SNO) && o.SUB_HEADING_NO == (newSH.Trim() != "" ? newSH.ToIntValue() : o.SUB_HEADING_NO)
                                                     orderby o.SNO
                                                     select o).ToList<TOOL_SCHED_SUB>();
                DataTable dataValue;
                dataValue = ToDataTable((from o in DB.TOOL_SCHED_SUB
                                         where o.PART_NO == newPartNo.ToString() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo.ToDecimalValue() != 0) ? newSeqNo.ToDecimalValue() : o.SEQ_NO) && o.CC_SNO == ((newCC.ToIntValue() != 0) ? newCC.ToIntValue() : o.CC_SNO) && o.SUB_HEADING_NO == ((newSH != "") ? newSH.ToIntValue() : o.SUB_HEADING_NO)
                                         orderby o.SNO descending
                                         select new { o.SNO }).ToList());
                int maxValue = 0;

                newSH = newSH.ToValueAsString().Trim();
                oldSH = oldSH.ToValueAsString().Trim();

                if (dataValue.Rows.Count > 0)
                {
                    maxValue = dataValue.Rows[0]["SNO"].ToValueAsString().ToIntValue();
                }


                //tool_sched_sub
                if (olddataValue.Count == 0) return 0;
                //if (newdataValue.Count > 0) return 2;

                foreach (TOOL_SCHED_SUB nts in newdataValue)
                {
                    foreach (TOOL_SCHED_SUB ots in olddataValue)
                    {
                        if (nts.ROUTE_NO == ots.ROUTE_NO && nts.SEQ_NO == ots.SEQ_NO && nts.CC_SNO == ots.CC_SNO && nts.SUB_HEADING_NO == ots.SUB_HEADING_NO && nts.TOOL_CODE == ots.TOOL_CODE)
                        {
                            varOverWrite = true;
                            break;
                        }
                        else
                        {
                            varAppend = true;
                        }
                    }

                    if (varOverWrite == true)
                    {
                        varAppend = false;
                        break;
                    }
                }

                if (varOverWrite == true)
                {
                    if (ShowInformationMessageInput("Tool Schedule details already Exists.\n Do you want to Overwrite?") == MessageBoxResult.Yes)
                    {
                        List<TOOL_SCHED_SUB> delschedsub = (from o in DB.TOOL_SCHED_SUB
                                                            where o.PART_NO == newPartNo.ToString().Trim() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo.ToDecimalValue() != 0) ? newSeqNo.ToDecimalValue() : o.SEQ_NO) && o.CC_SNO == ((newCC.ToIntValue() != 0) ? newCC.ToIntValue() : o.CC_SNO) && o.SUB_HEADING_NO == ((newSH.Trim() != "") ? newSH.ToIntValue() : o.SUB_HEADING_NO)
                                                            select o).ToList<TOOL_SCHED_SUB>();
                        DB.TOOL_SCHED_SUB.DeleteAllOnSubmit(delschedsub);
                        //DB.SubmitChanges();
                    }
                    else
                    {
                        return 2;
                    }
                }
                else if (varAppend == true)
                {
                    if (ShowInformationMessageInput("Tool Schedule details already Exists. \n" + "Do you want to append?") == MessageBoxResult.No)
                    {
                        return 2;
                    }
                }

                foreach (TOOL_SCHED_SUB nts in olddataValue)
                {
                    try
                    {
                        newnts = new TOOL_SCHED_SUB();
                        newnts.CATEGORY = nts.CATEGORY;
                        newnts.CC_SNO = nts.CC_SNO;
                        newnts.IDPK = 0;
                        newnts.PART_NO = newPartNo;
                        newnts.QTY = nts.QTY;
                        newnts.REMARKS = nts.REMARKS;
                        newnts.ROUTE_NO = nts.ROUTE_NO;
                        newnts.ROWID = Guid.NewGuid();
                        newnts.SEQ_NO = nts.SEQ_NO;
                        newnts.SUB_HEADING_NO = nts.SUB_HEADING_NO;
                        newnts.TOOL_CODE = nts.TOOL_CODE;
                        newnts.TOOL_CODE_END = nts.TOOL_CODE_END;
                        newnts.TOOL_DESC = nts.TOOL_DESC;
                        if (oldRouteNo.ToIntValue() != 0 && newRouteNo.ToIntValue() != 0)
                        {
                            newnts.ROUTE_NO = newRouteNo.ToDecimalValue();
                            if (oldSeqNo.ToDecimalValue() != 0 && newSeqNo.ToDecimalValue() != 0)
                            {
                                newnts.SEQ_NO = newSeqNo.ToDecimalValue();
                                if (oldCC.ToIntValue() != 0 && newCC.ToIntValue() != 0)
                                {
                                    newnts.CC_SNO = newCC.ToDecimalValue();
                                    if (oldSH != "" && newSH != "")
                                    {
                                        newnts.SUB_HEADING_NO = newSH.ToDecimalValue();
                                    }
                                }
                            }
                        }
                        List<TOOL_SCHED_SUB> validatesub = (from o in DB.TOOL_SCHED_SUB
                                                            where o.PART_NO == newnts.PART_NO && o.ROUTE_NO == newnts.ROUTE_NO && o.SEQ_NO == newnts.SEQ_NO && o.CC_SNO == newnts.CC_SNO && o.SUB_HEADING_NO == newnts.SUB_HEADING_NO
                                                            && o.TOOL_CODE.Trim() == newnts.TOOL_CODE
                                                            select o).ToList<TOOL_SCHED_SUB>();

                        List<TOOL_SCHED_MAIN> validatemain = (from o in DB.TOOL_SCHED_MAIN
                                                              where o.PART_NO == newnts.PART_NO && o.ROUTE_NO == newnts.ROUTE_NO && o.SEQ_NO == newnts.SEQ_NO && o.CC_SNO == newnts.CC_SNO && o.SUB_HEADING_NO == newnts.SUB_HEADING_NO
                                                              select o).ToList<TOOL_SCHED_MAIN>();

                        if (validatesub.Count == 0 && validatemain.Count > 0)
                        {
                            //if (newdataValue.Count > 0)
                            //{
                            //    maxValue = maxValue + 1;
                            //    newnts.SNO = maxValue;
                            //}
                            //else
                            //{
                            newnts.SNO = nts.SNO;
                            //}
                            insert = true;
                            DB.TOOL_SCHED_SUB.InsertOnSubmit(newnts);
                            submit = true;
                            DB.SubmitChanges();
                            dataCopied = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (submit == true)
                        {
                            if (insert == true)
                            {
                                ex.LogException();
                                DB.TOOL_SCHED_SUB.DeleteOnSubmit(newnts);
                            }
                        }
                    }
                }
                return dataCopied;
            }
            catch (Exception ex)
            {

                throw ex.LogException();
            }
        }

        public int CopyProductData_Tool_Sched_Main(string oldPartNo, string newPartNo, string oldRouteNo, string newRouteNo, string oldSeqNo, string newSeqNo, string oldCC = "0", string newCC = "0", string oldSH = "", string newSH = "")
        {
            bool varOverWrite = false;
            bool varAppend = false;
            bool submit = false;
            bool insert = false;
            TOOL_SCHED_MAIN newnts = new TOOL_SCHED_MAIN();
            int maxValue = 0;
            int dataCopied = 0;
            try
            {
                newSH = newSH.ToValueAsString().Trim();
                oldSH = oldSH.ToValueAsString().Trim();

                List<TOOL_SCHED_MAIN> olddataValue = (from o in DB.TOOL_SCHED_MAIN
                                                      where o.PART_NO == oldPartNo.ToString() && o.ROUTE_NO == ((oldRouteNo.ToIntValue() != 0) ? oldRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo.ToDecimalValue() != 0) ? oldSeqNo.ToDecimalValue() : o.SEQ_NO) && o.CC_SNO == (oldCC.ToIntValue() != 0 ? oldCC.ToIntValue() : o.CC_SNO) && o.SUB_HEADING_NO == ((oldSH != "") ? oldSH.ToIntValue() : o.SUB_HEADING_NO)
                                                      select o).ToList<TOOL_SCHED_MAIN>();

                List<TOOL_SCHED_MAIN> newdataValue = (from o in DB.TOOL_SCHED_MAIN
                                                      where o.PART_NO == newPartNo.ToString() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo.ToDecimalValue() != 0) ? newSeqNo.ToDecimalValue() : o.SEQ_NO) && o.CC_SNO == ((newCC.ToIntValue() != 0) ? newCC.ToIntValue() : o.CC_SNO) && o.SUB_HEADING_NO == ((newSH != "") ? newSH.ToIntValue() : o.SUB_HEADING_NO)
                                                      select o).ToList<TOOL_SCHED_MAIN>();

                //tool_sched_main
                if (olddataValue.Count == 0) return 0;
                //if (newdataValue.Count > 0) return 2;

                foreach (TOOL_SCHED_MAIN nts in newdataValue)
                {
                    foreach (TOOL_SCHED_MAIN ots in olddataValue)
                    {
                        if (nts.ROUTE_NO == ots.ROUTE_NO && nts.SEQ_NO == ots.SEQ_NO && nts.CC_SNO == ots.CC_SNO && nts.SUB_HEADING_NO == ots.SUB_HEADING_NO)
                        {
                            varOverWrite = true;
                            break;
                        }
                        else
                        {
                            varAppend = true;
                        }
                    }

                    if (varOverWrite == true)
                    {
                        varAppend = false;
                        break;
                    }
                }

                foreach (TOOL_SCHED_MAIN nts in olddataValue)
                {
                    try
                    {
                        newnts = new TOOL_SCHED_MAIN();
                        newnts.PART_NO = newPartNo;
                        newnts.ROWID = Guid.NewGuid();
                        newnts.ROUTE_NO = nts.ROUTE_NO;
                        newnts.SEQ_NO = nts.SEQ_NO;
                        newnts.CC_SNO = nts.CC_SNO;
                        newnts.SUB_HEADING_NO = nts.SUB_HEADING_NO;
                        newnts.SUB_HEADING = nts.SUB_HEADING;
                        newnts.TOP_NOTE = nts.TOP_NOTE;
                        newnts.BOT_NOTE = nts.BOT_NOTE;
                        if (oldRouteNo.ToIntValue() != 0 && newRouteNo.ToIntValue() != 0)
                        {
                            newnts.ROUTE_NO = newRouteNo.ToDecimalValue();
                            if (oldSeqNo.ToDecimalValue() != 0 && newSeqNo.ToDecimalValue() != 0)
                            {
                                newnts.SEQ_NO = newSeqNo.ToDecimalValue();
                                if (oldCC.ToIntValue() != 0 && newCC.ToIntValue() != 0)
                                {
                                    newnts.CC_SNO = newCC.ToDecimalValue();
                                    if (oldSH.ToIntValue() != 0 && newSH.ToIntValue() != 0)
                                    {
                                        newnts.SUB_HEADING_NO = newSH.ToDecimalValue();
                                        newnts.SUB_HEADING = nts.SUB_HEADING;
                                    }
                                }
                            }
                        }
                        newnts.ENTERED_BY = userInformation.UserName;
                        newnts.ENTERED_DATE = serverDateTime;
                        newnts.UPDATED_BY = null;
                        newnts.UPDATED_DATE = null;
                        insert = true;

                        List<TOOL_SCHED_MAIN> validatemain = (from o in DB.TOOL_SCHED_MAIN
                                                              where o.PART_NO == newnts.PART_NO && o.ROUTE_NO == newnts.ROUTE_NO && o.SEQ_NO == newnts.SEQ_NO && o.CC_SNO == newnts.CC_SNO && o.SUB_HEADING_NO == newnts.SUB_HEADING_NO
                                                              select o).ToList<TOOL_SCHED_MAIN>();

                        if (validatemain.Count == 0)
                        {
                            List<PROCESS_CC> newPS = (from o in DB.PROCESS_CC
                                                      where o.PART_NO == newPartNo && o.ROUTE_NO == newnts.ROUTE_NO && o.SEQ_NO == newnts.SEQ_NO
                                                      && o.CC_SNO == newnts.CC_SNO
                                                      select o).ToList<PROCESS_CC>();
                            if (newPS.Count > 0)
                            {
                                DB.TOOL_SCHED_MAIN.InsertOnSubmit(newnts);
                                submit = true;
                                DB.SubmitChanges();
                                dataCopied = 1;
                            }
                            /*
                            else
                            {
                                //ShowInformationMessage("The select data is not available in Process Sheet");
                                return 0;
                            }
                            */
                        }
                    }
                    catch (Exception ex)
                    {
                        if (submit == true)
                        {
                            if (insert == true)
                            {
                                ex.LogException();
                                DB.TOOL_SCHED_MAIN.DeleteOnSubmit(newnts);
                            }
                        }
                    }
                }
                return dataCopied;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex.LogException();
            }

        }

        public int CopyToolSchedule_TOOL_SCHED_ISSUE(string oldPartNo, string newPartNo, int oldRouteNo, int newRouteNo, decimal oldSeqNo, decimal newSeqNo, int oldCC = 0, int newCC = 0)
        {
            int dataCopied = 0;
            List<TOOL_SCHED_ISSUE> oldTSI = (from o in DB.TOOL_SCHED_ISSUE
                                             where o.PART_NO == oldPartNo && o.ROUTE_NO == ((oldRouteNo != 0) ? oldRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo != 0) ? oldSeqNo : o.SEQ_NO) && o.CC_SNO == ((oldCC != 0) ? oldCC : o.CC_SNO)
                                             select o).ToList<TOOL_SCHED_ISSUE>();

            List<TOOL_SCHED_ISSUE> newTSI = (from o in DB.TOOL_SCHED_ISSUE
                                             where o.PART_NO == newPartNo && o.ROUTE_NO == ((newRouteNo != 0) ? newRouteNo : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo != 0) ? newSeqNo : o.SEQ_NO) && o.CC_SNO == ((newCC != 0) ? newCC : o.CC_SNO)
                                             select o).ToList<TOOL_SCHED_ISSUE>();

            if (oldTSI.Count == 0) return 0;
            if (newTSI.Count > 0) return 0;

            foreach (TOOL_SCHED_ISSUE oTSI in oldTSI)
            {
                TOOL_SCHED_ISSUE tsi = new TOOL_SCHED_ISSUE();
                try
                {

                    tsi.PART_NO = newPartNo;
                    tsi.ROUTE_NO = oTSI.ROUTE_NO;
                    tsi.SEQ_NO = oTSI.SEQ_NO;

                    if (oldRouteNo != 0 & newRouteNo != 0)
                    {
                        tsi.ROUTE_NO = newRouteNo;
                        if (oldSeqNo != 0 & newSeqNo != 0)
                        {
                            tsi.SEQ_NO = newSeqNo;
                        }
                    }

                    tsi.CC_SNO = oTSI.CC_SNO;
                    tsi.TS_ISSUE_NO = oTSI.TS_ISSUE_NO;
                    tsi.TS_ISSUE_DATE = oTSI.TS_ISSUE_DATE;
                    tsi.TS_ISSUE_ALTER = oTSI.TS_ISSUE_ALTER;
                    tsi.TS_COMPILED_BY = oTSI.TS_COMPILED_BY;
                    tsi.TS_APPROVED_BY = oTSI.TS_APPROVED_BY;
                    tsi.IDPK = 0;

                    //tsi.ENTERED_DATE = serverDateTime;
                    //tsi.ENTERED_BY = userName;

                    tsi.ROWID = Guid.NewGuid();

                    TOOL_SCHED_ISSUE tsiExists = (from o in DB.TOOL_SCHED_ISSUE
                                                  where o.PART_NO == tsi.PART_NO && o.ROUTE_NO == tsi.ROUTE_NO && o.SEQ_NO == tsi.SEQ_NO && o.CC_SNO == tsi.CC_SNO
                                                  select o).FirstOrDefault<TOOL_SCHED_ISSUE>();

                    PROCESS_CC pccExists = (from o in DB.PROCESS_CC
                                            where o.PART_NO == tsi.PART_NO && o.ROUTE_NO == tsi.ROUTE_NO && o.SEQ_NO == tsi.SEQ_NO
                                            select o).FirstOrDefault<PROCESS_CC>();

                    if (tsiExists == null && pccExists != null)
                    {
                        DB.TOOL_SCHED_ISSUE.InsertOnSubmit(tsi);
                        DB.SubmitChanges();
                        dataCopied = 1;
                    }
                    tsi = null;
                }
                catch (System.Data.Linq.ChangeConflictException)
                {
                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                }
                catch (Exception ex)
                {
                    DB.TOOL_SCHED_ISSUE.DeleteOnSubmit(tsi);
                    throw ex.LogException();
                }
            }

            return dataCopied;
        }

        public int CopyProductData_PCCS(string oldPartNo, string newPartNo, string oldRouteNo, string newRouteNo, string oldSeqNo, string newSeqNo)
        {
            try
            {
                bool varOverWrite = false;
                bool varAppend = false;
                List<PCCS> olddataValue = (from o in DB.PCCS
                                           where o.PART_NO == oldPartNo.ToString() && o.ROUTE_NO == ((oldRouteNo.ToIntValue() != 0) ? oldRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((oldSeqNo.ToDecimalValue() != 0) ? oldSeqNo.ToDecimalValue() : o.SEQ_NO)
                                           select o).ToList<PCCS>();

                List<PCCS> newdataValue = (from o in DB.PCCS
                                           where o.PART_NO == newPartNo.ToString() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo.ToDecimalValue() != 0) ? newSeqNo.ToDecimalValue() : o.SEQ_NO)
                                           select o).ToList<PCCS>();

                if (olddataValue.Count == 0) return 0;

                foreach (PCCS nps in newdataValue)
                {
                    foreach (PCCS ops in olddataValue)
                    {
                        if (nps.ROUTE_NO == ops.ROUTE_NO && nps.SEQ_NO == ops.SEQ_NO)
                        {
                            varOverWrite = true;
                            break;
                        }
                        else
                        {
                            varAppend = true;
                        }
                    }


                    if (varOverWrite == true)
                    {
                        varAppend = false;
                        break;
                    }
                }

                if (newdataValue.Count != 0)
                {
                    if (varAppend == true)
                    {
                        if (MessageBox.Show(newdataValue.Count + " Control Plan details already exists. \nDo you want to Append? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            return 0;
                        }
                    }
                    else if (varOverWrite == true)
                    {
                        if (MessageBox.Show("Control Plan details already exists. \nDo you want to Overwrite? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            List<PCCS> pcc = new List<PCCS>();

                            if (!string.IsNullOrEmpty(newPartNo) && newRouteNo.ToIntValue() != 0 && newSeqNo.ToDecimalValue() == 0)
                            {
                                pcc = (from o in DB.PCCS
                                       where o.PART_NO == newPartNo && o.ROUTE_NO == newRouteNo.ToIntValue()
                                       select o).ToList<PCCS>();

                            }
                            else if (!string.IsNullOrEmpty(newPartNo) && newRouteNo.ToIntValue() != 0 && newSeqNo.ToDecimalValue() != 0)
                            {
                                pcc = (from o in DB.PCCS
                                       where o.PART_NO == newPartNo && o.ROUTE_NO == newRouteNo.ToIntValue() && o.SEQ_NO == newSeqNo.ToDecimalValue()
                                       select o).ToList<PCCS>();

                            }
                            else if (!string.IsNullOrEmpty(newPartNo) && newRouteNo.ToIntValue() == 0)
                            {
                                pcc = (from o in DB.PCCS
                                       where o.PART_NO == newPartNo
                                       select o).ToList<PCCS>();
                            }

                            if (pcc != null)
                            {
                                DB.PCCS.DeleteAllOnSubmit<PCCS>(pcc);
                                DB.SubmitChanges();
                            }
                            pcc = null;
                        }
                        else
                        {
                            return 2;
                        }
                    }

                }
                //if (varAppend == true)
                //{
                //    int i = 1;
                //    foreach (PCCS ops in olddataValue)
                //    {
                //        ops.SNO = newdataValue.Count + i;
                //    }
                //}

                foreach (PCCS ops in olddataValue)
                {
                    PCCS pccs = new PCCS();
                    try
                    {
                        pccs = new PCCS();
                        pccs.PART_NO = newPartNo;
                        pccs.ROUTE_NO = ops.ROUTE_NO;
                        pccs.SEQ_NO = ops.SEQ_NO;

                        if (oldRouteNo.ToIntValue() != 0 & newRouteNo.ToIntValue() != 0)
                        {
                            pccs.ROUTE_NO = newRouteNo.ToIntValue();
                            if (oldSeqNo.ToDecimalValue() != 0 & newSeqNo.ToDecimalValue() != 0)
                            {
                                pccs.SEQ_NO = newSeqNo.ToDecimalValue();
                            }
                        }
                        //ps.PART_NO = newPartNo;
                        //ps.ROUTE_NO = newRouteNo.ToIntValue();
                        //ps.SEQ_NO = newSeqNo.ToIntValue();
                        if (newdataValue.Count > 0 && varAppend == true)
                        {
                            List<PCCS> newdataValuF = (from o in DB.PCCS
                                                       where o.PART_NO == newPartNo.ToString() && o.ROUTE_NO == ((newRouteNo.ToIntValue() != 0) ? newRouteNo.ToIntValue() : o.ROUTE_NO) && o.SEQ_NO == ((newSeqNo.ToDecimalValue() != 0) ? newSeqNo.ToDecimalValue() : o.SEQ_NO)
                                                       select o).ToList<PCCS>();

                            pccs.SNO = newdataValuF.Count + 1;
                        }
                        else
                        {
                            pccs.SNO = ops.SNO;
                        }

                        pccs.ISR_NO = ops.ISR_NO;
                        pccs.FEATURE = ops.FEATURE;
                        pccs.CLASS = ops.CLASS;
                        pccs.SPEC = ops.SPEC;
                        pccs.CONTROL_SPEC = ops.CONTROL_SPEC;
                        pccs.DEPT_RESP = ops.DEPT_RESP;
                        pccs.FREQ_OF_INSP = ops.FREQ_OF_INSP;
                        pccs.GAUGES_USED = ops.GAUGES_USED;
                        pccs.GAUGE_RR = ops.GAUGE_RR;
                        pccs.INPROCESS_CONTROL = ops.INPROCESS_CONTROL;
                        pccs.METHOD_OF_STUDY = ops.METHOD_OF_STUDY;
                        pccs.SAMPLE_SIZE = ops.SAMPLE_SIZE;
                        pccs.CP = ops.CP;
                        pccs.CPK = ops.CPK;
                        pccs.PRD_PROC_COMP = ops.PRD_PROC_COMP;
                        pccs.FREQ_OF_STUDY = ops.FREQ_OF_STUDY;
                        pccs.COMMENTS = ops.COMMENTS;
                        pccs.SPEC_MIN = ops.SPEC_MIN;
                        pccs.SPEC_MAX = ops.SPEC_MAX;
                        pccs.CTRL_SPEC_MIN = ops.CTRL_SPEC_MIN;
                        pccs.CTRL_SPEC_MAX = ops.CTRL_SPEC_MAX;
                        pccs.CONTROL_METHOD = ops.CONTROL_METHOD;
                        pccs.REACTION_PLAN = ops.REACTION_PLAN;
                        pccs.PROCESS_FEATURE = ops.PROCESS_FEATURE;
                        pccs.SPEC_CHAR = ops.SPEC_CHAR;
                        pccs.ROWID = Guid.NewGuid();
                        pccs.DELETE_FLAG = false;
                        pccs.ENTERED_DATE = serverDateTime;
                        pccs.ENTERED_BY = userInformation.UserName;
                        PCCS pccsExists = (from o in DB.PCCS
                                           where o.PART_NO == pccs.PART_NO && o.ROUTE_NO == pccs.ROUTE_NO && o.SEQ_NO == pccs.SEQ_NO && o.SNO == pccs.SNO
                                           select o).FirstOrDefault<PCCS>();
                        PCCS pccsExistsFeature = (from o in DB.PCCS
                                                  where o.PART_NO == pccs.PART_NO && o.ROUTE_NO == pccs.ROUTE_NO && o.SEQ_NO == pccs.SEQ_NO && o.FEATURE == pccs.FEATURE
                                                  select o).FirstOrDefault<PCCS>();

                        if (pccsExists == null && pccsExistsFeature == null)
                        {
                            DB.PCCS.InsertOnSubmit(pccs);
                            DB.SubmitChanges();
                        }

                        pccs = null;

                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        //DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                        //DB.PCCS.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pccs);
                        //DB.PCCS.DeleteOnSubmit(pccs);
                        //  throw ex.LogException();
                        return -1;
                    }

                }


                return 1;
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                //throw ex.LogException();
                return 0;
            }
        }

        public int CopyProductData_PROD_DRAWING(string oldPartNo, string newPartNo)
        {
            try
            {
                List<PROD_DRAWING> olddataValue = (from o in DB.PROD_DRAWING
                                                   where o.PART_NO == oldPartNo.ToString()
                                                   select o).ToList<PROD_DRAWING>();

                List<PROD_DRAWING> newdataValue = (from o in DB.PROD_DRAWING
                                                   where o.PART_NO == newPartNo.ToString()
                                                   select o).ToList<PROD_DRAWING>();


                if (olddataValue == null) return 0;
                if (newdataValue.Count != 0)
                {
                    List<PROD_DRAWING> prd_Draw = new List<PROD_DRAWING>();

                    if (!string.IsNullOrEmpty(newPartNo))
                    {
                        prd_Draw = (from o in DB.PROD_DRAWING
                                    where o.PART_NO == newPartNo
                                    select o).ToList<PROD_DRAWING>();

                    }

                    if (prd_Draw != null)
                    {
                        DB.PROD_DRAWING.DeleteAllOnSubmit<PROD_DRAWING>(prd_Draw);
                        DB.SubmitChanges();
                    }
                    prd_Draw = null;
                }
                foreach (PROD_DRAWING ops in olddataValue)
                {
                    PROD_DRAWING ps = new PROD_DRAWING();

                    ps.PART_NO = newPartNo;
                    ps.DWG_TYPE = ops.DWG_TYPE;
                    ps.PAGE_NO = ops.PAGE_NO;
                    ps.PRD_DWG = ops.PRD_DWG;
                    DB.PROD_DRAWING.InsertOnSubmit(ps);
                    DB.SubmitChanges();
                    ps = null;

                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CopyProductData_PRD_DRAWING_BackUp(string oldPartNo, string newPartNo)
        {
            try
            {
                List<PRD_DRAWING> olddataValue = (from o in DB.PRD_DRAWING
                                                  where o.PART_NO == oldPartNo.ToString()
                                                  select o).ToList<PRD_DRAWING>();

                List<PRD_DRAWING> newdataValue = (from o in DB.PRD_DRAWING
                                                  where o.PART_NO == newPartNo.ToString()
                                                  select o).ToList<PRD_DRAWING>();


                if (olddataValue == null) return 0;
                if (newdataValue.Count != 0)
                {
                    List<PRD_DRAWING> prd_Draw = new List<PRD_DRAWING>();

                    if (!string.IsNullOrEmpty(newPartNo))
                    {
                        prd_Draw = (from o in DB.PRD_DRAWING
                                    where o.PART_NO == newPartNo
                                    select o).ToList<PRD_DRAWING>();

                    }

                    if (prd_Draw != null)
                    {
                        DB.PRD_DRAWING.DeleteAllOnSubmit<PRD_DRAWING>(prd_Draw);
                        DB.SubmitChanges();
                    }
                    prd_Draw = null;
                }
                foreach (PRD_DRAWING ops in olddataValue)
                {
                    PRD_DRAWING ps = new PRD_DRAWING();

                    ps.PART_NO = newPartNo;
                    ps.DWG_TYPE = ops.DWG_TYPE;
                    ps.PAGE_NO = ops.PAGE_NO;
                    ps.PRD_DWG = ops.PRD_DWG;
                    DB.PRD_DRAWING.InsertOnSubmit(ps);
                    DB.SubmitChanges();
                    ps = null;

                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int CopyProductData_PRD_DRAWING(string oldPartNo, string newPartNo, int dwgtype)
        {
            string message = "";
            try
            {
                List<PRD_DWG_ISSUE> olddataValue = (from o in DB.PRD_DWG_ISSUE
                                                    where o.PART_NO == oldPartNo.ToString()
                                                      && o.DWG_TYPE == dwgtype
                                                    select o).ToList<PRD_DWG_ISSUE>();

                List<PRD_DWG_ISSUE> newdataValue = (from o in DB.PRD_DWG_ISSUE
                                                    where o.PART_NO == newPartNo.ToString()
                                                    && o.DWG_TYPE == dwgtype
                                                    select o).ToList<PRD_DWG_ISSUE>();



                message = (dwgtype == 0 ? " Product " : " Sequence ");

                if (olddataValue.Count == 0 && newdataValue.Count > 0)
                {
                    //ShowInformationMessage("No data available to copy " + message + " Drawing details!");
                    return 0;
                }

                if (olddataValue.Count == 0) return 0;

                if (newdataValue.Count != 0)
                {
                    List<PRD_DWG_ISSUE> prd_Draw = new List<PRD_DWG_ISSUE>();

                    if (!string.IsNullOrEmpty(newPartNo))
                    {
                        prd_Draw = (from o in DB.PRD_DWG_ISSUE
                                    where o.PART_NO == newPartNo
                                    && o.DWG_TYPE == dwgtype
                                    select o).ToList<PRD_DWG_ISSUE>();
                    }


                    if (prd_Draw != null)
                    {

                        if (ShowInformationMessageInput(message + " Drawing details already Exists. \n" + "Do you want to overwrite?") == MessageBoxResult.No)
                        {
                            return 2;
                        }
                        DB.PRD_DWG_ISSUE.DeleteAllOnSubmit<PRD_DWG_ISSUE>(prd_Draw);
                        DB.SubmitChanges();
                    }
                    prd_Draw = null;
                }
                foreach (PRD_DWG_ISSUE ops in olddataValue)
                {
                    PRD_DWG_ISSUE ps = new PRD_DWG_ISSUE();
                    ps.PART_NO = newPartNo;
                    ps.COMPILED_BY = ops.COMPILED_BY;
                    ps.DWG_TYPE = ops.DWG_TYPE;
                    ps.ENTERED_BY = userInformation.UserName;
                    ps.ENTERED_DATE = serverDateTime;
                    ps.ISSUE_ALTER = ops.ISSUE_ALTER;
                    ps.ISSUE_DATE = ops.ISSUE_DATE;
                    ps.ISSUE_NO = ops.ISSUE_NO;
                    DB.PRD_DWG_ISSUE.InsertOnSubmit(ps);
                    DB.SubmitChanges();
                    ps = null;
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ReNumberProcessSheet(string oldPartno, int routeno, decimal oldSeqno, decimal newSeqno, bool isRenumbered = false)
        {
            int x = 0;
            x = CopyProcessSheet(oldPartno, oldPartno, routeno, routeno, oldSeqno, newSeqno, isRenumbered);
            x = CopyProcessCostCenter(oldPartno, oldPartno, routeno, routeno, oldSeqno, newSeqno);
            x = CopyProductData_Tool_Sched_Main(oldPartno, oldPartno, routeno.ToString(), routeno.ToString(), oldSeqno.ToString(), newSeqno.ToString());
            x = CopyToolSchedule_TOOL_SCHED_ISSUE(oldPartno, oldPartno, routeno, routeno, oldSeqno, newSeqno);
            x = CopyProcessSheet_PCCS(oldPartno, oldPartno, routeno, routeno, oldSeqno, newSeqno);

            List<TOOL_SCHED_SUB> lsttss = (from o in DB.TOOL_SCHED_SUB
                                           where o.PART_NO == oldPartno && o.ROUTE_NO == routeno && o.SEQ_NO == oldSeqno
                                           select o).ToList<TOOL_SCHED_SUB>();

            TOOL_SCHED_SUB tss = new TOOL_SCHED_SUB();
            try
            {
                //Bug Id : 925
                for (x = 0; x < lsttss.Count; x++)
                {
                    tss = lsttss[x];
                    tss.SEQ_NO = newSeqno;
                }
                DB.SubmitChanges();
                //if (tss != null)
                //{
                //    tss.SEQ_NO = newSeqno;
                //    DB.SubmitChanges();
                //}
                //tss = null;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                DB.TOOL_SCHED_SUB.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, tss);
                throw ex.LogException();
            }
            //tss = null;

            DeleteRecord_PROCESS_SHEET(oldPartno, routeno, oldSeqno);
            DeleteRecord_PROCESS_CC(oldPartno, routeno, oldSeqno);
            DeleteRecord_TOOL_SCHED_MAIN(oldPartno, routeno, oldSeqno);
            DeleteRecord_PCCS(oldPartno, routeno, oldSeqno);
            DeleteRecord_TOOL_SCHED_ISSUE(oldPartno, routeno, oldSeqno);


            return 1;
        }

        public void DeleteRecord_PROCESS_SHEET(string partno, int routeno, decimal seqno)
        {

            PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
                                where o.PART_NO == partno && o.ROUTE_NO == routeno && o.SEQ_NO == seqno
                                select o).FirstOrDefault<PROCESS_SHEET>();
            try
            {

                if (ps != null)
                {
                    DB.PROCESS_SHEET.DeleteOnSubmit(ps);
                    DB.SubmitChanges();
                }
                ps = null;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                DB.PROCESS_SHEET.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ps);
                throw ex.LogException();
            }
            ps = null;
        }

        public void DeleteRecord_PROCESS_CC(string partno, int routeno, decimal seqno)
        {

            PROCESS_CC pcc = (from o in DB.PROCESS_CC
                              where o.PART_NO == partno && o.ROUTE_NO == routeno && o.SEQ_NO == seqno
                              select o).FirstOrDefault<PROCESS_CC>();
            try
            {

                if (pcc != null)
                {
                    DB.PROCESS_CC.DeleteOnSubmit(pcc);
                    DB.SubmitChanges();
                }
                pcc = null;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                DB.PROCESS_CC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pcc);
                throw ex.LogException();
            }
            pcc = null;
        }

        public void DeleteRecord_TOOL_SCHED_MAIN(string partno, int routeno, decimal seqno)
        {

            TOOL_SCHED_MAIN tsm = (from o in DB.TOOL_SCHED_MAIN
                                   where o.PART_NO == partno && o.ROUTE_NO == routeno && o.SEQ_NO == seqno
                                   select o).FirstOrDefault<TOOL_SCHED_MAIN>();
            try
            {

                if (tsm != null)
                {
                    DB.TOOL_SCHED_MAIN.DeleteOnSubmit(tsm);
                    DB.SubmitChanges();
                }
                tsm = null;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                DB.TOOL_SCHED_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, tsm);
                throw ex.LogException();
            }
            tsm = null;
        }

        public void DeleteRecord_PCCS(string partno, int routeno, decimal seqno)
        {

            PCCS pccs = (from o in DB.PCCS
                         where o.PART_NO == partno && o.ROUTE_NO == routeno && o.SEQ_NO == seqno
                         select o).FirstOrDefault<PCCS>();
            try
            {

                if (pccs != null)
                {
                    DB.PCCS.DeleteOnSubmit(pccs);
                    DB.SubmitChanges();
                }
                pccs = null;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                DB.PCCS.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pccs);
                throw ex.LogException();
            }
            pccs = null;
        }

        public void DeleteRecord_TOOL_SCHED_ISSUE(string partno, int routeno, decimal seqno)
        {

            TOOL_SCHED_ISSUE tsi = (from o in DB.TOOL_SCHED_ISSUE
                                    where o.PART_NO == partno && o.ROUTE_NO == routeno && o.SEQ_NO == seqno
                                    select o).FirstOrDefault<TOOL_SCHED_ISSUE>();
            try
            {

                if (tsi != null)
                {
                    DB.TOOL_SCHED_ISSUE.DeleteOnSubmit(tsi);
                    DB.SubmitChanges();
                }
                tsi = null;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                DB.TOOL_SCHED_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, tsi);
                throw ex.LogException();
            }
            tsi = null;
        }

        private MessageBoxResult ShowInformationMessageInput(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        public PROCESS_MAIN GetCurrentProcessByPartNumber(PROCESS_MAIN paramEntity)
        {
            List<PROCESS_MAIN> lstEntity = null;
            PROCESS_MAIN entity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return entity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PROCESS_MAIN
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) &&
                                 Convert.ToBoolean(Convert.ToInt16(row.CURRENT_PROC)) == true && row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<PROCESS_MAIN>();
                }
                else
                {

                    lstEntity = (from row in DB.PROCESS_MAIN
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) &&
                                 Convert.ToBoolean(Convert.ToInt16(row.CURRENT_PROC)) == true
                                 select row).ToList<PROCESS_MAIN>();
                }

                entity = (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0 ? lstEntity[0] : new PROCESS_MAIN() { PART_NO = paramEntity.PART_NO, ROUTE_NO = -999999 });
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return entity;

        }

    }
}
