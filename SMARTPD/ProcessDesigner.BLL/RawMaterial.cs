using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data.Linq;

namespace ProcessDesigner.BLL
{
    public class RawMaterial : Essential, IDataManipulation
    {
        public RawMaterial(UserInformation userInformation)
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
        /// Get the Locations
        /// </summary>
        /// <param name="locationCode">Code of the Location </param>
        /// <returns>Returns All location if location code is not specified. otherwise return only specified location</returns>
        public List<DDLOC_MAST> GetLocations(string locationCode = "")
        {
            List<DDLOC_MAST> lstLocations = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstLocations;
                if (locationCode.IsNotNullOrEmpty())
                {
                    lstLocations = (from row in DB.DDLOC_MAST
                                    where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false) && row.LOC_CODE == locationCode
                                    select row).ToList<DDLOC_MAST>();

                }
                else
                {

                    lstLocations = (from row in DB.DDLOC_MAST
                                    where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false)
                                    select row).ToList<DDLOC_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstLocations;
        }

        /// <summary>
        /// Userd to get Raw Materials Size
        /// </summary>
        /// <param name="rawMaterialCode">Code of the Raw Material</param>
        /// <returns>List of Raw Material</returns>
        public List<DDRM_SIZE_MAST> GetRawMaterialsSize(DDRM_MAST paramEntity)
        {
            List<DDRM_SIZE_MAST> lstRawMaterial = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstRawMaterial;
                lstRawMaterial = (from row in DB.DDRM_SIZE_MAST
                                  where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false) && row.IDFK == paramEntity.IDPK
                                  orderby row.IDPK ascending
                                  select row).ToList<DDRM_SIZE_MAST>();

                //if (rawMaterialCode.IsNotNullOrEmpty())
                //{
                //    lstRawMaterial = (from row in DB.DDRM_SIZE_MAST
                //                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false ||row.DELETE_FLAG == false) && row.RM_CODE == rawMaterialCode
                //                      orderby row.RM_DIA_MIN
                //                      select row).ToList<DDRM_SIZE_MAST>();
                //}
                //else
                //{

                //    lstRawMaterial = (from row in DB.DDRM_SIZE_MAST
                //                      orderby row.RM_DIA_MIN
                //                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false ||row.DELETE_FLAG == false)
                //                      select row).ToList<DDRM_SIZE_MAST>();
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstRawMaterial;
        }

        /// <summary>
        /// Userd to get Raw Materials
        /// </summary>
        /// <param name="rawMaterialDescription">Code of the Raw Material</param>
        /// <returns>List of Raw Material</returns>
        public List<DDRM_MAST> GetRawMaterialsByCode(string rawMaterialCode = "")
        {
            List<DDRM_MAST> lstRawMaterial = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstRawMaterial;
                if (rawMaterialCode.IsNotNullOrEmpty())
                {
                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false) && row.RM_CODE == rawMaterialCode
                                      select row).ToList<DDRM_MAST>();
                }
                else
                {

                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false)
                                      select row).ToList<DDRM_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstRawMaterial;
        }

        public List<DDRM_MAST> GetRawMaterialsByPrimaryKey(DDRM_MAST paramEntity = null)
        {
            List<DDRM_MAST> lstRawMaterial = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstRawMaterial;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false) && row.IDPK == paramEntity.IDPK
                                      select row).ToList<DDRM_MAST>();
                }
                else
                {
                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (row.RM_CODE != null && row.RM_CODE.Trim() != "") && (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false)
                                      orderby row.IDPK descending
                                      select row).ToList<DDRM_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstRawMaterial;
        }

        /// <summary>
        /// Userd to get Raw Materials
        /// </summary>
        /// <param name="rawMaterialCode">Code of the Raw Material</param>
        /// <returns>List of Raw Material</returns>
        public List<DDRM_MAST> GetRawMaterialsByDescription(string rawMaterialDescription = "")
        {
            List<DDRM_MAST> lstRawMaterial = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstRawMaterial;
                if (rawMaterialDescription.IsNotNullOrEmpty())
                {
                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false) &&
                                      row.RM_DESC == rawMaterialDescription
                                      select row).ToList<DDRM_MAST>();
                }
                else
                {

                    lstRawMaterial = (from row in DB.DDRM_MAST
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false)
                                      select row).ToList<DDRM_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstRawMaterial;
        }

        /// <summary>
        /// Used to get Raw Material Class
        /// </summary>
        /// <param name="rawMaterialCode">Code of the Raw Material</param>
        /// <returns>List of Raw Material Class</returns>
        public List<RAW_MATERIAL_CLASS> GetRawMaterialClass(string rawMaterialCode = "")
        {
            List<RAW_MATERIAL_CLASS> lstRawMaterial = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstRawMaterial;
                if (rawMaterialCode.IsNotNullOrEmpty())
                {
                    lstRawMaterial = (from row in DB.RAW_MATERIAL_CLASS
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false) &&
                                       Convert.ToString(row.CODE) == rawMaterialCode.Substring(0, 1)
                                      select row).ToList<RAW_MATERIAL_CLASS>();
                }
                else
                {
                    lstRawMaterial = (from row in DB.RAW_MATERIAL_CLASS
                                      where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == false)
                                      select row).ToList<RAW_MATERIAL_CLASS>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstRawMaterial;
        }

        public bool Insert<T>(List<T> entities)
        {
            bool returnValue = false;

            foreach (T entity in entities)
            {
                if ((entity as DDRM_MAST).IsNotNullOrEmpty())
                {
                    DDRM_MAST obj = entity as DDRM_MAST;
                    try
                    {

                        obj.ROWID = Guid.NewGuid();
                        obj.DELETE_FLAG = false;
                        obj.ENTERED_BY = userName;
                        obj.ENTERED_DATE = serverDateTime;

                        DB.DDRM_MAST.InsertOnSubmit(obj);
                        DB.SubmitChanges();
                        DB.Refresh(RefreshMode.OverwriteCurrentValues);
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
                        DB.DDRM_MAST.DeleteOnSubmit(obj);

                    }
                }
                if ((entity as DDRM_SIZE_MAST).IsNotNullOrEmpty())
                {
                    DDRM_SIZE_MAST obj = entity as DDRM_SIZE_MAST;
                    try
                    {
                        if (obj.IDPK == 0) obj.IDPK = GenerateNextNumber("DDRM_SIZE_MAST", "IDPK").ToIntValue();
                        obj.ROWID = Guid.NewGuid();
                        obj.DELETE_FLAG = false;
                        obj.ENTERED_BY = userName;
                        obj.ENTERED_DATE = serverDateTime;

                        DB.DDRM_SIZE_MAST.InsertOnSubmit(obj);
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
                        DB.DDRM_SIZE_MAST.DeleteOnSubmit(obj);

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
                if ((entity as DDRM_MAST).IsNotNullOrEmpty())
                {
                    DDRM_MAST obj = null;
                    DDRM_MAST activeEntity = (entity as DDRM_MAST);
                    try
                    {
                        obj = (from row in DB.DDRM_MAST
                               where row.IDPK == activeEntity.IDPK
                               select row).SingleOrDefault<DDRM_MAST>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.RM_CODE = activeEntity.RM_CODE;
                            obj.RM_DESC = activeEntity.RM_DESC;
                            obj.LOC_COST = activeEntity.LOC_COST;
                            obj.EXP_COST = activeEntity.EXP_COST;
                            obj.DELETE_FLAG = false;
                            obj.UPDATED_BY = userName;
                            obj.UPDATED_DATE = serverDateTime;

                            DB.SubmitChanges();
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

                        DB.DDRM_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                    }
                }
                if ((entity as DDRM_SIZE_MAST).IsNotNullOrEmpty())
                {
                    DDRM_SIZE_MAST objChild = null;
                    DDRM_SIZE_MAST activeEntityChild = (entity as DDRM_SIZE_MAST);
                    try
                    {
                        objChild = (from row in DB.DDRM_SIZE_MAST
                                    where row.IDPK == activeEntityChild.IDPK
                                    select row).SingleOrDefault<DDRM_SIZE_MAST>();

                        if (objChild.IsNotNullOrEmpty())
                        {
                            objChild.RM_CODE = activeEntityChild.RM_CODE;
                            objChild.RM_DIA_MIN = activeEntityChild.RM_DIA_MIN;
                            objChild.RM_DIA_MAX = activeEntityChild.RM_DIA_MAX;
                            objChild.LOC_CODE = activeEntityChild.LOC_CODE.IsNotNullOrEmpty() ? activeEntityChild.LOC_CODE : "0";

                            objChild.DELETE_FLAG = false;
                            objChild.UPDATED_BY = userName;
                            objChild.UPDATED_DATE = serverDateTime;

                            DB.SubmitChanges();
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
                        DB.DDRM_SIZE_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, activeEntityChild);

                    }
                }

            }

            return returnValue;
        }

        public bool Delete<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as DDRM_MAST).IsNotNullOrEmpty())
                {
                    DDRM_MAST obj = null;
                    DDRM_MAST activeEntity = (entity as DDRM_MAST);
                    try
                    {
                        obj = (from row in DB.DDRM_MAST
                               where row.IDPK == activeEntity.IDPK
                               select row).SingleOrDefault<DDRM_MAST>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.RM_CODE = activeEntity.RM_CODE;
                            obj.RM_DESC = activeEntity.RM_DESC;
                            obj.LOC_COST = activeEntity.LOC_COST;
                            obj.EXP_COST = activeEntity.EXP_COST;
                            obj.DELETE_FLAG = true;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.DDRM_MAST.DeleteOnSubmit(obj);
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
                        DB.DDRM_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);

                    }
                }
                if ((entity as DDRM_SIZE_MAST).IsNotNullOrEmpty())
                {
                    DDRM_SIZE_MAST objChild = null;
                    DDRM_SIZE_MAST activeEntityChild = (entity as DDRM_SIZE_MAST);
                    try
                    {
                        objChild = (from row in DB.DDRM_SIZE_MAST
                                    where row.IDPK == activeEntityChild.IDPK
                                    select row).SingleOrDefault<DDRM_SIZE_MAST>();

                        if (objChild.IsNotNullOrEmpty())
                        {
                            objChild.RM_CODE = activeEntityChild.RM_CODE;
                            objChild.RM_DIA_MIN = activeEntityChild.RM_DIA_MIN;
                            objChild.RM_DIA_MAX = activeEntityChild.RM_DIA_MAX;
                            objChild.LOC_CODE = activeEntityChild.LOC_CODE.IsNotNullOrEmpty() ? activeEntityChild.LOC_CODE : "0";

                            objChild.DELETE_FLAG = false;
                            objChild.UPDATED_BY = userInformation.UserName;
                            objChild.UPDATED_DATE = serverDateTime;

                            DB.DDRM_SIZE_MAST.DeleteOnSubmit(objChild);
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
                        DB.DDRM_SIZE_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, activeEntityChild);

                    }
                }

            }
            return returnValue;
        }

        public bool DeleteRMSizeMaster(DDRM_MAST ddrmmast)
        {
            List<DDRM_SIZE_MAST> lstentity = new List<DDRM_SIZE_MAST>();
            bool delete = false;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return false;
                lstentity = (from row in DB.DDRM_SIZE_MAST
                             where row.RM_CODE == ddrmmast.RM_CODE
                             select row).ToList<DDRM_SIZE_MAST>();
                if (lstentity.Count > 0)
                {
                    delete = true;
                    DB.DDRM_SIZE_MAST.DeleteAllOnSubmit(lstentity);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (delete == true)
                {
                    ex.LogException();
                    DB.DDRM_SIZE_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstentity);
                }

            }
            return false;
        }
        /*
        public bool Delete<T>(List<T> entities)
        {
            bool returnValue = false;
            foreach (T entity in entities)
            {
                if ((entity as DDRM_MAST).IsNotNullOrEmpty())
                {
                    DDRM_MAST obj = null;
                    DDRM_MAST activeEntity = (entity as DDRM_MAST);
                    try
                    {
                        obj = (from row in DB.DDRM_MAST
                               where row.IDPK == activeEntity.IDPK
                               select row).SingleOrDefault<DDRM_MAST>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.RM_CODE = activeEntity.RM_CODE;
                            obj.RM_DESC = activeEntity.RM_DESC;
                            obj.LOC_COST = activeEntity.LOC_COST;
                            obj.EXP_COST = activeEntity.EXP_COST;
                            obj.DELETE_FLAG = true;
                            obj.UPDATED_BY = userInformation.UserName;
                            obj.UPDATED_DATE = serverDateTime;
                            DB.DDRM_MAST.DeleteOnSubmit(obj);
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
                        DB.DDRM_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);
                        throw ex.LogException();
                    }
                }
                if ((entity as DDRM_SIZE_MAST).IsNotNullOrEmpty())
                {
                    DDRM_SIZE_MAST objChild = null;
                    DDRM_SIZE_MAST activeEntityChild = (entity as DDRM_SIZE_MAST);
                    try
                    {
                        objChild = (from row in DB.DDRM_SIZE_MAST
                                    where row.IDPK == activeEntityChild.IDPK
                                    select row).SingleOrDefault<DDRM_SIZE_MAST>();

                        if (objChild.IsNotNullOrEmpty())
                        {
                            objChild.RM_CODE = activeEntityChild.RM_CODE;
                            objChild.RM_DIA_MIN = activeEntityChild.RM_DIA_MIN;
                            objChild.RM_DIA_MAX = activeEntityChild.RM_DIA_MAX;
                            objChild.LOC_CODE = activeEntityChild.LOC_CODE.IsNotNullOrEmpty() ? activeEntityChild.LOC_CODE : "0";

                            objChild.DELETE_FLAG = false;
                            objChild.UPDATED_BY = userInformation.UserName;
                            objChild.UPDATED_DATE = serverDateTime;

                            DB.DDRM_SIZE_MAST.DeleteOnSubmit(objChild);
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
                        DB.DDRM_SIZE_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, activeEntityChild);
                        throw ex.LogException();
                    }
                }

            }
            return returnValue;
        }
        */
    }
}
