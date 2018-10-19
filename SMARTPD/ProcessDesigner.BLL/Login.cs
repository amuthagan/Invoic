using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.DAL;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.Model;
using ProcessDesigner.HashEncryption;

namespace ProcessDesigner.BLL
{
    public class Login : Essential
    {

        PasswordManager pwdManager = null;
        public Login(string connectionStringName)
        {
            try
            {
                this.connectionStringName = connectionStringName;
                Dal = new DataAccessLayer(connectionStringName);
                if (!Dal.IsNotNullOrEmpty())
                {
                    throw new Exception("Connection could not be established");
                }

                DB = new SFLPD_UAT(Dal.connection);
                pwdManager = new PasswordManager();
                migrateUsers();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private string connectionStringName { get; set; }

        public bool IsValidUser(string userName, string password, out bool isForceToChangePassword)
        {
            isForceToChangePassword = false;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return false;
                SEC_USER_MASTER login = (from o in DB.SEC_USER_MASTER
                                         where o.USER_NAME == userName
                                         && (o.DELETE_FLAG == false || o.DELETE_FLAG == null)
                                         select o).SingleOrDefault<SEC_USER_MASTER>();
                if (login.IsNotNullOrEmpty())
                {
                    isForceToChangePassword = Convert.ToBoolean(Convert.ToInt16(login.FORCE_PASSWORD));
                    return pwdManager.IsPasswordMatch(password, login.SALT.ToArray(), login.PASSWORD.ToArray());
                }
                else return false;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
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

        private bool migrateUsers()
        {
            bool returnValue = false;
            try
            {
                //List<SEC_USER_MASTER> login = (from row in DB.SEC_USER_MASTER
                //                               where Convert.ToBoolean(Convert.ToInt16(row.FORCE_PASSWORD)) == true &&
                //                               row.SALT == null
                //                               select row).ToList<SEC_USER_MASTER>();

                List<SEC_USER_MASTER> login = (from row in DB.SEC_USER_MASTER
                                               where row.SALT == null
                                               select row).ToList<SEC_USER_MASTER>();

                foreach (SEC_USER_MASTER sec in login)
                {
                    byte[] salt;
                    sec.PASSWORD = pwdManager.GeneratePasswordHash((sec.USER_NAME.Length > 3 ? sec.USER_NAME.Substring(0, 3) : sec.USER_NAME) + "#123$", out salt); //adm#123$
                    sec.SALT = salt;
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnValue;

        }

        public bool GetisAdmin(string userName)
        {
            bool returnValue = false;
            try
            {
                if (DB.IsNotNullOrEmpty())
                {
                    SEC_USER_MASTER userMaster = (from row in DB.SEC_USER_MASTER
                                                  where
                                                  row.USER_NAME == userName
                                                  && (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                                  select row).FirstOrDefault();
                    if (userMaster.IsNotNullOrEmpty())
                    {
                        returnValue = userMaster.IS_ADMIN.ToBooleanAsString();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnValue;
        }

        public string GetVersion()
        {
            string returnValue = string.Empty;
            try
            {
                if (DB.IsNotNullOrEmpty())
                {

                    var result = (from t in DB.SMARTPD_VERSION
                                  // where t.UserID == 6
                                  orderby t.VERSION_NO descending
                                  select t.VERSION_NO).First();


                    //SMARTPD_VERSION userMaster = (from row in DB.SMARTPD_VERSION
                    //                            //  where
                    //                           //   row.ID == 1
                    //                              select row.VERSION_NO).Last();
                    if (result.IsNotNullOrEmpty())
                    {
                        returnValue = result.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnValue;
        }


        public bool IsValidVersionNo(string versionNo)
        {
            try
            {
                if (!DB.IsNotNullOrEmpty()) return false;
                SMARTPD_VERSION version = (from o in DB.SMARTPD_VERSION
                                           orderby o.ID descending
                                           select o).FirstOrDefault<SMARTPD_VERSION>();
                if (version != null)
                {
                    if (version.VERSION_NO.ToValueAsString().Trim() == versionNo.Trim())
                        return true;
                    else
                        return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


    }
}
