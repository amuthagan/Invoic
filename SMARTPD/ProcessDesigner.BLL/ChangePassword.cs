using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.HashEncryption;

namespace ProcessDesigner.BLL
{

    public class ChangePassword : Essential
    {
        PasswordManager pwdManager = null;
        public ChangePassword(UserInformation userInformation)
        {
            try
            {
                this.userInformation = userInformation;
                pwdManager = new PasswordManager();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool IsValidUser(string userName, string password)
        {
            try
            {
                if (!DB.IsNotNullOrEmpty()) return false;
                SEC_USER_MASTER login = (from o in DB.SEC_USER_MASTER
                                         where o.USER_NAME == userName
                                         && (o.DELETE_FLAG == false || o.DELETE_FLAG == null)
                                         select o).SingleOrDefault<SEC_USER_MASTER>();
                if (login.IsNotNullOrEmpty())
                {
                    return pwdManager.IsPasswordMatch(password, login.SALT.ToArray(), login.PASSWORD.ToArray());
                }
                else return false;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool ValidateAndUpdatePassword(String userName, String oldPassword, String newPassword, String verifyPassword, ref String message)
        {

            try
            {
                message = "";
                if (IsValidUser(userName, oldPassword))
                {
                    if (newPassword == verifyPassword)
                    {
                       // DB.SEC_USER_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.SEC_USER_MASTER);
                        List<SEC_USER_MASTER> lstUser = (from row in DB.SEC_USER_MASTER
                                                         where row.USER_NAME == userName
                                                         && (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                                         select row).ToList<SEC_USER_MASTER>();

                        foreach (SEC_USER_MASTER user in lstUser)
                        {
                            try
                            {
                                byte[] salt;
                                user.PASSWORD = pwdManager.GeneratePasswordHash(newPassword, out salt);
                                user.SALT = salt;
                                user.FORCE_PASSWORD = false;
                                DB.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.SEC_USER_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, user);
                            }

                        }

                        message = "Password Changed";
                        return true;
                    }
                    else
                    {
                        message = "Password verification error";
                        return false;
                    }
                }
                else
                {
                    message = "The Old Password is incorrect";
                    return false;
                }
            }
            catch (Exception ex)
            {
                DB.Transaction.Rollback();
                throw ex.LogException();
            }
        }
    }
}
