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
    public class PartSubmissionWarrantBll : Essential
    {

        public PartSubmissionWarrantBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool GetPSW(PartSubmissionWarrantModel psw)
        {
            try
            {
                var ps = (from p in DB.PRD_MAST
                          join r in DB.PRD_CIREF on p.PART_NO equals r.PART_NO
                          join i in DB.DDCI_INFO on r.CI_REF equals i.CI_REFERENCE
                          join c in DB.DDCUST_MAST on i.CUST_CODE equals c.CUST_CODE
                          join l in DB.DDLOC_MAST on p.BIF_PROJ equals l.LOC_CODE
                          where p.PART_NO == psw.PART_NO && r.CURRENT_CIREF == true
                          select new
                          {
                              c.CUST_NAME,
                              i.CUST_DWG_NO,
                              i.CUST_DWG_NO_ISSUE,
                              i.CUST_CODE,
                              i.CUST_STD_DATE,
                              p.PART_DESC,
                              p.FINISH_WT,
                              i.CUST_STD_NO,
                              l.PhoneNo,
                              l.FaxNo,
                              l.Address,
                              l.LOC
                          }).FirstOrDefault();
                if (ps != null)
                {
                    psw.CUSTOMERNAME = ps.CUST_NAME;
                    psw.CUST_DWG_NO = ps.CUST_DWG_NO;
                    psw.CUST_DWG_NO_ISSUE = ps.CUST_DWG_NO_ISSUE;
                    psw.SHOWNDRAWINGNO = ps.CUST_STD_NO;
                    psw.CUST_STD_DATE = ps.CUST_STD_DATE;
                    psw.PART_DESC = ps.PART_DESC;
                    psw.PHONENO = ps.PhoneNo;
                    psw.FAXNO = ps.FaxNo;
                    psw.ADDRESS = ps.Address;
                    psw.LOC = ps.LOC;
                }
                EXHIBIT_DOC exhibit = (from o in DB.EXHIBIT_DOC
                                       where o.DOC_NAME == "PSW"
                                       select o).FirstOrDefault<EXHIBIT_DOC>();
                if (exhibit != null)
                {
                    psw.EX_NO = exhibit.EX_NO;
                    psw.REVISION_NO = exhibit.REVISION_NO;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetBuyerCode(PartSubmissionWarrantModel psw)
        {
            try
            {
                var ps = (from p in DB.PRD_MAST
                          join r in DB.PRD_CIREF on p.PART_NO equals r.PART_NO
                          join i in DB.DDCI_INFO on r.CI_REF equals i.CI_REFERENCE
                          join c in DB.DDCUST_MAST on i.CUST_CODE equals c.CUST_CODE
                          join d in DB.CUST_MAST on c.CUST_CODE equals Convert.ToDecimal(d.CUST_CD)
                          where p.PART_NO == psw.PART_NO && r.CURRENT_CIREF == true
                          select new
                          {
                              d.CUST_REF_FOR_SFL
                          }).FirstOrDefault();
                if (ps != null)
                {
                    psw.BUYER = ps.CUST_REF_FOR_SFL;
                }
                else
                {
                    psw.BUYER = "";
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetNamePSW(PartSubmissionWarrantModel psw)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from n in DB.PSW_WAR_TITLE
                                  select new { n.NAME, n.TITLE }).ToList());
                if (dt != null)
                {
                    return dt.DefaultView;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }

        }

        public DataView GetApplicationPSW(PartSubmissionWarrantModel psw)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from n in DB.APPLICATION
                                  select new { n.NewApplication }).ToList());
                if (dt != null)
                {
                    return dt.DefaultView;
                }
                else
                {
                    return null;
                }

            }

            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }

        }

    }
}
