using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data;
using System.Reflection;


namespace ProcessDesigner.BLL
{
    public class FeatureMasterBll : Essential
    {
        public FeatureMasterBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public string GenerateFeatuerCode()
        {
            string featureMast = "";
            try
            {
                featureMast = (from o in DB.FEATURE_MASTER where o.FEATURE_CODE.ToString().StartsWith("F") select o.FEATURE_CODE).Max();
                featureMast = "F" + (Convert.ToInt32(featureMast.Replace("F", "").Trim()) + 1).ToString("0000");
                return featureMast;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataView GetOpertionMaster()
        {
            DataTable dataValue;
            // DB.DDOPER_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.DDOPER_MAST);
            dataValue = ToDataTable((from c in DB.DDOPER_MAST.AsEnumerable()
                                     where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)) && !c.OPER_DESC.StartsWith("FORG")
                                     orderby c.OPER_CODE ascending
                                     select new { c.OPER_CODE, c.OPER_DESC }).ToList());
            return dataValue.DefaultView;
        }

        public bool IsFeatureDescDuplicate(string desc, decimal operCode)
        {
            try
            {
                DataTable dt = new DataTable();
                if (desc.IsNotNullOrEmpty())
                {
                    //elect feature_desc from feature_master where feature_desc 
                    dt = ToDataTable((from o in DB.FEATURE_MASTER
                                      where o.FEATURE_DESC == desc && o.OPER_CODE == operCode && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select new { o.FEATURE_DESC }).ToList());

                }
                if (dt != null)
                {
                    if (dt.Rows.Count > 0) return false;
                }
                else
                {
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataView GetFeatureMaster(string operationCode)
        {
            DataTable dataValue;
            //   DB.DDOPER_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.DDOPER_MAST);
            if (operationCode == "") operationCode = "0";
            //dataValue = ToDataTable((from c in DB.FEATURE_MASTER.AsEnumerable()
            //                         where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)&& (c.OPER_CODE==operationCode)) 
            //                         orderby c.OPER_CODE ascending
            //                         select new {  c.FEATURE_CODE, c.FEATURE_DESC}).ToList().GroupBy(c=>c.FEATURE_CODE).Select(c=>c.First()).ToList());
            dataValue = ToDataTable((from c in DB.FEATURE_MASTER.AsEnumerable()
                                     where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)) && (c.OPER_CODE == Convert.ToDecimal(operationCode))
                                     orderby c.OPER_CODE ascending
                                     select new { c.FEATURE_CODE, c.FEATURE_DESC }).Distinct().ToList());
            return dataValue.DefaultView;
        }

        public DataView GetCharactersiticsMasterGrid(string operationCode, string featureCode)
        {
            DataTable dataValue;
            if (operationCode == "") operationCode = "0";
            //  DB.DDOPER_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.DDOPER_MAST);
            dataValue = ToDataTable((from c in DB.FEATURE_MASTER.AsEnumerable()
                                     where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)) && c.OPER_CODE == Convert.ToDecimal(operationCode) && c.FEATURE_CODE == featureCode
                                     orderby c.OPER_CODE ascending
                                     select new { c.OPER_CODE, c.FEATURE_CODE, c.FEATURE_DESC, c.SNO, c.MEASURING_TECHNIQUE, c.SAMPLE_SIZE, c.SAMPLE_FREQUENCY, c.CONTROL_METHOD, c.REACTION_PLAN }).ToList());
            return dataValue.DefaultView;
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
        public bool SaveFeatureCodeMaster(DataView dgvCharacterstic, string operCode, string operDesc, string featureCode, string featureDesc, ref string type)
        {
            FEATURE_MASTER featureMastUpdate = new FEATURE_MASTER();

            try
            {
                DataView dgvCharactersticFSave;
                decimal sampleSize = 0;
                dgvCharactersticFSave = dgvCharacterstic.ToTable().Copy().DefaultView;

                List<FEATURE_MASTER> lstexistingDatas = new List<FEATURE_MASTER>();
                lstexistingDatas = ((from c in DB.FEATURE_MASTER
                                     where c.OPER_CODE == Convert.ToDecimal(operCode.Trim()) && c.FEATURE_CODE == featureCode
                                     select c).ToList());
                if (lstexistingDatas.Count > 0)
                {
                    DB.FEATURE_MASTER.DeleteAllOnSubmit(lstexistingDatas);
                    DB.SubmitChanges();
                }
                int rown = 1;
                dgvCharactersticFSave.RowFilter = "MEASURING_TECHNIQUE is not null or SAMPLE_SIZE is not null or SAMPLE_FREQUENCY is not null or CONTROL_METHOD is not null  or REACTION_PLAN is not null";
                if (dgvCharactersticFSave.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < dgvCharactersticFSave.Count; i++)
                        {
                            if (dgvCharactersticFSave[i][5].ToString().Trim().Length == 0)
                            {
                                sampleSize = 0;
                            }
                            else
                            {
                                sampleSize = Convert.ToDecimal(dgvCharactersticFSave[i][5].ToString());
                            }
                            featureMastUpdate = new FEATURE_MASTER()
                            {
                                OPER_CODE = Convert.ToDecimal(operCode.Trim()),
                                FEATURE_CODE = featureCode,
                                FEATURE_DESC = featureDesc,
                                SNO = rown,
                                MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString(),
                                SAMPLE_SIZE = sampleSize,
                                SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString(),
                                CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString(),
                                REACTION_PLAN = dgvCharactersticFSave[i][8].ToString(),
                                ROWID = Guid.NewGuid()
                            };
                            rown = rown + 1;
                            DB.FEATURE_MASTER.InsertOnSubmit(featureMastUpdate);
                            DB.SubmitChanges();
                            type = "INS";
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.FEATURE_MASTER.DeleteOnSubmit(featureMastUpdate);

                    }
                }

                //if (ExtendedMethods.IsNotNullOrEmpty(dgvCharactersticFSave) && dgvCharactersticFSave.Count > 0) dgvCharactersticFSave.RowFilter = "MEASURING_TECHNIQUE is not null ";
                //Existing Codes Update
                //    dgvCharactersticFUpdate = dgvCharacterstic.ToTable().Copy().DefaultView;
                //    if (ExtendedMethods.IsNotNullOrEmpty(dgvCharactersticFUpdate) && dgvCharactersticFUpdate.Count > 0) dgvCharactersticFUpdate.RowFilter = "feature_code is not null";
                //    if (dgvCharactersticFUpdate.Count > 0)
                //    {
                //        for (int i = 0; i < dgvCharactersticFUpdate.Count; i++)
                //        {

                //            if (dgvCharactersticFUpdate[i][5].ToString().Trim().Length == 0)
                //            {
                //                sampleSize = 0;
                //            }
                //            else
                //            {
                //                sampleSize = Convert.ToDecimal(dgvCharactersticFUpdate[i][5].ToString());
                //            }
                //            featureMastUpdate = (from c in DB.FEATURE_MASTER
                //                                 where c.OPER_CODE == Convert.ToDecimal(operCode.Trim()) && c.SNO == Convert.ToDecimal(dgvCharactersticFUpdate[i]["SNO"].ToString()) && c.FEATURE_CODE == dgvCharactersticFUpdate[i]["FEATURE_CODE"].ToString()
                //                                 select c).FirstOrDefault<FEATURE_MASTER>();
                //            if (featureMastUpdate.IsNotNullOrEmpty())
                //            {
                //                try
                //                {
                //                    featureMastUpdate.FEATURE_DESC = featureDesc;
                //                    featureMastUpdate.FEATURE_CODE = featureCode;
                //                    featureMastUpdate.OPER_CODE = Convert.ToDecimal(operCode.Trim());
                //                    featureMastUpdate.MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString();
                //                    featureMastUpdate.SAMPLE_SIZE = sampleSize;
                //                    featureMastUpdate.SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString();
                //                    featureMastUpdate.CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString();
                //                    featureMastUpdate.REACTION_PLAN = dgvCharactersticFSave[i][8].ToString();
                //                    DB.SubmitChanges();
                //                    type = "UPD";
                //                }
                //                catch (Exception ex)
                //                {
                //                    DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMastUpdate);

                //                }

                //            }

                //        }
                //    }

                //    sampleSize = 0;

                //    if (ExtendedMethods.IsNotNullOrEmpty(dgvCharactersticFSave) && dgvCharactersticFSave.Count > 0) dgvCharactersticFSave.RowFilter = "MEASURING_TECHNIQUE is not null or SAMPLE_SIZE is not null or SAMPLE_FREQUENCY is not null or CONTROL_METHOD is not null  or REACTION_PLAN is not null";
                //    rowCount = dgvCharactersticFSave.Count;
                //    if (ExtendedMethods.IsNotNullOrEmpty(dgvCharactersticFSave) && dgvCharactersticFSave.Count > 0) dgvCharactersticFSave.RowFilter = "(MEASURING_TECHNIQUE is not null or SAMPLE_SIZE is not null or SAMPLE_FREQUENCY is not null or CONTROL_METHOD is not null  or REACTION_PLAN is not null) and feature_code is null";
                //    if (operCode == "") operCode = "0";
                //    rowCount = rowCount - dgvCharactersticFSave.Count;
                //    if (dgvCharactersticFSave.Count == 0)
                //    {
                //        featureMastUpdate = (from c in DB.FEATURE_MASTER
                //                             where c.FEATURE_CODE == featureCode
                //                             select c).FirstOrDefault<FEATURE_MASTER>();
                //        if (featureMastUpdate.IsNotNullOrEmpty())
                //        {
                //            try
                //            {
                //                // featureMastUpdate.FEATURE_CODE = featureCode;
                //                featureMastUpdate.FEATURE_DESC = featureDesc;
                //                DB.SubmitChanges();
                //            }
                //            catch (Exception)
                //            {
                //                DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMastUpdate);

                //            }

                //        }
                //        return true;
                //    }

                //    if (dgvCharactersticFSave.Count > 0)
                //    {
                //        try
                //        {

                //            for (int i = 0; i < dgvCharactersticFSave.Count; i++)
                //            {
                //                if (dgvCharactersticFSave[i][5].ToString().Trim().Length == 0)
                //                {
                //                    sampleSize = 0;
                //                }
                //                else
                //                {
                //                    sampleSize = Convert.ToDecimal(dgvCharactersticFSave[i][5].ToString());
                //                }
                //                featureMastUpdate = new FEATURE_MASTER()
                //                {
                //                    OPER_CODE = Convert.ToDecimal(operCode.Trim()),
                //                    FEATURE_CODE = featureCode,
                //                    FEATURE_DESC = featureDesc,
                //                    SNO = rowCount + 1,
                //                    MEASURING_TECHNIQUE = dgvCharactersticFSave[i][4].ToString(),
                //                    SAMPLE_SIZE = sampleSize,
                //                    SAMPLE_FREQUENCY = dgvCharactersticFSave[i][6].ToString(),
                //                    CONTROL_METHOD = dgvCharactersticFSave[i][7].ToString(),
                //                    REACTION_PLAN = dgvCharactersticFSave[i][8].ToString(),
                //                    ROWID = Guid.NewGuid()
                //                };
                //                rowCount = featureMastUpdate.SNO;
                //                DB.FEATURE_MASTER.InsertOnSubmit(featureMastUpdate);
                //                DB.SubmitChanges();
                //                type = "INS";
                //            }
                //            return true;
                //        }
                //        catch (Exception)
                //        {
                //            DB.FEATURE_MASTER.DeleteOnSubmit(featureMastUpdate);

                //        }

                //    }
            }
            catch (Exception ex)
            {
                // DB.FEATURE_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, featureMastUpdate);
                Dal.RollBackTransaction();
                throw ex.LogException();
            }
            return false;
        }
    }
}
