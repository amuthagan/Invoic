using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class PCCSBll : Essential
    {
        public PCCSBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool GetPartNoDetails(PCCSModel pccsModel)
        {
            try
            {
                DataTable dt = new DataTable();
                // DB.PRD_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.PRD_MAST);
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                if (dt != null)
                {
                    pccsModel.PartNoDetails = dt.DefaultView;
                    //  pccsModel.PartNoDetails.AddNew();
                }
                else
                {
                    pccsModel.PartNoDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;

            }
        }
        public bool GetCurrentProcessByPartNo(PCCSModel pccsModel)
        {
            try
            {

                if (pccsModel.PartNo.IsNotNullOrEmpty())
                {
                    PROCESS_MAIN processMain = (from c in DB.PROCESS_MAIN
                                                where c.PART_NO == pccsModel.PartNo && c.CURRENT_PROC == 1
                                                select c).FirstOrDefault<PROCESS_MAIN>();

                    if (processMain != null)
                    {
                        pccsModel.RouteNo = processMain.ROUTE_NO;
                    }
                    else if (processMain == null)
                    {
                        return false;
                    }
                }
                return true;


            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
            return true;
        }
        public bool GetRouteNoDetailsByPartNo(PCCSModel pccsModel, string partNo)
        {
            try
            {
                DataTable dt = new DataTable();
                if (partNo.IsNotNullOrEmpty())
                {
                    pccsModel.RouteNo = 0;
                    pccsModel.RouteNoDetails = null;

                    dt = ToDataTable((from o in DB.PROCESS_MAIN
                                      where o.PART_NO == partNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select new { o.ROUTE_NO }).ToList());

                }
                if (dt != null)
                {
                    //if (dt.Rows.Count > 0) pccsModel.RouteNo = Convert.ToDecimal(dt.Rows[0]["ROUTE_NO"].ToString());
                    pccsModel.RouteNoDetails = dt.DefaultView;

                    // pccsModel.RouteNoDetails.AddNew();
                }
                else
                {
                    pccsModel.RouteNoDetails = null;
                }


                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetSequenceNoDetailsByPartNoRouteNo(PCCSModel pccsModel, string partNo, decimal routeNo)
        {
            try
            {
                DataTable dt = new DataTable();
                if (partNo.IsNotNullOrEmpty())
                {
                    dt = ToDataTable((from o in DB.PROCESS_SHEET
                                      where o.PART_NO == partNo && o.ROUTE_NO == routeNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select new { o.SEQ_NO, o.OPN_DESC }).ToList());

                }
                if (dt != null)
                {
                    pccsModel.SeqNoDetails = dt.DefaultView;
                    try
                    {
                        if (pccsModel.SeqNoDetails.IsNotNullOrEmpty())
                        {
                            if (pccsModel.SeqNoDetails.Table.Columns.IndexOf("SnoSort") < 0)
                                pccsModel.SeqNoDetails.Table.Columns.Add("SnoSort", typeof(decimal));
                            for (int i = 0; i < pccsModel.SeqNoDetails.Count; i++)
                            {
                                pccsModel.SeqNoDetails[i]["SnoSort"] = pccsModel.SeqNoDetails[i]["SEQ_NO"];
                            }
                            pccsModel.SeqNoDetails.Sort = "SnoSort ASC";
                            foreach (DataRowView item in pccsModel.SeqNoDetails)
                            {
                                item["SnoSort"] = item["SEQ_NO"];

                            }
                            pccsModel.SeqNoDetails.Sort = "SnoSort ASC";

                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }
                    PROCESS_SHEET procesSheet = (from o in DB.PROCESS_SHEET
                                                 where o.PART_NO == partNo && o.ROUTE_NO == routeNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                 //where o.PART_NO == partNo && o.ROUTE_NO == routeNo && o.SEQ_NO == 20 && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                 select o).FirstOrDefault<PROCESS_SHEET>();
                    if (procesSheet.IsNotNullOrEmpty())
                    {
                        pccsModel.SeqNo = procesSheet.SEQ_NO.ToValueAsString();
                        pccsModel.OperDesc = procesSheet.OPN_DESC.ToValueAsString();
                    }
                    else
                    {
                        if (pccsModel.SeqNoDetails.Count > 0)
                        {
                            pccsModel.SeqNo = pccsModel.SeqNoDetails.Table.Rows[0]["SEQ_NO"].ToString();
                            pccsModel.OperDesc = pccsModel.SeqNoDetails.Table.Rows[0]["OPN_DESC"].ToString();
                        }
                    }
                    //  pccsModel.SeqNoDetails.AddNew();
                }
                else
                {
                    pccsModel.SeqNoDetails = null;
                }


                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetPccsDetails(PCCSModel pccsModel, string partNo, decimal? routeNo = 1, string seqNo = "0")
        {
            try
            {
                DataTable dt = new DataTable();

                if (partNo.IsNotNullOrEmpty() && seqNo.IsNotNullOrEmpty() && seqNo != "0")
                {
                    dt = ToDataTable((from o in DB.PCCS
                                      where o.PART_NO == partNo && o.ROUTE_NO == routeNo && o.SEQ_NO == Convert.ToDecimal(seqNo) && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      orderby o.ROUTE_NO, o.SEQ_NO, o.SNO
                                      select new { o.PART_NO, o.ROUTE_NO, o.SEQ_NO, o.SNO, o.ISR_NO, o.FEATURE, o.PROCESS_FEATURE, o.SPEC_MIN, o.SPEC_MAX, o.CTRL_SPEC_MIN, o.CTRL_SPEC_MAX, o.SPEC_CHAR, o.DEPT_RESP, o.GAUGES_USED, o.SAMPLE_SIZE, o.FREQ_OF_INSP, o.CONTROL_METHOD, o.REACTION_PLAN }).ToList());

                }
                else
                {
                    dt = ToDataTable((from o in DB.PCCS
                                      where o.PART_NO == partNo && o.ROUTE_NO == routeNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      orderby o.ROUTE_NO, o.SEQ_NO, o.SNO
                                      select new { o.PART_NO, o.ROUTE_NO, o.SEQ_NO, o.SNO, o.ISR_NO, o.FEATURE, o.PROCESS_FEATURE, o.SPEC_MIN, o.SPEC_MAX, o.CTRL_SPEC_MIN, o.CTRL_SPEC_MAX, o.SPEC_CHAR, o.DEPT_RESP, o.GAUGES_USED, o.SAMPLE_SIZE, o.FREQ_OF_INSP, o.CONTROL_METHOD, o.REACTION_PLAN }).ToList());

                }
                if (dt != null && dt.Rows.Count == 0 && seqNo != "0")
                {
                    pccsModel.PCCSDetails = dt.DefaultView;
                    DataRowView drv = pccsModel.PCCSDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = pccsModel.PCCSDetails.Count;
                    drv.EndEdit();
                    pccsModel.PCCSDetails.EndInit();
                    pccsModel.EditGenBtn = "Generate F5";
                }
                else if (dt.Rows.Count == 0 && seqNo == "0")
                {
                    pccsModel.PCCSDetails = dt.DefaultView;
                    DataRowView drv = pccsModel.PCCSDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = 1;
                    drv.EndEdit();
                    pccsModel.PCCSDetails.EndInit();
                    pccsModel.EditGenBtn = "Generate F5";
                }
                else if (dt.Rows.Count != 0 && seqNo == "0")
                {
                    pccsModel.PCCSDetails = dt.DefaultView;
                    DataRowView drv = pccsModel.PCCSDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = pccsModel.PCCSDetails.Count;
                    drv.EndEdit();
                    pccsModel.PCCSDetails.EndInit();
                    pccsModel.EditGenBtn = "Edit F5";
                }
                else if (dt.Rows.Count > 0 && seqNo != "0")
                {
                    pccsModel.PCCSDetails = dt.DefaultView;
                    DataRowView drv = pccsModel.PCCSDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = pccsModel.PCCSDetails.Count;
                    drv.EndEdit();
                    pccsModel.PCCSDetails.EndInit();
                    pccsModel.EditGenBtn = "Edit F5";
                }
                else
                {
                    pccsModel.PCCSDetails = null;
                }
                 
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool GetPccsRevisonDetails(PCCSModel pccsModel, string partNo, decimal? routeNo)
        {
            try
            {
                DataTable dt = new DataTable();
                if (partNo.IsNotNullOrEmpty())
                {

                    dt = ToDataTableWithType((from o in DB.PCCS_ISSUE
                                              where o.PART_NO == partNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              && o.ROUTE_NO == routeNo
                                              orderby o.ISSUE_NO
                                              select new { o.PART_NO, o.ROUTE_NO, ISSUE_NO = o.ISSUE_NO, o.ISSUE_DATE, o.ISSUE_ALTER, o.COMPILED_BY }).ToList());
                }

                //pccsModel.PccsRevisionDetails = new DataView();
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
                pccsModel.PccsRevisionDetails = (dtFinal != null) ? dtFinal.DefaultView : null;
                pccsModel.PccsRevisionDetails.Sort = "";

                //pccsModel.PccsRevisionDetails = dt.DefaultView;
                DataRowView drv = pccsModel.PccsRevisionDetails.AddNew();
                if (dt != null && partNo != "")
                {
                    if (pccsModel.PccsRevisionDetails.Count > 0)
                    {
                        drv.BeginEdit();
                        // drv["ISSUE_NO"] = pccsModel.PccsRevisionDetails.Count;
                        drv["ISSUE_NONUMERIC"] = pccsModel.PccsRevisionDetails.Count;
                        drv.EndEdit();
                        pccsModel.PccsRevisionDetails.EndInit();
                    }
                }
                else
                {
                    pccsModel.PccsRevisionDetails = null;
                }


                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool SavePccs(PCCSModel pccsModel, ref string typ)
        {
            try
            {
                DataTable dtPccsDetails = new DataTable();
                DataTable dtPccsRevisonDetails = new DataTable();
                bool _status = false;

                if (pccsModel.PartNo.IsNotNullOrEmpty() && pccsModel.SeqNo.IsNotNullOrEmpty() && pccsModel.RouteNo.IsNotNullOrEmpty())
                {

                    if (pccsModel.PCCSDetails.IsNotNullOrEmpty() && pccsModel.PCCSDetails.Count > 0)
                    {
                        //Save PCCS Details
                        DataTable dt = new DataTable();
                        List<PCCS> lstexistingDatas = new List<PCCS>();
                        dtPccsDetails = pccsModel.PCCSDetails.Table;
                        lstexistingDatas = ((from c in DB.PCCS
                                             where c.PART_NO == pccsModel.PartNo && c.ROUTE_NO == pccsModel.RouteNo && c.SEQ_NO == Convert.ToDecimal(pccsModel.SeqNo)
                                             select c).ToList());
                        if (lstexistingDatas.Count > 0)
                        {
                            DB.PCCS.DeleteAllOnSubmit(lstexistingDatas);
                            DB.SubmitChanges();
                        }

                        for (int i = 0; i < dtPccsDetails.Rows.Count; i++)
                        {
                            PCCS pccs = null;
                            try
                            {
                                if (dtPccsDetails.Rows[i]["REACTION_PLAN"].ToString() != "" || dtPccsDetails.Rows[i]["CONTROL_METHOD"].ToString() != "" || dtPccsDetails.Rows[i]["FREQ_OF_INSP"].ToString() != "" || dtPccsDetails.Rows[i]["GAUGES_USED"].ToString() != "" || dtPccsDetails.Rows[i]["DEPT_RESP"].ToString() != "" || dtPccsDetails.Rows[i]["SPEC_CHAR"].ToString() != "" || dtPccsDetails.Rows[i]["CTRL_SPEC_MAX"].ToString() != "" || dtPccsDetails.Rows[i]["CTRL_SPEC_MIN"].ToString() != "" || dtPccsDetails.Rows[i]["SPEC_MAX"].ToString() != "" || dtPccsDetails.Rows[i]["SPEC_MIN"].ToString() != "" || dtPccsDetails.Rows[i]["PROCESS_FEATURE"].ToString() != "" || dtPccsDetails.Rows[i]["ISR_NO"].ToValueAsString().ToDecimalValue() != 0 || dtPccsDetails.Rows[i]["FEATURE"].ToString() != "")
                                {
                                    pccs = new PCCS();
                                    pccs.PART_NO = pccsModel.PartNo.ToString();
                                    pccs.ROUTE_NO = pccsModel.RouteNo.ToValueAsString().ToDecimalValue();
                                    pccs.SEQ_NO = pccsModel.SeqNo.IsNumeric() ? Convert.ToDecimal(pccsModel.SeqNo) : 0;
                                    //pccs.SNO = dtPccsDetails.Rows[i]["SNO"].ToString().IsNumeric() ? Convert.ToDouble(dtPccsDetails.Rows[i]["SNO"].ToString()) : 0;
                                    pccs.SNO = dtPccsDetails.Rows[i]["SNO"].ToValueAsString().ToDoubleValue();
                                    pccs.ISR_NO = dtPccsDetails.Rows[i]["ISR_NO"].ToValueAsString().ToDecimalValue();
                                    pccs.FEATURE = dtPccsDetails.Rows[i]["FEATURE"].ToString();
                                    pccs.PROCESS_FEATURE = dtPccsDetails.Rows[i]["PROCESS_FEATURE"].ToString();
                                    pccs.SPEC_MIN = dtPccsDetails.Rows[i]["SPEC_MIN"].ToString();
                                    pccs.SPEC_MAX = dtPccsDetails.Rows[i]["SPEC_MAX"].ToString();
                                    pccs.CTRL_SPEC_MIN = dtPccsDetails.Rows[i]["CTRL_SPEC_MIN"].ToString();
                                    pccs.CTRL_SPEC_MAX = dtPccsDetails.Rows[i]["CTRL_SPEC_MAX"].ToString();
                                    pccs.SPEC_CHAR = dtPccsDetails.Rows[i]["SPEC_CHAR"].ToString();
                                    pccs.DEPT_RESP = dtPccsDetails.Rows[i]["DEPT_RESP"].ToString();
                                    pccs.GAUGES_USED = dtPccsDetails.Rows[i]["GAUGES_USED"].ToString();
                                    //pccs.SAMPLE_SIZE = dtPccsDetails.Rows[i]["SAMPLE_SIZE"].ToString().IsNumeric() ? Convert.ToDecimal(dtPccsDetails.Rows[i]["SAMPLE_SIZE"].ToString()) : 0;
                                    pccs.SAMPLE_SIZE = dtPccsDetails.Rows[i]["SAMPLE_SIZE"].ToValueAsString();
                                    pccs.FREQ_OF_INSP = dtPccsDetails.Rows[i]["FREQ_OF_INSP"].ToString();
                                    pccs.CONTROL_METHOD = dtPccsDetails.Rows[i]["CONTROL_METHOD"].ToString();
                                    pccs.REACTION_PLAN = dtPccsDetails.Rows[i]["REACTION_PLAN"].ToString();
                                    pccs.DELETE_FLAG = false;
                                    pccs.ENTERED_DATE = DateTime.Now;
                                    pccs.ENTERED_BY = userInformation.UserName;
                                    pccs.ROWID = Guid.NewGuid();
                                    DB.PCCS.InsertOnSubmit(pccs);
                                    DB.SubmitChanges();
                                    typ = "INS";
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.PCCS.DeleteOnSubmit(pccs);
                            }

                        }
                        _status = true;

                        //Save PCCS Revison Details
                        // GetPccsRevisonDetails(pccsModel, pccsModel.PartNo, pccsModel.RouteNo);
                        dtPccsRevisonDetails = pccsModel.PccsRevisionDetails.Table;
                        //PART_NO,ROUTE_NO,ISSUE_NO,ISSUE_DATE,ISSUE_ALTER,COMPILED_BY
                        decimal routeNo = pccsModel.RouteNo.ToValueAsString().ToDecimalValue();

                        List<PCCS_ISSUE> lstexistingDatasPccsIssue = new List<PCCS_ISSUE>();
                        dtPccsDetails = pccsModel.PCCSDetails.Table;
                        lstexistingDatasPccsIssue = ((from c in DB.PCCS_ISSUE
                                                      where c.PART_NO == pccsModel.PartNo
                                                      && c.ROUTE_NO == pccsModel.RouteNo
                                                      select c).ToList());
                        if (lstexistingDatasPccsIssue.Count > 0)
                        {
                            DB.PCCS_ISSUE.DeleteAllOnSubmit(lstexistingDatasPccsIssue);
                            DB.SubmitChanges();
                        }

                        for (int i = 0; i < dtPccsRevisonDetails.Rows.Count; i++)
                        {
                            if (dtPccsRevisonDetails.Rows[i]["ROUTE_NO"].IsNotNullOrEmpty())
                                routeNo = dtPccsRevisonDetails.Rows[i]["ROUTE_NO"].ToString().ToDecimalValue();
                            else
                                routeNo = pccsModel.RouteNo.ToValueAsString().ToDecimalValue();

                            //PCCS_ISSUE pccsRevison = (from c in DB.PCCS_ISSUE
                            //                          where c.PART_NO == pccsModel.PartNo && c.ROUTE_NO == routeNo && c.ISSUE_NO == (i + 1).ToString()
                            //                          select c).FirstOrDefault<PCCS_ISSUE>();
                            //if (!pccsRevison.IsNotNullOrEmpty())
                            //{
                            PCCS_ISSUE pccsRevison = null;
                            if (routeNo == pccsModel.RouteNo)
                                try
                                {
                                    if (dtPccsRevisonDetails.Rows[i]["ISSUE_DATE"].ToString().IsNotNullOrEmpty() || dtPccsRevisonDetails.Rows[i]["ISSUE_ALTER"].ToString().IsNotNullOrEmpty() || dtPccsRevisonDetails.Rows[i]["COMPILED_BY"].ToString().IsNotNullOrEmpty())
                                    {
                                        pccsRevison = new PCCS_ISSUE()
                                        {
                                            PART_NO = pccsModel.PartNo.ToString(),
                                            ROUTE_NO = pccsModel.RouteNo.ToValueAsString().ToDecimalValue(),
                                            ISSUE_NO = dtPccsRevisonDetails.Rows[i]["ISSUE_NO"].ToString(),
                                            // ISSUE_DATE = dtPccsRevisonDetails.Rows[i]["ISSUE_DATE"].ToString().ToDateTimeValue(),
                                            ISSUE_ALTER = dtPccsRevisonDetails.Rows[i]["ISSUE_ALTER"].ToString(),
                                            COMPILED_BY = dtPccsRevisonDetails.Rows[i]["COMPILED_BY"].ToString(),
                                            DELETE_FLAG = false,
                                            ENTERED_DATE = DateTime.Now,
                                            ENTERED_BY = userInformation.UserName,
                                            ROWID = Guid.NewGuid()
                                        };
                                        if (dtPccsRevisonDetails.Rows[i]["ISSUE_DATE"].ToString() != "")
                                            pccsRevison.ISSUE_DATE = Convert.ToDateTime(dtPccsRevisonDetails.Rows[i]["ISSUE_DATE"]);
                                        else
                                            pccsRevison.ISSUE_DATE = null;

                                        DB.PCCS_ISSUE.InsertOnSubmit(pccsRevison);
                                        DB.SubmitChanges();
                                        typ = "INS";
                                    }

                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                    DB.PCCS_ISSUE.DeleteOnSubmit(pccsRevison);
                                }
                          
                        }
                        _status = true;
                    }

                }

                return _status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;

            }
        }

        // Need to Implement in Grid
        public bool GetPccsComboValues(PCCSModel pccsModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PCCS_FEATURES
                                  select new { o.FEATURE }).ToList());
                pccsModel.FeatureCmb = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.PCCS_PROCESS
                                  select new { PROCESS_FEATURE = o.PCCS_PROCESS1 }).ToList());
                pccsModel.Process = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.SPECIAL_CHARACTER
                                  select new { o.SPEC_CHAR }).ToList());
                pccsModel.SplChar = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.PCCS_CONTROL_METHOD
                                  select new { o.CONTROL_METHOD }).ToList());
                pccsModel.Control = (dt != null) ? dt.DefaultView : null;

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool DeletePccsDetails(string partNo, decimal? routeNo)
        {
            try
            {
                List<PCCS> lstexistingDatas = new List<PCCS>();
                if (partNo.IsNotNullOrEmpty() && routeNo.IsNotNullOrEmpty())
                {
                    lstexistingDatas = ((from o in DB.PCCS
                                         where o.PART_NO == partNo && o.ROUTE_NO == routeNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                         select o).ToList());
                    if (lstexistingDatas.Count > 0)
                    {
                        DB.PCCS.DeleteAllOnSubmit(lstexistingDatas);
                        DB.SubmitChanges();
                    }
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    PCCS pccs = (from c in DB.PCCS
                    //                 where c.PART_NO == partNo && c.ROUTE_NO == routeNo && c.SEQ_NO == Convert.ToDecimal(dt.Rows[i]["SEQ_NO"].ToString()) && c.SNO == Convert.ToDouble(dt.Rows[i]["SNO"].ToString()) && c.ISR_NO == Convert.ToDecimal(dt.Rows[i]["ISR_NO"].ToString())
                    //                 select c).FirstOrDefault<PCCS>();

                    //    if (pccs != null)
                    //    {

                    //        if (pccs.DELETE_FLAG == true)
                    //        {
                    //            pccs.DELETE_FLAG = false;
                    //        }
                    //        else
                    //        {
                    //            pccs.DELETE_FLAG = true;
                    //        }

                    //        pccs.UPDATED_DATE = DateTime.Now;
                    //        pccs.UPDATED_BY = userInformation.UserName;
                    //        DB.SubmitChanges();

                    //    }
                    //    else if (pccs == null)
                    //    {
                    //        return false;
                    //    }
                    //}
                    return true;
                }
                return false;

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

        public bool DeletePccs(string partNo, decimal routeNo, decimal seqNo, double sno)
        {
            try
            {
                if (partNo.IsNotNullOrEmpty() && routeNo.IsNotNullOrEmpty())
                {
                    PCCS pccs = (from c in DB.PCCS
                                 where c.PART_NO == partNo && c.ROUTE_NO == routeNo && c.SEQ_NO == seqNo && c.SNO == sno
                                 select c).FirstOrDefault<PCCS>();

                    if (pccs != null)
                    {

                        if (pccs.DELETE_FLAG == true)
                        {
                            pccs.DELETE_FLAG = false;
                        }
                        else
                        {
                            pccs.DELETE_FLAG = true;
                        }

                        pccs.UPDATED_DATE = DateTime.Now;
                        pccs.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                        return true;
                    }
                    else if (pccs == null)
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
            return true;
        }

        //public string GetCustname(string partno)
        //{
        //    try
        //    {
        //        var innerQuery = (from o in DB.PRD_CIREF
        //                          where o.PART_NO == partno
        //                          select o.CI_REF).Distinct();

        //        var cust = (from i in DB.DDCI_INFO
        //                    join c in DB.DDCUST_MAST on i.CUST_CODE equals c.CUST_CODE
        //                    where innerQuery.Contains(i.CI_REFERENCE)
        //                    select new
        //                    {
        //                        c.CUST_CODE,
        //                        c.CUST_NAME
        //                    }).ToList();

        //        string custName = "";

        //        if (cust != null && cust.Count > 0) custName = cust[0].CUST_NAME;

        //        return custName;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        public DataTable GetResults(string partno, string routeno, int initial, int final, string doc_name, string isBlank = "0")
        {
            //string report = "select p.PART_NO, CAST(p.ROUTE_NO as varchar) as ROUTE_NO, CAST(p.ISR_NO as varchar) as ISR_NO,p.FEATURE,p.SPEC_MIN,p.SPEC_MAX,i.CUST_DWG_NO,i.CUST_DWG_NO_ISSUE,CAST(i.CUST_STD_DATE as varchar) as CUST_STD_DATE,e.EX_NO,e.REVISION_NO, '" + isBlank + "' as ISBLANK "
            //    + " from EXHIBIT_DOC e , (select CI_REF,PART_NO from PRD_CIREF where PART_NO='" + partno + "' AND CURRENT_CIREF = 1) d"
            //     + " JOIN DDCI_INFO i on d.CI_REF = i.CI_REFERENCE "
            //       + " right JOIN PCCS p on p.part_no = d.part_no "
            //       + " JOIN PROCESS_MAIN pm on pm.part_no = p.part_no "
            //        + " where p.PART_NO = '" + partno + "' and pm.ROUTE_NO = p.ROUTE_NO and ISNULL(p.DELETE_FLAG,0) = 0 and p.ISR_NO between " + initial + " and " + final + " "
            //          + " and e.DOC_NAME='" + doc_name + "'";

            string report = "select * from ( SELECT ROW_NUMBER() OVER(Partition by p.ISR_NO,p.FEATURE,p.ROUTE_NO,p.PART_NO ORDER BY p.FEATURE) AS Row_Number, p.PART_NO, CAST(p.ROUTE_NO as varchar) as ROUTE_NO, CAST(p.ISR_NO as varchar) as ISR_NO,p.FEATURE,p.SPEC_MIN,p.SPEC_MAX,i.CUST_DWG_NO,i.CUST_DWG_NO_ISSUE,CAST(i.CUST_STD_DATE as varchar) as CUST_STD_DATE,e.EX_NO,e.REVISION_NO, '" + isBlank + "' as ISBLANK "
                + " from EXHIBIT_DOC e , (select CI_REF,PART_NO from PRD_CIREF where PART_NO='" + partno + "' AND CURRENT_CIREF = 1) d"
                 + " JOIN DDCI_INFO i on d.CI_REF = i.CI_REFERENCE "
                   + " right JOIN PCCS p on p.part_no = d.part_no "
                   + " JOIN PROCESS_MAIN pm on pm.part_no = p.part_no "
                    + " where p.PART_NO = '" + partno + "' and pm.ROUTE_NO = p.ROUTE_NO and ISNULL(p.DELETE_FLAG,0) = 0 and p.ISR_NO between " + initial + " and " + final + " "
                      + " and e.DOC_NAME='" + doc_name + "'";
            // Commented by Jeyan - System should show reports only current process was set in process sheet.
            //if (routeno.IsNotNullOrEmpty())
            //{
            //    report = report + " and p.route_no='" + routeno + "'";
            //}
            //else
            //{
            //    //report = report.Replace("CAST(p.ROUTE_NO as varchar)", "''");
            //    report = report + " and pm.CURRENT_PROC = 1";
            //}
            report = report + " and pm.CURRENT_PROC = 1";
            report = report + " ) rn where rn.Row_Number=1 ORDER BY CAST(rn.ISR_NO AS int) ASC";

            var getreults = ToDataTable(DB.ExecuteQuery<RptDimensionalModel>(report).ToList());
            return getreults;
        }

        public DataTable GetCheckList(string partno, string doc_name)
        {
            string report = "select p.PART_NO, e.EX_NO, e.REVISION_NO"
                + " from pccs p, exhibit_doc e, ddci_info i"
                  + " where ISNULL(p.DELETE_FLAG,0) = 0 and doc_name='" + doc_name + "'"
                      + " and ci_reference=(select top 1 ci_ref from prd_ciref where part_no='" + partno + "' AND CURRENT_CIREF = 1)"
                        + " and p.part_no ='" + partno + "'"
                          + " ORDER BY CAST(p.ISR_NO as int) ASC";
            var getcheck = ToDataTable(DB.ExecuteQuery<RptDimensionalModel>(report).ToList());
            return getcheck;
        }

        public DataTable GetInitialInspection(string partno, string partDesc, string routeno, int initial)
        {

            DataTable dt = new DataTable();
            try
            {
                dt = ToDataTable((from d in DB.PCCS
                                  join p in DB.PROCESS_MAIN on d.PART_NO equals p.PART_NO
                                  where d.PART_NO == partno && d.ISR_NO != initial && d.ROUTE_NO == p.ROUTE_NO && (d.DELETE_FLAG == false || d.DELETE_FLAG == null) && p.CURRENT_PROC == 1
                                  orderby d.ISR_NO ascending
                                  select new
                                  {
                                      CUST_DWG_NO = "",
                                      PROD_DESC = "",
                                      p.PART_NO,
                                      d.ISR_NO,
                                      d.FEATURE,
                                      d.SPEC_MAX,
                                      d.SPEC_MIN,
                                      partDesc
                                  }).ToList());
                if (dt == null && dt.Rows.Count == 0)
                {
                    return null;
                }

                var ciref = (from m in DB.PRD_CIREF
                             join c in DB.DDCI_INFO on m.CI_REF equals c.CI_REFERENCE
                             where m.PART_NO == partno && m.CURRENT_CIREF == true
                             select new
                             {
                                 c.CUST_DWG_NO,
                                 c.PROD_DESC
                             }).FirstOrDefault();

                if (ciref != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["CUST_DWG_NO"] = ciref.CUST_DWG_NO;
                        dr["PROD_DESC"] = ciref.PROD_DESC;
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                e.LogException();
                return dt;
            }
        }

        public bool SaveDiagram(byte[] diagram, string partNo, decimal? routeNo)
        {
            PROC_FLOW_DGM procflowdgm = new PROC_FLOW_DGM();
            bool insert = false;
            bool update = false;
            bool submit = false;
            try
            {
                procflowdgm = (from c in DB.PROC_FLOW_DGM
                               where c.PART_NO == partNo &&
                               c.ROUTE_NO == routeNo
                               select c).SingleOrDefault<PROC_FLOW_DGM>();

                if (procflowdgm.IsNotNullOrEmpty())
                {
                    procflowdgm.UPDATED_BY = userInformation.UserName;
                    procflowdgm.UPDATED_DATE = serverDateTime;
                    procflowdgm.DELETE_FLAG = false;
                    procflowdgm.DIAGRAM = diagram;
                    update = true;
                }
                else
                {
                    procflowdgm = new PROC_FLOW_DGM();
                    procflowdgm.ENTERED_BY = userInformation.UserName;
                    procflowdgm.ENTERED_DATE = serverDate;
                    procflowdgm.DELETE_FLAG = false;
                    procflowdgm.DIAGRAM = diagram;
                    procflowdgm.PART_NO = partNo.Trim();
                    procflowdgm.ROUTE_NO = routeNo.ToValueAsString().ToDecimalValue();
                    insert = true;
                    DB.PROC_FLOW_DGM.InsertOnSubmit(procflowdgm);
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
                        DB.PROC_FLOW_DGM.DeleteOnSubmit(procflowdgm);
                    }
                    if (update == true)
                    {
                        DB.PROC_FLOW_DGM.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, procflowdgm);
                    }
                }
                ex.LogException();
                return false;
            }
        }
    }
}
