using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.Model;


namespace ProcessDesigner.BLL
{
   public class RoleUserSecurityDet : Essential
    {

        DataTable dttable;

        public RoleUserSecurityDet(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }


        public bool GetUsers(RoleUserSecurityModel security)
        {
            try
            {
                dttable = new DataTable();
                dttable = ToDataTable((from c in DB.SEC_USER_MASTER.AsEnumerable()
                                       where (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                                       select c).ToList());
                security.Users = dttable.DefaultView;
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetRoles(RoleUserSecurityModel security)
        {
            try
            {
                dttable = new DataTable();
                dttable = ToDataTable((from c in DB.SEC_ROLES_MASTER.AsEnumerable()
                                       select c).ToList());
                security.Roles = dttable.DefaultView;
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}
