using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class FeasibleReportAndCostSheet : Essential, IDataManipulation
    {
        public FeasibleReportAndCostSheet(UserInformation userInformation)
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

        public List<DDCI_INFO> GetEntitiesByPrimaryKey(DDCI_INFO paramEntity = null)
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
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDCI_INFO> GetEntitiesByCIReferenceNumber(DDCI_INFO paramEntity = null)
        {

            List<DDCI_INFO> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.CI_REFERENCE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDCI_INFO
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) &&
                                 row.CI_REFERENCE == paramEntity.CI_REFERENCE
                                 select row).ToList<DDCI_INFO>();
                }
                else
                {

                    lstEntity = (from row in DB.DDCI_INFO
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDCI_INFO>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<V_CI_REFERENCE_NUMBER> GetCIReferenceNumber(DDCI_INFO paramEntity = null)
        {

            List<V_CI_REFERENCE_NUMBER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.V_CI_REFERENCE_NUMBER
                                 where row.IDPK == paramEntity.IDPK
                                 select row).ToList<V_CI_REFERENCE_NUMBER>();
                }
                else
                {

                    lstEntity = (from row in DB.V_CI_REFERENCE_NUMBER
                                 select row).ToList<V_CI_REFERENCE_NUMBER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDCOST_PROCESS_DATA> GetCostDetails(DDCI_INFO paramEntity = null)
        {

            List<DDCOST_PROCESS_DATA> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDCOST_PROCESS_DATA
                                 where (row.CI_INFO_FK == paramEntity.IDPK || row.CI_REFERENCE == paramEntity.CI_REFERENCE)
                                 orderby row.SNO ascending
                                 select row).ToList<DDCOST_PROCESS_DATA>();
                }
                else
                {

                    lstEntity = (from row in DB.DDCOST_PROCESS_DATA
                                 orderby row.SNO ascending
                                 select row).ToList<DDCOST_PROCESS_DATA>();
                }

                //if (!DB.IsNotNullOrEmpty()) return lstEntity;
                //if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                //{
                //    lstEntity = (from row in DB.DDCOST_PROCESS_DATA
                //                 where row.CI_REFERENCE == paramEntity.CI_REFERENCE
                //                 orderby row.SNO ascending
                //                 select row).ToList<DDCOST_PROCESS_DATA>();
                //}
                //else
                //{

                //    lstEntity = (from row in DB.DDCOST_PROCESS_DATA
                //                 orderby row.SNO ascending
                //                 select row).ToList<DDCOST_PROCESS_DATA>();
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDSHAPE_DETAILS> GetShapeDetails(DDCI_INFO paramEntity = null)
        {

            List<DDSHAPE_DETAILS> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDSHAPE_DETAILS
                                 where (row.CIREF_NO_FK == paramEntity.IDPK || row.CI_REFERENCE == paramEntity.CI_REFERENCE)
                                 select row).ToList<DDSHAPE_DETAILS>();
                }
                else
                {

                    lstEntity = (from row in DB.DDSHAPE_DETAILS
                                 select row).ToList<DDSHAPE_DETAILS>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDSHAPE_DETAILS> GetShapeDetailsByCIReference(DDCI_INFO paramEntity = null)
        {

            List<DDSHAPE_DETAILS> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.CI_REFERENCE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDSHAPE_DETAILS
                                 where row.CI_REFERENCE == paramEntity.CI_REFERENCE
                                 select row).ToList<DDSHAPE_DETAILS>();
                }
                else
                {

                    lstEntity = (from row in DB.DDSHAPE_DETAILS
                                 select row).ToList<DDSHAPE_DETAILS>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }


        public List<CI_REFERENCE_ZONE> GetZoneDetails(CI_REFERENCE_ZONE paramEntity = null)
        {

            List<CI_REFERENCE_ZONE> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.CI_REFERENCE_ZONE
                                 where row.IDPK == paramEntity.IDPK
                                 orderby row.IDPK ascending
                                 select row).ToList<CI_REFERENCE_ZONE>();
                }
                else
                {

                    lstEntity = (from row in DB.CI_REFERENCE_ZONE
                                 orderby row.IDPK ascending
                                 select row).ToList<CI_REFERENCE_ZONE>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
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
                                 where row.CUST_CODE == paramEntity.CUST_CODE
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

        public List<SEC_USER_MASTER> GetUserDetails(SEC_USER_MASTER paramEntity = null, bool isDeletedRecordRequired = false)
        {

            List<SEC_USER_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.USER_NAME.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.SEC_USER_MASTER
                                 where row.USER_NAME == paramEntity.USER_NAME &&
                                 Convert.ToString(row.DESIGNATION).ToUpper() != "SYSTEM INCHARGE" && Convert.ToString(row.DESIGNATION).ToUpper() != "END USER"
                                 orderby row.USER_NAME ascending
                                 select row).ToList<SEC_USER_MASTER>();
                }
                else
                {

                    lstEntity = (from row in DB.SEC_USER_MASTER
                                 where Convert.ToString(row.DESIGNATION).ToUpper() != "SYSTEM INCHARGE" && Convert.ToString(row.DESIGNATION).ToUpper() != "END USER"
                                 orderby row.USER_NAME ascending
                                 select row).ToList<SEC_USER_MASTER>();
                }

                if (!isDeletedRecordRequired && lstEntity.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in lstEntity.AsEnumerable()
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 orderby row.USER_NAME ascending
                                 select row).ToList<SEC_USER_MASTER>();
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDRM_MAST> GetRawMaterialsDetails(DDRM_MAST paramEntity = null, bool isDeletedRecordRequired = false)
        {
            List<DDRM_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDRM_MAST
                                 where row.IDPK == paramEntity.IDPK
                                 select row).ToList<DDRM_MAST>();
                }
                else
                {

                    lstResult = (from row in DB.DDRM_MAST
                                 select row).ToList<DDRM_MAST>();
                }

                if (!isDeletedRecordRequired && lstResult.IsNotNullOrEmpty())
                {
                    lstResult = (from row in lstResult.AsEnumerable()
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDRM_MAST>();
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstResult;
        }

        public List<DDFINISH_MAST> GetFinishDetails(DDFINISH_MAST paramEntity = null, bool isDeletedRecordRequired = false)
        {
            List<DDFINISH_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDFINISH_MAST
                                 where row.FINISH_CODE == paramEntity.FINISH_CODE && (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDFINISH_MAST>();
                }
                else
                {

                    lstResult = (from row in DB.DDFINISH_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDFINISH_MAST>();
                }

                //if (!isDeletedRecordRequired && lstResult.IsNotNullOrEmpty())
                //{
                //    lstResult = (from row in lstResult.AsEnumerable()
                //                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                //                 select row).ToList<DDFINISH_MAST>();
                //}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<DDCOATING_MAST> GetTopCoatingDetails(DDCOATING_MAST paramEntity = null, bool isDeletedRecordRequired = false)
        {
            List<DDCOATING_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDCOATING_MAST
                                 where row.COATING_CODE == paramEntity.COATING_CODE && (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDCOATING_MAST>();
                }
                else
                {

                    lstResult = (from row in DB.DDCOATING_MAST
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDCOATING_MAST>();
                }

                //if (!isDeletedRecordRequired && lstResult.IsNotNullOrEmpty())
                //{
                //    lstResult = (from row in lstResult.AsEnumerable()
                //                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                //                 select row).ToList<DDCOATING_MAST>();
                //}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<DDOPER_MAST> GetOperationDetails(DDOPER_MAST paramEntity = null, bool includeProcess = true)
        {
            List<DDOPER_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDOPER_MAST
                                 where ((Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)) &&
                                 row.OPER_CODE == paramEntity.OPER_CODE && row.SHOW_IN_COST == "1"
                                 orderby row.OPER_DESC ascending
                                 select row).ToList<DDOPER_MAST>();
                }
                else
                {

                    lstResult = (from row in DB.DDOPER_MAST
                                 where ((Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)) &&
                                 row.SHOW_IN_COST == "1"
                                 orderby row.OPER_CODE ascending
                                 select row).ToList<DDOPER_MAST>();
                }

                if (includeProcess)
                {
                    List<V_RAW_MATERIAL_PROCESS_COST> lstRawMaterialProcess = GetRawMaterialProcessCostDetails().Where(row => row.OPERATION_NO == 1000).GroupBy(row => row.OPERATION_NO).Select(row => row.First()).ToList<V_RAW_MATERIAL_PROCESS_COST>();

                    List<DDOPER_MAST> lstProcess = (from row in lstRawMaterialProcess
                                                    where row.OPERATION_NO == 1000
                                                    select new DDOPER_MAST()
                                                    {
                                                        OPER_CODE = row.OPERATION_NO,
                                                        OPER_DESC = row.OPERATION_DESCRIPTION,
                                                        SHOW_IN_COST = "1",
                                                    }).ToList<DDOPER_MAST>();
                    lstResult.AddRange(lstProcess);

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<V_OPERATION_COST> GetOperationCostDetails(DDCOST_CENT_MAST paramEntity = null)
        {

            List<V_OPERATION_COST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.V_OPERATION_COST
                                 where row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 && ((Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false) || (row.DELETE_FLAG == null))
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_OPERATION_COST>();
                }
                else
                {
                    lstResult = (from row in DB.V_OPERATION_COST
                                 where ((Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false) || (row.DELETE_FLAG == null))
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_OPERATION_COST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<V_OPERATION_COST> GetOperationCostDetailsByOperation(V_OPERATION_COST paramEntity = null)
        {

            List<V_OPERATION_COST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.V_OPERATION_COST
                                 where row.COST_CENT_CODE == paramEntity.COST_CENT_CODE &&
                                 row.OPERATION_NO == paramEntity.OPERATION_NO &&
                                 row.LOC_CODE == paramEntity.LOC_CODE
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_OPERATION_COST>();
                }
                else
                {

                    lstResult = (from row in DB.V_OPERATION_COST
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_OPERATION_COST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<V_RAW_MATERIAL_PROCESS_COST> GetRawMaterialProcessCostDetails(DDCOST_CENT_MAST paramEntity = null)
        {

            List<V_RAW_MATERIAL_PROCESS_COST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.V_RAW_MATERIAL_PROCESS_COST
                                 where row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_RAW_MATERIAL_PROCESS_COST>();
                }
                else
                {

                    lstResult = (from row in DB.V_RAW_MATERIAL_PROCESS_COST
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_RAW_MATERIAL_PROCESS_COST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<V_COST_CENTER_PROCESS_COST> GetCostCenterProcessCostDetails(DDCOST_CENT_MAST paramEntity = null)
        {

            List<V_COST_CENTER_PROCESS_COST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.V_COST_CENTER_PROCESS_COST
                                 where row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_COST_CENTER_PROCESS_COST>();
                }
                else
                {

                    lstResult = (from row in DB.V_COST_CENTER_PROCESS_COST
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_COST_CENTER_PROCESS_COST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<V_FINISH_PROCESS_COST> GetFinishProcessCostDetails(DDCOST_CENT_MAST paramEntity = null)
        {

            List<V_FINISH_PROCESS_COST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.V_FINISH_PROCESS_COST
                                 where row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_FINISH_PROCESS_COST>();
                }
                else
                {

                    lstResult = (from row in DB.V_FINISH_PROCESS_COST
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_FINISH_PROCESS_COST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<V_OPERATION_PROCESS_COST> GetOperationProcessCostDetails(DDCOST_CENT_MAST paramEntity = null)
        {

            List<V_OPERATION_PROCESS_COST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.V_OPERATION_PROCESS_COST
                                 where row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_OPERATION_PROCESS_COST>();
                }
                else
                {

                    lstResult = (from row in DB.V_OPERATION_PROCESS_COST
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<V_OPERATION_PROCESS_COST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<DDCC_OUTPUT> GetCostCentreOutputDetails(DDCOST_CENT_MAST paramEntity = null)
        {

            List<DDCC_OUTPUT> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDCC_OUTPUT
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<DDCC_OUTPUT>();
                }
                else
                {

                    lstResult = (from row in DB.DDCC_OUTPUT
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<DDCC_OUTPUT>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public List<DDCOST_CENT_MAST> GetCostCentreDetails(DDCOST_CENT_MAST paramEntity = null)
        {

            List<DDCOST_CENT_MAST> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDCOST_CENT_MAST
                                 where row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<DDCOST_CENT_MAST>();
                }
                else
                {

                    lstResult = (from row in DB.DDCOST_CENT_MAST
                                 orderby row.COST_CENT_CODE ascending
                                 select row).ToList<DDCOST_CENT_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
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
                                 where row.LOC_CODE == paramEntity.LOC_CODE && (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
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

        public DateTime? ServerDateTime()
        {
            return serverDate;
        }

        public List<DDSTD_NOTES> GetStandardNotes(DDSTD_NOTES paramEntity = null)
        {
            List<DDSTD_NOTES> lstResult = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstResult;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstResult = (from row in DB.DDSTD_NOTES
                                 where row.SNO == paramEntity.SNO && row.STD_NOTES != null
                                 orderby row.SNO ascending
                                 select row).ToList<DDSTD_NOTES>();

                }
                else
                {

                    lstResult = (from row in DB.DDSTD_NOTES
                                 where row.STD_NOTES != null
                                 orderby row.SNO ascending
                                 select row).ToList<DDSTD_NOTES>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstResult;
        }

        public string CreateCIReferenceNumber(DDCI_INFO paramEntity)
        {
            string sReturnValue = null;
            try
            {
                if (!paramEntity.IsNotNullOrEmpty()) return sReturnValue;
                if (!paramEntity.ZONE_CODE.IsNotNullOrEmpty()) return sReturnValue;

                string zoneCode = paramEntity.ZONE_CODE;
                DateTime? enquiryReceivedOn = paramEntity.ENQU_RECD_ON;

                if (!enquiryReceivedOn.ToDateAsString().IsNotNullOrEmpty()) return sReturnValue;

                zoneCode = zoneCode.ToValueAsString().Trim();
                if (!zoneCode.IsNotNullOrEmpty()) zoneCode = "X";

                DateTime receivedon = new DateTime(2000, 02, 04); //Tested Date
                receivedon = Convert.ToDateTime(enquiryReceivedOn.ToDateAsString());
                if (!enquiryReceivedOn.IsNotNullOrEmpty()) receivedon = DateTime.Now;


                List<DDCI_INFO> lstResult = null;

                if (!DB.IsNotNullOrEmpty()) return sReturnValue;

                string year = receivedon.Year.ToValueAsString().Length >= 4 ? receivedon.Year.ToValueAsString().Substring(2, 2) :
                                                                              receivedon.Year.ToValueAsString();

                string day = receivedon.Day.ToValueAsString().PadLeft(2, '0');

                DB.Refresh(RefreshMode.OverwriteCurrentValues);
                lstResult = (from row in DB.DDCI_INFO
                             where row.CI_REFERENCE.Substring(0, 1) == zoneCode &&
                             row.CI_REFERENCE.Substring(1, 2) == year &&
                             Convert.ToInt16(row.CI_REFERENCE.Substring(3, 2)) == receivedon.Month &&
                             row.CI_REFERENCE.Substring(5, 2) == day
                             orderby Convert.ToInt16(row.CI_REFERENCE.Substring(7, 3)) descending
                             select row).ToList<DDCI_INFO>();

                if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                {
                    if (lstResult[0].ENQU_RECD_ON.IsNotNullOrEmpty())
                    {
                        receivedon = Convert.ToDateTime(lstResult[0].ENQU_RECD_ON.ToDateAsString());
                    }
                    else
                    {
                        receivedon = DateTime.Now;
                    }
                    if (lstResult[0].CI_REFERENCE.IsNotNullOrEmpty())
                    {
                        sReturnValue = zoneCode + receivedon.ToString("yyMMdd") +
                            (lstResult[0].CI_REFERENCE.Substring(7, 3).ToIntValue() + 1).ToValueAsString().PadLeft(3, '0');
                    }
                }
                else
                {
                    sReturnValue = zoneCode + receivedon.ToString("yyMMdd") + "001";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return sReturnValue.Trim();
        }

        public bool IsValidCIReferenceNumber(DDCI_INFO paramEntity, OperationMode operationMode, out string message)
        {
            bool bReturnValue = false;
            List<DDCI_INFO> lstResult = null;
            message = "";
            try
            {
                if (!paramEntity.IsNotNullOrEmpty()) return bReturnValue;
                string ciReferenceNumber = paramEntity.CI_REFERENCE;

                if (!ciReferenceNumber.IsNotNullOrEmpty())
                {
                    message = PDMsg.NotEmpty("CI Reference Number");
                    return bReturnValue;
                }

                if (ciReferenceNumber.Length < 10)
                {

                    if (operationMode == OperationMode.Edit)
                    {
                        lstResult = (from row in DB.DDCI_INFO
                                     where Convert.ToString(row.CI_REFERENCE) == ciReferenceNumber
                                     orderby Convert.ToString(row.CI_REFERENCE.Substring(7, 3)) descending
                                     select row).ToList<DDCI_INFO>();
                        if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                        {
                            bReturnValue = true;
                            return bReturnValue;
                        }
                    }

                    message = "Enter CI Reference in the Valid Format";
                    return bReturnValue;
                }

                if (!DB.IsNotNullOrEmpty()) return bReturnValue;

                List<CI_REFERENCE_ZONE> lstZone = (from row in DB.CI_REFERENCE_ZONE
                                                   where row.CODE == ciReferenceNumber.Substring(0, 1)
                                                   orderby row.IDPK ascending
                                                   select row).ToList<CI_REFERENCE_ZONE>();


                if (!lstZone.IsNotNullOrEmpty() || lstZone.Count == 0)
                {
                    if (operationMode == OperationMode.Edit)
                    {
                        lstResult = (from row in DB.DDCI_INFO
                                     where Convert.ToString(row.CI_REFERENCE) == ciReferenceNumber
                                     orderby Convert.ToString(row.CI_REFERENCE.Substring(7, 3)) descending
                                     select row).ToList<DDCI_INFO>();
                        if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                        {
                            bReturnValue = true;
                            return bReturnValue;
                        }
                    }

                    message = "Enter CI Reference in the Valid Format";
                    return bReturnValue;
                }

                string zoneCode = ciReferenceNumber.Substring(0, 1);

                if (zoneCode != lstZone[0].CODE.Trim())
                {
                    if (operationMode == OperationMode.Edit)
                    {
                        lstResult = (from row in DB.DDCI_INFO
                                     where Convert.ToString(row.CI_REFERENCE) == ciReferenceNumber
                                     orderby Convert.ToString(row.CI_REFERENCE.Substring(7, 3)) descending
                                     select row).ToList<DDCI_INFO>();
                        if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                        {
                            bReturnValue = true;
                            return bReturnValue;
                        }
                    }

                    message = "Enter CI Reference in the Valid Format";
                    return bReturnValue;
                }

                string syear = ciReferenceNumber.Substring(1, 2).ToIntValue() == 0 ?
                    "20" + ciReferenceNumber.Substring(1, 2) : ciReferenceNumber.Substring(1, 2);

                string smonth = ciReferenceNumber.Substring(3, 2);
                string sday = ciReferenceNumber.Substring(5, 2);

                int year;
                int month;
                int day;

                if (!int.TryParse(syear, out year))
                {
                    message = "Invalid CI Reference Format\r\nEnter Valid Year";
                    return bReturnValue;
                }

                if (!int.TryParse(smonth, out month) || smonth.ToIntValue() > 12)
                {
                    message = "Invalid CI Reference Format\r\nEnter Valid Month";
                    return bReturnValue;
                }

                if (!int.TryParse(sday, out day) || sday.ToIntValue() > 31 || sday.ToIntValue() <= 0)
                {
                    message = "Invalid CI Reference Format\r\nEnter Valid Day";
                    return bReturnValue;
                }

                DateTime receivedon = new DateTime(year, month, 01);

                //if (ciReferenceNumber.Substring(1, 2).ToIntValue() < 0)
                //{
                //    message = "Enter Valid Year";
                //    return bReturnValue;
                //}

                //if (ciReferenceNumber.Substring(3, 2).ToIntValue() <= 0 || ciReferenceNumber.Substring(3, 2).ToIntValue() > 12)
                //{
                //    message = "Enter Valid Month";
                //    return bReturnValue;
                //}

                //if (ciReferenceNumber.Substring(5, 2).ToIntValue() <= 0 || ciReferenceNumber.Substring(5, 2).ToIntValue() > 31)
                //{
                //    message = "Enter Valid Day";
                //    return bReturnValue;
                //}

                if (ciReferenceNumber.Substring(5, 2).ToIntValue() > receivedon.AddMonths(1).AddDays(-1).Day)
                {
                    message = "Enter Valid Day";
                    return bReturnValue;
                }

                string ssequenceNumber = ciReferenceNumber.Substring(7, 3);
                int sequenceNumber;
                if (!int.TryParse(ssequenceNumber, out sequenceNumber))
                {
                    message = "Enter Valid Day";
                    return bReturnValue;
                }

                //if (ciReferenceNumber.Substring(7, 3).ToIntValue() <= 0)
                //{
                //    message = "Enter Valid Number";
                //    return bReturnValue;
                //}

                syear = ciReferenceNumber.Substring(1, 2);

                lstResult = (from row in DB.DDCI_INFO
                             where Convert.ToString(row.CI_REFERENCE.Substring(0, 1)) == zoneCode &&
                             Convert.ToString(row.CI_REFERENCE.Substring(1, 2)) == syear &&
                             Convert.ToString(row.CI_REFERENCE.Substring(3, 2)) == smonth &&
                             Convert.ToString(row.CI_REFERENCE.Substring(5, 2)) == sday &&
                             Convert.ToString(row.CI_REFERENCE).Trim() == ciReferenceNumber.Trim()
                             orderby Convert.ToString(row.CI_REFERENCE.Substring(7, 3)) descending
                             select row).ToList<DDCI_INFO>();


                int no_of_count_checking = 0;
                if (operationMode == OperationMode.Edit || operationMode == OperationMode.Delete || operationMode == OperationMode.Update)
                {
                    no_of_count_checking = 1;
                }
                if (lstResult.IsNotNullOrEmpty() && lstResult.Count > no_of_count_checking && operationMode != OperationMode.Print)
                {
                    message = PDMsg.AlreadyExists("CI Reference Number");
                    return bReturnValue;
                }
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return bReturnValue;
        }

        public bool Insert<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as DDCI_INFO).IsNotNullOrEmpty())
                {
                    DDCI_INFO obj = entity as DDCI_INFO;
                    try
                    {
                        if (obj.IDPK <= 0) obj.IDPK = GenerateNextNumber("DDCI_INFO", "IDPK").ToIntValue();
                        obj.NO_OF_PCS = obj.NO_OF_PCS.ToValueAsString().ToIntValue() <= 0 ? 100 : obj.NO_OF_PCS;

                        obj.ROWID = Guid.NewGuid();
                        obj.DELETE_FLAG = false;
                        obj.ENTERED_BY = userName;
                        obj.ENTERED_DATE = serverDateTime;

                        DB.DDCI_INFO.InsertOnSubmit(obj);
                        DB.SubmitChanges();

                        ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDCI_INFO.DeleteOnSubmit(obj);

                    }

                }
                if ((entity as DDCOST_PROCESS_DATA).IsNotNullOrEmpty())
                {
                    DDCOST_PROCESS_DATA obj = entity as DDCOST_PROCESS_DATA;
                    try
                    {
                        if (obj.IDPK <= 0) obj.IDPK = GenerateNextNumber("DDCOST_PROCESS_DATA", "IDPK").ToIntValue();
                        obj.ROWID = Guid.NewGuid();
                        DB.DDCOST_PROCESS_DATA.InsertOnSubmit(obj);
                        DB.SubmitChanges();

                        ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (InvalidOperationException)
                    {
                        DB.DDCOST_PROCESS_DATA.DeleteOnSubmit(obj);
                        DB.SubmitChanges();

                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDCOST_PROCESS_DATA.DeleteOnSubmit(obj);

                    }

                }
                if ((entity as DDSTD_NOTES).IsNotNullOrEmpty())
                {
                    DDSTD_NOTES objSTD = (entity as DDSTD_NOTES);
                    try
                    {
                        if (objSTD.SNO <= 0) objSTD.SNO = GenerateNextNumber("DDSTD_NOTES", "SNO").ToIntValue();
                        //objSTD.ROWID = Guid.NewGuid();

                        DB.DDSTD_NOTES.InsertOnSubmit(objSTD);
                        DB.SubmitChanges();

                        ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDSTD_NOTES.DeleteOnSubmit(objSTD);

                    }
                }

                if ((entity as DDSHAPE_DETAILS).IsNotNullOrEmpty())
                {
                    DDSHAPE_DETAILS objSHAPE = (entity as DDSHAPE_DETAILS);
                    try
                    {
                        if (objSHAPE.IDPK <= 0) objSHAPE.IDPK = GenerateNextNumber("DDSHAPE_DETAILS", "IDPK").ToIntValue();
                        objSHAPE.ROWID = Guid.NewGuid();

                        DB.DDSHAPE_DETAILS.InsertOnSubmit(objSHAPE);
                        DB.SubmitChanges();

                        ChangeSet cs = DB.GetChangeSet();
                        returnValue = cs.Inserts.Count > 0 ? true : false;
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDSHAPE_DETAILS.DeleteOnSubmit(objSHAPE);

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
                if ((entity as DDCI_INFO).IsNotNullOrEmpty())
                {
                    DDCI_INFO obj = null;
                    DDCI_INFO activeEntity = (entity as DDCI_INFO);

                    obj = (from row in DB.DDCI_INFO
                           where row.IDPK == activeEntity.IDPK
                           select row).SingleOrDefault<DDCI_INFO>();
                    if (obj.IsNotNullOrEmpty())
                    {
                        try
                        {
                            obj.CI_REFERENCE = activeEntity.CI_REFERENCE;
                            obj.ENQU_RECD_ON = activeEntity.ENQU_RECD_ON;
                            obj.FR_CS_DATE = activeEntity.FR_CS_DATE;
                            obj.PROD_DESC = activeEntity.PROD_DESC;
                            obj.CUST_CODE = activeEntity.CUST_CODE;
                            obj.CUST_DWG_NO = activeEntity.CUST_DWG_NO;
                            obj.CUST_DWG_NO_ISSUE = activeEntity.CUST_DWG_NO_ISSUE;
                            obj.EXPORT = activeEntity.EXPORT;
                            obj.NUMBER_OFF = activeEntity.NUMBER_OFF;
                            obj.POTENTIAL = activeEntity.POTENTIAL;
                            obj.SFL_SHARE = activeEntity.SFL_SHARE;
                            obj.REMARKS = activeEntity.REMARKS;
                            obj.RESPONSIBILITY = activeEntity.RESPONSIBILITY;
                            obj.PENDING = activeEntity.PENDING;
                            obj.FEASIBILITY = activeEntity.FEASIBILITY;
                            obj.REJECT_REASON = activeEntity.REJECT_REASON;
                            obj.LOC_CODE = activeEntity.LOC_CODE;
                            obj.CHEESE_WT = activeEntity.CHEESE_WT;
                            obj.FINISH_WT = activeEntity.FINISH_WT;
                            obj.FINISH_CODE = activeEntity.FINISH_CODE;
                            obj.SUGGESTED_RM = activeEntity.SUGGESTED_RM;
                            obj.RM_COST = activeEntity.RM_COST;
                            obj.FINAL_COST = activeEntity.FINAL_COST;
                            obj.COST_NOTES = activeEntity.COST_NOTES;
                            obj.PROCESSED_BY = activeEntity.PROCESSED_BY;
                            obj.ORDER_DT = activeEntity.ORDER_DT;
                            obj.PRINT = activeEntity.PRINT;
                            obj.ALLOT_PART_NO = activeEntity.ALLOT_PART_NO;
                            obj.PART_NO_REQ_DATE = activeEntity.PART_NO_REQ_DATE;
                            obj.CUST_STD_NO = activeEntity.CUST_STD_NO;
                            obj.CUST_STD_DATE = activeEntity.CUST_STD_DATE;
                            obj.AUTOPART = activeEntity.AUTOPART;
                            obj.SAFTYPART = activeEntity.SAFTYPART;
                            obj.APPLICATION = activeEntity.APPLICATION;
                            obj.STATUS = activeEntity.STATUS;
                            obj.CUSTOMER_NEED_DT = activeEntity.CUSTOMER_NEED_DT;
                            obj.MKTG_COMMITED_DT = activeEntity.MKTG_COMMITED_DT;
                            obj.PPAP_LEVEL = activeEntity.PPAP_LEVEL;
                            obj.DEVL_METHOD = activeEntity.DEVL_METHOD;
                            obj.PPAP_FORGING = activeEntity.PPAP_FORGING;
                            obj.PPAP_SAMPLE = activeEntity.PPAP_SAMPLE;
                            obj.PACKING = activeEntity.PACKING;
                            obj.NATURE_PACKING = activeEntity.NATURE_PACKING;
                            obj.SPL_CHAR = activeEntity.SPL_CHAR;
                            obj.OTHER_CUST_REQ = activeEntity.OTHER_CUST_REQ;
                            obj.ATP_DATE = activeEntity.ATP_DATE;
                            obj.SIMILAR_PART_NO = activeEntity.SIMILAR_PART_NO;
                            obj.GENERAL_REMARKS = activeEntity.GENERAL_REMARKS;
                            obj.MONTHLY = activeEntity.MONTHLY;
                            obj.MKTG_COMMITED_DATE = activeEntity.MKTG_COMMITED_DATE;
                            obj.ROWID = activeEntity.ROWID;
                            obj.COATING_CODE = activeEntity.COATING_CODE;
                            obj.REALISATION = activeEntity.REALISATION;
                            obj.NO_OF_PCS = obj.NO_OF_PCS.ToValueAsString().ToIntValue() <= 0 ? 100 : obj.NO_OF_PCS;
                            obj.ZONE_CODE = activeEntity.ZONE_CODE;
                            obj.RM_FACTOR = activeEntity.RM_FACTOR;
                            obj.IS_COMBINED = activeEntity.IS_COMBINED;
                            //foreach (DDCOST_PROCESS_DATA associationEntity in obj.DDCOST_PROCESS_DATA)
                            //{
                            //    obj.DDCOST_PROCESS_DATA.Remove(associationEntity);
                            //}

                            obj.DELETE_FLAG = false;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                        catch (ChangeConflictException)
                        {
                            foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(RefreshMode.KeepChanges);
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DDCI_INFO.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                        }
                    }
                    else
                    {
                        returnValue = Delete<DDCI_INFO>(new List<DDCI_INFO> { activeEntity });
                        returnValue = Insert<DDCI_INFO>(new List<DDCI_INFO> { activeEntity });
                    }
                }
                if ((entity as DDCOST_PROCESS_DATA).IsNotNullOrEmpty())
                {
                    DDCOST_PROCESS_DATA objChild = null;
                    DDCOST_PROCESS_DATA activeEntityChild = (entity as DDCOST_PROCESS_DATA);

                    objChild = (from row in DB.DDCOST_PROCESS_DATA
                                where row.IDPK == activeEntityChild.IDPK
                                select row).SingleOrDefault<DDCOST_PROCESS_DATA>();

                    if (objChild.IsNotNullOrEmpty())
                    {
                        //objChild = activeEntityChild.DeepCopy<DDCOST_PROCESS_DATA>();

                        returnValue = Delete<DDCOST_PROCESS_DATA>(new List<DDCOST_PROCESS_DATA> { objChild });
                        returnValue = Insert<DDCOST_PROCESS_DATA>(new List<DDCOST_PROCESS_DATA> { objChild });
                        returnValue = true;
                    }
                    else
                    {
                        returnValue = Delete<DDCOST_PROCESS_DATA>(new List<DDCOST_PROCESS_DATA> { activeEntityChild });
                        returnValue = Insert<DDCOST_PROCESS_DATA>(new List<DDCOST_PROCESS_DATA> { activeEntityChild });
                    }
                    returnValue = true;
                }
                if ((entity as DDSTD_NOTES).IsNotNullOrEmpty())
                {
                    DDSTD_NOTES objSTD = null;
                    DDSTD_NOTES activeSTD = (entity as DDSTD_NOTES);

                    objSTD = (from row in DB.DDSTD_NOTES
                              where row.SNO == activeSTD.SNO
                              select row).SingleOrDefault<DDSTD_NOTES>();

                    if (objSTD.IsNotNullOrEmpty())
                    {
                        try
                        {
                            if (objSTD.STD_NOTES != activeSTD.STD_NOTES)
                            {
                                objSTD.STD_NOTES = activeSTD.STD_NOTES;
                                objSTD.ROWID = activeSTD.ROWID;

                                DB.SubmitChanges();

                                ChangeSet cs = DB.GetChangeSet();
                                returnValue = cs.Updates.Count > 0 ? true : false;
                            }
                            returnValue = true;
                        }
                        catch (ChangeConflictException)
                        {
                            foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(RefreshMode.KeepChanges);
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DDSTD_NOTES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objSTD);

                        }
                    }
                    else if (activeSTD.STD_NOTES.ToValueAsString().IsNotNullOrEmpty())
                    {
                        returnValue = Delete<DDSTD_NOTES>(new List<DDSTD_NOTES> { activeSTD });
                        returnValue = Insert<DDSTD_NOTES>(new List<DDSTD_NOTES> { activeSTD });
                    }
                }

                if ((entity as DDSHAPE_DETAILS).IsNotNullOrEmpty())
                {
                    DDSHAPE_DETAILS objSHAPE = null;
                    DDSHAPE_DETAILS activeSHAPE = (entity as DDSHAPE_DETAILS);

                    objSHAPE = (from row in DB.DDSHAPE_DETAILS
                                where row.IDPK == activeSHAPE.IDPK
                                select row).SingleOrDefault<DDSHAPE_DETAILS>();

                    if (objSHAPE.IsNotNullOrEmpty())
                    {
                        try
                        {
                            objSHAPE.CI_REFERENCE = activeSHAPE.CI_REFERENCE;
                            objSHAPE.SHAPE_CODE = activeSHAPE.SHAPE_CODE;
                            objSHAPE.WEIGHT_OPTION = activeSHAPE.WEIGHT_OPTION;
                            objSHAPE.HEAD1 = activeSHAPE.HEAD1;
                            objSHAPE.VAL1 = activeSHAPE.VAL1;
                            objSHAPE.HEAD2 = activeSHAPE.HEAD2;
                            objSHAPE.VAL2 = activeSHAPE.VAL2;
                            objSHAPE.HEAD3 = activeSHAPE.HEAD3;
                            objSHAPE.VAL3 = activeSHAPE.VAL3;
                            objSHAPE.VOLUME = activeSHAPE.VOLUME;
                            objSHAPE.SIGN = activeSHAPE.SIGN;
                            objSHAPE.SNO = activeSHAPE.SNO;
                            objSHAPE.ROWID = activeSHAPE.ROWID;
                            objSHAPE.CIREF_NO_FK = activeSHAPE.CIREF_NO_FK;

                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                        catch (ChangeConflictException)
                        {
                            foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                            {
                                conflict.Resolve(RefreshMode.KeepChanges);
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DDSTD_NOTES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objSHAPE);

                        }
                    }
                    else
                    {
                        returnValue = Delete<DDSHAPE_DETAILS>(new List<DDSHAPE_DETAILS> { activeSHAPE });
                        returnValue = Insert<DDSHAPE_DETAILS>(new List<DDSHAPE_DETAILS> { activeSHAPE });
                    }
                }
            }
            returnValue = true;
            return returnValue;
        }

        public bool Delete<T>(List<T> entities)
        {
            bool returnValue = (!entities.IsNotNullOrEmpty() || (entities.IsNotNullOrEmpty() && entities.Count == 0 ? true : false));
            foreach (T entity in entities)
            {
                if ((entity as DDCI_INFO).IsNotNullOrEmpty())
                {
                    DDCI_INFO obj = null;
                    DDCI_INFO activeEntity = (entity as DDCI_INFO);
                    try
                    {
                        obj = (from row in DB.DDCI_INFO
                               where row.IDPK == activeEntity.IDPK
                               select row).SingleOrDefault<DDCI_INFO>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.DELETE_FLAG = true;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Updates.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDCI_INFO.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                    }
                }
                if ((entity as DDCOST_PROCESS_DATA).IsNotNullOrEmpty())
                {
                    DDCOST_PROCESS_DATA objChild = null;
                    DDCOST_PROCESS_DATA activeEntityChild = (entity as DDCOST_PROCESS_DATA);
                    try
                    {
                        objChild = (from row in DB.DDCOST_PROCESS_DATA
                                    where row.IDPK == activeEntityChild.IDPK
                                    select row).SingleOrDefault<DDCOST_PROCESS_DATA>();

                        if (objChild.IsNotNullOrEmpty())
                        {
                            DB.DDCOST_PROCESS_DATA.DeleteOnSubmit(objChild);
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Deletes.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDCOST_PROCESS_DATA.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objChild);

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
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Deletes.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDSTD_NOTES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objSTD);

                    }
                }

                if ((entity as DDSHAPE_DETAILS).IsNotNullOrEmpty())
                {
                    DDSHAPE_DETAILS objSHAPE = null;
                    DDSHAPE_DETAILS activeSHAPE = (entity as DDSHAPE_DETAILS);
                    try
                    {
                        objSHAPE = (from row in DB.DDSHAPE_DETAILS
                                    where row.IDPK == activeSHAPE.IDPK
                                    select row).SingleOrDefault<DDSHAPE_DETAILS>();

                        if (objSHAPE.IsNotNullOrEmpty())
                        {
                            DB.DDSHAPE_DETAILS.DeleteOnSubmit(objSHAPE);
                            DB.SubmitChanges();

                            ChangeSet cs = DB.GetChangeSet();
                            returnValue = cs.Deletes.Count > 0 ? true : false;
                            returnValue = true;
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDSTD_NOTES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, objSHAPE);

                    }
                }
            }
            return returnValue;
        }

    }
}
