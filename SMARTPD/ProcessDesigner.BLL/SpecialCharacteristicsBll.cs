using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class SpecialCharacteristicsBll : Essential
    {
        public SpecialCharacteristicsBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataTable GetSpecialCharacteristics()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from p in DB.PCCS
                                  join pc in DB.PRD_CIREF on p.PART_NO equals pc.PART_NO
                                  join di in DB.DDCI_INFO on pc.CI_REF equals di.CI_REFERENCE
                                  join dc in DB.DDCUST_MAST on di.CUST_CODE equals dc.CUST_CODE
                                  join prd in DB.PRD_MAST on p.PART_NO equals prd.PART_NO
                                  where pc.CURRENT_CIREF == true && (p.SPEC_CHAR.StartsWith("C") || p.SPEC_CHAR.StartsWith("K") || p.SPEC_CHAR.StartsWith("M"))
                                  orderby p.PART_NO
                                  select new
                                  {
                                      p.PART_NO,
                                      p.SEQ_NO,
                                      prd.PART_DESC,
                                      dc.CUST_NAME,
                                      di.CUST_STD_NO,
                                      di.LOC_CODE,
                                      p.FEATURE,
                                      p.CTRL_SPEC_MIN,
                                      p.CTRL_SPEC_MAX,
                                      p.SPEC_CHAR
                                  }).ToList());
                return dt;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
