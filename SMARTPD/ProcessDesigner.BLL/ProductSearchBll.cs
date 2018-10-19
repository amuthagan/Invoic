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
    public class ProductSearchBll : Essential
    {
        public ProductSearchBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DDRM_MAST> GetRMSpecCombo()
        {
            List<DDRM_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.DDRM_MAST
                             where row.DELETE_FLAG == false || row.DELETE_FLAG == null
                             select row).ToList<DDRM_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        public List<DDFINISH_MAST> GetFinishCombo()
        {
            List<DDFINISH_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.DDFINISH_MAST
                             //where row.DELETE_FLAG == false || row.DELETE_FLAG == null
                             select row).ToList<DDFINISH_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        public List<DDLOC_MAST> GetLocationCombo()
        {
            List<DDLOC_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.DDLOC_MAST
                             //where row.DELETE_FLAG == false || row.DELETE_FLAG == null
                             select row).ToList<DDLOC_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }



        public List<DDCOST_CENT_MAST> GetCostCentreCombo(string locationcode)
        {
            List<DDCOST_CENT_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (locationcode.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDCOST_CENT_MAST
                                 //where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                 where
                                 row.LOC_CODE.ToUpper() == locationcode.ToValueAsString().ToUpper()
                                 select row).ToList<DDCOST_CENT_MAST>();
                }
                else
                {
                    lstEntity = (from row in DB.DDCOST_CENT_MAST
                                 where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                 select row).ToList<DDCOST_CENT_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        public List<DDOPER_MAST> GetOperationCombo()
        {
            List<DDOPER_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.DDOPER_MAST
                             //where row.DELETE_FLAG == false || row.DELETE_FLAG == null
                             select row).ToList<DDOPER_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        public List<DDCUST_MAST> GetCustomerCombo()
        {
            List<DDCUST_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.DDCUST_MAST
                             //where row.DELETE_FLAG == false || row.DELETE_FLAG == null
                             select row).ToList<DDCUST_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        /// <summary>
        /// This function is used to fetch all the rows from the table FASTENERS_MASTER
        /// </summary>
        /// <returns></returns>
        public List<FASTENERS_MASTER> GetFamilyCombo()
        {
            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.FASTENERS_MASTER
                             //where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                             where row.TYPE == null && row.SUBTYPE == null
                             select row).ToList<FASTENERS_MASTER>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }


        /// <summary>
        /// This function is used load the type 'TYPE','HEAD FORMS','SHANK FORM','END FORM','DRIVING FEATURE','ADDITIONAL FEATURE','KEYWORDS'
        /// </summary>
        /// <param name="productcategory"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<FASTENERS_MASTER> GetTypeFastenersCombo(string family, string type)
        {
            List<FASTENERS_MASTER> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (family.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 //where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                 where row.TYPE.ToUpper() == type.ToUpper()
                                 && row.SUBTYPE != null && row.PRODUCT_CATEGORY.ToUpper() == family.ToValueAsString().ToUpper()
                                 select row).ToList<FASTENERS_MASTER>();
                }
                else
                {
                    lstEntity = (from row in DB.FASTENERS_MASTER
                                 //where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                 where row.TYPE.ToUpper() == type.ToUpper()
                                 && row.SUBTYPE != null
                                 select row).ToList<FASTENERS_MASTER>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        public List<PRD_CLASS_MAST> GetProductFamilyCombo()
        {
            List<PRD_CLASS_MAST> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                lstEntity = (from row in DB.PRD_CLASS_MAST
                             select row).ToList<PRD_CLASS_MAST>();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return lstEntity;
        }

        public DataView GetHeadStyleCombo()
        {
            DataTable dataEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                dataEntity = ToDataTable((from c in DB.PRD_FAMILY
                                          select new { c.HEAD_STYLE }).Distinct().ToList());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dataEntity.DefaultView;
        }

        public DataView GetProductTypeCombo()
        {
            DataTable dataEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                dataEntity = ToDataTable((from c in DB.PRD_FAMILY
                                          select new { c.TYPE }).Distinct().ToList());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dataEntity.DefaultView;
        }

        public DataView GetApplicationCombo()
        {
            DataTable dataEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                dataEntity = ToDataTable((from c in DB.PRD_FAMILY
                                          select new { c.SUB_TYPE }).Distinct().ToList());
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dataEntity.DefaultView;
        }


        public DataView GetProductSearchDetails(ProductSearchModel productSearchModel)
        {
            StringBuilder sbsql = new StringBuilder();
            StringBuilder sbTables = new StringBuilder();
            StringBuilder sbCondition = new StringBuilder();
            int varPrdMast = 0;
            int varCIreference = 0;
            //string varCondition = "";
            int varProcessMain = 0;
            int varProcessSheet = 0;
            int varProcessCC = 0;
            int varToolSchedule = 0;

            try
            {

                sbsql.Append("SELECT isnull(P.PART_NO,'') PART_NO,isnull(P.PART_DESC,'') PART_DESC,'' as CC_CODE,");
                sbsql.Append("'' WIRE_SIZE_MAX, '' WIRE_SIZE_MIN,isnull(P.QUALITY,'') QUALITY ");
                sbsql.Append(",''  CUST_DWG_NO,'' as CUST_NAME ");
                sbsql.Append(" FROM PRD_MAST  P ");
                if (productSearchModel.HeatTreatment.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.HEAT_TREATMENT_DESC) LIKE '%" + sqlEncode(productSearchModel.HeatTreatment.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.Quality.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.QUALITY) LIKE '%" + sqlEncode(productSearchModel.Quality.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.ThreadSize.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.DIA_CD) LIKE '%" + sqlEncode(productSearchModel.ThreadSize.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }
                if (productSearchModel.ThreadClass.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.THREAD_CLASS) LIKE '%" + sqlEncode(productSearchModel.ThreadClass.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.ThreadStd.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.THREAD_STD) LIKE '%" + sqlEncode(productSearchModel.ThreadStd.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }


                if (productSearchModel.ManufacturedAt.ToValueAsString().Trim() != "" && productSearchModel.ManufacturedAtText.ToValueAsString().Trim() != "") //ltbLocation
                {
                    //sbCondition.Append(" AND  UPPER(P.BIF_PROJ) = '" + sqlEncode(productSearchModel.ManufacturedAt.ToValueAsString().ToUpper().Trim().Substring(0, 1)) + "'");
                    //Search result not displayed with combination of "Manufactured at" ex : MM, KK ...
                    sbCondition.Append(" AND  UPPER(P.BIF_PROJ) = '" + sqlEncode(productSearchModel.ManufacturedAt.ToValueAsString().ToUpper().Trim()) + "'");
                    varPrdMast = 1;
                }


                if (productSearchModel.Description.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.PART_DESC) LIKE '%" + sqlEncode(productSearchModel.Description.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.Keyword.ToValueAsString().Trim() != "" && productSearchModel.KeywordText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.KEYWORDS) LIKE '%" + sqlEncode(productSearchModel.Keyword.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }



                if (productSearchModel.HeadForm.ToValueAsString().Trim() != "" && productSearchModel.HeadFormText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.HEAD_STYLE) LIKE '%" + sqlEncode(productSearchModel.HeadForm.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }


                if (productSearchModel.ShankForm.ToValueAsString().Trim() != "" && productSearchModel.ShankFormText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  UPPER(P.APPLICATION) LIKE '%" + sqlEncode(productSearchModel.ShankForm.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }


                if (productSearchModel.EndForm.ToValueAsString().Trim() != "" && productSearchModel.EndFormText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  P.PRD_CLASS_CD like '%" + sqlEncode(productSearchModel.EndForm.ToValueAsString().ToUpper()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.AdditionalFeature.ToValueAsString().Trim() != "" && productSearchModel.AdditionalFeatureText.ToValueAsString().ToUpper().Trim() != "")
                {
                    sbCondition.Append(" AND  P.ADDL_FEATURE LIKE '%" + sqlEncode(productSearchModel.AdditionalFeature.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.DrivingFeature.ToValueAsString().Trim() != "" && productSearchModel.DrivingFeatureText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  P.PRD_GRP_CD LIKE '%" + sqlEncode(productSearchModel.DrivingFeature.ToValueAsString().ToUpper().Trim()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.ProductFamily.ToValueAsString().Trim() != "" && productSearchModel.ProductFamilyText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  P.FAMILY LIKE '%" + sqlEncode(productSearchModel.ProductFamily.ToValueAsString().ToUpper()) + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.ProductType.ToValueAsString().Trim() != "" && productSearchModel.ProductTypeText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  P.TYPE LIKE '%" + sqlEncode(productSearchModel.ProductType.ToValueAsString()).ToUpper().Trim() + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.FamilyType.ToValueAsString().Trim() != "" && productSearchModel.FamilyTypeText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  P.TYPE LIKE '%" + sqlEncode(productSearchModel.FamilyType.ToValueAsString()).ToUpper().Trim() + "%'");
                    varPrdMast = 1;
                }

                

                if (productSearchModel.HeadStyle.ToValueAsString().Trim() != "" && productSearchModel.HeadStyleText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  P.HEAD_STYLE LIKE '%" + sqlEncode(productSearchModel.HeadStyle.ToValueAsString()).ToUpper().Trim() + "%'");
                    varPrdMast = 1;
                }

                if (productSearchModel.Application.ToValueAsString().Trim() != "" && productSearchModel.ApplicationText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  P.APPLICATION LIKE '%" + sqlEncode(productSearchModel.Application.ToValueAsString()).ToUpper().Trim() + "%'");
                    varPrdMast = 1;
                }


                if (productSearchModel.Family.ToValueAsString().Trim() != "" && productSearchModel.FamilyText.ToValueAsString().Trim().Trim() != "")
                {
                    if (productSearchModel.FamilyText.ToValueAsString().ToUpper().Trim() == "EXTERNAL THREADED")
                        sbCondition.Append(" AND ( P.FAMILY LIKE '%" + sqlEncode(productSearchModel.Family.ToValueAsString().ToUpper().Trim()) + "%' OR P.FAMILY='02') ");
                    else if (productSearchModel.FamilyText.ToValueAsString().ToUpper().Trim() == "INTERNAL THREADED")
                        sbCondition.Append(" AND ( P.FAMILY LIKE '%" + sqlEncode(productSearchModel.Family.ToValueAsString().ToUpper().Trim()) + "%' OR P.FAMILY='09') ");
                    else if (productSearchModel.FamilyText.ToValueAsString().ToUpper().Trim() == "NON THREADED")
                        sbCondition.Append(" AND ( P.FAMILY LIKE '%" + sqlEncode(productSearchModel.Family.ToValueAsString().ToUpper().Trim()) + "%' OR P.FAMILY='06') ");
                    else
                    {
                        sbCondition.Append(" and ( p.family like '%" + sqlEncode(productSearchModel.Family.ToValueAsString().Trim()) + "%') ");
                    }
                    varPrdMast = 1;
                }


                //By Customer Details
                if (productSearchModel.Customer.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND I.CUST_CODE = " + productSearchModel.Customer.ToValueAsString().ToUpper().Trim().Replace("'", ""));
                    varPrdMast = 1;
                    varCIreference = 1;
                }


                if (productSearchModel.Finish.ToValueAsString().Trim() != "" && productSearchModel.FinishText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND  I.FINISH_CODE = '" + sqlEncode(productSearchModel.Finish.ToValueAsString().ToUpper().Trim()) + "'");
                    varPrdMast = 1;
                    varCIreference = 1;
                }

                if (productSearchModel.CustDrwgNo.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND I.CUST_DWG_NO LIKE '%" + sqlEncode(productSearchModel.CustDrwgNo.ToValueAsString().Trim()) + "%'");
                    varPrdMast = 1;
                    varCIreference = 1;
                }

                //By Process Main
                if (productSearchModel.RMSpec.ToValueAsString().Trim() != "" && productSearchModel.RMSpecText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND M.RM_CD = '" + sqlEncode(productSearchModel.RMSpec.ToValueAsString().Trim()) + "'");
                    varPrdMast = 1;
                    varProcessMain = 1;
                }

                //Thru Process Sheet
                if (productSearchModel.Operation.ToValueAsString().Trim() != "" && productSearchModel.OperationText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND UPPER(S.OPN_CD) = '" + sqlEncode(productSearchModel.Operation.ToValueAsString().Trim()) + "'");
                    varPrdMast = 1;
                    varProcessSheet = 1;
                }

                //Thru Process Cost Centre
                if (productSearchModel.CostCentre.ToValueAsString().Trim() != "" && productSearchModel.CostCentreText.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND UPPER(PROCESS_CC.CC_CODE) = '" + sqlEncode(productSearchModel.CostCentre.ToValueAsString().Trim()) + "'");
                    varPrdMast = 1;
                    varProcessCC = 1;
                }

                if (productSearchModel.MinRMSize.ToValueAsString().Trim() != "" && productSearchModel.MaxRMSize.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" and ISNULL(PROCESS_CC.WIRE_SIZE_MIN , 0) between  " + productSearchModel.MinRMSize.ToValueAsString() + " and " + productSearchModel.MaxRMSize.ToValueAsString());
                    sbCondition.Append(" and ISNULL(PROCESS_CC.WIRE_SIZE_MAX , 0) between  " + productSearchModel.MinRMSize.ToValueAsString() + " and " + productSearchModel.MaxRMSize.ToValueAsString());
                    varPrdMast = 1;
                    varProcessCC = 1;
                }
                else if (productSearchModel.MinRMSize.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" and ISNULL(PROCESS_CC.WIRE_SIZE_MIN , 0) >= " + productSearchModel.MinRMSize.ToValueAsString());
                    sbCondition.Append(" and ISNULL(PROCESS_CC.WIRE_SIZE_MAX , 0) >= " + productSearchModel.MinRMSize.ToValueAsString());  
                    varPrdMast = 1;
                    varProcessCC = 1;
                }
                else if (productSearchModel.MaxRMSize.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" and ISNULL(PROCESS_CC.WIRE_SIZE_MIN , 0) <=  " + productSearchModel.MaxRMSize.ToValueAsString());
                    sbCondition.Append(" and ISNULL(PROCESS_CC.WIRE_SIZE_MAX , 0) <=  " + productSearchModel.MaxRMSize.ToValueAsString());
                    varPrdMast = 1;
                    varProcessCC = 1;
                }

                //Thru Tool Schedule

                if (productSearchModel.ToolCode.ToValueAsString().Trim() != "")
                {
                    sbCondition.Append(" AND UPPER(C.TOOL_CODE) ='" + sqlEncode(productSearchModel.ToolCode.ToValueAsString().ToUpper().Trim()) + "'");
                    varToolSchedule = 1;
                    varPrdMast = 1;
                }

                if (varProcessMain == 1)
                {
                    sbCondition.Append(" AND M.PART_NO = P.PART_NO ");
                    sbTables.Append(",PROCESS_MAIN M ");
                }

                if (varProcessSheet == 1)
                {
                    sbCondition.Append(" AND S.PART_NO = P.PART_NO ");
                    sbTables.Append(",PROCESS_SHEET S ");
                }

                if (varProcessCC == 1)
                {
                    sbCondition.Append(" and process_cc.Part_No = p.Part_No ");
                    sbTables.Append(",Process_cc ");
                }

                if (varToolSchedule == 1)
                {
                    sbCondition.Append(" AND C.PART_NO = P.PART_NO ");
                    sbTables.Append(",TOOL_SCHED_SUB C ");
                }

                if (varCIreference == 1)
                {
                    sbCondition.Append(" AND R.CI_REF=I.CI_REFERENCE AND R.SNO=1 AND  R.PART_NO = P.PART_NO AND R.CURRENT_CIREF = 1 ");
                    sbTables.Append(",PRD_CIREF R,DDCI_INFO I ");
                }

                if (productSearchModel.SpecialParts == true)
                {
                    sbCondition.Append(" AND ( UPPER(P.PART_NO) LIKE 'M%' OR UPPER(P.PART_NO) LIKE 'S%') ");
                    varPrdMast = 1;
                }

                //Display the deleted product 
                //sbCondition.Append(" AND ISNULL(P.DELETE_FLAG,0)=0 ");
                sbCondition.Append(" AND P.PART_NO IS NOT NULL And RTRIM(LTRIM(P.PART_NO)) != '' ");


                if (varPrdMast == 1)
                {
                    //sbsql.Append(sbTables.ToString() + " where " + Right$(varCondition, Len(varCondition) - 5)
                    sbsql.Append(sbTables.ToString() + " WHERE " + sbCondition.ToString().Substring(4));

                    if (varProcessMain == 1 || varProcessSheet == 1 || varProcessCC == 1 || varToolSchedule == 1 || varCIreference == 1)
                    {
                        sbsql.Append(" group by p.part_no,p.part_desc,p.quality  ");
                    }
                }
                else
                {
                    sbsql.Append(" WHERE P.PART_NO IS NOT NULL And RTRIM(LTRIM(P.PART_NO)) != '' ");
                }

                sbsql.Append(" order by PART_NO");
                /*
                if InStr(1, Sql, "from") = 0 Then
                Sql = "select part_no,part_desc,quality from prd_mast"
            End If
            ssPartNo.RemoveAll
            */
                //var GetCollection = ToDataTable(DB.ExecuteQuery<CostSheetSearchModel>(sbsql.ToString()).ToList());
                var getcollection = ToDataTable(DB.ExecuteQuery<GridProductSearchModel>(sbsql.ToString()).ToList());



                sbsql = new StringBuilder();
                sbsql.Append("select PROCESSCC.PART_NO,");
                sbsql.Append("ISNULL(PROCESSCC.CC_CODE,'') as CC_CODE,");
                sbsql.Append("convert(varchar, cast(PROCESSCC.WIRE_SIZE_MAX as money)) WIRE_SIZE_MAX, convert(varchar, cast(PROCESSCC.WIRE_SIZE_MIN as money)) WIRE_SIZE_MIN ");
                sbsql.Append(" FROM PROCESS_CC PROCESSCC ");
                sbsql.Append(" WHERE ROUTE_NO = (SELECT MIN(ROUTE_NO) FROM PROCESS_MAIN WHERE PROCESS_MAIN.PART_NO = PROCESSCC.PART_NO AND CURRENT_PROC = 1) ");
                sbsql.Append(" AND SEQ_NO = (SELECT MIN(SEQ_NO) FROM PROCESS_SHEET WHERE PROCESS_SHEET.PART_NO = PROCESSCC.PART_NO AND ");
                sbsql.Append(" ROUTE_NO = (SELECT MIN(ROUTE_NO) FROM PROCESS_MAIN WHERE PROCESS_MAIN.PART_NO = PROCESSCC.PART_NO AND ");
                sbsql.Append(" CURRENT_PROC = 1) AND LTRIM(OPN_DESC) LIKE 'FORG%') AND CC_SNO = 1 ");

                if (productSearchModel.MinRMSize.ToValueAsString().Trim() != "" && productSearchModel.MaxRMSize.ToValueAsString().Trim() != "")
                {
                    sbsql.Append(" and ISNULL(PROCESSCC.WIRE_SIZE_MIN , 0) between  " + productSearchModel.MinRMSize.ToValueAsString() + " and " + productSearchModel.MaxRMSize.ToValueAsString());
                    sbsql.Append(" and ISNULL(PROCESSCC.WIRE_SIZE_MAX , 0) between  " + productSearchModel.MinRMSize.ToValueAsString() + " and " + productSearchModel.MaxRMSize.ToValueAsString());                   
                }
                else if (productSearchModel.MinRMSize.ToValueAsString().Trim() != "")
                {
                    sbsql.Append(" and ISNULL(PROCESSCC.WIRE_SIZE_MIN , 0) >= " + productSearchModel.MinRMSize.ToValueAsString());
                    sbsql.Append(" and ISNULL(PROCESSCC.WIRE_SIZE_MAX , 0) >= " + productSearchModel.MinRMSize.ToValueAsString());                  
                }
                else if (productSearchModel.MaxRMSize.ToValueAsString().Trim() != "")
                {
                    sbsql.Append(" and ISNULL(PROCESSCC.WIRE_SIZE_MIN , 0) <=  " + productSearchModel.MaxRMSize.ToValueAsString());
                    sbsql.Append(" and ISNULL(PROCESSCC.WIRE_SIZE_MAX , 0) <=  " + productSearchModel.MaxRMSize.ToValueAsString());                   
                }

                var getProcess = ToDataTable(DB.ExecuteQuery<GridProductSearchModel>(sbsql.ToString()).ToList());

                sbsql = new StringBuilder();
                sbsql.Append("select b.part_no,a.cust_dwg_no, c.cust_name from ddci_info a, prd_ciref b, ddcust_mast c where a.ci_reference = b.ci_ref and b.CURRENT_CIREF = 1 ");
                sbsql.Append(" and a.cust_code = c.cust_code ");

                var getDrawing = ToDataTable(DB.ExecuteQuery<GridProductSearchModel>(sbsql.ToString()).ToList());

                DataRow[] drSelect;
                foreach (DataRow drRow in getcollection.Rows)
                {
                    try
                    {
                        drSelect = getProcess.Select("PART_NO='" + drRow["PART_NO"].ToValueAsString() + "'");
                        if (drSelect.Length > 0)
                        {
                            drRow["CC_CODE"] = drSelect[0]["CC_CODE"];
                            drRow["WIRE_SIZE_MAX"] = drSelect[0]["WIRE_SIZE_MAX"];
                            drRow["WIRE_SIZE_MIN"] = drSelect[0]["WIRE_SIZE_MIN"];
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        drSelect = getDrawing.Select("PART_NO='" + drRow["PART_NO"].ToValueAsString() + "'");
                        if (drSelect.Length > 0)
                        {
                            drRow["CUST_DWG_NO"] = drSelect[0]["CUST_DWG_NO"];
                            drRow["CUST_NAME"] = drSelect[0]["CUST_NAME"];
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return getcollection.DefaultView;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return null;
        }


        private string sqlEncode(string sqlValue)
        {
            return sqlValue.Replace("'", "''");
        }

        public string ShowDrawing(string part_no)
        {
            PROD_DRAWING ddDrawing = null;
            //PROD_DRAWING 

            //byte[] photosource = null;
            //System.IO.MemoryStream strm=null;
            try
            {
                ddDrawing = (from c in DB.PROD_DRAWING
                             where c.PART_NO == part_no
                             select c).FirstOrDefault<PROD_DRAWING>();
                if (ddDrawing.IsNotNullOrEmpty())
                {
                    if (ddDrawing.PRD_DWG != null)
                    {
                        return ddDrawing.PRD_DWG.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
                //return "";
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return "";
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


    }
}
