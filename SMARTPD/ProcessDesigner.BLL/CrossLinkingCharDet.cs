using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{

    public class CrossLinkingCharBll : Essential
    {
        public CrossLinkingCharBll(UserInformation userInformation)
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

        /// <summary>
        /// Product Type Based Bottom Grid Datas Loaded
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        /// <param name="featureCode"></param>
        /// <returns></returns>
        public bool GetGridDetailsProdCategCross(CrossLinkingCharModel crossModel)
        {
            DataSet dsMaster = new DataSet();
            List<StringBuilder> sbSQL = new List<StringBuilder>();
            try
            {
                FASTENERS_MASTER fas = new FASTENERS_MASTER();

                lstDbParameter.Clear();
                sbSQL.Add(new StringBuilder("select SUBTYPE,feature_code,g.type,sub_type,linked_with,sno,g.prd_code from forging_master g, fasteners_master m where M.prd_code=g.prd_code and (g.delete_flag='false' or g.delete_flag is null)  and (linked_with ='' or linked_with is null)"));
                //lstDbParameter.Add(DBParameter("type", crossModel.Type));
                //lstDbParameter.Add(DBParameter("subType", crossModel.ProductType));
                //lstDbParameter.Add(DBParameter("featureCode", crossModel.FeatureCode));
                dsMaster = Dal.GetDataSet(sbSQL);
                if (dsMaster.Tables[0] != null)
                {
                    crossModel.DtClassificationPrdType = dsMaster.Tables[0].DefaultView;
                    //crossModel.DtClassificationPrdType.AddNew();

                }
                else
                {
                    crossModel.DtClassificationPrdType = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Linked Type Based Bottom Grid Datas Loaded
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        /// <param name="featureCode"></param>
        /// <returns></returns>
        public bool GetGridDetailsLinkedWithCross(CrossLinkingCharModel crossModel)
        {
            DataSet dsMaster = new DataSet();
            List<StringBuilder> sbSQL = new List<StringBuilder>();
            try
            {
                //   if (!type.IsNotNullOrEmpty() || !subType.IsNotNullOrEmpty() || featureCode.IsNotNullOrEmpty()) return dsMaster;
                sbSQL.Add(new StringBuilder("select SUBTYPE,feature_code,g.type,sub_type,linked_with,sno,g.prd_code from forging_master g, fasteners_master m where M.prd_code=g.prd_code and (g.delete_flag='false' or g.delete_flag is null) and (linked_with !='' and linked_with is not null)"));
                lstDbParameter.Clear();
                //lstDbParameter.Add(DBParameter("type", crossModel.Type));
                //lstDbParameter.Add(DBParameter("subType", crossModel.ProductType));
                //lstDbParameter.Add(DBParameter("featureCode", crossModel.FeatureCode));
                dsMaster = Dal.GetDataSet(sbSQL);
                if (dsMaster.Tables[0] != null)
                {
                    crossModel.DtClassificationlinkedWith = dsMaster.Tables[0].DefaultView;
                    //  crossModel.DtClassificationlinkedWith.AddNew();
                }
                else
                {
                    crossModel.DtClassificationlinkedWith = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Feature Grid Datas
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        /// <param name="featureCode"></param>
        /// <returns></returns>
        public bool GetGridFeature(CrossLinkingCharModel crossModel)
        {
            DataSet dsMaster = new DataSet();
            DataTable dtMaster = null;
            if (!crossModel.ProductType.IsNotNullOrEmpty()) crossModel.ProductType = "";
            if (!crossModel.Type.IsNotNullOrEmpty()) crossModel.Type = "";
            List<StringBuilder> sbSQL = new List<StringBuilder>();
            sbSQL.Add(new StringBuilder("select distinct g.feature_code,feature_desc as Feature from forging_master g ,feature_master m where m.feature_code=g.feature_code and OPER_CODE='1030' and type=  " + ToDbParameter("type") + " and sub_type=  " + ToDbParameter("subType") + "  and (m.delete_flag='false' or m.delete_flag is null) order by feature_code")); //output
            lstDbParameter.Clear();
            lstDbParameter.Add(DBParameter("type", crossModel.Type));
            lstDbParameter.Add(DBParameter("subType", crossModel.ProductType));
            dtMaster = Dal.GetDataSet(sbSQL, lstDbParameter).Tables[0];
            dsMaster.Tables.Add(dtMaster.Copy());
            dsMaster.Tables[dsMaster.Tables.Count - 1].Namespace = "LEFT_OUTPUT";
            //return dsMaster;
            if (dsMaster.Tables[0] != null)
            {
                crossModel.DtFeatureDetails = dsMaster.Tables[0].DefaultView;
                DataRowView drv = crossModel.DtFeatureDetails.AddNew();
                drv.BeginEdit();
                drv["feature_code"] = GenerateFeatuerCode();
                drv.EndEdit();
            }
            else
            {
                crossModel.DtFeatureDetails = null;
            }

            return true;
        }

        /// <summary>
        /// Characteristics Grid Datas
        /// </summary>
        /// <param name="featureCode"></param>
        /// <returns></returns>
        /// 
        public bool GetGridCharacteristicsMaster(CrossLinkingCharModel crossModel)
        {

            DataSet dsMaster = new DataSet();
            DataTable dtMaster;
            //  if (!featureCode.IsNotNullOrEmpty()) return dsMaster;
            List<StringBuilder> sbSQL = new List<StringBuilder>();
            sbSQL.Add(new StringBuilder("select oper_code,feature_code,feature_desc,sno,measuring_technique,sample_size,sample_frequency,control_method,reaction_plan  from feature_master where feature_code like 'F%' and OPER_CODE='1030' and (delete_flag='false' or delete_flag is null)"));
            lstDbParameter.Clear();
            // lstDbParameter.Add(DBParameter("featureCode", featureCode));
            dtMaster = Dal.GetDataSet(sbSQL).Tables[0];
            dsMaster.Tables.Add(dtMaster.Copy());
            dsMaster.Tables[dsMaster.Tables.Count - 1].Namespace = "RIGHT_OUTPUT";
            //return dsMaster;
            if (dsMaster.Tables[0] != null)
            {
                crossModel.DtCharacteristicsDetails = dsMaster.Tables[0].DefaultView;
                crossModel.DtCharacteristicsDetails.AddNew();
            }
            else
            {
                crossModel.DtCharacteristicsDetails = null;
            }

            return true;
        }
        //public DataSet GetGridCharacteristicsMaster()
        //{

        //    DataSet dsMaster = new DataSet();
        //    DataTable dtMaster;
        //    //  if (!featureCode.IsNotNullOrEmpty()) return dsMaster;
        //    List<StringBuilder> sbSQL = new List<StringBuilder>();
        //   // sbSQL.Add(new StringBuilder("select oper_code,feature_code,feature_desc,sno,measuring_technique,sample_size,sample_frequency,control_method,reaction_plan  from feature_master where feature_code like 'F%' and OPER_CODE='1030' and (delete_flag='false' or delete_flag is null) and Feature_code= " + ToDbParameter("featureCode")));
        //    sbSQL.Add(new StringBuilder("select oper_code,feature_code,feature_desc,sno,measuring_technique,sample_size,sample_frequency,control_method,reaction_plan  from feature_master where feature_code like 'F%' and OPER_CODE='1030' and (delete_flag='false' or delete_flag is null)"));
        //    //lstDbParameter.Clear();
        //    //lstDbParameter.Add(DBParameter("featureCode", featureCode));
        //    dtMaster = Dal.GetDataSet(sbSQL).Tables[0];
        //    dsMaster.Tables.Add(dtMaster.Copy());
        //    dsMaster.Tables[dsMaster.Tables.Count - 1].Namespace = "RIGHT_OUTPUT";
        //    return dsMaster;
        //}


        // Combo 
        /// <summary>
        /// Type Data Combo Loaded
        /// </summary>
        /// <returns></returns>
        public bool GetTypeCmb(CrossLinkingCharModel crossModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.TYPE == null && o.SUBTYPE == null && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PRODUCT_CATEGORY }).ToList());
                if (dt != null)
                {
                    crossModel.DtCmbType = dt.DefaultView;
                    return true;

                }
                else
                {
                    crossModel.DtCmbType = null;
                    return true;
                }


            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
                // throw ex.LogException();
            }

            //DataSet dsMaster = new DataSet();
            //List<StringBuilder> sbSQL = new List<StringBuilder>();
            //try
            //{
            //    Dal = userInformation.Dal;
            //    sbSQL.Add(new StringBuilder("select PRODUCT_CATEGORY from fasteners_master where type is null and subtype is null and (delete_flag='false' or delete_flag is null)")); // Type
            //    dsMaster = Dal.GetDataSet(sbSQL, null);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
        }

        /// <summary>
        /// Product Type Combo Loaded
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        public bool GetProductTypeCmb(CrossLinkingCharModel crossModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.TYPE != null && o.SUBTYPE == null && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null)) && o.PRODUCT_CATEGORY == crossModel.Type
                                  select new { o.TYPE, o.PRD_CODE }).Distinct().ToList());
                if (dt != null)
                {
                    crossModel.DtCmbPrdType = dt.DefaultView;

                }
                else
                {
                    crossModel.DtCmbPrdType = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            //sbSQL = new StringBuilder();
            //try
            //{
            //    Dal = userInformation.Dal;
            //    sqlDictionary = new Dictionary<string, StringBuilder>();
            //    sbSQL.Append("select type,prd_code from fasteners_master where type is not  null and subtype is  null and (delete_flag='false' or delete_flag is null) and product_category = " + ToDbParameter("pi_productCategory"));
            //    sqlDictionary.Add("fasteners_master", sbSQL);
            //    lstDbParameter.Clear();
            //    lstDbParameter.Add(DBParameter("pi_productCategory", productCategory));
            //    dsResult = Dal.GetDataSet(sqlDictionary, lstDbParameter);
            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
            //return dsResult;
        }

        /// <summary>
        /// Linked With Combo Loaded
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        public bool GetLinkedTypeCmb(CrossLinkingCharModel crossModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.TYPE == null && o.SUBTYPE != null && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null)) && o.PRODUCT_CATEGORY == crossModel.Type
                                  select new { o.SUBTYPE, o.PRD_CODE }).Distinct().ToList());
                if (dt != null)
                {
                    crossModel.DtCmbLinkedWith = dt.DefaultView;

                }
                else
                {
                    crossModel.DtCmbLinkedWith = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            //sbSQL = new StringBuilder();
            //try
            //{
            //    Dal = userInformation.Dal;
            //    sbSQL = new StringBuilder();
            //    sbSQL.Append("select SUBTYPE,prd_code from fasteners_master where type is null and subtype is not null and (delete_flag='false' or delete_flag is null) and  product_category = " + ToDbParameter("pi_productCategory"));
            //    sqlDictionary.Add("fasteners_master_Subtype", sbSQL);
            //    lstDbParameter.Clear();
            //    lstDbParameter.Add(DBParameter("pi_productCategory", productCategory));
            //    dsResult = Dal.GetDataSet(sqlDictionary, lstDbParameter);
            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
            //return dsResult;
        }

        /// <summary>
        /// Product Type Based Bottom Grid --> Combo Datas Loaded
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public bool GetGridComboProdCategCross(CrossLinkingCharModel crossModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.SUBTYPE != null && o.PRODUCT_CATEGORY == crossModel.Type && o.TYPE == crossModel.ProductType && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.SUBTYPE, o.PRD_CODE }).ToList());
                if (dt != null)
                {
                    crossModel.DtClassificationCmbPrdType = dt.DefaultView;

                }
                else
                {
                    crossModel.DtClassificationCmbPrdType = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            //DataSet dsMaster = new DataSet();
            //List<StringBuilder> sbSQL = new List<StringBuilder>();
            //try
            //{
            //    if (!type.IsNotNullOrEmpty()) type = "";
            //    if (!subType.IsNotNullOrEmpty()) subType = "";

            //    sbSQL.Add(new StringBuilder("Select SUBTYPE ,PRD_CODE from fasteners_master where product_category=  " + ToDbParameter("Type") + " and type= " + ToDbParameter("subType") + " AND subtype IS NOT NULL")); //ddoper_mast
            //    lstDbParameter.Clear();
            //    lstDbParameter.Add(DBParameter("type", type));
            //    lstDbParameter.Add(DBParameter("subType", subType));
            //    dsMaster = Dal.GetDataSet(sbSQL, lstDbParameter);
            //    return dsMaster;
            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
        }
        /// <summary>
        /// Linked With Based Bottom Grid --> Combo Datas Loaded
        /// </summary>
        /// <param name="linkedWith"></param>
        /// <returns></returns>
        public bool GetGridComboLinkedWithCross(CrossLinkingCharModel crossModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.SUBTYPE != null && o.TYPE == crossModel.LinkedWith && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.SUBTYPE, o.PRD_CODE }).ToList());
                if (dt != null)
                {
                    crossModel.DtClassificationCmblinkedWith = dt.DefaultView;

                }
                else
                {
                    crossModel.DtClassificationCmblinkedWith = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }


            //DataSet dsMaster = new DataSet();
            //List<StringBuilder> sbSQL = new List<StringBuilder>();
            //try
            //{
            //    sbSQL.Add(new StringBuilder(" select SUBTYPE ,PRD_CODE from fasteners_master where type =  " + ToDbParameter("linkedWith") + " and subtype is not null "));
            //    lstDbParameter.Clear();
            //    lstDbParameter.Add(DBParameter("linkedWith", linkedWith));
            //    dsMaster = Dal.GetDataSet(sbSQL, lstDbParameter);
            //    return dsMaster;

            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GenerateFeatuerCode()
        {
            string featureMast = "";
            try
            {
                featureMast = (from o in DB.FEATURE_MASTER where o.OPER_CODE == 1030 select o.FEATURE_CODE).Max();
                featureMast = "F" + (Convert.ToInt32(featureMast.Replace("F", "").Trim()) + 1).ToString("0000");
                return featureMast;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return featureMast;
                // throw ex.LogException();
            }
        }

        public string GenerateProductCodeForginType(string type = "", string subType = "")
        {
            string forgingMast = "";
            try
            {
                forgingMast = (from o in DB.FORGING_MASTER where o.PRD_CODE.ToString().StartsWith("PR") select o.PRD_CODE).Max();
                if (forgingMast == null)
                {
                    forgingMast = "0";
                }
                else
                {
                    forgingMast = forgingMast.Replace("PR", "").Trim();
                }
                forgingMast = "PR" + Convert.ToInt32(Convert.ToInt32(forgingMast) + 1).ToString("0000");
                return forgingMast;
            }
            catch (Exception ex)
            {
                ex.LogException();
                forgingMast = "0";
                forgingMast = "PR" + Convert.ToInt32(Convert.ToInt32(forgingMast) + 1).ToString("0000");
                return forgingMast;
            }
        }
        /// <summary>
        ///  Save Characteristics Master Grid
        /// </summary>
        /// <param name="dgvCharactersticFSave"></param>
        /// <param name="newOperationCode"></param>
        /// <param name="newFeaturCode"></param>
        /// <param name="newFeaturDesc"></param>
        /// <param name="mode"></param>
        public bool SaveCharacterMasters(DataView dgvCharactersticFSave, string type, string subType, string linkedWith)
        {
            string prdCode = "";
            string newFeaturCode = "";
            string newFeaturDesc = "";
            decimal newOperationCode = 1030;
            decimal rowCount = 0;
            FORGING_MASTER forgingMast = new FORGING_MASTER();
            FEATURE_MASTER featureMast = new FEATURE_MASTER();
            try
            {
                if (dgvCharactersticFSave.Count > 0)
                {
                    DataTable dataValue = new DataTable();
                    try
                    {
                        dataValue = ToDataTable((from c in DB.FORGING_MASTER
                                                 where c.TYPE == type && c.SUB_TYPE == subType && c.PRD_CODE.ToString().StartsWith("PR")
                                                 select c).ToList());

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }

                    for (int i = 0; i < dgvCharactersticFSave.Count; i++)
                    {
                        if (dgvCharactersticFSave[i][4].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][6].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][7].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][8].ToString().Trim().IsNotNullOrEmpty())
                        {
                            newFeaturCode = dgvCharactersticFSave[i]["FEATURE_CODE"].ToString();
                            newFeaturDesc = dgvCharactersticFSave[i]["FEATURE_DESC"].ToString();
                            prdCode = GenerateProductCodeForginType(type, subType);

                            try
                            {
                                forgingMast = (from c in DB.FORGING_MASTER
                                               where c.TYPE == type && c.SUB_TYPE == subType && c.PRD_CODE == prdCode
                                               select c).FirstOrDefault<FORGING_MASTER>();
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                forgingMast = null;
                            }

                            if (forgingMast == null)
                            {
                                try
                                {
                                    try
                                    {
                                        forgingMast = new FORGING_MASTER();
                                        forgingMast.FEATURE_CODE = newFeaturCode;
                                        forgingMast.PRD_CODE = prdCode;
                                        forgingMast.SNO = (dataValue.Rows.Count == 0) ? 1 : (dataValue.Rows.Count + 1);
                                        forgingMast.TYPE = type;
                                        forgingMast.SUB_TYPE = subType;
                                        forgingMast.LINKED_WITH = linkedWith;
                                        forgingMast.ROWID = Guid.NewGuid();
                                        forgingMast.DELETE_FLAG = false;
                                        forgingMast.ENTERED_DATE = DateTime.Now;
                                        forgingMast.ENTERED_BY = userInformation.UserName;
                                        DB.FORGING_MASTER.InsertOnSubmit(forgingMast);
                                        DB.SubmitChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.LogException();
                                        DB.FORGING_MASTER.DeleteOnSubmit(forgingMast);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                    DB.FORGING_MASTER.DeleteOnSubmit(forgingMast);
                                }
                            }
                            else
                            {
                                try
                                {
                                    forgingMast.PRD_CODE = prdCode;
                                    forgingMast.TYPE = type;
                                    forgingMast.SUB_TYPE = subType;
                                    forgingMast.LINKED_WITH = linkedWith;
                                    forgingMast.DELETE_FLAG = false;
                                    forgingMast.UPDATED_DATE = DateTime.Now;
                                    forgingMast.UPDATED_BY = userInformation.UserName;
                                    DB.SubmitChanges();
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                    DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, forgingMast);

                                }
                            }
                        }
                    }

                    for (int i = 0; i < dgvCharactersticFSave.Count; i++)
                    {
                        decimal? strVal = 0;
                        newFeaturCode = dgvCharactersticFSave[i]["FEATURE_CODE"].ToString();
                        newFeaturDesc = dgvCharactersticFSave[i]["FEATURE_DESC"].ToString();

                        if (dgvCharactersticFSave[i]["SNO"].ToString().IsNotNullOrEmpty())
                        {
                            strVal = Convert.ToDecimal(dgvCharactersticFSave[i]["SNO"].ToString());
                            rowCount = Convert.ToDecimal(strVal);
                        }
                        else
                        {
                            try
                            {
                                strVal = ((from c in DB.FEATURE_MASTER
                                           where c.OPER_CODE == newOperationCode && c.FEATURE_CODE == newFeaturCode
                                           select c.SNO).Max());
                            }
                            catch (Exception)
                            {
                                strVal = 0;
                            }
                            if (!strVal.IsNotNullOrEmpty() || strVal == 0)
                            {
                                rowCount = 0;
                            }
                            else
                            {
                                rowCount = Convert.ToDecimal(strVal);
                            }
                            rowCount = rowCount + 1;
                        }

                        try
                        {
                            featureMast = (from c in DB.FEATURE_MASTER
                                           where c.OPER_CODE == newOperationCode && c.FEATURE_CODE == newFeaturCode && c.SNO == rowCount && c.MEASURING_TECHNIQUE == dgvCharactersticFSave[i][4].ToString()
                                           select c).FirstOrDefault<FEATURE_MASTER>();
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            featureMast = null;
                        }
                        if (featureMast == null)
                        {
                            try
                            {

                                int sampSize = 0;
                                if (!dgvCharactersticFSave[i][5].ToString().IsNotNullOrEmpty())
                                {
                                    // sampSize = 0;

                                }
                                else
                                {
                                    sampSize = Convert.ToInt32(dgvCharactersticFSave[i][5].ToString());
                                }
                                if (dgvCharactersticFSave[i][4].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][6].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][7].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][8].ToString().Trim().IsNotNullOrEmpty())
                                {
                                    try
                                    {
                                        featureMast = new FEATURE_MASTER();
                                        featureMast.OPER_CODE = newOperationCode;
                                        featureMast.FEATURE_CODE = newFeaturCode;
                                        featureMast.FEATURE_DESC = newFeaturDesc;
                                        featureMast.SNO = rowCount;
                                        featureMast.MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString();
                                        featureMast.SAMPLE_SIZE = sampSize;
                                        featureMast.SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString();
                                        featureMast.CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString();
                                        featureMast.REACTION_PLAN = dgvCharactersticFSave[i][8].ToString();
                                        featureMast.ROWID = Guid.NewGuid();
                                        featureMast.DELETE_FLAG = false;
                                        featureMast.ENTERED_DATE = DateTime.Now;
                                        featureMast.ENTERED_BY = userInformation.UserName;
                                        DB.FEATURE_MASTER.InsertOnSubmit(featureMast);
                                        DB.SubmitChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.LogException();
                                        DB.FEATURE_MASTER.DeleteOnSubmit(featureMast);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.FEATURE_MASTER.DeleteOnSubmit(featureMast);
                            }

                        }
                        else
                        {
                            try
                            {
                                featureMast.MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString();
                                featureMast.SAMPLE_SIZE = Convert.ToInt16(dgvCharactersticFSave[i][5].ToString());
                                featureMast.SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString();
                                featureMast.CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString();
                                featureMast.REACTION_PLAN = dgvCharactersticFSave[i][8].ToString();
                                featureMast.DELETE_FLAG = false;
                                featureMast.UPDATED_DATE = DateTime.Now;
                                featureMast.UPDATED_BY = userInformation.UserName;
                                DB.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMast);
                            }


                        }

                    }
                    return true;
                }
                else
                {
                    return true;

                }
            }
            catch (Exception ex)
            {
                Dal.RollBackTransaction();
                DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, forgingMast);
                DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMast);
                ex.LogException();
            }
            return false;
        }
        public bool UpdateCharacteristicsmaster(DataView dgvCharactersticFSave)
        {
            FEATURE_MASTER featureMast = new FEATURE_MASTER();
            string newFeaturCode = "";
            string newFeaturDesc = "";
            decimal newOperationCode = 1030;
            decimal rowCount = 0;
            //if (dgvCharactersticFSave.Count > 0)
            //{
            //    for (int i = 0; i < dgvCharactersticFSave.Count - 1; i++)
            //    {
            //        featureMast = (from c in DB.FEATURE_MASTER
            //                       where c.OPER_CODE == 1030 && c.FEATURE_CODE == dgvCharactersticFSave[i]["FEATURE_CODE"].ToString() && c.SNO == Convert.ToDecimal(dgvCharactersticFSave[i]["SNO"].ToString())
            //                       select c).FirstOrDefault<FEATURE_MASTER>();
            //        if (featureMast != null)
            //        {
            //            try
            //            {
            //                featureMast.MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString();
            //                featureMast.SAMPLE_SIZE = Convert.ToInt16(dgvCharactersticFSave[i][5].ToString());
            //                featureMast.SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString();
            //                featureMast.CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString();
            //                featureMast.REACTION_PLAN = dgvCharactersticFSave[i][8].ToString();
            //                featureMast.DELETE_FLAG = false;
            //                featureMast.UPDATED_DATE = DateTime.Now;
            //                featureMast.UPDATED_BY = userInformation.UserName;
            //                DB.SubmitChanges();


            //            }
            //            catch (Exception)
            //            {
            //                DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMast);
            //                return false;
            //            }


            //        }
            //    }
            //    return true;
            //}
            for (int i = 0; i < dgvCharactersticFSave.Count; i++)
            {
                decimal? strVal = 0;
                newFeaturCode = dgvCharactersticFSave[i]["FEATURE_CODE"].ToString();
                newFeaturDesc = dgvCharactersticFSave[i]["FEATURE_DESC"].ToString();

                if (dgvCharactersticFSave[i]["SNO"].ToString().IsNotNullOrEmpty())
                {
                    strVal = Convert.ToDecimal(dgvCharactersticFSave[i]["SNO"].ToString());
                    rowCount = Convert.ToDecimal(strVal);
                }
                else
                {
                    try
                    {
                        strVal = ((from c in DB.FEATURE_MASTER
                                   where c.OPER_CODE == newOperationCode && c.FEATURE_CODE == newFeaturCode
                                   select c.SNO).Max());
                    }
                    catch (Exception)
                    {
                        strVal = 0;
                    }
                    if (!strVal.IsNotNullOrEmpty() || strVal == 0)
                    {
                        rowCount = 0;
                    }
                    else
                    {
                        rowCount = Convert.ToDecimal(strVal);
                    }
                    rowCount = rowCount + 1;
                }


                //featureMast = (from c in DB.FEATURE_MASTER
                //               where c.OPER_CODE == newOperationCode && c.FEATURE_CODE == newFeaturCode && c.SNO == rowCount && c.MEASURING_TECHNIQUE == dgvCharactersticFSave[i][4].ToString()
                //               select c).FirstOrDefault<FEATURE_MASTER>();
                featureMast = (from c in DB.FEATURE_MASTER
                               where c.OPER_CODE == newOperationCode && c.FEATURE_CODE == newFeaturCode && c.SNO == rowCount
                               select c).FirstOrDefault<FEATURE_MASTER>();

                if (featureMast == null)
                {
                    try
                    {
                        int sampSize = 0;
                        if (!dgvCharactersticFSave[i][5].ToString().IsNotNullOrEmpty())
                        {
                            // sampSize = 0;

                        }
                        else
                        {
                            sampSize = Convert.ToInt32(dgvCharactersticFSave[i][5].ToString());
                        }
                        if (dgvCharactersticFSave[i][4].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][6].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][7].ToString().Trim().IsNotNullOrEmpty() || dgvCharactersticFSave[i][8].ToString().Trim().IsNotNullOrEmpty())
                        {
                            try
                            {
                                featureMast = new FEATURE_MASTER();
                                featureMast.OPER_CODE = newOperationCode;
                                featureMast.FEATURE_CODE = newFeaturCode;
                                featureMast.FEATURE_DESC = newFeaturDesc;
                                featureMast.SNO = rowCount;
                                featureMast.MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString();
                                featureMast.SAMPLE_SIZE = sampSize;
                                featureMast.SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString();
                                featureMast.CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString();
                                featureMast.REACTION_PLAN = dgvCharactersticFSave[i][8].ToString();
                                featureMast.ROWID = Guid.NewGuid();
                                featureMast.DELETE_FLAG = false;
                                featureMast.ENTERED_DATE = DateTime.Now;
                                featureMast.ENTERED_BY = userInformation.UserName;
                                DB.FEATURE_MASTER.InsertOnSubmit(featureMast);
                                DB.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.FEATURE_MASTER.DeleteOnSubmit(featureMast);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.FEATURE_MASTER.DeleteOnSubmit(featureMast);
                    }

                }
                else
                {
                    try
                    {
                        featureMast.MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString();
                        featureMast.SAMPLE_SIZE = Convert.ToInt16(dgvCharactersticFSave[i][5].ToString());
                        featureMast.SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString();
                        featureMast.CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString();
                        featureMast.REACTION_PLAN = dgvCharactersticFSave[i][8].ToString();
                        featureMast.DELETE_FLAG = false;
                        featureMast.UPDATED_DATE = DateTime.Now;
                        featureMast.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMast);
                    }


                }

            }
            return false;
        }
        public bool DeleteFeatureCharacteristicsDetails(decimal operationCode, string featurCode, decimal sno = 0)
        {
            if (!operationCode.IsNotNullOrEmpty() || !featurCode.IsNotNullOrEmpty() || !sno.IsNotNullOrEmpty()) return false;
            FEATURE_MASTER featureMast = new FEATURE_MASTER();
            try
            {
                featureMast = (from c in DB.FEATURE_MASTER
                               where c.OPER_CODE == operationCode && c.FEATURE_CODE == featurCode && c.SNO == sno
                               select c).FirstOrDefault<FEATURE_MASTER>();

                if (featureMast != null)
                {

                    featureMast.DELETE_FLAG = true;
                    featureMast.UPDATED_DATE = DateTime.Now;
                    featureMast.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMast);
                ex.LogException();
                return false;
            }
            //return false;
        }
        public bool DeleteFeatureDetails(decimal operationCode, string featurCode)
        {
            if (!operationCode.IsNotNullOrEmpty() || !featurCode.IsNotNullOrEmpty()) return false;
            List<FEATURE_MASTER> lstexistingDatas = new List<FEATURE_MASTER>();
            try
            {
                lstexistingDatas = (from c in DB.FEATURE_MASTER
                                    where c.OPER_CODE == operationCode && c.FEATURE_CODE == featurCode
                                    select c).ToList();
                if (lstexistingDatas.Count > 0)
                {
                    DB.FEATURE_MASTER.DeleteAllOnSubmit(lstexistingDatas);
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstexistingDatas);
                ex.LogException();
                return false;
            }
            //return false;
        }
        public bool DeletePrdCategorybasedDetails(string featurCode, string prdCode, string type, string subType, decimal sno = 0)
        {
            if (!featurCode.IsNotNullOrEmpty() || !prdCode.IsNotNullOrEmpty() || !type.IsNotNullOrEmpty() || !sno.IsNotNullOrEmpty() || !subType.IsNotNullOrEmpty()) return false;
            FORGING_MASTER forgMast = new FORGING_MASTER();
            try
            {
                forgMast = (from c in DB.FORGING_MASTER
                            where c.FEATURE_CODE == featurCode && c.PRD_CODE == prdCode && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == sno
                            select c).FirstOrDefault<FORGING_MASTER>();

                if (forgMast != null)
                {

                    forgMast.DELETE_FLAG = true;
                    forgMast.UPDATED_DATE = DateTime.Now;
                    forgMast.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, forgMast);
                ex.LogException();
                return false;
            }
            //return false;
        }
        public bool DeleteLnkedWithbasedDetails(string featurCode, string prdCode, string type, string subType, string linkedWith, decimal sno = 0)
        {
            if (!featurCode.IsNotNullOrEmpty() || !prdCode.IsNotNullOrEmpty() || !type.IsNotNullOrEmpty() || !sno.IsNotNullOrEmpty() || !subType.IsNotNullOrEmpty() || !linkedWith.IsNotNullOrEmpty()) return false;
            FORGING_MASTER forgMast = new FORGING_MASTER();
            try
            {
                forgMast = (from c in DB.FORGING_MASTER
                            where c.FEATURE_CODE == featurCode && c.PRD_CODE == prdCode && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == sno && c.LINKED_WITH == linkedWith
                            select c).FirstOrDefault<FORGING_MASTER>();

                if (forgMast != null)
                {

                    forgMast.DELETE_FLAG = true;
                    forgMast.UPDATED_DATE = DateTime.Now;
                    forgMast.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, forgMast);
                ex.LogException();
                return false;
            }
            //return false;
        }

        /// <summary>
        ///  Save ForginMaster Master Grid
        /// </summary>
        /// <param name="dgvCharactersticFSave"></param>
        /// <param name="newOperationCode"></param>
        /// <param name="newFeaturCode"></param>
        /// <param name="newFeaturDesc"></param>
        /// <param name="mode"></param>
        public bool SaveForginMaster(DataView dgvForginMasterFSave, string type, string subType, string linkedWith)
        {
            FASTENERS_MASTER fasternersMast = new FASTENERS_MASTER();
            FORGING_MASTER forgineMast = new FORGING_MASTER();
            string newFeaturCode = "";
            decimal rowCount = 0;
            try
            {
                //string s=(from o in DB.FASTENERS_MASTER where o.SUBTYPE == prdName select o.PRD_CODE).Single<FASTENERS_MASTER>().ToString();
                if (dgvForginMasterFSave.Count > 0)
                {
                    //    //New Implementation
                    //    DataView dtTemp = dgvForginMasterFSave.Table.Copy().DefaultView.ToTable(true, "FEATURE_CODE").DefaultView;
                    //    if (dtTemp.Count > 0)
                    //        for (int i = 0; i < dtTemp.ToTable(true, "FEATURE_CODE").Rows.Count - 1; i++)
                    //        {
                    //            List<FORGING_MASTER> lstexistingDatas = new List<FORGING_MASTER>();

                    //            try
                    //            {
                    //                if (dgvForginMasterFSave[0][0].ToString().IsNotNullOrEmpty())
                    //                {
                    //                    lstexistingDatas = (from c in DB.FORGING_MASTER
                    //                                        where c.TYPE == type && c.FEATURE_CODE == dtTemp[i]["FEATURE_CODE"].ToString()
                    //                                        select c).ToList();
                    //                    DB.FORGING_MASTER.DeleteAllOnSubmit(lstexistingDatas);
                    //                    DB.SubmitChanges();
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstexistingDatas);
                    //                throw ex.LogException();
                    //            }
                    //        }


                    // End 

                    for (int i = 0; i < dgvForginMasterFSave.Count; i++)
                    {
                        newFeaturCode = dgvForginMasterFSave[i]["FEATURE_CODE"].ToString();

                        try
                        {
                            rowCount = (from c in DB.FORGING_MASTER
                                        where c.TYPE == type && c.SUB_TYPE == subType && c.FEATURE_CODE == dgvForginMasterFSave[i]["FEATURE_CODE"].ToString()
                                        select c).Max(x => x.SNO);
                            rowCount = i + rowCount;
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            rowCount = 1;
                        }

                        if (dgvForginMasterFSave[i][0].ToString().IsNotNullOrEmpty())
                        {
                            fasternersMast = (from c in DB.FASTENERS_MASTER
                                              where c.SUBTYPE == dgvForginMasterFSave[i][0].ToString()
                                                && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                              select c).FirstOrDefault<FASTENERS_MASTER>();
                            if (fasternersMast != null)
                            {
                                try
                                {
                                    forgineMast = (from c in DB.FORGING_MASTER
                                                   where c.FEATURE_CODE == newFeaturCode && c.PRD_CODE == fasternersMast.PRD_CODE && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == rowCount
                                                          && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                   select c).FirstOrDefault<FORGING_MASTER>();
                                    if (!forgineMast.IsNotNullOrEmpty())
                                    {
                                        if (IsFeatureCodeAvailable(newFeaturCode))
                                        {
                                            forgineMast = new FORGING_MASTER()
                                            {
                                                FEATURE_CODE = newFeaturCode,
                                                PRD_CODE = fasternersMast.PRD_CODE,
                                                SNO = rowCount,
                                                TYPE = type,
                                                SUB_TYPE = subType,
                                                //LINKED_WITH = linkedWith,
                                                ROWID = Guid.NewGuid()
                                            };

                                            DB.FORGING_MASTER.InsertOnSubmit(forgineMast);
                                            DB.SubmitChanges();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                    DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                                }

                            }

                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Dal.RollBackTransaction();
                DB.FASTENERS_MASTER.DeleteOnSubmit(fasternersMast);
                DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                ex.LogException();
            }
            return false;
        }
        public bool IsFeatureCodeAvailable(string newFeaturCode)
        {
            FEATURE_MASTER feature = new FEATURE_MASTER();
            try
            {
                decimal newOperationCode = 1030;
                feature = (from c in DB.FEATURE_MASTER
                           where c.OPER_CODE == newOperationCode && c.FEATURE_CODE == newFeaturCode
                           select c).FirstOrDefault<FEATURE_MASTER>();
                if (feature.IsNotNullOrEmpty())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }


        }
        public bool UpdateForginMaster(DataView dgvForginMasterFSave, string type, string subType, string linkedWith)
        {
            FASTENERS_MASTER fasternersMast = new FASTENERS_MASTER();
            FORGING_MASTER forgineMast = new FORGING_MASTER();
            string newFeaturCode = "";
            decimal rowCount = 0;
            try
            {
                //string s=(from o in DB.FASTENERS_MASTER where o.SUBTYPE == prdName select o.PRD_CODE).Single<FASTENERS_MASTER>().ToString();
                if (dgvForginMasterFSave.Count > 0)
                {
                    //    //New Implementation
                    //    DataView dtTemp = dgvForginMasterFSave.Table.Copy().DefaultView.ToTable(true, "FEATURE_CODE").DefaultView;
                    //    if (dtTemp.Count > 0)
                    //        for (int i = 0; i < dtTemp.ToTable(true, "FEATURE_CODE").Rows.Count - 1; i++)
                    //        {
                    //            List<FORGING_MASTER> lstexistingDatas = new List<FORGING_MASTER>();

                    //            try
                    //            {
                    //                if (dgvForginMasterFSave[0][0].ToString().IsNotNullOrEmpty())
                    //                {
                    //                    lstexistingDatas = (from c in DB.FORGING_MASTER
                    //                                        where c.TYPE == type && c.FEATURE_CODE == dtTemp[i]["FEATURE_CODE"].ToString()
                    //                                        select c).ToList();
                    //                    DB.FORGING_MASTER.DeleteAllOnSubmit(lstexistingDatas);
                    //                    DB.SubmitChanges();
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstexistingDatas);
                    //                throw ex.LogException();
                    //            }
                    //        }


                    // End 

                    for (int i = 0; i < dgvForginMasterFSave.Count; i++)
                    {
                        newFeaturCode = dgvForginMasterFSave[i]["FEATURE_CODE"].ToString();
                        if (dgvForginMasterFSave[i][0].ToString().IsNotNullOrEmpty())
                        {
                            fasternersMast = (from c in DB.FASTENERS_MASTER
                                              where c.SUBTYPE == dgvForginMasterFSave[i][0].ToString()
                                                && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                              select c).FirstOrDefault<FASTENERS_MASTER>();
                            if (fasternersMast != null)
                            {
                                try
                                {
                                    try
                                    {
                                        forgineMast = (from c in DB.FORGING_MASTER
                                                       where c.FEATURE_CODE == newFeaturCode && c.PRD_CODE == dgvForginMasterFSave[i]["prd_code"].ToString() && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == Convert.ToDecimal(dgvForginMasterFSave[i]["SNO"].ToString())
                                                              && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                       select c).FirstOrDefault<FORGING_MASTER>();
                                        if (forgineMast.IsNotNullOrEmpty())
                                        {
                                            DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                                            DB.SubmitChanges();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.LogException();

                                    }
                                    try
                                    {
                                        rowCount = (from c in DB.FORGING_MASTER
                                                    where c.TYPE == type && c.SUB_TYPE == subType && c.FEATURE_CODE == dgvForginMasterFSave[i]["FEATURE_CODE"].ToString()
                                                    select c).Max(x => x.SNO);
                                        rowCount = i + rowCount;
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.LogException();
                                        rowCount = 1;
                                    }

                                    forgineMast = (from c in DB.FORGING_MASTER
                                                   where c.FEATURE_CODE == newFeaturCode && c.PRD_CODE == fasternersMast.PRD_CODE && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == rowCount
                                                          && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                   select c).FirstOrDefault<FORGING_MASTER>();
                                    if (!forgineMast.IsNotNullOrEmpty())
                                    {
                                        if (IsFeatureCodeAvailable(newFeaturCode))
                                        {
                                            forgineMast = new FORGING_MASTER()
                                            {
                                                FEATURE_CODE = newFeaturCode,
                                                PRD_CODE = fasternersMast.PRD_CODE,
                                                SNO = rowCount,
                                                TYPE = type,
                                                SUB_TYPE = subType,
                                                //LINKED_WITH = linkedWith,
                                                ROWID = Guid.NewGuid()
                                            };

                                            DB.FORGING_MASTER.InsertOnSubmit(forgineMast);
                                            DB.SubmitChanges();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                    DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                                }

                            }

                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Dal.RollBackTransaction();
                DB.FASTENERS_MASTER.DeleteOnSubmit(fasternersMast);
                DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                ex.LogException();
            }
            return false;
        }
        //public bool UpdateForginMaster(DataView dgvForginMasterFSave, string type, string subType, string linkedWith)
        //{
        //    FASTENERS_MASTER fasternersMast = new FASTENERS_MASTER();
        //    FORGING_MASTER forgineMast = new FORGING_MASTER();
        //    string newFeaturCode = "";
        //    decimal rowCount = 0;
        //    try
        //    {
        //        //string s=(from o in DB.FASTENERS_MASTER where o.SUBTYPE == prdName select o.PRD_CODE).Single<FASTENERS_MASTER>().ToString();
        //        if (dgvForginMasterFSave.Count > 0)
        //        {
        //            //    //New Implementation
        //            //    DataView dtTemp = dgvForginMasterFSave.Table.Copy().DefaultView.ToTable(true, "FEATURE_CODE").DefaultView;
        //            //    if (dtTemp.Count > 0)
        //            //        for (int i = 0; i < dtTemp.ToTable(true, "FEATURE_CODE").Rows.Count - 1; i++)
        //            //        {
        //            //            List<FORGING_MASTER> lstexistingDatas = new List<FORGING_MASTER>();

        //            //            try
        //            //            {
        //            //                if (dgvForginMasterFSave[0][0].ToString().IsNotNullOrEmpty())
        //            //                {
        //            //                    lstexistingDatas = (from c in DB.FORGING_MASTER
        //            //                                        where c.TYPE == type && c.FEATURE_CODE == dtTemp[i]["FEATURE_CODE"].ToString()
        //            //                                        select c).ToList();
        //            //                    DB.FORGING_MASTER.DeleteAllOnSubmit(lstexistingDatas);
        //            //                    DB.SubmitChanges();
        //            //                }
        //            //            }
        //            //            catch (Exception ex)
        //            //            {
        //            //                DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstexistingDatas);
        //            //                throw ex.LogException();
        //            //            }
        //            //        }


        //            // End 

        //            for (int i = 0; i < dgvForginMasterFSave.Count; i++)
        //            {
        //                newFeaturCode = dgvForginMasterFSave[i]["FEATURE_CODE"].ToString();

        //                try
        //                {
        //                    rowCount = (from c in DB.FORGING_MASTER
        //                                where c.TYPE == type && c.SUB_TYPE == subType && c.FEATURE_CODE == dgvForginMasterFSave[i]["FEATURE_CODE"].ToString()
        //                                select c).Max(x => x.SNO);
        //                    rowCount = i + rowCount;
        //                }
        //                catch (Exception)
        //                {
        //                    rowCount = 1;
        //                }

        //                if (dgvForginMasterFSave[i][0].ToString().IsNotNullOrEmpty())
        //                {
        //                    fasternersMast = (from c in DB.FASTENERS_MASTER
        //                                      where c.SUBTYPE == dgvForginMasterFSave[i][0].ToString()
        //                                        && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
        //                                      select c).FirstOrDefault<FASTENERS_MASTER>();
        //                    if (fasternersMast != null)
        //                    {
        //                        try
        //                        {
        //                            forgineMast = (from c in DB.FORGING_MASTER
        //                                           where c.FEATURE_CODE == newFeaturCode && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == rowCount
        //                                                  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
        //                                           select c).FirstOrDefault<FORGING_MASTER>();
        //                            if (forgineMast.IsNotNullOrEmpty())
        //                            {
        //                                forgineMast.PRD_CODE = fasternersMast.PRD_CODE;
        //                                forgineMast.TYPE = type;
        //                                forgineMast.SUB_TYPE = subType;
        //                                // DB.FORGING_MASTER.InsertOnSubmit(forgineMast);
        //                                DB.SubmitChanges();
        //                            }
        //                        }
        //                        catch (Exception)
        //                        {
        //                        }

        //                    }

        //                }
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Dal.RollBackTransaction();
        //        DB.FASTENERS_MASTER.DeleteOnSubmit(fasternersMast);
        //        DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
        //        ex.LogException();
        //    }
        //    return false;
        //}
        public bool SaveFastnersMaster(DataView dgvForginMasterFSave, string type, string subType, string linkedWith)
        {
            FASTENERS_MASTER fasternersMast = new FASTENERS_MASTER();
            FORGING_MASTER forgineMast = new FORGING_MASTER();
            decimal rowCount;
            string newFeaturCode = "";
            try
            {

                ////string s=(from o in DB.FASTENERS_MASTER where o.SUBTYPE == prdName select o.PRD_CODE).Single<FASTENERS_MASTER>().ToString();
                if (dgvForginMasterFSave.Count > 0)
                {
                    //    //New Implementation
                    //    List<FORGING_MASTER> lstexistingDatas = new List<FORGING_MASTER>();

                    //    try
                    //    {
                    //        if (dgvForginMasterFSave[0][0].ToString().IsNotNullOrEmpty())
                    //        {

                    //            lstexistingDatas = (from c in DB.FORGING_MASTER.AsEnumerable()
                    //                                where c.TYPE == type && c.SUB_TYPE == subType && c.FEATURE_CODE == newFeaturCode && (!string.IsNullOrEmpty(c.LINKED_WITH))
                    //                                select c).ToList();
                    //            DB.FORGING_MASTER.DeleteAllOnSubmit(lstexistingDatas);
                    //            DB.SubmitChanges();
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        DB.FORGING_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstexistingDatas);
                    //        throw ex.LogException();
                    //    }

                    //    // End 
                    for (int i = 0; i < dgvForginMasterFSave.Count; i++)
                    {
                        newFeaturCode = dgvForginMasterFSave[i]["FEATURE_CODE"].ToString();
                        try
                        {
                            rowCount = (from c in DB.FORGING_MASTER
                                        where c.TYPE == type && c.SUB_TYPE == subType && c.FEATURE_CODE == dgvForginMasterFSave[i]["FEATURE_CODE"].ToString()
                                        select c).Max(x => x.SNO);
                            rowCount = i + rowCount;
                        }
                        catch (Exception)
                        {
                            rowCount = 1;
                        }
                        string prdCode = "";
                        if (dgvForginMasterFSave[i][0].ToString().IsNotNullOrEmpty())
                        {
                            fasternersMast = (from c in DB.FASTENERS_MASTER
                                              where c.SUBTYPE == dgvForginMasterFSave[i][0].ToString()
                                                && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                              select c).FirstOrDefault<FASTENERS_MASTER>();
                            if (fasternersMast != null)
                            {
                                prdCode = fasternersMast.PRD_CODE;
                                forgineMast = (from c in DB.FORGING_MASTER
                                               where c.FEATURE_CODE == newFeaturCode && c.PRD_CODE == fasternersMast.PRD_CODE && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == rowCount
                                                      && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                               select c).FirstOrDefault<FORGING_MASTER>();
                                try
                                {
                                    if (!forgineMast.IsNotNullOrEmpty())
                                    {
                                        if (IsFeatureCodeAvailable(newFeaturCode))
                                        {
                                            forgineMast = new FORGING_MASTER()
                                            {
                                                FEATURE_CODE = newFeaturCode,
                                                PRD_CODE = prdCode,
                                                SNO = rowCount,
                                                TYPE = type,
                                                SUB_TYPE = subType,
                                                LINKED_WITH = linkedWith,
                                                ROWID = Guid.NewGuid()
                                            };

                                            DB.FORGING_MASTER.InsertOnSubmit(forgineMast);
                                            DB.SubmitChanges();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                    DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Dal.RollBackTransaction();
                DB.FASTENERS_MASTER.DeleteOnSubmit(fasternersMast);
                DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);

                throw ex.LogException();
            }
            return false;
        }
        public bool UpdateFastnersMaster(DataView dgvForginMasterFSave, string type, string subType, string linkedWith)
        {
            FASTENERS_MASTER fasternersMast = null;
            FORGING_MASTER forgineMast = new FORGING_MASTER();
            decimal rowCount;
            string newFeaturCode = "";
            try
            {
                if (dgvForginMasterFSave.Count > 0)
                {

                    for (int i = 0; i < dgvForginMasterFSave.Count; i++)
                    {
                        newFeaturCode = dgvForginMasterFSave[i]["FEATURE_CODE"].ToString();
                        type = dgvForginMasterFSave[i]["type"].ToString();
                        subType = dgvForginMasterFSave[i]["sub_type"].ToString();
                        linkedWith = dgvForginMasterFSave[i]["linked_with"].ToString();

                        if (dgvForginMasterFSave[i][0].ToString().IsNotNullOrEmpty())
                        {
                            try
                            {
                                List<FASTENERS_MASTER> lstFASTENERS_MASTER = (from c in DB.FASTENERS_MASTER
                                                                              where c.SUBTYPE == dgvForginMasterFSave[i][0].ToString() && c.TYPE == dgvForginMasterFSave[i][4].ToString()
                                                                              select c).ToList<FASTENERS_MASTER>();
                                if (lstFASTENERS_MASTER.IsNotNullOrEmpty() && lstFASTENERS_MASTER.Count > 0)
                                {
                                    fasternersMast = lstFASTENERS_MASTER[0];
                                }
                                //fasternersMast = (from c in DB.FASTENERS_MASTER
                                //                  where c.SUBTYPE == dgvForginMasterFSave[i][0].ToString() && c.TYPE == dgvForginMasterFSave[i][4].ToString()
                                //                  select c).FirstOrDefault<FASTENERS_MASTER>();
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                            }
                            string prdCode = "";
                            if (fasternersMast != null)
                            {
                                prdCode = fasternersMast.PRD_CODE;
                                try
                                {
                                    forgineMast = (from c in DB.FORGING_MASTER
                                                   where c.FEATURE_CODE == newFeaturCode && c.PRD_CODE == dgvForginMasterFSave[i]["prd_code"].ToString() && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == Convert.ToDecimal(dgvForginMasterFSave[i]["SNO"].ToString())
                                                          && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                   select c).FirstOrDefault<FORGING_MASTER>();
                                    if (forgineMast.IsNotNullOrEmpty())
                                    {
                                        DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                                        DB.SubmitChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                }
                                try
                                {
                                    rowCount = (from c in DB.FORGING_MASTER
                                                where c.TYPE == type && c.SUB_TYPE == subType && c.FEATURE_CODE == dgvForginMasterFSave[i]["FEATURE_CODE"].ToString()
                                                select c).Max(x => x.SNO);
                                    rowCount = i + rowCount;
                                }
                                catch (Exception)
                                {
                                    rowCount = 1;
                                }
                                //if (!dgvForginMasterFSave[i]["SNO"].ToString().IsNotNullOrEmpty()) dgvForginMasterFSave[i]["SNO"] = rowCount;

                                try
                                {
                                    forgineMast = (from c in DB.FORGING_MASTER
                                                   where c.FEATURE_CODE == newFeaturCode && c.PRD_CODE == fasternersMast.PRD_CODE && c.TYPE == type && c.SUB_TYPE == subType && c.SNO == rowCount
                                                          && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                   select c).FirstOrDefault<FORGING_MASTER>();

                                    if (!forgineMast.IsNotNullOrEmpty())
                                    {
                                        forgineMast = new FORGING_MASTER();
                                        forgineMast.PRD_CODE = prdCode;
                                        forgineMast.TYPE = type;
                                        forgineMast.SNO = rowCount;
                                        forgineMast.FEATURE_CODE = newFeaturCode;
                                        forgineMast.SUB_TYPE = subType;
                                        forgineMast.LINKED_WITH = linkedWith;
                                        forgineMast.ROWID = Guid.NewGuid();
                                        DB.FORGING_MASTER.InsertOnSubmit(forgineMast);
                                        DB.SubmitChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                    DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Dal.RollBackTransaction();
                DB.FASTENERS_MASTER.DeleteOnSubmit(fasternersMast);
                DB.FORGING_MASTER.DeleteOnSubmit(forgineMast);

                throw ex.LogException();
            }
            return false;
        }
    }
}
