using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.HashEncryption;
using System.Data;

namespace ProcessDesigner.BLL
{
    public class SecurityUsersBll : Essential
    {
        PasswordManager pwdManager = null;
        public SecurityUsersBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
            pwdManager = new PasswordManager();
        }

        public bool GetDesignation(SecurityUsersModel security)
        {
            try
            {

                DataTable dttable = new DataTable();
                //Keep the record when delete the user and maintain delete flag - 17/12/2015
                dttable = ToDataTable((from c in DB.SEC_USER_MASTER.AsEnumerable()
                                       where (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                                       orderby c.DESIGNATION
                                       select new { c.DESIGNATION }).Distinct().ToList());
                security.Design = dttable.DefaultView;

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool UpdateSecurityUsers(SecurityUsersModel user)
        {
            user.Status = "";
            try
            {
                if (user.Mode == "I")
                {
                    bool insert = true;
                    //Keep the record when delete the user and maintain delete flag - 17/12/2015
                    SEC_USER_MASTER secUsers = (from o in DB.SEC_USER_MASTER
                                                where o.USER_NAME == user.UserName
                                                && (o.DELETE_FLAG == false || o.DELETE_FLAG == null)
                                                select o).FirstOrDefault<SEC_USER_MASTER>();
                    try
                    {

                        if (secUsers != null)
                        {
                            secUsers = null;
                            user.Status = PDMsg.AlreadyExists("User");
                            return false;
                        }
                        secUsers = new SEC_USER_MASTER();
                        secUsers = (from o in DB.SEC_USER_MASTER
                                    where o.USER_NAME == user.UserName
                                    && (o.DELETE_FLAG == true)
                                    select o).FirstOrDefault<SEC_USER_MASTER>();
                        if (secUsers != null)
                        {
                            insert = false;
                            secUsers.DELETE_FLAG = false;
                        }
                        else
                        {
                            secUsers = new SEC_USER_MASTER();
                        }

                        secUsers.USER_NAME = user.UserName;
                        secUsers.FULL_NAME = user.FullName;
                        secUsers.DESIGNATION = user.Designation;
                        secUsers.IS_ADMIN = user.IsAdmin;

                        byte[] salt;
                        secUsers.PASSWORD = pwdManager.GeneratePasswordHash(user.Password, out salt);
                        secUsers.SALT = salt;
                        secUsers.FORCE_PASSWORD = false;
                        secUsers.ENTERED_BY = userInformation.UserName;
                        secUsers.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                        if (insert == true)
                        {
                            secUsers.ROWID = Guid.NewGuid();
                            DB.SEC_USER_MASTER.InsertOnSubmit(secUsers);
                        }
                        // AssignRolesForUser(user.UserName);
                        DB.SubmitChanges();

                        secUsers = null;
                        user.Status = PDMsg.SavedSuccessfully;
                        return true;
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                        user.Status = PDMsg.SavedSuccessfully;

                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.SEC_USER_MASTER.DeleteOnSubmit(secUsers);

                    }
                }
                else if (user.Mode == "U")
                {
                    SEC_USER_MASTER users = (from o in DB.SEC_USER_MASTER
                                             where o.USER_NAME == user.UserName
                                             && (o.DELETE_FLAG == false || o.DELETE_FLAG == null)
                                             select o).FirstOrDefault<SEC_USER_MASTER>();
                    try
                    {

                        if (users != null)
                        {
                            users.USER_NAME = user.UserName;
                            users.FULL_NAME = user.FullName;
                            users.DESIGNATION = user.Designation;

                            if (user.Password != "!@#$%^&*()")
                            {
                                byte[] salt;
                                users.PASSWORD = pwdManager.GeneratePasswordHash(user.Password, out salt);
                                users.SALT = salt;
                                users.FORCE_PASSWORD = false;
                            }
                            users.IS_ADMIN = user.IsAdmin;
                            users.UPDATED_BY = userInformation.UserName;
                            users.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                            DB.SubmitChanges();
                            users = null;
                            user.Status = PDMsg.UpdatedSuccessfully;
                            return true;
                        }
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                        user.Status = PDMsg.UpdatedSuccessfully;

                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.SEC_USER_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, users);

                    }
                }
                else if (user.Mode == "D")
                {
                    SEC_USER_MASTER users = (from o in DB.SEC_USER_MASTER
                                             where o.USER_NAME == user.UserName
                                             select o).FirstOrDefault<SEC_USER_MASTER>();
                    if (users != null)
                    {
                        //Keep the record when delete the user and maintain delete flag - 17/12/2015
                        users.DELETE_FLAG = true;
                        users.UPDATED_BY = userInformation.UserName;
                        users.UPDATED_DATE = DateTime.Now;
                        //DB.SEC_USER_MASTER.DeleteOnSubmit(users);
                        DB.SubmitChanges();
                        users = null;
                        user.Status = PDMsg.DeletedSuccessfully;
                        List<SEC_USER_ROLES> lstsecuserroles = (from role in DB.SEC_USER_ROLES
                                           where
                                           role.USER_NAME.ToUpper().Trim() == user.UserName.ToUpper().Trim()
                                           select role).ToList<SEC_USER_ROLES>();
                        DB.SEC_USER_ROLES.DeleteAllOnSubmit(lstsecuserroles);
                        DB.SubmitChanges();

                        return true;
                    }
                    else if (users == null)
                    {
                        user.Status = PDMsg.DoesNotExists("User");
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return true;
        }

        /// <summary>
        /// assign default roles administrator for the new user
        /// </summary>
        /// <param name="username"></param>
        private bool AssignRolesForUser(string username)
        {
            SEC_USER_ROLES sec_user_roles = null;
            try
            {
                sec_user_roles = new SEC_USER_ROLES();
                sec_user_roles.USER_NAME = username;
                sec_user_roles.ROLE_NAME = "Administrator";
                sec_user_roles.ROWID = Guid.NewGuid();
                sec_user_roles.DELETE_FLAG = false;
                sec_user_roles.ENTERED_DATE = serverDateTime;
                sec_user_roles.ENTERED_BY = userInformation.UserName;
                DB.SEC_USER_ROLES.InsertOnSubmit(sec_user_roles);
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
                DB.SEC_USER_ROLES.DeleteOnSubmit(sec_user_roles);

            }
            return false;
        }

        public bool CheckIsAdminAvailable()
        {
            //Keep the record when delete the user and maintain delete flag - 17/12/2015
            SEC_USER_MASTER users = (from o in DB.SEC_USER_MASTER
                                     where o.IS_ADMIN == true
                                     && (o.DELETE_FLAG == false || o.DELETE_FLAG == null)
                                     select o).FirstOrDefault<SEC_USER_MASTER>();

            try
            {
                if (users != null)
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

        public string GetUserRole(string userName)
        {
            string returnValue = "";
            try
            {
                if (DB.IsNotNullOrEmpty())
                {
                    SEC_USER_ROLES userRoles = (from row in DB.SEC_USER_ROLES
                                                where
                                                row.USER_NAME == userName
                                                orderby row.ROLE_NAME
                                                select row).FirstOrDefault();
                    if (userRoles.IsNotNullOrEmpty())
                    {
                        returnValue = userRoles.ROLE_NAME;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnValue;
        }


    }


}
