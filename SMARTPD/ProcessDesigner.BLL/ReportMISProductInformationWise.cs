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
    public class ReportMISProductInformationWise : Essential
    {

        public ReportMISProductInformationWise(UserInformation userInformation)
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

        public System.Data.DataSet GetAllPartNos(PRD_MAST productMaster = null, DDCUST_MAST customerMaster = null, List<DDLOC_MAST> lstLocation = null)
        {

            System.Data.DataSet dsReport = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT ROW_NUMBER() OVER(ORDER BY M.PART_NO) AS SNO, ");
                sb.Append("  M.PART_NO, ");
                sb.Append("  P.PART_DESC, ");
                sb.Append("  T.CUST_NAME, ");
                sb.Append("  P.BIF_PROJ, ");
                sb.Append("  P.BIF_FORECAST, ");
                sb.Append("  P.FINISH_WT, ");
                sb.Append("  M.CHEESE_WT, ");
                sb.Append("  M.RM_WT, ");
                sb.Append("  P.PG_CATEGORY, ");
                sb.Append("  P.QUALITY, ");
                sb.Append("  P.SAMP_SUBMIT_DATE, ");
                sb.Append("  P.PSW_ST, ");
                sb.Append("  C.CUST_DWG_NO, ");
                sb.Append("  P.DOC_REL_DATE ");
                sb.Append("FROM dbo.PRD_MAST             AS P ");
                sb.Append("LEFT OUTER JOIN dbo.PRD_CIREF AS F ");
                sb.Append("ON F.PART_NO = P.PART_NO ");
                sb.Append("LEFT OUTER JOIN dbo.DDCI_INFO AS C ");
                sb.Append("ON C.CI_REFERENCE = F.CI_REF, ");
                sb.Append("  dbo.PROCESS_MAIN AS M, ");
                sb.Append("  dbo.DDCUST_MAST  AS T ");
                sb.Append("WHERE P.PART_NO    = M.PART_NO ");
                sb.Append("AND M.CURRENT_PROC = 1 ");
                sb.Append("AND T.CUST_CODE    = C.CUST_CODE ");

                if (customerMaster.IsNotNullOrEmpty() && customerMaster.CUST_CODE.IsNotNullOrEmpty() && customerMaster.CUST_CODE != -999999)
                {
                    sb.Append(" AND C.CUST_CODE    = '" + customerMaster.CUST_CODE + "' ");
                }

                if (productMaster.IsNotNullOrEmpty())
                {
                    if (productMaster.PART_NO.IsNotNullOrEmpty())
                        sb.Append(" AND P.PART_NO LIKE '" + productMaster.PART_NO + "%' ");

                    if (productMaster.PART_DESC.IsNotNullOrEmpty())
                        sb.Append(" AND upper(P.PART_DESC) LIKE '%" + productMaster.PART_DESC.ToUpper() + "%' ");

                    if (productMaster.QUALITY.IsNotNullOrEmpty())
                        sb.Append(" AND P.QUALITY LIKE '" + productMaster.QUALITY + "%' ");

                    if (productMaster.PG_CATEGORY.IsNotNullOrEmpty())
                        sb.Append(" AND P.PG_CATEGORY = '" + productMaster.PG_CATEGORY + "' ");

                    if (productMaster.PSW_ST.IsNotNullOrEmpty() && (productMaster.PSW_ST.ToUpper() == "YES" || productMaster.PSW_ST.ToUpper() == "NO"))
                        sb.Append(" AND P.PSW_ST = '" + productMaster.PSW_ST + "' ");

                    if (productMaster.PSW_ST.IsNotNullOrEmpty() && (productMaster.PSW_ST.ToUpper() == "NONE"))
                        sb.Append(" AND P.PSW_ST IS NULL ");

                }

                if (lstLocation.IsNotNullOrEmpty() && lstLocation.Count > 0 && lstLocation.Count < 4)
                {
                    string locsql = "";
                    foreach (DDLOC_MAST loc in lstLocation)
                    {
                        locsql = locsql + "(P.BIF_PROJ LIKE '" + loc.LOC_CODE + "%' OR P.BIF_FORECAST LIKE '" + loc.LOC_CODE + "%')";
                        if (lstLocation[lstLocation.Count - 1].GetHashCode() != loc.GetHashCode())
                        {
                            locsql = locsql + " OR ";
                        }
                    }
                    sb.Append(" AND (" + locsql + ") ");

                }

                sb.Append(" order by M.PART_NO");

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

        /// <summary>
        /// Get the Locations
        /// </summary>
        /// <param name="locationCode">Code of the Location </param>
        /// <returns>Returns All location if location code is not specified. otherwise return only specified location</returns>
        public List<DDLOC_MAST> GetLocationDetails(DDLOC_MAST paramEntity = null)
        {
            List<DDLOC_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDLOC_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.LOC_CODE == paramEntity.LOC_CODE
                                 orderby row.LOCATION ascending
                                 select row).ToList<DDLOC_MAST>();

                }
                else
                {

                    lstResult = (from row in DB.DDLOC_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 orderby row.LOCATION ascending
                                 select row).ToList<DDLOC_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<PG_CATEGORY> GetProductPGCategoryByPrimaryKey(PG_CATEGORY paramEntity = null)
        {

            List<PG_CATEGORY> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PG_CAT.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PG_CATEGORY
                                 where row.PG_CAT == paramEntity.PG_CAT
                                 select row).ToList<PG_CATEGORY>();
                }
                else
                {

                    lstEntity = (from row in DB.PG_CATEGORY
                                 select row).ToList<PG_CATEGORY>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

    }
}
