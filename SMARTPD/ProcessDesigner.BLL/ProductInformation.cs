using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.DAL;
using System.ComponentModel;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class ProductInformation : Essential, IDataManipulation
    {
        public ProductInformation(UserInformation userInformation)
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

        public List<PartNumberConfig> GetPartNumberConfigByPrimaryKey(PartNumberConfig partNoConfig = null)
        {

            List<PartNumberConfig> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (partNoConfig.IsNotNullOrEmpty() && partNoConfig.Code.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && Convert.ToBoolean(Convert.ToInt16(row.IsObsolete)) == false && row.ID == partNoConfig.ID
                                 orderby row.ID, row.CurrentValue descending
                                 select row).ToList<PartNumberConfig>();
                }
                else
                {

                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && Convert.ToBoolean(Convert.ToInt16(row.IsObsolete)) == false
                                 orderby row.ID, row.CurrentValue descending
                                 select row).ToList<PartNumberConfig>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PRD_MAST> GetEntityByPrimaryKey(PRD_MAST paramEntity = null)
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

                    //lstEntity = (from row in DB.PRD_MAST
                    //             where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                    //             select row).ToList<PRD_MAST>();
                    lstEntity = DB.ExecuteQuery<PRD_MAST>("select * from PRD_MAST where DELETE_FLAG = 0 or DELETE_FLAG is null").ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public PRD_MAST GetEntityByPartNumber(PRD_MAST paramEntity = null)
        {

            List<PRD_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_MAST
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<PRD_MAST>();
                }
                else
                {

                    lstEntity = (from row in DB.PRD_MAST
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false
                                 select row).ToList<PRD_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0 ? lstEntity[0] : null;
        }

        public PRD_MAST GetEntityByPartNumber(string param = null)
        {

            List<PRD_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                if (param.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_MAST
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.PART_NO == param
                                 select row).ToList<PRD_MAST>();
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0 ? lstEntity[0] : null;
        }

        public List<PRD_MANUFACTURING_STANDARD> GetManufacturingStandardByPrimaryKey(PRD_MANUFACTURING_STANDARD paramEntity = null)
        {

            List<PRD_MANUFACTURING_STANDARD> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_MANUFACTURING_STANDARD
                                 where row.IDPK == paramEntity.IDPK
                                 select row).ToList<PRD_MANUFACTURING_STANDARD>();
                }
                else
                {

                    lstEntity = (from row in DB.PRD_MANUFACTURING_STANDARD
                                 select row).ToList<PRD_MANUFACTURING_STANDARD>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PRD_THREAD_CODE> GetThreadCodeByPrimaryKey(PRD_THREAD_CODE paramEntity = null)
        {

            List<PRD_THREAD_CODE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_THREAD_CODE
                                 where row.IDPK == paramEntity.IDPK
                                 select row).ToList<PRD_THREAD_CODE>();
                }
                else
                {

                    lstEntity = (from row in DB.PRD_THREAD_CODE
                                 select row).ToList<PRD_THREAD_CODE>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<SIM_TO_STD_MAST> GetSimilarityByPrimaryKey(SIM_TO_STD_MAST paramEntity = null)
        {

            List<SIM_TO_STD_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.STS_CD.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.SIM_TO_STD_MAST
                                 where row.STS_CD == paramEntity.STS_CD
                                 select row).ToList<SIM_TO_STD_MAST>();
                }
                else
                {

                    lstEntity = (from row in DB.SIM_TO_STD_MAST
                                 select row).ToList<SIM_TO_STD_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<FASTENERS_MASTER> GetProductFamilyByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null)
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == null && row.SUBTYPE == null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {

                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == null && row.SUBTYPE == null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        /// <summary>
        /// Get the Locations
        /// </summary>
        /// <param name="locationCode">Code of the Location </param>
        /// <returns>Returns All location if location code is not specified. otherwise return only specified location</returns>
        public List<DDLOC_MAST> GetLocationByPrimaryKey(DDLOC_MAST paramEntity = null)
        {
            List<DDLOC_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDLOC_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.LOC_CODE == paramEntity.LOC_CODE
                                 select row).ToList<DDLOC_MAST>();

                }
                else
                {

                    lstResult = (from row in DB.DDLOC_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDLOC_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<HEAT_TREAT_MAST> GetHeatTreatmentByPrimaryKey(HEAT_TREAT_MAST paramEntity = null)
        {
            List<HEAT_TREAT_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.HEAT_TREAT_MAST
                                 where row.HT_CD == paramEntity.HT_CD
                                 select row).ToList<HEAT_TREAT_MAST>();

                }
                else
                {

                    lstResult = (from row in DB.HEAT_TREAT_MAST
                                 select row).ToList<HEAT_TREAT_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<PRD_PSW_APPROVED> GetPSWApprovedByPrimaryKey(PRD_PSW_APPROVED paramEntity = null)
        {

            List<PRD_PSW_APPROVED> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_PSW_APPROVED
                                 where row.IDPK == paramEntity.IDPK
                                 select row).ToList<PRD_PSW_APPROVED>();
                }
                else
                {

                    lstEntity = (from row in DB.PRD_PSW_APPROVED
                                 select row).ToList<PRD_PSW_APPROVED>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public int GetForecastLocation(ref DataTable dtForecastLocation)
        {
            List<DDLOC_MAST> lstEntity = null;

            try
            {
                if (!DB.IsNotNullOrEmpty()) return -255;
                lstEntity = (from row in DB.DDLOC_MAST
                             where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null
                             select row).ToList<DDLOC_MAST>();

                if (!lstEntity.IsNotNullOrEmpty()) return -255;
                dtForecastLocation = lstEntity.ToDataTable<DDLOC_MAST>();
                return 0;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -255;
            }
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

        public List<FASTENERS_MASTER> GetProductKeywordsByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null)
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "KEYWORDS" && row.SUBTYPE != null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {

                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "KEYWORDS" && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<FASTENERS_MASTER> GetProductAdditionalFeatureByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null)
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "ADDITIONAL FEATURE" && row.SUBTYPE != null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {

                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "ADDITIONAL FEATURE" && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<FASTENERS_MASTER> GetProductDrivingFeatureByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null)
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "DRIVING FEATURE" && row.SUBTYPE != null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {

                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "DRIVING FEATURE" && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<FASTENERS_MASTER> GetProductEndFormByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null, string type = "END FORM")
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == type && row.SUBTYPE != null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == type && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<FASTENERS_MASTER> GetProductHeadFormByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null, string type = "HEAD FORMS")
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == type && row.SUBTYPE != null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == type && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<FASTENERS_MASTER> GetProductShankFormByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null, string type = "SHANK FORM")
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == type && row.SUBTYPE != null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == type && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<FASTENERS_MASTER> GetProductTypeByPrimaryKey(FASTENERS_MASTER paramEntity = null, string productCategory = null)
        {

            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PRD_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "TYPE" && row.SUBTYPE != null && row.PRD_CODE == paramEntity.PRD_CODE
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 where row.TYPE == "TYPE" && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }

                if (productCategory.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where row.PRODUCT_CATEGORY == productCategory
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PROCESS_ISSUE> GetProcessIssueByPrimaryKey(PROCESS_ISSUE paramEntity = null)
        {

            List<PROCESS_ISSUE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.ISSUE_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PROCESS_ISSUE
                                 where row.ISSUE_NO == paramEntity.ISSUE_NO
                                 select row).ToList<PROCESS_ISSUE>();
                }
                else
                {
                    lstEntity = (from row in DB.PROCESS_ISSUE
                                 select row).ToList<PROCESS_ISSUE>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PROCESS_ISSUE> GetProcessIssueByPartNumber(PRD_MAST paramEntity = null)
        {

            List<PROCESS_ISSUE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PROCESS_ISSUE
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<PROCESS_ISSUE>();
                }
                else
                {
                    lstEntity = (from row in DB.PROCESS_ISSUE
                                 select row).ToList<PROCESS_ISSUE>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }


        public string GetResponsibility(PRD_MAST paramEntity = null)
        {
            string sReturnValue = "";
            List<PROCESS_ISSUE> lstEntity = lstEntity = (from row in GetProcessIssueByPrimaryKey()
                                                         where row.PART_NO == paramEntity.PART_NO
                                                         select row).ToList<PROCESS_ISSUE>();
            try
            {
                if (!lstEntity.IsNotNullOrEmpty() || lstEntity.Count == 0) return sReturnValue;
                sReturnValue = lstEntity[lstEntity.Count - 1].COMPILED_BY.ToValueAsString().Trim();

                if (!sReturnValue.IsNotNullOrEmpty())
                    sReturnValue = paramEntity.ALLOTTED_BY.ToValueAsString().Trim();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return sReturnValue;
        }

        public List<PRD_DWG_ISSUE> GetDrawingIssueDetailsByPartNumber(PRD_DWG_ISSUE paramEntity = null)
        {

            List<PRD_DWG_ISSUE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_DWG_ISSUE
                                 where row.PART_NO == paramEntity.PART_NO && row.DWG_TYPE == paramEntity.DWG_TYPE
                                 orderby row.ISSUE_DATE descending, row.ISSUE_NO descending
                                 select row).ToList<PRD_DWG_ISSUE>();
                }
                else
                {
                    lstEntity = (from row in DB.PRD_DWG_ISSUE
                                 where row.DWG_TYPE == 1
                                 orderby row.ISSUE_DATE descending, row.ISSUE_NO descending
                                 select row).ToList<PRD_DWG_ISSUE>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }
        public List<PRD_DWG_ISSUE> GetDrawingIssueDetailsByPartNumberIsActive(PRD_DWG_ISSUE paramEntity = null)
        {

            List<PRD_DWG_ISSUE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_DWG_ISSUE
                                 where row.PART_NO == paramEntity.PART_NO && row.DWG_TYPE == paramEntity.DWG_TYPE && row.ISACTIVE_PRDMAST == true
                                 orderby row.ISSUE_DATE descending, row.ISSUE_NO descending
                                 select row).ToList<PRD_DWG_ISSUE>();
                }
                else
                {
                    lstEntity = (from row in DB.PRD_DWG_ISSUE
                                 where row.DWG_TYPE == 1
                                 orderby row.ISSUE_DATE descending, row.ISSUE_NO descending
                                 select row).ToList<PRD_DWG_ISSUE>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<MFM_MAST> GetManufacturingMasterByPartNumber(PRD_MAST paramEntity = null)
        {
            List<MFM_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.MFM_MAST
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<MFM_MAST>();
                }
                else
                {
                    lstEntity = (from row in DB.MFM_MAST
                                 select row).ToList<MFM_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PRD_CIREF> GetCIRefernceByPartNumber(PRD_MAST paramEntity = null)
        {

            List<PRD_CIREF> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_CIREF
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<PRD_CIREF>();
                }
                else
                {
                    lstEntity = (from row in DB.PRD_CIREF
                                 select row).ToList<PRD_CIREF>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PRD_CIREF> GetCIRefernce(DDCI_INFO paramEntity = null)
        {
            bool oldIsDefaultSubmitRequired = IsDefaultSubmitRequired;
            List<PRD_CIREF> lstEntity = null;
            try
            {

                IsDefaultSubmitRequired = false;
                if (!DB.IsNotNullOrEmpty()) return null;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.CI_REFERENCE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_CIREF
                                 where row.CI_REF == paramEntity.CI_REFERENCE
                                 select row).ToList<PRD_CIREF>();
                }
                else
                {
                    lstEntity = (from row in DB.PRD_CIREF
                                 select row).ToList<PRD_CIREF>();
                }
            }
            catch (Exception ex)
            {
                IsDefaultSubmitRequired = oldIsDefaultSubmitRequired;
                throw ex.LogException();
            }
            IsDefaultSubmitRequired = oldIsDefaultSubmitRequired;
            return lstEntity;
        }

        public List<V_CI_REFERENCE_NUMBER_PI> GetCIReferenceNumber(DDCI_INFO paramEntity = null)
        {

            List<V_CI_REFERENCE_NUMBER_PI> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.V_CI_REFERENCE_NUMBER_PI
                                 where row.CI_REFERENCE_IDPK == paramEntity.IDPK
                                 orderby row.CI_REFERENCE ascending
                                 select row).ToList<V_CI_REFERENCE_NUMBER_PI>();
                }
                else
                {

                    lstEntity = (from row in DB.V_CI_REFERENCE_NUMBER_PI
                                 orderby row.CI_REFERENCE ascending
                                 select row).ToList<V_CI_REFERENCE_NUMBER_PI>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public DDCI_INFO GetFRCSByPartNumber(DDCI_INFO paramEntity = null)
        {

            List<DDCI_INFO> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDCI_INFO
                                 where row.IDPK == paramEntity.IDPK
                                 select row).ToList<DDCI_INFO>();
                }
                else
                {
                    lstEntity = (from row in DB.DDCI_INFO
                                 select row).ToList<DDCI_INFO>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0 ? lstEntity[0] : null;
        }

        public string GetFinishDetails(DDFINISH_MAST paramEntity = null)
        {
            string sReturnValue = "";
            List<DDFINISH_MAST> lstEntity = lstEntity = (from row in DB.DDFINISH_MAST
                                                         where row.FINISH_CODE == paramEntity.FINISH_CODE
                                                         select row).ToList<DDFINISH_MAST>();
            try
            {
                if (!lstEntity.IsNotNullOrEmpty() || lstEntity.Count == 0) return sReturnValue;
                //sReturnValue = lstEntity[0].FINISH_CODE + " - (" + lstEntity[0].FINISH_DESC + ")";
                //sReturnValue = lstEntity[0].FINISH_CODE + " - " + lstEntity[0].FINISH_DESC + "";
                sReturnValue = lstEntity[0].FINISH_DESC.ToValueAsString();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return sReturnValue;
        }

        public string GetTopCoatDetails(DDCOATING_MAST paramEntity = null)
        {
            string sReturnValue = "";
            List<DDCOATING_MAST> lstEntity = lstEntity = (from row in DB.DDCOATING_MAST
                                                          where row.COATING_CODE == paramEntity.COATING_CODE
                                                          select row).ToList<DDCOATING_MAST>();
            try
            {
                if (!lstEntity.IsNotNullOrEmpty() || lstEntity.Count == 0) return sReturnValue;
                //sReturnValue = lstEntity[0].FINISH_CODE + " - (" + lstEntity[0].FINISH_DESC + ")";
                //sReturnValue = lstEntity[0].FINISH_CODE + " - " + lstEntity[0].FINISH_DESC + "";
                sReturnValue = lstEntity[0].COATING_DESC.ToValueAsString();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return sReturnValue;
        }

        public List<DEV_REPORT_SUB> GetDevelopmentSubReportByPartNumber(PRD_MAST paramEntity = null)
        {

            List<DEV_REPORT_SUB> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DEV_REPORT_SUB
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<DEV_REPORT_SUB>();
                }
                else
                {
                    lstEntity = (from row in DB.DEV_REPORT_SUB
                                 select row).ToList<DEV_REPORT_SUB>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PROCESS_SHEET> GetProcessSheetByPartNumber(PRD_MAST paramEntity = null)
        {

            List<PROCESS_SHEET> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PROCESS_SHEET
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<PROCESS_SHEET>();
                }
                else
                {
                    lstEntity = (from row in DB.PROCESS_SHEET
                                 select row).ToList<PROCESS_SHEET>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<TOOL_SCHED_SUB> GetSubToolScheduleByPartNumber(PRD_MAST paramEntity = null)
        {

            List<TOOL_SCHED_SUB> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.TOOL_SCHED_SUB
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<TOOL_SCHED_SUB>();
                }
                else
                {
                    lstEntity = (from row in DB.TOOL_SCHED_SUB
                                 select row).ToList<TOOL_SCHED_SUB>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<TOOL_SCHED_ISSUE> GetToolScheduleIssueByPartNumber(PRD_MAST paramEntity = null)
        {

            List<TOOL_SCHED_ISSUE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.TOOL_SCHED_ISSUE
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<TOOL_SCHED_ISSUE>();
                }
                else
                {
                    lstEntity = (from row in DB.TOOL_SCHED_ISSUE
                                 select row).ToList<TOOL_SCHED_ISSUE>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public string LoggedOnUserName()
        {
            return userName;

        }

        public List<V_COST_SHEET_PROCESS> GetCostSheetProcessByPartNumber(PRD_MAST paramEntity = null)
        {

            List<V_COST_SHEET_PROCESS> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.V_COST_SHEET_PROCESS
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<V_COST_SHEET_PROCESS>();
                }
                else
                {
                    lstEntity = (from row in DB.V_COST_SHEET_PROCESS
                                 select row).ToList<V_COST_SHEET_PROCESS>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<V_FORGING_OPERATION_PROCESS> GetForgingOperationProcessByPartNumber(PRD_MAST paramEntity = null)
        {

            List<V_FORGING_OPERATION_PROCESS> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.V_FORGING_OPERATION_PROCESS
                                 where row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<V_FORGING_OPERATION_PROCESS>();
                }
                else
                {
                    lstEntity = (from row in DB.V_FORGING_OPERATION_PROCESS
                                 select row).ToList<V_FORGING_OPERATION_PROCESS>();


                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public DateTime? ServerDateTime()
        {
            return serverDate;
        }

        public bool Insert<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as PRD_MAST).IsNotNullOrEmpty())
                {
                    PRD_MAST obj = entity as PRD_MAST;
                    try
                    {
                        if (obj.IDPK <= 0) obj.IDPK = GenerateNextNumber("PRD_MAST", "IDPK").ToIntValue();

                        obj.ROWID = Guid.NewGuid();
                        obj.DELETE_FLAG = false;
                        obj.ENTERED_BY = userName;
                        obj.ENTERED_DATE = serverDateTime;

                        DB.PRD_MAST.InsertOnSubmit(obj);
                        DB.SubmitChanges();

                        System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PRD_MAST.DeleteOnSubmit(obj);
                    }

                }
                if ((entity as PRD_CIREF).IsNotNullOrEmpty())
                {
                    PRD_CIREF obj = entity as PRD_CIREF;
                    try
                    {
                        if (obj.IDPK <= 0) obj.IDPK = GenerateNextNumber("PRD_CIREF", "IDPK").ToIntValue();
                        obj.ROWID = Guid.NewGuid();
                        DB.PRD_CIREF.InsertOnSubmit(obj);
                        DB.SubmitChanges();

                        System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PRD_CIREF.DeleteOnSubmit(obj);

                    }

                }
                if ((entity as MFM_MAST).IsNotNullOrEmpty())
                {
                    MFM_MAST objMFM = (entity as MFM_MAST);
                    try
                    {
                        if (objMFM.IDPK <= 0) objMFM.IDPK = GenerateNextNumber("MFM_MAST", "IDPK").ToIntValue();
                        objMFM.ROWID = Guid.NewGuid();

                        DB.MFM_MAST.InsertOnSubmit(objMFM);
                        DB.SubmitChanges();

                        System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.MFM_MAST.DeleteOnSubmit(objMFM);

                    }
                }

                if ((entity as PRD_DWG_ISSUE).IsNotNullOrEmpty())
                {
                    PRD_DWG_ISSUE objPRD_DWG_ISSUE = (entity as PRD_DWG_ISSUE);
                    try
                    {
                        if (objPRD_DWG_ISSUE.IDPK <= 0) objPRD_DWG_ISSUE.IDPK = GenerateNextNumber("PRD_DWG_ISSUE", "IDPK").ToIntValue();

                        DB.PRD_DWG_ISSUE.InsertOnSubmit(objPRD_DWG_ISSUE);
                        DB.SubmitChanges();

                        System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PRD_DWG_ISSUE.DeleteOnSubmit(objPRD_DWG_ISSUE);

                    }
                }

            }

            returnValue = true;
            return returnValue;
        }

        public bool Update<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as PRD_MAST).IsNotNullOrEmpty())
                {
                    PRD_MAST obj = null;
                    PRD_MAST activeEntity = (entity as PRD_MAST);

                    obj = (from row in DB.PRD_MAST
                           where row.IDPK == activeEntity.IDPK
                           select row).SingleOrDefault<PRD_MAST>();
                    if (obj.IsNotNullOrEmpty())
                    {
                        try
                        {
                            obj.PART_NO = activeEntity.PART_NO;
                            obj.PART_DESC = activeEntity.PART_DESC;
                            obj.SIM_TO_STD_CD = activeEntity.SIM_TO_STD_CD;
                            obj.PRD_CLASS_CD = activeEntity.PRD_CLASS_CD;
                            obj.MFG_STD = activeEntity.MFG_STD;
                            obj.ED_CD = activeEntity.ED_CD;
                            obj.THREAD_CD = activeEntity.THREAD_CD;
                            obj.DIA_CD = activeEntity.DIA_CD;
                            obj.QUALITY = activeEntity.QUALITY;
                            obj.BIF_PROJ = activeEntity.BIF_PROJ;
                            obj.BIF_FORECAST = activeEntity.BIF_FORECAST;
                            obj.FINISH_WT = activeEntity.FINISH_WT;
                            obj.FINISH_WT_EST = activeEntity.FINISH_WT_EST;
                            obj.HEAT_TREATMENT_CD = activeEntity.HEAT_TREATMENT_CD;
                            obj.HEAT_TREATMENT_DESC = activeEntity.HEAT_TREATMENT_DESC;
                            obj.PRD_GRP_CD = activeEntity.PRD_GRP_CD;
                            obj.MACHINE_CD = activeEntity.MACHINE_CD;
                            obj.ALLOT_DATE = activeEntity.ALLOT_DATE;
                            obj.DOC_REL_DATE = activeEntity.DOC_REL_DATE;
                            obj.DOC_REL_REMARKS = activeEntity.DOC_REL_REMARKS;
                            obj.FAMILY = activeEntity.FAMILY;
                            obj.HEAD_STYLE = activeEntity.HEAD_STYLE;
                            obj.TYPE = activeEntity.TYPE;
                            obj.APPLICATION = activeEntity.APPLICATION;
                            obj.USER_CD = activeEntity.USER_CD;
                            obj.THREAD_CLASS = activeEntity.THREAD_CLASS;
                            obj.THREAD_STD = activeEntity.THREAD_STD;
                            obj.PG_CATEGORY = activeEntity.PG_CATEGORY;
                            obj.NOS_PER_PACK = activeEntity.NOS_PER_PACK;
                            obj.SAMP_SUBMIT_DATE = activeEntity.SAMP_SUBMIT_DATE;
                            obj.ALLOTTED_BY = activeEntity.ALLOTTED_BY;
                            obj.PSW_ST = activeEntity.PSW_ST;
                            obj.DOC_REL_TYPE = activeEntity.DOC_REL_TYPE;
                            obj.HOLD_ME = activeEntity.HOLD_ME;
                            obj.ADDL_FEATURE = activeEntity.ADDL_FEATURE;
                            obj.KEYWORDS = activeEntity.KEYWORDS;
                            obj.CANCEL_STATUS = activeEntity.CANCEL_STATUS;
                            obj.ROWID = Guid.NewGuid();
                            obj.PART_CONFIG_ID = activeEntity.PART_CONFIG_ID;
                            obj.LOC_CODE = activeEntity.LOC_CODE;

                            obj.DELETE_FLAG = false;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.SubmitChanges();

                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.PRD_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                        }
                    }
                    else
                    {
                        returnValue = Delete<PRD_MAST>(new List<PRD_MAST> { activeEntity });
                        returnValue = Insert<PRD_MAST>(new List<PRD_MAST> { activeEntity });
                    }
                }
                if ((entity as PRD_CIREF).IsNotNullOrEmpty())
                {
                    PRD_CIREF objChild = null;
                    PRD_CIREF activeEntityChild = (entity as PRD_CIREF);

                    objChild = (from row in DB.PRD_CIREF
                                where row.IDPK == activeEntityChild.IDPK
                                select row).SingleOrDefault<PRD_CIREF>();

                    if (objChild.IsNotNullOrEmpty())
                    {
                        ////objChild = activeEntityChild.DeepCopy<DDCOST_PROCESS_DATA>();

                        //returnValue = Delete<PRD_CIREF>(new List<PRD_CIREF> { objChild });
                        //returnValue = Insert<PRD_CIREF>(new List<PRD_CIREF> { objChild });

                        objChild.PART_NO = activeEntityChild.PART_NO;
                        objChild.CI_REF = activeEntityChild.CI_REF;
                        objChild.SNO = activeEntityChild.SNO;
                        objChild.ROWID = activeEntityChild.ROWID;
                        objChild.IDPK = activeEntityChild.IDPK;
                        objChild.CIREF_NO_FK = activeEntityChild.CIREF_NO_FK;

                        returnValue = true;
                    }
                    else
                    {
                        returnValue = Delete<PRD_CIREF>(new List<PRD_CIREF> { activeEntityChild });
                        returnValue = Insert<PRD_CIREF>(new List<PRD_CIREF> { activeEntityChild });
                    }
                    returnValue = true;
                }
                if ((entity as MFM_MAST).IsNotNullOrEmpty())
                {
                    MFM_MAST objMFM = null;
                    MFM_MAST activeMFM = (entity as MFM_MAST);

                    objMFM = (from row in DB.MFM_MAST
                              where row.IDPK == activeMFM.IDPK
                              select row).SingleOrDefault<MFM_MAST>();

                    if (objMFM.IsNotNullOrEmpty())
                    {
                        try
                        {

                            objMFM.PART_NO = activeMFM.PART_NO;
                            objMFM.DOC_REL_DT_PLAN = activeMFM.DOC_REL_DT_PLAN;
                            objMFM.DOC_REL_DT_ACTUAL = activeMFM.DOC_REL_DT_ACTUAL;
                            objMFM.TOOLS_READY_DT_PLAN = activeMFM.TOOLS_READY_DT_PLAN;
                            objMFM.TOOLS_READY_ACTUAL_DT = activeMFM.TOOLS_READY_ACTUAL_DT;
                            objMFM.FORGING_PLAN_DT = activeMFM.FORGING_PLAN_DT;
                            objMFM.FORGING_ACTUAL_DT = activeMFM.FORGING_ACTUAL_DT;
                            objMFM.SECONDARY_PLAN_DT = activeMFM.SECONDARY_PLAN_DT;
                            objMFM.SECONDARY_ACTUAL_DT = activeMFM.SECONDARY_ACTUAL_DT;
                            objMFM.HEAT_TREATMENT_PLAN_DT = activeMFM.HEAT_TREATMENT_PLAN_DT;
                            objMFM.HEAT_TREATMENT_ACTUAL = activeMFM.HEAT_TREATMENT_ACTUAL;
                            objMFM.ISSR_PLAN_DT = activeMFM.ISSR_PLAN_DT;
                            objMFM.ISSR_ACTUAL_DT = activeMFM.ISSR_ACTUAL_DT;
                            objMFM.PPAP_PLAN = activeMFM.PPAP_PLAN;
                            objMFM.PPAP_ACTUAL_DT = activeMFM.PPAP_ACTUAL_DT;
                            objMFM.SAMPLE_QTY = activeMFM.SAMPLE_QTY;
                            objMFM.REMARKS = activeMFM.REMARKS;
                            objMFM.RESP = activeMFM.RESP;
                            objMFM.PSW_DATE = activeMFM.PSW_DATE;
                            objMFM.STATUS = activeMFM.STATUS;
                            objMFM.HOLDME = activeMFM.HOLDME;
                            objMFM.TIME_BOGAUGE_PLAN = activeMFM.TIME_BOGAUGE_PLAN;
                            objMFM.TIME_BOGAUGE_ACTUAL = activeMFM.TIME_BOGAUGE_ACTUAL;
                            objMFM.ROWID = activeMFM.ROWID;
                            //objMFM.IDPK = activeMFM.IDPK;

                            objMFM.DELETE_FLAG = false;
                            objMFM.UPDATED_BY = userInformation.UserName;
                            objMFM.UPDATED_DATE = serverDateTime;

                            DB.SubmitChanges();

                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;

                            returnValue = true;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.MFM_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objMFM);

                        }
                    }
                    else if (activeMFM.PART_NO.ToValueAsString().IsNotNullOrEmpty())
                    {
                        returnValue = Delete<MFM_MAST>(new List<MFM_MAST> { activeMFM });
                        returnValue = Insert<MFM_MAST>(new List<MFM_MAST> { activeMFM });
                    }
                }

                if ((entity as PRD_DWG_ISSUE).IsNotNullOrEmpty())
                {
                    PRD_DWG_ISSUE objPRD_DWG_ISSUE = null;
                    PRD_DWG_ISSUE activePRD_DWG_ISSUE = (entity as PRD_DWG_ISSUE);

                    objPRD_DWG_ISSUE = (from row in DB.PRD_DWG_ISSUE
                                        where row.IDPK == activePRD_DWG_ISSUE.IDPK
                                        select row).SingleOrDefault<PRD_DWG_ISSUE>();

                    if (objPRD_DWG_ISSUE.IsNotNullOrEmpty())
                    {
                        try
                        {
                            objPRD_DWG_ISSUE.PART_NO = activePRD_DWG_ISSUE.PART_NO;
                            objPRD_DWG_ISSUE.DWG_TYPE = activePRD_DWG_ISSUE.DWG_TYPE;
                            objPRD_DWG_ISSUE.ISSUE_NO = activePRD_DWG_ISSUE.ISSUE_NO;
                            objPRD_DWG_ISSUE.ISSUE_DATE = activePRD_DWG_ISSUE.ISSUE_DATE;
                            objPRD_DWG_ISSUE.ISSUE_ALTER = activePRD_DWG_ISSUE.ISSUE_ALTER;
                            objPRD_DWG_ISSUE.COMPILED_BY = activePRD_DWG_ISSUE.COMPILED_BY;
                            objPRD_DWG_ISSUE.DELETE_FLAG = activePRD_DWG_ISSUE.DELETE_FLAG;
                            objPRD_DWG_ISSUE.UPDATED_DATE = activePRD_DWG_ISSUE.UPDATED_DATE;
                            objPRD_DWG_ISSUE.UPDATED_BY = activePRD_DWG_ISSUE.UPDATED_BY;
                            objPRD_DWG_ISSUE.IDPK = activePRD_DWG_ISSUE.IDPK;
                            objPRD_DWG_ISSUE.PRM_FK = activePRD_DWG_ISSUE.PRM_FK;

                            DB.SubmitChanges();

                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DDSTD_NOTES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objPRD_DWG_ISSUE);

                        }
                    }
                    else
                    {
                        returnValue = Delete<PRD_DWG_ISSUE>(new List<PRD_DWG_ISSUE> { activePRD_DWG_ISSUE });
                        returnValue = Insert<PRD_DWG_ISSUE>(new List<PRD_DWG_ISSUE> { activePRD_DWG_ISSUE });
                    }
                }
            }
            returnValue = true;
            return returnValue;
        }

        public bool Delete<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as PRD_MAST).IsNotNullOrEmpty())
                {
                    PRD_MAST obj = null;
                    PRD_MAST activeEntity = (entity as PRD_MAST);
                    try
                    {
                        obj = (from row in DB.PRD_MAST
                               where row.IDPK == activeEntity.IDPK
                               select row).SingleOrDefault<PRD_MAST>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.DELETE_FLAG = true;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.SubmitChanges();

                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PRD_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                    }
                }
                if ((entity as PRD_CIREF).IsNotNullOrEmpty())
                {
                    PRD_CIREF objChild = null;
                    PRD_CIREF activeEntityChild = (entity as PRD_CIREF);
                    try
                    {
                        objChild = (from row in DB.PRD_CIREF
                                    where row.IDPK == activeEntityChild.IDPK
                                    select row).SingleOrDefault<PRD_CIREF>();

                        if (objChild.IsNotNullOrEmpty())
                        {
                            DB.PRD_CIREF.DeleteOnSubmit(objChild);

                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Deletes.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PRD_CIREF.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objChild);

                    }

                }
                if ((entity as DDSTD_NOTES).IsNotNullOrEmpty())
                {
                    DDSTD_NOTES objSTD = null;
                    DDSTD_NOTES activeSTD = (entity as DDSTD_NOTES);
                    try
                    {
                        objSTD = (from row in DB.DDSTD_NOTES
                                  where row.SNO == activeSTD.SNO
                                  select row).SingleOrDefault<DDSTD_NOTES>();

                        if (objSTD.IsNotNullOrEmpty())
                        {
                            DB.DDSTD_NOTES.DeleteOnSubmit(objSTD);
                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Deletes.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDSTD_NOTES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objSTD);

                    }
                }

                if ((entity as PRD_DWG_ISSUE).IsNotNullOrEmpty())
                {
                    PRD_DWG_ISSUE objPRD_DWG_ISSUE = null;
                    PRD_DWG_ISSUE activePRD_DWG_ISSUE = (entity as PRD_DWG_ISSUE);
                    try
                    {
                        objPRD_DWG_ISSUE = (from row in DB.PRD_DWG_ISSUE
                                            where row.IDPK == activePRD_DWG_ISSUE.IDPK
                                            select row).SingleOrDefault<PRD_DWG_ISSUE>();

                        if (objPRD_DWG_ISSUE.IsNotNullOrEmpty())
                        {
                            DB.PRD_DWG_ISSUE.DeleteOnSubmit(objPRD_DWG_ISSUE);
                            System.Data.Linq.ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Deletes.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        foreach (System.Data.Linq.ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PRD_DWG_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objPRD_DWG_ISSUE);

                    }
                }
            }
            return returnValue;
        }
        public bool UpdatePrdDrgIssue(string partNo, decimal issueNo, string locationCode, DateTime? issuedate)
        {

            List<PRD_DWG_ISSUE> lstexistingDatasPccsIssue = new List<PRD_DWG_ISSUE>();
            lstexistingDatasPccsIssue = ((from c in DB.PRD_DWG_ISSUE
                                          where c.PART_NO == partNo && c.DWG_TYPE == 1
                                          select c).ToList());
            if (lstexistingDatasPccsIssue.Count > 0)
            {
                try
                {

                    foreach (PRD_DWG_ISSUE prdDwg in lstexistingDatasPccsIssue)
                    {
                        prdDwg.ISACTIVE_PRDMAST = false;
                        DB.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    DB.PRD_DWG_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstexistingDatasPccsIssue);
                }
            }

            PRD_DWG_ISSUE prdDwgCurrent = (from c in DB.PRD_DWG_ISSUE
                                           where c.PART_NO == partNo && c.DWG_TYPE == 1 && c.ISSUE_NO == issueNo && c.Loc_Code == locationCode && c.ISSUE_DATE == issuedate
                                           select c).FirstOrDefault<PRD_DWG_ISSUE>();
            if (prdDwgCurrent.IsNotNullOrEmpty())
            {
                try
                {
                    prdDwgCurrent.ISACTIVE_PRDMAST = true;
                    DB.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    DB.PRD_DWG_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, prdDwgCurrent);
                }
            }
            return true;
        }
        public DataTable GetInitialInspection(string partno, int initial)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from d in DB.PCCS
                                  join p in DB.PROCESS_MAIN on d.PART_NO equals p.PART_NO
                                  where d.PART_NO == partno && d.ISR_NO != initial && d.ROUTE_NO == p.ROUTE_NO && (d.DELETE_FLAG == false || d.DELETE_FLAG == null) && p.CURRENT_PROC == 1
                                  orderby d.ISR_NO ascending
                                  select new
                                  {
                                      CUST_DWG_NO = "",
                                      PROD_DESC = "",
                                      p.PART_NO,
                                      d.ISR_NO,
                                      d.FEATURE,
                                      d.SPEC_MAX,
                                      d.SPEC_MIN
                                  }).ToList());
                if (dt == null && dt.Rows.Count == 0)
                {
                    return null;
                }

                var ciref = (from m in DB.PRD_CIREF
                             join c in DB.DDCI_INFO on m.CI_REF equals c.CI_REFERENCE
                             where m.PART_NO == partno && m.CURRENT_CIREF == true
                             select new
                             {
                                 c.CUST_DWG_NO,
                                 c.PROD_DESC
                             }).FirstOrDefault();

                if (ciref != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["CUST_DWG_NO"] = ciref.CUST_DWG_NO;
                        dr["PROD_DESC"] = ciref.PROD_DESC;
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }

    }
}
