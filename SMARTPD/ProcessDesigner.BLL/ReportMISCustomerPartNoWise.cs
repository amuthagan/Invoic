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
    public class ReportMISCustomerPartNoWise : Essential
    {
        public ReportMISCustomerPartNoWise(UserInformation userInformation)
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
                    lstEntity = lstEntity.GroupBy(row => ((row.CUST_DWG_NO == null || Convert.ToString(row.CUST_DWG_NO).Trim().Length == 0) ? null : Convert.ToString(row.CUST_DWG_NO).Trim())).Select(row => new DDCI_INFO()
                    {
                        CI_REFERENCE = row.First().CI_REFERENCE,
                        ENQU_RECD_ON = row.First().ENQU_RECD_ON,
                        FR_CS_DATE = row.First().FR_CS_DATE,
                        PROD_DESC = row.First().PROD_DESC,
                        CUST_CODE = row.First().CUST_CODE,
                        CUST_DWG_NO = ((row.First().CUST_DWG_NO == null || Convert.ToString(row.First().CUST_DWG_NO).Trim().Length == 0) ? null : Convert.ToString(row.First().CUST_DWG_NO).Trim()),
                        CUST_DWG_NO_ISSUE = row.First().CUST_DWG_NO_ISSUE,
                        EXPORT = row.First().EXPORT,
                        NUMBER_OFF = row.First().NUMBER_OFF,
                        POTENTIAL = row.First().POTENTIAL,
                        SFL_SHARE = row.First().SFL_SHARE,
                        REMARKS = row.First().REMARKS,
                        RESPONSIBILITY = row.First().RESPONSIBILITY,
                        PENDING = row.First().PENDING,
                        FEASIBILITY = row.First().FEASIBILITY,
                        REJECT_REASON = row.First().REJECT_REASON,
                        LOC_CODE = row.First().LOC_CODE,
                        CHEESE_WT = row.First().CHEESE_WT,
                        FINISH_WT = row.First().FINISH_WT,
                        FINISH_CODE = row.First().FINISH_CODE,
                        SUGGESTED_RM = row.First().SUGGESTED_RM,
                        RM_COST = row.First().RM_COST,
                        FINAL_COST = row.First().FINAL_COST,
                        COST_NOTES = row.First().COST_NOTES,
                        PROCESSED_BY = row.First().PROCESSED_BY,
                        ORDER_DT = row.First().ORDER_DT,
                        PRINT = row.First().PRINT,
                        ALLOT_PART_NO = row.First().ALLOT_PART_NO,
                        PART_NO_REQ_DATE = row.First().PART_NO_REQ_DATE,
                        CUST_STD_NO = row.First().CUST_STD_NO,
                        CUST_STD_DATE = row.First().CUST_STD_DATE,
                        AUTOPART = row.First().AUTOPART,
                        SAFTYPART = row.First().SAFTYPART,
                        APPLICATION = row.First().APPLICATION,
                        STATUS = row.First().STATUS,
                        CUSTOMER_NEED_DT = row.First().CUSTOMER_NEED_DT,
                        MKTG_COMMITED_DT = row.First().MKTG_COMMITED_DT,
                        PPAP_LEVEL = row.First().PPAP_LEVEL,
                        DEVL_METHOD = row.First().DEVL_METHOD,
                        PPAP_FORGING = row.First().PPAP_FORGING,
                        PPAP_SAMPLE = row.First().PPAP_SAMPLE,
                        PACKING = row.First().PACKING,
                        NATURE_PACKING = row.First().NATURE_PACKING,
                        SPL_CHAR = row.First().SPL_CHAR,
                        OTHER_CUST_REQ = row.First().OTHER_CUST_REQ,
                        ATP_DATE = row.First().ATP_DATE,
                        SIMILAR_PART_NO = row.First().SIMILAR_PART_NO,
                        GENERAL_REMARKS = row.First().GENERAL_REMARKS,
                        MONTHLY = row.First().MONTHLY,
                        MKTG_COMMITED_DATE = row.First().MKTG_COMMITED_DATE,
                        ROWID = row.First().ROWID,
                        DELETE_FLAG = row.First().DELETE_FLAG,
                        ENTERED_DATE = row.First().ENTERED_DATE,
                        ENTERED_BY = row.First().ENTERED_BY,
                        UPDATED_DATE = row.First().UPDATED_DATE,
                        UPDATED_BY = row.First().UPDATED_BY,
                        IDPK = row.First().IDPK,
                        COATING_CODE = row.First().COATING_CODE,
                        REALISATION = row.First().REALISATION,
                        NO_OF_PCS = row.First().NO_OF_PCS,
                        ZONE_CODE = row.First().ZONE_CODE,
                        RM_FACTOR = row.First().RM_FACTOR,
                        IS_COMBINED = row.First().IS_COMBINED,
                    }).OrderBy(row => ((row.CUST_DWG_NO == null || Convert.ToString(row.CUST_DWG_NO).Trim().Length == 0) ? null : Convert.ToString(row.CUST_DWG_NO).Trim())).Distinct<DDCI_INFO>().ToList<DDCI_INFO>();


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

        public System.Data.DataSet GetAllCustomerPartNos(PRD_MAST productMaster = null, DDCI_INFO customerInfo = null, DataRowView customerMaster = null)
        {

            System.Data.DataSet dsReport = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                sb.Append(" ROW_NUMBER() OVER(ORDER BY A.PART_NO) AS SNO, ");
                sb.Append("  a.PART_NO, ");
                sb.Append("  a.PART_DESC, ");
                //sb.Append("  a.SIM_TO_STD_CD, ");
                //sb.Append("  a.PRD_CLASS_CD, ");
                //sb.Append("  a.MFG_STD, ");
                //sb.Append("  a.ED_CD, ");
                //sb.Append("  a.THREAD_CD, ");
                //sb.Append("  a.DIA_CD, ");
                //sb.Append("  a.QUALITY, ");
                //sb.Append("  a.BIF_PROJ, ");
                //sb.Append("  a.BIF_FORECAST, ");
                //sb.Append("  a.FINISH_WT, ");
                //sb.Append("  a.FINISH_WT_EST, ");
                //sb.Append("  a.HEAT_TREATMENT_CD, ");
                //sb.Append("  a.HEAT_TREATMENT_DESC, ");
                //sb.Append("  a.PRD_GRP_CD, ");
                //sb.Append("  a.MACHINE_CD, ");
                //sb.Append("  a.ALLOT_DATE, ");
                //sb.Append("  a.DOC_REL_DATE, ");
                //sb.Append("  a.DOC_REL_REMARKS, ");
                //sb.Append("  a.FAMILY, ");
                //sb.Append("  a.HEAD_STYLE, ");
                //sb.Append("  a.TYPE, ");
                //sb.Append("  a.APPLICATION, ");
                //sb.Append("  a.USER_CD, ");
                //sb.Append("  a.THREAD_CLASS, ");
                //sb.Append("  a.THREAD_STD, ");
                //sb.Append("  a.PG_CATEGORY, ");
                //sb.Append("  a.NOS_PER_PACK, ");
                //sb.Append("  a.SAMP_SUBMIT_DATE, ");
                //sb.Append("  a.ALLOTTED_BY, ");
                //sb.Append("  a.PSW_ST, ");
                //sb.Append("  a.DOC_REL_TYPE, ");
                //sb.Append("  a.HOLD_ME, ");
                //sb.Append("  a.ADDL_FEATURE, ");
                //sb.Append("  a.KEYWORDS, ");
                //sb.Append("  a.CANCEL_STATUS, ");
                //sb.Append("  a.IDPK AS PRD_MAST_IDPK, ");
                //sb.Append("  a.PART_CONFIG_ID, ");
                //sb.Append("  a.LOC_CODE, ");
                //sb.Append("  C.CI_REFERENCE, ");
                //sb.Append("  C.ENQU_RECD_ON, ");
                //sb.Append("  C.FR_CS_DATE, ");
                //sb.Append("  C.PROD_DESC, ");
                //sb.Append("  C.CUST_CODE, ");
                sb.Append("  C.CUST_DWG_NO ");
                //sb.Append("  C.CUST_DWG_NO_ISSUE, ");
                //sb.Append("  C.EXPORT, ");
                //sb.Append("  C.NUMBER_OFF, ");
                //sb.Append("  C.POTENTIAL, ");
                //sb.Append("  C.SFL_SHARE, ");
                //sb.Append("  C.REMARKS, ");
                //sb.Append("  C.RESPONSIBILITY, ");
                //sb.Append("  C.PENDING, ");
                //sb.Append("  C.FEASIBILITY, ");
                //sb.Append("  C.REJECT_REASON, ");
                //sb.Append("  C.LOC_CODE, ");
                //sb.Append("  C.CHEESE_WT, ");
                //sb.Append("  C.FINISH_WT, ");
                //sb.Append("  C.FINISH_CODE, ");
                //sb.Append("  C.SUGGESTED_RM, ");
                //sb.Append("  C.RM_COST, ");
                //sb.Append("  C.FINAL_COST, ");
                //sb.Append("  C.COST_NOTES, ");
                //sb.Append("  C.PROCESSED_BY, ");
                //sb.Append("  C.ORDER_DT, ");
                //sb.Append("  C.ALLOT_PART_NO, ");
                //sb.Append("  C.PART_NO_REQ_DATE, ");
                //sb.Append("  C.CUST_STD_NO, ");
                //sb.Append("  C.CUST_STD_DATE, ");
                //sb.Append("  C.AUTOPART, ");
                //sb.Append("  C.SAFTYPART, ");
                //sb.Append("  C.APPLICATION, ");
                //sb.Append("  C.STATUS, ");
                //sb.Append("  C.CUSTOMER_NEED_DT, ");
                //sb.Append("  C.MKTG_COMMITED_DT, ");
                //sb.Append("  C.PPAP_LEVEL, ");
                //sb.Append("  C.DEVL_METHOD, ");
                //sb.Append("  C.PPAP_FORGING, ");
                //sb.Append("  C.PPAP_SAMPLE, ");
                //sb.Append("  C.PACKING, ");
                //sb.Append("  C.NATURE_PACKING, ");
                //sb.Append("  C.SPL_CHAR, ");
                //sb.Append("  C.OTHER_CUST_REQ, ");
                //sb.Append("  C.ATP_DATE, ");
                //sb.Append("  C.SIMILAR_PART_NO, ");
                //sb.Append("  C.GENERAL_REMARKS, ");
                //sb.Append("  C.MONTHLY, ");
                //sb.Append("  C.MKTG_COMMITED_DATE, ");
                //sb.Append("  C.IDPK AS CI_IDPK, ");
                //sb.Append("  C.COATING_CODE, ");
                //sb.Append("  C.REALISATION, ");
                //sb.Append("  C.NO_OF_PCS, ");
                //sb.Append("  C.ZONE_CODE, ");
                //sb.Append("  C.RM_FACTOR, ");
                //sb.Append("  C.IS_COMBINED ");
                //sb.Append("FROM prd_mast a ");
                //sb.Append("LEFT OUTER JOIN prd_ciref b ");
                //sb.Append("ON a.part_no = b.part_no ");
                //sb.Append("LEFT OUTER JOIN ddci_info c ");
                //sb.Append("ON b.ci_ref = c.ci_reference");
                //sb.Append(" where  b.CURRENT_CIREF = 1 ");
                sb.Append("FROM prd_mast a ,prd_ciref b ,ddci_info c where a.part_no = b.part_no and b.ci_ref=c.ci_reference and b.CURRENT_CIREF = 1");

                if (customerInfo.IsNotNullOrEmpty() && customerInfo.CUST_DWG_NO.ToValueAsString().Trim().Length > 0)
                {
                    sb.Append(" and RTRIM(LTRIM(cust_dwg_no)) like '" + customerInfo.CUST_DWG_NO.ToValueAsString() + "%'");
                }

                if (productMaster.IsNotNullOrEmpty() && productMaster.PART_NO.ToValueAsString().Trim().Length > 0)
                {
                    sb.Append(" and RTRIM(LTRIM(a.part_no)) like '" + productMaster.PART_NO.ToValueAsString() + "%'");
                }

                if (customerMaster.IsNotNullOrEmpty() && customerMaster["CUST_CODE"].ToValueAsString().Trim().Length > 0)
                {
                    sb.Append(" and RTRIM(LTRIM(cust_code)) = '" + customerMaster["CUST_CODE"].ToValueAsString() + "'");
                }

                sb.Append(" order by a.PART_NO");

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
