using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class ECNPCNBll : Essential
    {
        StringBuilder sbSql;
        public ECNPCNBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataView GetECNDetails(string startDate, string endDate)
        {
            try
            {
                sbSql = new StringBuilder();
                sbSql.Append("select part_no as COLUMN0, part_desc as COLUMN1,convert(varchar(50),sfl_draw_issueno1) as COLUMN2,convert(varchar,sfl_draw_issuedate1,103) as COLUMN3 from dd_pcn where ");
                sbSql.Append(" sfl_draw_issuedate1 ");
                sbSql.Append(" between convert(date,'" + startDate + "',103) and convert(date,'" + endDate + "',103) ");
                sbSql.Append("and requsted_by='CUSTOMER' order by part_no");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbSql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetPCNDetails(string startDate, string endDate)
        {
            try
            {
                sbSql = new StringBuilder();
                //sql_qry = "select part_no, part_desc,sfl_draw_issueno,sfl_draw_issue_date from dd_pcn where sfl_draw_issue_date between TO_DATE('" & txtStartDate & "', 'dd/mm/yyyy') AND TO_DATE('" & txtEndDate & "', 'dd/mm/yyyy') and requsted_by='SFL' order by part_no "
                sbSql.Append("select part_no as COLUMN0, part_desc as COLUMN1, convert(varchar(50),sfl_draw_issueno) as COLUMN2,convert(varchar,sfl_draw_issue_date,103) as COLUMN3 from dd_pcn where ");
                sbSql.Append(" sfl_draw_issue_date ");
                sbSql.Append(" between convert(date,'" + startDate + "',103) and convert(date,'" + endDate + "',103) ");
                sbSql.Append("and requsted_by='SFL' order by part_no");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbSql.ToString()).ToList());
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}
