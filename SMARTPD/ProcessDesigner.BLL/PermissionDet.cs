using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.Model;


namespace ProcessDesigner.BLL
{
    public class PermissionDet : Essential
    {
        public PermissionDet(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        /// <summary>
        /// This function is used to fetch the list of rights for each role
        /// </summary>
        /// <param name="rolename">role name</param>
        /// <returns>Datatable</returns>
        public DataTable GetRolePermission(string rolename)
        {
            try
            {
                //SELECT ROLE_NAME, OBJECT_NAME,PERM_ADD, PERM_EDIT, PERM_VIEW, PERM_DELETE, PERM_PRINT FROM SEC_ROLE_OBJECT_PERMISSION WHERE ROLE_NAME = 'Administrator' Order by Object_name
                List<SEC_ROLE_OBJECT_PERMISSION> lstpermission = null;

                lstpermission = (from permission in DB.SEC_ROLE_OBJECT_PERMISSION.AsEnumerable()
                                 where permission.ROLE_NAME.ToUpper() == rolename.Trim().ToUpper()
                                 select permission).ToList<SEC_ROLE_OBJECT_PERMISSION>();
                return ToDataTable(lstpermission);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// This function is used to fetch the list of rights for each role
        /// </summary>
        /// <param name="rolename">role name</param>
        /// <returns>Datatable</returns>
        public List<SEC_ROLE_OBJECT_PERMISSION> GetRolePermissionList(string rolename)
        {
            try
            {
                //SELECT ROLE_NAME, OBJECT_NAME,PERM_ADD, PERM_EDIT, PERM_VIEW, PERM_DELETE, PERM_PRINT FROM SEC_ROLE_OBJECT_PERMISSION WHERE ROLE_NAME = 'Administrator' Order by Object_name
                List<SEC_ROLE_OBJECT_PERMISSION> lstpermission = null;

                lstpermission = (from permission in DB.SEC_ROLE_OBJECT_PERMISSION.AsEnumerable()
                                 where permission.ROLE_NAME.ToUpper() == rolename.Trim().ToUpper()
                                 select permission).ToList<SEC_ROLE_OBJECT_PERMISSION>();
                return lstpermission;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool SavePermission(DataTable permission, string rolename)
        {
            try
            {
                SEC_ROLE_OBJECT_PERMISSION sc = new SEC_ROLE_OBJECT_PERMISSION();

                EnumerableRowCollection<SEC_ROLE_OBJECT_PERMISSION> sec_role_object_permission_arr = from row in permission.AsEnumerable()
                                                                                                     select new SEC_ROLE_OBJECT_PERMISSION
                                                                                                     {
                                                                                                         ROLE_NAME = row["ROLE_NAME"].ToValueAsString(),
                                                                                                         OBJECT_NAME = row["opn_code"].ToValueAsString(),
                                                                                                         PERM_ADD = row["PERM_ADD"].ToBooleanAsString(),
                                                                                                         PERM_DELETE = row["PERM_DELETE"].ToBooleanAsString(),
                                                                                                         PERM_EDIT = row["PERM_EDIT"].ToBooleanAsString(),
                                                                                                         PERM_PRINT = row["PERM_PRINT"].ToBooleanAsString(),
                                                                                                         DELETE_FLAG = false,
                                                                                                         ENTERED_BY = row["ENTERED_BY"].ToValueAsString(),
                                                                                                         ENTERED_DATE = Convert.ToDateTime(row["ENTERED_DATE"]),
                                                                                                         UPDATED_DATE = System.DateTime.Now,
                                                                                                         UPDATED_BY = userInformation.UserName
                                                                                                     };



                foreach (SEC_ROLE_OBJECT_PERMISSION sec_role_object_permission in sec_role_object_permission_arr)
                {
                    //DB.SEC_ROLE_OBJECT_PERMISSION.Attach(sec_role_object_permission);
                }
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

                return false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool IsPrintDisable(string formName)
        {
            try
            {

                if (formName.IsNotNullOrEmpty())
                {
                    SEC_OBJECTS_MASTER processMain = (from c in DB.SEC_OBJECTS_MASTER
                                                      where c.OBJECT_NAME == formName
                                                      select c).FirstOrDefault<SEC_OBJECTS_MASTER>();

                    if (processMain != null)
                    {
                        return (processMain.ISPRINT_DISABLE == null || processMain.ISPRINT_DISABLE.Value == false) ? true : false;
                    }
                    else if (processMain == null)
                    {
                        return false;
                    }
                }
                return true;


            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return true;
        }

        public bool SavePermission(List<SEC_ROLE_OBJECT_PERMISSION> permission, string rolename)
        {
            try
            {
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

                return false;
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.SEC_ROLE_OBJECT_PERMISSION.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, permission);

            }
            return false;
        }
    }
}
