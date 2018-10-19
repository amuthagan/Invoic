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
    public class FlxReportsBll : Essential
    {
        StringBuilder sbSql;
        public FlxReportsBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }


        public DataView PPAPSubmitted(string selUserName, string startDate, string endDate)
        {
            try
            {
                sbSql = new StringBuilder();
                //sbSql.Append("select part_no as COLUMN0,part_desc as COLUMN1,bif_proj as COLUMN2 from prd_mast where samp_submit_date is not null and ");
                //sbSql.Append(" samp_submit_date  >= convert(date,'" + startDate + "',103) ");
                //sbSql.Append(" and allot_date <= convert(date,'" + endDate + "',103) and part_no in ");
                //sbSql.Append(" ( select part_no from pccs_issue where (part_no,issue_date) in ");
                //sbSql.Append(" ( select part_no,max(issue_date) from pccs_issue where upper(compiled_by) ");
                //sbSql.Append(" =upper('" + selUserName + "') group by part_no ))");

                sbSql.Append(" select part_no as COLUMN0,part_desc as COLUMN1,bif_proj as COLUMN2 from prd_mast where samp_submit_date is not null and  ");
                sbSql.Append(" samp_submit_date >= convert(date,'" + startDate + "',103) ");
                sbSql.Append(" and allot_date <= convert(date,'" + endDate + "',103) and part_no in ");
                sbSql.Append(" (select a.part_no from pccs_issue a ");
                sbSql.Append(" inner join ");
                sbSql.Append(" ( ");
                sbSql.Append(" select part_no,max(issue_date) as issue_date from pccs_issue where upper(compiled_by) ");
                sbSql.Append(" =upper('" + selUserName + "') group by part_no ");
                sbSql.Append(" )  b ");
                sbSql.Append(" on  a.PART_NO = b.PART_NO ");
                sbSql.Append(" and a.issue_date=b.issue_date) ");

                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbSql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView DocumentReleased(string selUserName, string startDate, string endDate)
        {
            try
            {
                sbSql = new StringBuilder();
                sbSql.Append(" select part_no as COLUMN0,part_desc as COLUMN1,bif_proj as COLUMN2 from prd_mast where doc_rel_date is not null and  ");
                sbSql.Append(" doc_rel_date >= convert(date,'" + startDate + "',103) ");
                sbSql.Append(" and allot_date <= convert(date,'" + endDate + "',103) and part_no in ");
                sbSql.Append(" (select a.part_no from pccs_issue a ");
                sbSql.Append(" inner join ");
                sbSql.Append(" ( ");
                sbSql.Append(" select part_no,max(issue_date) as issue_date from pccs_issue where upper(compiled_by) ");
                sbSql.Append(" =upper('" + selUserName + "') group by part_no ");
                sbSql.Append(" )  b ");
                sbSql.Append(" on  a.PART_NO = b.PART_NO ");
                sbSql.Append(" and a.issue_date=b.issue_date) ");

                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbSql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView PartNoAllotted(string selUserName, string startDate, string endDate)
        {
            try
            {
                sbSql = new StringBuilder();
                sbSql.Append(" select part_no as COLUMN0,part_desc as COLUMN1,bif_proj as COLUMN2 from prd_mast where allot_date is not null and  ");
                sbSql.Append(" convert(date,allot_date) >= convert(date,'" + startDate + "',103) ");
                sbSql.Append(" and convert(date,allot_date) <= convert(date,'" + endDate + "',103) and part_no in ");
                sbSql.Append(" (select a.part_no from pccs_issue a ");
                sbSql.Append(" inner join ");
                sbSql.Append(" ( ");
                sbSql.Append(" select part_no,max(issue_date) as issue_date from pccs_issue where upper(compiled_by) ");
                sbSql.Append(" =upper('" + selUserName + "') group by part_no ");
                sbSql.Append(" )  b ");
                sbSql.Append(" on  a.PART_NO = b.PART_NO ");
                sbSql.Append(" and a.issue_date=b.issue_date) ");

                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbSql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<SEC_USER_MASTER> GetUserList()
        {
            try
            {
                return ((from c in DB.SEC_USER_MASTER
                         where (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                         select c).ToList<SEC_USER_MASTER>());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
