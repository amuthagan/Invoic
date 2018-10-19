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
    public class ControlPlanRptBll : Essential
    {
        public ControlPlanRptBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public bool GetCPRptCPM(ControlPlanRptModel cpRptModel)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.DESIGN.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER1 = o.MEMBER }).ToList());
                cpRptModel.DtCtm1 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.MKT.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER2 = o.MEMBER }).ToList());
                cpRptModel.DtCtm2 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.PRODU.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER3 = o.MEMBER }).ToList());
                cpRptModel.DtCtm3 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.QUALITY.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER4 = o.MEMBER }).ToList());
                cpRptModel.DtCtm4 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.PROCESS.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER5 = o.MEMBER }).ToList());
                cpRptModel.DtCtm5 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.OTHERS.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER6 = o.MEMBER }).ToList());
                cpRptModel.DtCtm6 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN_MEMBER
                                  where o.DEPT == Member_Dept.OTHERS.ToString()
                                  orderby o.SNO ascending
                                  select new { CORE_TEAM_MEMBER7 = o.MEMBER }).ToList());
                cpRptModel.DtCtm7 = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.DDUSER_LIST
                                  where o.USER_GROUP == "Manager"
                                  select new { KEY_CONTACT_PERSON = o.USER_FULL_NAME }).ToList());
                cpRptModel.dtKeyContactPerson = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN
                                  where o.KEY_CONTACT_FAXNO.StartsWith("+91-44") && o.KEY_CONTACT_FAXNO != null
                                  select new { o.KEY_CONTACT_FAXNO }).Distinct().ToList());
                cpRptModel.DtFax = (dt != null) ? dt.DefaultView : null;

                dt = ToDataTable((from o in DB.CONTROL_PLAN
                                  where o.KEY_CONTACT_PHONE.StartsWith("+91-44") && o.KEY_CONTACT_PHONE != null
                                  select new { o.KEY_CONTACT_PHONE }).Distinct().ToList());
                cpRptModel.DtPhone = (dt != null) ? dt.DefaultView : null;

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public bool GetCPRptLoadCurrentPartNo(ControlPlanRptModel cpRptModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (cpRptModel.PartNo.IsNotNullOrEmpty() && cpRptModel.RouteNo.IsNotNullOrEmpty())
                {


                    dt = ToDataTable((from o in DB.CONTROL_PLAN
                                      where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue()
                                      select new { o.PART_NO, o.ROUTE_NO, o.CONTROL_PLAN_NO, o.CONTROL_PLAN_TYPE, o.KEY_CONTACT_PERSON, o.KEY_CONTACT_PHONE, o.KEY_CONTACT_FAXNO, o.CORE_TEAM_MEMBER1, o.CORE_TEAM_MEMBER2, o.CORE_TEAM_MEMBER3, o.CORE_TEAM_MEMBER4, o.CORE_TEAM_MEMBER5, o.CORE_TEAM_MEMBER6, o.CORE_TEAM_MEMBER7, o.SUPP_PL_APP_DATE, o.OTHER_PL_APP_DATE }).ToList());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        cpRptModel.ControlPlanNo = dt.Rows[0]["CONTROL_PLAN_NO"].ToString();
                        cpRptModel.ControlPlanType = dt.Rows[0]["CONTROL_PLAN_TYPE"].ToString();
                        cpRptModel.KeyContactPerson = dt.Rows[0]["KEY_CONTACT_PERSON"].ToString();
                        cpRptModel.Phone = dt.Rows[0]["KEY_CONTACT_PHONE"].ToString();
                        cpRptModel.Fax = dt.Rows[0]["KEY_CONTACT_FAXNO"].ToString();
                        cpRptModel.Ctm1 = dt.Rows[0]["CORE_TEAM_MEMBER1"].ToString();
                        cpRptModel.Ctm2 = dt.Rows[0]["CORE_TEAM_MEMBER2"].ToString();
                        cpRptModel.Ctm3 = dt.Rows[0]["CORE_TEAM_MEMBER3"].ToString();
                        cpRptModel.Ctm4 = dt.Rows[0]["CORE_TEAM_MEMBER4"].ToString();
                        cpRptModel.Ctm5 = dt.Rows[0]["CORE_TEAM_MEMBER5"].ToString();
                        cpRptModel.Ctm6 = dt.Rows[0]["CORE_TEAM_MEMBER6"].ToString();
                        cpRptModel.Ctm7 = dt.Rows[0]["CORE_TEAM_MEMBER7"].ToString();
                        cpRptModel.SupplierApprDate = ExtendedMethods.ToDateTimeValue(dt.Rows[0]["SUPP_PL_APP_DATE"].ToString());
                        cpRptModel.OtherApprDate = ExtendedMethods.ToDateTimeValue(dt.Rows[0]["OTHER_PL_APP_DATE"].ToString());
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex.LogException();

            }

        }

        public bool GetCPRptLoadSeqNo(ControlPlanRptModel cpRptModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (cpRptModel.PartNo.IsNotNullOrEmpty() && cpRptModel.RouteNo.IsNotNullOrEmpty())
                {
                    dt = ToDataTable((from o in DB.PROCESS_SHEET
                                      where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue()
                                      select new { o.SEQ_NO }).ToList());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        cpRptModel.DtSeqNumber = dt.DefaultView;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex.LogException();

            }

        }
        public bool IsCPRptLoadCurrentPartNo(ControlPlanRptModel cpRptModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (cpRptModel.PartNo.IsNotNullOrEmpty() && cpRptModel.RouteNo.IsNotNullOrEmpty())
                {

                    dt = ToDataTable((from o in DB.CONTROL_PLAN
                                      where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue()
                                      select new { o.PART_NO, o.ROUTE_NO, o.CONTROL_PLAN_NO, o.CONTROL_PLAN_TYPE, o.KEY_CONTACT_PERSON, o.KEY_CONTACT_PHONE, o.KEY_CONTACT_FAXNO, o.CORE_TEAM_MEMBER1, o.CORE_TEAM_MEMBER2, o.CORE_TEAM_MEMBER3, o.CORE_TEAM_MEMBER4, o.CORE_TEAM_MEMBER5, o.CORE_TEAM_MEMBER6, o.CORE_TEAM_MEMBER7, o.SUPP_PL_APP_DATE, o.OTHER_PL_APP_DATE }).ToList());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex.LogException();

            }

        }
        public bool SaveCPRptControlPlan(ControlPlanRptModel cpRptModel, ref string typ)
        {
            try
            {

                if (cpRptModel.PartNo.IsNotNullOrEmpty() && cpRptModel.RouteNo.IsNotNullOrEmpty())
                {

                    CONTROL_PLAN controlPlan = (from o in DB.CONTROL_PLAN
                                                where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue()
                                                select o).FirstOrDefault<CONTROL_PLAN>();
                    if (!controlPlan.IsNotNullOrEmpty())
                    {
                        try
                        {
                            controlPlan = new CONTROL_PLAN();
                            controlPlan.ROWID = Guid.NewGuid();
                            controlPlan.CONTROL_PLAN_NO = cpRptModel.ControlPlanNo;
                            controlPlan.PART_NO = cpRptModel.PartNo;
                            controlPlan.ROUTE_NO = cpRptModel.RouteNo.ToDecimalValue();
                            controlPlan.CONTROL_PLAN_TYPE = cpRptModel.ControlPlanType;
                            controlPlan.KEY_CONTACT_FAXNO = cpRptModel.Fax;
                            controlPlan.KEY_CONTACT_PERSON = cpRptModel.KeyContactPerson;
                            controlPlan.KEY_CONTACT_PHONE = cpRptModel.Phone;
                            controlPlan.CORE_TEAM_MEMBER1 = cpRptModel.Ctm1;
                            controlPlan.CORE_TEAM_MEMBER2 = cpRptModel.Ctm2;
                            controlPlan.CORE_TEAM_MEMBER3 = cpRptModel.Ctm3;
                            controlPlan.CORE_TEAM_MEMBER4 = cpRptModel.Ctm4;
                            controlPlan.CORE_TEAM_MEMBER5 = cpRptModel.Ctm5;
                            controlPlan.CORE_TEAM_MEMBER6 = cpRptModel.Ctm6;
                            controlPlan.CORE_TEAM_MEMBER7 = cpRptModel.Ctm7;
                            controlPlan.SUPP_PL_APP_DATE = cpRptModel.SupplierApprDate.ToDateAsString();
                            controlPlan.OTHER_PL_APP_DATE = cpRptModel.OtherApprDate.ToDateAsString();
                            DB.CONTROL_PLAN.InsertOnSubmit(controlPlan);
                            DB.SubmitChanges();
                            typ = "INS";
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.CONTROL_PLAN.DeleteOnSubmit(controlPlan);
                            return false;
                        }
                    }
                    else if (controlPlan.IsNotNullOrEmpty())
                    {
                        try
                        {
                            controlPlan.CONTROL_PLAN_TYPE = cpRptModel.ControlPlanType;
                            controlPlan.KEY_CONTACT_FAXNO = cpRptModel.Fax;
                            controlPlan.KEY_CONTACT_PERSON = cpRptModel.KeyContactPerson;
                            controlPlan.KEY_CONTACT_PHONE = cpRptModel.Phone;
                            controlPlan.CORE_TEAM_MEMBER1 = cpRptModel.Ctm1;
                            controlPlan.CORE_TEAM_MEMBER2 = cpRptModel.Ctm2;
                            controlPlan.CORE_TEAM_MEMBER3 = cpRptModel.Ctm3;
                            controlPlan.CORE_TEAM_MEMBER4 = cpRptModel.Ctm4;
                            controlPlan.CORE_TEAM_MEMBER5 = cpRptModel.Ctm5;
                            controlPlan.CORE_TEAM_MEMBER6 = cpRptModel.Ctm6;
                            controlPlan.CORE_TEAM_MEMBER7 = cpRptModel.Ctm7;
                            controlPlan.SUPP_PL_APP_DATE = cpRptModel.SupplierApprDate.ToDateAsString();
                            controlPlan.OTHER_PL_APP_DATE = cpRptModel.OtherApprDate.ToDateAsString();
                            DB.SubmitChanges();
                            typ = "UPD";
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.CONTROL_PLAN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, controlPlan);
                            return false;
                        }
                    }

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
                return false;
                //throw ex.LogException();

            }

        }
        public bool GetCPRptIssueNoCurrentPartNo(ControlPlanRptModel cpRptModel)
        {
            try
            {
                //select issue_no from pccs_issue where  part_no='M40050' and route_no='1'  
                //and issue_date = (select max(issue_date) from pccs_issue where part_no ='M40050' and route_no='1' ) 
                //
                DataTable dt = new DataTable();
                DateTime? pccsIssueMaxDate = (from o in DB.PCCS_ISSUE
                                              where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue()
                                              select o.ISSUE_DATE).Max();
                if (pccsIssueMaxDate.IsNotNullOrEmpty())
                {
                    PCCS_ISSUE pccsIssue = (from o in DB.PCCS_ISSUE
                                            where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue() && o.ISSUE_DATE == pccsIssueMaxDate.Value.Date
                                            select o).FirstOrDefault<PCCS_ISSUE>();
                    if (pccsIssue.IsNotNullOrEmpty())
                    {
                        if (cpRptModel.ControlPlanType == "PRE LAUNCH")
                        {
                            cpRptModel.ControlPlanNo = "CP-PL/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssue.ISSUE_NO.ToIntValue());
                        }
                        else if (cpRptModel.ControlPlanType == "PRODUCTION")
                        {
                            cpRptModel.ControlPlanNo = "CP-PN/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssue.ISSUE_NO.ToIntValue());
                        }
                        else if (cpRptModel.ControlPlanType == "PROTOTYPE")
                        {

                        }
                    }
                    return true;
                }
                else
                {
                    try
                    {

                        PCCS_ISSUE pccsIssueMaxNo = (from o in DB.PCCS_ISSUE
                                                     where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue()
                                                     select o).LastOrDefault<PCCS_ISSUE>();
                        if (pccsIssueMaxNo.IsNotNullOrEmpty())
                        {

                            if (cpRptModel.ControlPlanType == "PRE LAUNCH")
                            {
                                cpRptModel.ControlPlanNo = "CP-PL/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssueMaxNo.ISSUE_NO.ToIntValue());
                            }
                            else if (cpRptModel.ControlPlanType == "PRODUCTION")
                            {
                                cpRptModel.ControlPlanNo = "CP-PN/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssueMaxNo.ISSUE_NO.ToIntValue());
                            }
                            else if (cpRptModel.ControlPlanType == "PROTOTYPE")
                            {

                            }

                        }
                        else
                        {
                            if (cpRptModel.ControlPlanType == "PRE LAUNCH")
                            {
                                cpRptModel.ControlPlanNo = "CP-PL/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssueMaxNo.ISSUE_NO.ToIntValue());
                            }
                            else if (cpRptModel.ControlPlanType == "PRODUCTION")
                            {
                                cpRptModel.ControlPlanNo = "CP-PN/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssueMaxNo.ISSUE_NO.ToIntValue());
                            }
                            else if (cpRptModel.ControlPlanType == "PROTOTYPE")
                            {

                            }
                        }

                    }
                    catch (Exception)
                    {
                        if (cpRptModel.ControlPlanType == "PRE LAUNCH")
                        {
                            cpRptModel.ControlPlanNo = "CP-PL/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", 0);
                        }
                        else if (cpRptModel.ControlPlanType == "PRODUCTION")
                        {
                            cpRptModel.ControlPlanNo = "CP-PN/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", 0);
                        }
                        else if (cpRptModel.ControlPlanType == "PROTOTYPE")
                        {

                        }
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        private string custdwgNo = "";
        private string supplierCode = "";
        private void RetreieveCustomerName(ControlPlanRptModel cpRptModel)
        {
            try
            {
                string sql = "";
                sql = " select c.cust_code,cust_name,cust_dwg_no,prod_desc,cust_dwg_no_issue,cust_std_date from ddci_info i, ddcust_mast c where i.cust_code=c.cust_code and  ci_reference in (select ci_ref from prd_ciref where part_no='" + cpRptModel.PartNo + "' and current_ciref = 1)";
                DataTable dt = ToDataTableWithType(DB.ExecuteQuery<CustomerDetailsModel>(sql).ToList());
                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0]["cust_std_date"].IsNotNullOrEmpty())
                    //{
                    //    if (!dt.Rows[0]["cust_dwg_no_issue"].IsNotNullOrEmpty() && (dt.Rows[0]["cust_std_date"].ToValueAsString() == "" || !dt.Rows[0]["cust_std_date"].IsNotNullOrEmpty() || dt.Rows[0]["cust_std_date"].ToString() == "12:00:00 AM" || dt.Rows[0]["cust_std_date"].ToString() == "7:01:01 PM" || Convert.ToDateTime(dt.Rows[0]["cust_std_date"]).ToString("dd/MM/yyyy") == "31/12/1899"))
                    //    {
                    //        custdwgNo = (!dt.Rows[0]["cust_dwg_no"].IsNotNullOrEmpty()) ? "" : dt.Rows[0]["cust_dwg_no"].ToValueAsString();
                    //    }
                    //    else if ((!dt.Rows[0]["cust_dwg_no_issue"].IsNotNullOrEmpty()) || dt.Rows[0]["cust_std_date"].ToValueAsString() == "12:00:00 AM" || dt.Rows[0]["cust_std_date"].ToString() == "7:01:01 PM" || Convert.ToDateTime(dt.Rows[0]["cust_std_date"]).ToString("dd/MM/yyyy") == "31/12/1899" && dt.Rows[0]["cust_dwg_no_issue"] != "")
                    //    {
                    //        custdwgNo = dt.Rows[0]["cust_dwg_no"] + "/" + dt.Rows[0]["cust_dwg_no_issue"];
                    //    }
                    //    else if ((!dt.Rows[0]["cust_dwg_no_issue"].IsNotNullOrEmpty()) || dt.Rows[0]["cust_std_date"].ToValueAsString() == "12:00:00 AM" || dt.Rows[0]["cust_std_date"].ToString() == "7:01:01 PM" || Convert.ToDateTime(dt.Rows[0]["cust_std_date"]).ToString("dd/MM/yyyy") == "31/12/1899" && !dt.Rows[0]["cust_dwg_no_issue"].IsNotNullOrEmpty())
                    //    {
                    //        custdwgNo = (!dt.Rows[0]["cust_dwg_no"].IsNotNullOrEmpty()) ? "" : dt.Rows[0]["cust_dwg_no"].ToValueAsString();
                    //    }
                    //    else
                    //    {
                    //        custdwgNo = dt.Rows[0]["cust_dwg_no"] + " / " + dt.Rows[0]["cust_dwg_no_issue"] + ((dt.Rows[0]["cust_std_date"].IsNotNullOrEmpty()) ? " / " + Convert.ToDateTime(dt.Rows[0]["cust_std_date"]).ToString("dd/MM/yyyy") : "");
                    //    }
                    //}
                    //else
                    //{
                    //    custdwgNo = dt.Rows[0]["cust_dwg_no"]  + "/" + dt.Rows[0]["cust_dwg_no_issue"];
                    //}

                    custdwgNo = dt.Rows[0]["cust_dwg_no"] + ((dt.Rows[0]["cust_dwg_no_issue"].IsNotNullOrEmpty()) ? " / " + dt.Rows[0]["cust_dwg_no_issue"] : "") + ((dt.Rows[0]["cust_std_date"].IsNotNullOrEmpty()) ? " / " + Convert.ToDateTime(dt.Rows[0]["cust_std_date"]).ToString("dd/MM/yyyy") : "");
                    
                    sql = "select cust_ref_for_sfl from cust_mast where cust_cd = '" + dt.Rows[0]["cust_code"] + "'";
                    DataTable dtcust = ToDataTableWithType(DB.ExecuteQuery<CustomerDetailsModel>(sql).ToList());
                    if (dtcust.Rows.Count > 0)
                    {
                        supplierCode = (!dtcust.Rows[0]["CUST_REF_FOR_SFL"].IsNotNullOrEmpty()) ? "" : dtcust.Rows[0]["CUST_REF_FOR_SFL"].ToValueAsString();

                    }
                    else
                    {
                        supplierCode = "    ";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        public DataView PrintCPRpt(ControlPlanRptModel cpRptModel, string seqNos)
        {
            DataView dtv = new DataView();
            string sqlLoca, sqlCustQry, sqlOrgDateQry, sqlRevDateQry = "";
            string supplierLoc = "";
            try
            {
                StringBuilder sbsql = new StringBuilder();
                sqlLoca = " select bif_proj from prd_mast where part_no= '" + cpRptModel.PartNo + "'";
                // sqlCustQry = "select CUST_DWG_NO from  ddci_info where  ci_reference in (select ci_ref from prd_ciref where part_no='" + cpRptModel.PartNo + "' AND CURRENT_CIREF = 1 )";
                sqlOrgDateQry = "select issue_date from pccs_issue  where (ISSUE_NO='1' or ISSUE_NO='01' or ISSUE_NO='001') AND part_no =  '" + cpRptModel.PartNo + "' and route_no in (select route_no from process_main where part_no = '" + cpRptModel.PartNo + "' and current_proc = 1)";
                //  "select min(ISSUE_DATE) as ISSUE_DATE from PCCS_ISSUE where PART_NO='" + cpRptModel.PartNo + "'";

                ///Location
                try
                {


                    supplierLoc = DB.ExecuteQuery<string>(sqlLoca.ToString()).FirstOrDefault<string>().ToValueAsString();
                    if (supplierLoc.ToUpper().Trim() == "MM" || supplierLoc.ToUpper().Trim() == "MN" || supplierLoc.ToUpper().Trim() == "MS")
                    {
                        supplierLoc = "Padi";
                    }
                    else if (supplierLoc.ToUpper().Trim() == "KK" || supplierLoc.ToUpper().Trim() == "KS")
                    {
                        supplierLoc = "Krishnapuram";
                    }
                    else
                    {
                        supplierLoc = "Pondy";
                    }
                    ///End
                    //IEnumerable<string> sqlCusts = DB.ExecuteQuery<string>(sqlCustQry).ToList();
                    //if (sqlCusts != null)
                    //{
                    //    sqlCustQry = sqlCusts.FirstOrDefault();
                    //}
                    //else
                    //{
                    //    sqlCustQry = "";
                    //}

                    //List<PCCS_ISSUE> lstOrgDate = (from row in DB.PCCS_ISSUE
                    //                               where row.PART_NO == cpRptModel.PartNo
                    //                               orderby row.ISSUE_DATE
                    //                               select row).ToList<PCCS_ISSUE>();

                    /// OrgDate -Revised Date
                    DateTime? sqlOrgDate = null;
                    DateTime? sqlRevDate = null;
                    //if (lstOrgDate.IsNotNullOrEmpty() && lstOrgDate.Count > 0)
                    //{
                    //    sqlOrgDate = lstOrgDate[0].ISSUE_DATE;
                    //    sqlRevDate = lstOrgDate[lstOrgDate.Count - 1].ISSUE_DATE;
                    //}
                    sqlOrgDate = DB.ExecuteQuery<DateTime>(sqlOrgDateQry.ToString()).FirstOrDefault<DateTime>();
                    if (sqlOrgDate != null && Convert.ToDateTime(sqlOrgDate).ToString("dd/MM/yyyy") != "01/01/0001")
                    {

                        if (sqlOrgDate.IsNotNullOrEmpty())
                            sqlOrgDateQry = Convert.ToDateTime(sqlOrgDate).ToString("dd/MM/yyyy");
                        else
                            sqlOrgDateQry = "--";
                    }
                    else
                    {
                        sqlOrgDateQry = "--";
                    }
                    //IEnumerable<Nullable<DateTime>> sqlRevDate = DB.ExecuteQuery<Nullable<DateTime>>(sqlRevDateQry);

                    sqlRevDateQry = "select max(ISSUE_DATE) as ISSUE_DATE from PCCS_ISSUE where PART_NO='" + cpRptModel.PartNo + "' and route_no='" + cpRptModel.RouteNo + "'";
                    sqlRevDate = DB.ExecuteQuery<DateTime>(sqlRevDateQry.ToString()).FirstOrDefault<DateTime>();
                    sqlRevDateQry = "select * from pccs_issue where  part_no='" + cpRptModel.PartNo + "' and route_no='" + cpRptModel.RouteNo + "'";
                    DataTable dtrev = ToDataTableWithType(DB.ExecuteQuery<PCCS_ISSUE>(sqlRevDateQry).ToList());
                    if (sqlRevDate != null && Convert.ToDateTime(sqlRevDate).ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        if (sqlRevDate.IsNotNullOrEmpty())
                            dtrev.DefaultView.RowFilter = "issue_date='" + sqlRevDate.ToDateAsString() + "'";
                        if (dtrev.Rows.Count > 0 && dtrev.DefaultView[0]["issue_date"].IsNotNullOrEmpty())
                        {
                            if (dtrev.DefaultView[0]["issue_no"].ToValueAsString() == "1")
                            {
                                sqlRevDateQry = "--";
                            }
                            else
                            {
                                sqlRevDateQry = Convert.ToDateTime(dtrev.DefaultView[0]["issue_date"]).ToString("dd/MM/yyyy");
                            }
                        }
                        else
                        {
                            sqlRevDateQry = "--";
                        }
                        dtrev.DefaultView.RowFilter = string.Empty;                   
                    }
                    else
                    {
                        sqlRevDateQry = "--";
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                /// End OrgDate -Revised Date
                /// 
                RetreieveCustomerName(cpRptModel);
                string suppApprDate = "____";
                string suppOtherDate = "____";
                if (cpRptModel.SupplierApprDate != null)
                {
                    if (cpRptModel.SupplierApprDate.IsNotNullOrEmpty())
                        suppApprDate = Convert.ToDateTime(cpRptModel.SupplierApprDate).ToString("dd/MM/yyyy");
                    else
                    {
                        suppApprDate = "____";
                    }
                }
                if (cpRptModel.OtherApprDate != null)
                {
                    if (cpRptModel.OtherApprDate.IsNotNullOrEmpty())
                        suppOtherDate = Convert.ToDateTime(cpRptModel.OtherApprDate).ToString("dd/MM/yyyy");
                    else
                    {
                        suppOtherDate = "____";
                    }
                }
                string partNum = cpRptModel.PartNo;
                try
                {
                    DataTable dt = new DataTable();
                    decimal? dwgIssueMaxNo = 0;
                    try
                    {
                        dwgIssueMaxNo = (from o in DB.PRD_DWG_ISSUE
                                         where o.PART_NO == cpRptModel.PartNo && o.DWG_TYPE == 0
                                         select o.ISSUE_NO).Max();
                    }
                    catch (Exception ex)
                    {

                    }

                    if (dwgIssueMaxNo.IsNotNullOrEmpty() && dwgIssueMaxNo != 0)
                    {
                        // Check DWG_TYPE as 0 to get date from product drawing (if it is 1 then it is sequence drawing)
                        PRD_DWG_ISSUE dwgIssue = (from o in DB.PRD_DWG_ISSUE
                                                  where o.PART_NO == cpRptModel.PartNo && o.ISSUE_NO == dwgIssueMaxNo && o.DWG_TYPE == 0
                                                  select o).FirstOrDefault<PRD_DWG_ISSUE>();
                        if (dwgIssue.IsNotNullOrEmpty())
                        {
                            if (dwgIssue.ISSUE_NO.IsNotNullOrEmpty())
                                partNum = partNum + "/ " + dwgIssue.ISSUE_NO;
                            if (dwgIssue.ISSUE_DATE.IsNotNullOrEmpty())
                            {
                                partNum = partNum + "/ " + Convert.ToDateTime(dwgIssue.ISSUE_DATE).ToString("dd/MM/yyyy");
                            }
                        }
                        else
                        {
                            partNum = cpRptModel.PartNo + "/ --";
                        }
                    }
                }
                catch (Exception ex)
                {
                    partNum = cpRptModel.PartNo + "/ --";
                    ex.LogException();
                }

                //new by me

                DateTime? pccsIssueMaxDate = (from o in DB.PCCS_ISSUE
                                              where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue()
                                              select o.ISSUE_DATE).Max();
                if (pccsIssueMaxDate.IsNotNullOrEmpty())
                {
                    PCCS_ISSUE pccsIssue = (from o in DB.PCCS_ISSUE
                                            where o.PART_NO == cpRptModel.PartNo && o.ROUTE_NO == cpRptModel.RouteNo.ToDecimalValue() && o.ISSUE_DATE == pccsIssueMaxDate.Value.Date
                                            select o).FirstOrDefault<PCCS_ISSUE>();
                    if (pccsIssue.IsNotNullOrEmpty())
                    {
                        if (cpRptModel.ControlPlanType == "PRE LAUNCH")
                        {
                            cpRptModel.ControlPlanNo = "CP-PL/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssue.ISSUE_NO.ToIntValue());
                        }
                        else if (cpRptModel.ControlPlanType == "PRODUCTION")
                        {
                            cpRptModel.ControlPlanNo = "CP-PN/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssue.ISSUE_NO.ToIntValue());
                        }
                        else if (cpRptModel.ControlPlanType == "PROTOTYPE")
                        {
                            cpRptModel.ControlPlanNo = "CP-PT/" + cpRptModel.PartNo + "/ " + String.Format("{0,2:00}", pccsIssue.ISSUE_NO.ToIntValue());
                        }
                    }

                }
                
                //end new

                sbsql.Append("select '" + cpRptModel.PartNo + "' as PartNo,  '" + cpRptModel.RouteNo + "' as ProcessNo,  '" + cpRptModel.ControlPlanType + "' as ControlPlanType, '" + cpRptModel.ControlPlanNo + "' as ControlPlanNo, '" + cpRptModel.Fax + "' as Fax, '" + cpRptModel.Phone + "' as Phone,  '" + cpRptModel.Ctm1 + "' as Ctm1,  '" + cpRptModel.Ctm2 + "' as Ctm2,");
                sbsql.Append(" '" + cpRptModel.Ctm3 + "' as Ctm3, '" + cpRptModel.Ctm4 + "' as Ctm4, '" + cpRptModel.Ctm5 + "' as Ctm5, '" + cpRptModel.Ctm6 + "' as Ctm6,  '" + cpRptModel.Ctm7 + "' as Ctm7, '" + partNum + "' as SupplierPartNo, '" + custdwgNo + "' as CustomerPartNo, d.part_desc as PartNameDesc,d.bif_proj as locCode,'" + supplierLoc + "' as SupplierLocation,'" + supplierCode + "' as SupplierCode, '' as PreparedBy, '' as ApprovedBy, '" + sqlOrgDateQry + "' as DateOrg, '" + sqlRevDateQry + "' as DateRev,'" + suppApprDate + "' as SupplierApprDate,'" + suppOtherDate + "' as OtherApprDate, '" + cpRptModel.KeyContactPerson + "' as ContactPerson, ' ___ ' as CustomerEnggApprov,");
                sbsql.Append(" ' ____ ' as CustomerQualityApprov, ' ____ ' as OtherApprovalcustomer,cast(b.seq_no as varchar) as PartProcessNo,b.opn_desc as ProcessNameOperationDesc, (SELECT Stuff((SELECT N'/ ' + cc_code FROM process_cc where part_no=a.part_no and route_no=C.route_no and seq_no= CAST(b.seq_no AS VARCHAR) FOR XML PATH(''),TYPE) .value('text()[1]','nvarchar(max)'),1,2,N'')  ) AS MachineDevice,cast(a.ISR_NO as varchar)  as IsrNo,a.feature as Product,cast(a.PROCESS_FEATURE as varchar)  as Process,ISNULL(a.SPEC_CHAR,'') as SplChar,(ISNULL(a.CTRL_SPEC_MIN,'')+ (CASE WHEN SUBSTRING(a.CTRL_SPEC_MIN, 1, LEN(a.CTRL_SPEC_MIN)) Like '%[0-9]%' AND SUBSTRING(a.CTRL_SPEC_MAX, 1, LEN(a.CTRL_SPEC_MAX)) Like '%[0-9]%' THEN  '/' ELSE  ' ' END) +ISNULL(a.CTRL_SPEC_MAX,'')) as ProcessSpec,");
                sbsql.Append("a.gauges_used as Gauges_Used,cast(a.sample_size as varchar) as SampleSize,a.freq_of_insp as SampleFreq,a.control_method as ControlMethod,a.reaction_plan as ReactionPlan,(select max(issue_date) from pccs_issue p where p.part_no =b.part_no and p.route_no=b.route_no) as issue_date ");
                if (seqNos == "")
                {
                    sbsql.Append("from pccs a,process_sheet b,process_cc c ,prd_mast  d where A.part_no = d.part_no and A.part_no = B.part_no and a.route_no = b.route_no and a.seq_no = b.seq_no and B.part_no = C.part_no and  b.route_no = c.route_no and b.seq_no = c.seq_no and C.part_no = '" + cpRptModel.PartNo + "'  and C.route_no ='" + cpRptModel.RouteNo + "' and cc_sno='1' ORDER BY A.seq_no,A.SNO");

                }
                else
                {
                    sbsql.Append("from pccs a,process_sheet b,process_cc c ,prd_mast  d where A.part_no = d.part_no and A.part_no = B.part_no and a.route_no = b.route_no and a.seq_no = b.seq_no and B.part_no = C.part_no and  b.route_no = c.route_no and b.seq_no = c.seq_no and C.part_no = '" + cpRptModel.PartNo + "'  and C.route_no ='" + cpRptModel.RouteNo + "' and c.seq_no in('" + seqNos + "') and cc_sno='1' ORDER BY A.seq_no,A.SNO");
                }

                dtv = ToDataTable(DB.ExecuteQuery<RptControlPlanModel>(sbsql.ToString()).ToList()).DefaultView;
                return dtv;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return dtv;
            }
        }
    }

}
