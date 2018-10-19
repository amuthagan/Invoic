using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;

namespace ProcessDesigner.BLL
{
    public class Security : Essential
    {
        public List<SEC_USER_MASTER> GetUsers()
        {
            List<SEC_USER_MASTER> lstUsers = null;
            try
            {
                lstUsers = (from u in DB.SEC_USER_MASTER
                            select u).ToList<SEC_USER_MASTER>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstUsers;

        }
    }
}
