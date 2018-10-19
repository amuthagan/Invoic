using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class QcpBll : Essential
    {
        public QcpBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataTable GetQCP(PCCSModel pccs, QcpModel qcp)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from r in DB.PRD_CIREF
                                  join p in DB.PRD_MAST on r.PART_NO equals p.PART_NO
                                  into a from b in a.DefaultIfEmpty()
                                  join i in DB.DDCI_INFO on r.CI_REF equals i.CI_REFERENCE
                                  join c in DB.DDCUST_MAST on i.CUST_CODE equals c.CUST_CODE
                                  join e in DB.PROCESS_MAIN on r.PART_NO equals e.PART_NO
                                  join d in DB.CONTROL_PLAN on e.ROUTE_NO equals d.ROUTE_NO
                                  into y from z in y.DefaultIfEmpty()
                                  where z.PART_NO == e.PART_NO && e.CURRENT_PROC == 1 && r.PART_NO == qcp.PartNo.ToUpper() && r.CURRENT_CIREF == true
                                  select new
                                  {
                                      c.CUST_NAME,
                                      i.ENQU_RECD_ON,
                                      b.PART_DESC,
                                      b.BIF_PROJ,
                                      r.PART_NO,
                                      e.ROUTE_NO,
                                      z.CORE_TEAM_MEMBER1,
                                      z.CORE_TEAM_MEMBER2,
                                      z.CORE_TEAM_MEMBER3,
                                      z.CORE_TEAM_MEMBER4,
                                      z.CORE_TEAM_MEMBER5,
                                      z.CORE_TEAM_MEMBER6,
                                      EX_NO = "",
                                      REVISION_NO = ""
                                  }).ToList());

                if (dt != null && dt.Rows.Count > 0)
                {
                    EXHIBIT_DOC exhibit = (from o in DB.EXHIBIT_DOC
                                           where o.DOC_NAME == "PQPSO"
                                           select o).FirstOrDefault<EXHIBIT_DOC>();
                    if (exhibit != null)
                    {
                        dt.Rows[0]["EX_NO"] = exhibit.EX_NO;
                        dt.Rows[0]["REVISION_NO"] = exhibit.REVISION_NO;
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
