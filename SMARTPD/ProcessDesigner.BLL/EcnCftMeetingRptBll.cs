using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data;
using System.Reflection;

namespace ProcessDesigner.BLL
{
    public class EcnCftMeetingRptBll : Essential
    {

        public EcnCftMeetingRptBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public bool GetCPRptCPM(EcnCftMeetingRptModel cpMeetModel)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.DESIGN.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER1 = o.MEMBER }).ToList());
                cpMeetModel.DtCtm1 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.MKT.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER2 = o.MEMBER }).ToList());
                cpMeetModel.DtCtm2 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.PRODU.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER3 = o.MEMBER }).ToList());
                cpMeetModel.DtCtm3 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.QUALITY.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER4 = o.MEMBER }).ToList());
                cpMeetModel.DtCtm4 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.PROCESS.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER5 = o.MEMBER }).ToList());
                cpMeetModel.DtCtm5 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.OTHERS.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER6 = o.MEMBER }).ToList());
                cpMeetModel.DtCtm6 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.OTHERS.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER7 = o.MEMBER }).ToList());
                cpMeetModel.DtCtm7 = (dt != null) ? dt.DefaultView : null;


                return true;
            }
            catch (Exception ex)
            {
                ex.LogException(); return false;
            }
        }
        public bool GetPartNoDetails(EcnCftMeetingRptModel cpMeetModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                cpMeetModel.DtPartNumber = (dt != null) ? dt.DefaultView : null;
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException(); return false;
            }
        }
        public bool GetRecordsByPartNo(EcnCftMeetingRptModel cpMeetModel, ref string stsMsg)
        {

            try
            {
                decimal? maxddPcn = null;
                try
                {
                    maxddPcn = ((from c in DB.DD_PCN
                                 where c.REQUSTED_BY == "CUSTOMER" && c.PART_NO == cpMeetModel.PartNo
                                 select c.SFL_DRAW_ISSUENO1).Max());

                }
                catch (Exception ex)
                {
                    ex.LogException();
                    maxddPcn = null;
                }
                if (maxddPcn.IsNotNullOrEmpty())
                {
                    cpMeetModel.CFTMeetingIssueNo = maxddPcn.ToValueAsString();
                }
                else
                {
                    cpMeetModel.CFTMeetingIssueNo = "";
                    //stsMsg = "No issue No. for PCN Sheet";
                    stsMsg = "";
                }


                // DataTable dt = new DataTable();
                DateTime? dtMeet;
                ECN_CFT_MEET_MASTER cftM = null;
                ECN_CFT_MEET_MASTER cft = (from o in DB.ECN_CFT_MEET_MASTER
                                           where o.PART_NO == cpMeetModel.PartNo
                                           select o).FirstOrDefault<ECN_CFT_MEET_MASTER>();
                if (cft.IsNotNullOrEmpty())
                {

                    dtMeet = cft.CFT_MEET_DATE;
                    cftM = (from o in DB.ECN_CFT_MEET_MASTER
                            where o.PART_NO == cpMeetModel.PartNo && o.CFT_MEET_DATE == dtMeet
                            select o).FirstOrDefault<ECN_CFT_MEET_MASTER>();
                }
                if (cftM.IsNotNullOrEmpty())
                {
                    cpMeetModel.Customer = cftM.CUSTOMER.ToValueAsString();
                    cpMeetModel.AnnualRequirments = cftM.ANNUAL_REQURIE.ToValueAsString();
                    cpMeetModel.PPAPProductionQty = cftM.ORD_QTY.ToValueAsString();
                    cpMeetModel.PPAPSampleQty = cftM.PPAP_QTY.ToValueAsString();
                    cpMeetModel.CustPartNo = cftM.CUST_PART_NO.ToValueAsString();


                    cpMeetModel.NatureOfChange = cftM.NATURE_OF_CHANGE.ToValueAsString();
                    cpMeetModel.StockPositionFG = cftM.FG.ToValueAsString();
                    cpMeetModel.Wip = cftM.WIP.ToValueAsString();
                    cpMeetModel.Disposition = cftM.DISPOSITION.ToValueAsString();
                    if (cftM.RECEIVING_DATE.ToString().IsNotNullOrEmpty())
                    {
                        cpMeetModel.ReceivingDate = cftM.RECEIVING_DATE.Value.Date;
                        if (cpMeetModel.ReceivingDate.ToValueAsString().Trim() == "01/01/0001")
                            cpMeetModel.ReceivingDate = null;
                    }
                    else
                    {
                        cpMeetModel.ReceivingDate = null;
                    }
                    if (cftM.REVIEW_DATE.ToString().IsNotNullOrEmpty())
                    {
                        cpMeetModel.ReviewDate = cftM.REVIEW_DATE.Value.Date;
                        if (cpMeetModel.ReviewDate.ToValueAsString().Trim() == "01/01/0001")
                            cpMeetModel.ReviewDate = null;
                    }
                    else
                    {
                        cpMeetModel.ReviewDate = null;
                    }
                    switch (cftM.COST_IMPACT.ToValueAsString().ToUpper())
                    {
                        case "Y":
                        case "YES":
                            cpMeetModel.CostimpactYes = true;
                            cpMeetModel.CostimpactNo = false;
                            break;
                        case "N":
                        case "NO":
                            cpMeetModel.CostimpactYes = false;
                            cpMeetModel.CostimpactNo = true;
                            break;
                        default:
                            cpMeetModel.CostimpactYes = false;
                            cpMeetModel.CostimpactNo = false;
                            break;
                    }
                    switch (cftM.LOCATION.ToValueAsString().ToUpper())
                    {
                        case "M":
                            cpMeetModel.rbtnMIsChecked = true;
                            cpMeetModel.rbtnKIsChecked = false;
                            cpMeetModel.rbtnYIsChecked = false;
                            break;
                        case "K":
                            cpMeetModel.rbtnMIsChecked = false;
                            cpMeetModel.rbtnKIsChecked = true;
                            cpMeetModel.rbtnYIsChecked = false;
                            break;
                        case "Y":
                            cpMeetModel.rbtnMIsChecked = false;
                            cpMeetModel.rbtnKIsChecked = false;
                            cpMeetModel.rbtnYIsChecked = true;
                            break;
                        default:
                            cpMeetModel.rbtnMIsChecked = false;
                            cpMeetModel.rbtnKIsChecked = false;
                            cpMeetModel.rbtnYIsChecked = false;
                            break;
                    }
                    if (cftM.CUST_REQ_DATE.ToString().IsNotNullOrEmpty())
                    {
                        cpMeetModel.CustRequiredDate = cftM.CUST_REQ_DATE;
                        if (cpMeetModel.CustRequiredDate.ToValueAsString().Trim() == "01/01/0001")
                            cpMeetModel.CustRequiredDate = null;
                    }
                    else
                    {
                        cpMeetModel.CustRequiredDate = null;
                    }

                    cpMeetModel.CFTMeetingIssueNo = cftM.CFT_MEET_ISSUE_NO.ToValueAsString();

                    if (cftM.CFT_MEET_DATE.ToString().IsNotNullOrEmpty())
                    {
                        cpMeetModel.CFTMeetingDate = Convert.ToDateTime(cftM.CFT_MEET_DATE);
                        if (cpMeetModel.CFTMeetingDate.ToValueAsString().Trim() == "01/01/0001")
                            cpMeetModel.CFTMeetingDate = null;
                    }
                    else
                    {
                        cpMeetModel.CFTMeetingDate = null;
                    }
                    cpMeetModel.Application = cftM.APPLICATION.ToValueAsString();
                    cpMeetModel.IsSafetyPart = cftM.SAFTY_PART.ToValueAsString();

                    cpMeetModel.CustPartName = cftM.CUST_PART_DESC;

                    cpMeetModel.PG = cftM.PG_CATEGORY;
                    cpMeetModel.PartDesc = cftM.SFL_PART_DESC;
                    cpMeetModel.RMSpec = cftM.RM_SPEC;
                    cpMeetModel.PackingSpec = cftM.PACKING;
                    cpMeetModel.CheesWt = cftM.CHEESE_WT.ToValueAsString();
                    cpMeetModel.FinishWt = cftM.FINISH_WT.ToValueAsString();


                    switch (cpMeetModel.IsSafetyPart)
                    {
                        case "Y":
                            cpMeetModel.ChkSPYesIsChecked = true;
                            cpMeetModel.ChkSPNoIsChecked = false;
                            break;
                        case "N":
                            cpMeetModel.ChkSPYesIsChecked = false;
                            cpMeetModel.ChkSPNoIsChecked = true;
                            break;

                        default:
                            cpMeetModel.ChkSPYesIsChecked = false;
                            cpMeetModel.ChkSPNoIsChecked = false;
                            break;
                    }
                    cpMeetModel.IsProtoType = cftM.PROTO_TYPE.ToValueAsString();
                    switch (cpMeetModel.IsProtoType)
                    {
                        case "Y":
                            cpMeetModel.ChkPTYesIsChecked = true;
                            cpMeetModel.ChkPTNoIsChecked = false;
                            break;
                        case "N":
                            cpMeetModel.ChkPTYesIsChecked = false;
                            cpMeetModel.ChkPTNoIsChecked = true;
                            break;

                        default:
                            cpMeetModel.ChkPTYesIsChecked = false;
                            cpMeetModel.ChkPTNoIsChecked = false;
                            break;
                    }
                    cpMeetModel.Remarks = cftM.REMARK.ToValueAsString();

                    //cpMeetModel.DtTimigChart[4][2] = (cftM.TIME_DOC_REL_START.IsNotNullOrEmpty()) ? cftM.TIME_DOC_REL_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[5][2] = (cftM.TIME_BOGAUGE_START.IsNotNullOrEmpty()) ? cftM.TIME_BOGAUGE_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[6][2] = (cftM.TIME_TOOL_MANUFAC_START.IsNotNullOrEmpty()) ? cftM.TIME_TOOL_MANUFAC_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[9][2] = (cftM.TIME_FORG_START.IsNotNullOrEmpty()) ? cftM.TIME_FORG_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[10][2] = (cftM.TIME_SECO_START.IsNotNullOrEmpty()) ? cftM.TIME_SECO_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[11][2] = (cftM.TIME_HEAT_START.IsNotNullOrEmpty()) ? cftM.TIME_HEAT_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[13][2] = (cftM.TIME_PPAP_START.IsNotNullOrEmpty()) ? cftM.TIME_PPAP_START.Value.Date.ToDateAsString() : "";

                    //cpMeetModel.DtTimigChart[4][4] = (cftM.TIME_DOC_REL_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_DOC_REL_REVISED.Value.Date.ToDateAsString() : "";
                    ////DOnt uncomment //cpMeetModel.DtTimigChart[5][4] = dt.Rows[0]["BOGAUGE_ACTUAL"].Value.Date.ToDateAsString();
                    //cpMeetModel.DtTimigChart[6][4] = (cftM.TIME_TOOL_MANUFAC_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_TOOL_MANUFAC_REVISED.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[9][4] = (cftM.TIME_FORG_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_FORG_REVISED.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[10][4] = (cftM.TIME_SECO_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_SECO_REVISED.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[11][4] = (cftM.TIME_HEAT_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_HEAT_REVISED.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[13][4] = (cftM.TIME_PPAP_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_PPAP_REVISED.Value.Date.ToDateAsString() : "";

                    //cpMeetModel.DtTimigChart[5][3] = (cftM.TIME_BOGAUGE_END.IsNotNullOrEmpty()) ? cftM.TIME_BOGAUGE_END.Value.Date.ToDateAsString() : "";


                    //cpMeetModel.DtTimigChart[8][2] = (cftM.TIME_RM_PLAN_START.IsNotNullOrEmpty()) ? cftM.TIME_RM_PLAN_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[8][3] = (cftM.TIME_RM_PLAN_END.IsNotNullOrEmpty()) ? cftM.TIME_RM_PLAN_END.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[8][4] = (cftM.TIME_RM_PLAN_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_RM_PLAN_REVISED.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[8][5] = (cftM.TIME_RM_FINSIH.IsNotNullOrEmpty()) ? cftM.TIME_RM_FINSIH.Value.Date.ToDateAsString() : "";


                    //cpMeetModel.DtTimigChart[7][2] = (cftM.TIME_BOTOOLS_PLAN_START.IsNotNullOrEmpty()) ? cftM.TIME_BOTOOLS_PLAN_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[7][3] = (cftM.TIME_BOTOOLS_PLAN_END.IsNotNullOrEmpty()) ? cftM.TIME_BOTOOLS_PLAN_END.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[7][4] = (cftM.TIME_BOTOOLS_PLAN_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_BOTOOLS_PLAN_REVISED.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[7][5] = (cftM.TIME_BOTOOLS_FINISH.IsNotNullOrEmpty()) ? cftM.TIME_BOTOOLS_FINISH.Value.Date.ToDateAsString() : "";


                    //cpMeetModel.DtTimigChart[15][2] = (cftM.TIME_CORRECTIVE_PLAN_START.IsNotNullOrEmpty()) ? cftM.TIME_CORRECTIVE_PLAN_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[15][3] = (cftM.TIME_CORRECTIVE_PLAN_END.IsNotNullOrEmpty()) ? cftM.TIME_CORRECTIVE_PLAN_END.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[15][4] = (cftM.TIME_CORRECTIVE_PLAN_REVISED.IsNotNullOrEmpty()) ? cftM.TIME_CORRECTIVE_PLAN_REVISED.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[15][5] = (cftM.TIME_CORRECTIVE_FINISH.IsNotNullOrEmpty()) ? cftM.TIME_CORRECTIVE_FINISH.Value.Date.ToDateAsString() : "";


                    //cpMeetModel.DtTimigChart[16][2] = (cftM.TIME_PRODUCT_PLAN_START.IsNotNullOrEmpty()) ? cftM.TIME_PRODUCT_PLAN_START.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[16][3] = (cftM.TIME_PRODUCT_PLAN_END.IsNotNullOrEmpty()) ? cftM.TIME_PRODUCT_PLAN_END.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[16][4] = (cftM.TIME_PRODUCT_PLAN_MODIFY.IsNotNullOrEmpty()) ? cftM.TIME_PRODUCT_PLAN_MODIFY.Value.Date.ToDateAsString() : "";
                    //cpMeetModel.DtTimigChart[16][5] = (cftM.TIME_PRODUCT_FINISH.IsNotNullOrEmpty()) ? cftM.TIME_PRODUCT_FINISH.Value.Date.ToDateAsString() : "";

                    if (cftM.TIME_DOC_REL_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[4][2] = cftM.TIME_DOC_REL_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[4][2] = DBNull.Value;
                    }

                    if (cftM.TIME_BOGAUGE_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[5][2] = cftM.TIME_BOGAUGE_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[5][2] = DBNull.Value;
                    }

                    if (cftM.TIME_TOOL_MANUFAC_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[6][2] = cftM.TIME_TOOL_MANUFAC_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[6][2] = DBNull.Value;
                    }

                    if (cftM.TIME_TOOL_MANUFAC_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[6][2] = cftM.TIME_TOOL_MANUFAC_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[6][2] = DBNull.Value;
                    }

                    if (cftM.TIME_FORG_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[9][2] = cftM.TIME_FORG_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[9][2] = DBNull.Value;
                    }
                    if (cftM.TIME_SECO_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[10][2] = cftM.TIME_SECO_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[10][2] = DBNull.Value;
                    }
                    if (cftM.TIME_HEAT_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[11][2] = cftM.TIME_HEAT_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[11][2] = DBNull.Value;
                    }
                    if (cftM.TIME_PPAP_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[13][2] = cftM.TIME_PPAP_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[13][2] = DBNull.Value;

                    }
                    if (cftM.TIME_DOC_REL_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[4][4] = cftM.TIME_DOC_REL_REVISED;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[4][4] = DBNull.Value;

                    }
                    if (cftM.TIME_TOOL_MANUFAC_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[6][4] = cftM.TIME_TOOL_MANUFAC_REVISED;
                    }
                    else
                    {

                        cpMeetModel.DtTimigChart[6][4] = DBNull.Value;
                    }
                    if (cftM.TIME_FORG_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[9][4] = cftM.TIME_FORG_REVISED;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[9][4] = DBNull.Value;

                    }
                    if (cftM.TIME_SECO_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[10][4] = cftM.TIME_SECO_REVISED;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[10][4] = DBNull.Value;

                    }
                    if (cftM.TIME_HEAT_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[11][4] = cftM.TIME_HEAT_REVISED;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[11][4] = DBNull.Value;
                    }
                    if (cftM.TIME_PPAP_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[13][4] = cftM.TIME_PPAP_REVISED;
                    }
                    else
                    {

                        cpMeetModel.DtTimigChart[13][4] = DBNull.Value;
                    }
                    if (cftM.TIME_BOGAUGE_END.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[5][3] = cftM.TIME_BOGAUGE_END;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[5][3] = DBNull.Value;

                    }
                    if (cftM.TIME_RM_PLAN_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[8][2] = cftM.TIME_RM_PLAN_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[8][2] = DBNull.Value;

                    }

                    if (cftM.TIME_RM_PLAN_END.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[8][3] = cftM.TIME_RM_PLAN_END;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[8][3] = DBNull.Value;

                    }

                    if (cftM.TIME_RM_PLAN_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[8][4] = cftM.TIME_RM_PLAN_REVISED;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[8][4] = DBNull.Value;

                    }

                    if (cftM.TIME_RM_FINSIH.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[8][5] = cftM.TIME_RM_FINSIH;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[8][5] = DBNull.Value;

                    }

                    if (cftM.TIME_BOTOOLS_PLAN_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[7][2] = cftM.TIME_BOTOOLS_PLAN_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[7][2] = DBNull.Value;

                    }

                    if (cftM.TIME_BOTOOLS_PLAN_END.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[7][3] = cftM.TIME_BOTOOLS_PLAN_END;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[7][3] = DBNull.Value;

                    }

                    if (cftM.TIME_BOTOOLS_PLAN_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[7][4] = cftM.TIME_BOTOOLS_PLAN_REVISED;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[7][4] = DBNull.Value;
                    }

                    if (cftM.TIME_BOTOOLS_FINISH.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[7][5] = cftM.TIME_BOTOOLS_FINISH;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[7][5] = DBNull.Value;
                    }

                    if (cftM.TIME_CORRECTIVE_PLAN_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[15][2] = cftM.TIME_CORRECTIVE_PLAN_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[15][2] = DBNull.Value;
                    }

                    if (cftM.TIME_CORRECTIVE_PLAN_END.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[15][3] = cftM.TIME_CORRECTIVE_PLAN_END;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[15][3] = DBNull.Value;
                    }

                    if (cftM.TIME_CORRECTIVE_PLAN_REVISED.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[15][4] = cftM.TIME_CORRECTIVE_PLAN_REVISED;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[15][4] = DBNull.Value;
                    }

                    if (cftM.TIME_CORRECTIVE_FINISH.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[15][5] = cftM.TIME_CORRECTIVE_FINISH;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[15][5] = DBNull.Value;
                    }

                    if (cftM.TIME_PRODUCT_PLAN_START.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[16][2] = cftM.TIME_PRODUCT_PLAN_START;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[16][2] = DBNull.Value;
                    }

                    if (cftM.TIME_PRODUCT_PLAN_END.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[16][3] = cftM.TIME_PRODUCT_PLAN_END;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[16][3] = DBNull.Value;
                    }

                    if (cftM.TIME_PRODUCT_PLAN_MODIFY.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[16][4] = cftM.TIME_PRODUCT_PLAN_MODIFY;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[16][4] = DBNull.Value;
                    }

                    if (cftM.TIME_PRODUCT_FINISH.IsNotNullOrEmpty())
                    {
                        cpMeetModel.DtTimigChart[16][5] = cftM.TIME_PRODUCT_FINISH;
                    }
                    else
                    {
                        cpMeetModel.DtTimigChart[16][5] = DBNull.Value;
                    }


                    Outer1(cpMeetModel, ref stsMsg);


                }
                else
                {
                    cpMeetModel.Customer = "";
                    cpMeetModel.AnnualRequirments = "";
                    cpMeetModel.PPAPProductionQty = "";
                    cpMeetModel.PPAPSampleQty = "300";
                    cpMeetModel.CustPartNo = "";


                    cpMeetModel.NatureOfChange = "";
                    cpMeetModel.StockPositionFG = "";
                    cpMeetModel.Wip = "";
                    cpMeetModel.Disposition = "";
                    cpMeetModel.Remarks = "";
                    if (cftM.IsNotNullOrEmpty())
                    {
                        if (cftM.RECEIVING_DATE.ToString().IsNotNullOrEmpty())
                        {
                            cpMeetModel.ReceivingDate = cftM.RECEIVING_DATE.Value.Date;
                            if (cpMeetModel.ReceivingDate.ToValueAsString().Trim() == "01/01/0001")
                                cpMeetModel.ReceivingDate = null;
                        }
                        else
                        {
                            cpMeetModel.ReceivingDate = null;
                        }
                        if (cftM.REVIEW_DATE.ToString().IsNotNullOrEmpty())
                        {
                            cpMeetModel.ReviewDate = cftM.REVIEW_DATE.Value.Date;
                            if (cpMeetModel.ReviewDate.ToValueAsString().Trim() == "01/01/0001")
                                cpMeetModel.ReviewDate = null;
                        }
                        else
                        {
                            cpMeetModel.ReviewDate = null;
                        }
                        if (cftM.CUST_REQ_DATE.ToString().IsNotNullOrEmpty())
                        {
                            cpMeetModel.CustRequiredDate = cftM.CUST_REQ_DATE;
                            if (cpMeetModel.CustRequiredDate.ToValueAsString().Trim() == "01/01/0001")
                                cpMeetModel.CustRequiredDate = null;
                        }
                        else
                        {
                            cpMeetModel.CustRequiredDate = null;
                        }

                        cpMeetModel.CFTMeetingIssueNo = cftM.CFT_MEET_ISSUE_NO.ToValueAsString();

                        if (cftM.CFT_MEET_DATE.ToString().IsNotNullOrEmpty())
                        {
                            cpMeetModel.CFTMeetingDate = Convert.ToDateTime(cftM.CFT_MEET_DATE);
                            if (cpMeetModel.CFTMeetingDate.ToValueAsString().Trim() == "01/01/0001")
                                cpMeetModel.CFTMeetingDate = null;
                        }
                        else
                        {
                            cpMeetModel.CFTMeetingDate = null;
                        }
                    }
                    else
                    {
                        cpMeetModel.ReceivingDate = null;
                        cpMeetModel.CFTMeetingDate = null;
                        cpMeetModel.CustRequiredDate = null;
                        cpMeetModel.ReviewDate = null;
                    }


                    Outer1(cpMeetModel, ref stsMsg);
                    //stsMsg = PDMsg.DoesNotExists("PartNo");
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException(); return false;
            }
        }
        //public bool Outer(EcnCftMeetingRptModel cpMeetModel, ref string stsMsg)
        //{
        //    string qry = "";
        //    try
        //    {
        //        DataTable dtProcessSheet, dtCpmMem, dtPccs = new DataTable();
        //        PROCESS_MAIN process_main = (from o in DB.PROCESS_MAIN
        //                                     where o.CURRENT_PROC == 1 && o.PART_NO == cpMeetModel.PartNo
        //                                     select o).FirstOrDefault<PROCESS_MAIN>();
        //        if (process_main.IsNotNullOrEmpty())
        //        {

        //            qry = "select b.seq_no,b.cc_code,a.opn_desc from process_sheet a,process_cc b where a.part_no = b.part_no and a.route_no ='" + process_main.ROUTE_NO.ToString() + "' and b.route_no ='" + process_main.ROUTE_NO.ToString() + "' and a.seq_no = b.seq_no and b.part_no ='" + cpMeetModel.PartNo + "' and b.seq_no in (select distinct (seq_no) from process_cc where part_no ='" + cpMeetModel.PartNo + "' ) order by b.seq_no,b.cc_sno";
        //            dtProcessSheet = Dal.GetDataTable(qry);
        //            if (dtProcessSheet.IsNotNullOrEmpty())
        //            {
        //                cpMeetModel.DtMidGrid = (dtProcessSheet.Rows.Count > 0) ? dtProcessSheet.DefaultView : null;
        //            }

        //        }
        //        else if (process_main == null)
        //        {
        //            qry = "select b.seq_no,b.cc_code,a.opn_desc from process_sheet a,process_cc b where a.part_no = b.part_no and 1=2 order by b.seq_no,b.cc_sno";
        //            dtProcessSheet = Dal.GetDataTable(qry);
        //            if (dtProcessSheet.IsNotNullOrEmpty())
        //            {
        //                cpMeetModel.DtMidGrid = (dtProcessSheet.Rows.Count > 0) ? dtProcessSheet.DefaultView : null;
        //            }
        //            stsMsg = "Current Process not specified for this Part No";
        //            return false;
        //        }

        //        ECN_MFM_MAST ddTimeMast = (from o in DB.ECN_MFM_MAST
        //                                   where o.PART_NO == cpMeetModel.PartNo
        //                                   select o).FirstOrDefault<ECN_MFM_MAST>();
        //        if (ddTimeMast.IsNotNullOrEmpty())
        //        {
        //            // cpMeetModel.DtTimigChart = dtTime.DefaultView;
        //            cpMeetModel.DtTimigChart[4][3] = (ddTimeMast.DOC_REL_DT_PLAN.IsNotNullOrEmpty()) ? ddTimeMast.DOC_REL_DT_PLAN.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[5][3] = (ddTimeMast.TIME_BOGAUGE_PLAN.IsNotNullOrEmpty()) ? ddTimeMast.TIME_BOGAUGE_PLAN.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[6][3] = (ddTimeMast.TOOLS_READY_DT_PLAN.IsNotNullOrEmpty()) ? ddTimeMast.TOOLS_READY_DT_PLAN.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[9][3] = (ddTimeMast.FORGING_PLAN_DT.IsNotNullOrEmpty()) ? ddTimeMast.FORGING_PLAN_DT.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[10][3] = (ddTimeMast.SECONDARY_PLAN_DT.IsNotNullOrEmpty()) ? ddTimeMast.SECONDARY_PLAN_DT.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[11][3] = (ddTimeMast.HEAT_TREATMENT_PLAN_DT.IsNotNullOrEmpty()) ? ddTimeMast.HEAT_TREATMENT_PLAN_DT.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[13][3] = (ddTimeMast.PPAP_PLAN.IsNotNullOrEmpty()) ? ddTimeMast.PPAP_PLAN.Value.Date.ToDateAsString() : "";

        //            cpMeetModel.DtTimigChart[4][5] = (ddTimeMast.DOC_REL_DT_ACTUAL.IsNotNullOrEmpty()) ? ddTimeMast.DOC_REL_DT_ACTUAL.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[5][5] = (ddTimeMast.TIME_BOGAUGE_ACTUAL.IsNotNullOrEmpty()) ? ddTimeMast.TIME_BOGAUGE_ACTUAL.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[6][5] = (ddTimeMast.TOOLS_READY_ACTUAL_DT.IsNotNullOrEmpty()) ? ddTimeMast.TOOLS_READY_ACTUAL_DT.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[9][5] = (ddTimeMast.FORGING_ACTUAL_DT.IsNotNullOrEmpty()) ? ddTimeMast.FORGING_ACTUAL_DT.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[10][5] = (ddTimeMast.SECONDARY_ACTUAL_DT.IsNotNullOrEmpty()) ? ddTimeMast.SECONDARY_ACTUAL_DT.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[11][5] = (ddTimeMast.HEAT_TREATMENT_ACTUAL.IsNotNullOrEmpty()) ? ddTimeMast.HEAT_TREATMENT_ACTUAL.Value.Date.ToDateAsString() : "";
        //            cpMeetModel.DtTimigChart[13][5] = (ddTimeMast.PPAP_ACTUAL_DT.IsNotNullOrEmpty()) ? ddTimeMast.PPAP_ACTUAL_DT.Value.Date.ToDateAsString() : "";

        //        }

        //        dtCpmMem = ToDataTableWithType((from O in DB.CONTROL_PLAN
        //                                        where O.PART_NO == cpMeetModel.PartNo
        //                                        select new { O.CORE_TEAM_MEMBER1, O.CORE_TEAM_MEMBER2, O.CORE_TEAM_MEMBER3, O.CORE_TEAM_MEMBER4, O.CORE_TEAM_MEMBER5, O.CORE_TEAM_MEMBER6, O.CORE_TEAM_MEMBER7 }).ToList());
        //        if (dtCpmMem.IsNotNullOrEmpty())
        //        {
        //            if (dtCpmMem.Rows.Count > 0)
        //            {
        //                cpMeetModel.Ctm1 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER1"].ToValueAsString();
        //                cpMeetModel.Ctm2 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER2"].ToValueAsString();
        //                cpMeetModel.Ctm3 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER3"].ToValueAsString();
        //                cpMeetModel.Ctm4 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER4"].ToValueAsString();
        //                cpMeetModel.Ctm5 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER5"].ToValueAsString();
        //                cpMeetModel.Ctm6 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER6"].ToValueAsString();
        //                cpMeetModel.Ctm7 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER7"].ToValueAsString();
        //            }
        //        }

        //        PRD_MAST prdMast = (from O in DB.PRD_MAST
        //                            where O.PART_NO == cpMeetModel.PartNo
        //                            select O).FirstOrDefault<PRD_MAST>();
        //        if (prdMast.IsNotNullOrEmpty())
        //        {
        //            cpMeetModel.FinishWt = prdMast.FINISH_WT.ToValueAsString();
        //            cpMeetModel.Application = prdMast.APPLICATION.ToValueAsString();
        //            cpMeetModel.PG = prdMast.PG_CATEGORY.ToValueAsString();
        //            cpMeetModel.Location = prdMast.BIF_PROJ.ToValueAsString();
        //            //dtCpmMem.Rows[0]["ALLOT_DATE"].ToValueAsString();
        //            switch (cpMeetModel.Location)
        //            {
        //                case "MM":
        //                case "MN":
        //                case "MS":
        //                    cpMeetModel.rbtnMIsChecked = true;
        //                    cpMeetModel.rbtnKIsChecked = false;
        //                    cpMeetModel.rbtnYIsChecked = false;
        //                    break;
        //                case "KK":
        //                case "KS":
        //                    cpMeetModel.rbtnMIsChecked = false;
        //                    cpMeetModel.rbtnKIsChecked = true;
        //                    cpMeetModel.rbtnYIsChecked = false;
        //                    break;
        //                case "YY":
        //                case "YS":
        //                case "YK":
        //                    cpMeetModel.rbtnMIsChecked = false;
        //                    cpMeetModel.rbtnKIsChecked = false;
        //                    cpMeetModel.rbtnYIsChecked = true;
        //                    break;
        //                default:
        //                    cpMeetModel.rbtnMIsChecked = false;
        //                    cpMeetModel.rbtnKIsChecked = false;
        //                    cpMeetModel.rbtnYIsChecked = false;
        //                    break;
        //            }

        //        }

        //        PRD_CIREF prdCiRef = (from O in DB.PRD_CIREF
        //                              where O.PART_NO == cpMeetModel.PartNo && O.CURRENT_CIREF == true
        //                              select O).FirstOrDefault<PRD_CIREF>();
        //        if (prdCiRef.IsNotNullOrEmpty())
        //        {
        //            DDCI_INFO ddCiInfo = (from o in DB.DDCI_INFO
        //                                  where o.CI_REFERENCE == prdCiRef.CI_REF
        //                                  select o).FirstOrDefault<DDCI_INFO>();
        //            if (ddCiInfo.IsNotNullOrEmpty())
        //            {
        //                cpMeetModel.AnnualRequirments = ddCiInfo.POTENTIAL.ToValueAsString();
        //                cpMeetModel.CustPartNo = ddCiInfo.CUST_DWG_NO.ToValueAsString();
        //                if (cpMeetModel.CustPartNo.Length > 0)
        //                {
        //                    cpMeetModel.CustPartNo = cpMeetModel.CustPartNo + ((ddCiInfo.CUST_DWG_NO_ISSUE.IsNotNullOrEmpty()) ? "" : "/") + ddCiInfo.CUST_DWG_NO_ISSUE.ToValueAsString();
        //                }
        //                if (cpMeetModel.CustPartNo.Length > 0)
        //                {
        //                    string cust_std_date;
        //                    cust_std_date = ddCiInfo.CUST_STD_DATE.ToDateAsString();
        //                    cpMeetModel.CustPartNo = cpMeetModel.CustPartNo + ((!cust_std_date.IsNotNullOrEmpty() || cust_std_date == "12:00:00 AM" || cust_std_date == "7:01:01 PM" || cust_std_date == "31/12/1899") ? "" : "/");
        //                    //IIf(IsNull(!cust_std_date) Or cust_std_date = "12:00:00 AM" Or cust_std_date = "7:01:01 PM" Or cust_std_date = "31/12/1899", "", "/" & Format(!cust_std_date, "DD-MM-YY"))
        //                    if (ddCiInfo.CUST_STD_DATE.ToString().IsNotNullOrEmpty())
        //                    {
        //                        cpMeetModel.CustPartNo = cpMeetModel.CustPartNo;
        //                        //+ string.Format("DD-MM-YYYY", ddCiInfo.CUST_STD_DATE);
        //                    }
        //                    cpMeetModel.CustPartName = ddCiInfo.PROD_DESC.ToValueAsString();
        //                }

        //            }
        //            if (ddCiInfo.IsNotNullOrEmpty())
        //            {
        //                DDCUST_MAST ddCustName = (from o in DB.DDCUST_MAST
        //                                          where o.CUST_CODE == ddCiInfo.CUST_CODE.ToValueAsString().ToDecimalValue()
        //                                          select o).FirstOrDefault<DDCUST_MAST>();
        //                if (ddCustName.IsNotNullOrEmpty())
        //                    cpMeetModel.Customer = (ddCustName.IsNotNullOrEmpty()) ? ddCustName.CUST_NAME.ToValueAsString() : "";
        //            }
        //        }
        //        //PCCS
        //        List<string> specChar = new List<string>() { "CRITICAL", "MAJOR", "KEY" };

        //        dtPccs = ToDataTableWithType((from o in DB.PCCS
        //                                      where o.PART_NO == cpMeetModel.PartNo
        //                                      && o.ROUTE_NO == process_main.ROUTE_NO.ToString().ToDecimalValue()
        //                                      && specChar.Contains(o.SPEC_CHAR)
        //                                      select new { o.SPEC_MIN, o.SPEC_MAX, o.FEATURE, o.PART_NO }).ToList());
        //        if (dtPccs.IsNotNullOrEmpty())
        //        {
        //            if (dtPccs.Rows.Count > 0)
        //            {
        //                if (dtPccs.Rows.Count > 13)
        //                {
        //                    stsMsg = "Special Characteristic OverFlows upto " + dtPccs.Rows.Count + ". Only First 8 Features will be listed";
        //                    //                            return false;
        //                }
        //                if (!cpMeetModel.DtSpecialCharacteristics.IsNotNullOrEmpty())
        //                {
        //                    DataTable dtTemp = new DataTable();
        //                    if ((!dtTemp.Columns.Contains("FEATURE")) && (!dtTemp.Columns.Contains("SPEC_MINMAX")))
        //                    {
        //                        dtTemp.Columns.Add("FEATURE");
        //                        dtTemp.Columns.Add("SPEC_MINMAX");
        //                    }
        //                    cpMeetModel.DtSpecialCharacteristics = dtTemp.DefaultView;
        //                }

        //                string strTemp = "";
        //                for (int i = 0; i < dtPccs.Rows.Count; i++)
        //                {
        //                    if (dtPccs.Rows.Count < 9)
        //                    {
        //                        DataRowView dr = cpMeetModel.DtSpecialCharacteristics.AddNew();
        //                        dr["FEATURE"] = dtPccs.Rows[i]["FEATURE"];
        //                        strTemp = dtPccs.Rows[i]["SPEC_MIN"].ToString();
        //                        strTemp = ((dtPccs.Rows[i]["SPEC_MAX"].IsNotNullOrEmpty()) ? (strTemp + "/" + dtPccs.Rows[i]["SPEC_MAX"].ToString()) : strTemp);
        //                        dr["SPEC_MINMAX"] = (strTemp.IsNotNullOrEmpty()) ? strTemp : "";
        //                    }

        //                }

        //                PROCESS_MAIN ddProcessMain = (from o in DB.PROCESS_MAIN
        //                                              where o.PART_NO == cpMeetModel.PartNo
        //                                              select o).FirstOrDefault<PROCESS_MAIN>();
        //                if (ddProcessMain.IsNotNullOrEmpty())
        //                {
        //                    cpMeetModel.CheesWt = ddProcessMain.CHEESE_WT.ToString();

        //                    DDRM_MAST dtRm = (from o in DB.DDRM_MAST
        //                                      where o.RM_CODE == ddProcessMain.RM_CD.ToString()
        //                                      select o).FirstOrDefault<DDRM_MAST>();
        //                    if (dtRm.IsNotNullOrEmpty())
        //                        cpMeetModel.RMSpec = dtRm.RM_DESC;

        //                    dtRm = (from o in DB.DDRM_MAST
        //                            where o.RM_CODE == ddProcessMain.ALT_RM_CD.ToString()
        //                            select o).FirstOrDefault<DDRM_MAST>();
        //                    if (dtRm.IsNotNullOrEmpty()) cpMeetModel.RMSpec = cpMeetModel.RMSpec + dtRm.RM_DESC;

        //                    dtRm = (from o in DB.DDRM_MAST
        //                            where o.RM_CODE == ddProcessMain.ALT_RM_CD1.ToString()
        //                            select o).FirstOrDefault<DDRM_MAST>();
        //                    if (dtRm.IsNotNullOrEmpty()) cpMeetModel.RMSpec = cpMeetModel.RMSpec + dtRm.RM_DESC;

        //                }

        //            }
        //            else
        //            {
        //                if (cpMeetModel.DtSpecialCharacteristics == null)
        //                {
        //                    DataTable dtTemp = new DataTable();
        //                    if ((!dtTemp.Columns.Contains("FEATURE")) && (!dtTemp.Columns.Contains("SPEC_MINMAX")))
        //                    {
        //                        dtTemp.Columns.Add("FEATURE");
        //                        dtTemp.Columns.Add("SPEC_MINMAX");
        //                    }
        //                    cpMeetModel.DtSpecialCharacteristics = dtTemp.DefaultView;
        //                }

        //            }
        //        }
        //        else
        //        {
        //            if ((!cpMeetModel.DtSpecialCharacteristics.Table.Columns.Contains("FEATURE")) && (cpMeetModel.DtSpecialCharacteristics.Table.Columns.Contains("SPEC_MINMAX")))
        //            {
        //                cpMeetModel.DtSpecialCharacteristics.Table.Columns.Add("FEATURE");
        //                cpMeetModel.DtSpecialCharacteristics.Table.Columns.Add("SPEC_MINMAX");
        //            }


        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.LogException(); return false;
        //    }
        //}
        //public bool GetRecordsByPartNo1(EcnCftMeetingRptModel cpMeetModel, ref string stsMsg)
        //{

        //    try
        //    {
        //        decimal? maxddPcn = null;
        //        try
        //        {
        //            maxddPcn = ((from c in DB.DD_PCN
        //                         where c.REQUSTED_BY == "CUSTOMER" && c.PART_NO == cpMeetModel.PartNo
        //                         select c.SFL_DRAW_ISSUENO1).Max());

        //        }
        //        catch (Exception ex)
        //        {
        //            ex.LogException();
        //            maxddPcn = null;
        //        }
        //        if (maxddPcn.IsNotNullOrEmpty())
        //        {
        //            cpMeetModel.CFTMeetingIssueNo = maxddPcn.ToValueAsString();
        //        }
        //        else
        //        {
        //            stsMsg = "No issue No. for MP Sheet";
        //        }

        //        DataTable dt = new DataTable();
        //        DateTime? dtMeet;
        //        dt = ToDataTableWithType((from o in DB.ECN_CFT_MEET_MASTER
        //                                  where o.PART_NO == cpMeetModel.PartNo
        //                                  select new { o.CFT_MEET_DATE }).ToList());
        //        if (dt.IsNotNullOrEmpty())
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                dtMeet = Convert.ToDateTime(dt.Rows[0]["CFT_MEET_DATE"]);
        //                dt = ToDataTableWithType((from o in DB.ECN_CFT_MEET_MASTER
        //                                          where o.PART_NO == cpMeetModel.PartNo && o.CFT_MEET_DATE == dtMeet
        //                                          select o).ToList());
        //                if (dt.IsNotNullOrEmpty())
        //                {
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        cpMeetModel.Customer = dt.Rows[0]["CUSTOMER"].ToValueAsString();
        //                        cpMeetModel.AnnualRequirments = dt.Rows[0]["ANNUAL_REQURIE"].ToValueAsString();
        //                        cpMeetModel.PPAPProductionQty = dt.Rows[0]["ORD_QTY"].ToValueAsString();
        //                        cpMeetModel.PPAPSampleQty = dt.Rows[0]["PPAP_QTY"].ToValueAsString();
        //                        cpMeetModel.CustPartNo = dt.Rows[0]["CUST_PART_NO"].ToValueAsString();

        //                        cpMeetModel.CustPartNo = dt.Rows[0]["NATURE_OF_CHANGE"].ToValueAsString();
        //                        cpMeetModel.CustPartNo = dt.Rows[0]["FG"].ToValueAsString();
        //                        cpMeetModel.CustPartNo = dt.Rows[0]["WIP"].ToValueAsString();
        //                        cpMeetModel.CustPartNo = dt.Rows[0]["DISPOSITION"].ToValueAsString();
        //                        if (dt.Rows[0]["RECEIVING_DATE"].ToString().IsNotNullOrEmpty())
        //                        {
        //                            cpMeetModel.ReceivingDate = Convert.ToDateTime(dt.Rows[0]["RECEIVING_DATE"]);
        //                            if (cpMeetModel.ReceivingDate.ToValueAsString().Trim() == "01/01/0001")
        //                                cpMeetModel.ReceivingDate = null;
        //                        }
        //                        if (dt.Rows[0]["REVIEW_DATE"].ToString().IsNotNullOrEmpty())
        //                        {
        //                            cpMeetModel.ReviewDate = Convert.ToDateTime(dt.Rows[0]["REVIEW_DATE"]);
        //                            if (cpMeetModel.ReviewDate.ToValueAsString().Trim() == "01/01/0001")
        //                                cpMeetModel.ReviewDate = null;
        //                        }
        //                        switch (dt.Rows[0]["COST_IMPACT"].ToValueAsString().ToUpper())
        //                        {
        //                            case "Y":
        //                            case "YES":
        //                                cpMeetModel.CostimpactYes = true;
        //                                cpMeetModel.CostimpactNo = false;
        //                                break;
        //                            case "N":
        //                            case "NO":
        //                                cpMeetModel.CostimpactYes = false;
        //                                cpMeetModel.CostimpactNo = true;
        //                                break;
        //                            default:
        //                                cpMeetModel.CostimpactYes = false;
        //                                cpMeetModel.CostimpactNo = false;
        //                                break;
        //                        }
        //                        switch (dt.Rows[0]["LOCATION"].ToValueAsString().ToUpper())
        //                        {
        //                            case "M":
        //                                cpMeetModel.rbtnMIsChecked = true;
        //                                cpMeetModel.rbtnKIsChecked = false;
        //                                cpMeetModel.rbtnYIsChecked = false;
        //                                break;
        //                            case "K":
        //                                cpMeetModel.rbtnMIsChecked = false;
        //                                cpMeetModel.rbtnKIsChecked = true;
        //                                cpMeetModel.rbtnYIsChecked = false;
        //                                break;
        //                            case "Y":
        //                                cpMeetModel.rbtnMIsChecked = false;
        //                                cpMeetModel.rbtnKIsChecked = false;
        //                                cpMeetModel.rbtnYIsChecked = true;
        //                                break;
        //                            default:
        //                                cpMeetModel.rbtnMIsChecked = false;
        //                                cpMeetModel.rbtnKIsChecked = false;
        //                                cpMeetModel.rbtnYIsChecked = false;
        //                                break;
        //                        }
        //                        if (dt.Rows[0]["CUST_REQ_DATE"].ToString().IsNotNullOrEmpty())
        //                        {
        //                            cpMeetModel.CustRequiredDate = Convert.ToDateTime(dt.Rows[0]["CUST_REQ_DATE"]);
        //                            if (cpMeetModel.CustRequiredDate.ToValueAsString().Trim() == "01/01/0001")
        //                                cpMeetModel.CustRequiredDate = null;
        //                        }

        //                        cpMeetModel.CFTMeetingIssueNo = dt.Rows[0]["CFT_MEET_ISSUE_NO"].ToValueAsString();

        //                        if (dt.Rows[0]["CFT_MEET_DATE"].ToString().IsNotNullOrEmpty())
        //                        {
        //                            cpMeetModel.CFTMeetingDate = Convert.ToDateTime(dt.Rows[0]["CFT_MEET_DATE"]);
        //                            if (cpMeetModel.CFTMeetingDate.ToValueAsString().Trim() == "01/01/0001")
        //                                cpMeetModel.CFTMeetingDate = null;
        //                        }
        //                        cpMeetModel.Application = dt.Rows[0]["Application"].ToValueAsString();
        //                        cpMeetModel.IsSafetyPart = dt.Rows[0]["safty_part"].ToValueAsString();
        //                        switch (cpMeetModel.IsSafetyPart)
        //                        {
        //                            case "Y":
        //                                cpMeetModel.ChkSPYesIsChecked = true;
        //                                cpMeetModel.ChkSPNoIsChecked = false;
        //                                break;
        //                            case "N":
        //                                cpMeetModel.ChkSPYesIsChecked = false;
        //                                cpMeetModel.ChkSPNoIsChecked = true;
        //                                break;

        //                            default:
        //                                cpMeetModel.ChkSPYesIsChecked = false;
        //                                cpMeetModel.ChkSPNoIsChecked = false;
        //                                break;
        //                        }
        //                        cpMeetModel.IsProtoType = dt.Rows[0]["proto_type"].ToValueAsString();
        //                        switch (cpMeetModel.IsProtoType)
        //                        {
        //                            case "Y":
        //                                cpMeetModel.ChkPTYesIsChecked = true;
        //                                cpMeetModel.ChkPTNoIsChecked = false;
        //                                break;
        //                            case "N":
        //                                cpMeetModel.ChkPTYesIsChecked = false;
        //                                cpMeetModel.ChkPTNoIsChecked = true;
        //                                break;

        //                            default:
        //                                cpMeetModel.ChkPTYesIsChecked = false;
        //                                cpMeetModel.ChkPTNoIsChecked = false;
        //                                break;
        //                        }
        //                        cpMeetModel.Remarks = dt.Rows[0]["remark"].ToValueAsString();

        //                        cpMeetModel.DtTimigChart[4][2] = dt.Rows[0]["TIME_DOC_REL_START"].ToString();
        //                        cpMeetModel.DtTimigChart[5][2] = dt.Rows[0]["TIME_BOGAUGE_START"].ToString();
        //                        cpMeetModel.DtTimigChart[6][2] = dt.Rows[0]["TIME_TOOL_MANUFAC_START"].ToString();
        //                        cpMeetModel.DtTimigChart[9][2] = dt.Rows[0]["TIME_FORG_START"].ToString();
        //                        cpMeetModel.DtTimigChart[10][2] = dt.Rows[0]["TIME_SECO_START"].ToString();
        //                        cpMeetModel.DtTimigChart[11][2] = dt.Rows[0]["TIME_HEAT_START"].ToString();
        //                        cpMeetModel.DtTimigChart[13][2] = dt.Rows[0]["TIME_PPAP_START"].ToString();

        //                        cpMeetModel.DtTimigChart[4][4] = dt.Rows[0]["TIME_DOC_REL_REVISED"].ToString();
        //                        // cpMeetModel.DtTimigChart[5][4] = dt.Rows[0]["BOGAUGE_ACTUAL"].ToString();
        //                        cpMeetModel.DtTimigChart[6][4] = dt.Rows[0]["TIME_TOOL_MANUFAC_REVISED"].ToString();
        //                        cpMeetModel.DtTimigChart[9][4] = dt.Rows[0]["TIME_FORG_REVISED"].ToString();
        //                        cpMeetModel.DtTimigChart[10][4] = dt.Rows[0]["TIME_SECO_REVISED"].ToString();
        //                        cpMeetModel.DtTimigChart[11][4] = dt.Rows[0]["TIME_HEAT_REVISED"].ToString();
        //                        cpMeetModel.DtTimigChart[13][4] = dt.Rows[0]["TIME_PPAP_REVISED"].ToString();

        //                        cpMeetModel.DtTimigChart[5][3] = dt.Rows[0]["TIME_BOGAUGE_END"].ToString();
        //                        //cpMeetModel.DtTimigChart[5][4] = dt.Rows[0]["TIME_BOGAUGE_PLAN_REVISED"].ToString();
        //                        //cpMeetModel.DtTimigChart[5][5] = dt.Rows[0]["TIME_BOGAUGE_FINISH"].ToString();


        //                        cpMeetModel.DtTimigChart[8][2] = dt.Rows[0]["TIME_RM_PLAN_START"].ToString();
        //                        cpMeetModel.DtTimigChart[8][3] = dt.Rows[0]["TIME_RM_PLAN_END"].ToString();
        //                        cpMeetModel.DtTimigChart[8][4] = dt.Rows[0]["TIME_RM_PLAN_REVISED"].ToString();
        //                        cpMeetModel.DtTimigChart[8][5] = dt.Rows[0]["TIME_RM_FINSIH"].ToString();


        //                        cpMeetModel.DtTimigChart[7][2] = dt.Rows[0]["TIME_BOTOOLS_PLAN_START"].ToString();
        //                        cpMeetModel.DtTimigChart[7][3] = dt.Rows[0]["TIME_BOTOOLS_PLAN_END"].ToString();
        //                        cpMeetModel.DtTimigChart[7][4] = dt.Rows[0]["TIME_BOTOOLS_PLAN_REVISED"].ToString();
        //                        cpMeetModel.DtTimigChart[7][5] = dt.Rows[0]["TIME_BOTOOLS_FINISH"].ToString();


        //                        cpMeetModel.DtTimigChart[15][2] = dt.Rows[0]["TIME_CORRECTIVE_PLAN_START"].ToString();
        //                        cpMeetModel.DtTimigChart[15][3] = dt.Rows[0]["TIME_CORRECTIVE_PLAN_END"].ToString();
        //                        cpMeetModel.DtTimigChart[15][4] = dt.Rows[0]["TIME_CORRECTIVE_PLAN_REVISED"].ToString();
        //                        cpMeetModel.DtTimigChart[15][5] = dt.Rows[0]["TIME_CORRECTIVE_FINISH"].ToString();


        //                        cpMeetModel.DtTimigChart[16][2] = dt.Rows[0]["TIME_PRODUCT_PLAN_START"].ToString();
        //                        cpMeetModel.DtTimigChart[16][3] = dt.Rows[0]["TIME_PRODUCT_PLAN_END"].ToString();
        //                        cpMeetModel.DtTimigChart[16][4] = dt.Rows[0]["TIME_PRODUCT_PLAN_MODIFY"].ToString();
        //                        cpMeetModel.DtTimigChart[16][5] = dt.Rows[0]["TIME_PRODUCT_FINISH"].ToString();
        //                        Outer(cpMeetModel, ref stsMsg);
        //                    }
        //                    else
        //                    {
        //                        Outer(cpMeetModel, ref stsMsg);
        //                    }
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.LogException(); return false;
        //    }
        //}
        public bool Outer1(EcnCftMeetingRptModel cpMeetModel, ref string stsMsg)
        {
            string qry = "";
            try
            {
                cpMeetModel.PackingSpec = cpMeetModel.PackingSpec.IsNotNullOrEmpty() ? cpMeetModel.PackingSpec : "Standard Carton";
                DataTable dtCurrentProcess, dtProcessMain, dtProcessSheet, dtCpmMem, dtPrdMast, dtCiRef, dtCustMast, dtCustName, dtPccs = new DataTable();
                dtCurrentProcess = ToDataTableWithType((from o in DB.PROCESS_MAIN
                                                        where o.CURRENT_PROC == 1 && o.PART_NO == cpMeetModel.PartNo
                                                        select new { o.ROUTE_NO, o.PART_NO }).ToList());
                try
                {
                    if (dtCurrentProcess.IsNotNullOrEmpty())
                    {
                        if (dtCurrentProcess.Rows.Count == 0)
                        {
                            stsMsg = "Current Process not specified for this Part No";
                            //return false;
                        }

                    }
                    DataTable dt1, dt2 = new DataTable();
                    string lastValue = "";

                    qry = "select b.seq_no,b.cc_code,a.opn_desc from process_sheet a,process_cc b where a.part_no = b.part_no and a.route_no ='" + dtCurrentProcess.Rows[0]["route_no"].ToString() + "' and b.route_no ='" + dtCurrentProcess.Rows[0]["route_no"].ToString() + "' and a.seq_no = b.seq_no and b.part_no ='" + cpMeetModel.PartNo + "' and b.seq_no in (select distinct (seq_no) from process_cc where part_no ='" + cpMeetModel.PartNo + "' ) order by b.seq_no,b.cc_sno";
                    dtProcessSheet = Dal.GetDataTable(qry);
                    if (dtProcessSheet.IsNotNullOrEmpty())
                    {
                        dt1 = dtProcessSheet.Clone();
                        dt2 = dtProcessSheet.Clone();

                        if (dtProcessSheet.Rows.Count > 0)
                        {
                            dtProcessSheet.DefaultView.ToTable(true, "seq_no");
                            dtProcessSheet.DefaultView.ToTable(true, "seq_no");
                            cpMeetModel.DtMidGrid = dtProcessSheet.DefaultView;

                            try
                            {
                                dt1 = dtProcessSheet.Copy();
                                foreach (DataRow row in dt1.Rows)
                                {
                                    if (row["seq_no"].ToValueAsString() != lastValue)
                                    {
                                        dt2.ImportRow(row);
                                    }
                                    lastValue = row["seq_no"].ToValueAsString();
                                }
                                cpMeetModel.DtMidGrid = dt2.DefaultView;
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                            }

                            //var ValuetoReturn = (from Rows in dtProcessSheet.AsEnumerable()
                            //                     select Rows["seq_no"]).Distinct().ToList();
                            //string[] obeDistinct = { "seq_no" };
                            //dtProcessSheet.DefaultView.ToTable("ProcessSheet", true, obeDistinct);
                            //cpMeetModel.DtMidGrid = dtProcessSheet.DefaultView;
                            //cpMeetModel.DtMidGrid = ToDataTableWithType((from Rows in dtProcessSheet.AsEnumerable()
                            //                         select Rows).Distinct().ToList()).DefaultView;
                        }
                        else
                        {
                            qry = "select b.seq_no,b.cc_code,a.opn_desc from process_sheet a,process_cc b where a.part_no = b.part_no and 1=2 order by b.seq_no,b.cc_sno";
                            dtProcessSheet = Dal.GetDataTable(qry);
                            if (dtProcessSheet.IsNotNullOrEmpty())
                            {
                                dtProcessSheet.DefaultView.ToTable(true, "seq_no");
                                cpMeetModel.DtMidGrid = dtProcessSheet.DefaultView;
                                try
                                {
                                    dt1 = dtProcessSheet.Copy();
                                    foreach (DataRow row in dt1.Rows)
                                    {
                                        if (row["seq_no"].ToValueAsString() != lastValue)
                                        {
                                            dt2.ImportRow(row);
                                        }
                                        lastValue = row["seq_no"].ToValueAsString();
                                    }
                                    cpMeetModel.DtMidGrid = dt2.DefaultView;
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                }
                            }

                        }
                    }
                    else
                    {
                        qry = "select b.seq_no,b.cc_code,a.opn_desc from process_sheet a,process_cc b where a.part_no = b.part_no and 1=2 order by b.seq_no,b.cc_sno";
                        dtProcessSheet = Dal.GetDataTable(qry);
                        dt1 = dtProcessSheet.Clone();
                        dt2 = dtProcessSheet.Clone();

                        if (dtProcessSheet.IsNotNullOrEmpty())
                        {
                            dtProcessSheet.DefaultView.ToTable(true, "seq_no");
                            cpMeetModel.DtMidGrid = dtProcessSheet.DefaultView;
                            try
                            {
                                dt1 = dtProcessSheet.Copy();
                                foreach (DataRow row in dt1.Rows)
                                {
                                    if (row["seq_no"].ToValueAsString() != lastValue)
                                    {
                                        dt2.ImportRow(row);
                                    }
                                    lastValue = row["seq_no"].ToValueAsString();
                                }
                                cpMeetModel.DtMidGrid = dt2.DefaultView;
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                            }
                        }
                        else
                        {
                            cpMeetModel.DtMidGrid = dtProcessSheet.DefaultView;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                try
                {

                    ECN_MFM_MAST dtTim = (from o in DB.ECN_MFM_MAST
                                          where o.PART_NO == cpMeetModel.PartNo
                                          select o).FirstOrDefault<ECN_MFM_MAST>();
                    if (dtTim.IsNotNullOrEmpty())
                    {
                        if (dtTim.DOC_REL_DT_PLAN.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[4][3] = dtTim.DOC_REL_DT_PLAN;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[4][3] = DBNull.Value;
                        }
                        if (dtTim.TIME_BOGAUGE_PLAN.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[5][3] = dtTim.TIME_BOGAUGE_PLAN;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[5][3] = DBNull.Value;
                        }
                        if (dtTim.TOOLS_READY_DT_PLAN.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[6][3] = dtTim.TOOLS_READY_DT_PLAN;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[6][3] = DBNull.Value;
                        }
                        if (dtTim.FORGING_PLAN_DT.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[9][3] = dtTim.FORGING_PLAN_DT;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[9][3] = DBNull.Value;
                        }
                        if (dtTim.SECONDARY_PLAN_DT.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[10][3] = dtTim.SECONDARY_PLAN_DT;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[10][3] = DBNull.Value;
                        }
                        if (dtTim.HEAT_TREATMENT_PLAN_DT.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[11][3] = dtTim.HEAT_TREATMENT_PLAN_DT;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[11][3] = DBNull.Value;
                        }
                        if (dtTim.PPAP_PLAN.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[13][3] = dtTim.PPAP_PLAN;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[13][3] = DBNull.Value;
                        }
                        if (dtTim.DOC_REL_DT_ACTUAL.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[4][5] = dtTim.DOC_REL_DT_ACTUAL;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[4][5] = DBNull.Value;
                        }
                        if (dtTim.TIME_BOGAUGE_ACTUAL.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[5][5] = dtTim.TIME_BOGAUGE_ACTUAL;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[5][5] = DBNull.Value;
                        }
                        if (dtTim.TOOLS_READY_ACTUAL_DT.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[6][5] = dtTim.TOOLS_READY_ACTUAL_DT;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[6][5] = DBNull.Value;
                        }
                        if (dtTim.FORGING_ACTUAL_DT.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[9][5] = dtTim.FORGING_ACTUAL_DT;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[9][5] = DBNull.Value;
                        }
                        if (dtTim.SECONDARY_ACTUAL_DT.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[10][5] = dtTim.SECONDARY_ACTUAL_DT;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[10][5] = DBNull.Value;
                        }
                        if (dtTim.HEAT_TREATMENT_ACTUAL.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[11][5] = dtTim.HEAT_TREATMENT_ACTUAL;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[11][5] = DBNull.Value;
                        }
                        if (dtTim.PPAP_ACTUAL_DT.IsNotNullOrEmpty())
                        {
                            cpMeetModel.DtTimigChart[13][5] = dtTim.PPAP_ACTUAL_DT;
                        }
                        else
                        {
                            cpMeetModel.DtTimigChart[13][5] = DBNull.Value;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                try
                {
                    dtCpmMem = ToDataTableWithType((from O in DB.CONTROL_PLAN
                                                    where O.PART_NO == cpMeetModel.PartNo && O.ROUTE_NO == dtCurrentProcess.Rows[0]["route_no"].ToString().ToDecimalValue()
                                                    select new { O.CORE_TEAM_MEMBER1, O.CORE_TEAM_MEMBER2, O.CORE_TEAM_MEMBER3, O.CORE_TEAM_MEMBER4, O.CORE_TEAM_MEMBER5, O.CORE_TEAM_MEMBER6, O.CORE_TEAM_MEMBER7 }).ToList());
                    if (dtCpmMem.IsNotNullOrEmpty())
                    {
                        if (dtCpmMem.Rows.Count > 0)
                        {
                            cpMeetModel.Ctm1 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER1"].ToValueAsString();
                            cpMeetModel.Ctm2 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER2"].ToValueAsString();
                            cpMeetModel.Ctm3 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER3"].ToValueAsString();
                            cpMeetModel.Ctm4 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER4"].ToValueAsString();
                            cpMeetModel.Ctm5 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER5"].ToValueAsString();
                            cpMeetModel.Ctm6 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER6"].ToValueAsString();
                            cpMeetModel.Ctm7 = dtCpmMem.Rows[0]["CORE_TEAM_MEMBER7"].ToValueAsString();
                        }
                        else
                        {
                            cpMeetModel.Ctm1 = "";
                            cpMeetModel.Ctm2 = "";
                            cpMeetModel.Ctm3 = "";
                            cpMeetModel.Ctm4 = "";
                            cpMeetModel.Ctm5 = "";
                            cpMeetModel.Ctm6 = "";
                            cpMeetModel.Ctm7 = "";
                        }
                    }
                    else
                    {
                        cpMeetModel.Ctm1 = "";
                        cpMeetModel.Ctm2 = "";
                        cpMeetModel.Ctm3 = "";
                        cpMeetModel.Ctm4 = "";
                        cpMeetModel.Ctm5 = "";
                        cpMeetModel.Ctm6 = "";
                        cpMeetModel.Ctm7 = "";
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                try
                {
                    dtPrdMast = ToDataTableWithType((from O in DB.PRD_MAST
                                                     where O.PART_NO == cpMeetModel.PartNo
                                                     select new { O.FINISH_WT, O.APPLICATION, O.PG_CATEGORY, O.BIF_PROJ, O.ALLOT_DATE }).ToList());
                    if (dtPrdMast.IsNotNullOrEmpty())
                    {
                        if (dtPrdMast.Rows.Count > 0)
                        {
                            cpMeetModel.FinishWt = dtPrdMast.Rows[0]["FINISH_WT"].ToValueAsString();
                            //cpMeetModel.Application = dtPrdMast.Rows[0]["APPLICATION"].ToValueAsString();
                            cpMeetModel.PG = dtPrdMast.Rows[0]["PG_CATEGORY"].ToValueAsString();
                            cpMeetModel.Location = dtPrdMast.Rows[0]["BIF_PROJ"].ToValueAsString();
                            //cpMeetModel.DtTimigChart[4][2] = dtCpmMem.Rows[0]["ALLOT_DATE"].ToValueAsString();
                            switch (cpMeetModel.Location)
                            {
                                case "MM":
                                case "MN":
                                case "MS":
                                    cpMeetModel.rbtnMIsChecked = true;
                                    cpMeetModel.rbtnKIsChecked = false;
                                    cpMeetModel.rbtnYIsChecked = false;
                                    break;
                                case "KK":
                                case "KS":
                                    cpMeetModel.rbtnMIsChecked = false;
                                    cpMeetModel.rbtnKIsChecked = true;
                                    cpMeetModel.rbtnYIsChecked = false;
                                    break;
                                case "YY":
                                case "YS":
                                case "YK":
                                    cpMeetModel.rbtnMIsChecked = false;
                                    cpMeetModel.rbtnKIsChecked = false;
                                    cpMeetModel.rbtnYIsChecked = true;
                                    break;
                                default:
                                    cpMeetModel.rbtnMIsChecked = false;
                                    cpMeetModel.rbtnKIsChecked = false;
                                    cpMeetModel.rbtnYIsChecked = false;
                                    break;
                            }
                        }
                        else
                        {
                            cpMeetModel.FinishWt = "";
                            cpMeetModel.Application = "";
                            cpMeetModel.PG = "";
                            cpMeetModel.Location = "";

                        }
                    }
                    else
                    {
                        cpMeetModel.FinishWt = "";
                        cpMeetModel.Application = "";
                        cpMeetModel.PG = "";
                        cpMeetModel.Location = "";

                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                try
                {
                    dtCiRef = ToDataTableWithType((from O in DB.PRD_CIREF
                                                   where O.PART_NO == cpMeetModel.PartNo
                                                   orderby O.CURRENT_CIREF descending
                                                   select new { O.CI_REF }).ToList());
                    if (dtCiRef.IsNotNullOrEmpty())
                    {
                        if (dtCiRef.Rows.Count > 0)
                        {
                            dtCustMast = ToDataTableWithType((from o in DB.DDCI_INFO
                                                              where o.CI_REFERENCE == dtCiRef.Rows[0]["CI_REF"].ToValueAsString()
                                                              select new { o.CUST_CODE, o.PROD_DESC, o.CUST_DWG_NO, o.CUST_DWG_NO_ISSUE, o.CUST_STD_DATE, o.POTENTIAL, o.APPLICATION, o.SAFTYPART, o.STATUS, o.CUSTOMER_NEED_DT, o.PACKING, o.NATURE_PACKING, o.PPAP_FORGING, o.PPAP_SAMPLE }).ToList());
                            if (dtCustMast.IsNotNullOrEmpty())
                            {
                                if (dtCustMast.Rows.Count > 0)
                                {
                                    cpMeetModel.AnnualRequirments = dtCustMast.Rows[0]["POTENTIAL"].ToValueAsString();
                                    cpMeetModel.CustPartNo = dtCustMast.Rows[0]["CUST_DWG_NO"].ToValueAsString();
                                    if (cpMeetModel.CustPartNo.Length > 0)
                                    {
                                        cpMeetModel.CustPartNo = cpMeetModel.CustPartNo + ((dtCustMast.Rows[0]["CUST_DWG_NO_ISSUE"].IsNotNullOrEmpty()) ? ("/" + dtCustMast.Rows[0]["CUST_DWG_NO_ISSUE"].ToValueAsString()) : "");
                                    }

                                    if (cpMeetModel.CustPartNo.Length > 0)
                                    {
                                        string cust_std_date;
                                        cust_std_date = dtCustMast.Rows[0]["CUST_STD_DATE"].ToValueAsString();
                                        cpMeetModel.CustPartNo = cpMeetModel.CustPartNo + ((!cust_std_date.IsNotNullOrEmpty() || cust_std_date == "12:00:00 AM" || cust_std_date == "7:01:01 PM" || cust_std_date == "31/12/1899") ? "" : "/" + dtCustMast.Rows[0]["CUST_STD_DATE"].ToDateAsString());
                                        //IIf(IsNull(!cust_std_date) Or cust_std_date = "12:00:00 AM" Or cust_std_date = "7:01:01 PM" Or cust_std_date = "31/12/1899", "", "/" & Format(!cust_std_date, "DD-MM-YY"))
                                        //if (dtCustMast.Rows[0]["CUST_STD_DATE"].ToString().IsNotNullOrEmpty())
                                        //{
                                        //    cpMeetModel.CustPartNo = cpMeetModel.CustPartNo + dtCustMast.Rows[0]["CUST_STD_DATE"].ToDateAsString();
                                        //}
                                        //else
                                        //{
                                        //    cpMeetModel.CustPartNo = "";
                                        //}
                                        cpMeetModel.CustPartName = dtCustMast.Rows[0]["PROD_DESC"].ToValueAsString();
                                    }

                                }
                                else
                                {
                                    cpMeetModel.AnnualRequirments = "";
                                    cpMeetModel.CustPartNo = "";
                                    cpMeetModel.CustPartName = "";

                                }
                            }
                            dtCustName = ToDataTableWithType((from o in DB.DDCUST_MAST
                                                              where o.CUST_CODE == dtCustMast.Rows[0]["CUST_CODE"].ToString().ToDecimalValue()
                                                              select new { o.CUST_NAME }).ToList());
                            if (dtCustName.IsNotNullOrEmpty())
                            {
                                cpMeetModel.Customer = (dtCustName.Rows.Count > 0) ? dtCustName.Rows[0]["CUST_NAME"].ToValueAsString() : "";
                            }
                            else
                            {
                                cpMeetModel.Customer = "";
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                //PCCS
                try
                {
                    List<string> specChar = new List<string>() { "CRITICAL", "MAJOR", "KEY" };
                    if (dtCurrentProcess.Rows.Count > 0)
                        dtPccs = ToDataTableWithType((from o in DB.PCCS
                                                      where o.PART_NO == cpMeetModel.PartNo
                                                      && o.ROUTE_NO == dtCurrentProcess.Rows[0]["route_no"].ToString().ToDecimalValue()
                                                      && specChar.Contains(o.SPEC_CHAR)
                                                      select new { o.SPEC_MIN, o.SPEC_MAX, o.FEATURE, o.PART_NO }).ToList());
                    if (dtPccs.IsNotNullOrEmpty())
                    {
                        if (dtPccs.Rows.Count > 0)
                        {
                            if (dtPccs.Rows.Count > 8)
                            {
                                stsMsg = "Special Characteristic OverFlows upto " + dtPccs.Rows.Count + ". Only First 8 Features will be listed";
                                //                            return false;
                            }
                            if (!cpMeetModel.DtSpecialCharacteristics.IsNotNullOrEmpty())
                            {
                                DataTable dtTemp = new DataTable();
                                if ((!dtTemp.Columns.Contains("FEATURE")) && (!dtTemp.Columns.Contains("SPEC_MINMAX")))
                                {
                                    dtTemp.Columns.Add("FEATURE");
                                    dtTemp.Columns.Add("SPEC_MINMAX");
                                }
                                cpMeetModel.DtSpecialCharacteristics = dtTemp.DefaultView;
                            }

                            string strTemp = "";
                            for (int i = 0; i < dtPccs.Rows.Count; i++)
                            {
                                if (dtPccs.Rows.Count < 9)
                                {
                                    DataRowView dr = cpMeetModel.DtSpecialCharacteristics.AddNew();
                                    dr["FEATURE"] = dtPccs.Rows[i]["FEATURE"];
                                    strTemp = dtPccs.Rows[i]["SPEC_MIN"].ToString();
                                    strTemp = ((dtPccs.Rows[i]["SPEC_MAX"].IsNotNullOrEmpty()) ? (strTemp + "/" + dtPccs.Rows[i]["SPEC_MAX"].ToString()) : strTemp);
                                    dr["SPEC_MINMAX"] = (strTemp.IsNotNullOrEmpty()) ? strTemp : "";
                                }
                            }
                        }
                    }  
                 

                    dtProcessMain = ToDataTableWithType((from o in DB.PROCESS_MAIN
                                                         where o.CURRENT_PROC == 1 && o.PART_NO == cpMeetModel.PartNo
                                                         select new { o.CHEESE_WT, o.RM_CD, o.ALT_RM_CD, o.ALT_RM_CD1 }).ToList());
                    if (dtProcessMain.IsNotNullOrEmpty())
                    {
                        if (dtProcessMain.Rows.Count > 0)
                        {
                            cpMeetModel.CheesWt = dtProcessMain.Rows[0]["CHEESE_WT"].ToString();

                            DDRM_MAST dtRm = (from o in DB.DDRM_MAST
                                              where o.RM_CODE == dtProcessMain.Rows[0]["RM_CD"].ToString()
                                              select o).FirstOrDefault<DDRM_MAST>();
                            if (dtRm.IsNotNullOrEmpty())
                                cpMeetModel.RMSpec = dtRm.RM_DESC;

                            //int altRMCD = ((dtProcessMain.Rows[0]["ALT_RM_CD"].IsNotNullOrEmpty()) ? Convert.ToInt32(dtProcessMain.Rows[0]["ALT_RM_CD"]) : 0);
                            //int altRMCD1 = ((dtProcessMain.Rows[0]["ALT_RM_CD1"].IsNotNullOrEmpty()) ? Convert.ToInt32(dtProcessMain.Rows[0]["ALT_RM_CD1"]) : 0);
                            
                            //if (altRMCD != 0)
                            //{
                            //    dtRm = (from o in DB.DDRM_MAST
                            //            where o.RM_CODE == dtProcessMain.Rows[0]["ALT_RM_CD"].ToString()
                            //            select o).FirstOrDefault<DDRM_MAST>();
                            //    if (dtRm.IsNotNullOrEmpty()) cpMeetModel.RMSpec = cpMeetModel.RMSpec + ((dtRm.RM_DESC.IsNotNullOrEmpty()) ? ("/" + dtRm.RM_DESC) : "");
                            //}
                            //if (altRMCD1 != 0)
                            //{
                            //    dtRm = (from o in DB.DDRM_MAST
                            //            where o.RM_CODE == dtProcessMain.Rows[0]["ALT_RM_CD1"].ToString()
                            //            select o).FirstOrDefault<DDRM_MAST>();
                            //    if (dtRm.IsNotNullOrEmpty()) cpMeetModel.RMSpec = cpMeetModel.RMSpec + ((dtRm.RM_DESC.IsNotNullOrEmpty()) ? ("/" + dtRm.RM_DESC) : "");
                            //}
                        }
                        else
                        {
                            cpMeetModel.CheesWt = "";
                        }
                    }                   

                    if (cpMeetModel.DtSpecialCharacteristics == null)
                    {
                        DataTable dtTemp = new DataTable();
                        if ((!dtTemp.Columns.Contains("FEATURE")) && (!dtTemp.Columns.Contains("SPEC_MINMAX")))
                        {
                            dtTemp.Columns.Add("FEATURE");
                            dtTemp.Columns.Add("SPEC_MINMAX");
                        }
                        cpMeetModel.DtSpecialCharacteristics = dtTemp.DefaultView;
                    }


                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException(); return false;
            }
        }
        public bool SaveCFTMeeting1(EcnCftMeetingRptModel cpMeetModel, ref string stsMsg)
        {
            DataTable dt = new DataTable();
            ECN_MFM_MAST mfmMast = (from o in DB.ECN_MFM_MAST
                                    where o.PART_NO == cpMeetModel.PartNo
                                    select o).FirstOrDefault<ECN_MFM_MAST>();
            try
            {
                if (!mfmMast.IsNotNullOrEmpty())
                {
                    mfmMast = new ECN_MFM_MAST();
                }

                if (cpMeetModel.DtTimigChart[4][3].ToString().IsNotNullOrEmpty())
                {
                    mfmMast.DOC_REL_DT_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][3]);
                }
                else
                {
                    mfmMast.DOC_REL_DT_PLAN = null;
                }

                if (mfmMast.DOC_REL_DT_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.DOC_REL_DT_PLAN = null;

                if (cpMeetModel.DtTimigChart[5][3].ToString().IsNotNullOrEmpty())
                {
                    mfmMast.TIME_BOGAUGE_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[5][3].ToString());
                }
                else
                {
                    mfmMast.TIME_BOGAUGE_PLAN = null;
                }
                if (mfmMast.TIME_BOGAUGE_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.TIME_BOGAUGE_PLAN = null;

                if (cpMeetModel.DtTimigChart[6][3].ToString().IsNotNullOrEmpty())
                {
                    mfmMast.TOOLS_READY_DT_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][3].ToString());
                }
                else
                {
                    mfmMast.TOOLS_READY_DT_PLAN = null;
                }

                if (mfmMast.TOOLS_READY_DT_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.TOOLS_READY_DT_PLAN = null;

                if (cpMeetModel.DtTimigChart[9][3].ToString().IsNotNullOrEmpty())
                {
                    mfmMast.FORGING_PLAN_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][3].ToString());
                }
                else
                {
                    mfmMast.FORGING_PLAN_DT = null;
                }

                if (mfmMast.FORGING_PLAN_DT.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.FORGING_PLAN_DT = null;

                if (cpMeetModel.DtTimigChart[10][3].ToString().IsNotNullOrEmpty())
                {
                    mfmMast.SECONDARY_PLAN_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][3].ToString());
                }
                else
                {
                    mfmMast.SECONDARY_PLAN_DT = null;
                }

                if (mfmMast.SECONDARY_PLAN_DT.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.SECONDARY_PLAN_DT = null;

                if (cpMeetModel.DtTimigChart[11][3].ToString().IsNotNullOrEmpty())
                {
                    mfmMast.HEAT_TREATMENT_PLAN_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][3].ToString());
                }
                else
                {
                    mfmMast.HEAT_TREATMENT_PLAN_DT = null;
                }

                if (mfmMast.HEAT_TREATMENT_PLAN_DT.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.HEAT_TREATMENT_PLAN_DT = null;

                if (cpMeetModel.DtTimigChart[13][3].ToString().IsNotNullOrEmpty())
                {
                    mfmMast.PPAP_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][3].ToString());
                }
                else
                {
                    mfmMast.PPAP_PLAN = null;
                }

                if (mfmMast.PPAP_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.PPAP_PLAN = null;
                mfmMast.SAMPLE_QTY = cpMeetModel.PPAPSampleQty.Trim().ToDecimalValue();
                //if (cpMeetModel.DtTimigChart[4][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.DOC_REL_DT_ACTUAL = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][5].ToString());
                //if (mfmMast.DOC_REL_DT_ACTUAL.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.DOC_REL_DT_ACTUAL = null;

                //if (cpMeetModel.DtTimigChart[5][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.TIME_BOGAUGE_ACTUAL = Convert.ToDateTime(cpMeetModel.DtTimigChart[5][5].ToString());
                //if (mfmMast.TIME_BOGAUGE_ACTUAL.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.TIME_BOGAUGE_ACTUAL = null;

                //if (cpMeetModel.DtTimigChart[6][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.TOOLS_READY_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][5].ToString());
                //if (mfmMast.TOOLS_READY_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.TOOLS_READY_ACTUAL_DT = null;

                //if (cpMeetModel.DtTimigChart[9][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.FORGING_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][5].ToString());
                //if (mfmMast.FORGING_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.FORGING_ACTUAL_DT = null;

                //if (cpMeetModel.DtTimigChart[10][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.SECONDARY_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][5].ToString());
                //if (mfmMast.SECONDARY_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.SECONDARY_ACTUAL_DT = null;

                //if (cpMeetModel.DtTimigChart[11][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.HEAT_TREATMENT_ACTUAL = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][5].ToString());
                //if (mfmMast.HEAT_TREATMENT_ACTUAL.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.HEAT_TREATMENT_ACTUAL = null;

                //if (cpMeetModel.DtTimigChart[13][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.PPAP_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][5].ToString());
                //if (mfmMast.PPAP_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.PPAP_ACTUAL_DT = null;

                if (!mfmMast.IsNotNullOrEmpty())
                {
                    DB.ECN_MFM_MAST.InsertOnSubmit(mfmMast);
                    DB.SubmitChanges();
                }
                else
                {
                    DB.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.ECN_MFM_MAST.DeleteOnSubmit(mfmMast);
            }

            ECN_CFT_MEET_MASTER cftMeet = (from o in DB.ECN_CFT_MEET_MASTER
                                           where o.PART_NO == cpMeetModel.PartNo && o.CFT_MEET_ISSUE_NO == cpMeetModel.CFTMeetingIssueNo.ToDecimalValue()
                                           select o).FirstOrDefault<ECN_CFT_MEET_MASTER>();
            try
            {

                if (!cftMeet.IsNotNullOrEmpty())
                {
                    cftMeet = new ECN_CFT_MEET_MASTER();
                }
                cftMeet.PART_NO = cpMeetModel.PartNo;
                cftMeet.CFT_MEET_DATE = cpMeetModel.CFTMeetingDate;
                cftMeet.CUSTOMER = cpMeetModel.Customer;
                cftMeet.ANNUAL_REQURIE = cpMeetModel.AnnualRequirments.ToDecimalValue();
                cftMeet.ORD_QTY = cpMeetModel.PPAPProductionQty.ToDecimalValue();
                cftMeet.PPAP_QTY = cpMeetModel.PPAPSampleQty.ToDecimalValue();
                if (cpMeetModel.rbtnMIsChecked == true)
                {
                    cftMeet.LOCATION = "M";
                }
                else if (cpMeetModel.rbtnKIsChecked)
                {
                    cftMeet.LOCATION = "K";
                }
                else if (cpMeetModel.rbtnYIsChecked)
                {
                    cftMeet.LOCATION = "Y";
                }
                else
                {
                    cftMeet.LOCATION = "";
                }
                cftMeet.CUST_PART_NO = cpMeetModel.CustPartNo;
                cftMeet.CUST_REQ_DATE = cpMeetModel.CustRequiredDate;
                cftMeet.CUST_PART_DESC = cpMeetModel.CustPartName;
                cftMeet.APPLICATION = cpMeetModel.Application;
                cftMeet.PG_CATEGORY = cpMeetModel.PG;
                cftMeet.SAFTY_PART = (cpMeetModel.ChkSPYesIsChecked == true) ? "Y" : "N";
                cftMeet.PROTO_TYPE = (cpMeetModel.ChkPTYesIsChecked == true) ? "Y" : "N";
                cftMeet.SFL_PART_DESC = cpMeetModel.PartDesc;
                cftMeet.RM_SPEC = cpMeetModel.RMSpec;
                cftMeet.PACKING = cpMeetModel.PackingSpec;
                cftMeet.CHEESE_WT = cpMeetModel.CheesWt.ToDecimalValue();
                cftMeet.FINISH_WT = cpMeetModel.FinishWt.ToDecimalValue();
                cftMeet.APPLICATION = cpMeetModel.Application;
                cftMeet.RATE = 0;
                cftMeet.REMARK = cpMeetModel.Remarks;
                cftMeet.NATURE_OF_CHANGE = cpMeetModel.NatureOfChange;
                cftMeet.FG = cpMeetModel.StockPositionFG.ToDecimalValue();
                cftMeet.WIP = cpMeetModel.Wip.ToDecimalValue();
                cftMeet.DISPOSITION = cpMeetModel.Disposition;

                if (cpMeetModel.ReceivingDate.ToString().IsNotNullOrEmpty())
                    cftMeet.RECEIVING_DATE = cpMeetModel.ReceivingDate;
                if (cftMeet.RECEIVING_DATE.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.RECEIVING_DATE = null;
                cftMeet.COST_IMPACT = (cpMeetModel.CostimpactYes == true) ? "YES" : "NO";

                if (cpMeetModel.ReviewDate.ToString().IsNotNullOrEmpty())
                    cftMeet.REVIEW_DATE = cpMeetModel.ReviewDate;
                if (cftMeet.REVIEW_DATE.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.REVIEW_DATE = null;

                if (cpMeetModel.DtTimigChart[4][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_DOC_REL_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][2].ToString());
                if (cftMeet.TIME_DOC_REL_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_DOC_REL_START = null;

                if (cpMeetModel.DtTimigChart[5][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOGAUGE_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[5][2].ToString());
                if (cftMeet.TIME_BOGAUGE_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOGAUGE_START = null;

                if (cpMeetModel.DtTimigChart[6][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_TOOL_MANUFAC_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][2].ToString());
                if (cftMeet.TIME_TOOL_MANUFAC_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_TOOL_MANUFAC_START = null;

                if (cpMeetModel.DtTimigChart[9][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_FORG_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][2].ToString());
                if (cftMeet.TIME_FORG_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_FORG_START = null;

                if (cpMeetModel.DtTimigChart[10][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_SECO_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][2].ToString());
                if (cftMeet.TIME_SECO_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_SECO_START = null;

                if (cpMeetModel.DtTimigChart[11][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_HEAT_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][2].ToString());
                if (cftMeet.TIME_HEAT_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_HEAT_START = null;

                if (cpMeetModel.DtTimigChart[13][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PPAP_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][2].ToString());
                if (cftMeet.TIME_PPAP_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PPAP_START = null;

                if (cpMeetModel.DtTimigChart[4][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_DOC_REL_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][4].ToString());
                if (cftMeet.TIME_DOC_REL_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_DOC_REL_REVISED = null;

                if (cpMeetModel.DtTimigChart[6][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_TOOL_MANUFAC_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][4].ToString());
                if (cftMeet.TIME_TOOL_MANUFAC_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_TOOL_MANUFAC_REVISED = null;

                if (cpMeetModel.DtTimigChart[9][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_FORG_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][4].ToString());
                if (cftMeet.TIME_FORG_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_FORG_REVISED = null;

                if (cpMeetModel.DtTimigChart[10][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_SECO_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][4].ToString());
                if (cftMeet.TIME_SECO_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_SECO_REVISED = null;

                if (cpMeetModel.DtTimigChart[11][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_HEAT_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][4].ToString());
                if (cftMeet.TIME_HEAT_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_HEAT_REVISED = null;

                if (cpMeetModel.DtTimigChart[13][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PPAP_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][4].ToString());
                if (cftMeet.TIME_PPAP_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PPAP_REVISED = null;

                if (cpMeetModel.DtTimigChart[8][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][2].ToString());
                if (cftMeet.TIME_RM_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[8][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][3].ToString());
                if (cftMeet.TIME_RM_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[8][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_PLAN_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][4].ToString());
                if (cftMeet.TIME_RM_PLAN_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_PLAN_REVISED = null;

                if (cpMeetModel.DtTimigChart[8][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_FINSIH = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][5].ToString());
                if (cftMeet.TIME_RM_FINSIH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_FINSIH = null;
                //
                if (cpMeetModel.DtTimigChart[8][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOGAUGE_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][5].ToString());
                if (cftMeet.TIME_BOGAUGE_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOGAUGE_START = null;

                if (cpMeetModel.DtTimigChart[8][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOGAUGE_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][5].ToString());
                if (cftMeet.TIME_BOGAUGE_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOGAUGE_END = null;

                if (cpMeetModel.DtTimigChart[7][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][2].ToString());
                if (cftMeet.TIME_BOTOOLS_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[7][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][3].ToString());
                if (cftMeet.TIME_BOTOOLS_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[7][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_PLAN_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][4].ToString());
                if (cftMeet.TIME_BOTOOLS_PLAN_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_PLAN_REVISED = null;

                if (cpMeetModel.DtTimigChart[7][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_FINISH = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][5].ToString());
                if (cftMeet.TIME_BOTOOLS_FINISH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_FINISH = null;

                if (cpMeetModel.DtTimigChart[15][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][2].ToString());
                if (cftMeet.TIME_CORRECTIVE_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[15][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][3].ToString());
                if (cftMeet.TIME_CORRECTIVE_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[15][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_PLAN_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][4].ToString());
                if (cftMeet.TIME_CORRECTIVE_PLAN_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_PLAN_REVISED = null;

                if (cpMeetModel.DtTimigChart[15][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_FINISH = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][5].ToString());
                if (cftMeet.TIME_CORRECTIVE_FINISH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_FINISH = null;

                if (cpMeetModel.DtTimigChart[16][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][2].ToString());
                if (cftMeet.TIME_PRODUCT_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[16][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][3].ToString());
                if (cftMeet.TIME_PRODUCT_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[16][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_PLAN_MODIFY = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][4].ToString());
                if (cftMeet.TIME_PRODUCT_PLAN_MODIFY.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_PLAN_MODIFY = null;

                if (cpMeetModel.DtTimigChart[16][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_FINISH = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][5].ToString());
                if (cftMeet.TIME_PRODUCT_FINISH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_FINISH = null;

                if (!cftMeet.IsNotNullOrEmpty())
                {
                    DB.ECN_CFT_MEET_MASTER.InsertOnSubmit(cftMeet);
                    DB.SubmitChanges();
                }
                else
                {
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.ECN_CFT_MEET_MASTER.DeleteOnSubmit(cftMeet);
            }

            DataTable dtCurrentProcess = new DataTable();
            dtCurrentProcess = ToDataTableWithType((from o in DB.PROCESS_MAIN
                                                    where o.CURRENT_PROC == 1 && o.PART_NO == cpMeetModel.PartNo
                                                    select new { o.ROUTE_NO }).ToList());
            CONTROL_PLAN cplan = null;
            try
            {
                if (!dtCurrentProcess.IsNotNullOrEmpty())
                {
                    if (dtCurrentProcess.Rows.Count > 0)
                    {
                        cplan = (from o in DB.CONTROL_PLAN
                                 where o.PART_NO == cpMeetModel.PartNo && o.ROUTE_NO == dtCurrentProcess.Rows[0]["route_no"].ToString().ToDecimalValue()
                                 select o).FirstOrDefault<CONTROL_PLAN>();
                    }
                    if (!cplan.IsNotNullOrEmpty())
                    {
                        cplan = new CONTROL_PLAN();
                    }
                    cplan.CORE_TEAM_MEMBER1 = cpMeetModel.Ctm1;
                    cplan.CORE_TEAM_MEMBER2 = cpMeetModel.Ctm1;
                    cplan.CORE_TEAM_MEMBER3 = cpMeetModel.Ctm1;
                    cplan.CORE_TEAM_MEMBER4 = cpMeetModel.Ctm1;
                    cplan.CORE_TEAM_MEMBER5 = cpMeetModel.Ctm1;
                    cplan.CORE_TEAM_MEMBER6 = cpMeetModel.Ctm1;
                    cplan.CORE_TEAM_MEMBER7 = cpMeetModel.Ctm1;
                    if (!cplan.IsNotNullOrEmpty())
                    {
                        DB.CONTROL_PLAN.InsertOnSubmit(cplan);
                        DB.SubmitChanges();
                    }
                    else
                    {
                        DB.SubmitChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return false;
        }
        public bool SaveCFTMeeting(EcnCftMeetingRptModel cpMeetModel, ref string stsMsg)
        {
            DataTable dt = new DataTable();
            bool isNewCft = false;
            ECN_MFM_MAST mfmMast = (from o in DB.ECN_MFM_MAST
                                    where o.PART_NO == cpMeetModel.PartNo
                                    select o).FirstOrDefault<ECN_MFM_MAST>();
            try
            {
                if (!mfmMast.IsNotNullOrEmpty())
                {
                    mfmMast = new ECN_MFM_MAST();
                    mfmMast.ROWID = Guid.NewGuid();
                    isNewCft = true;
                    stsMsg = "INS";
                }
                mfmMast.PART_NO = cpMeetModel.PartNo;
                if (cpMeetModel.DtTimigChart[4][3].ToString().IsNotNullOrEmpty())
                    mfmMast.DOC_REL_DT_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][3]);
                if (mfmMast.DOC_REL_DT_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.DOC_REL_DT_PLAN = null;

                if (cpMeetModel.DtTimigChart[5][3].ToString().IsNotNullOrEmpty())
                    mfmMast.TIME_BOGAUGE_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[5][3]);
                if (mfmMast.TIME_BOGAUGE_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.TIME_BOGAUGE_PLAN = null;

                if (cpMeetModel.DtTimigChart[6][3].ToString().IsNotNullOrEmpty())
                    mfmMast.TOOLS_READY_DT_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][3]);
                if (mfmMast.TOOLS_READY_DT_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.TOOLS_READY_DT_PLAN = null;

                if (cpMeetModel.DtTimigChart[9][3].ToString().IsNotNullOrEmpty())
                    mfmMast.FORGING_PLAN_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][3]);
                if (mfmMast.FORGING_PLAN_DT.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.FORGING_PLAN_DT = null;

                if (cpMeetModel.DtTimigChart[10][3].ToString().IsNotNullOrEmpty())
                    mfmMast.SECONDARY_PLAN_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][3]);
                if (mfmMast.SECONDARY_PLAN_DT.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.SECONDARY_PLAN_DT = null;

                if (cpMeetModel.DtTimigChart[11][3].ToString().IsNotNullOrEmpty())
                    mfmMast.HEAT_TREATMENT_PLAN_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][3]);
                if (mfmMast.HEAT_TREATMENT_PLAN_DT.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.HEAT_TREATMENT_PLAN_DT = null;

                if (cpMeetModel.DtTimigChart[13][3].ToString().IsNotNullOrEmpty())
                    mfmMast.PPAP_PLAN = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][3]);
                if (mfmMast.PPAP_PLAN.ToValueAsString().Trim() == "01/01/0001")
                    mfmMast.PPAP_PLAN = null;
                mfmMast.SAMPLE_QTY = cpMeetModel.PPAPSampleQty.Trim().ToDecimalValue();
                //if (cpMeetModel.DtTimigChart[4][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.DOC_REL_DT_ACTUAL = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][5].ToString());
                //if (mfmMast.DOC_REL_DT_ACTUAL.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.DOC_REL_DT_ACTUAL = null;

                //if (cpMeetModel.DtTimigChart[5][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.TIME_BOGAUGE_ACTUAL = Convert.ToDateTime(cpMeetModel.DtTimigChart[5][5].ToString());
                //if (mfmMast.TIME_BOGAUGE_ACTUAL.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.TIME_BOGAUGE_ACTUAL = null;

                //if (cpMeetModel.DtTimigChart[6][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.TOOLS_READY_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][5].ToString());
                //if (mfmMast.TOOLS_READY_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.TOOLS_READY_ACTUAL_DT = null;

                //if (cpMeetModel.DtTimigChart[9][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.FORGING_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][5].ToString());
                //if (mfmMast.FORGING_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.FORGING_ACTUAL_DT = null;

                //if (cpMeetModel.DtTimigChart[10][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.SECONDARY_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][5].ToString());
                //if (mfmMast.SECONDARY_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.SECONDARY_ACTUAL_DT = null;

                //if (cpMeetModel.DtTimigChart[11][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.HEAT_TREATMENT_ACTUAL = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][5].ToString());
                //if (mfmMast.HEAT_TREATMENT_ACTUAL.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.HEAT_TREATMENT_ACTUAL = null;

                //if (cpMeetModel.DtTimigChart[13][5].ToString().IsNotNullOrEmpty())
                //    mfmMast.PPAP_ACTUAL_DT = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][5].ToString());
                //if (mfmMast.PPAP_ACTUAL_DT.ToValueAsString().Trim() == "01/01/0001")
                //    mfmMast.PPAP_ACTUAL_DT = null;

                if (isNewCft)
                {
                    DB.ECN_MFM_MAST.InsertOnSubmit(mfmMast);
                    DB.SubmitChanges();
                }
                else
                {
                    DB.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.ECN_MFM_MAST.DeleteOnSubmit(mfmMast);
            }
            isNewCft = false;
            ECN_CFT_MEET_MASTER cftMeet = (from o in DB.ECN_CFT_MEET_MASTER
                                           where o.PART_NO == cpMeetModel.PartNo && o.CFT_MEET_ISSUE_NO == cpMeetModel.CFTMeetingIssueNo.ToDecimalValue()
                                           select o).FirstOrDefault<ECN_CFT_MEET_MASTER>();
            try
            {

                if (!cftMeet.IsNotNullOrEmpty())
                {
                    cftMeet = new ECN_CFT_MEET_MASTER();
                    cftMeet.CFT_MEET_ISSUE_NO = cpMeetModel.CFTMeetingIssueNo.ToIntValue();
                    isNewCft = true;
                }
                cftMeet.PART_NO = cpMeetModel.PartNo;
                cftMeet.CFT_MEET_DATE = cpMeetModel.CFTMeetingDate;
                cftMeet.CUSTOMER = cpMeetModel.Customer;
                cftMeet.ANNUAL_REQURIE = cpMeetModel.AnnualRequirments.ToDecimalValue();
                cftMeet.ORD_QTY = cpMeetModel.PPAPProductionQty.ToDecimalValue();
                cftMeet.PPAP_QTY = cpMeetModel.PPAPSampleQty.ToDecimalValue();
                if (cpMeetModel.rbtnMIsChecked == true)
                {
                    cftMeet.LOCATION = "M";
                }
                else if (cpMeetModel.rbtnKIsChecked)
                {
                    cftMeet.LOCATION = "K";
                }
                else if (cpMeetModel.rbtnYIsChecked)
                {
                    cftMeet.LOCATION = "Y";
                }
                else
                {
                    cftMeet.LOCATION = "";
                }
                cftMeet.CUST_PART_NO = cpMeetModel.CustPartNo;
                cftMeet.CUST_REQ_DATE = cpMeetModel.CustRequiredDate;
                cftMeet.CUST_PART_DESC = cpMeetModel.CustPartName;
                cftMeet.APPLICATION = cpMeetModel.Application;
                cftMeet.PG_CATEGORY = cpMeetModel.PG;
                cftMeet.SAFTY_PART = (cpMeetModel.ChkSPYesIsChecked == true) ? "Y" : "N";
                cftMeet.PROTO_TYPE = (cpMeetModel.ChkPTYesIsChecked == true) ? "Y" : "N";
                cftMeet.SFL_PART_DESC = cpMeetModel.PartDesc;
                cftMeet.RM_SPEC = cpMeetModel.RMSpec;
                cftMeet.PACKING = cpMeetModel.PackingSpec;
                cftMeet.CHEESE_WT = cpMeetModel.CheesWt.ToDecimalValue();
                cftMeet.FINISH_WT = cpMeetModel.FinishWt.ToDecimalValue();
                cftMeet.RATE = 0;
                cftMeet.REMARK = cpMeetModel.Remarks;
                cftMeet.NATURE_OF_CHANGE = cpMeetModel.NatureOfChange;
                cftMeet.FG = cpMeetModel.StockPositionFG.ToDecimalValue();
                cftMeet.WIP = cpMeetModel.Wip.ToDecimalValue();
                cftMeet.DISPOSITION = cpMeetModel.Disposition;

                if (cpMeetModel.ReceivingDate.ToString().IsNotNullOrEmpty())
                    cftMeet.RECEIVING_DATE = cpMeetModel.ReceivingDate;
                if (cftMeet.RECEIVING_DATE.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.RECEIVING_DATE = null;
                cftMeet.COST_IMPACT = (cpMeetModel.CostimpactYes == true) ? "YES" : "NO";

                if (cpMeetModel.ReviewDate.ToString().IsNotNullOrEmpty())
                    cftMeet.REVIEW_DATE = cpMeetModel.ReviewDate;
                if (cftMeet.REVIEW_DATE.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.REVIEW_DATE = null;

                if (cpMeetModel.DtTimigChart[4][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_DOC_REL_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][2]);
                if (cftMeet.TIME_DOC_REL_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_DOC_REL_START = null;

                if (cpMeetModel.DtTimigChart[5][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOGAUGE_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[5][2]);
                if (cftMeet.TIME_BOGAUGE_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOGAUGE_START = null;

                if (cpMeetModel.DtTimigChart[6][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_TOOL_MANUFAC_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][2]);
                if (cftMeet.TIME_TOOL_MANUFAC_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_TOOL_MANUFAC_START = null;

                if (cpMeetModel.DtTimigChart[9][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_FORG_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][2]);
                if (cftMeet.TIME_FORG_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_FORG_START = null;

                if (cpMeetModel.DtTimigChart[10][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_SECO_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][2]);
                if (cftMeet.TIME_SECO_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_SECO_START = null;

                if (cpMeetModel.DtTimigChart[11][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_HEAT_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][2]);
                if (cftMeet.TIME_HEAT_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_HEAT_START = null;

                if (cpMeetModel.DtTimigChart[13][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PPAP_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][2]);
                if (cftMeet.TIME_PPAP_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PPAP_START = null;

                if (cpMeetModel.DtTimigChart[4][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_DOC_REL_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[4][4]);
                if (cftMeet.TIME_DOC_REL_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_DOC_REL_REVISED = null;

                if (cpMeetModel.DtTimigChart[6][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_TOOL_MANUFAC_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[6][4]);
                if (cftMeet.TIME_TOOL_MANUFAC_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_TOOL_MANUFAC_REVISED = null;

                if (cpMeetModel.DtTimigChart[9][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_FORG_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[9][4]);
                if (cftMeet.TIME_FORG_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_FORG_REVISED = null;

                if (cpMeetModel.DtTimigChart[10][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_SECO_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[10][4]);
                if (cftMeet.TIME_SECO_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_SECO_REVISED = null;

                if (cpMeetModel.DtTimigChart[11][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_HEAT_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[11][4]);
                if (cftMeet.TIME_HEAT_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_HEAT_REVISED = null;

                if (cpMeetModel.DtTimigChart[13][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PPAP_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[13][4]);
                if (cftMeet.TIME_PPAP_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PPAP_REVISED = null;

                if (cpMeetModel.DtTimigChart[8][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][2]);
                if (cftMeet.TIME_RM_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[8][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][3]);
                if (cftMeet.TIME_RM_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[8][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_PLAN_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][4]);
                if (cftMeet.TIME_RM_PLAN_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_PLAN_REVISED = null;

                if (cpMeetModel.DtTimigChart[8][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_RM_FINSIH = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][5]);
                if (cftMeet.TIME_RM_FINSIH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_RM_FINSIH = null;
                //
                if (cpMeetModel.DtTimigChart[8][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOGAUGE_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][5]);
                if (cftMeet.TIME_BOGAUGE_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOGAUGE_START = null;

                if (cpMeetModel.DtTimigChart[8][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOGAUGE_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[8][5]);
                if (cftMeet.TIME_BOGAUGE_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOGAUGE_END = null;

                if (cpMeetModel.DtTimigChart[7][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][2]);
                if (cftMeet.TIME_BOTOOLS_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[7][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][3]);
                if (cftMeet.TIME_BOTOOLS_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[7][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_PLAN_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][4]);
                if (cftMeet.TIME_BOTOOLS_PLAN_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_PLAN_REVISED = null;

                if (cpMeetModel.DtTimigChart[7][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_BOTOOLS_FINISH = Convert.ToDateTime(cpMeetModel.DtTimigChart[7][5]);
                if (cftMeet.TIME_BOTOOLS_FINISH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_BOTOOLS_FINISH = null;

                if (cpMeetModel.DtTimigChart[15][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][2]);
                if (cftMeet.TIME_CORRECTIVE_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[15][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][3]);
                if (cftMeet.TIME_CORRECTIVE_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[15][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_PLAN_REVISED = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][4]);
                if (cftMeet.TIME_CORRECTIVE_PLAN_REVISED.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_PLAN_REVISED = null;

                if (cpMeetModel.DtTimigChart[15][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_CORRECTIVE_FINISH = Convert.ToDateTime(cpMeetModel.DtTimigChart[15][5]);
                if (cftMeet.TIME_CORRECTIVE_FINISH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_CORRECTIVE_FINISH = null;

                if (cpMeetModel.DtTimigChart[16][2].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_PLAN_START = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][2]);
                if (cftMeet.TIME_PRODUCT_PLAN_START.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_PLAN_START = null;

                if (cpMeetModel.DtTimigChart[16][3].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_PLAN_END = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][3]);
                if (cftMeet.TIME_PRODUCT_PLAN_END.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_PLAN_END = null;

                if (cpMeetModel.DtTimigChart[16][4].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_PLAN_MODIFY = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][4]);
                if (cftMeet.TIME_PRODUCT_PLAN_MODIFY.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_PLAN_MODIFY = null;

                if (cpMeetModel.DtTimigChart[16][5].ToString().IsNotNullOrEmpty())
                    cftMeet.TIME_PRODUCT_FINISH = Convert.ToDateTime(cpMeetModel.DtTimigChart[16][5]);
                if (cftMeet.TIME_PRODUCT_FINISH.ToValueAsString().Trim() == "01/01/0001")
                    cftMeet.TIME_PRODUCT_FINISH = null;

                if (isNewCft)
                {
                    DB.ECN_CFT_MEET_MASTER.InsertOnSubmit(cftMeet);
                    DB.SubmitChanges();
                }
                else
                {
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.ECN_CFT_MEET_MASTER.DeleteOnSubmit(cftMeet);
            }
            isNewCft = false;
            DataTable dtCurrentProcess = new DataTable();

            CONTROL_PLAN cplan = null;

            try
            {
                dtCurrentProcess = ToDataTableWithType((from o in DB.PROCESS_MAIN
                                                        where o.CURRENT_PROC == 1 && o.PART_NO == cpMeetModel.PartNo
                                                        select new { o.ROUTE_NO }).ToList());
                if (dtCurrentProcess.IsNotNullOrEmpty())
                {
                    if (dtCurrentProcess.Rows.Count > 0)
                    {
                        cplan = (from o in DB.CONTROL_PLAN
                                 where o.PART_NO == cpMeetModel.PartNo && o.ROUTE_NO == dtCurrentProcess.Rows[0]["route_no"].ToString().ToDecimalValue()
                                 select o).FirstOrDefault<CONTROL_PLAN>();
                    }
                    if (!cplan.IsNotNullOrEmpty())
                    {
                        isNewCft = true;
                        cplan = new CONTROL_PLAN();
                        cplan.ROWID = Guid.NewGuid();
                        if (dtCurrentProcess.Rows.Count > 0)
                            cplan.ROUTE_NO = dtCurrentProcess.Rows[0]["route_no"].ToString().ToDecimalValue();
                    }
                    cplan.PART_NO = cpMeetModel.PartNo;
                    cplan.CONTROL_PLAN_NO = "CP-PL/" + cplan.PART_NO + "/" + String.Format("{0:D2}", cplan.ROUTE_NO.ToString().ToIntValue());
                    cplan.CORE_TEAM_MEMBER1 = cpMeetModel.Ctm1;
                    cplan.CORE_TEAM_MEMBER2 = cpMeetModel.Ctm2;
                    cplan.CORE_TEAM_MEMBER3 = cpMeetModel.Ctm3;
                    cplan.CORE_TEAM_MEMBER4 = cpMeetModel.Ctm4;
                    cplan.CORE_TEAM_MEMBER5 = cpMeetModel.Ctm5;
                    cplan.CORE_TEAM_MEMBER6 = cpMeetModel.Ctm6;
                    cplan.CORE_TEAM_MEMBER7 = cpMeetModel.Ctm7;
                    if (isNewCft)
                    {
                        DB.CONTROL_PLAN.InsertOnSubmit(cplan);
                        DB.SubmitChanges();
                    }
                    else
                    {
                        DB.SubmitChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            isNewCft = false;
            DataTable dtSplProcess = new DataTable();
            try
            {
                dtSplProcess = ToDataTableWithType((from o in DB.CFT_SPECIAL_CHAR
                                                    where o.CFT_MEET_ISSUE_NO == cpMeetModel.CFTMeetingIssueNo.ToDecimalValue() && o.PART_NO == cpMeetModel.PartNo
                                                    select o).ToList());
                if (dtSplProcess.Rows.Count > 0)
                {
                    for (int i = 0; i < cpMeetModel.DtSpecialCharacteristics.Count; i++)
                    {
                        CFT_SPECIAL_CHAR splChar = new CFT_SPECIAL_CHAR();
                        try
                        {
                            splChar.PART_NO = cpMeetModel.PartNo;
                            splChar.CFT_MEET_ISSUE_NO = cpMeetModel.CFTMeetingIssueNo.ToDecimalValue();
                            splChar.SLNO = i + 1;
                            splChar.ROWID = Guid.NewGuid();
                            splChar.SPECIFICATION = cpMeetModel.DtSpecialCharacteristics[i]["SPECIFICATION"].ToString();
                            splChar.FEATURE = cpMeetModel.DtSpecialCharacteristics[i]["FEATURE"].ToString();
                            DB.CFT_SPECIAL_CHAR.InsertOnSubmit(splChar);
                            DB.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.CFT_SPECIAL_CHAR.DeleteOnSubmit(splChar);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return true;
        }
    }

}
