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
    public class TfcBll : Essential
    {
        public TfcBll(UserInformation userInfo)
        {
            this.userInformation = userInfo;
        }

        public bool GetPartNoDetails(TfcModel tfcModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                if (dt != null)
                {
                    tfcModel.PartNoDetails = dt.DefaultView;
                }
                else
                {
                    tfcModel.PartNoDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetOtherDetails(TfcModel tm)
        {
            try
            {
                var ps = (from a in DB.PRD_CIREF
                          join b in DB.DDCI_INFO on a.CI_REF equals b.CI_REFERENCE
                          join c in DB.DDCUST_MAST on b.CUST_CODE equals c.CUST_CODE
                          where a.SNO == 1 && a.PART_NO == tm.PartNo
                          select new { b.CUST_DWG_NO, c.CUST_NAME }).FirstOrDefault();
                if (ps != null)
                {
                    tm.CUSTOMERPARTNO = ps.CUST_DWG_NO;
                    tm.CUSTOMERNAME = ps.CUST_NAME;
                }
                else
                {
                    tm.CUSTOMERPARTNO = "";
                    tm.CUSTOMERNAME = "";
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public string GetRoute(TfcModel tm)
        {
            string allow = "YES";
            try
            {
                DataTable dtroute, dtpccs;
                dtroute = ToDataTable((from r in DB.PROCESS_MAIN
                                       where r.CURRENT_PROC == 1 && r.PART_NO == tm.PartNo
                                       select new { r.ROUTE_NO }).ToList());
                if (dtroute.IsNotNullOrEmpty())
                {
                    if (dtroute.Rows.Count > 0)
                    {
                        dtpccs = ToDataTable((from p in DB.PCCS
                                              where p.PART_NO == tm.PartNo && p.ROUTE_NO == dtroute.Rows[0]["ROUTE_NO"].ToString().ToDecimalValue() && (p.SPEC_CHAR.Contains("CRITICAL") || p.SPEC_CHAR.Contains("MAJOR") || p.SPEC_CHAR.Contains("KEY"))
                                              select new { p.SPEC_MIN, p.SPEC_MAX, p.FEATURE }).ToList());
                        if (dtpccs.Rows.Count > 0)
                        {
                            allow = "NO";
                        }
                        else
                        {
                            allow = "YES";
                        }
                    }
                }
                return allow;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }

        public bool GetDateApplication(TfcModel tm)
        {
            try
            {
                var dp = (from a in DB.CFT_MEET_MASTER
                          where a.PART_NO == tm.PartNo
                          select new { a.APPLICATION, a.CFT_MEET_DATE }).FirstOrDefault();
                if (dp != null)
                {
                    tm.APPLICATION = dp.APPLICATION;
                    tm.DATE = dp.CFT_MEET_DATE.ToFormattedDateAsString("dd/MM/yyyy");
                }
                if (tm.APPLICATION == null)
                {
                    tm.APPLICATION = "";
                }
                if (tm.DATE == null)
                {
                    tm.DATE = "";
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetAll(TfcModel tfcmodel)
        {
            try
            {
                var getall = (from a in DB.TFCOMMIT
                              where a.PART_NO == tfcmodel.PartNo
                              select a).FirstOrDefault();
                if (getall != null)
                {
                    tfcmodel.AUTO_PART = getall.AUTO_PART;
                    tfcmodel.CUST_PROG = getall.CUST_PROG;
                    tfcmodel.Q1 = getall.Q1;
                    tfcmodel.Q2 = getall.Q2;
                    tfcmodel.Q3 = getall.Q3;
                    tfcmodel.Q4 = getall.Q4;
                    tfcmodel.Q5 = getall.Q5;
                    tfcmodel.Q6 = getall.Q6;
                    tfcmodel.Q7 = getall.Q7;
                    tfcmodel.Q8 = getall.Q8;
                    tfcmodel.Q9 = getall.Q9;
                    tfcmodel.Q10 = getall.Q10;
                    tfcmodel.Q11 = getall.Q11;
                    tfcmodel.Q12 = getall.Q12;
                    tfcmodel.Q13 = getall.Q13;
                    tfcmodel.CONCLUSION = getall.CONCLUSION.ToString();
                    tfcmodel.IMPACT1 = getall.IMPACT1;
                    tfcmodel.IMPACT2 = getall.IMPACT2;
                    tfcmodel.IMPACT3 = getall.IMPACT3;
                    tfcmodel.IMPACT4 = getall.IMPACT4;
                    tfcmodel.IMPACT5 = getall.IMPACT5;
                    tfcmodel.IMPACT6 = getall.IMPACT6;
                    tfcmodel.IMPACT7 = getall.IMPACT7;
                    tfcmodel.REMARKS1 = getall.REASON1;
                    tfcmodel.REMARKS2 = getall.REASON2;
                    tfcmodel.REMARKS3 = getall.REASON3;
                    tfcmodel.REMARKS4 = getall.REASON4;
                    tfcmodel.REMARKS5 = getall.REASON5;
                    tfcmodel.REMARKS6 = getall.REASON6;
                    tfcmodel.REMARKS7 = getall.REASON7;
                    tfcmodel.Q1 = getall.Q1;
                    tfcmodel.Q2 = getall.Q2;
                    tfcmodel.Q3 = getall.Q3;
                    tfcmodel.Q4 = getall.Q4;
                    tfcmodel.Q5 = getall.Q5;
                    tfcmodel.Q6 = getall.Q6;
                    tfcmodel.Q7 = getall.Q7;
                    tfcmodel.Q8 = getall.Q8;
                    tfcmodel.Q9 = getall.Q9;
                    tfcmodel.Q10 = getall.Q10;
                    tfcmodel.Q11 = getall.Q11;
                    tfcmodel.Q12 = getall.Q12;
                    tfcmodel.Q13 = getall.Q13;
                }
                else
                {
                    tfcmodel.AUTO_PART = "";
                    tfcmodel.CUST_PROG = "";
                    tfcmodel.Q1 = "";
                    tfcmodel.Q2 = "";
                    tfcmodel.Q3 = "";
                    tfcmodel.Q4 = "";
                    tfcmodel.Q5 = "";
                    tfcmodel.Q6 = "";
                    tfcmodel.Q7 = "";
                    tfcmodel.Q8 = "";
                    tfcmodel.Q9 = "";
                    tfcmodel.Q10 = "";
                    tfcmodel.Q11 = "";
                    tfcmodel.Q12 = "";
                    tfcmodel.Q13 = "";
                    tfcmodel.CONCLUSION = "";
                    tfcmodel.IMPACT1 = "";
                    tfcmodel.IMPACT2 = "";
                    tfcmodel.IMPACT3 = "";
                    tfcmodel.IMPACT4 = "";
                    tfcmodel.IMPACT5 = "";
                    tfcmodel.IMPACT6 = "";
                    tfcmodel.IMPACT7 = "";
                    tfcmodel.REMARKS1 = "";
                    tfcmodel.REMARKS2 = "";
                    tfcmodel.REMARKS3 = "";
                    tfcmodel.REMARKS4 = "";
                    tfcmodel.REMARKS5 = "";
                    tfcmodel.REMARKS6 = "";
                    tfcmodel.REMARKS7 = "";
                    tfcmodel.Q1 = "";
                    tfcmodel.Q2 = "";
                    tfcmodel.Q3 = "";
                    tfcmodel.Q4 = "";
                    tfcmodel.Q5 = "";
                    tfcmodel.Q6 = "";
                    tfcmodel.Q7 = "";
                    tfcmodel.Q8 = "";
                    tfcmodel.Q9 = "";
                    tfcmodel.Q10 = "";
                    tfcmodel.Q11 = "";
                    tfcmodel.Q12 = "";
                    tfcmodel.Q13 = "";
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool AddEditTfc(TfcModel tfcmodel, string mode)
        {
            bool _status = false;
            TFCOMMIT tfc = (from t in DB.TFCOMMIT
                            where t.PART_NO == tfcmodel.PartNo
                            select t).FirstOrDefault<TFCOMMIT>();
            try
            {
                if (tfc == null)
                {
                    mode = "Add";
                    tfc = new TFCOMMIT();
                    tfc.PART_NO = tfcmodel.PartNo;
                    tfc.AUTO_PART = tfcmodel.AUTO_PART;
                    tfc.CUST_PROG = tfcmodel.CUST_PROG;
                    tfc.Q1 = tfcmodel.Q1;
                    tfc.Q2 = tfcmodel.Q2;
                    tfc.Q3 = tfcmodel.Q3;
                    tfc.Q4 = tfcmodel.Q4;
                    tfc.Q5 = tfcmodel.Q5;
                    tfc.Q6 = tfcmodel.Q6;
                    tfc.Q7 = tfcmodel.Q7;
                    tfc.Q8 = tfcmodel.Q8;
                    tfc.Q9 = tfcmodel.Q9;
                    tfc.Q10 = tfcmodel.Q10;
                    tfc.Q11 = tfcmodel.Q11;
                    tfc.Q12 = tfcmodel.Q12;
                    tfc.Q13 = tfcmodel.Q13;
                    tfc.IMPACT1 = tfcmodel.IMPACT1;
                    tfc.IMPACT2 = tfcmodel.IMPACT2;
                    tfc.IMPACT3 = tfcmodel.IMPACT3;
                    tfc.IMPACT4 = tfcmodel.IMPACT4;
                    tfc.IMPACT5 = tfcmodel.IMPACT5;
                    tfc.IMPACT6 = tfcmodel.IMPACT6;
                    tfc.IMPACT7 = tfcmodel.IMPACT7;
                    tfc.REASON1 = tfcmodel.REMARKS1;
                    tfc.REASON2 = tfcmodel.REMARKS2;
                    tfc.REASON3 = tfcmodel.REMARKS3;
                    tfc.REASON4 = tfcmodel.REMARKS4;
                    tfc.REASON5 = tfcmodel.REMARKS5;
                    tfc.REASON6 = tfcmodel.REMARKS6;
                    tfc.REASON7 = tfcmodel.REMARKS7;
                    tfc.CONCLUSION = tfcmodel.CONCLUSION.ToDecimalValue();
                    tfc.ROWID = Guid.NewGuid();
                    DB.TFCOMMIT.InsertOnSubmit(tfc);
                    DB.SubmitChanges();
                    _status = true;
                }
                else if (tfc != null)
                {
                    mode = "Edit";
                    tfc.AUTO_PART = tfcmodel.AUTO_PART;
                    tfc.CUST_PROG = tfcmodel.CUST_PROG;
                    tfc.Q1 = tfcmodel.Q1;
                    tfc.Q2 = tfcmodel.Q2;
                    tfc.Q3 = tfcmodel.Q3;
                    tfc.Q4 = tfcmodel.Q4;
                    tfc.Q5 = tfcmodel.Q5;
                    tfc.Q6 = tfcmodel.Q6;
                    tfc.Q7 = tfcmodel.Q7;
                    tfc.Q8 = tfcmodel.Q8;
                    tfc.Q9 = tfcmodel.Q9;
                    tfc.Q10 = tfcmodel.Q10;
                    tfc.Q11 = tfcmodel.Q11;
                    tfc.Q12 = tfcmodel.Q12;
                    tfc.Q13 = tfcmodel.Q13;
                    tfc.IMPACT1 = tfcmodel.IMPACT1;
                    tfc.IMPACT2 = tfcmodel.IMPACT2;
                    tfc.IMPACT3 = tfcmodel.IMPACT3;
                    tfc.IMPACT4 = tfcmodel.IMPACT4;
                    tfc.IMPACT5 = tfcmodel.IMPACT5;
                    tfc.IMPACT6 = tfcmodel.IMPACT6;
                    tfc.IMPACT7 = tfcmodel.IMPACT7;
                    tfc.REASON1 = tfcmodel.REMARKS1;
                    tfc.REASON2 = tfcmodel.REMARKS2;
                    tfc.REASON3 = tfcmodel.REMARKS3;
                    tfc.REASON4 = tfcmodel.REMARKS4;
                    tfc.REASON5 = tfcmodel.REMARKS5;
                    tfc.REASON6 = tfcmodel.REMARKS6;
                    tfc.REASON7 = tfcmodel.REMARKS7;
                    tfc.CONCLUSION = tfcmodel.CONCLUSION.ToDecimalValue();
                    DB.SubmitChanges();
                    _status = false;

                }
                return _status;
            }
            catch (Exception ex)
            {
                if (mode == "Add")
                {
                    DB.TFCOMMIT.DeleteOnSubmit(tfc);
                }
                else if (mode == "Edit")
                {
                    DB.TFCOMMIT.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, tfc);
                }
                throw ex.LogException();
            }
        }


        public DataTable print(TfcModel tfcmodel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from p in DB.PROCESS_MAIN
                                  from c in DB.CONTROL_PLAN.Where(mapping => mapping.PART_NO == p.PART_NO && mapping.ROUTE_NO == p.ROUTE_NO).DefaultIfEmpty()                                                                 
                                  where p.ROUTE_NO == tfcmodel.ROUTENO.ToDecimalValue() && p.PART_NO == tfcmodel.PartNo
                                  select new
                                  {
                                      p.PART_NO,
                                      p.ROUTE_NO,
                                      c.CORE_TEAM_MEMBER1,
                                      c.CORE_TEAM_MEMBER2,
                                      c.CORE_TEAM_MEMBER3,
                                      c.CORE_TEAM_MEMBER4,
                                      c.CORE_TEAM_MEMBER5,
                                      c.CORE_TEAM_MEMBER6,
                                      c.CORE_TEAM_MEMBER7,
                                      EX_NO = "",
                                      REVISION_NO = "",
                                      CUST_NAME = "",
                                      CUST_PARTNO = "",
                                      DATE = "",
                                      PART_NAME = "",
                                      APPLIC_ATION = "",
                                      AUTOPART_YES = "",
                                      AUTOPART_NO = "",
                                      PROTOTYPE = "",
                                      PRELAUNCH = "",
                                      PRODUCTION = "",
                                      FEASIBLE_CHANGE = "",
                                      FEASIBLE_PRODUCT = "",
                                      NOT_FEASIBLE = "",
                                      Q1 = "",
                                      Q2 = "",
                                      Q3 = "",
                                      Q4 = "",
                                      Q5 = "",
                                      Q6 = "",
                                      Q7 = "",
                                      Q8 = "",
                                      Q9 = "",
                                      Q10 = "",
                                      Q11 = "",
                                      Q12 = "",
                                      Q13 = "",
                                      IMPACT1 = "",
                                      IMPACT2 = "",
                                      IMPACT3 = "",
                                      IMPACT4 = "",
                                      IMPACT5 = "",
                                      IMPACT6 = "",
                                      IMPACT7 = "",
                                      REASON1 = "",
                                      REASON2 = "",
                                      REASON3 = "",
                                      REASON4 = "",
                                      REASON5 = "",
                                      REASON6 = "",
                                      REASON7 = ""
                                  }).ToList());
                if (dt != null && dt.Rows.Count > 0)
                {
                    EXHIBIT_DOC exhibit = (from o in DB.EXHIBIT_DOC
                                           where o.DOC_NAME == "TFC"
                                           select o).FirstOrDefault<EXHIBIT_DOC>();
                    if (exhibit != null)
                    {
                        dt.Rows[0]["EX_NO"] = exhibit.EX_NO;
                        dt.Rows[0]["REVISION_NO"] = exhibit.REVISION_NO;
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Rows[0]["CUST_NAME"] = tfcmodel.CUSTOMERNAME;
                    dt.Rows[0]["CUST_PARTNO"] = tfcmodel.CUSTOMERPARTNO;
                    dt.Rows[0]["DATE"] = tfcmodel.DATE;
                    dt.Rows[0]["PART_NAME"] = tfcmodel.PartDesc;
                    dt.Rows[0]["APPLIC_ATION"] = tfcmodel.APPLICATION;
                    dt.Rows[0]["AUTOPART_YES"] = tfcmodel.AUTOPARTYES;
                    dt.Rows[0]["AUTOPART_NO"] = tfcmodel.AUTOPARTNO;
                    dt.Rows[0]["PROTOTYPE"] = tfcmodel.PROTOTYPE;
                    dt.Rows[0]["PRELAUNCH"] = tfcmodel.PRELAUNCH;
                    dt.Rows[0]["PRODUCTION"] = tfcmodel.PRODUCTION;
                    dt.Rows[0]["FEASIBLE_CHANGE"] = tfcmodel.FEASIBLECHANGE;
                    dt.Rows[0]["FEASIBLE_PRODUCT"] = tfcmodel.FEASIBLEPRODUCT;
                    dt.Rows[0]["NOT_FEASIBLE"] = tfcmodel.NOTFEASIBLE;
                    dt.Rows[0]["Q1"] = tfcmodel.Q1;
                    dt.Rows[0]["Q2"] = tfcmodel.Q2;
                    dt.Rows[0]["Q3"] = tfcmodel.Q3;
                    dt.Rows[0]["Q4"] = tfcmodel.Q4;
                    dt.Rows[0]["Q5"] = tfcmodel.Q5;
                    dt.Rows[0]["Q6"] = tfcmodel.Q6;
                    dt.Rows[0]["Q7"] = tfcmodel.Q7;
                    dt.Rows[0]["Q8"] = tfcmodel.Q8;
                    dt.Rows[0]["Q9"] = tfcmodel.Q9;
                    dt.Rows[0]["Q10"] = tfcmodel.Q10;
                    dt.Rows[0]["Q11"] = tfcmodel.Q11;
                    dt.Rows[0]["Q12"] = tfcmodel.Q12;
                    dt.Rows[0]["Q13"] = tfcmodel.Q13;
                    dt.Rows[0]["IMPACT1"] = tfcmodel.IMPACT1;
                    dt.Rows[0]["IMPACT2"] = tfcmodel.IMPACT2;
                    dt.Rows[0]["IMPACT3"] = tfcmodel.IMPACT3;
                    dt.Rows[0]["IMPACT4"] = tfcmodel.IMPACT4;
                    dt.Rows[0]["IMPACT5"] = tfcmodel.IMPACT5;
                    dt.Rows[0]["IMPACT6"] = tfcmodel.IMPACT6;
                    dt.Rows[0]["IMPACT7"] = tfcmodel.IMPACT7;
                    dt.Rows[0]["REASON1"] = tfcmodel.REMARKS1;
                    dt.Rows[0]["REASON2"] = tfcmodel.REMARKS2;
                    dt.Rows[0]["REASON3"] = tfcmodel.REMARKS3;
                    dt.Rows[0]["REASON4"] = tfcmodel.REMARKS4;
                    dt.Rows[0]["REASON5"] = tfcmodel.REMARKS5;
                    dt.Rows[0]["REASON6"] = tfcmodel.REMARKS6;
                    dt.Rows[0]["REASON7"] = tfcmodel.REMARKS7;
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
