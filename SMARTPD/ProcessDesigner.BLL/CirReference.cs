using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.Common;

namespace ProcessDesigner.BLL
{
   public class CirReference : Essential
    {
        public DataTable dtFinish { get; set; }
        public DataTable dtCoating { get; set; }
        public DataTable ForecastLocation { get; set; }
        StringBuilder sqlQry = new StringBuilder();

        public CirReference(UserInformation userInformation)
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
        public int getCIRRefNo(ref DataTable dtIrRefNo, string zone)
        {
            try
            {
                sqlQry.Append(" select replace(New_ci_reference,' ','')  as New_ci_reference from (");
                sqlQry.Append(" SELECT '" + zone + "' ||SUBSTR(ci_reference,2,6) ||TO_CHAR(MAX(SUBSTR(ci_reference,8,3)) + 1,'000') AS New_ci_reference");
                sqlQry.Append(" FROM ddci_info WHERE SUBSTR(ci_reference,1,1)='" + zone + "' AND SUBSTR(ci_reference,2,6)  = (SELECT TO_CHAR(to_date(sysdate,'dd/mm/yyyy'),'yymmdd') FROM DUAL)");
                sqlQry.Append(" GROUP BY ci_reference)");

                dtIrRefNo = Dal.GetDataTable(sqlQry);
                if ((dtIrRefNo == null) || (dtIrRefNo.Rows.Count == 0))
                {
                    sqlQry.Append("SELECT ('" + zone + "'|| (SELECT TO_CHAR(to_date(sysdate,'dd/mm/yyyy'),'yymmdd')FROM DUAL )||'001') as New_ci_reference FROM dual");
                    dtIrRefNo = Dal.GetDataTable(sqlQry);
                }

                return 0;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -255;
            }
        }
        public int getdtFinish(ref DataTable dtFinish)
        {
            try
            {
                sqlQry.Append("  select finish_code,finish_desc  from ddfinish_mast order by finish_code asc ");
                dtFinish = Dal.GetDataTable(sqlQry);
                return 0;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -255;
            }
        }
        public int getdtCoating(ref DataTable dtCoating)
        {
            try
            {
                sqlQry.Append("  select coating_code,coating_desc from ddcoating_mast  order by coating_code asc ");
                dtCoating = Dal.GetDataTable(sqlQry);
                return 0;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -255;
            }
        }
        public int getCustomer(ref DataTable dtCustomer)
        {
            try
            {
                sqlQry.Append("  select cust_code,cust_name from ddcust_mast ");
                dtCustomer = Dal.GetDataTable(sqlQry);
                return 0;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -255;
            }
        }
    }
}
