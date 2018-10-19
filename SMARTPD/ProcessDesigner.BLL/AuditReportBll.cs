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
    public class AuditReportBll : Essential
    {
        public AuditReportBll(UserInformation userinfo)
        {
            this.userInformation = userinfo;
        }

        public bool GeneratePrintDetails(AuditReportModel auditreport)
        {
            try
            {
                string getQuery = "";
                getQuery = "select p.PART_NO,a.ISSUENO,a.ISSUEDATE,b.ISSUENO AS SEQ_ISSUENO,b.ISSUEDATE AS SEQ_ISSUEDATE "
                           + "from prd_mast p left join  "
                           + "(select max(issue_no) as issueno,convert(varchar,max(issue_date),103) as issuedate,part_no from prd_dwg_issue where dwg_type=0 "
                           + "group by part_no) a on p.PART_NO = a.PART_NO left join "
                           + "(select max(issue_no) as issueno,convert(varchar,max(issue_date),103) as issuedate,part_no from prd_dwg_issue where dwg_type=1 "
                           + "group by part_no) b on p.PART_NO = b.PART_NO ";
                if (auditreport.NineDigitPartNo)
                {
                    getQuery = getQuery + "where len(p.part_no)=9 order by part_no";
                }
                else if (auditreport.SixDigitPartNo)
                {
                    getQuery = getQuery + "where len(p.part_no)=6 order by part_no";
                }

                DataTable dtAuditReport = ToDataTable(DB.ExecuteQuery<AuditReportDetails>(getQuery).ToList());

                string getQuery2 = "";
                getQuery2 = " select prd.PART_NO,min(dci.CUST_DWG_NO) as CUST_DWG_NO,max(dci.CUST_DWG_NO_ISSUE) as CUS_ISSUENO,"
                                 + " CASE convert(varchar,max(dci.CUST_STD_DATE),103) "
                                 + "  WHEN '30/12/1899' THEN SUBSTRING(convert(varchar,max(dci.CUST_STD_DATE),109),13,10)"
                                 + "  ELSE convert(varchar,max(dci.CUST_STD_DATE),103) "
                                 + " END as CUS_ISSUEDATE,dm.CUST_NAME,dci.CUST_STD_NO "
                                 + "from ddcust_mast dm join ddci_info dci on dm.CUST_CODE = dci.CUST_CODE join prd_ciref pci on dci.CI_REFERENCE=pci.CI_REF join prd_mast prd on pci.PART_NO=prd.PART_NO ";
                if (auditreport.NineDigitPartNo)
                {
                    getQuery2 = getQuery2 + "where len(prd.part_no)=9 AND pci.CURRENT_CIREF = 1 group by prd.PART_NO,dm.CUST_NAME,dci.cust_std_no";
                }
                else if (auditreport.SixDigitPartNo)
                {
                    getQuery2 = getQuery2 + "where len(prd.part_no)=6 AND pci.CURRENT_CIREF = 1 group by prd.PART_NO,dm.CUST_NAME,dci.cust_std_no";
                }

                DataTable dtAuditReport2 = ToDataTable(DB.ExecuteQuery<AuditReportDetails>(getQuery2).ToList());

                if (dtAuditReport != null && dtAuditReport.Rows.Count > 0 && dtAuditReport2 != null && dtAuditReport2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAuditReport.Rows)
                    {
                        DataRow[] drs = dtAuditReport2.Select("PART_NO = '" + dr["PART_NO"].ToString() + "'");
                        if (drs.Length > 0)
                        {
                            dr.BeginEdit();
                            dr["CUST_DWG_NO"] = drs[0]["CUST_DWG_NO"].ToString();
                            dr["CUS_ISSUENO"] = drs[0]["CUS_ISSUENO"].ToString();
                            dr["CUS_ISSUEDATE"] = drs[0]["CUS_ISSUEDATE"].ToString();
                            dr["CUST_STD_NO"] = drs[0]["CUST_STD_NO"].ToString();
                            dr["CUST_NAME"] = drs[0]["CUST_NAME"].ToString();
                            dr.EndEdit();
                        }
                    }
                }

                auditreport.DVAuditReport = dtAuditReport.DefaultView;
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }

    partial class AuditReportDetails
    {
        public string PART_NO { get; set; }
        public Nullable<Decimal> ISSUENO { get; set; }
        public string ISSUEDATE { get; set; }
        public Nullable<Decimal> SEQ_ISSUENO { get; set; }
        public string SEQ_ISSUEDATE { get; set; }
        public string CUST_DWG_NO { get; set; }
        public string CUS_ISSUENO { get; set; }
        public string CUS_ISSUEDATE { get; set; }
        public string CUST_STD_NO { get; set; }
        public string CUST_NAME { get; set; }
    }
}
