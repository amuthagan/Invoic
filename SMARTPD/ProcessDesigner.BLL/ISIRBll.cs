using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using System.ComponentModel;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class ISIRBll : Essential
    {
        public ISIRBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool GetISIR(ISIRModel im)
        {
            try
            {
                var ps = (from p in DB.PRD_MAST
                          join r in DB.PRD_CIREF on p.PART_NO equals r.PART_NO
                          join i in DB.DDCI_INFO on r.CI_REF equals i.CI_REFERENCE
                          join c in DB.DDCUST_MAST on i.CUST_CODE equals c.CUST_CODE
                          where p.PART_NO == im.PART_NO && r.CURRENT_CIREF == true
                          select new
                          {
                              c.CUST_NAME,
                              p.PART_DESC,
                          }).FirstOrDefault();
                if (ps != null)
                {
                    im.CUST_NAME = ps.CUST_NAME;
                    im.DESCRIPTION = ps.PART_DESC;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
