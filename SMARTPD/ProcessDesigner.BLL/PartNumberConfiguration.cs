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
    public class PartNumberConfiguration : Essential
    {
        public PartNumberConfiguration(UserInformation userInformation)
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

        public List<PartNumberConfig> GetEntitiesByID(PartNumberConfig partNoConfig = null)
        {

            List<PartNumberConfig> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (partNoConfig.IsNotNullOrEmpty() && partNoConfig.ID.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.ID == partNoConfig.ID
                                 select row).ToList<PartNumberConfig>();
                }
                else
                {

                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false
                                 select row).ToList<PartNumberConfig>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PartNumberConfig> GetEntitiesByCode(PartNumberConfig partNoConfig = null)
        {

            List<PartNumberConfig> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (partNoConfig.IsNotNullOrEmpty() && partNoConfig.Code.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.Code == partNoConfig.Code
                                 select row).ToList<PartNumberConfig>();
                }
                else
                {

                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false
                                 select row).ToList<PartNumberConfig>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }


        public List<PartNumberConfig> GetEntitiesByDescription(PartNumberConfig partNoConfig = null)
        {

            List<PartNumberConfig> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (partNoConfig.IsNotNullOrEmpty() && partNoConfig.Description.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.Description == partNoConfig.Description
                                 select row).ToList<PartNumberConfig>();
                }
                else
                {

                    lstEntity = (from row in DB.PartNumberConfig
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false
                                 select row).ToList<PartNumberConfig>();
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
        public List<DDLOC_MAST> GetLocationsByCode(DDLOC_MAST locationEntity = null)
        {
            List<DDLOC_MAST> lstLocations = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstLocations;
                if (locationEntity.IsNotNullOrEmpty() && locationEntity.LOC_CODE.IsNotNullOrEmpty())
                {
                    lstLocations = (from row in DB.DDLOC_MAST
                                    where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.LOC_CODE == locationEntity.LOC_CODE
                                    select row).ToList<DDLOC_MAST>();

                }
                else
                {

                    lstLocations = (from row in DB.DDLOC_MAST
                                    where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false
                                    select row).ToList<DDLOC_MAST>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstLocations;
        }

        public bool Insert<T>(List<T> entities)
        {
            bool returnValue = false;
            if (!DB.IsNotNullOrEmpty()) return returnValue;


            foreach (T entity in entities)
            {

                if ((entity as PartNumberConfig).IsNotNullOrEmpty())
                {
                    PartNumberConfig obj = entity as PartNumberConfig;
                    try
                    {
                        obj.DELETE_FLAG = false;
                        obj.ENTERED_BY = userName;
                        obj.ENTERED_DATE = serverDateTime;
                        PartNumberConfig partNumConfig = (from c in DB.PartNumberConfig
                                                          where c.ID == obj.ID
                                                          select c).SingleOrDefault<PartNumberConfig>();
                        if (partNumConfig == null)
                        {
                            DB.PartNumberConfig.InsertOnSubmit(obj);
                        }
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
                        DB.PartNumberConfig.DeleteOnSubmit(obj);
                    }
                }


            }

            returnValue = true;

            return returnValue;
        }

        public bool Update<T>(List<T> entities)
        {
            bool returnValue = false;
            if (!DB.IsNotNullOrEmpty()) return returnValue;

            foreach (T entity in entities)
            {
                if ((entity as PartNumberConfig).IsNotNullOrEmpty())
                {
                    PartNumberConfig obj = null;
                    PartNumberConfig activeEntity = (entity as PartNumberConfig);
                    try
                    {

                        obj = (from row in DB.PartNumberConfig
                               where row.ID == activeEntity.ID
                               select row).SingleOrDefault<PartNumberConfig>();
                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.Code = activeEntity.Code;
                            obj.Description = activeEntity.Description;
                            obj.Location_code = activeEntity.Location_code;
                            obj.Prefix = activeEntity.Prefix;
                            obj.BeginningNo = activeEntity.BeginningNo;
                            obj.EndingNo = activeEntity.EndingNo;
                            obj.Suffix = activeEntity.Suffix;
                            obj.CurrentValue = activeEntity.CurrentValue;
                            obj.IsObsolete = activeEntity.IsObsolete;

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
                        DB.PartNumberConfig.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);
                        throw ex.LogException();
                    }
                }

            }

            return returnValue;
        }

        public bool Delete<T>(List<T> entities)
        {
            bool returnValue = false;
            try
            {
                //foreach (T entity in entities)
                //{
                //    try
                //    {
                //        if ((entity as PartNumberConfig).IsNotNullOrEmpty())
                //        {
                //            PartNumberConfig obj = null;
                //            PartNumberConfig activeEntity = (entity as PartNumberConfig);

                //            obj = (from row in DB.PartNumberConfig
                //                   where row.Code == activeEntity.Code
                //                   select row).SingleOrDefault<PartNumberConfig>();
                //            if (obj.IsNotNullOrEmpty())
                //            {
                //                obj.Description = activeEntity.Description;
                //                obj.LOC_COST = activeEntity.LOC_COST;
                //                obj.EXP_COST = activeEntity.EXP_COST;
                //                obj.DELETE_FLAG = true;
                //                obj.UPDATED_BY = userInformation.UserName;
                //                obj.UPDATED_DATE = serverDateTime;
                //                DB.SubmitChanges();
                //                returnValue = true;
                //            }
                //        }
                //        if ((entity as DDRM_SIZE_MAST).IsNotNullOrEmpty())
                //        {
                //            DDRM_SIZE_MAST objChild = null;
                //            DDRM_SIZE_MAST activeEntityChild = (entity as DDRM_SIZE_MAST);

                //            objChild = (from row in DB.DDRM_SIZE_MAST
                //                        where row.ROWID == activeEntityChild.ROWID
                //                        select row).SingleOrDefault<DDRM_SIZE_MAST>();

                //            if (objChild.IsNotNullOrEmpty())
                //            {
                //                objChild.RM_DIA_MIN = activeEntityChild.RM_DIA_MIN;
                //                objChild.RM_DIA_MAX = activeEntityChild.RM_DIA_MAX;
                //                objChild.LOC_CODE = activeEntityChild.LOC_CODE.IsNotNullOrEmpty() ? activeEntityChild.LOC_CODE : "0";

                //                objChild.DELETE_FLAG = false;
                //                objChild.UPDATED_BY = userInformation.UserName;
                //                objChild.UPDATED_DATE = serverDateTime;

                //                DB.SubmitChanges();
                //                returnValue = true;
                //            }

                //        }
                //    }
                //    catch (ChangeConflictException)
                //    {
                //        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                //        {
                //            conflict.Resolve(RefreshMode.KeepChanges);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnValue;
        }


    }
}
