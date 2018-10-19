using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Windows;
using ProcessDesigner.DAL;
using System.Configuration;


namespace ProcessDesigner.BLL
{
    public class ProcessSheetBll : Essential
    {
        //new by me
        public string Previous_seq;
        public string Current_seq;
        public bool UpdateFlag = false;

        //end new
        public ProcessSheetBll(UserInformation userinfo)
        {
            this.userInformation = userinfo;
        }

        public bool GetProductMaster(ProcessSheetModel processSheet)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                if (dt != null)
                {

                    processSheet.DVProductMaster = dt.DefaultView;
                }
                else
                {
                    processSheet.DVProductMaster = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetProcessMain(ProcessSheetModel processSheet, int currentproc = 0)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable((from o in DB.PROCESS_MAIN
                                  where o.PART_NO == processSheet.PART_NO && o.CURRENT_PROC == ((currentproc > 0) ? currentproc : o.CURRENT_PROC) && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  orderby o.ROUTE_NO
                                  select new { o.ROUTE_NO, o.CURRENT_PROC }).ToList());
                if (dt != null)
                {

                    processSheet.DVProcessMain = dt.DefaultView;
                }
                else
                {
                    processSheet.DVProcessMain = null;
                }


                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public int RouteNoGeneration(ProcessSheetModel processSheet)
        {
            try
            {
                if (processSheet.PART_NO == "") return 0;

                try
                {
                    decimal routeno = (from o in DB.PROCESS_MAIN
                                       where o.PART_NO == processSheet.PART_NO && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                       select o.ROUTE_NO).Max();
                    if (routeno == null) routeno = 0;

                    return (Convert.ToInt16(routeno) + 1);
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    return 1;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetProcessSheetDetils(ProcessSheetModel processSheet)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable((from o in DB.PROCESS_MAIN
                                  where o.PART_NO == processSheet.PART_NO && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.ROWID, o.PART_NO, o.ROUTE_NO, o.CURRENT_PROC, o.TKO_CD, o.AJAX_CD, o.RM_CD, o.ALT_RM_CD, o.ALT_RM_CD1, o.WIRE_ROD_CD, o.ALT_WIRE_ROD_CD, o.ALT_WIRE_ROD_CD1, o.CHEESE_WT, o.CHEESE_WT_EST, o.ACTIVE_PART, o.RM_WT }).ToList());

                if (dt != null)
                {
                    processSheet.DVProcessMainDetails = dt.DefaultView;
                }
                else
                {
                    processSheet.DVProcessMainDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool RetreiveCIRefence(ProcessSheetModel processSheet)
        {
            try
            {
                string varciref = "";
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_CIREF
                                  where o.PART_NO == processSheet.PART_NO && o.CURRENT_CIREF == true
                                  select new { o.CI_REF, o.PART_NO, o.SNO }).ToList());

                if (dt != null && dt.Rows.Count != 0)
                {
                    varciref = dt.Rows[0]["CI_REF"].ToString();
                    processSheet.CI_REFERENCE = varciref;
                }
                else
                {
                    varciref = "";
                    processSheet.CI_REFERENCE = "";
                }

                if (varciref != "")
                {
                    dt = ToDataTable((from o in DB.DDCI_INFO
                                      where o.CI_REFERENCE == varciref && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select new { o.CI_REFERENCE, o.CUST_DWG_NO, o.CUST_CODE, o.FINISH_CODE, o.CUST_DWG_NO_ISSUE, o.CUST_STD_DATE }).ToList());

                    if (dt != null && dt.Rows.Count != 0)
                    {
                        processSheet.CI_REFERENCE = dt.Rows[0]["CI_REFERENCE"].ToString();
                    }
                }
                varciref = "";

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private int GetValidNumber(string issueNo)
        {

            int output;
            int finalValue = 0;
            if (int.TryParse(issueNo, out output) == true)
            {
                finalValue = int.Parse(issueNo);
            }
            else
            {
                finalValue = 0;
            }
            return finalValue;
        }

        public bool GetGridDetails(ProcessSheetModel processSheet)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTableWithType((from o in DB.PROCESS_SHEET
                                          where o.PART_NO == processSheet.PART_NO && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                          orderby o.SEQ_NO
                                          select new { o.ROWID, o.PART_NO, o.ROUTE_NO, SEQ_NO = o.SEQ_NO.ToString(), SORTCOL = o.SEQ_NO, OPER_CODE = o.OPN_CD, o.OPN_DESC, TRANSPORT_CD = o.TRANSPORT, o.FMEA_RISK, UNIT_OF_MEASURE = o.UNIT_OF_MEASURE.ToString(), GROSS_WEIGHT = o.GROSS_WEIGHT.ToString(), NET_WEIGHT = o.NET_WEIGHT.ToString() }).ToList());

                processSheet.DVProcessSheet = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTableWithType((from o in DB.PROCESS_ISSUE
                                          where o.PART_NO == processSheet.PART_NO
                                          orderby o.ISSUE_NO
                                          select new { o.ROWID, o.PART_NO, o.ROUTE_NO, o.ISSUE_NO, SORTCOL = o.ISSUE_NO, o.ISSUE_DATE, o.ISSUE_ALTER, o.COMPILED_BY }).ToList());
                dt.Columns.Add("ISSUE_NONUMERIC", typeof(System.Int32));
                foreach (DataRow dr in dt.Rows)
                {
                    int output;
                    if (int.TryParse(dr["ISSUE_NO"].ToValueAsString(), out output) == true)
                    {
                        dr["ISSUE_NONUMERIC"] = dr["ISSUE_NO"];
                    }
                    else
                    {
                        dr["ISSUE_NONUMERIC"] = 0;
                    }
                }

                dt.DefaultView.Sort = "ISSUE_NONUMERIC";
                DataTable dtFinal = dt.DefaultView.ToTable();
                processSheet.DVProcessIssue = (dtFinal != null) ? dtFinal.DefaultView : null;
                processSheet.DVProcessIssue.Sort = "";

                //original

                dt = ToDataTableWithType((from o in DB.PROCESS_CC
                                          where o.PART_NO == processSheet.PART_NO
                                          select new { o.ROWID, o.PART_NO, o.ROUTE_NO, o.SEQ_NO, o.CC_SNO, COST_CENT_CODE = o.CC_CODE, o.WIRE_SIZE_MIN, o.WIRE_SIZE_MAX, o.OUTPUT }).ToList());
                //original end
                //new
                //dt = ToDataTableWithType((from o in DB.PROCESS_CC
                //                          where o.PART_NO == processSheet.PART_NO && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                //                         select new { o.ROWID, o.PART_NO, o.ROUTE_NO, o.SEQ_NO, o.CC_SNO, COST_CENT_CODE = o.CC_CODE, o.WIRE_SIZE_MIN, o.WIRE_SIZE_MAX, o.OUTPUT }).ToList());


                //new end
                processSheet.DVProcessCC = (dt != null) ? dt.DefaultView : null;

                var innerQuery = (from o in DB.PRD_MAST
                                  where o.PART_NO == processSheet.PART_NO
                                  && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select o.BIF_PROJ).Distinct();

                dt = ToDataTable((from o in DB.DDCOST_CENT_MAST
                                  where innerQuery.Contains(o.LOC_CODE)
                                  select new { o.COST_CENT_CODE, o.COST_CENT_DESC, o.LOC_CODE }).ToList());

                processSheet.DVCCMaster = (dt != null) ? dt.DefaultView : null;

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetDropDownSource(ProcessSheetModel processSheet)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.DDRM_MAST
                                  where (o.RM_CODE.StartsWith("3") || o.RM_CODE.StartsWith("4") || o.RM_CODE.StartsWith("1") || o.RM_CODE.StartsWith("0")) && o.RM_CODE.Trim() != "" && o.RM_CODE != null
                                  && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  orderby o.RM_CODE
                                  select new { o.RM_CODE, o.RM_DESC }).ToList());

                if (dt != null)
                {
                    processSheet.DVRMBasic = dt.Copy().DefaultView;
                    processSheet.DVAltRMBasic1 = dt.Copy().DefaultView;
                    processSheet.DVAltRMBasic2 = dt.Copy().DefaultView;
                }
                else
                {
                    processSheet.DVRMBasic = null;
                    processSheet.DVAltRMBasic1 = null;
                    processSheet.DVAltRMBasic2 = null;
                }

                dt = ToDataTable((from o in DB.DDRM_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null)) && o.RM_CODE.Trim() != "" && o.RM_CODE != null
                                  orderby o.RM_CODE
                                  select new { o.RM_CODE, o.RM_DESC }).ToList());

                if (dt != null)
                {
                    processSheet.AllDVWire = dt.Copy().DefaultView;
                    processSheet.DVWire = dt.Copy().DefaultView;
                    processSheet.DVAltWire1 = dt.Copy().DefaultView;
                    processSheet.DVAltWire2 = dt.Copy().DefaultView;
                }
                else
                {
                    processSheet.AllDVWire = null;
                    processSheet.DVWire = null;
                    processSheet.DVAltWire1 = null;
                    processSheet.DVAltWire2 = null;
                }

                dt = ToDataTable((from o in DB.DDOPER_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.OPER_CODE, o.OPER_DESC }).ToList());

                processSheet.DVOperationCode = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.TRANSPORT_MAST
                                  select new { o.TRANSPORT_CD, o.TRANSPORT_DESC }).ToList());

                processSheet.DVTransport = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.FMEA_RISK_MASTER
                                  select new { o.FMEA_RISK }).ToList());

                processSheet.DVFMEArisk = (dt != null) ? dt.DefaultView : null;


                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetUnitDetails(ProcessSheetModel processSheet)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.DDUNIT_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.UNIT_CODE, o.UNIT_OF_MEAS }).ToList());
                if (dt != null)
                {
                    processSheet.UnitDetails = dt.DefaultView;
                }
                else
                {
                    processSheet.UnitDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool DeleteProcessSheet(DataRowView drv, string partNo, int routeNo = 0)
        {
            bool _status = false;

            try
            {
                if (drv["SEQ_NO"].ToString().Trim() != "")
                {
                    PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
                                        where o.PART_NO == partNo && o.ROUTE_NO == ((routeNo != 0) ? routeNo : o.ROUTE_NO) && o.SEQ_NO == ((drv["SEQ_NO"].ToString().ToDecimalValue() != 0) ? drv["SEQ_NO"].ToString().ToDecimalValue() : o.SEQ_NO)
                                        select o).FirstOrDefault<PROCESS_SHEET>();

                    if (ps != null)
                    {
                        DB.PROCESS_SHEET.DeleteOnSubmit(ps);
                        DB.SubmitChanges();
                    }
                    ps = null;

                    List<PROCESS_CC> pccList = (from o in DB.PROCESS_CC
                                                where o.PART_NO == partNo && o.ROUTE_NO == ((routeNo != 0) ? routeNo : o.ROUTE_NO) && o.SEQ_NO == ((drv["SEQ_NO"].ToString().ToDecimalValue() != 0) ? drv["SEQ_NO"].ToString().ToDecimalValue() : o.SEQ_NO)
                                                select o).ToList<PROCESS_CC>();

                    if (pccList.Count > 0)
                    {
                        foreach (PROCESS_CC pcc in pccList)
                        {
                            DB.PROCESS_CC.DeleteOnSubmit(pcc);
                        }
                        DB.SubmitChanges();
                    }

                    List<TOOL_SCHED_MAIN> tsmList = (from o in DB.TOOL_SCHED_MAIN
                                                     where o.PART_NO == partNo && o.ROUTE_NO == ((routeNo != 0) ? routeNo : o.ROUTE_NO) && o.SEQ_NO == ((drv["SEQ_NO"].ToString().ToDecimalValue() != 0) ? drv["SEQ_NO"].ToString().ToDecimalValue() : o.SEQ_NO)
                                                     select o).ToList<TOOL_SCHED_MAIN>();

                    if (tsmList.Count > 0)
                    {
                        foreach (TOOL_SCHED_MAIN tsm in tsmList)
                        {
                            DB.TOOL_SCHED_MAIN.DeleteOnSubmit(tsm);
                        }
                        DB.SubmitChanges();
                    }


                    List<TOOL_SCHED_SUB> tssList = (from o in DB.TOOL_SCHED_SUB
                                                    where o.PART_NO == partNo && o.ROUTE_NO == ((routeNo != 0) ? routeNo : o.ROUTE_NO) && o.SEQ_NO == ((drv["SEQ_NO"].ToString().ToDecimalValue() != 0) ? drv["SEQ_NO"].ToString().ToDecimalValue() : o.SEQ_NO)
                                                    select o).ToList<TOOL_SCHED_SUB>();

                    if (tssList.Count > 0)
                    {
                        foreach (TOOL_SCHED_SUB tss in tssList)
                        {
                            DB.TOOL_SCHED_SUB.DeleteOnSubmit(tss);
                        }
                        DB.SubmitChanges();
                    }


                    List<PCCS> pcsList = (from o in DB.PCCS
                                          where o.PART_NO == partNo && o.ROUTE_NO == ((routeNo != 0) ? routeNo : o.ROUTE_NO) && o.SEQ_NO == ((drv["SEQ_NO"].ToString().ToDecimalValue() != 0) ? drv["SEQ_NO"].ToString().ToDecimalValue() : o.SEQ_NO)
                                          select o).ToList<PCCS>();

                    if (pcsList.Count > 0)
                    {
                        foreach (PCCS pcs in pcsList)
                        {
                            DB.PCCS.DeleteOnSubmit(pcs);
                        }
                        DB.SubmitChanges();
                    }

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

            return _status;
        }

        public bool DeleteProcessIssue(DataRowView drv, string partNo, int routeNo = 0)
        {
            bool _status = false;

            try
            {
                if (drv["ISSUE_NO"].ToString().Trim() != "")
                {
                    PROCESS_ISSUE pi = (from o in DB.PROCESS_ISSUE
                                        where o.PART_NO == partNo && o.ROUTE_NO == routeNo && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
                                        select o).FirstOrDefault<PROCESS_ISSUE>();

                    if (pi != null)
                    {
                        DB.PROCESS_ISSUE.DeleteOnSubmit(pi);
                        DB.SubmitChanges();
                    }
                    pi = null;

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

            return _status;
        }

        public bool DeleteProcessCC(DataRowView drv, string partNo, int routeNo = 0)
        {
            bool _status = false;

            try
            {
                if (drv["ROWID"].ToString().Trim() != "")
                {
                    PROCESS_CC pcc = (from o in DB.PROCESS_CC
                                      where o.PART_NO == partNo && o.ROWID == (Guid)drv["ROWID"]
                                      select o).FirstOrDefault<PROCESS_CC>();

                    if (pcc != null)
                    {
                        DB.PROCESS_CC.DeleteOnSubmit(pcc);
                        DB.SubmitChanges();
                    }
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

            return _status;
        }

        public int RecordCount(string partNo)
        {
            try
            {
                if (partNo == "") return 0;

                try
                {
                    int cnt = (from o in DB.PRD_MAST
                               where o.PART_NO == partNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                               select o.PART_NO).Count();
                    if (cnt == null) cnt = 0;

                    return cnt;
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        //UpdateProcessSheet original funtion
        //public bool UpdateProcessSheet(ProcessSheetModel processSheet)
        //{

        //    bool _status = false;
        //    DataView dvProcessSheet = processSheet.DVProcessSheet.Table.Copy().DefaultView;
        //    DataView dvProcessCC = processSheet.DVProcessCC.Table.Copy().DefaultView;
        //    DataView dvProcessIssue = processSheet.DVProcessIssue.Table.Copy().DefaultView;


        //    try
        //    {
        //        // Process Main updates
        //        processSheet.DVProcessMainDetails.Table.AcceptChanges();
        //        foreach (DataRow dr in processSheet.DVProcessMainDetails.Table.Rows)
        //        {
        //            dvProcessSheet.RowFilter = "Convert(ROUTE_NO, 'System.String') = '" + dr["ROUTE_NO"].ToString().Trim() + "' AND Convert(SEQ_NO, 'System.String') <> ''";

        //            if (processSheet.PART_NO != "" && dr["ROUTE_NO"].ToString().Trim() != "" && dvProcessSheet.Count > 0)
        //            {


        //                PROCESS_MAIN pm = (from o in DB.PROCESS_MAIN
        //                                   where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
        //                                   select o).FirstOrDefault<PROCESS_MAIN>();
        //                try
        //                {

        //                    if (pm == null)
        //                    {
        //                        pm = new PROCESS_MAIN();
        //                        pm.PART_NO = processSheet.PART_NO.Trim();
        //                        pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
        //                        pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
        //                        pm.AJAX_CD = dr["AJAX_CD"].ToString();
        //                        pm.TKO_CD = dr["TKO_CD"].ToString();
        //                        pm.RM_CD = dr["RM_CD"].ToString();
        //                        pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
        //                        pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
        //                        pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
        //                        pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
        //                        pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
        //                        pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
        //                        pm.ENTERED_BY = userInformation.UserName;
        //                        pm.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                        pm.ROWID = Guid.NewGuid();
        //                        DB.PROCESS_MAIN.InsertOnSubmit(pm);
        //                        DB.SubmitChanges();
        //                    }
        //                    pm = null;
        //                }
        //                catch (System.Data.Linq.ChangeConflictException)
        //                {
        //                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.PROCESS_MAIN.DeleteOnSubmit(pm);
        //                    throw ex.LogException();
        //                }
        //                pm = null;


        //                pm = (from o in DB.PROCESS_MAIN
        //                      where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
        //                      select o).FirstOrDefault<PROCESS_MAIN>();
        //                try
        //                {

        //                    if (pm != null)
        //                    {
        //                        //  pm.PART_NO = processSheet.PART_NO.Trim();
        //                        pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
        //                        pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
        //                        pm.AJAX_CD = dr["AJAX_CD"].ToString();
        //                        pm.TKO_CD = dr["TKO_CD"].ToString();
        //                        pm.RM_CD = dr["RM_CD"].ToString();
        //                        pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
        //                        pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
        //                        pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
        //                        pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
        //                        pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
        //                        pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
        //                        pm.UPDATED_BY = userInformation.UserName;
        //                        pm.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                        DB.SubmitChanges();
        //                    }
        //                    pm = null;

        //                }
        //                catch (System.Data.Linq.ChangeConflictException)
        //                {
        //                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.PROCESS_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pm);
        //                    throw ex.LogException();
        //                }
        //            }
        //        }


        //        //Process Sheet Update
        //        dvProcessSheet.Table.AcceptChanges();
        //        dvProcessSheet.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(OPER_CODE, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessSheet)
        //        {

        //            //original

        //            PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
        //                                where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //                                select o).FirstOrDefault<PROCESS_SHEET>();

        //            try
        //            {

        //                if (ps == null)
        //                {

        //                    ps = new PROCESS_SHEET();
        //                    ps.PART_NO = processSheet.PART_NO.Trim();
        //                    ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
        //                    ps.OPN_DESC = drv["OPN_DESC"].ToString();
        //                    ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
        //                    ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
        //                    ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
        //                    ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.ENTERED_BY = userInformation.UserName;
        //                    ps.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    ps.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_SHEET.InsertOnSubmit(ps);
        //                    DB.SubmitChanges();
        //                }
        //                ps = null;
        //            }
        //            //original end




        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_SHEET.DeleteOnSubmit(ps);
        //                throw ex.LogException();
        //            }
        //            ps = null;

        //            ps = (from o in DB.PROCESS_SHEET
        //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //                  select o).FirstOrDefault<PROCESS_SHEET>();
        //            var t = drv["ROWID"].ToString();


        //            try
        //            {

        //                if (ps != null)
        //                {


        //                    // ps.PART_NO = processSheet.PART_NO.Trim();//old
        //                    ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
        //                    ps.OPN_DESC = drv["OPN_DESC"].ToString();
        //                    ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
        //                    ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
        //                    ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
        //                    ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.UPDATED_BY = userInformation.UserName;
        //                    ps.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();

        //                }
        //                ps = null;

        //            }


        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_SHEET.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ps);
        //                throw ex.LogException();
        //            }

        //        }

        //        //Process Issue Update
        //        dvProcessIssue.Table.AcceptChanges();
        //        dvProcessIssue.RowFilter = "Convert(ISSUE_NO, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessIssue)
        //        {



        //            PROCESS_ISSUE pi = (from o in DB.PROCESS_ISSUE
        //                                where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
        //                                select o).FirstOrDefault<PROCESS_ISSUE>();
        //            try
        //            {

        //                if (pi == null)
        //                {
        //                    pi = new PROCESS_ISSUE();
        //                    pi.PART_NO = processSheet.PART_NO.Trim();
        //                    pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
        //                    pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
        //                    pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
        //                    pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
        //                    //pi.ENTERED_BY = userInformation.UserName;
        //                    //pi.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    pi.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_ISSUE.InsertOnSubmit(pi);
        //                    DB.SubmitChanges();
        //                }
        //                pi = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_ISSUE.DeleteOnSubmit(pi);
        //                throw ex.LogException();
        //            }
        //            pi = null;


        //            pi = (from o in DB.PROCESS_ISSUE
        //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
        //                  select o).FirstOrDefault<PROCESS_ISSUE>();

        //            try
        //            {

        //                if (pi != null)
        //                {
        //                    // pi.PART_NO = processSheet.PART_NO.Trim();
        //                    pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
        //                    pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
        //                    pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
        //                    pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
        //                    //pi.UPDATED_BY = userInformation.UserName;
        //                    //pi.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pi = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pi);
        //                throw ex.LogException();
        //            }

        //        }

        //        //original code for process_cc
        //        //ProcessCC Update
        //        dvProcessCC.Table.AcceptChanges();
        //        dvProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessCC)
        //        {

        //            //PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //            //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //            //                  select o).FirstOrDefault<PROCESS_CC>();



        //            PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //                              where o.ROWID.ToString() == drv["ROWID"].ToString()
        //                              select o).FirstOrDefault<PROCESS_CC>();

        //            try
        //            {

        //                if (pcc == null)
        //                {
        //                    pcc = new PROCESS_CC();
        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = GenerateSNO(drv, processSheet.PART_NO.Trim());
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    pcc.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_CC.InsertOnSubmit(pcc);
        //                    DB.SubmitChanges();
        //                }


        //                else
        //                {

        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = drv["CC_SNO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pcc = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.DeleteOnSubmit(pcc);
        //                throw ex.LogException();
        //            }
        //            pcc = null;


        //            pcc = (from o in DB.PROCESS_CC
        //                   where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //                   select o).FirstOrDefault<PROCESS_CC>();

        //            try
        //            {

        //                if (pcc != null)
        //                {

        //                    //  pcc.PART_NO = processSheet.PART_NO.Trim();//original comment
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    //pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();//comment by me
        //                    //if (pcc.SEQ_NO == Previous_seq)
        //                    //{
        //                    //    drv["SEQ_NO"] = Current_seq.ToString().ToDecimalValue();
        //                    //}
        //                    ////else
        //                    //{
        //                    //    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    //}
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.UPDATED_BY = userInformation.UserName;
        //                    //pcc.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }

        //                pcc = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pcc);
        //                throw ex.LogException();
        //            }

        //        }

        //        _status = true;
        //    }
        //    catch (System.Data.Linq.ChangeConflictException)
        //    {
        //        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //    }
        //    catch (Exception ex)
        //    {
        //        _status = false;
        //        throw ex.LogException();
        //    }
        //    return _status;
        //}
        //original end for process_CC

        //original end processsheetfunction by me

        //new code by me process_CC
        //ProcessCC Update
        //        dvProcessCC.Table.AcceptChanges();
        //        //dvProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";//new original
        //        dvProcessCC.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(COST_CENT_CODE, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessCC)
        //        {

        //            PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //                              where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //                              && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //                              select o).FirstOrDefault<PROCESS_CC>();
        //            try
        //            {

        //            if (pcc == null)
        //              { 
        //            PROCESS_CC prow = (from o in DB.PROCESS_CC
        //                              where o.ROWID.ToString() == drv["ROWID"].ToString()
        //                              select o).FirstOrDefault<PROCESS_CC>();


        //                if (prow == null)
        //                {
        //                    pcc = new PROCESS_CC();
        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = GenerateSNO(drv, processSheet.PART_NO.Trim());
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    pcc.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_CC.InsertOnSubmit(pcc);
        //                    DB.SubmitChanges();
        //                }


        //                else
        //                {
        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = drv["CC_SNO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //            }
        //                pcc = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.DeleteOnSubmit(pcc);
        //                throw ex.LogException();
        //            }
        //            pcc = null;


        //            pcc = (from o in DB.PROCESS_CC
        //                   where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //                   select o).FirstOrDefault<PROCESS_CC>();

        //            try
        //            {

        //                if (pcc != null)
        //                {
        //                    //  pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.UPDATED_BY = userInformation.UserName;
        //                    //pcc.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pcc = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pcc);
        //                throw ex.LogException();
        //            }

        //        }

        //        _status = true;
        //    }
        //    catch (System.Data.Linq.ChangeConflictException)
        //    {
        //        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //    }
        //    catch (Exception ex)
        //    {
        //        _status = false;
        //        throw ex.LogException();
        //    }
        //    return _status;
        //}
        //new end code by me process_CC

        private DateTime? ConvertToDateTime(object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return null;
            }
        }

        public decimal GenerateSNO(DataRowView drv, string partNo)
        {
            try
            {
                decimal sno = (from o in DB.PROCESS_CC
                               where o.PART_NO == partNo && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
                               select o.CC_SNO).Max();

                return Convert.ToDecimal(sno) + 1;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return 1;
            }
        }

        public DataTable GetProcessCC(string part_no, string route_no)
        {
            //DataView dtCIReference;
            //RPDModel model_rpd = new RPDModel();
            //string getQuery = "select a.PART_NO,m.PART_DESC, CAST(j.CHEESE_WT as varchar) as CHEESE_WT, j.WIRE_ROD_CD, c.cust_name, i.cust_dwg_no, Cast(m.QUALITY as varchar) as QUALITY, CAST(m.FINISH_WT as varchar) as FINISH_WT,"
            //    + " CAST(a.wire_size_max as varchar) as wire_size_max, CAST(a.wire_size_min as varchar) as wire_size_min, CAST(a.seq_no as varchar) as seq_no, e.OPN_DESC, CAST(e.OPN_CD as varchar) as OPN_CD, a.CC_CODE, CAST(a.OUTPUT as varchar) as OUTPUT, CAST(k.ISSUE_DATE as varchar) as ISSUE_DATE, k.ISSUE_NO, k.ISSUE_ALTER, k.COMPILED_BY"
            //    + " from ddci_info i, process_main j, process_issue k, process_cc a, prd_mast m, PROCESS_SHEET e, ddcust_mast c,(select ci_ref from prd_ciref where part_no='" + part_no + "' ) d where i.cust_code=c.cust_code "
            //    + " and d.CI_REF = i.CI_REFERENCE and e.SEQ_NO = a.SEQ_NO"
            //    + " and a.ROUTE_NO = e.ROUTE_NO"
            //    + " and j.PART_NO = m.PART_NO"
            //    + " and a.PART_NO = e.PART_NO and e.PART_NO = m.PART_NO"
            //    + " and k.ISSUE_DATE in(select max(ISSUE_DATE) from process_issue where part_no='" + part_no + "')"
            //    + " and k.ISSUE_NO in(select max(ISSUE_NO) from process_issue where part_no='" + part_no + "')"
            //    + " and k.ROUTE_NO in(select ROUTE_NO from PROCESS_SHEET where part_no='" + part_no + "')"
            //    + " and k.PART_NO = a.PART_NO"
            //    + " and a.PART_NO = '" + part_no + "'";

            string getQuery = "select a.PART_NO,m.PART_DESC, CAST(j.CHEESE_WT as varchar) as CHEESE_WT, j.WIRE_ROD_CD, d.cust_name, d.cust_dwg_no, Cast(m.QUALITY as varchar) as QUALITY, CAST(m.FINISH_WT as varchar) as FINISH_WT,"
               + " CAST(a.wire_size_max as varchar) as wire_size_max, CAST(a.wire_size_min as varchar) as wire_size_min, CAST(e.seq_no as varchar) as seq_no, e.OPN_DESC, CAST(e.OPN_CD as varchar) as OPN_CD, a.CC_CODE, CAST(a.OUTPUT as varchar) as OUTPUT, k.ISSUE_DATE, k.ISSUE_NO, k.ISSUE_ALTER, k.COMPILED_BY"
               + " from prd_mast m JOIN PROCESS_SHEET e ON m.PART_NO = e.PART_NO JOIN process_main j ON e.PART_NO = j.PART_NO and e.ROUTE_NO = j.ROUTE_NO "
               + " LEFT JOIN process_cc a ON j.PART_NO = a.PART_NO and a.SEQ_NO = e.SEQ_NO and a.ROUTE_NO = e.ROUTE_NO  LEFT JOIN (select pc.part_no,pc.ci_ref,i.cust_dwg_no,c.cust_name from prd_ciref pc JOIN ddci_info i ON CI_REF = i.CI_REFERENCE LEFT JOIN ddcust_mast c ON i.cust_code=c.cust_code where pc.part_no='" + part_no + "' AND pc.CURRENT_CIREF = 1 ) d ON j.PART_NO = d.PART_NO "
               + " LEFT JOIN (select PART_NO,ROUTE_NO, CAST(ISSUE_DATE as varchar) as ISSUE_DATE, ISSUE_NO, ISSUE_ALTER, COMPILED_BY from process_issue ii where ISSUE_NO in(select max(ISSUE_NO) from process_issue where part_no='" + part_no + "' and ROUTE_NO = '" + route_no + "' and ISSUE_DATE in(select max(ISSUE_DATE) from process_issue where part_no='" + part_no + "' and ROUTE_NO = '" + route_no + "'))) k ON e.PART_NO = k.PART_NO AND e.ROUTE_NO = k.ROUTE_NO "
               + " where e.PART_NO = '" + part_no + "' and e.ROUTE_NO = '" + route_no + "'";


            var getprocess = ToDataTable(DB.ExecuteQuery<RptProcessSheetModel>(getQuery).ToList());
            return getprocess;
            //return dtCIReference = ToDataTableWithType(DB.ExecuteQuery<RptProcessSheetModel>(getQuery).ToList()).DefaultView;
            //     return dtCIReference = ToDataTable(DB.ExecuteQuery(GetQuery.ToString())).DefaultView;
        }

        //new funtion written by me
        //public void GetCompareSequenceNo( DataView dvProcessSheet,string part_no,string route_no)
        //{
        //    DataTable d;
        //    DataTable Od;
        //    Od = dvProcessSheet.ToTable();
        //    //DataRowView drv,
        //    //    PROCESS_SHEET  ps = (from o in DB.PROCESS_SHEET
        //    //              where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //    //              select o).Select<PROCESS_SHEET>();
        //    //DataTable dt=ps;
        //     string getQuery = "select * from PROCESS_SHEET o  where e.PART_NO = '" + part_no + "' and e.ROUTE_NO = '" + route_no + "'"; 
        //     var getprocess = ToDataTable(DB.ExecuteQuery<ProcessSheetModel>(getQuery).ToList());    
        //            d=ToDataTable(DB.ExecuteQuery<ProcessSheetModel>(getQuery).ToList());


        //            try
        //            {

        //                if (Od.Rows.Count==d.Rows.Count)
        //                {
        //                    for (int i = 1; i <= Od.Rows.Count; i++)
        //                    {
        //                        for (int j = 1; j <= Od.Rows.Count; j++)
        //                        {
        //                            if (Od.Rows[j][i] != d.Rows[j][i])
        //                            {

        //                            }
        //                        }
        //                    }
        //                }
        //                //ps = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_SHEET.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ps);
        //                throw ex.LogException();
        //            }

        //}


        //  new updateprocesssheet function changes by me
        //public bool UpdateProcessSheet(ProcessSheetModel processSheet)
        //{

        //    bool _status = false;
        //    DataView dvProcessSheet = processSheet.DVProcessSheet.Table.Copy().DefaultView;
        //    DataView dvProcessCC = processSheet.DVProcessCC.Table.Copy().DefaultView;
        //    DataView dvProcessIssue = processSheet.DVProcessIssue.Table.Copy().DefaultView;


        //    try
        //    {
        //        // Process Main updates
        //        processSheet.DVProcessMainDetails.Table.AcceptChanges();
        //        foreach (DataRow dr in processSheet.DVProcessMainDetails.Table.Rows)
        //        {
        //            dvProcessSheet.RowFilter = "Convert(ROUTE_NO, 'System.String') = '" + dr["ROUTE_NO"].ToString().Trim() + "' AND Convert(SEQ_NO, 'System.String') <> ''";

        //            if (processSheet.PART_NO != "" && dr["ROUTE_NO"].ToString().Trim() != "" && dvProcessSheet.Count > 0)
        //            {


        //                PROCESS_MAIN pm = (from o in DB.PROCESS_MAIN
        //                                   where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
        //                                   select o).FirstOrDefault<PROCESS_MAIN>();
        //                try
        //                {

        //                    if (pm == null)
        //                    {
        //                        pm = new PROCESS_MAIN();
        //                        pm.PART_NO = processSheet.PART_NO.Trim();
        //                        pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
        //                        pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
        //                        pm.AJAX_CD = dr["AJAX_CD"].ToString();
        //                        pm.TKO_CD = dr["TKO_CD"].ToString();
        //                        pm.RM_CD = dr["RM_CD"].ToString();
        //                        pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
        //                        pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
        //                        pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
        //                        pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
        //                        pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
        //                        pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
        //                        pm.ENTERED_BY = userInformation.UserName;
        //                        pm.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                        pm.ROWID = Guid.NewGuid();
        //                        DB.PROCESS_MAIN.InsertOnSubmit(pm);
        //                        DB.SubmitChanges();
        //                    }
        //                    pm = null;
        //                }
        //                catch (System.Data.Linq.ChangeConflictException)
        //                {
        //                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.PROCESS_MAIN.DeleteOnSubmit(pm);
        //                    throw ex.LogException();
        //                }
        //                pm = null;


        //                pm = (from o in DB.PROCESS_MAIN
        //                      where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
        //                      select o).FirstOrDefault<PROCESS_MAIN>();
        //                try
        //                {

        //                    if (pm != null)
        //                    {
        //                        //  pm.PART_NO = processSheet.PART_NO.Trim();
        //                        pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
        //                        pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
        //                        pm.AJAX_CD = dr["AJAX_CD"].ToString();
        //                        pm.TKO_CD = dr["TKO_CD"].ToString();
        //                        pm.RM_CD = dr["RM_CD"].ToString();
        //                        pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
        //                        pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
        //                        pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
        //                        pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
        //                        pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
        //                        pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
        //                        pm.UPDATED_BY = userInformation.UserName;
        //                        pm.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                        DB.SubmitChanges();
        //                    }
        //                    pm = null;

        //                }
        //                catch (System.Data.Linq.ChangeConflictException)
        //                {
        //                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.PROCESS_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pm);
        //                    throw ex.LogException();
        //                }
        //            }
        //        }


        //        //Process Sheet Update
        //        dvProcessSheet.Table.AcceptChanges();
        //        dvProcessSheet.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(OPER_CODE, 'System.String') <> ''";

        //        foreach (DataRowView drv in dvProcessSheet)
        //        {

        //            //original

        //            //PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
        //            //                    where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //            //                    select o).FirstOrDefault<PROCESS_SHEET>();

        //            //try
        //            //{

        //            //    if (ps == null)
        //            //    {

        //            //        ps = new PROCESS_SHEET();
        //            //        ps.PART_NO = processSheet.PART_NO.Trim();
        //            //        ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //            //        ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //            //        ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
        //            //        ps.OPN_DESC = drv["OPN_DESC"].ToString();
        //            //        ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
        //            //        ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
        //            //        ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
        //            //        ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
        //            //        ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
        //            //        ps.ENTERED_BY = userInformation.UserName;
        //            //        ps.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //            //        ps.ROWID = Guid.NewGuid();
        //            //        DB.PROCESS_SHEET.InsertOnSubmit(ps);
        //            //        DB.SubmitChanges();
        //            //    }
        //            //    ps = null;
        //            //}
        //            //original end

        //            //new by me
        //        PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
        //                            where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //                            select o).FirstOrDefault<PROCESS_SHEET>();
        //        //DataTable dt = new DataTable();
        //        //dt = ToDataTable((from o in DB.PROCESS_SHEET
        //        //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //        //                  select new { o.PART_NO, o.ROUTE_NO, o.SEQ_NO }).ToList());

        //        //if (dt != null)
        //        //{

        //        //    processSheet.DVProcessSheet = dt.DefaultView;
        //        //}
        //        try
        //            {

        //                if (ps == null)
        //                {
        //                    PROCESS_SHEET pnew = (from o in DB.PROCESS_SHEET
        //                                          where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ROWID.ToString() == drv["ROWID"].ToString()
        //                                          select o).FirstOrDefault<PROCESS_SHEET>();

        //                    if (pnew != null)
        //                    {
        //                        //Previous_seq = Convert.ToString(pnew.SEQ_NO);
        //                        pnew.DELETE_FLAG = true;
        //                        UpdateFlag = true;
        //                        DB.SubmitChanges();
        //                    }

        //                    ps = new PROCESS_SHEET();
        //                    ps.PART_NO = processSheet.PART_NO.Trim();
        //                    ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
        //                    ps.OPN_DESC = drv["OPN_DESC"].ToString();
        //                    ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
        //                    ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
        //                    ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
        //                    ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.ENTERED_BY = userInformation.UserName;
        //                    ps.ENTERED_DATE = userInformation.Dal.ServerDateTime;
        //                    //new
        //                    //Current_seq = drv["SEQ_NO"].ToString();
        //                    if (UpdateFlag == true)
        //                    {
        //                        ps.DELETE_FLAG = false;

        //                    }
        //                    //new end
        //                    ps.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_SHEET.InsertOnSubmit(ps);
        //                    DB.SubmitChanges();
        //                    //if (UpdateFlag == true)
        //                    //{

        //                    //    UpdateSeqNo(ps.PART_NO, ps.ROUTE_NO, Convert.ToDecimal(Previous_seq), Current_seq);
        //                    //}
        //                    //new ny me
        //                    //Current_seq = ps.SEQ_NO;
        //                    //Current_seq = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    //end new
        //                }
        //                ps = null;
        //            }
        //            //end new me

        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_SHEET.DeleteOnSubmit(ps);
        //                throw ex.LogException();
        //            }
        //            ps = null;

        //            ps = (from o in DB.PROCESS_SHEET
        //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //                  select o).FirstOrDefault<PROCESS_SHEET>();
        //            var t = drv["ROWID"].ToString();



        //            try
        //            {

        //                if (ps != null)
        //                {

        //                    // ps = new PROCESS_SHEET();//by me
        //                    // ps.PART_NO = processSheet.PART_NO.Trim();//old
        //                    ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
        //                    ps.OPN_DESC = drv["OPN_DESC"].ToString();
        //                    ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
        //                    ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
        //                    ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
        //                    ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.UPDATED_BY = userInformation.UserName;
        //                    ps.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();

        //                }
        //                ps = null;

        //            }


        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_SHEET.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ps);
        //                throw ex.LogException();
        //            }

        //        }

        //        //Process Issue Update
        //        dvProcessIssue.Table.AcceptChanges();
        //        dvProcessIssue.RowFilter = "Convert(ISSUE_NO, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessIssue)
        //        {



        //            PROCESS_ISSUE pi = (from o in DB.PROCESS_ISSUE
        //                                where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
        //                                select o).FirstOrDefault<PROCESS_ISSUE>();
        //            try
        //            {

        //                if (pi == null)
        //                {
        //                    pi = new PROCESS_ISSUE();
        //                    pi.PART_NO = processSheet.PART_NO.Trim();
        //                    pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
        //                    pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
        //                    pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
        //                    pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
        //                    //pi.ENTERED_BY = userInformation.UserName;
        //                    //pi.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    pi.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_ISSUE.InsertOnSubmit(pi);
        //                    DB.SubmitChanges();
        //                }
        //                pi = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_ISSUE.DeleteOnSubmit(pi);
        //                throw ex.LogException();
        //            }
        //            pi = null;


        //            pi = (from o in DB.PROCESS_ISSUE
        //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
        //                  select o).FirstOrDefault<PROCESS_ISSUE>();

        //            try
        //            {

        //                if (pi != null)
        //                {
        //                    // pi.PART_NO = processSheet.PART_NO.Trim();
        //                    pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
        //                    pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
        //                    pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
        //                    pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
        //                    //pi.UPDATED_BY = userInformation.UserName;
        //                    //pi.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pi = null;

        //                //new updateflag process_cc

        //            }

        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pi);
        //                throw ex.LogException();
        //            }

        //        }

        //        //original code for process_cc
        //        //ProcessCC Update
        //        dvProcessCC.Table.AcceptChanges();
        //        dvProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";


        //        foreach (DataRowView drv in dvProcessCC)
        //        {
        //            //uncomment by me
        //            //PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //            //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //            //                  select o).FirstOrDefault<PROCESS_CC>();
        //            //uncomment by me

        //            //comment by me
        //            PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //                              where o.ROWID.ToString() == drv["ROWID"].ToString()
        //                              select o).FirstOrDefault<PROCESS_CC>();
        //            //comment by me




        //            try
        //            {

        //                if (pcc == null)
        //                {
        //                    pcc = new PROCESS_CC();
        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();

        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = GenerateSNO(drv, processSheet.PART_NO.Trim());
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    pcc.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_CC.InsertOnSubmit(pcc);
        //                    DB.SubmitChanges();
        //                }


        //                else
        //                {

        //                    //new edited
        //                    //if (pcc.SEQ_NO == Convert.ToDecimal(Previous_seq))
        //                    //{
        //                    //    drv["SEQ_NO"] = Convert.ToDecimal(Current_seq);
        //                    //}
        //                    //end new
        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = drv["CC_SNO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pcc = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.DeleteOnSubmit(pcc);
        //                throw ex.LogException();
        //            }
        //            pcc = null;


        //            pcc = (from o in DB.PROCESS_CC
        //                   where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //                   select o).FirstOrDefault<PROCESS_CC>();

        //            try
        //            {

        //                if (pcc != null)
        //                {

        //                    //  pcc.PART_NO = processSheet.PART_NO.Trim();//original comment
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();

        //                    //if (pcc.SEQ_NO == Convert.ToDecimal(Previous_seq))
        //                    //{
        //                    //    drv["SEQ_NO"] = Convert.ToDecimal(Current_seq);
        //                    //}
        //                    //else
        //                    //{
        //                    //    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    //}
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.UPDATED_BY = userInformation.UserName;
        //                    //pcc.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }

        //                pcc = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pcc);
        //                throw ex.LogException();
        //            }

        //        }

        //        _status = true;
        //    }
        //    catch (System.Data.Linq.ChangeConflictException)
        //    {
        //        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //    }
        //    catch (Exception ex)
        //    {
        //        _status = false;
        //        throw ex.LogException();
        //    }
        //    return _status;
        //}

        //end new update process sheet function
        //new update seq_no mari
        //public bool UpdateSeqNo(string partno, decimal route_no, decimal prevseqno, string currentseqno)
        //{
        //    bool b1 = false;
        //    DataTable pccdt = new DataTable();
        //    DataTable psdt = new DataTable();
        //    try { 
        //    //pccdt = ToDataTable((from o in DB.PROCESS_CC
        //    //                     where o.PART_NO == partno.Trim() && o.ROUTE_NO == route_no && o.SEQ_NO == prevseqno
        //    //                select new { o.PART_NO, o.ROUTE_NO, o.SEQ_NO, o.CC_CODE, o.CC_SNO }).ToList());
        //        pccdt = ToDataTable((from o in DB.PROCESS_CC
        //                             where o.PART_NO == partno.Trim() && o.ROUTE_NO == route_no && o.SEQ_NO == prevseqno
        //                             select o).ToList());
        //        psdt = ToDataTable((from o in DB.PROCESS_SHEET
        //                            where o.PART_NO == partno.Trim() && o.ROUTE_NO == route_no && o.SEQ_NO == currentseqno.ToDecimalValue()
        //                            select o).ToList());
        //    for (int i = 0; i < pccdt.Rows.Count; i++)
        //    {
        //        PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //                          where o.PART_NO == partno.Trim() && o.ROUTE_NO == route_no && o.SEQ_NO == Convert.ToDecimal(pccdt.Rows[i]["SEQ_NO"])
        //                          select o).FirstOrDefault<PROCESS_CC>();
        //        if (pcc != null)
        //        {
        //            string s = pccdt.Rows[i]["ROUTE_NO"].ToString();
        //            pcc.ROUTE_NO = pccdt.Rows[i]["ROUTE_NO"].ToString().ToDecimalValue();  //drv["ROUTE_NO"].ToString().ToDecimalValue();

        //            pcc.SEQ_NO = psdt.Rows[0]["SEQ_NO"].ToString().ToDecimalValue(); //currentseqno.ToDecimalValue(); //pccdt.Rows[i]["SEQ_NO"].ToString().ToDecimalValue();
        //            pcc.CC_CODE = pccdt.Rows[i]["CC_CODE"].ToString();
        //            pcc.WIRE_SIZE_MIN = pccdt.Rows[i]["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //            pcc.WIRE_SIZE_MAX = pccdt.Rows[i]["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //            pcc.OUTPUT = pccdt.Rows[i]["OUTPUT"].ToString().ToDecimalValue();
        //            DB.SubmitChanges();
        //            b1 = true;
        //        }
        //    }
        //    }
        //    catch (Exception e) {
        //        throw e.LogException();
        //    }
        //        return b1;
        //}
        //end


        //final update process sheet by nandakumar use it
        public bool UpdateProcessSheetNandakumarr(ProcessSheetModel processSheet)
        {
            bool _status = false;
            DataView dvProcessSheet = processSheet.DVProcessSheet.Table.Copy().DefaultView;
            DataView dvProcessCC = processSheet.DVProcessCC.Table.Copy().DefaultView;
            DataView dvProcessIssue = processSheet.DVProcessIssue.Table.Copy().DefaultView;
            //new by nandhu
            try
            {
                // Process Main updates
                processSheet.DVProcessMainDetails.Table.AcceptChanges();
                foreach (DataRow dr in processSheet.DVProcessMainDetails.Table.Rows)
                {
                    dvProcessSheet.RowFilter = "Convert(ROUTE_NO, 'System.String') = '" + dr["ROUTE_NO"].ToString().Trim() + "' AND Convert(SEQ_NO, 'System.String') <> ''";
                    if (processSheet.PART_NO != "" && dr["ROUTE_NO"].ToString().Trim() != "" && dvProcessSheet.Count > 0)
                    {
                        PROCESS_MAIN pm = (from o in DB.PROCESS_MAIN
                                           where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
                                           select o).FirstOrDefault<PROCESS_MAIN>();
                        try
                        {
                            if (pm == null)
                            {
                                pm = new PROCESS_MAIN();
                                pm.PART_NO = processSheet.PART_NO.Trim();
                                pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
                                pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
                                pm.AJAX_CD = dr["AJAX_CD"].ToString();
                                pm.TKO_CD = dr["TKO_CD"].ToString();
                                pm.RM_CD = dr["RM_CD"].ToString();
                                pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
                                pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
                                pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
                                pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
                                pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
                                pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
                                pm.ENTERED_BY = userInformation.UserName;
                                pm.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                pm.ROWID = Guid.NewGuid();
                                DB.PROCESS_MAIN.InsertOnSubmit(pm);
                                DB.SubmitChanges();
                            }
                            pm = null;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                        catch (Exception ex)
                        {
                            DB.PROCESS_MAIN.DeleteOnSubmit(pm);
                            throw ex.LogException();
                        }
                        pm = null;


                        pm = (from o in DB.PROCESS_MAIN
                              where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
                              select o).FirstOrDefault<PROCESS_MAIN>();
                        try
                        {

                            if (pm != null)
                            {
                                //  pm.PART_NO = processSheet.PART_NO.Trim();
                                pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
                                pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
                                pm.AJAX_CD = dr["AJAX_CD"].ToString();
                                pm.TKO_CD = dr["TKO_CD"].ToString();
                                pm.RM_CD = dr["RM_CD"].ToString();
                                pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
                                pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
                                pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
                                pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
                                pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
                                pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
                                pm.UPDATED_BY = userInformation.UserName;
                                pm.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                DB.SubmitChanges();
                            }
                            pm = null;

                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                        catch (Exception ex)
                        {
                            DB.PROCESS_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pm);
                            throw ex.LogException();
                        }
                    }
                }


                //Process Sheet Update
                dvProcessSheet.Table.AcceptChanges();
                dvProcessSheet.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(OPER_CODE, 'System.String') <> ''";

                foreach (DataRowView drv in dvProcessSheet)
                {
                    //PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
                    //                    where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
                    //                    select o).FirstOrDefault<PROCESS_SHEET>();
                    PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
                                        where o.ROWID.ToString() == drv["ROWID"].ToString()
                                        select o).FirstOrDefault<PROCESS_SHEET>();
                    try
                    {
                        if (ps == null)
                        {
                            /*
                            PROCESS_SHEET pnew = (from o in DB.PROCESS_SHEET
                                                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ROWID.ToString() == drv["ROWID"].ToString()
                                                  select o).FirstOrDefault<PROCESS_SHEET>();

                            if (pnew != null)
                            {
                                Previous_seq = Convert.ToString(pnew.SEQ_NO);
                                pnew.DELETE_FLAG = true;
                                UpdateFlag = true;
                                DB.SubmitChanges();
                            }
                            */
                            ps = new PROCESS_SHEET();
                            ps.PART_NO = processSheet.PART_NO.Trim();
                            ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
                            ps.OPN_DESC = drv["OPN_DESC"].ToString();
                            ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
                            ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
                            ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
                            ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
                            ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
                            ps.ENTERED_BY = userInformation.UserName;
                            ps.ENTERED_DATE = userInformation.Dal.ServerDateTime;
                            Current_seq = drv["SEQ_NO"].ToString();
                            ps.ROWID = Guid.NewGuid();
                            DB.PROCESS_SHEET.InsertOnSubmit(ps);
                            DB.SubmitChanges();
                        }
                        else
                        {
                            if (drv["SEQ_NO"].ToValueAsString() != ps.SEQ_NO.ToValueAsString())
                            {
                                ps.DELETE_FLAG = true;
                                DB.SubmitChanges();
                                ps = new PROCESS_SHEET();
                                ps.PART_NO = processSheet.PART_NO.Trim();
                                ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                                ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                                ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
                                ps.OPN_DESC = drv["OPN_DESC"].ToString();
                                ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
                                ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
                                ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
                                ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
                                ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
                                ps.ENTERED_BY = userInformation.UserName;
                                ps.ENTERED_DATE = userInformation.Dal.ServerDateTime;
                                Current_seq = drv["SEQ_NO"].ToString();
                                ps.ROWID = Guid.NewGuid();
                                DB.PROCESS_SHEET.InsertOnSubmit(ps);
                                DB.SubmitChanges();
                            }
                            else
                            {
                                ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                                ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                                ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
                                ps.OPN_DESC = drv["OPN_DESC"].ToString();
                                ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
                                ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
                                ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
                                ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
                                ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
                                ps.UPDATED_BY = userInformation.UserName;
                                ps.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                DB.SubmitChanges();
                            }
                        }
                        ps = null;
                    }
                    //end new me

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

                //Process Issue Update
                dvProcessIssue.Table.AcceptChanges();
                dvProcessIssue.RowFilter = "Convert(ISSUE_NO, 'System.String') <> ''";
                foreach (DataRowView drv in dvProcessIssue)
                {
                    PROCESS_ISSUE pi = (from o in DB.PROCESS_ISSUE
                                        where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
                                        select o).FirstOrDefault<PROCESS_ISSUE>();
                    try
                    {
                        if (pi == null)
                        {
                            pi = new PROCESS_ISSUE();
                            pi.PART_NO = processSheet.PART_NO.Trim();
                            pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
                            pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
                            pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
                            pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
                            //pi.ENTERED_BY = userInformation.UserName;
                            //pi.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                            pi.ROWID = Guid.NewGuid();
                            DB.PROCESS_ISSUE.InsertOnSubmit(pi);
                            DB.SubmitChanges();
                        }
                        pi = null;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        DB.PROCESS_ISSUE.DeleteOnSubmit(pi);
                        throw ex.LogException();
                    }
                    pi = null;


                    pi = (from o in DB.PROCESS_ISSUE
                          where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
                          select o).FirstOrDefault<PROCESS_ISSUE>();
                    try
                    {
                        if (pi != null)
                        {
                            // pi.PART_NO = processSheet.PART_NO.Trim();
                            pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
                            pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
                            pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
                            pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
                            //pi.UPDATED_BY = userInformation.UserName;
                            //pi.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                            DB.SubmitChanges();
                        }
                        pi = null;

                        //new updateflag process_cc

                    }

                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        DB.PROCESS_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pi);
                        throw ex.LogException();
                    }

                }

                //original code for process_cc
                //ProcessCC Update
                dvProcessCC.Table.AcceptChanges();
                dvProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";

                //List<PROCESS_CC> pccList = (from o in DB.PROCESS_CC
                //                            where o.PART_NO == processSheet.PART_NO && o.ROUTE_NO == processSheet.ROUTE_NO
                //                            select o).ToList<PROCESS_CC>();

                //foreach (PROCESS_CC pc in pccList)
                //{
                //    DB.PROCESS_CC.DeleteOnSubmit(pc);
                //    DB.SubmitChanges();
                //}

                //DB.PROCESS_CC.delete(pcc);


                foreach (DataRowView drv in dvProcessCC)
                {
                    //PROCESS_CC pcc = new PROCESS_CC();
                    PROCESS_CC pcc = (from o in DB.PROCESS_CC
                                      where o.ROWID.ToString() == drv["ROWID"].ToString()
                                      select o).FirstOrDefault<PROCESS_CC>();
                    try
                    {
                        if (pcc == null)
                        {
                            pcc = new PROCESS_CC();
                            pcc.PART_NO = processSheet.PART_NO.Trim();
                            pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            pcc.CC_SNO = GenerateSNO(drv, processSheet.PART_NO.Trim());
                            pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
                            pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
                            pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
                            pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
                            pcc.ROWID = Guid.NewGuid();
                            DB.PROCESS_CC.InsertOnSubmit(pcc);
                            DB.SubmitChanges();

                        }
                        else
                        {
                            if (pcc.SEQ_NO.ToValueAsString() != drv["SEQ_NO"].ToValueAsString())
                            {
                                DB.PROCESS_CC.DeleteOnSubmit(pcc);
                                DB.SubmitChanges();
                                pcc = new PROCESS_CC();
                                pcc.PART_NO = processSheet.PART_NO.Trim();
                                pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                                pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                                pcc.CC_SNO = GenerateSNO(drv, processSheet.PART_NO.Trim());
                                pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
                                pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
                                pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
                                pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
                                pcc.ROWID = Guid.NewGuid();
                                DB.PROCESS_CC.InsertOnSubmit(pcc);
                                DB.SubmitChanges();
                            }
                            else
                            {
                                pcc.PART_NO = processSheet.PART_NO.Trim();
                                pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                                pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                                pcc.CC_SNO = drv["CC_SNO"].ToString().ToDecimalValue();
                                pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
                                pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
                                pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
                                pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
                                DB.SubmitChanges();
                            }
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
                _status = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                _status = false;
                throw ex.LogException();
            }
            return _status;
        }
        //end function edited by nandakumar update process sheet use it


        //UpdateProcessSheet original funtion
        //public bool UpdateProcessSheet(ProcessSheetModel processSheet)
        //{

        //    bool _status = false;
        //    DataView dvProcessSheet = processSheet.DVProcessSheet.Table.Copy().DefaultView;
        //    DataView dvProcessCC = processSheet.DVProcessCC.Table.Copy().DefaultView;
        //    DataView dvProcessIssue = processSheet.DVProcessIssue.Table.Copy().DefaultView;


        //    try
        //    {
        //        // Process Main updates
        //        processSheet.DVProcessMainDetails.Table.AcceptChanges();
        //        foreach (DataRow dr in processSheet.DVProcessMainDetails.Table.Rows)
        //        {
        //            dvProcessSheet.RowFilter = "Convert(ROUTE_NO, 'System.String') = '" + dr["ROUTE_NO"].ToString().Trim() + "' AND Convert(SEQ_NO, 'System.String') <> ''";

        //            if (processSheet.PART_NO != "" && dr["ROUTE_NO"].ToString().Trim() != "" && dvProcessSheet.Count > 0)
        //            {


        //                PROCESS_MAIN pm = (from o in DB.PROCESS_MAIN
        //                                   where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
        //                                   select o).FirstOrDefault<PROCESS_MAIN>();
        //                try
        //                {

        //                    if (pm == null)
        //                    {
        //                        pm = new PROCESS_MAIN();
        //                        pm.PART_NO = processSheet.PART_NO.Trim();
        //                        pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
        //                        pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
        //                        pm.AJAX_CD = dr["AJAX_CD"].ToString();
        //                        pm.TKO_CD = dr["TKO_CD"].ToString();
        //                        pm.RM_CD = dr["RM_CD"].ToString();
        //                        pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
        //                        pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
        //                        pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
        //                        pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
        //                        pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
        //                        pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
        //                        pm.ENTERED_BY = userInformation.UserName;
        //                        pm.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                        pm.ROWID = Guid.NewGuid();
        //                        DB.PROCESS_MAIN.InsertOnSubmit(pm);
        //                        DB.SubmitChanges();
        //                    }
        //                    pm = null;
        //                }
        //                catch (System.Data.Linq.ChangeConflictException)
        //                {
        //                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.PROCESS_MAIN.DeleteOnSubmit(pm);
        //                    throw ex.LogException();
        //                }
        //                pm = null;


        //                pm = (from o in DB.PROCESS_MAIN
        //                      where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
        //                      select o).FirstOrDefault<PROCESS_MAIN>();
        //                try
        //                {

        //                    if (pm != null)
        //                    {
        //                        //  pm.PART_NO = processSheet.PART_NO.Trim();
        //                        pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
        //                        pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
        //                        pm.AJAX_CD = dr["AJAX_CD"].ToString();
        //                        pm.TKO_CD = dr["TKO_CD"].ToString();
        //                        pm.RM_CD = dr["RM_CD"].ToString();
        //                        pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
        //                        pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
        //                        pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
        //                        pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
        //                        pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
        //                        pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
        //                        pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
        //                        pm.UPDATED_BY = userInformation.UserName;
        //                        pm.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                        DB.SubmitChanges();
        //                    }
        //                    pm = null;

        //                }
        //                catch (System.Data.Linq.ChangeConflictException)
        //                {
        //                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.PROCESS_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pm);
        //                    throw ex.LogException();
        //                }
        //            }
        //        }


        //        //Process Sheet Update
        //        dvProcessSheet.Table.AcceptChanges();
        //        dvProcessSheet.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(OPER_CODE, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessSheet)
        //        {



        //            PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
        //                                where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //                                select o).FirstOrDefault<PROCESS_SHEET>();
        //            try
        //            {

        //                if (ps == null)
        //                {
        //                    ps = new PROCESS_SHEET();
        //                    ps.PART_NO = processSheet.PART_NO.Trim();
        //                    ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
        //                    ps.OPN_DESC = drv["OPN_DESC"].ToString();
        //                    ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
        //                    ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
        //                    ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
        //                    ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.ENTERED_BY = userInformation.UserName;
        //                    ps.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    ps.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_SHEET.InsertOnSubmit(ps);
        //                    DB.SubmitChanges();
        //                }
        //                ps = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_SHEET.DeleteOnSubmit(ps);
        //                throw ex.LogException();
        //            }
        //            ps = null;


        //            ps = (from o in DB.PROCESS_SHEET
        //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
        //                  select o).FirstOrDefault<PROCESS_SHEET>();

        //            try
        //            {

        //                if (ps != null)
        //                {
        //                    // ps.PART_NO = processSheet.PART_NO.Trim();
        //                    ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
        //                    ps.OPN_DESC = drv["OPN_DESC"].ToString();
        //                    ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
        //                    ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
        //                    ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
        //                    ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
        //                    ps.UPDATED_BY = userInformation.UserName;
        //                    ps.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                ps = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_SHEET.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ps);
        //                throw ex.LogException();
        //            }

        //        }

        //        //Process Issue Update
        //        dvProcessIssue.Table.AcceptChanges();
        //        dvProcessIssue.RowFilter = "Convert(ISSUE_NO, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessIssue)
        //        {



        //            PROCESS_ISSUE pi = (from o in DB.PROCESS_ISSUE
        //                                where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
        //                                select o).FirstOrDefault<PROCESS_ISSUE>();
        //            try
        //            {

        //                if (pi == null)
        //                {
        //                    pi = new PROCESS_ISSUE();
        //                    pi.PART_NO = processSheet.PART_NO.Trim();
        //                    pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
        //                    pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
        //                    pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
        //                    pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
        //                    //pi.ENTERED_BY = userInformation.UserName;
        //                    //pi.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    pi.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_ISSUE.InsertOnSubmit(pi);
        //                    DB.SubmitChanges();
        //                }
        //                pi = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_ISSUE.DeleteOnSubmit(pi);
        //                throw ex.LogException();
        //            }
        //            pi = null;


        //            pi = (from o in DB.PROCESS_ISSUE
        //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
        //                  select o).FirstOrDefault<PROCESS_ISSUE>();

        //            try
        //            {

        //                if (pi != null)
        //                {
        //                    // pi.PART_NO = processSheet.PART_NO.Trim();
        //                    pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
        //                    pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
        //                    pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
        //                    pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
        //                    //pi.UPDATED_BY = userInformation.UserName;
        //                    //pi.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pi = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pi);
        //                throw ex.LogException();
        //            }

        //        }


        //        //ProcessCC Update
        //        dvProcessCC.Table.AcceptChanges();
        //        dvProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";
        //        foreach (DataRowView drv in dvProcessCC)
        //        {

        //            //PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //            //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //            //                  select o).FirstOrDefault<PROCESS_CC>();

        //            PROCESS_CC pcc = (from o in DB.PROCESS_CC
        //                              where o.ROWID.ToString() == drv["ROWID"].ToString()
        //                              select o).FirstOrDefault<PROCESS_CC>();

        //            try
        //            {

        //                if (pcc == null)
        //                {
        //                    pcc = new PROCESS_CC();
        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = GenerateSNO(drv, processSheet.PART_NO.Trim());
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;

        //                    pcc.ROWID = Guid.NewGuid();
        //                    DB.PROCESS_CC.InsertOnSubmit(pcc);
        //                    DB.SubmitChanges();
        //                }
        //                else
        //                {
        //                    pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_SNO = drv["CC_SNO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.ENTERED_BY = userInformation.UserName;
        //                    //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pcc = null;
        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.DeleteOnSubmit(pcc);
        //                throw ex.LogException();
        //            }
        //            pcc = null;


        //            pcc = (from o in DB.PROCESS_CC
        //                   where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
        //                   select o).FirstOrDefault<PROCESS_CC>();

        //            try
        //            {

        //                if (pcc != null)
        //                {
        //                    //  pcc.PART_NO = processSheet.PART_NO.Trim();
        //                    pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
        //                    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
        //                    pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
        //                    pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
        //                    pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
        //                    pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
        //                    //pcc.UPDATED_BY = userInformation.UserName;
        //                    //pcc.UPDATED_DATE = userInformation.Dal.ServerDateTime;
        //                    DB.SubmitChanges();
        //                }
        //                pcc = null;

        //            }
        //            catch (System.Data.Linq.ChangeConflictException)
        //            {
        //                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //            }
        //            catch (Exception ex)
        //            {
        //                DB.PROCESS_CC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pcc);
        //                throw ex.LogException();
        //            }

        //        }

        //        _status = true;
        //    }
        //    catch (System.Data.Linq.ChangeConflictException)
        //    {
        //        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
        //    }
        //    catch (Exception ex)
        //    {
        //        _status = false;
        //        throw ex.LogException();
        //    }
        //    return _status;
        //}
        //end original

        public bool UpdateProcessSheet(ProcessSheetModel processSheet)
        {

            bool _status = false;
            DataView dvProcessSheet = processSheet.DVProcessSheet.Table.Copy().DefaultView;
            DataView dvProcessCC = processSheet.DVProcessCC.Table.Copy().DefaultView;
            DataView dvProcessIssue = processSheet.DVProcessIssue.Table.Copy().DefaultView;

            //new by nandhu


            try
            {
                // Process Main updates
                processSheet.DVProcessMainDetails.Table.AcceptChanges();
                foreach (DataRow dr in processSheet.DVProcessMainDetails.Table.Rows)
                {
                    dvProcessSheet.RowFilter = "Convert(ROUTE_NO, 'System.String') = '" + dr["ROUTE_NO"].ToString().Trim() + "' AND Convert(SEQ_NO, 'System.String') <> ''";

                    if (processSheet.PART_NO != "" && dr["ROUTE_NO"].ToString().Trim() != "" && dvProcessSheet.Count > 0)
                    {


                        PROCESS_MAIN pm = (from o in DB.PROCESS_MAIN
                                           where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
                                           select o).FirstOrDefault<PROCESS_MAIN>();
                        try
                        {

                            if (pm == null)
                            {
                                pm = new PROCESS_MAIN();
                                pm.PART_NO = processSheet.PART_NO.Trim();
                                pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
                                pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
                                pm.AJAX_CD = dr["AJAX_CD"].ToString();
                                pm.TKO_CD = dr["TKO_CD"].ToString();
                                pm.RM_CD = dr["RM_CD"].ToString();
                                pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
                                pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
                                pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
                                pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
                                pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
                                pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
                                pm.ENTERED_BY = userInformation.UserName;
                                pm.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                pm.ROWID = Guid.NewGuid();
                                DB.PROCESS_MAIN.InsertOnSubmit(pm);
                                DB.SubmitChanges();
                            }
                            pm = null;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                        catch (Exception ex)
                        {
                            DB.PROCESS_MAIN.DeleteOnSubmit(pm);
                            throw ex.LogException();
                        }
                        pm = null;


                        pm = (from o in DB.PROCESS_MAIN
                              where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == dr["ROUTE_NO"].ToString().ToDecimalValue()
                              select o).FirstOrDefault<PROCESS_MAIN>();
                        try
                        {

                            if (pm != null)
                            {
                                //  pm.PART_NO = processSheet.PART_NO.Trim();
                                pm.ROUTE_NO = dr["ROUTE_NO"].ToString().ToDecimalValue();
                                pm.CURRENT_PROC = dr["CURRENT_PROC"].ToString().ToDecimalValue();
                                pm.AJAX_CD = dr["AJAX_CD"].ToString();
                                pm.TKO_CD = dr["TKO_CD"].ToString();
                                pm.RM_CD = dr["RM_CD"].ToString();
                                pm.ALT_RM_CD = dr["ALT_RM_CD"].ToString();
                                pm.ALT_RM_CD1 = dr["ALT_RM_CD1"].ToString();
                                pm.WIRE_ROD_CD = dr["WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD = dr["ALT_WIRE_ROD_CD"].ToString();
                                pm.ALT_WIRE_ROD_CD1 = dr["ALT_WIRE_ROD_CD1"].ToString();
                                pm.CHEESE_WT = dr["CHEESE_WT"].ToString().ToDecimalValue();
                                pm.CHEESE_WT_EST = dr["CHEESE_WT_EST"].ToString().ToDecimalValue();
                                pm.RM_WT = dr["RM_WT"].ToString().ToDecimalValue();
                                pm.UPDATED_BY = userInformation.UserName;
                                pm.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                DB.SubmitChanges();
                            }
                            pm = null;

                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                        catch (Exception ex)
                        {
                            DB.PROCESS_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pm);
                            throw ex.LogException();
                        }
                    }
                }


                //Process Sheet Update
                dvProcessSheet.Table.AcceptChanges();
                dvProcessSheet.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(OPER_CODE, 'System.String') <> ''";

                foreach (DataRowView drv in dvProcessSheet)
                {

                    //new by me
                    //DataTable dt = new DataTable();
                    //DataView tempProcessSheetview = processSheet.DVProcessSheet.Table.Copy().DefaultView;
                    //dt = tempProcessSheetview.ToTable();
                    //DataTable copyDataTable;

                    //copyDataTable = dt.Copy();
                    //var toRemove = new string[] { "OPER_CODE", "OPN_DESC", "TRANSPORT_CD", "FMEA_RISK", "UNIT_OF_MEASURE", "GROSS_WEIGHT", "NET_WEIGHT", "SORTCOL", "ROWID" };
                    //foreach (var col in toRemove)
                    //    copyDataTable.Columns.Remove(col);

                    //string partNo = processSheet.PART_NO;
                    //int routeNo = processSheet.ROUTE_NO.ToString().ToIntValue();
                    ////Dal.SeqNoUpdate(partNo, routeNo, copyDataTable);
                    //Dal.UpdateSeqNo(partNo, routeNo);


                    //dt = ToDataTable((from o in DB.PROCESS_SHEET
                    //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue()
                    //                  select new { o.PART_NO, o.ROUTE_NO, o.SEQ_NO }).ToList());

                    //end new

                    PROCESS_SHEET ps = (from o in DB.PROCESS_SHEET
                                        where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
                                        select o).FirstOrDefault<PROCESS_SHEET>();
                    //DataTable dt = new DataTable();
                    //dt = ToDataTable((from o in DB.PROCESS_SHEET
                    //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
                    //                  select new { o.PART_NO, o.ROUTE_NO, o.SEQ_NO }).ToList());

                    
                    try
                    {

                        if (ps == null)
                        {
                            PROCESS_SHEET pnew = (from o in DB.PROCESS_SHEET
                                                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ROWID.ToString() == drv["ROWID"].ToString()
                                                  select o).FirstOrDefault<PROCESS_SHEET>();

                            if (pnew != null)
                            {
                                Previous_seq = Convert.ToString(pnew.SEQ_NO);
                                pnew.DELETE_FLAG = true;
                                UpdateFlag = true;
                                DB.SubmitChanges();
                            }

                            ps = new PROCESS_SHEET();
                            ps.PART_NO = processSheet.PART_NO.Trim();
                            ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
                            ps.OPN_DESC = drv["OPN_DESC"].ToString();
                            ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
                            ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
                            ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
                            ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
                            ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
                            ps.ENTERED_BY = userInformation.UserName;
                            ps.ENTERED_DATE = userInformation.Dal.ServerDateTime;
                            //new
                            if (pnew != null)
                            {
                                Current_seq = drv["SEQ_NO"].ToString();
                                if (UpdateFlag == true)
                                {
                                    ps.DELETE_FLAG = false;

                                }
                            }
                            //new end
                            ps.ROWID = Guid.NewGuid();
                            DB.PROCESS_SHEET.InsertOnSubmit(ps);
                            DB.SubmitChanges();
                            //if (UpdateFlag == true)
                            //{
                            //    string partNo = ProcessSheet.PART_NO;
                            //    int routeNo = ProcessSheet.ROUTE_NO.ToString().ToIntValue();
                            //    System.Resources.ResourceManager myManager;
                            //    myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                            //    string conStr = myManager.GetString("ConnectionString");
                            //    DataAccessLayer dal = new DataAccessLayer(conStr);
                            //    dal.UpdateSeqNo(partNo, routeNo);
                            //    ProcessSheetViewModel viewmodel = new ProcessSheetViewModel();
                            //string partNo = processSheet.PART_NO;
                            //int routeNo = processSheet.ROUTE_NO.ToString().ToIntValue();
                            //Dal.UpdateSeqNo(partNo, routeNo);

                            //}
                            //new ny me
                            //Current_seq = ps.SEQ_NO;
                            //Current_seq = drv["SEQ_NO"].ToString().ToDecimalValue();
                            //end new
                        }
                        ps = null;
                    }
                    //end new me

                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        DB.PROCESS_SHEET.DeleteOnSubmit(ps);
                        throw ex.LogException();
                    }
                    ps = null;

                    ps = (from o in DB.PROCESS_SHEET
                          where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue()
                          select o).FirstOrDefault<PROCESS_SHEET>();
                    var t = drv["ROWID"].ToString();



                    try
                    {

                        if (ps != null)
                        {

                            // ps = new PROCESS_SHEET();//by me
                            // ps.PART_NO = processSheet.PART_NO.Trim();//old
                            ps.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            ps.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            ps.OPN_CD = drv["OPER_CODE"].ToString().ToDecimalValue();
                            ps.OPN_DESC = drv["OPN_DESC"].ToString();
                            ps.TRANSPORT = drv["TRANSPORT_CD"].ToString().ToDecimalValue();
                            ps.FMEA_RISK = drv["FMEA_RISK"].ToString();
                            ps.UNIT_OF_MEASURE = drv["UNIT_OF_MEASURE"].ToString();
                            ps.GROSS_WEIGHT = drv["GROSS_WEIGHT"].ToString().ToDecimalValue();
                            ps.NET_WEIGHT = drv["NET_WEIGHT"].ToString().ToDecimalValue();
                            ps.UPDATED_BY = userInformation.UserName;
                            ps.UPDATED_DATE = userInformation.Dal.ServerDateTime;
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

                }

                //Process Issue Update
                dvProcessIssue.Table.AcceptChanges();
                dvProcessIssue.RowFilter = "Convert(ISSUE_NO, 'System.String') <> ''";
                foreach (DataRowView drv in dvProcessIssue)
                {



                    PROCESS_ISSUE pi = (from o in DB.PROCESS_ISSUE
                                        where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
                                        select o).FirstOrDefault<PROCESS_ISSUE>();
                    try
                    {

                        if (pi == null)
                        {
                            pi = new PROCESS_ISSUE();
                            pi.PART_NO = processSheet.PART_NO.Trim();
                            pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
                            pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
                            pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
                            pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
                            //pi.ENTERED_BY = userInformation.UserName;
                            //pi.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                            pi.ROWID = Guid.NewGuid();
                            DB.PROCESS_ISSUE.InsertOnSubmit(pi);
                            DB.SubmitChanges();
                        }
                        pi = null;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        DB.PROCESS_ISSUE.DeleteOnSubmit(pi);
                        throw ex.LogException();
                    }
                    pi = null;


                    pi = (from o in DB.PROCESS_ISSUE
                          where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.ISSUE_NO == drv["ISSUE_NO"].ToString()
                          select o).FirstOrDefault<PROCESS_ISSUE>();

                    try
                    {

                        if (pi != null)
                        {
                            // pi.PART_NO = processSheet.PART_NO.Trim();
                            pi.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            pi.ISSUE_NO = drv["ISSUE_NO"].ToString();
                            pi.ISSUE_DATE = ConvertToDateTime(drv["ISSUE_DATE"]);
                            pi.ISSUE_ALTER = drv["ISSUE_ALTER"].ToString();
                            pi.COMPILED_BY = drv["COMPILED_BY"].ToString();
                            //pi.UPDATED_BY = userInformation.UserName;
                            //pi.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                            DB.SubmitChanges();
                        }
                        pi = null;

                        //new updateflag process_cc

                    }

                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    }
                    catch (Exception ex)
                    {
                        DB.PROCESS_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, pi);
                        throw ex.LogException();
                    }

                }

                //original code for process_cc
                //ProcessCC Update
                dvProcessCC.Table.AcceptChanges();
                dvProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";


                foreach (DataRowView drv in dvProcessCC)
                {
                    //uncomment by me
                    //PROCESS_CC pcc = (from o in DB.PROCESS_CC
                    //                  where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
                    //                  select o).FirstOrDefault<PROCESS_CC>();
                    //uncomment by me

                    //comment by me
                    PROCESS_CC pcc = (from o in DB.PROCESS_CC
                                      where o.ROWID.ToString() == drv["ROWID"].ToString()
                                      select o).FirstOrDefault<PROCESS_CC>();
                    //comment by me




                    try
                    {

                        if (pcc == null)
                        {
                            pcc = new PROCESS_CC();
                            pcc.PART_NO = processSheet.PART_NO.Trim();
                            pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();

                            pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            pcc.CC_SNO = GenerateSNO(drv, processSheet.PART_NO.Trim());
                            pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
                            pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
                            pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
                            pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
                            //pcc.ENTERED_BY = userInformation.UserName;
                            //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                            pcc.ROWID = Guid.NewGuid();
                            DB.PROCESS_CC.InsertOnSubmit(pcc);
                            DB.SubmitChanges();
                        }


                        else
                        {

                            //new edited
                            //if (pcc.SEQ_NO == Convert.ToDecimal(Previous_seq))
                            //{
                            //    drv["SEQ_NO"] = Convert.ToDecimal(Current_seq);
                            //}
                            //end new
                            pcc.PART_NO = processSheet.PART_NO.Trim();
                            pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            pcc.CC_SNO = drv["CC_SNO"].ToString().ToDecimalValue();
                            pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
                            pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
                            pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
                            pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
                            //pcc.ENTERED_BY = userInformation.UserName;
                            //pcc.ENTERED_DATE = userInformation.Dal.ServerDateTime;
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


                    pcc = (from o in DB.PROCESS_CC
                           where o.PART_NO == processSheet.PART_NO.Trim() && o.ROUTE_NO == drv["ROUTE_NO"].ToString().ToDecimalValue() && o.SEQ_NO == drv["SEQ_NO"].ToString().ToDecimalValue() && o.CC_CODE == drv["COST_CENT_CODE"].ToString()
                           select o).FirstOrDefault<PROCESS_CC>();

                    try
                    {

                        if (pcc != null)
                        {

                            //  pcc.PART_NO = processSheet.PART_NO.Trim();//original comment
                            pcc.ROUTE_NO = drv["ROUTE_NO"].ToString().ToDecimalValue();
                            //new ny nandhini
                            //if (pcc.SEQ_NO == Convert.ToDecimal(Previous_seq))
                            //{
                            //    drv["SEQ_NO"] = Convert.ToDecimal(Current_seq);
                            //}
                            //else
                            //{
                            //    pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            //}
                            //end new
                            pcc.SEQ_NO = drv["SEQ_NO"].ToString().ToDecimalValue();
                            pcc.CC_CODE = drv["COST_CENT_CODE"].ToString();
                            pcc.WIRE_SIZE_MIN = drv["WIRE_SIZE_MIN"].ToString().ToDecimalValue();
                            pcc.WIRE_SIZE_MAX = drv["WIRE_SIZE_MAX"].ToString().ToDecimalValue();
                            pcc.OUTPUT = drv["OUTPUT"].ToString().ToDecimalValue();
                            //pcc.UPDATED_BY = userInformation.UserName;
                            //pcc.UPDATED_DATE = userInformation.Dal.ServerDateTime;
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

                }

                _status = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                _status = false;
                throw ex.LogException();
            }
            return _status;
        }
    }
}