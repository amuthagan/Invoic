using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
namespace ProcessDesigner.BLL
{
    public class APQPBll : Essential
    {
        DataTable dtAPQPData;
        public APQPBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        private void BuildAPQPChart()
        {
            try
            {
                dtAPQPData = new DataTable();
                dtAPQPData.Columns.Add("HIDDEN_SL_NO");
                dtAPQPData.Columns.Add("SL_NO");
                dtAPQPData.Columns.Add("APQP_ACTIVITY");
                dtAPQPData.Columns.Add("STATUS");
                dtAPQPData.Columns.Add("COMPLETION_DATE", Type.GetType("System.DateTime"));
                dtAPQPData.Columns.Add("COMMENT");
                dtAPQPData.Columns.Add("LEAD_RESP");
                dtAPQPData.Columns.Add("DUE_DATE", Type.GetType("System.DateTime"));

                AddRow("", "III", "Process Design And Development", "");
                AddRow("0", "1.", "Does Customer have any packaging standards?", "Mktg");
                AddRow("1", "2.", "Has Product / Process Qualitysystem review been conducted in line with Appendix - A4?", "CFT");
                AddRow("2", "3.", "Has Process flow Chart been developed with the aid of Checklist Appendix A-6?", "D&D");
                AddRow("3", "4.", "Has PFMEA been developed? Appendix A7", "D&D");
                AddRow("4", "5.", "Has pre-launch control plan been prepared?", "CFT");
                AddRow("5", "6.", "Does adequate process instructions exist for all operating personnel to address all quality requirements?", "Mfg");
                AddRow("6", "7.", "Has MSA Plan been developed for this product?", "QAD");
                AddRow("7", "8.", "Has preliminary process capability plan been developed?", "CFT");
                AddRow("8", "9.", "Has packaging specifications been developed?", "CFT");
                AddRow("", "IV", "Product and Process Validation", "");
                AddRow("9", "1.", "Does the production trial run comply with planned manufacturing procecss?", "D&D,Mfg");
                AddRow("10", "2.", "Is MSA satisfactory?", "QAD");
                AddRow("11", "3.", "Has preliminary process capability studies have been conducted on characteristics identified in the control plan?", "Mfg,D&D");
                AddRow("12", "4.", "Satisfactory production validation testing?", "QAD");
                AddRow("13", "5.", "PPAP - PSW sent?", "D&D");
                AddRow("", "", "Post PPAP - PSW Approval", "");
                AddRow("14", "6.", "Has packaging evaluation been done?", "Mktg");
                AddRow("15", "7.", "Release of production control plan", "D&D");
                AddRow("", "", "Post Bulk Establishment", "");
                AddRow("16", "8.", "Has a formal quality planning sign-off been conducted?", "D&D");
                AddRow("", "V", "Feedback assessment and corrective action", "");
                AddRow("17", "1.", "Continuous improvement review", "CFT");
                AddRow("18", "2.", "Customer satisfaction review", "CFT");
                dtAPQPData.AcceptChanges();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public DataTable GetApQpChart(string partNo)
        {
            try
            {
                List<APQP_ACTIVITY> lstActivity = new List<APQP_ACTIVITY>();
                List<MFM_MAST> lstMFM = new List<MFM_MAST>();
                List<CFT_MEET_MASTER> lstCFT = new List<CFT_MEET_MASTER>();
                DateTime? dCFT = null;
                DateTime? dPPAP = null;

                DataRow[] drRow;
                BuildAPQPChart();

                if (partNo.ToValueAsString() != "")
                {
                    lstActivity = (from row in DB.APQP_ACTIVITY
                                   where row.PART_NO == partNo
                                   orderby row.SL_NO
                                   select row).ToList<APQP_ACTIVITY>();

                    lstMFM = (from row in DB.MFM_MAST
                              where row.PART_NO == partNo
                              select row).ToList<MFM_MAST>();

                    lstCFT = (from row in DB.CFT_MEET_MASTER
                              where row.PART_NO == partNo
                              select row).ToList<CFT_MEET_MASTER>();
                    foreach (CFT_MEET_MASTER cmm in lstCFT)
                    {
                        if (dCFT == null)
                        {
                            dCFT = cmm.CFT_MEET_DATE;
                        }
                        else
                        {
                            if (cmm.CFT_MEET_DATE > dCFT)
                            {
                                dCFT = cmm.CFT_MEET_DATE;
                            }
                        }
                        dPPAP = cmm.TIME_PPAP_START;
                    }

                    drRow = dtAPQPData.Select("HIDDEN_SL_NO = '13'");
                    if (drRow.Length > 0 && lstMFM.Count > 0)
                    {
                        if (lstMFM[0].PPAP_PLAN != null)
                        {
                            drRow[0]["DUE_DATE"] = Convert.ToDateTime(lstMFM[0].PPAP_PLAN);
                        }
                    }

                    if (dCFT != null)
                    {
                        for (int ictr = 2; ictr <= 8; ictr++)
                        {
                            dtAPQPData.Rows[ictr]["COMPLETION_DATE"] = Convert.ToDateTime(dCFT);
                        }
                    }

                    if (dPPAP != null)
                    {
                        dtAPQPData.Rows[11]["DUE_DATE"] = Convert.ToDateTime(dPPAP);
                        dtAPQPData.Rows[12]["DUE_DATE"] = Convert.ToDateTime(dPPAP);
                        dtAPQPData.Rows[13]["DUE_DATE"] = Convert.ToDateTime(dPPAP);
                        dtAPQPData.Rows[14]["DUE_DATE"] = Convert.ToDateTime(dPPAP);
                    }

                    foreach (APQP_ACTIVITY apq in lstActivity)
                    {
                        drRow = dtAPQPData.Select("HIDDEN_SL_NO = '" + apq.SL_NO.ToValueAsString() + "'");
                        if (drRow.Length > 0)
                        {
                            /*
                            dtAPQPData.Columns.Add("STATUS");
                            dtAPQPData.Columns.Add("COMPLETION_DATE");
                            dtAPQPData.Columns.Add("COMMENT");
                            dtAPQPData.Columns.Add("LEAD_RESP");
                            dtAPQPData.Columns.Add("DUE_DATE");
                            */
                            drRow[0]["STATUS"] = apq.STATUS;
                            if (apq.COMPLETION_DATE != null)
                            {
                                drRow[0]["COMPLETION_DATE"] = Convert.ToDateTime(apq.COMPLETION_DATE);
                            }
                            else
                            {
                                drRow[0]["COMPLETION_DATE"] = DBNull.Value;
                            }

                            drRow[0]["COMMENT"] = apq.COMMENTS_ACTION_REQ;
                            if (apq.DUE_DATE != null)
                            {
                                drRow[0]["DUE_DATE"] = Convert.ToDateTime(apq.DUE_DATE);
                            }
                            else
                            {
                                drRow[0]["DUE_DATE"] = DBNull.Value;
                            }
                        }
                    }
                }
                return dtAPQPData;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddRow(string hiddenSlNo, string slNo, string apQpActivity, string leadResp)
        {
            DataRow drRow;
            try
            {
                drRow = dtAPQPData.NewRow();
                drRow["HIDDEN_SL_NO"] = hiddenSlNo;
                drRow["SL_NO"] = slNo;
                drRow["APQP_ACTIVITY"] = apQpActivity;
                drRow["LEAD_RESP"] = leadResp;
                dtAPQPData.Rows.Add(drRow);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetPartNoDetails()
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

        public bool Save(DataTable dtAPQP, string partNo)
        {
            bool blnAdd = false;
            bool insert = false;
            bool update = false;
            bool submit = false;
            List<APQP_ACTIVITY> lstActivity = new List<APQP_ACTIVITY>();
            APQP_ACTIVITY apqpActivity = new APQP_ACTIVITY();
            try
            {
                lstActivity = (from row in DB.APQP_ACTIVITY
                               where row.PART_NO == partNo
                               orderby row.SL_NO
                               select row).ToList<APQP_ACTIVITY>();
                foreach (DataRow drRow in dtAPQP.Rows)
                {
                    blnAdd = false;
                    insert = false;
                    update = false;
                    submit = false;

                    if (drRow["HIDDEN_SL_NO"].ToValueAsString().Trim() != "")
                    {
                        apqpActivity = new APQP_ACTIVITY();
                        apqpActivity = (from row in lstActivity
                                        where row.SL_NO == Convert.ToInt16(drRow["HIDDEN_SL_NO"].ToValueAsString())
                                        select row).FirstOrDefault<APQP_ACTIVITY>();
                        if (apqpActivity == null)
                        {
                            apqpActivity = new APQP_ACTIVITY();
                            blnAdd = true;
                            apqpActivity.ROWID = Guid.NewGuid();
                            apqpActivity.PART_NO = partNo;
                            apqpActivity.SL_NO = Convert.ToInt16(drRow["HIDDEN_SL_NO"].ToValueAsString());
                        }

                        apqpActivity.STATUS = drRow["STATUS"].ToValueAsString().Trim();
                        if (drRow["COMPLETION_DATE"] != DBNull.Value)
                        {
                            apqpActivity.COMPLETION_DATE = Convert.ToDateTime(drRow["COMPLETION_DATE"]);
                        }
                        else
                        {
                            apqpActivity.COMPLETION_DATE = null;
                        }

                        apqpActivity.COMMENTS_ACTION_REQ = drRow["COMMENT"].ToValueAsString().Trim();
                        if (drRow["DUE_DATE"] != DBNull.Value)
                        {
                            apqpActivity.DUE_DATE = Convert.ToDateTime(drRow["DUE_DATE"]);
                        }
                        else
                        {
                            apqpActivity.DUE_DATE = null;
                        }
                        if (blnAdd == true)
                        {
                            insert = true;
                            DB.APQP_ACTIVITY.InsertOnSubmit(apqpActivity);
                        }
                        else
                        {
                            update = true;
                        }
                        submit = true;
                        DB.SubmitChanges();                        
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (update == true)
                    {
                        DB.APQP_ACTIVITY.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, apqpActivity);
                    }
                    if (insert == true)
                    {
                        DB.APQP_ACTIVITY.DeleteOnSubmit(apqpActivity);
                    }
                }
                throw ex.LogException();
            }
        }

    }
}
