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
    public class DDPerformanceBll : Essential
    {
        StringBuilder sbsql = new StringBuilder();
        public DDPerformanceBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataView CostSheetReceived(string startDate, string endDate, string city)
        {
            sbsql = new StringBuilder();
            try
            {
                sbsql.Append(" select ci_reference as COLUMN0,upper(prod_desc) as COLUMN1,cust_dwg_no as COLUMN2,cust_name  as COLUMN3,replace(convert(char(11),enqu_recd_on,103),' ',' ') as COLUMN4,'' as COLUMN5 from ddci_info a , ");
                sbsql.Append(" ddcust_mast b where convert(date,enqu_recd_on ,103) between convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103) and ci_reference not like 'O%' ");
                ValidateCity(city, "a.loc_code");
                sbsql.Append(" and ci_reference not like 'U%' and a.cust_code=b.cust_code order by COLUMN0 ");
               
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView CostSheetCompleted(string startDate, string endDate, string city)
        {
            sbsql = new StringBuilder();
            try
            {
                sbsql.Append("select ci_reference as COLUMN0,upper(prod_desc) as COLUMN1,cust_dwg_no as COLUMN2,cust_name as COLUMN3,replace(convert(char(11),fr_cs_date,103),' ',' ') as COLUMN4 from ddci_info a ,");
                sbsql.Append(" ddcust_mast b where convert(date,fr_cs_date ,103) between convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103) and ci_reference not like 'O%' and ci_reference not like 'U%' ");
                ValidateCity(city, "a.loc_code");
                sbsql.Append(" and a.cust_code= b.cust_code and pending ='0' order by COLUMN0 ");
               
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        public DataView PartNumberAllotted(string startDate, string endDate, string city)
        {
            sbsql = new StringBuilder();
            try
            {
                sbsql.Append("select a.part_no as COLUMN0,part_desc as COLUMN1,pg_category as COLUMN2,b.cust_name as COLUMN3,replace(convert(char(11),allot_date,103),' ',' ') as COLUMN4,e.compiled_by as COLUMN5 from prd_mast a,ddcust_mast b, ");
                
                sbsql.Append(" prd_ciref c,ddci_info d,pccs_issue e  where a.part_no=c.part_no and a.part_no=e.part_no and a.part_no=c.part_no AND c.CURRENT_CIREF = 1 ");
                //sbsql.Append(" prd_ciref c,ddci_info d,pccs_issue e  where a.part_no=c.part_no and a.part_no=e.part_no and a.part_no=c.part_no ");
                
                sbsql.Append(" and e.issue_no =(select max(issue_no) from pccs_issue where part_no=a.part_no and part_no=c.part_no) and route_no=1 and c.ci_ref=d.ci_reference ");
                ValidateCity(city, "a.bif_proj");
                sbsql.Append(" and b.cust_code=d.cust_code and ( a.hold_me=0 or a.hold_me is null ) and (A.CANCEL_STATUS = 0 OR A.CANCEL_STATUS IS NULL) and convert(date,allot_date ,103) between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) and convert(date,'" + endDate + "',103) order by COLUMN0 ");
             
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView DocumentReleased(string startDate, string endDate, string city)
        {
            try
            {
                sbsql = new StringBuilder();
                sbsql.Append("select a.part_no as COLUMN0,part_desc as COLUMN1,pg_category as COLUMN2,b.cust_name as COLUMN3,replace(convert(char(11),DOC_REL_date,103),' ',' ') as COLUMN4,e.compiled_by as COLUMN5 from prd_mast a,ddcust_mast b,");
                
                sbsql.Append("prd_ciref c, ddci_info d,pccs_issue e  where a.part_no=c.part_no and a.part_no=e.part_no and a.part_no=c.part_no AND c.CURRENT_CIREF = 1 and e.issue_no =(select max(issue_no) from pccs_issue where part_no=a.part_no and part_no=c.part_no) and route_no=1 and c.ci_ref=d.ci_reference ");
                //sbsql.Append("prd_ciref c, ddci_info d,pccs_issue e  where a.part_no=c.part_no and a.part_no=e.part_no and a.part_no=c.part_no and e.issue_no =(select max(issue_no) from pccs_issue where part_no=a.part_no and part_no=c.part_no) and route_no=1 and c.ci_ref=d.ci_reference ");
                
                ValidateCity(city, "a.bif_proj");
                sbsql.Append(" and b.cust_code=d.cust_code and  (a.HOLD_ME = 0 OR a.HOLD_ME IS NULL) and (a.CANCEL_STATUS = 0 OR a.CANCEL_STATUS IS NULL) and convert(date,doc_rel_date ,103) between ");
                sbsql.Append("convert(date,'" + startDate + "', 103)");
                sbsql.Append(" AND convert(date,'" + endDate + "', 103) order by COLUMN0 ");
                
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView SampleSubmitted(string startDate, string endDate, string city)
        {
            try
            {
                sbsql = new StringBuilder();

                sbsql.Append("select a.part_no as COLUMN0,part_desc as COLUMN1,pg_category as COLUMN2,b.cust_name as COLUMN3,");
                sbsql.Append(" replace(convert(char(11),samp_submit_date,103),' ',' ') as COLUMN4, ");
                sbsql.Append("e.compiled_by as COLUMN5 from prd_mast a  ");
                sbsql.Append("left join prd_ciref c  ");
                sbsql.Append("on a.part_no=c.part_no ");
                sbsql.Append("left join  ddci_info d ");
                sbsql.Append("on c.ci_ref=d.ci_reference ");
                sbsql.Append("right join ddcust_mast b ");
                sbsql.Append("on b.cust_code = d.cust_code ");
                sbsql.Append("left join  ");
                sbsql.Append("pccs_issue e   ");
                sbsql.Append("on a.part_no=e.part_no  ");
                
                sbsql.Append("where c.CURRENT_CIREF = 1 and e.issue_no =(select max(issue_no) from pccs_issue where part_no=a.part_no)  ");
                //sbsql.Append("where e.issue_no =(select max(issue_no) from pccs_issue where part_no=a.part_no)  ");
                
                sbsql.Append("and (a.HOLD_ME = 0 OR a.HOLD_ME IS NULL)   ");
                ValidateCity(city, "a.bif_proj");
                sbsql.Append(" and convert(date,samp_subMit_date, 103) between convert(date,'" + startDate + "', 103) AND  ");
                sbsql.Append("convert(date,'" + endDate + "', 103) and   ");
                sbsql.Append("e.route_no IN (select route_no from process_main where part_no = a.part_no and current_proc = 1) order by COLUMN0 ");
              
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void GetDDPerformanceSummary(DDPerformanceSummaryModel ddpfsm)
        {
            try
            {
                sbsql = new StringBuilder();
                //Cost sheet received Last Month
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),112) ");
                ddpfsm.CSR_LM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Cost Sheet Completed Last Month
                sbsql = new StringBuilder();
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),112) ");
                sbsql.Append(" AND PENDING = '0' ");
                ddpfsm.CSC_LM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Part No Allotted Last Month
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), ALLOT_DATE, 112) = Convert(nvarchar(6),DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),112) ");
                ddpfsm.PNA_LM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Document Released Last Month
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), DOC_REL_DATE, 112) = Convert(nvarchar(6),DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),112) ");
                ddpfsm.DR_LM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Samples Submitted Last Month
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), SAMP_SUBMIT_DATE, 112) = Convert(nvarchar(6),DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),112) ");
                ddpfsm.SS_LM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                sbsql = new StringBuilder();
                //Cost sheet received This Month
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.CSR_TM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Cost Sheet Completed This Month
                sbsql = new StringBuilder();
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append(" AND PENDING = '0' ");
                ddpfsm.CSC_TM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Part No Allotted This Month
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), ALLOT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.PNA_TM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Document Released This Month
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), DOC_REL_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.DR_TM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Samples Submitted This Month
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), SAMP_SUBMIT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.SS_TM = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Cost sheet received this year
                sbsql = new StringBuilder();
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'04' ");
                sbsql.Append("AND Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.CSR_TY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Cost Sheet Completed This year
                sbsql = new StringBuilder();
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'04' ");
                sbsql.Append("AND Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append(" AND PENDING = '0' ");
                ddpfsm.CSC_TY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Part No Allotted This Year
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                //sbsql.Append("WHERE CONVERT(nvarchar(6), ALLOT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), ALLOT_DATE, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'04' ");
                sbsql.Append("AND Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.PNA_TY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Document Released This Year
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                //sbsql.Append("WHERE CONVERT(nvarchar(6), DOC_REL_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), DOC_REL_DATE, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'04' ");
                sbsql.Append("AND Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.DR_TY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Samples Submitted This Year
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                //sbsql.Append("WHERE CONVERT(nvarchar(6), SAMP_SUBMIT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), SAMP_SUBMIT_DATE, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'04' ");
                sbsql.Append("AND Convert(nvarchar(6),GETDATE(),112) ");
                ddpfsm.SS_TY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Cost sheet received last year
                sbsql = new StringBuilder();
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 2))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) END)+'04' ");
                sbsql.Append("AND  ");
                sbsql.Append("(CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'03' ");
                ddpfsm.CSR_LY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Cost Sheet Completed last year
                sbsql = new StringBuilder();
                sbsql.Append("select ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO ");
                sbsql.Append("WHERE CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U*' ");
                sbsql.Append("AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 2))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) END)+'04' ");
                sbsql.Append("AND  ");
                sbsql.Append("(CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'03' ");
                sbsql.Append(" AND PENDING = '0' ");
                ddpfsm.CSC_LY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Part No Allotted last Year
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                //sbsql.Append("WHERE CONVERT(nvarchar(6), ALLOT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), ALLOT_DATE, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 2))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) END)+'04' ");
                sbsql.Append("AND  ");
                sbsql.Append("(CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'03' ");
                ddpfsm.PNA_LY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Document Released last Year
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                //sbsql.Append("WHERE CONVERT(nvarchar(6), DOC_REL_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), DOC_REL_DATE, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 2))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) END)+'04' ");
                sbsql.Append("AND  ");
                sbsql.Append("(CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'03' ");
                ddpfsm.DR_LY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Samples Submitted last Year
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(PART_NO),0) FROM PRD_MAST ");
                //sbsql.Append("WHERE CONVERT(nvarchar(6), SAMP_SUBMIT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                sbsql.Append("WHERE CONVERT(nvarchar(6), SAMP_SUBMIT_DATE, 112) ");
                sbsql.Append("BETWEEN (CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 2))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) END)+'04' ");
                sbsql.Append("AND  ");
                sbsql.Append("(CASE WHEN MONTH(GETDATE()) < 4 THEN ");
                sbsql.Append("RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE()) - 1))) ");
                sbsql.Append("ELSE RTRIM(LTRIM(CONVERT(nvarchar(4),YEAR(GETDATE())))) END)+'03' ");
                ddpfsm.SS_LY = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();


                //Cost Sheet Pending Domestic
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO WHERE ");
                sbsql.Append("CI_REFERENCE NOT LIKE 'O%' AND CI_REFERENCE NOT LIKE 'U%' ");
                sbsql.Append(" AND CI_REFERENCE NOT LIKE 'X%' ");
                sbsql.Append(" AND PENDING = '1'");
                ddpfsm.CSPD = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Cost Sheet Pending Export
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO WHERE ");
                sbsql.Append("CI_REFERENCE LIKE 'X%' ");
                sbsql.Append(" AND PENDING = '1'");
                ddpfsm.CSPE = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

                //Part No Allocation Pending
                sbsql = new StringBuilder();
                sbsql.Append("SELECT ISNULL(COUNT(CI_REFERENCE),0) FROM DDCI_INFO WHERE ");
                sbsql.Append("ALLOT_PART_NO = 1");
                ddpfsm.PNAP = DB.ExecuteQuery<int>(sbsql.ToString()).FirstOrDefault<int>().ToValueAsString();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public DataView GetLocationCombo()
        {
            DataTable dttable;
            DataRow drRow;
            dttable = new DataTable();
            DataSet dsMaster = new DataSet();
            List<StringBuilder> sbSQL = new List<StringBuilder>();

            try
            {
                dttable = ToDataTable((from c in DB.DDLOC_MAST.AsEnumerable()
                                       orderby c.LOC_CODE ascending
                                       select new { c.LOC_CODE, c.LOCATION }).ToList());

                drRow = dttable.NewRow();

                drRow["LOC_CODE"] = "All";
                drRow["LOCATION"] = "All";
                dttable.Rows.InsertAt(drRow, 0);
                return dttable.DefaultView;
                //dsMaster=dalConnect.GetDataTable(sbSQL,)
            }
            catch (Exception ex)
            {
                return null;
                throw ex.LogException();
            }
        }

        private string sqlEncode(string sqlValue)
        {
            return sqlValue.Replace("'", "''");
        }

        private string ValidateCity(string city, string colname)
        {
            city = sqlEncode(city);
            if (city.Trim().Length > 0 && city != "All")
            {

                if (city.Substring(0, 1).ToUpper() == "K" || city.Substring(0, 1).ToUpper() == "Y" || city.Substring(0, 1).ToUpper() == "M")
                {
                    sbsql.Append(" and " + colname + " like '" + city.Substring(0, 1).ToUpper() + "%'");
                }
                else
                {
                    sbsql.Append(" and " + colname + " = '" + city + "'");
                }
            }
            return city;
        }

        public DataView GetPartNosAllottedSummary()
        {
            try
            {
                sbsql = new StringBuilder();
                //"AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),GETDATE(),112) "
                sbsql.Append(" SELECT PART_NO as COLUMN0, PART_DESC as COLUMN1, CONVERT(NVARCHAR(15),ALLOT_DATE,103) AS COLUMN2 FROM PRD_MAST WHERE ");
                sbsql.Append(" CONVERT(nvarchar(6), ALLOT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetDocumentsReleasedSummary()
        {
            try
            {
                sbsql = new StringBuilder();
                //"AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),GETDATE(),112) "
                sbsql.Append("SELECT PART_NO AS COLUMN0, PART_DESC AS COLUMN1, CONVERT(NVARCHAR(15),DOC_REL_DATE,103) AS COLUMN2 FROM PRD_MAST WHERE ");
                sbsql.Append(" CONVERT(nvarchar(6), DOC_REL_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetSamplesSubmittedSummary()
        {
            try
            {
                sbsql = new StringBuilder();
                //"AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),GETDATE(),112) "
                sbsql.Append("SELECT PART_NO AS COLUMN0, PART_DESC AS COLUMN1, CONVERT(NVARCHAR(15),SAMP_SUBMIT_DATE,103) AS COLUMN2 FROM PRD_MAST WHERE ");
                sbsql.Append(" CONVERT(nvarchar(6), SAMP_SUBMIT_DATE, 112) = Convert(nvarchar(6),GETDATE(),112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetAwaitingPartNoAllocationSummary()
        {
            try
            {
                sbsql = new StringBuilder();
                //"AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),GETDATE(),112) "
                sbsql.Append("SELECT CI_REFERENCE as COLUMN0, PROD_DESC as COLUMN1, CONVERT(NVARCHAR(15),PART_NO_REQ_DATE,103) FROM DDCI_INFO WHERE ");
                sbsql.Append(" ALLOT_PART_NO = 1 ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetDummyData()
        {
            try
            {
                sbsql = new StringBuilder();
                //"AND CONVERT(nvarchar(6), ENQU_RECD_ON, 112) = Convert(nvarchar(6),GETDATE(),112) "
                sbsql.Append("SELECT CI_REFERENCE as COLUMN0, PROD_DESC as COLUMN1, CONVERT(NVARCHAR(15),PART_NO_REQ_DATE,103) FROM DDCI_INFO WHERE ");
                sbsql.Append(" 1=0 ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}
