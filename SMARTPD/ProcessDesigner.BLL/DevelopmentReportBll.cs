using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{

    public class DevelopmentReportBll : Essential
    {

        public DevelopmentReportBll(UserInformation userInformation)
        {
            try
            {
                this.userInformation = userInformation;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }
        public bool GetTabGridDetails(DevelopmentReportModel devModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (devModel.PartNo.IsNotNullOrEmpty())
                {
                    GetTabMainFormDetails(devModel);
                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_DESIGN
                                              where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.DEV_RUN_NO, o.SNO, o.CFT_DISS, o.JUSTIFICATION, o.TGR, o.TWG }).ToList());
                    if (dt != null) devModel.DesignAssumptionDetails = dt.DefaultView;

                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_SUB
                                              where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.DEV_RUN_NO, o.STAGE_NO, o.PROBLEM_FACED, o.COUNTER_MEASURE, o.REWORK_TOOL_DESIGN, o.REWORK_TOOL_MFG, o.REMARKS }).ToList());
                    if (dt != null) devModel.LogDetails = dt.DefaultView;

                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_SHORT_CLOSURE
                                              where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.RUN_NO, o.SNO, o.REASON, o.WHY, o.SHORT_ACTION, o.SHORT_ACTION_DATE, o.LONG_ACTION, o.LONG_ACTION_DATE, o.TRIAL_DATE }).ToList());
                    if (dt != null) devModel.ShortClosureDetails = dt.DefaultView;

                }
                else
                {
                    GetTabMainFormDetails(devModel);
                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_DESIGN
                                              where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.DEV_RUN_NO, o.SNO, o.CFT_DISS, o.JUSTIFICATION, o.TGR, o.TWG }).ToList());

                    devModel.DesignAssumptionDetails = dt.DefaultView;

                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_SUB
                                              where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.DEV_RUN_NO, o.STAGE_NO, o.PROBLEM_FACED, o.COUNTER_MEASURE, o.REWORK_TOOL_DESIGN, o.REWORK_TOOL_MFG, o.REMARKS }).ToList());
                    devModel.LogDetails = dt.DefaultView;

                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_SHORT_CLOSURE
                                              where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.RUN_NO, o.SNO, o.REASON, o.WHY, o.SHORT_ACTION, o.SHORT_ACTION_DATE, o.LONG_ACTION, o.LONG_ACTION_DATE, o.TRIAL_DATE }).ToList());
                    devModel.ShortClosureDetails = dt.DefaultView;

                }


                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetTabMainFormDetails(DevelopmentReportModel devModel)
        {
            try
            {
                DataTable dt = new DataTable();
                DateTime? datReport;
                if (devModel.PartNo.IsNotNullOrEmpty())
                {
                    if (devModel.PartNo.IsNotNullOrEmpty() && devModel.RunNo.IsNotNullOrEmpty())
                    {
                        dt = ToDataTableWithType((from o in DB.DEV_REPORT_MAIN
                                                  where o.PART_NO == devModel.PartNo && o.DEV_RUN_NO == devModel.RunNo.ToDecimalValue() && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                  select new { o.PART_NO, o.DEV_RUN_NO, o.CUST_COMP, o.CUST_COMP_DESC, o.FORGE_DDREP, o.FORGE_ZAPREP, o.RECORD, o.REPORT_DATE, o.NO_FORG_SHT }).ToList());
                    }
                    else
                    {
                        dt = ToDataTableWithType((from o in DB.DEV_REPORT_MAIN
                                                  where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                  select new { o.PART_NO, o.DEV_RUN_NO, o.CUST_COMP, o.CUST_COMP_DESC, o.FORGE_DDREP, o.FORGE_ZAPREP, o.RECORD, o.REPORT_DATE, o.NO_FORG_SHT }).ToList());
                    }


                    if (dt != null)
                    {
                        devModel.DevMainDetails = dt.DefaultView;
                        if (dt.Rows.Count > 0)
                        {
                            if (devModel.RunNoDetails.Count > 0)
                                devModel.RunNo = dt.DefaultView[0]["DEV_RUN_NO"].ToString();

                            devModel.IsDoYouHaveCustComplaint = dt.DefaultView[0]["CUST_COMP"].ToString().ToBooleanAsString();
                            if (!dt.DefaultView[0]["CUST_COMP"].ToString().IsNotNullOrEmpty())
                            {
                                devModel.LabelNatureOfComplaint = "Yes / No";
                            }
                            else if (dt.DefaultView[0]["CUST_COMP"].ToString().ToUpper() == "NO")
                            {
                                devModel.LabelNatureOfComplaint = "No";
                                devModel.IsDoYouHaveCustComplaint = false;
                            }
                            else if (dt.DefaultView[0]["CUST_COMP"].ToString().ToUpper() == "YES")
                            {
                                devModel.LabelNatureOfComplaint = "Yes";
                                devModel.IsDoYouHaveCustComplaint = true;
                            }

                            devModel.NatureOfComplaint = dt.DefaultView[0]["CUST_COMP_DESC"].ToString();
                            devModel.DADRep = dt.DefaultView[0]["FORGE_DDREP"].ToString();
                            devModel.ZapRep = dt.DefaultView[0]["FORGE_ZAPREP"].ToString();
                            devModel.RecordOfCFTDiscussion = dt.DefaultView[0]["RECORD"].ToString();
                            //datReport = dt.DefaultView[0]["REPORT_DATE"].ToString().ToDateTimeValue();
                            if (dt.DefaultView[0]["REPORT_DATE"].ToString().IsNotNullOrEmpty())
                                devModel.RunDate = Convert.ToDateTime(dt.DefaultView[0]["REPORT_DATE"]);
                            if (devModel.RunDate.ToValueAsString().Trim() == "01/01/0001")
                            {
                                devModel.RunDate = null;
                            }
                            devModel.NoOfForginShift = dt.DefaultView[0]["NO_FORG_SHT"].ToString();
                            if (dt.DefaultView[0]["NO_FORG_SHT"].ToString() == "0")
                            {
                                devModel.NoOfForginShift = "";
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetPartNoDetails(DevelopmentReportModel devModel)
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
                    devModel.PartNoDetails = dt.DefaultView;
                    //  pccsModel.PartNoDetails.AddNew();
                }
                else
                {
                    devModel.PartNoDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public bool GetRunNoDetails(DevelopmentReportModel devModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.DEV_REPORT_MAIN
                                  where o.PART_NO == devModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  orderby o.DEV_RUN_NO ascending
                                  select new { o.DEV_RUN_NO }).ToList());
                if (dt != null)
                {
                    devModel.RunNoDetails = dt.DefaultView;
                    if (devModel.RunNoDetails.Count == 0)
                        devModel.RunNo = "1";
                    else if (devModel.RunNoDetails.Count > 0)
                        devModel.RunNo = dt.DefaultView[0]["DEV_RUN_NO"].ToString();
                }
                else
                {
                    devModel.RunNoDetails = null;
                    devModel.RunNo = "1";
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataSet RetrieveDevReport(string partNumber, Int16? runningNumber = null)
        {

            try
            {
                if (String.IsNullOrEmpty(partNumber)) throw new Exception("PartNo should not be empty");

                partNumber = partNumber.Trim();

                sbSQL.AppendLine("SELECT part_no, ");
                sbSQL.AppendLine("  dev_run_no, ");
                sbSQL.AppendLine("  cust_comp, ");
                sbSQL.AppendLine("  cust_comp_desc, ");
                sbSQL.AppendLine("  forge_ddrep, ");
                sbSQL.AppendLine("  forge_zaprep, ");
                sbSQL.AppendLine("  record, ");
                sbSQL.AppendLine("  report_date, ");
                sbSQL.AppendLine("  no_forg_sht ");
                sbSQL.AppendLine("FROM dev_report_main ");
                sbSQL.AppendLine("WHERE part_no  = :pi_partNumber ");

                lstDbParameter.Clear();
                lstDbParameter.Add(DBParameter(":pi_partNumber", partNumber));

                if (runningNumber != null)
                {
                    sbSQL.AppendLine("AND dev_run_no = :pi_RunningNumber");
                    lstDbParameter.Add(DBParameter(":pi_RunningNumber", runningNumber));
                }

                sbSQL.AppendLine("ORDER BY dev_run_no");

                sqlDictionary.Add("dev_report_main", sbSQL);
                dsResult = Dal.GetDataSet(sqlDictionary, lstDbParameter);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {

            }

            return dsResult;
        }

        public string GenerateRunningNo(string partNumber)
        {
            decimal? runNoMast = 1;
            try
            {

                try
                {
                    runNoMast = (from o in DB.DEV_REPORT_MAIN where o.PART_NO == partNumber.Trim() select o.DEV_RUN_NO).Max();
                    if (!runNoMast.IsNotNullOrEmpty() || runNoMast == 0)
                        runNoMast = 1;
                    else
                        runNoMast = (runNoMast + 1);
                    return runNoMast.ToValueAsString();
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    runNoMast = 1;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {

            }
            return runNoMast.ToValueAsString();
        }

        public DataSet GetAssumptionsLOGShortClosure(string partNumber, Int16 runningNumber = 1)
        {
            StringBuilder sbLOGSQL = null;
            StringBuilder sbClosureSQL = null;

            try
            {
                if (String.IsNullOrEmpty(partNumber)) throw new Exception("PartNo should not be empty");
                partNumber = partNumber.Trim();

                if (runningNumber == 0)
                {
                    runningNumber = 1;
                }
                sqlDictionary = new Dictionary<string, StringBuilder>();

                sbSQL = new StringBuilder();
                sbSQL.AppendLine("SELECT part_no, ");
                sbSQL.AppendLine("  dev_run_no, ");
                sbSQL.AppendLine("  sno, ");
                sbSQL.AppendLine("  cft_diss, ");
                sbSQL.AppendLine("  justification, ");
                sbSQL.AppendLine("  tgr, ");
                sbSQL.AppendLine("  NULL AS tgw "); ////This Column got error in existing DB
                sbSQL.AppendLine("FROM dev_report_design ");
                sbSQL.AppendLine("WHERE part_no =:pi_partNumber ");
                sbSQL.AppendLine("AND dev_run_no=:pi_RunningNumber");

                sqlDictionary.Add("dev_report_design", sbSQL);

                sbLOGSQL = new StringBuilder();
                sbLOGSQL.AppendLine("SELECT part_no, ");
                sbLOGSQL.AppendLine("  dev_run_no, ");
                sbLOGSQL.AppendLine("  stage_no, ");
                sbLOGSQL.AppendLine("  problem_faced, ");
                sbLOGSQL.AppendLine("  counter_measure, ");
                sbLOGSQL.AppendLine("  rework_tool_design, ");
                sbLOGSQL.AppendLine("  rework_tool_mfg, ");
                sbLOGSQL.AppendLine("  remarks ");
                sbLOGSQL.AppendLine("FROM dev_report_sub ");
                sbLOGSQL.AppendLine("WHERE part_no =:pi_partNumber ");
                sbLOGSQL.AppendLine("AND dev_run_no=:pi_RunningNumber");

                sqlDictionary.Add("dev_report_sub", sbLOGSQL);

                sbClosureSQL = new StringBuilder();
                sbClosureSQL.AppendLine("SELECT part_no, ");
                sbClosureSQL.AppendLine("  run_no, ");
                sbClosureSQL.AppendLine("  sno, ");
                sbClosureSQL.AppendLine("  reason, ");
                sbClosureSQL.AppendLine("  why, ");
                sbClosureSQL.AppendLine("  short_action, ");
                sbClosureSQL.AppendLine("  short_action_date, ");
                sbClosureSQL.AppendLine("  long_action, ");
                sbClosureSQL.AppendLine("  long_action_date, ");
                sbClosureSQL.AppendLine("  trial_date ");
                sbClosureSQL.AppendLine("FROM dev_report_short_closure ");
                sbClosureSQL.AppendLine("WHERE part_no=:pi_partNumber ");
                sbClosureSQL.AppendLine("AND run_no=:pi_RunningNumber");

                sqlDictionary.Add("dev_report_short_closure", sbLOGSQL);

                lstDbParameter.Add(DBParameter(":pi_partNumber", partNumber));
                lstDbParameter.Add(DBParameter(":pi_RunningNumber", runningNumber));

                dsResult = Dal.GetDataSet(sqlDictionary, lstDbParameter);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sbLOGSQL.IsNotNullOrEmpty()) sbLOGSQL.Clear();
                sbLOGSQL = null;

                if (sbClosureSQL.IsNotNullOrEmpty()) sbClosureSQL.Clear();
                sbClosureSQL = null;
            }

            return dsResult;
        }

        public DataTable GetRunningNumber(string partNumber)
        {
            StringBuilder sbLOGSQL = null;
            StringBuilder sbClosureSQL = null;

            try
            {
                if (String.IsNullOrEmpty(partNumber)) throw new Exception("PartNo should not be empty");
                partNumber = partNumber.Trim();

                tableName = "DEV_REPORT_MAIN";
                whereClause = "PART_NO=:pi_partnumber";
                lstDbParameter.Clear();
                lstDbParameter.Add(DBParameter(":pi_partnumber", partNumber));
                columnNames = new List<string>() { "DEV_RUN_NO" };

                dtResult = GetTableData(tableName, whereClause, lstDbParameter, columnNames);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sbLOGSQL.IsNotNullOrEmpty()) sbLOGSQL.Clear();
                sbLOGSQL = null;

                if (sbClosureSQL.IsNotNullOrEmpty()) sbClosureSQL.Clear();
                sbClosureSQL = null;
            }

            return dtResult;
        }

        public bool DeleteDesignAssumptDetails(string partNo, decimal runNo, decimal sno = 0)
        {
            if (!partNo.IsNotNullOrEmpty() || !runNo.IsNotNullOrEmpty() || !sno.IsNotNullOrEmpty()) return false;
            DEV_REPORT_DESIGN designAssumpt = new DEV_REPORT_DESIGN();
            try
            {
                designAssumpt = (from c in DB.DEV_REPORT_DESIGN
                                 where c.PART_NO == partNo && c.DEV_RUN_NO == runNo && c.SNO == sno
                                 select c).FirstOrDefault<DEV_REPORT_DESIGN>();

                if (designAssumpt != null)
                {

                    designAssumpt.DELETE_FLAG = true;
                    designAssumpt.UPDATED_DATE = DateTime.Now;
                    designAssumpt.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                DB.DEV_REPORT_DESIGN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, designAssumpt);
                ex.LogException();
                return false;
            }
            //return false;
        }
        public bool DeleteLogDetails(string partNo, decimal runNo, decimal sno = 0)
        {
            if (!partNo.IsNotNullOrEmpty() || !runNo.IsNotNullOrEmpty() || !sno.IsNotNullOrEmpty()) return false;
            DEV_REPORT_SUB log = new DEV_REPORT_SUB();
            try
            {
                log = (from c in DB.DEV_REPORT_SUB
                       where c.PART_NO == partNo && c.DEV_RUN_NO == runNo && c.STAGE_NO == sno
                       select c).FirstOrDefault<DEV_REPORT_SUB>();

                if (log != null)
                {

                    log.DELETE_FLAG = true;
                    log.UPDATED_DATE = DateTime.Now;
                    log.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                DB.DEV_REPORT_SUB.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, log);
                ex.LogException();
                return false;
            }
            //return false;
        }
        public bool DeleteShortClosureDetails(string partNo, decimal runNo, decimal sno = 0)
        {
            if (!partNo.IsNotNullOrEmpty() || !runNo.IsNotNullOrEmpty() || !sno.IsNotNullOrEmpty()) return false;
            DEV_REPORT_SHORT_CLOSURE shortClosure = new DEV_REPORT_SHORT_CLOSURE();
            try
            {
                shortClosure = (from c in DB.DEV_REPORT_SHORT_CLOSURE
                                where c.PART_NO == partNo && c.RUN_NO == runNo && c.SNO == sno
                                select c).FirstOrDefault<DEV_REPORT_SHORT_CLOSURE>();

                if (shortClosure != null)
                {

                    shortClosure.DELETE_FLAG = true;
                    shortClosure.UPDATED_DATE = DateTime.Now;
                    shortClosure.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                DB.DEV_REPORT_SHORT_CLOSURE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, shortClosure);
                ex.LogException();
                return false;
            }
            //return false;
        }
        public bool SaveDevelopmentRpt(DevelopmentReportModel devModel, ref string typ)
        {
            bool _status = false;

            try
            {
                DataTable dtDevRptMainDetails = new DataTable();
                DataTable dtDesignAssumptionDetails = new DataTable();
                DataTable dtLogDetails = new DataTable();
                DataTable dtShortClouserDetails = new DataTable();

                if (devModel.PartNo.IsNotNullOrEmpty() && devModel.RunNo.IsNotNullOrEmpty() && devModel.RunDate.IsNotNullOrEmpty())
                {
                    dtDesignAssumptionDetails = devModel.DesignAssumptionDetails.ToTable().Copy();
                    dtLogDetails = devModel.LogDetails.ToTable().Copy();
                    dtShortClouserDetails = devModel.ShortClosureDetails.ToTable().Copy();
                    //dtDevRptMainDetails = devModel.DevMainDetails.Table;
                    dtDesignAssumptionDetails.AcceptChanges();
                    dtLogDetails.AcceptChanges();
                    dtShortClouserDetails.AcceptChanges();
                    #region DevelopmentRptMain

                    DEV_REPORT_MAIN devRptMain = (from c in DB.DEV_REPORT_MAIN
                                                  where c.PART_NO == devModel.PartNo && c.DEV_RUN_NO == devModel.RunNo.ToDecimalValue()
                                                  select c).FirstOrDefault<DEV_REPORT_MAIN>();
                    if (devModel.IsDoYouHaveCustComplaint == false)
                    {
                        devModel.LabelNatureOfComplaint = "No";
                    }
                    else if (devModel.IsDoYouHaveCustComplaint == true)
                    {
                        devModel.LabelNatureOfComplaint = "Yes";
                    }
                    if (!devRptMain.IsNotNullOrEmpty())
                    {
                        try
                        {
                            devRptMain = new DEV_REPORT_MAIN()
                            {
                                PART_NO = devModel.PartNo.ToString(),
                                DEV_RUN_NO = devModel.RunNo.ToDecimalValue(),
                                CUST_COMP = devModel.LabelNatureOfComplaint,
                                CUST_COMP_DESC = devModel.NatureOfComplaint,
                                FORGE_DDREP = devModel.DADRep,
                                FORGE_ZAPREP = devModel.ZapRep,
                                RECORD = devModel.RecordOfCFTDiscussion,
                                // REPORT_DATE = devModel.RunDate,
                                NO_FORG_SHT = devModel.NoOfForginShift.ToDecimalValue(),
                                DELETE_FLAG = false,
                                ENTERED_DATE = DateTime.Now,
                                ENTERED_BY = userInformation.UserName,
                                ROWID = Guid.NewGuid()
                            };
                            if (devModel.RunDate.ToString() != "")
                                devRptMain.REPORT_DATE = Convert.ToDateTime(devModel.RunDate);
                            else
                                devRptMain.REPORT_DATE = null;
                            DB.DEV_REPORT_MAIN.InsertOnSubmit(devRptMain);
                            DB.SubmitChanges();
                            typ = "INS";
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DEV_REPORT_MAIN.DeleteOnSubmit(devRptMain);
                        }
                    }
                    else
                    {
                        if (devRptMain.IsNotNullOrEmpty())
                        {
                            try
                            {
                                devRptMain.CUST_COMP = devModel.LabelNatureOfComplaint.ToString();
                                devRptMain.CUST_COMP_DESC = devModel.NatureOfComplaint;
                                devRptMain.FORGE_DDREP = devModel.DADRep;
                                devRptMain.FORGE_ZAPREP = devModel.ZapRep;
                                devRptMain.RECORD = devModel.RecordOfCFTDiscussion;
                                //devRptMain.REPORT_DATE = devModel.RunDate;
                                if (devModel.RunDate.ToString() != "")
                                    devRptMain.REPORT_DATE = Convert.ToDateTime(devModel.RunDate);
                                else
                                    devRptMain.REPORT_DATE = null;
                                devRptMain.NO_FORG_SHT = devModel.NoOfForginShift.ToDecimalValue();
                                devRptMain.DELETE_FLAG = false;
                                devRptMain.UPDATED_DATE = DateTime.Now;
                                devRptMain.UPDATED_BY = userInformation.UserName;
                                DB.SubmitChanges();
                                typ = "UPD";
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.DEV_REPORT_DESIGN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, devRptMain);
                            }

                        }
                    }
                    _status = true;
                    #endregion

                    #region DesignAssumptionDetails
                    List<DEV_REPORT_DESIGN> lstexistingDatas = new List<DEV_REPORT_DESIGN>();
                    lstexistingDatas = ((from c in DB.DEV_REPORT_DESIGN
                                         where c.PART_NO == devModel.PartNo && c.DEV_RUN_NO == devModel.RunNo.ToDecimalValue()
                                         select c).ToList());
                    if (lstexistingDatas.Count > 0)
                    {
                        DB.DEV_REPORT_DESIGN.DeleteAllOnSubmit(lstexistingDatas);
                        DB.SubmitChanges();
                    }
                    int rown = 1;
                    for (int i = 0; i < dtDesignAssumptionDetails.Rows.Count; i++)
                    {
                        DEV_REPORT_DESIGN designAssumpt = (from c in DB.DEV_REPORT_DESIGN
                                                           where c.PART_NO == devModel.PartNo && c.DEV_RUN_NO == devModel.RunNo.ToDecimalValue() && c.SNO == rown
                                                           select c).FirstOrDefault<DEV_REPORT_DESIGN>();
                        try
                        {
                            if (designAssumpt.IsNotNullOrEmpty())
                            {
                                DB.DEV_REPORT_DESIGN.DeleteOnSubmit(designAssumpt);
                                DB.SubmitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                        }
                        try
                        {
                            if (dtDesignAssumptionDetails.Rows[i]["CFT_DISS"].ToString().IsNotNullOrEmpty() || dtDesignAssumptionDetails.Rows[i]["JUSTIFICATION"].ToString().IsNotNullOrEmpty() || dtDesignAssumptionDetails.Rows[i]["TGR"].ToString().IsNotNullOrEmpty() || dtDesignAssumptionDetails.Rows[i]["TWG"].ToString().IsNotNullOrEmpty())
                            {

                                designAssumpt = new DEV_REPORT_DESIGN()
                                   {
                                       PART_NO = devModel.PartNo.ToString(),
                                       DEV_RUN_NO = devModel.RunNo.ToDecimalValue(),
                                       SNO = rown,
                                       CFT_DISS = dtDesignAssumptionDetails.Rows[i]["CFT_DISS"].ToString(),
                                       JUSTIFICATION = dtDesignAssumptionDetails.Rows[i]["JUSTIFICATION"].ToString(),
                                       TGR = dtDesignAssumptionDetails.Rows[i]["TGR"].ToString(),
                                       TWG = dtDesignAssumptionDetails.Rows[i]["TWG"].ToString(),
                                       DELETE_FLAG = false,
                                       ENTERED_DATE = DateTime.Now,
                                       ENTERED_BY = userInformation.UserName,
                                       ROWID = Guid.NewGuid()
                                   };
                                DB.DEV_REPORT_DESIGN.InsertOnSubmit(designAssumpt);
                                DB.SubmitChanges();
                                rown = rown + 1;
                                // typ = "INS";
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DEV_REPORT_DESIGN.DeleteOnSubmit(designAssumpt);
                            rown = rown + 1;
                        }

                        //else
                        //{
                        //    if (designAssumpt.IsNotNullOrEmpty())
                        //    {
                        //        try
                        //        {
                        //            if (dtDesignAssumptionDetails.Rows[i]["CFT_DISS"].ToString().IsNotNullOrEmpty() || dtDesignAssumptionDetails.Rows[i]["JUSTIFICATION"].ToString().IsNotNullOrEmpty() || dtDesignAssumptionDetails.Rows[i]["TGR"].ToString().IsNotNullOrEmpty() || dtDesignAssumptionDetails.Rows[i]["TWG"].ToString().IsNotNullOrEmpty())
                        //            {
                        //                designAssumpt.CFT_DISS = dtDesignAssumptionDetails.Rows[i]["CFT_DISS"].ToString();
                        //                designAssumpt.JUSTIFICATION = dtDesignAssumptionDetails.Rows[i]["JUSTIFICATION"].ToString();
                        //                designAssumpt.TGR = dtDesignAssumptionDetails.Rows[i]["TGR"].ToString();
                        //                designAssumpt.TWG = dtDesignAssumptionDetails.Rows[i]["TWG"].ToString();
                        //                designAssumpt.DELETE_FLAG = false;
                        //                designAssumpt.UPDATED_DATE = DateTime.Now;
                        //                designAssumpt.UPDATED_BY = userInformation.UserName;
                        //                DB.SubmitChanges();
                        //                //  typ = "UPD";
                        //            }

                        //        }
                        //        catch (Exception)
                        //        {
                        //            DB.DEV_REPORT_DESIGN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, designAssumpt);
                        //        }

                        //    }

                        //}

                    }
                    _status = true;
                    #endregion

                    #region LogDetails
                    rown = 1;
                    List<DEV_REPORT_SUB> lstexistingDatasLog = new List<DEV_REPORT_SUB>();
                    lstexistingDatasLog = ((from c in DB.DEV_REPORT_SUB
                                            where c.PART_NO == devModel.PartNo && c.DEV_RUN_NO == devModel.RunNo.ToDecimalValue()
                                            select c).ToList());
                    if (lstexistingDatasLog.Count > 0)
                    {
                        DB.DEV_REPORT_SUB.DeleteAllOnSubmit(lstexistingDatasLog);
                        DB.SubmitChanges();
                    }

                    for (int i = 0; i < dtLogDetails.Rows.Count; i++)
                    {
                        DEV_REPORT_SUB logDetails = (from c in DB.DEV_REPORT_SUB
                                                     where c.PART_NO == devModel.PartNo && c.DEV_RUN_NO == devModel.RunNo.ToDecimalValue() && c.STAGE_NO == rown
                                                     select c).FirstOrDefault<DEV_REPORT_SUB>();
                        try
                        {
                            if (logDetails.IsNotNullOrEmpty())
                            {
                                DB.DEV_REPORT_SUB.DeleteOnSubmit(logDetails);
                                DB.SubmitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                        }
                        //if (!logDetails.IsNotNullOrEmpty())
                        //{
                        try
                        {
                            if (!dtLogDetails.Rows[i]["REWORK_TOOL_DESIGN"].IsNotNullOrEmpty())
                            {
                                dtLogDetails.Rows[i]["REWORK_TOOL_DESIGN"] = "0";

                            }
                            if (!dtLogDetails.Rows[i]["REWORK_TOOL_MFG"].IsNotNullOrEmpty())
                            {
                                dtLogDetails.Rows[i]["REWORK_TOOL_MFG"] = "0";
                            }
                            if (dtLogDetails.Rows[i]["PROBLEM_FACED"].ToString().IsNotNullOrEmpty() || dtLogDetails.Rows[i]["COUNTER_MEASURE"].ToString().IsNotNullOrEmpty() || (dtLogDetails.Rows[i]["REWORK_TOOL_DESIGN"].ToString() != "0") || (dtLogDetails.Rows[i]["REWORK_TOOL_MFG"].ToString() != "0") || dtLogDetails.Rows[i]["REMARKS"].ToString().IsNotNullOrEmpty())
                            {
                                logDetails = new DEV_REPORT_SUB()
                                {
                                    PART_NO = devModel.PartNo.ToString(),
                                    DEV_RUN_NO = devModel.RunNo.ToDecimalValue(),
                                    STAGE_NO = rown,
                                    PROBLEM_FACED = dtLogDetails.Rows[i]["PROBLEM_FACED"].ToString(),
                                    COUNTER_MEASURE = dtLogDetails.Rows[i]["COUNTER_MEASURE"].ToString(),
                                    REWORK_TOOL_DESIGN = dtLogDetails.Rows[i]["REWORK_TOOL_DESIGN"].ToString().ToDecimalValue(),
                                    REWORK_TOOL_MFG = dtLogDetails.Rows[i]["REWORK_TOOL_MFG"].ToString().ToDecimalValue(),
                                    REMARKS = dtLogDetails.Rows[i]["REMARKS"].ToString(),
                                    DELETE_FLAG = false,
                                    ENTERED_DATE = DateTime.Now,
                                    ENTERED_BY = userInformation.UserName,
                                    ROWID = Guid.NewGuid()
                                };
                                DB.DEV_REPORT_SUB.InsertOnSubmit(logDetails);
                                DB.SubmitChanges();
                                //   typ = "INS";
                                rown = rown + 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DEV_REPORT_SUB.DeleteOnSubmit(logDetails);
                        }
                        //}
                        //else
                        //{
                        //    if (logDetails.IsNotNullOrEmpty())
                        //    {
                        //        try
                        //        {
                        //            try
                        //            {
                        //                DB.DEV_REPORT_SUB.DeleteOnSubmit(logDetails);
                        //                DB.SubmitChanges();
                        //            }
                        //            catch (Exception)
                        //            {
                        //                DB.DEV_REPORT_SUB.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, logDetails);
                        //            }
                        //            if (dtLogDetails.Rows[i]["PROBLEM_FACED"].ToString().IsNotNullOrEmpty() || dtLogDetails.Rows[i]["COUNTER_MEASURE"].ToString().IsNotNullOrEmpty() || (dtLogDetails.Rows[i]["REWORK_TOOL_DESIGN"].ToString() != "0") || (dtLogDetails.Rows[i]["REWORK_TOOL_MFG"].ToString() != "0") || dtLogDetails.Rows[i]["REMARKS"].ToString().IsNotNullOrEmpty())
                        //            {
                        //                logDetails = new DEV_REPORT_SUB()
                        //                {
                        //                    PART_NO = devModel.PartNo.ToString(),
                        //                    DEV_RUN_NO = devModel.RunNo.ToDecimalValue(),
                        //                    STAGE_NO = dtLogDetails.Rows[i]["STAGE_NO"].ToString().ToDecimalValue(),
                        //                    PROBLEM_FACED = dtLogDetails.Rows[i]["PROBLEM_FACED"].ToString(),
                        //                    COUNTER_MEASURE = dtLogDetails.Rows[i]["COUNTER_MEASURE"].ToString(),
                        //                    REWORK_TOOL_DESIGN = dtLogDetails.Rows[i]["REWORK_TOOL_DESIGN"].ToString().ToDecimalValue(),
                        //                    REWORK_TOOL_MFG = dtLogDetails.Rows[i]["REWORK_TOOL_MFG"].ToString().ToDecimalValue(),
                        //                    REMARKS = dtLogDetails.Rows[i]["REMARKS"].ToString(),
                        //                    DELETE_FLAG = false,
                        //                    ENTERED_DATE = DateTime.Now,
                        //                    ENTERED_BY = userInformation.UserName,
                        //                    ROWID = Guid.NewGuid(),
                        //                    UPDATED_DATE = DateTime.Now,
                        //                    UPDATED_BY = userInformation.UserName
                        //                };
                        //                DB.DEV_REPORT_SUB.InsertOnSubmit(logDetails);
                        //                DB.SubmitChanges();
                        //                //   typ = "UPD";
                        //            }
                        //        }
                        //        catch (Exception)
                        //        {
                        //            DB.DEV_REPORT_SUB.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, logDetails);
                        //        }

                        //    }

                        //}

                    }
                    _status = true;
                    #endregion

                    #region ShortClouserDetails
                    rown = 1;
                    List<DEV_REPORT_SHORT_CLOSURE> lstexistingDatasShortClosur = new List<DEV_REPORT_SHORT_CLOSURE>();
                    lstexistingDatasShortClosur = ((from c in DB.DEV_REPORT_SHORT_CLOSURE
                                                    where c.PART_NO == devModel.PartNo && c.RUN_NO == devModel.RunNo.ToDecimalValue()
                                                    select c).ToList());
                    if (lstexistingDatasShortClosur.Count > 0)
                    {
                        DB.DEV_REPORT_SHORT_CLOSURE.DeleteAllOnSubmit(lstexistingDatasShortClosur);
                        DB.SubmitChanges();
                    }

                    for (int i = 0; i < dtShortClouserDetails.Rows.Count; i++)
                    {
                        DEV_REPORT_SHORT_CLOSURE shrortClourDetails = (from c in DB.DEV_REPORT_SHORT_CLOSURE
                                                                       where c.PART_NO == devModel.PartNo && c.RUN_NO == devModel.RunNo.ToDecimalValue() && c.SNO == rown
                                                                       select c).FirstOrDefault<DEV_REPORT_SHORT_CLOSURE>();
                        try
                        {
                            if (shrortClourDetails.IsNotNullOrEmpty())
                            {
                                DB.DEV_REPORT_SHORT_CLOSURE.DeleteOnSubmit(shrortClourDetails);
                                DB.SubmitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                        }

                        try
                        {
                            shrortClourDetails = new DEV_REPORT_SHORT_CLOSURE()
                            {
                                PART_NO = devModel.PartNo.ToString(),
                                RUN_NO = devModel.RunNo.ToDecimalValue(),
                                SNO = rown,
                                REASON = dtShortClouserDetails.Rows[i]["REASON"].ToString(),
                                WHY = dtShortClouserDetails.Rows[i]["WHY"].ToString(),
                                SHORT_ACTION = dtShortClouserDetails.Rows[i]["SHORT_ACTION"].ToString(),
                                // SHORT_ACTION_DATE = dtShortClouserDetails.Rows[i]["SHORT_ACTION_DATE"].ToString().ToDateTimeValue(),
                                LONG_ACTION = dtShortClouserDetails.Rows[i]["LONG_ACTION"].ToString(),
                                //LONG_ACTION_DATE = dtShortClouserDetails.Rows[i]["LONG_ACTION_DATE"].ToString().ToDateTimeValue(),
                                //TRIAL_DATE = dtShortClouserDetails.Rows[i]["TRIAL_DATE"].ToString().ToDateTimeValue(),
                                DELETE_FLAG = false,
                                ENTERED_DATE = DateTime.Now,
                                ENTERED_BY = userInformation.UserName,
                                ROWID = Guid.NewGuid()
                            };
                            if (dtShortClouserDetails.Rows[i]["SHORT_ACTION_DATE"].ToString() != "")
                                shrortClourDetails.SHORT_ACTION_DATE = Convert.ToDateTime(dtShortClouserDetails.Rows[i]["SHORT_ACTION_DATE"]);
                            else
                                shrortClourDetails.SHORT_ACTION_DATE = null;

                            if (dtShortClouserDetails.Rows[i]["LONG_ACTION_DATE"].ToString() != "")
                                shrortClourDetails.LONG_ACTION_DATE = Convert.ToDateTime(dtShortClouserDetails.Rows[i]["LONG_ACTION_DATE"]);
                            else
                                shrortClourDetails.LONG_ACTION_DATE = null;

                            if (dtShortClouserDetails.Rows[i]["TRIAL_DATE"].ToString() != "")
                                shrortClourDetails.TRIAL_DATE = Convert.ToDateTime(dtShortClouserDetails.Rows[i]["TRIAL_DATE"]);
                            else
                                shrortClourDetails.TRIAL_DATE = null;
                            DB.DEV_REPORT_SHORT_CLOSURE.InsertOnSubmit(shrortClourDetails);
                            DB.SubmitChanges();
                            //  typ = "INS";
                            rown = rown + 1;
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DEV_REPORT_SHORT_CLOSURE.DeleteOnSubmit(shrortClourDetails);
                        }
                        //}
                        //else
                        //{
                        //    if (shrortClourDetails.IsNotNullOrEmpty())
                        //    {
                        //        try
                        //        {
                        //            if (dtShortClouserDetails.Rows[i]["REASON"].ToString().IsNotNullOrEmpty() || dtShortClouserDetails.Rows[i]["WHY"].ToString().IsNotNullOrEmpty() || dtShortClouserDetails.Rows[i]["SHORT_ACTION"].ToString().IsNotNullOrEmpty() || dtShortClouserDetails.Rows[i]["LONG_ACTION"].ToString().IsNotNullOrEmpty())
                        //            {

                        //                shrortClourDetails.REASON = dtShortClouserDetails.Rows[i]["REASON"].ToString();
                        //                shrortClourDetails.WHY = dtShortClouserDetails.Rows[i]["WHY"].ToString();
                        //                shrortClourDetails.SHORT_ACTION = dtShortClouserDetails.Rows[i]["SHORT_ACTION"].ToString();
                        //                // shrortClourDetails.SHORT_ACTION_DATE = dtShortClouserDetails.Rows[i]["SHORT_ACTION_DATE"].ToString().ToDateTimeValue();
                        //                shrortClourDetails.LONG_ACTION = dtShortClouserDetails.Rows[i]["LONG_ACTION"].ToString();
                        //                // shrortClourDetails.LONG_ACTION_DATE = dtShortClouserDetails.Rows[i]["LONG_ACTION_DATE"].ToString().ToDateTimeValue();
                        //                // shrortClourDetails.TRIAL_DATE = dtShortClouserDetails.Rows[i]["TRIAL_DATE"].ToString().ToDateTimeValue();
                        //                shrortClourDetails.DELETE_FLAG = false;
                        //                shrortClourDetails.UPDATED_DATE = DateTime.Now;
                        //                shrortClourDetails.UPDATED_BY = userInformation.UserName;

                        //                if (dtShortClouserDetails.Rows[i]["SHORT_ACTION_DATE"].ToString() != "")
                        //                    shrortClourDetails.SHORT_ACTION_DATE = Convert.ToDateTime(dtShortClouserDetails.Rows[i]["SHORT_ACTION_DATE"]);
                        //                else
                        //                    shrortClourDetails.SHORT_ACTION_DATE = null;

                        //                if (dtShortClouserDetails.Rows[i]["LONG_ACTION_DATE"].ToString() != "")
                        //                    shrortClourDetails.LONG_ACTION_DATE = Convert.ToDateTime(dtShortClouserDetails.Rows[i]["LONG_ACTION_DATE"]);
                        //                else
                        //                    shrortClourDetails.LONG_ACTION_DATE = null;

                        //                if (dtShortClouserDetails.Rows[i]["TRIAL_DATE"].ToString() != "")
                        //                    shrortClourDetails.TRIAL_DATE = Convert.ToDateTime(dtShortClouserDetails.Rows[i]["TRIAL_DATE"]);
                        //                else
                        //                    shrortClourDetails.TRIAL_DATE = null;

                        //                DB.SubmitChanges();
                        //                //  typ = "UPD";
                        //            }
                        //        }
                        //        catch (Exception)
                        //        {
                        //            DB.DEV_REPORT_SHORT_CLOSURE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, shrortClourDetails);
                        //        }

                        //    }

                        //}

                    }
                    _status = true;
                    #endregion

                }
                return _status;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        #region PrintReport

        public DataView GetTabPrintReportDevMainDetails(DevelopmentReportModel devModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (devModel.PartNo.IsNotNullOrEmpty() && devModel.RunNo.IsNotNullOrEmpty())
                {
                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_MAIN
                                              where o.PART_NO == devModel.PartNo && o.DEV_RUN_NO == devModel.RunNo.ToDecimalValue() && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new
                                              {
                                                  o.PART_NO,
                                                  DEV_RUN_NO = (int)o.DEV_RUN_NO,
                                                  o.CUST_COMP,
                                                  CUST_COMP_DESC = devModel.PartNoDesc,
                                                  o.FORGE_DDREP,
                                                  o.FORGE_ZAPREP,
                                                  o.RECORD,
                                                  o.REPORT_DATE,
                                                  NO_FORG_SHT = (int)o.NO_FORG_SHT
                                              }).ToList());
                    if (dt != null)
                    {
                        devModel.DevMainDetails = dt.DefaultView;
                    }
                }
                return devModel.DevMainDetails;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetTabPrintReportDesignAssumptionDetails(DevelopmentReportModel devModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (devModel.PartNo.IsNotNullOrEmpty() && devModel.RunNo.IsNotNullOrEmpty())
                {

                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_DESIGN
                                              where o.PART_NO == devModel.PartNo && o.DEV_RUN_NO == devModel.RunNo.ToDecimalValue() && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, DEV_RUN_NO = (int)o.DEV_RUN_NO, SNO = (int)o.SNO, o.CFT_DISS, o.JUSTIFICATION, o.TGR, o.TWG }).ToList());
                    if (dt != null) devModel.DesignAssumptionDetails = dt.DefaultView;
                }

                return devModel.DesignAssumptionDetails;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetTabPrintReportLogDetails(DevelopmentReportModel devModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (devModel.PartNo.IsNotNullOrEmpty() && devModel.RunNo.IsNotNullOrEmpty())
                {

                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_SUB
                                              where o.PART_NO == devModel.PartNo && o.DEV_RUN_NO == devModel.RunNo.ToDecimalValue() && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, DEV_RUN_NO = (int)o.DEV_RUN_NO, STAGE_NO = (int)o.STAGE_NO, o.PROBLEM_FACED, o.COUNTER_MEASURE, o.REWORK_TOOL_DESIGN, o.REWORK_TOOL_MFG, o.REMARKS }).ToList());
                    if (dt != null) devModel.LogDetails = dt.DefaultView;

                }
                return devModel.LogDetails;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetTabPrintReportShortClouserDetails(DevelopmentReportModel devModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (devModel.PartNo.IsNotNullOrEmpty() && devModel.RunNo.IsNotNullOrEmpty())
                {
                    dt = ToDataTableWithType((from o in DB.DEV_REPORT_SHORT_CLOSURE
                                              where o.PART_NO == devModel.PartNo && o.RUN_NO == devModel.RunNo.ToDecimalValue() && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, RUN_NO = (int)o.RUN_NO, SNO = (int)o.SNO, o.REASON, o.WHY, o.SHORT_ACTION, o.SHORT_ACTION_DATE, o.LONG_ACTION, o.LONG_ACTION_DATE, o.TRIAL_DATE }).ToList());
                    if (dt != null) devModel.ShortClosureDetails = dt.DefaultView;
                }

                return devModel.ShortClosureDetails;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        #endregion

    }
}
