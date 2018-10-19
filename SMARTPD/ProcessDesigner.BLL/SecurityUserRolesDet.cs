using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System.Collections.ObjectModel;


namespace ProcessDesigner.BLL
{
    public class SecurityUserRolesDet : Essential
    {
        public SecurityUserRolesDet()
        {
            ////this.userInformation = (UserInformation)App.Current.Properties["userinfo"]; 
        }

        public SecurityUserRolesDet(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        /// <summary>
        /// Get Available Roles
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SEC_ROLES_MASTER> GetAvailableRoles(string username)
        {

            var oc = new ObservableCollection<SEC_ROLES_MASTER>();

            List<SEC_ROLES_MASTER> lstrole = null;
            try
            {
                lstrole = (from role in DB.SEC_ROLES_MASTER.AsEnumerable()
                           where Convert.ToBoolean(Convert.ToInt16(role.DELETE_FLAG)) == false
                           && !DB.SEC_USER_ROLES.Any(m => m.ROLE_NAME == role.ROLE_NAME && m.USER_NAME == username)
                           select role).ToList<SEC_ROLES_MASTER>();

                foreach (var item in lstrole)
                {
                    oc.Add(item);
                }
                //lstrole.ForEach(x => oc.Add(x));
                return oc;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Get the available for the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ObservableCollection<SEC_USER_ROLES> GetAvailableRolesForTheUser(string username)
        {
            List<SEC_USER_ROLES> lstrole = null;
            var oc = new ObservableCollection<SEC_USER_ROLES>();
            try
            {
                lstrole = (from role in DB.SEC_USER_ROLES.AsEnumerable()
                           where Convert.ToBoolean(Convert.ToInt16(role.DELETE_FLAG)) == false
                           && role.USER_NAME.ToUpper() == username.ToUpper()
                           select role).ToList<SEC_USER_ROLES>();

                foreach (var item in lstrole)
                {
                    oc.Add(item);
                }
                //lstrole.ForEach(x => oc.Add(x));
                return oc;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        /// <summary>
        /// Save the roles for the selected user
        /// </summary>
        /// <param name="lstrole"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool SaveRolesForTheUser(ObservableCollection<SEC_USER_ROLES> lstrole, string username)
        {
            bool submit = false;
            bool insert = false;
            List<SEC_USER_ROLES> lstsecuserroles;
            SEC_USER_ROLES newsecuser = new SEC_USER_ROLES();
            try
            {
                lstsecuserroles = (from role in DB.SEC_USER_ROLES
                                   where 
                                   role.USER_NAME.ToUpper().Trim() == username.ToUpper().Trim()
                                   select role).ToList<SEC_USER_ROLES>();
                DB.SEC_USER_ROLES.DeleteAllOnSubmit(lstsecuserroles);
                DB.SubmitChanges();



                foreach (SEC_USER_ROLES secuserroles in lstrole)
                {
                    newsecuser = new SEC_USER_ROLES();
                    newsecuser.DELETE_FLAG = false;
                    newsecuser.ENTERED_BY = username;
                    newsecuser.ENTERED_DATE = serverDateTime;
                    newsecuser.IDPK = 0;
                    newsecuser.ROLE_NAME = secuserroles.ROLE_NAME;
                    newsecuser.ROWID = Guid.NewGuid();
                    newsecuser.USER_NAME = username;
                    insert = true;
                    DB.SEC_USER_ROLES.InsertOnSubmit(newsecuser);
                    submit = true;
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (insert == true)
                    {
                        DB.SEC_USER_ROLES.DeleteOnSubmit(newsecuser);
                        DB.SubmitChanges();
                    }
                }
                throw ex.LogException();
            }
        }
    }
}
