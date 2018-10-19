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
    public class ReportOTPMCharts : Essential
    {
        public ReportOTPMCharts(UserInformation userInformation)
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

        public List<MFM_MAST> GetAllLeadTime()
        {
            List<MFM_MAST> lstEntity = null;
            System.Data.DataTable dtReport = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT b.PG_CATEGORY AS PG, ");
                sb.Append("  a.DOC_REL_DT_ACTUAL, ");
                sb.Append("  a.PPAP_ACTUAL_DT, ");
                sb.Append("  a.PART_NO, ");
                sb.Append("  b.SAMP_SUBMIT_DATE ");
                sb.Append("FROM dbo.PRD_MAST            AS b ");
                sb.Append("LEFT OUTER JOIN dbo.MFM_MAST AS a ");
                sb.Append("ON b.PART_NO             = a.PART_NO ");
                sb.Append("WHERE b.PG_CATEGORY     IN ( '1', '2', '3', '3F' ) ");
                sb.Append("AND b.ALLOT_DATE        IS NOT NULL ");
                sb.Append("AND a.DOC_REL_DT_ACTUAL IS NOT NULL ");
                sb.Append("AND b.SAMP_SUBMIT_DATE IS NOT NULL");

                dtReport = Dal.GetDataTable(sb);
                if (dtReport.IsNotNullOrEmpty())
                {
                    dtReport.TableName = "MFM_MAST";
                    lstEntity = (from row in dtReport.AsEnumerable()
                                 select new MFM_MAST()
                                 {
                                     PART_NO = Convert.ToString(row.Field<string>("PART_NO")),
                                     RESP = Convert.ToString(row.Field<string>("PG")),
                                     DOC_REL_DT_ACTUAL = row.Field<DateTime?>("DOC_REL_DT_ACTUAL"),
                                     PPAP_ACTUAL_DT = row.Field<DateTime?>("PPAP_ACTUAL_DT"),
                                     ENTERED_DATE = row.Field<DateTime?>("SAMP_SUBMIT_DATE"),
                                 }).ToList<MFM_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<MFM_MAST> GetAllPlanAdherence()
        {
            List<MFM_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.MFM_MAST
                             where row.PPAP_ACTUAL_DT != null && row.STATUS == null
                             select row).ToList<MFM_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PRD_MAST> GetAllFirstTimeRight()
        {

            List<PRD_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.PRD_MAST
                             where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.ALLOT_DATE != null && row.DOC_REL_DATE != null && row.SAMP_SUBMIT_DATE != null
                             select row).ToList<PRD_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

    }
}
