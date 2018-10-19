using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using ProcessDesigner.Common;

namespace ProcessDesigner.BLL
{
    public class UserRoleDet : Essential
    {

        //IEnumerable<SEC_ROLE_OBJECT_PERMISSION> insertpermission;

        public UserRoleDet(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }


        /// <summary>
        /// This function is used to save the roles
        /// </summary>
        /// <param name="secrolesmaster"></param>
        /// <returns></returns>
        public bool SaveRoles(SEC_ROLES_MASTER secrolesmaster, OperationMode mode)
        {
            string oldRoleName = "";
            SEC_ROLES_MASTER editsecrolesmaster = new SEC_ROLES_MASTER();
            bool submit = false;
            bool insert = false;
            bool update = false;
            bool insertpermission = false;
            List<SEC_ROLE_OBJECT_PERMISSION> lstPermission = new List<SEC_ROLE_OBJECT_PERMISSION>();
            try
            {
                if (mode == OperationMode.AddNew)
                {
                    secrolesmaster.ROWID = Guid.NewGuid();
                    secrolesmaster.ENTERED_BY = this.userInformation.UserName;
                    secrolesmaster.ENTERED_DATE = DateTime.Now;
                    secrolesmaster.DELETE_FLAG = false;
                    insert = true;
                    DB.SEC_ROLES_MASTER.InsertOnSubmit(secrolesmaster);
                    if (InsertDefaultPermissionForRole(secrolesmaster.ROLE_NAME, ref lstPermission))
                    {
                        submit = true;
                        insertpermission = true;
                        DB.SEC_ROLE_OBJECT_PERMISSION.InsertAllOnSubmit(lstPermission);
                        DB.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    editsecrolesmaster = (from c in DB.SEC_ROLES_MASTER
                                          where c.ROWID == secrolesmaster.ROWID
                                          select c).Single<SEC_ROLES_MASTER>();
                    oldRoleName = editsecrolesmaster.ROLE_NAME;
                    editsecrolesmaster.ROLE_NAME = secrolesmaster.ROLE_NAME;
                    editsecrolesmaster.UPDATED_DATE = serverDateTime;
                    editsecrolesmaster.UPDATED_BY = userInformation.UserName;
                    submit = true;
                    DB.SubmitChanges();
                    if (oldRoleName.Trim() != editsecrolesmaster.ROLE_NAME.Trim())
                    {
                        SaveRoleForPermission(oldRoleName, editsecrolesmaster.ROLE_NAME);
                        UpdateRolesForUser(oldRoleName, editsecrolesmaster.ROLE_NAME);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                if (submit == true)
                {
                    if (insert == true)
                    {
                        DB.SEC_ROLES_MASTER.DeleteOnSubmit(secrolesmaster);
                    }
                    else if (update == true)
                    {
                        DB.SEC_ROLES_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, editsecrolesmaster);
                    }
                    if (insertpermission == true)
                    {
                        DB.SEC_ROLE_OBJECT_PERMISSION.DeleteAllOnSubmit(lstPermission);
                    }
                }
                
            }
            return false;
        }

        /// <summary>
        /// Check  duplicate role name
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public bool CheckDuplicate(SEC_ROLES_MASTER secroleparam, OperationMode opermode)
        {
            SEC_ROLES_MASTER secrolemaster = null;

            try
            {
                if (opermode == OperationMode.Edit)
                {
                    secrolemaster = (from secrole in DB.SEC_ROLES_MASTER
                                     where secrole.ROLE_NAME.ToUpper() == secroleparam.ROLE_NAME.ToUpper().Trim()
                                          && Convert.ToBoolean(Convert.ToInt16(secrole.DELETE_FLAG)) == false
                                          && secrole.ROWID != secroleparam.ROWID
                                     select secrole).SingleOrDefault<SEC_ROLES_MASTER>();
                }
                else
                {
                    secrolemaster = (from secrole in DB.SEC_ROLES_MASTER
                                     where secrole.ROLE_NAME.ToUpper() == secroleparam.ROLE_NAME.ToUpper().Trim()
                                          && Convert.ToBoolean(Convert.ToInt16(secrole.DELETE_FLAG)) == false
                                     select secrole).SingleOrDefault<SEC_ROLES_MASTER>();
                }
                if (secrolemaster == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Insert default permission for the role
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns>boolean</returns>
        private bool InsertDefaultPermissionForRole(string rolename, ref List<SEC_ROLE_OBJECT_PERMISSION> parampermission)
        {
            try
            {
                List<SEC_ROLE_OBJECT_PERMISSION> lstpermission = new List<SEC_ROLE_OBJECT_PERMISSION>();

                SEC_ROLE_OBJECT_PERMISSION permission;


                //bool submit = false;
                /*
                The below commented line not working due to
                explicit-construction-of-entity-type-in-query-is-not-allowed
                IEnumerable<SEC_ROLE_OBJECT_PERMISSION> lstpermission = from row in DB.SEC_OBJECTS_MASTER
                                                                        select new SEC_ROLE_OBJECT_PERMISSION
                                                                        {
                                                                            OBJECT_NAME = row.OBJECT_NAME,
                                                                            ROLE_NAME=rolename.ToUpper(),
                                                                            PERM_ADD=false,
                                                                            PERM_EDIT=false,
                                                                            PERM_VIEW=false,
                                                                            PERM_PRINT=false,
                                                                            DELETE_FLAG=false,
                                                                            ENTERED_DATE=DateTime.Now,
                                                                            ENTERED_BY=userInformation.UserName
                                                                        };
                DB.SEC_ROLE_OBJECT_PERMISSION.InsertAllOnSubmit(lstpermission.ToList());
                DB.SubmitChanges();
                */

                IEnumerable<SEC_OBJECTS_MASTER> lstobjmaster = (from row in DB.SEC_OBJECTS_MASTER
                                                                select row);

                foreach (SEC_OBJECTS_MASTER objmaster in lstobjmaster)
                {
                    permission = new SEC_ROLE_OBJECT_PERMISSION();
                    permission.ROWID = Guid.NewGuid();
                    permission.ROLE_NAME = rolename;
                    permission.DELETE_FLAG = false;
                    permission.ENTERED_DATE = DateTime.Now;
                    permission.ENTERED_BY = userInformation.UserName;
                    permission.OBJECT_NAME = objmaster.OBJECT_NAME;
                    permission.PERM_ADD = false;
                    permission.PERM_DELETE = false;
                    permission.PERM_EDIT = false;
                    permission.PERM_PRINT = false;
                    permission.PERM_VIEW = false;
                    permission.PERM_SHOW = false;
                    lstpermission.Add(permission);
                    ///DB.SEC_ROLE_OBJECT_PERMISSION.InsertOnSubmit(permission);
                }
                parampermission = lstpermission;
                return true;
            }
            catch (Exception ex)
            {

                throw ex.LogException();
            }
        }

        private bool SaveRoleForPermission(string oldrolename, string newrolename)
        {
            List<SEC_ROLE_OBJECT_PERMISSION> lstpermission = null;
            List<SEC_ROLE_OBJECT_PERMISSION> newpermission = new List<SEC_ROLE_OBJECT_PERMISSION>();
            SEC_ROLE_OBJECT_PERMISSION permission;
            bool insert = false;
            bool submit = false;
           // bool update = false;
            try
            {
                lstpermission = (from dspermission in DB.SEC_ROLE_OBJECT_PERMISSION.AsEnumerable()
                                 where dspermission.ROLE_NAME.ToUpper() == oldrolename.Trim().ToUpper()
                                 select dspermission).ToList<SEC_ROLE_OBJECT_PERMISSION>();
                foreach (SEC_ROLE_OBJECT_PERMISSION roleperm in lstpermission)
                {
                    permission = new SEC_ROLE_OBJECT_PERMISSION();
                    permission.ROWID = Guid.NewGuid();
                    permission.ROLE_NAME = newrolename;
                    permission.DELETE_FLAG = false;
                    permission.ENTERED_DATE = serverDate;
                    permission.ENTERED_BY = userInformation.UserName;
                    permission.OBJECT_NAME = roleperm.OBJECT_NAME;
                    permission.PERM_ADD = roleperm.PERM_ADD;
                    permission.PERM_DELETE = roleperm.PERM_DELETE;
                    permission.PERM_EDIT = roleperm.PERM_EDIT;
                    permission.PERM_PRINT = roleperm.PERM_PRINT;
                    permission.PERM_VIEW = roleperm.PERM_VIEW;
                    permission.PERM_SHOW = roleperm.PERM_SHOW;
                    newpermission.Add(permission);
                }
                insert = true;
                if (newpermission.Count > 0)
                {
                    DB.SEC_ROLE_OBJECT_PERMISSION.DeleteAllOnSubmit(lstpermission);
                }
                DB.SEC_ROLE_OBJECT_PERMISSION.InsertAllOnSubmit(newpermission);
                submit = true;
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
                if (submit == true)
                {
                    if (insert == true)
                    {
                        DB.SEC_ROLE_OBJECT_PERMISSION.DeleteAllOnSubmit(newpermission);
                    }
                }


            }
            return false;

        }

        /// <summary>
        /// This function is used to remove the role name from the database
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public bool DeleteRole(string rolename)
        {
            try
            {
                IEnumerable<SEC_ROLE_OBJECT_PERMISSION> secroleobjpermission;
                IEnumerable<SEC_USER_ROLES> secuserroles;
                SEC_ROLES_MASTER secrolesmaster;


                secroleobjpermission = from details in DB.SEC_ROLE_OBJECT_PERMISSION
                                       where details.ROLE_NAME == rolename
                                       select details;

                secuserroles = from details in DB.SEC_USER_ROLES
                               where details.ROLE_NAME.Trim().ToUpper() == rolename.Trim().ToUpper()
                               select details;

                secrolesmaster = (from details in DB.SEC_ROLES_MASTER
                                  where details.ROLE_NAME.Trim().ToUpper() == rolename.Trim().ToUpper()
                                  select details).SingleOrDefault();

                if (secuserroles != null)
                {
                    if (secuserroles.Count() > 0)
                    {
                        DB.SEC_USER_ROLES.DeleteAllOnSubmit(secuserroles);
                    }
                }

                if (secroleobjpermission != null)
                {
                    if (secroleobjpermission.Count() > 0)
                    {
                        DB.SEC_ROLE_OBJECT_PERMISSION.DeleteAllOnSubmit(secroleobjpermission);
                    }
                }

                if (secrolesmaster != null)
                {
                    DB.SEC_ROLES_MASTER.DeleteOnSubmit(secrolesmaster);
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


        public bool CheckUserExistForRole(string rolename)
        {
            IEnumerable<SEC_USER_ROLES> secuserroles;
            try
            {
                secuserroles = from details in DB.SEC_USER_ROLES
                               where details.ROLE_NAME == rolename
                               select details;
                if (secuserroles.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        private void UpdateRolesForUser(string oldrolename, string newrolename)
        {
            List<SEC_USER_ROLES> lstuserroles = new List<SEC_USER_ROLES>();
            bool submit = false;
            try
            {
                lstuserroles = (from details in DB.SEC_USER_ROLES
                                where details.ROLE_NAME.ToUpper().Trim() == oldrolename.ToUpper().Trim()
                                && (details.DELETE_FLAG == false || details.DELETE_FLAG == null)
                                select details).ToList<SEC_USER_ROLES>();

                foreach (SEC_USER_ROLES userroles in lstuserroles)
                {
                    userroles.ROLE_NAME = newrolename;
                }
                submit = true;
                DB.SubmitChanges();
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                
            }
            catch (Exception ex)
            {
                ex.LogException();
                if (submit == true)
                {
                    DB.SEC_USER_ROLES.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstuserroles);
                }
                
            }
        }
    }
}
