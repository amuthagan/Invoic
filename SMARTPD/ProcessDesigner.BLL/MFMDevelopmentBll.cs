using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class MFMDevelopmentBll : Essential
    {
        public MFMDevelopmentBll(UserInformation userinfo)
        {
            this.userInformation = userinfo;
        }

        public bool GetDropdownDetails(MFMDevelopmentModel mfmdev)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTableWithType((from o in DB.DDLOC_MAST
                                          where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                          select new { o.LOC_CODE, o.LOCATION }).ToList());
                if (dt != null)
                {

                    mfmdev.DVLocation = dt.DefaultView;
                }
                else
                {
                    mfmdev.DVLocation = null;
                }

                dt = ToDataTableWithType((from o in DB.DDCUST_MAST
                                          where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                          orderby o.CUST_NAME
                                          select new { o.CUST_CODE, o.CUST_NAME }).ToList());
                if (dt != null)
                {

                    mfmdev.DVCustomer = dt.DefaultView;
                }
                else
                {
                    mfmdev.DVCustomer = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool LoadTime_Refresh(MFMDevelopmentModel mfmdev)
        {
            try
            {
                DataTable category = null;
                DataTable loadtime = GetEmptyTable();
                string sql1 = (mfmdev.STAGE_START == "Allotted Date") ? "B.Allot_date" : TextReturned(mfmdev.STAGE_START);
                string sqlvari = "DATEDIFF(d," + TextReturned(mfmdev.STAGE_END) + "," + sql1 + ") ";
                string sqlvari2 = sql1 + " is not null and " + TextReturned(mfmdev.STAGE_END) + " is not null and ";
                string sqlcategory = "";
                string sqlLeadtime = "";

                if (mfmdev.TARGET_TIME != "")
                {
                    sqlLeadtime = "select A.PART_NO,PG_CATEGORY,convert(varchar, samp_submit_date, 103) DEV_DATE," + sqlvari + "  LEAD_TIME ,b.bif_proj UNIT from mfm_mast a RIGHT JOIN prd_mast b ON a.part_no = b.part_no where " + sqlvari + " <= " + mfmdev.TARGET_TIME + " and b.pg_category in ('1','2','3','3F') and " + sqlvari2 + " samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) and samp_submit_date is not null order by LEAD_TIME, b.pg_category";
                    sqlcategory = "select count(*) CNT ,PG_CATEGORY PG from mfm_mast a RIGHT JOIN prd_mast b ON a.part_no = b.part_no where (" + sqlvari + ") <= " + mfmdev.TARGET_TIME + " and b.pg_category in ('1','2','3','3F') and " + sqlvari2 + " samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) and samp_submit_date is not null group by b.pg_category";
                }
                else
                {
                    sqlLeadtime = "select A.PART_NO,PG_CATEGORY,convert(varchar, samp_submit_date, 103) DEV_DATE," + sqlvari + "  LEAD_TIME ,b.bif_proj UNIT from mfm_mast a RIGHT JOIN prd_mast b ON a.part_no = b.part_no where b.pg_category in ('1','2','3','3F') and " + sqlvari2 + " samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) and samp_submit_date is not null order by LEAD_TIME, b.pg_category";
                    sqlcategory = "select count(*) CNT ,PG_CATEGORY PG from mfm_mast a RIGHT JOIN prd_mast b ON a.part_no = b.part_no where  b.pg_category in ('1','2','3','3F') and " + sqlvari2 + " samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103)  and samp_submit_date is not null group by b.pg_category";
                }

                string sqltotal = "select count(*) CNT, PG_CATEGORY PG from mfm_mast a RIGHT JOIN prd_mast b ON a.part_no = b.part_no where b.pg_category in ('1','2','3','3F') and samp_submit_date is not null and  samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) and (b.HOLD_ME = 0 OR b.HOLD_ME IS NULL) and (b.CANCEL_STATUS = 0 OR b.CANCEL_STATUS IS NULL) group by b.pg_category";

                category = ToDataTableWithType(DB.ExecuteQuery<CategoryModel>(sqlcategory).ToList());


                for (int k = 1; k <= 2; k++)
                {
                    if (category != null && category.Rows.Count > 0)
                    {
                        foreach (DataRow dr in category.Rows)
                        {
                            if (dr["PG"].ToString() == "1")
                            {
                                loadtime.Rows[0][k] = dr["CNT"];
                                loadtime.Rows[3][k] = loadtime.Rows[3][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                            }
                            else if (dr["PG"].ToString() == "2")
                            {
                                loadtime.Rows[1][k] = dr["CNT"];
                                loadtime.Rows[3][k] = loadtime.Rows[3][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                            }
                            else if (dr["PG"].ToString() == "3" || dr["PG"].ToString() == "3F")
                            {
                                loadtime.Rows[2][k] = loadtime.Rows[2][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                                loadtime.Rows[3][k] = loadtime.Rows[3][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                            }
                        }
                    }

                    category = ToDataTableWithType(DB.ExecuteQuery<CategoryModel>(sqltotal).ToList());
                }

                mfmdev.DVLeadTime = loadtime.DefaultView;

                mfmdev.DTLeadTimePrint = ToDataTableWithType(DB.ExecuteQuery<LeadTimePrint>(sqlLeadtime).ToList());

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private String TextReturned(string stage)
        {

            if (stage == "Document Date")
            {
                return "A.DOC_REL_DT_ACTUAL";
            }
            else if (stage == "Tool Date")
            {
                return "A.TOOLS_READY_ACTUAL_DT";
            }
            else if (stage == "Forging Date")
            {
                return "A.FORGING_ACTUAL_DT";
            }
            else if (stage == "Secondary Date")
            {
                return "GREATEST(A.SECONDARY_ACTUAL_DT,A.HEAT_TREATMENT_ACTUAL)";
            }
            else if (stage == "PPAP Date")
            {
                return "A.PPAP_ACTUAL_DT";
            }
            else if (stage == "PSW Date")
            {
                return "A.PSW_DATE";
            }
            else
            {
                return "";
            }
        }

        private DataTable GetEmptyTable()
        {
            try
            {
                DataTable dt = new DataTable("Category");
                dt.Columns.Add(new DataColumn("CATEGORY"));
                dt.Columns.Add(new DataColumn("PARTS_QUALIFIED", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("TOTAL_PARTS", typeof(Decimal)));

                DataRow drow = dt.NewRow();
                drow["CATEGORY"] = "A(PG1)";
                dt.Rows.Add(drow);

                drow = dt.NewRow();
                drow["CATEGORY"] = "B(PG2)";
                dt.Rows.Add(drow);

                drow = dt.NewRow();
                drow["CATEGORY"] = "C(PG3)";
                dt.Rows.Add(drow);

                drow = dt.NewRow();
                drow["CATEGORY"] = "TOTAL";
                dt.Rows.Add(drow);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool PSW_Refresh(MFMDevelopmentModel mfmdev)
        {
            try
            {
                DataTable category = null;
                DataTable psw = GetEmptyTable();
                string sql = "";
                string tabl = "";
                string ty = "";
                string sqlcategory = "";
                string sqlpswApp = "";

                if (mfmdev.LOC_CODE == "" && mfmdev.CUST_CODE == "")
                {
                    sql = " AND c.CURRENT_CIREF = 1 ";
                    tabl = "LEFT JOIN prd_ciref c ON b.part_no = c.part_no LEFT JOIN ddci_info d ON c.ci_Ref = d.ci_reference ";
                }
                else if (mfmdev.LOC_CODE != "" && mfmdev.CUST_CODE == "")
                {
                    sql = " AND BIF_PROJ = '" + mfmdev.LOC_CODE + "' AND c.CURRENT_CIREF = 1 ";
                    tabl = "LEFT JOIN prd_ciref c ON b.part_no = c.part_no LEFT JOIN ddci_info d ON c.ci_Ref = d.ci_reference ";
                }
                else if (mfmdev.LOC_CODE == "" && mfmdev.CUST_CODE != "")
                {
                    sql = " and d.cust_code = '" + mfmdev.CUST_CODE + "' AND c.CURRENT_CIREF = 1";
                    tabl = "JOIN prd_ciref c ON b.part_no = c.part_no JOIN ddci_info d ON c.ci_Ref = d.ci_reference ";
                }
                else if (mfmdev.LOC_CODE != "" && mfmdev.CUST_CODE != "")
                {
                    sql = " AND BIF_PROJ = '" + mfmdev.LOC_CODE + "' and d.cust_code = '" + mfmdev.CUST_CODE + "' AND c.CURRENT_CIREF = 1 ";
                    tabl = "JOIN prd_ciref c ON b.part_no = c.part_no JOIN ddci_info d ON c.ci_Ref = d.ci_reference ";
                }

                if (mfmdev.AwaitingPSWApproval)
                {
                    ty = "'NO'";
                }
                else
                {
                    ty = "'YES','WAIVED'";
                }
                sqlpswApp = "select b.PART_NO,b.PG_CATEGORY,b.bif_proj UNIT,convert(Varchar, b.SAMP_SUBMIT_DATE, 103) SAMP_SUBMIT_DATE,b.PSW_ST PSW_STATUS,convert(Varchar, a.PSW_date, 103) APPROVED,e.CUST_NAME ,d.CUST_DWG_NO from mfm_mast a RIGHT JOIN prd_mast b ON  a.part_no = b.part_no " + tabl + " LEFT JOIN ddcust_mast e ON d.cust_code = e.cust_code where samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) and samp_submit_date is not null and b.psw_St in (" + ty + ")" + sql + " and (b.HOLD_ME = 0 OR b.HOLD_ME IS NULL)  and (b.CANCEL_STATUS = 0 OR b.CANCEL_STATUS IS NULL) order by samp_submit_date";

                String sqlpswcount = "select count(*) cnt from Prd_mast b  where samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) and (b.HOLD_ME = 0 OR b.HOLD_ME IS NULL) and (b.CANCEL_STATUS = 0 OR b.CANCEL_STATUS IS NULL) and samp_submit_date is not null ";

                sqlcategory = "select count(*) cnt ,PG_CATEGORY PG from mfm_mast a RIGHT JOIN prd_mast b ON  a.part_no = b.part_no " + tabl + " where  b.pg_category in ('1','2','3','3F') and  samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) and samp_submit_date is not null and b.psw_St in (" + ty + ")" + sql + "and (b.HOLD_ME = 0 OR b.HOLD_ME IS NULL) and (b.CANCEL_STATUS = 0 OR b.CANCEL_STATUS IS NULL) group by b.pg_category";

                string sqltotal = "select count(*) cnt, PG_CATEGORY PG from mfm_mast a RIGHT JOIN prd_mast b ON  a.part_no = b.part_no " + tabl + " where b.pg_category in ('1','2','3','3F') and samp_submit_date is not null and  samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) " + sql + " and (b.HOLD_ME = 0 OR b.HOLD_ME IS NULL) and (b.CANCEL_STATUS = 0 OR b.CANCEL_STATUS IS NULL) group by b.pg_category";

                category = ToDataTableWithType(DB.ExecuteQuery<CategoryModel>(sqlcategory).ToList());


                for (int k = 1; k <= 2; k++)
                {
                    if (category != null && category.Rows.Count > 0)
                    {
                        foreach (DataRow dr in category.Rows)
                        {
                            if (dr["PG"].ToString() == "1")
                            {
                                psw.Rows[0][k] = dr["CNT"];
                                psw.Rows[3][k] = psw.Rows[3][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                            }
                            else if (dr["PG"].ToString() == "2")
                            {
                                psw.Rows[1][k] = dr["CNT"];
                                psw.Rows[3][k] = psw.Rows[3][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                            }
                            else if (dr["PG"].ToString() == "3" || dr["PG"].ToString() == "3F")
                            {
                                psw.Rows[2][k] = psw.Rows[2][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                                psw.Rows[3][k] = psw.Rows[3][k].ToString().ToDecimalValue() + dr["CNT"].ToString().ToDecimalValue();
                            }
                        }
                    }

                    category = ToDataTableWithType(DB.ExecuteQuery<CategoryModel>(sqltotal).ToList());
                }

                mfmdev.DVPSW = psw.DefaultView;

                category = ToDataTableWithType(DB.ExecuteQuery<CategoryModel>(sqlpswcount).ToList());
                if (category != null && category.Rows.Count > 0)
                {
                    mfmdev.PSWCount = category.Rows[0]["CNT"].ToString();
                }
                else
                {
                    mfmdev.PSWCount = "";
                }

                mfmdev.DTPSWPrint = ToDataTable(DB.ExecuteQuery<PSWPrint>(sqlpswApp).ToList());

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataTable GetMFMDevelopment(MFMDevelopmentModel mfmdev)
        {
            try
            {               
                String sql = "";
                String createSQL = "";

                if (mfmdev.LOC_CODE == "" && mfmdev.CUST_CODE == "")
                {
                    sql = " order by CUST_NAME,LOC_CODE,PART_NO ";
                }
                else if (mfmdev.LOC_CODE != "" && mfmdev.CUST_CODE == "")
                {
                    sql = "AND LOC_CODE = '" + mfmdev.LOC_CODE + "' order by CUST_NAME,PART_NO";
                }
                else if (mfmdev.LOC_CODE == "" && mfmdev.CUST_CODE != "")
                {
                    sql = "AND CUST_CODE = '" + mfmdev.CUST_CODE + "' order by CUST_NAME,PART_NO";
                }
                else if (mfmdev.LOC_CODE != "" && mfmdev.CUST_CODE != "")
                {
                    sql = "AND LOC_CODE = '" + mfmdev.LOC_CODE + "' AND CUST_CODE = '" + mfmdev.CUST_CODE + "' order by LOC_CODE,PART_NO";
                }

                if (mfmdev.AwaitingDoc)
                {
                    createSQL = "SELECT C.LOC_CODE, F.CUST_NAME, A.PART_NO, B.PART_DESC, B.PG_CATEGORY,convert(Varchar, B.ALLOT_DATE, 103) ALLOT_DATE"
                              + " FROM MFM_MAST A LEFT JOIN PRD_MAST B ON A.PART_NO = B.PART_NO LEFT JOIN DDLOC_MAST C ON B.BIF_PROJ = C.LOC_CODE LEFT JOIN PRD_CIREF D ON B.PART_NO = D.PART_NO LEFT JOIN DDCI_INFO E ON D.CI_REF = E.CI_REFERENCE LEFT JOIN DDCUST_MAST F ON E.CUST_CODE = F.CUST_CODE "
                              + " WHERE ( b.part_no like 'M%' or b.part_no like 'S%' ) and  A.DOC_REL_DT_ACTUAL IS NULL AND A.DOC_REL_DT_PLAN IS NOT NULL  "
                              + " AND B.ALLOT_DATE > convert(datetime, '31/03/1999', 103) AND (B.cancel_status = 0 OR B.cancel_status IS NULL) "
                              + " AND (B.HOLD_ME = 0 OR B.HOLD_ME IS NULL)  AND D.CURRENT_CIREF = 1 AND B.samp_submit_date is null";

                    if (mfmdev.LOC_CODE == "" && mfmdev.CUST_CODE == "")
                    {
                        sql = " order by F.CUST_NAME,C.loc_code,A.PART_NO";
                    }
                    else if (mfmdev.LOC_CODE != "" && mfmdev.CUST_CODE == "")
                    {
                        sql = " AND C.LOC_CODE = '" + mfmdev.LOC_CODE + "' order by F.CUST_NAME,A.PART_NO";
                    }
                    else if (mfmdev.LOC_CODE == "" && mfmdev.CUST_CODE != "")
                    {
                        sql = " AND F.CUST_code = '" + mfmdev.CUST_CODE + "' order by F.CUST_NAME,A.PART_NO";
                    }
                    else if (mfmdev.LOC_CODE != "" && mfmdev.CUST_CODE != "")
                    {
                        sql = " AND C.LOC_CODE = '" + mfmdev.LOC_CODE + "' AND F.CUST_code = '" + mfmdev.CUST_CODE + "' order by C.LOC_CODE,A.PART_NO";
                    }

                    createSQL = createSQL + sql;
                    return ToDataTable(DB.ExecuteQuery<AwaitingDoc>(createSQL).ToList());
                }
                else if (mfmdev.AwaitingTools)
                {
                    createSQL = "SELECT LOC_CODE,convert(Varchar,CUST_CODE) CUST_CODE,CUST_NAME, PART_NO, PART_DESC, PG_CATEGORY,convert(Varchar, DOC_REL_DT_ACTUAL, 103) DOC_REL_DT_ACTUAL FROM MFM_MAST_VIEW"
                                + " WHERE TOOLS_READY_ACTUAL_DT IS NULL AND DOC_REL_DT_ACTUAL > convert(datetime, '31/03/1999', 103) AND (CANCEL_STATUS=0 OR cancel_status IS NULL) "
                                + "  AND (HOLD_ME = 0 OR HOLD_ME IS NULL) AND ALLOT_DATE > convert(datetime, '31/03/1999', 103) and samp_submit_date is null ";

                    createSQL = createSQL + sql;
                    return ToDataTable(DB.ExecuteQuery<AwaitingTool>(createSQL).ToList());
                }
                else if (mfmdev.AwaitingForging)
                {
                    createSQL = " select g.LOC_CODE, h.CUST_NAME, a.PART_NO,b.PART_DESC,b.PG_CATEGORY,convert(Varchar, a.DOC_REL_DT_ACTUAL, 103) DOC_REL_DT_ACTUAL "
                                  + " from mfm_mast a,prd_mast b,ddci_info c,process_cc d,process_main e,ddcost_cent_mast f,ddloc_mast g,ddcust_mast h,"
                                  + " ddrm_mast i,process_sheet j,prd_ciref p where a.part_no = b.part_no and b.part_no = p.part_no and "
                                  + " p.ci_ref = c.ci_reference and c.cust_code = h.cust_code and b.bif_proj = g.loc_code and b.part_no = e.part_no "
                                  + " and e.part_no = j.part_no and e.route_no = j.route_no and i.rm_code = e.rm_cd and j.part_no = d.part_no and "
                                  + " j.route_no = d.route_no and j.seq_no = d.seq_no and d.cc_code collate SQL_Latin1_General_CP1_CI_AS = f.cost_cent_code and j.opn_cd >= 1010 and "
                                  + " j.opn_cd <= 1040 AND p.CURRENT_CIREF = 1 and e.route_no =1 and d.cc_sno = 1 and a.DOC_REL_DT_ACTUAL is not null and "
                                  + " a.FORGING_ACTUAL_DT  is null and a.PPAP_ACTUAL_DT is null and g.LOC_CODE='" + mfmdev.LOC_CODE + "' and a.DOC_REL_DT_ACTUAL > convert(datetime, '31/03/1999', 103) "
                                  + " and b.ALLOT_DATE > convert(datetime, '31/03/1999', 103) and b.SAMP_SUBMIT_DATE is null ";

                    return ToDataTable(DB.ExecuteQuery<AwaitingForging>(createSQL).ToList());
                }
                else if (mfmdev.AwaitingSecondary)
                {
                    createSQL = "SELECT LOC_CODE, CUST_NAME, PART_NO, PART_DESC, PG_CATEGORY,convert(Varchar, DOC_REL_DT_ACTUAL, 103) DOC_REL_DT_ACTUAL FROM MFM_MAST_VIEW "
                                  + " WHERE FORGING_ACTUAL_DT IS not NULL AND SECONDARY_ACTUAL_DT IS NULL AND HEAT_TREATMENT_ACTUAL IS NULL AND DOC_REL_Dt_actual > convert(datetime, '31/03/1999', 103) "
                                  + " AND ALLOT_DATE > convert(datetime, '31/03/1999', 103) and samp_submit_date is null AND (HOLD_ME = 0 OR HOLD_ME IS NULL) ";

                    createSQL = createSQL + sql;
                    return ToDataTable(DB.ExecuteQuery<AwaitingTool>(createSQL).ToList());
                }
                else if (mfmdev.AwaitingPPAP)
                {
                    createSQL = "SELECT LOC_CODE,CUST_NAME,PART_NO, PART_DESC, PG_CATEGORY,convert(Varchar, DOC_REL_DT_ACTUAL, 103) DOC_REL_DT_ACTUAL, RESP, REMARKS FROM MFM_MAST_VIEW"
                               + " WHERE (SECONDARY_ACTUAL_DT is not null OR  HEAT_TREATMENT_ACTUAL is NOT null ) and PPAP_ACTUAL_DT is null AND "
                               + " ALLOT_DATE > convert(datetime, '31/03/1999', 103) and samp_submit_date is null AND"
                               + " DOC_REL_DT_ACTUAL > convert(datetime, '31/03/1999', 103) AND (HOLD_ME = 0 OR HOLD_ME IS NULL)  and"
                               + " LOC_CODE ='" + mfmdev.LOC_CODE + "' ";

                    return ToDataTable(DB.ExecuteQuery<AwaitingPPAP>(createSQL).ToList());
                }
                else if (mfmdev.FirstTimeRight)
                {
                    createSQL = "select CUST_NAME,a.PART_NO,PART_DESC,PG_CATEGORY,PSW_ST,convert(Varchar, SAMP_SUBMIT_DATE, 103) SAMP_SUBMIT_DATE"
                                 + " From  mfm_mast_view a ,dev_report_main b where samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) "
                                 + " and a.part_no=b.part_no and b.dev_run_no=1 and a.psw_st='YES' AND (A.CANCEL_STATUS = 0 OR A.CANCEL_STATUS IS NULL) AND (A.HOLD_ME = 0 OR A.HOLD_ME IS NULL) AND b.CUST_COMP='NO' order by pg_category";

                    return ToDataTable(DB.ExecuteQuery<FirstTimeRight>(createSQL).ToList());
                }
                else if (mfmdev.CustomerComp)
                {
                    createSQL = "select CUST_NAME,a.PART_NO,PART_DESC,PG_CATEGORY,PSW_ST,convert(Varchar, SAMP_SUBMIT_DATE, 103) SAMP_SUBMIT_DATE"
                                 + " From  mfm_mast_view a ,dev_report_main b where samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) "
                                 + " and a.part_no=b.part_no and b.dev_run_no=1 and (A.HOLD_ME = 0 OR A.HOLD_ME IS NULL) and (A.CANCEL_STATUS = 0 OR A.CANCEL_STATUS IS NULL) AND b.CUST_COMP='YES' order by pg_category";

                    return ToDataTable(DB.ExecuteQuery<FirstTimeRight>(createSQL).ToList());
                }
                else if (mfmdev.NoOfShifts)
                {
                    createSQL = "select CUST_NAME,a.PART_NO,PART_DESC,PG_CATEGORY,NO_OF_SHIFTS,NO_OF_TOOL_CHNG,convert(Varchar, SAMP_SUBMIT_DATE, 103) SAMP_SUBMIT_DATE"
                                 + " From  mfm_mast_view a ,dev_report_main b where a.samp_submit_date between convert(datetime, '" + mfmdev.START_DATE.ToFormattedDateAsString() + "', 103) and convert(datetime, '" + mfmdev.END_DATE.ToFormattedDateAsString() + "', 103) "
                                 + " and a.part_no=b.part_no and (a.HOLD_ME = 0 OR a.HOLD_ME IS NULL) and (a.CANCEL_STATUS = 0 OR a.CANCEL_STATUS IS NULL) AND  dev_run_no=1 order by a.pg_category";

                    return ToDataTable(DB.ExecuteQuery<NoOfShifts>(createSQL).ToList());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }

    partial class CategoryModel
    {
        public int CNT
        { get; set; }

        public string PG
        { get; set; }
    }

    partial class LeadTimePrint
    {
        public string PART_NO
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public string DEV_DATE
        { get; set; }

        public int LEAD_TIME
        { get; set; }

        public string UNIT
        { get; set; }
    }

    partial class PSWPrint
    {
        public string PART_NO
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public string UNIT
        { get; set; }

        public string SAMP_SUBMIT_DATE
        { get; set; }

        public string PSW_STATUS
        { get; set; }

        public string APPROVED
        { get; set; }

        public string CUST_NAME
        { get; set; }

        public string CUST_DWG_NO
        { get; set; }
    }

    partial class AwaitingDoc
    {
        public string LOC_CODE
        { get; set; }

        public string CUST_NAME
        { get; set; }

        public string PART_NO
        { get; set; }

        public string PART_DESC
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public string ALLOT_DATE
        { get; set; }

    }

    partial class AwaitingTool
    {
        public string LOC_CODE
        { get; set; }

        public string CUST_CODE
        { get; set; }

        public string CUST_NAME
        { get; set; }

        public string PART_NO
        { get; set; }

        public string PART_DESC
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public string DOC_REL_DT_ACTUAL
        { get; set; }

    }

    partial class AwaitingForging
    {
        public string LOC_CODE
        { get; set; }

        public string CUST_NAME
        { get; set; }

        public string PART_NO
        { get; set; }

        public string PART_DESC
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public string DOC_REL_DT_ACTUAL
        { get; set; }

    }

    partial class AwaitingPPAP
    {
        public string LOC_CODE
        { get; set; }

        public string CUST_NAME
        { get; set; }

        public string PART_NO
        { get; set; }

        public string PART_DESC
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public string DOC_REL_DT_ACTUAL
        { get; set; }

        public string RESP
        { get; set; }

        public string REMARKS
        { get; set; }

    }

    partial class FirstTimeRight
    {

        public string CUST_NAME
        { get; set; }

        public string PART_NO
        { get; set; }

        public string PART_DESC
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public string PSW_ST
        { get; set; }

        public string SAMP_SUBMIT_DATE
        { get; set; }
    }

    partial class NoOfShifts
    {

        public string CUST_NAME
        { get; set; }

        public string PART_NO
        { get; set; }

        public string PART_DESC
        { get; set; }

        public string PG_CATEGORY
        { get; set; }

        public Nullable<Decimal> NO_OF_SHIFTS
        { get; set; }

        public Nullable<Decimal> NO_OF_TOOL_CHNG
        { get; set; }

        public string SAMP_SUBMIT_DATE
        { get; set; }
    }
}
