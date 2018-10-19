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
    public class ReportPendingPartNumberStatus : Essential
    {
        public ReportPendingPartNumberStatus(UserInformation userInformation)
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

        //public List<MFM_MAST> GetAllPendingPartNumber(DateTime plannedDocumentReleaseDate)
        //{
        //    List<MFM_MAST> lstEntity = null;
        //    System.Data.DataTable dtReport = null;
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append("SELECT TOP 9223372036854775807 ");
        //        sb.Append("WITH TIES A.PART_NO, ");
        //        sb.Append("  A.DOC_REL_DT_PLAN, ");
        //        sb.Append("  A.DOC_REL_DT_ACTUAL, ");
        //        sb.Append("  A.TOOLS_READY_DT_PLAN, ");
        //        sb.Append("  A.TOOLS_READY_ACTUAL_DT, ");
        //        sb.Append("  A.FORGING_PLAN_DT, ");
        //        sb.Append("  A.FORGING_ACTUAL_DT, ");
        //        sb.Append("  A.SECONDARY_PLAN_DT, ");
        //        sb.Append("  A.SECONDARY_ACTUAL_DT, ");
        //        sb.Append("  A.HEAT_TREATMENT_PLAN_DT, ");
        //        sb.Append("  A.HEAT_TREATMENT_ACTUAL, ");
        //        sb.Append("  A.ISSR_PLAN_DT, ");
        //        sb.Append("  A.ISSR_ACTUAL_DT, ");
        //        sb.Append("  A.PPAP_PLAN, ");
        //        sb.Append("  A.PPAP_ACTUAL_DT, ");
        //        sb.Append("  C.COMPILED_BY, ");
        //        sb.Append("  NULL                            AS REMARKS ");
        //        sb.Append("FROM dbo.MFM_MAST                 AS A ");
        //        sb.Append("LEFT OUTER JOIN dbo.PROCESS_ISSUE AS C ");
        //        sb.Append("ON A.PART_NO   = C.PART_NO ");
        //        sb.Append("AND C.ISSUE_NO = 1, ");
        //        sb.Append("  dbo.PRD_MAST AS B ");
        //        sb.Append("WHERE A.DOC_REL_DT_PLAN >= '" + plannedDocumentReleaseDate.ToString("dd-MMM-yyyy") + "' ");
        //        sb.Append("AND A.PPAP_ACTUAL_DT    IS NULL ");
        //        sb.Append("AND A.PPAP_PLAN         IS NOT NULL ");
        //        sb.Append("AND A.STATUS            IS NULL ");
        //        sb.Append("AND A.PART_NO            = B.PART_NO ");
        //        sb.Append("ORDER BY A.DOC_REL_DT_PLAN");
        //        dtReport = Dal.GetDataTable(sb);
        //        if (dtReport.IsNotNullOrEmpty())
        //        {
        //            dtReport.TableName = "MFM_MAST";
        //            lstEntity = (from row in dtReport.AsEnumerable()
        //                         select new MFM_MAST()
        //                         {
        //                             PART_NO = row.Field<string>("PART_NO"),
        //                             DOC_REL_DT_PLAN = row.Field<DateTime?>("DOC_REL_DT_PLAN"),
        //                             DOC_REL_DT_ACTUAL = row.Field<DateTime?>("DOC_REL_DT_ACTUAL"),
        //                             TOOLS_READY_DT_PLAN = row.Field<DateTime?>("TOOLS_READY_DT_PLAN"),
        //                             TOOLS_READY_ACTUAL_DT = row.Field<DateTime?>("TOOLS_READY_ACTUAL_DT"),
        //                             FORGING_PLAN_DT = row.Field<DateTime?>("FORGING_PLAN_DT"),
        //                             FORGING_ACTUAL_DT = row.Field<DateTime?>("FORGING_ACTUAL_DT"),
        //                             SECONDARY_PLAN_DT = row.Field<DateTime?>("SECONDARY_PLAN_DT"),
        //                             SECONDARY_ACTUAL_DT = row.Field<DateTime?>("SECONDARY_ACTUAL_DT"),
        //                             HEAT_TREATMENT_PLAN_DT = row.Field<DateTime?>("HEAT_TREATMENT_PLAN_DT"),
        //                             HEAT_TREATMENT_ACTUAL = row.Field<DateTime?>("HEAT_TREATMENT_ACTUAL"),
        //                             ISSR_PLAN_DT = row.Field<DateTime?>("ISSR_PLAN_DT"),
        //                             ISSR_ACTUAL_DT = row.Field<DateTime?>("ISSR_ACTUAL_DT"),
        //                             PPAP_PLAN = row.Field<DateTime?>("PPAP_PLAN"),
        //                             PPAP_ACTUAL_DT = row.Field<DateTime?>("PPAP_ACTUAL_DT"),
        //                             SAMPLE_QTY = row.Field<decimal>("SAMPLE_QTY"),
        //                             REMARKS = row.Field<string>("REMARKS"),
        //                             RESP = row.Field<string>("COMPILED_BY"),
        //                             PSW_DATE = row.Field<DateTime?>("PSW_DATE"),
        //                             STATUS = row.Field<decimal?>("STATUS"),
        //                             HOLDME = row.Field<decimal?>("HOLDME"),
        //                             TIME_BOGAUGE_PLAN = row.Field<DateTime?>("TIME_BOGAUGE_PLAN"),
        //                             TIME_BOGAUGE_ACTUAL = row.Field<DateTime?>("TIME_BOGAUGE_ACTUAL"),
        //                             ROWID = row.Field<Guid>("ROWID"),
        //                             DELETE_FLAG = row.Field<bool?>("DELETE_FLAG"),
        //                             ENTERED_DATE = row.Field<DateTime?>("ENTERED_DATE"),
        //                             ENTERED_BY = row.Field<string>("ENTERED_BY"),
        //                             UPDATED_DATE = row.Field<DateTime?>("UPDATED_DATE"),
        //                             UPDATED_BY = row.Field<string>("UPDATED_BY"),
        //                             IDPK = row.Field<int>("IDPK"),
        //                         }).ToList<MFM_MAST>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //    return lstEntity;
        //}

        public System.Data.DataTable GetAllPendingPartNumber(DateTime plannedDocumentReleaseDate)
        {
            System.Data.DataTable dtReport = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT TOP 9223372036854775807 ");
                sb.Append("WITH TIES A.PART_NO, ");
                sb.Append("  A.DOC_REL_DT_PLAN, ");
                sb.Append("  A.DOC_REL_DT_ACTUAL, ");
                sb.Append("  A.TOOLS_READY_DT_PLAN, ");
                sb.Append("  A.TOOLS_READY_ACTUAL_DT, ");
                sb.Append("  A.FORGING_PLAN_DT, ");
                sb.Append("  A.FORGING_ACTUAL_DT, ");
                sb.Append("  A.SECONDARY_PLAN_DT, ");
                sb.Append("  A.SECONDARY_ACTUAL_DT, ");
                sb.Append("  A.HEAT_TREATMENT_PLAN_DT, ");
                sb.Append("  A.HEAT_TREATMENT_ACTUAL, ");
                sb.Append("  A.ISSR_PLAN_DT, ");
                sb.Append("  A.ISSR_ACTUAL_DT, ");
                sb.Append("  A.PPAP_PLAN, ");
                sb.Append("  A.PPAP_ACTUAL_DT, ");
                sb.Append("  ( ");
                sb.Append("  CASE B.HOLD_ME ");
                sb.Append("    WHEN 1 ");
                sb.Append("    THEN 'HOLD' ");
                sb.Append("  END) AS HOLD_ME, ");
                sb.Append("  ( ");
                sb.Append("  CASE B.CANCEL_STATUS ");
                sb.Append("    WHEN 1 ");
                sb.Append("    THEN 'CANCELLED' ");
                sb.Append("  END) AS CANCEL_STATUS, ");
                sb.Append("  C.COMPILED_BY, ");
                sb.Append("  REMARKS, ");
                sb.Append("  CONVERT(datetime,'" + plannedDocumentReleaseDate.ToString("dd-MMM-yyyy") + "',103) AS USR_INPUT_DOC_REL_DT_PLAN,");
                sb.Append("   B.IDPK");
                sb.Append("  FROM dbo.MFM_MAST                 AS A ");
                sb.Append("  LEFT OUTER JOIN dbo.PROCESS_ISSUE AS C ");
                sb.Append("    ON A.PART_NO   = C.PART_NO ");
                sb.Append("    AND C.ISSUE_NO = '1', ");
                sb.Append("  dbo.PRD_MAST AS B ");
                sb.Append("  WHERE A.DOC_REL_DT_PLAN >= '" + plannedDocumentReleaseDate.ToString("dd-MMM-yyyy") + "' ");
                sb.Append("  AND A.PPAP_ACTUAL_DT    IS NULL ");
                sb.Append("  AND A.PPAP_PLAN         IS NOT NULL ");
                sb.Append("  AND A.STATUS            IS NULL ");
                sb.Append("  AND A.PART_NO            = B.PART_NO ");
                sb.Append("  ORDER BY A.DOC_REL_DT_PLAN");
                dtReport = Dal.GetDataTable(sb);
                if (dtReport.IsNotNullOrEmpty())
                {
                    dtReport.TableName = "MFM_MAST";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return dtReport;
        }

    }
}
