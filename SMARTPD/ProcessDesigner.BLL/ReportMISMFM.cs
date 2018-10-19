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
    public class ReportMISMFM : Essential
    {
        public ReportMISMFM(UserInformation userInformation)
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

        public List<DDCUST_MAST> GetCustomerDetails(DDCUST_MAST paramEntity = null)
        {

            List<DDCUST_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.CUST_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDCUST_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.CUST_CODE == paramEntity.CUST_CODE
                                 orderby row.CUST_CODE ascending
                                 select row).ToList<DDCUST_MAST>();
                }
                else
                {

                    lstEntity = (from row in DB.DDCUST_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 orderby row.CUST_CODE ascending
                                 select row).ToList<DDCUST_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDCI_INFO> GetCustomerPartNumber(DDCI_INFO paramEntity = null)
        {

            List<DDCI_INFO> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDCI_INFO
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.IDPK == paramEntity.IDPK
                                 select row).ToList<DDCI_INFO>();
                }
                else
                {

                    lstEntity = (from row in DB.DDCI_INFO
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDCI_INFO>();
                }
                if (lstEntity.IsNotNullOrEmpty())
                    lstEntity = lstEntity.GroupBy(row => row.CUST_DWG_NO).Select(row => row.First()).OrderBy(row => row.CUST_DWG_NO).Distinct<DDCI_INFO>().ToList<DDCI_INFO>();


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PRD_MAST> GetPartNumber(PRD_MAST paramEntity = null)
        {

            List<PRD_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.IDPK == paramEntity.IDPK
                                 select row).ToList<PRD_MAST>();
                }
                else
                {

                    lstEntity = (from row in DB.PRD_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<PRD_MAST>();
                }
                if (lstEntity.IsNotNullOrEmpty())
                    lstEntity = lstEntity.GroupBy(row => row.PART_NO).Select(row => row.First()).Distinct<PRD_MAST>().OrderBy(row => row.PART_NO).ToList<PRD_MAST>();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public System.Data.DataSet GetAllMFM(string startDate, string endDate, PRD_MAST productMaster = null, DDCI_INFO customerInfo = null, DDCUST_MAST customerMaster = null)
        {

            System.Data.DataSet dsReport = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ROW_NUMBER() OVER(ORDER BY MFM_MAST.PART_NO) AS SNO, ");
                sb.Append("  MFM_MAST.PART_NO, ");
                sb.Append("  CAST(DOC_REL_DT_PLAN as varchar(10)) as DOC_REL_DT_PLAN, ");
                sb.Append("  CAST(DOC_REL_DT_ACTUAL as varchar(10)) as DOC_REL_DT_ACTUAL, ");
                sb.Append("  CAST(TOOLS_READY_DT_PLAN as varchar(10)) as TOOLS_READY_DT_PLAN, ");
                sb.Append("  CAST(TOOLS_READY_ACTUAL_DT as varchar(10)) as TOOLS_READY_ACTUAL_DT, ");
                sb.Append("  CAST(FORGING_PLAN_DT as varchar(10)) as FORGING_PLAN_DT, ");
                sb.Append("  CAST(FORGING_ACTUAL_DT as varchar(10)) as FORGING_ACTUAL_DT, ");
                sb.Append("  CAST(SECONDARY_PLAN_DT as varchar(10)) as SECONDARY_PLAN_DT, ");
                sb.Append("  CAST(SECONDARY_ACTUAL_DT as varchar(10)) as SECONDARY_ACTUAL_DT, ");
                sb.Append("  CAST(HEAT_TREATMENT_PLAN_DT as varchar(10)) as HEAT_TREATMENT_PLAN_DT, ");
                sb.Append("  CAST(HEAT_TREATMENT_ACTUAL as varchar(10)) as HEAT_TREATMENT_ACTUAL, ");
                sb.Append("  CAST(ISSR_PLAN_DT as varchar(10)) as ISSR_PLAN_DT, ");
                sb.Append("  CAST(ISSR_ACTUAL_DT as varchar(10)) as ISSR_ACTUAL_DT, ");
                sb.Append("  CAST(PPAP_PLAN as varchar(10)) as PPAP_PLAN, ");
                sb.Append("  CAST(PPAP_ACTUAL_DT as varchar(10)) as PPAP_ACTUAL_DT, ");
                sb.Append("  CAST(PSW_DATE as varchar(10)) as PSW_DATE, ");
                sb.Append("  SAMPLE_QTY, ");
                sb.Append("  RESP ");
                sb.Append("FROM MFM_MAST ");

                if (customerMaster.IsNotNullOrEmpty() && customerMaster.CUST_CODE.ToValueAsString().Trim().Length > 0)
                {
                    sb.Append(" , ");
                    sb.Append("  prd_mast, ");
                    sb.Append("  prd_ciref, ");
                    sb.Append("  ddci_info ");
                }
                sb.Append(" WHERE 1 = 1 ");
                sb.Append(" AND DOC_REL_DT_PLAN BETWEEN  " + "'" + startDate + "'" + "  AND  " + "'" + endDate + "'");

                if (customerMaster.IsNotNullOrEmpty() && customerMaster.CUST_CODE.ToValueAsString().Trim().Length > 0)
                {

                    sb.Append(" AND prd_ciref.part_no    = prd_mast.part_no ");
                    sb.Append(" AND mfm_mast.part_no       = prd_mast.part_no ");

                    sb.Append(" AND ddci_info.ci_reference = prd_ciref.ci_ref AND prd_ciref.CURRENT_CIREF = 1 ");
                    //sb.Append(" AND ddci_info.ci_reference = prd_ciref.ci_ref");

                    sb.Append(" AND cust_code = '" + customerMaster.CUST_CODE.ToValueAsString() + "'");
                }

                if (productMaster.IsNotNullOrEmpty() && productMaster.PART_NO.ToValueAsString().Trim().Length > 0)
                {
                    sb.Append(" and MFM_MAST.part_no like '" + productMaster.PART_NO.ToValueAsString() + "%'");
                }

                sb.Append(" order by MFM_MAST.PART_NO");

                List<StringBuilder> sqlList = new List<StringBuilder>() { sb };

                dsReport = Dal.GetDataSet(sqlList);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                    dsReport.Tables[0].TableName = "PRD_MAST_CI";

                DataTable dtCompany = new DataTable();
                dtCompany.TableName = "CompanyName";
                dtCompany.Columns.Add("Name");
                dtCompany.Columns.Add("ShortName");
                dtCompany.Columns.Add("Phone");
                dtCompany.Columns.Add("Fax");
                dtCompany.Columns.Add("Mobile");
                dtCompany.Columns.Add("EMail");
                dtCompany.Columns.Add("Title");
                dtCompany.Columns.Add("ReportTitle");
                if (dsReport.IsNotNullOrEmpty())
                    dsReport.Tables.Add(dtCompany);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return dsReport;
        }
    }
}
