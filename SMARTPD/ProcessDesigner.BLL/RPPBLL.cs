using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data.Linq;

namespace ProcessDesigner.BLL
{
    public class RPPBLL : Essential
    {

        public RPPBLL(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }


        public DataView GetGridData(RPDModel rpdmodeldata)
        {
            try
            {
                DataView griddata;
                return griddata = ToDataTable((from i in DB.DDCI_CHAR
                                               where i.CI_REFERENCE == rpdmodeldata.CI_REFERENCE
                                               select new
                                               {
                                                   SLNO = i.SLNO,
                                                   CHARACTERISTIC = i.CHARACTERISTIC,
                                                   SEVERITY = i.SEVERITY,
                                                   CUSTOMER_EXP = i.CUSTOMER_EXP
                                               }).ToList()).DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        public DataView GetCIReferrence()
        {
            DataView dtCIReference;
            RPDModel model_rpd = new RPDModel();

            string getQuery = "SELECT  i.CI_REFERENCE, i.CUST_DWG_NO as PART_NO,f.PART_NO as SFLPART_NO, i.IDPK as IDPK,f.CIREF_NO_FK as CIREF_NO_FK  "
                             + " FROM DDCI_INFO i  	   left JOIN PRD_CIREF f   	  ON i.CI_REFERENCE = f.CI_REF";

            return dtCIReference = ToDataTable(DB.ExecuteQuery<RPDModel_Notify>(getQuery).ToList()).DefaultView;
            //     return dtCIReference = ToDataTable(DB.ExecuteQuery(GetQuery.ToString())).DefaultView;


        }
        //select cust_code,cust_name from ddcust_mast

        public DataView GetCustomerCode(decimal cust_code)
        {
            try
            {
                DataView dtCustName;
                return dtCustName = ToDataTable((from c in DB.DDCUST_MAST.AsEnumerable()
                                                 where c.CUST_CODE == ((cust_code != 0) ? cust_code : c.CUST_CODE) && (c.DELETE_FLAG == false || c.DELETE_FLAG == null)
                                                 orderby c.CUST_CODE ascending
                                                 select new { CUST_CODE = c.CUST_CODE, CUST_NAME = c.CUST_NAME }).ToList()).DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetPartNo()
        {
            try
            {

                DataView dtPartNo;
                return dtPartNo = ToDataTable((from c in DB.PRD_MAST.AsEnumerable()
                                               orderby c.PART_NO ascending
                                               select new { PART_NO = c.PART_NO, PART_DESC = c.PART_DESC }).ToList()).DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataTable GetReportData(RPDModel rpdmodeldata)
        {
            try
            {
                DataTable dtReportData;
                return dtReportData = ToDataTable((from o in DB.DDCI_INFO.AsEnumerable()
                                                   where (int)o.IDPK == (int)rpdmodeldata.IDPK && o.CI_REFERENCE == rpdmodeldata.CI_REFERENCE
                                                   select o).ToList());

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        //private string format = "DD/MM/YYYY";
        public DataView DtCustDataview;
        public bool GetRPDData(RPDModel rpdmodeldata)
        {
            try
            {
                List<DDCI_INFO> lstddciInfo = (from o in DB.DDCI_INFO.AsEnumerable()
                                               where (int)o.IDPK == (int)rpdmodeldata.IDPK && o.CI_REFERENCE == rpdmodeldata.CI_REFERENCE
                                               select o).ToList<DDCI_INFO>();

                DDCI_INFO ddciInfo = null;
                if (lstddciInfo != null && lstddciInfo.Count > 0)
                {
                    ddciInfo = lstddciInfo[0];

                    rpdmodeldata.CUST_DWG_NO = ddciInfo.CUST_DWG_NO.ToValueAsString();
                    rpdmodeldata.PROD_DESC = ddciInfo.PROD_DESC.ToValueAsString();
                    rpdmodeldata.CUST_STD_DATE = ddciInfo.CUST_STD_DATE;
                    rpdmodeldata.CUST_STD_NO = ddciInfo.CUST_STD_NO.ToValueAsString();
                    rpdmodeldata.CUST_CODE = ddciInfo.CUST_CODE;
                    DtCustDataview = GetCustomerCode(rpdmodeldata.CUST_CODE.ToString().ToDecimalValue());
                    if (DtCustDataview != null && DtCustDataview.Count > 0)
                    {
                        rpdmodeldata.CUST_NAME = DtCustDataview.Table.Rows[0]["CUST_NAME"].ToString();
                    }
                    else
                    {
                        rpdmodeldata.CUST_NAME = "";
                    }
                    rpdmodeldata.CUST_DWG_NO_ISSUE = ddciInfo.CUST_DWG_NO_ISSUE.ToValueAsString();
                    rpdmodeldata.CUST_STD_DATE_NEW = ddciInfo.CUST_STD_DATE.ToDateAsString("DD/MM/YYYY");
                    rpdmodeldata.ATP_DATE = ddciInfo.ATP_DATE;
                    if (string.IsNullOrEmpty(ddciInfo.EXPORT) || ((ddciInfo.EXPORT) == "0"))
                    {
                        rpdmodeldata.EXPORT = "Domestic";
                    }
                    else if ((ddciInfo.EXPORT) == "1")
                    {
                        rpdmodeldata.EXPORT = "Export";
                    }
                    else if ((ddciInfo.EXPORT) == "2")
                    {
                        rpdmodeldata.EXPORT = "Retail";
                    }
                    else if ((ddciInfo.EXPORT) == "3")
                    {
                        rpdmodeldata.EXPORT = "Wef";
                    }
                    else
                    {
                        rpdmodeldata.EXPORT = "Domestic";
                    }

                    rpdmodeldata.SIMILAR_PART_NO = ddciInfo.SIMILAR_PART_NO.ToValueAsString();
                    rpdmodeldata.GENERAL_REMARKS = ddciInfo.GENERAL_REMARKS.ToValueAsString();

                    if (ddciInfo.POTENTIAL == null)
                    {
                        rpdmodeldata.POTENTIAL = "0";
                    }
                    else
                    {
                        rpdmodeldata.POTENTIAL = ddciInfo.POTENTIAL.ToString();
                    }

                    if (ddciInfo.MONTHLY == null)
                    {
                        rpdmodeldata.MONTHLY = "0";
                    }
                    else
                    {
                        rpdmodeldata.MONTHLY = ddciInfo.MONTHLY.ToString();
                    }

                    if (string.IsNullOrEmpty(ddciInfo.APPLICATION) == false)
                    {
                        rpdmodeldata.APPLICATION = ddciInfo.APPLICATION.ToString();
                    }
                    if (string.IsNullOrEmpty(ddciInfo.AUTOPART) == false || ddciInfo.AUTOPART == null)
                    {

                        switch (ddciInfo.AUTOPART)
                        {
                            case "Y":
                                rpdmodeldata.AutoPart_Yes = true;
                                rpdmodeldata.AutoPart_No = false;
                                break;
                            case "N":
                                rpdmodeldata.AutoPart_Yes = false;
                                rpdmodeldata.AutoPart_No = true;
                                break;
                            default:
                                rpdmodeldata.AutoPart_Yes = true;
                                break;
                        }
                    }
                    else
                    {
                        rpdmodeldata.AutoPart_Yes = true;
                    }
                    if (string.IsNullOrEmpty(ddciInfo.SAFTYPART) == false || ddciInfo.SAFTYPART == null)
                    {
                        //RPDModelData.SAFTYPART = ddciInfo.SAFTYPART.ToString();
                        switch (ddciInfo.SAFTYPART)
                        {
                            case "Y":
                                rpdmodeldata.Safety_Yes = true;
                                rpdmodeldata.Safety_No = false;
                                break;
                            case "N":
                                rpdmodeldata.Safety_Yes = false;
                                rpdmodeldata.Safety_No = true;
                                break;
                            default:
                                rpdmodeldata.Safety_Yes = true;
                                break;
                        }
                    }
                    else
                    {
                        rpdmodeldata.Safety_Yes = true;
                    }

                    if (Convert.ToDecimal(ddciInfo.STATUS) == 0)
                    {
                        rpdmodeldata.Opt_Prototype = true;
                        rpdmodeldata.Opt_PreLaunch = false;
                        rpdmodeldata.Opt_Production = false;
                    }
                    else if (Convert.ToDecimal(ddciInfo.STATUS) == 1)
                    {
                        rpdmodeldata.Opt_Prototype = false;
                        rpdmodeldata.Opt_PreLaunch = true;
                        rpdmodeldata.Opt_Production = false;
                    }
                    else if (Convert.ToDecimal(ddciInfo.STATUS) == 2)
                    {
                        rpdmodeldata.Opt_Prototype = false;
                        rpdmodeldata.Opt_PreLaunch = false;
                        rpdmodeldata.Opt_Production = true;
                    }
                    else
                    {
                        rpdmodeldata.Opt_Prototype = true;
                    }

                    if (Convert.ToDecimal(ddciInfo.DEVL_METHOD) == 0)
                    {
                        rpdmodeldata.Opt_Devlp_Proto = true;
                        rpdmodeldata.Opt_Devlp_Prelaunch = false;

                    }
                    else if (Convert.ToDecimal(ddciInfo.DEVL_METHOD) == 1)
                    {
                        rpdmodeldata.Opt_Devlp_Proto = false;
                        rpdmodeldata.Opt_Devlp_Prelaunch = true;

                    }
                    else
                    {
                        rpdmodeldata.Opt_Devlp_Proto = true;
                    }


                    rpdmodeldata.CUSTOMER_NEED_DT = ddciInfo.CUSTOMER_NEED_DT;
                    rpdmodeldata.MKTG_COMMITED_DT = ddciInfo.MKTG_COMMITED_DT;

                    if (string.IsNullOrEmpty(ddciInfo.PPAP_LEVEL) || ddciInfo.PPAP_LEVEL == null)
                    {
                        rpdmodeldata.PPAP_LEVEL = "1";
                    }
                    else
                    {
                        rpdmodeldata.PPAP_LEVEL = ddciInfo.PPAP_LEVEL.ToString();
                    }

                    if (ddciInfo.PPAP_FORGING == null)
                    {
                        rpdmodeldata.PPAP_FORGING = "0";
                    }
                    else
                    {
                        rpdmodeldata.PPAP_FORGING = ddciInfo.PPAP_FORGING.ToString();
                    }

                    if (ddciInfo.PPAP_SAMPLE == null)
                    {
                        rpdmodeldata.PPAP_SAMPLE = "0";
                    }
                    else
                    {
                        rpdmodeldata.PPAP_SAMPLE = ddciInfo.PPAP_SAMPLE.ToString();
                    }


                    if (ddciInfo.PACKING != null)
                    {
                        if (ddciInfo.PACKING == 1)
                        {
                            rpdmodeldata.Opt_Special = true;
                        }
                        else
                        {
                            rpdmodeldata.Opt_Stand = true;
                        }
                    }
                    else
                    {
                        rpdmodeldata.Opt_Stand = true;
                    }


                    rpdmodeldata.PACKING = ddciInfo.PACKING;
                    rpdmodeldata.NATURE_PACKING = ddciInfo.NATURE_PACKING;

                    if (ddciInfo.SPL_CHAR != null)
                    {
                        if (ddciInfo.SPL_CHAR == 1)
                        {
                            rpdmodeldata.Opt_Special_Yes = true;
                        }
                        else
                        {
                            rpdmodeldata.Opt_Special_No = true;
                        }
                    }
                    else
                    {
                        rpdmodeldata.Opt_Special_Yes = true;
                    }

                    rpdmodeldata.SPL_CHAR = ddciInfo.SPL_CHAR;
                    rpdmodeldata.OTHER_CUST_REQ = ddciInfo.OTHER_CUST_REQ;
                    rpdmodeldata.IDPK = ddciInfo.IDPK;

                    return true;
                }
                else
                {
                    // RPDModelData = null;
                    rpdmodeldata.CUST_DWG_NO = string.Empty;
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool CheckDuplicateGrid(RPDModel rpdmodeldata)
        {
            try
            {
                var lst = from b in rpdmodeldata.GridData.Table.AsEnumerable()
                          group b by b.Field<string>("CHARACTERISTIC") into g
                          let count = g.Count()
                          where count > 1
                          select new
                          {
                              ChargeTag = g.Key,
                              Count = count,
                          };



                foreach (var key in lst)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public bool CheckCIRefIsthere(string ciref)
        {
            try
            {
                DDCI_INFO ddciInfo = (from o in DB.DDCI_INFO.AsEnumerable()
                                      where o.CI_REFERENCE == ciref.ToString().Trim()
                                      && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select o).FirstOrDefault<DDCI_INFO>();
                if (ddciInfo != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public bool SaveRPDData(RPDModel rpdmodeldata)
        {
            try
            {
                return SaveRecord(rpdmodeldata);

                //if (SaveRecord(RPDModelData) == true)
                //{
                //     if (SaveGridData(RPDModelData)== true)
                //     {
                //         return true ;
                //     }
                //}
                //return false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool SaveRecord(RPDModel rpdmodeldata)
        {

            GenerateNextNumber("DDCI_INFO", "IDPK").ToIntValue();

            DDCI_INFO ddciInfo = (from o in DB.DDCI_INFO.AsEnumerable()
                                  where o.CI_REFERENCE == rpdmodeldata.CI_REFERENCE && o.IDPK == rpdmodeldata.IDPK
                                  && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select o).FirstOrDefault<DDCI_INFO>();



            if (ddciInfo != null)
            {
                // ddciInfo.CI_REFERENCE = RPDModelData.CI_REFERENCE;
                ddciInfo.ENQU_RECD_ON = rpdmodeldata.ENQU_RECD_ON;
                ddciInfo.FR_CS_DATE = rpdmodeldata.FR_CS_DATE;
                ddciInfo.PROD_DESC = rpdmodeldata.PROD_DESC;
                ddciInfo.CUST_CODE = rpdmodeldata.CUST_CODE;
                ddciInfo.CUST_DWG_NO = rpdmodeldata.CUST_DWG_NO;
                ddciInfo.CUST_DWG_NO_ISSUE = rpdmodeldata.CUST_DWG_NO_ISSUE;
                ddciInfo.EXPORT = rpdmodeldata.EXPORT;
                ddciInfo.NUMBER_OFF = rpdmodeldata.NUMBER_OFF;
                ddciInfo.POTENTIAL = rpdmodeldata.POTENTIAL.ToDecimalValue();
                ddciInfo.SFL_SHARE = rpdmodeldata.SFL_SHARE;
                ddciInfo.REMARKS = rpdmodeldata.REMARKS;
                ddciInfo.RESPONSIBILITY = rpdmodeldata.RESPONSIBILITY;
                ddciInfo.PENDING = rpdmodeldata.PENDING;
                ddciInfo.FEASIBILITY = rpdmodeldata.FEASIBILITY;
                ddciInfo.REJECT_REASON = rpdmodeldata.REJECT_REASON;
                ddciInfo.LOC_CODE = rpdmodeldata.LOC_CODE;
                ddciInfo.CHEESE_WT = rpdmodeldata.CHEESE_WT;
                ddciInfo.FINISH_WT = rpdmodeldata.FINISH_WT;
                //  ddciInfo.FINISH_CODE = RPDModelData.FINISH_CODE;
                ddciInfo.SUGGESTED_RM = rpdmodeldata.SUGGESTED_RM;
                ddciInfo.RM_COST = rpdmodeldata.RM_COST;
                ddciInfo.FINAL_COST = rpdmodeldata.FINAL_COST;
                ddciInfo.COST_NOTES = rpdmodeldata.COST_NOTES;
                ddciInfo.PROCESSED_BY = rpdmodeldata.PROCESSED_BY;
                ddciInfo.ORDER_DT = rpdmodeldata.ORDER_DT;
                ddciInfo.PRINT = rpdmodeldata.PRINT;
                ddciInfo.ALLOT_PART_NO = rpdmodeldata.ALLOT_PART_NO;
                ddciInfo.PART_NO_REQ_DATE = rpdmodeldata.PART_NO_REQ_DATE;
                ddciInfo.CUST_STD_NO = rpdmodeldata.CUST_STD_NO;

                //if (rpdmodeldata.CUST_STD_DATE_NEW.ToString().Trim() == "")
                //{
                //    ddciInfo.CUST_STD_DATE = Convert.ToDateTime(rpdmodeldata.CUST_STD_DATE_NEW.ToString());
                //    //RPDModelData.CUST_STD_DATE;
                //}
                ddciInfo.CUST_STD_DATE = rpdmodeldata.CUST_STD_DATE;


                ddciInfo.AUTOPART = rpdmodeldata.AUTOPART;
                ddciInfo.SAFTYPART = rpdmodeldata.SAFTYPART;
                ddciInfo.APPLICATION = rpdmodeldata.APPLICATION;
                ddciInfo.STATUS = rpdmodeldata.STATUS;
                ddciInfo.CUSTOMER_NEED_DT = rpdmodeldata.CUSTOMER_NEED_DT;
                ddciInfo.MKTG_COMMITED_DT = rpdmodeldata.MKTG_COMMITED_DT;
                ddciInfo.PPAP_LEVEL = rpdmodeldata.PPAP_LEVEL;
                ddciInfo.DEVL_METHOD = rpdmodeldata.DEVL_METHOD;
                ddciInfo.PPAP_FORGING = rpdmodeldata.PPAP_FORGING.ToDecimalValue();
                ddciInfo.PPAP_SAMPLE = rpdmodeldata.PPAP_SAMPLE.ToDecimalValue();
                ddciInfo.PACKING = rpdmodeldata.PACKING;
                ddciInfo.NATURE_PACKING = rpdmodeldata.NATURE_PACKING;
                ddciInfo.SPL_CHAR = rpdmodeldata.SPL_CHAR;
                ddciInfo.OTHER_CUST_REQ = rpdmodeldata.OTHER_CUST_REQ;
                ddciInfo.ATP_DATE = rpdmodeldata.ATP_DATE;
                ddciInfo.SIMILAR_PART_NO = rpdmodeldata.SIMILAR_PART_NO;
                ddciInfo.GENERAL_REMARKS = rpdmodeldata.GENERAL_REMARKS;
                ddciInfo.MONTHLY = rpdmodeldata.MONTHLY.ToDecimalValue();
                ddciInfo.MKTG_COMMITED_DATE = rpdmodeldata.MKTG_COMMITED_DATE;
                ddciInfo.DELETE_FLAG = false;
                ddciInfo.UPDATED_DATE = DateTime.Now;
                ddciInfo.UPDATED_BY = userInformation.UserName;
                ddciInfo.COATING_CODE = rpdmodeldata.COATING_CODE;
                ddciInfo.REALISATION = rpdmodeldata.REALISATION;
                ddciInfo.NO_OF_PCS = rpdmodeldata.NO_OF_PCS;

                DB.SubmitChanges();

                SaveGridData(rpdmodeldata);

                //  return SaveGridData(RPDModelData.DeepCopy<RPDModel>());

                // ChangeSet cs = DB.GetChangeSet();
                //  return  cs.Updates.Count > 0 ? true : false;
            }
            else if (ddciInfo == null)
            {
                ddciInfo = new DDCI_INFO();
                ddciInfo.CI_REFERENCE = rpdmodeldata.CI_REFERENCE;
                ddciInfo.ENQU_RECD_ON = rpdmodeldata.ENQU_RECD_ON;
                ddciInfo.FR_CS_DATE = rpdmodeldata.FR_CS_DATE;
                ddciInfo.PROD_DESC = rpdmodeldata.PROD_DESC;
                ddciInfo.CUST_CODE = rpdmodeldata.CUST_CODE;
                ddciInfo.CUST_DWG_NO = rpdmodeldata.CUST_DWG_NO;
                ddciInfo.CUST_DWG_NO_ISSUE = rpdmodeldata.CUST_DWG_NO_ISSUE;
                ddciInfo.EXPORT = rpdmodeldata.EXPORT;
                ddciInfo.NUMBER_OFF = rpdmodeldata.NUMBER_OFF;
                ddciInfo.POTENTIAL = rpdmodeldata.POTENTIAL.ToDecimalValue();
                ddciInfo.SFL_SHARE = rpdmodeldata.SFL_SHARE;
                ddciInfo.REMARKS = rpdmodeldata.REMARKS;
                ddciInfo.RESPONSIBILITY = rpdmodeldata.RESPONSIBILITY;
                ddciInfo.PENDING = rpdmodeldata.PENDING;
                ddciInfo.FEASIBILITY = rpdmodeldata.FEASIBILITY;
                ddciInfo.REJECT_REASON = rpdmodeldata.REJECT_REASON;
                ddciInfo.LOC_CODE = rpdmodeldata.LOC_CODE;
                ddciInfo.CHEESE_WT = rpdmodeldata.CHEESE_WT;
                ddciInfo.FINISH_WT = rpdmodeldata.FINISH_WT;
                //  ddciInfo.FINISH_CODE = RPDModelData.FINISH_CODE;
                ddciInfo.SUGGESTED_RM = rpdmodeldata.SUGGESTED_RM;
                ddciInfo.RM_COST = rpdmodeldata.RM_COST;
                ddciInfo.FINAL_COST = rpdmodeldata.FINAL_COST;
                ddciInfo.COST_NOTES = rpdmodeldata.COST_NOTES;
                ddciInfo.PROCESSED_BY = rpdmodeldata.PROCESSED_BY;
                ddciInfo.ORDER_DT = rpdmodeldata.ORDER_DT;
                ddciInfo.PRINT = rpdmodeldata.PRINT;
                ddciInfo.ALLOT_PART_NO = rpdmodeldata.ALLOT_PART_NO;
                ddciInfo.PART_NO_REQ_DATE = rpdmodeldata.PART_NO_REQ_DATE;
                ddciInfo.CUST_STD_NO = rpdmodeldata.CUST_STD_NO;
                ddciInfo.CUST_STD_DATE = rpdmodeldata.CUST_STD_DATE;
                ddciInfo.AUTOPART = rpdmodeldata.AUTOPART;
                ddciInfo.SAFTYPART = rpdmodeldata.SAFTYPART;
                ddciInfo.APPLICATION = rpdmodeldata.APPLICATION;
                ddciInfo.STATUS = rpdmodeldata.STATUS;
                ddciInfo.CUSTOMER_NEED_DT = rpdmodeldata.CUSTOMER_NEED_DT;
                ddciInfo.MKTG_COMMITED_DT = rpdmodeldata.MKTG_COMMITED_DT;
                ddciInfo.PPAP_LEVEL = rpdmodeldata.PPAP_LEVEL;
                ddciInfo.DEVL_METHOD = rpdmodeldata.DEVL_METHOD;
                ddciInfo.PPAP_FORGING = rpdmodeldata.PPAP_FORGING.ToDecimalValue();
                ddciInfo.PPAP_SAMPLE = rpdmodeldata.PPAP_SAMPLE.ToDecimalValue();
                ddciInfo.PACKING = rpdmodeldata.PACKING;
                ddciInfo.NATURE_PACKING = rpdmodeldata.NATURE_PACKING;
                ddciInfo.SPL_CHAR = rpdmodeldata.SPL_CHAR;
                ddciInfo.OTHER_CUST_REQ = rpdmodeldata.OTHER_CUST_REQ;
                ddciInfo.ATP_DATE = rpdmodeldata.ATP_DATE;
                ddciInfo.SIMILAR_PART_NO = rpdmodeldata.SIMILAR_PART_NO;
                ddciInfo.GENERAL_REMARKS = rpdmodeldata.GENERAL_REMARKS;
                ddciInfo.MONTHLY = rpdmodeldata.MONTHLY.ToDecimalValue();
                ddciInfo.MKTG_COMMITED_DATE = rpdmodeldata.MKTG_COMMITED_DATE;
                ddciInfo.DELETE_FLAG = false;
                ddciInfo.UPDATED_DATE = DateTime.Now;
                ddciInfo.UPDATED_BY = userInformation.UserName;
                ddciInfo.COATING_CODE = rpdmodeldata.COATING_CODE;
                ddciInfo.REALISATION = rpdmodeldata.REALISATION;
                ddciInfo.NO_OF_PCS = rpdmodeldata.NO_OF_PCS;
                ddciInfo.ROWID = Guid.NewGuid();
                ddciInfo.IDPK = GenerateNextNumber("DDCI_INFO", "IDPK").ToIntValue();
                DB.DDCI_INFO.InsertOnSubmit(ddciInfo);
                DB.SubmitChanges();
                ChangeSet cs = DB.GetChangeSet();
                return cs.Inserts.Count > 0 ? true : false;
            }
            return true;

        }

        public bool SaveGridData(RPDModel rpdmodeldata)
        {
            try
            {
                int inc = 1;
                // if (rpdmodeldata.GridData.Table.Rows.Count == 0 || rpdmodeldata.GridData.Table == null)
                // {
                List<DDCI_CHAR> delschedsub = (from o in DB.DDCI_CHAR
                                               where o.CI_REFERENCE == rpdmodeldata.CI_REFERENCE
                                               select o).ToList<DDCI_CHAR>();
                DB.DDCI_CHAR.DeleteAllOnSubmit(delschedsub);
                DB.SubmitChanges();
                //   return true;
                //  }

                foreach (DataRowView dr in rpdmodeldata.GridData)
                {

                    if (rpdmodeldata.CI_REFERENCE.ToString() != "")
                    {
                        if (dr["CHARACTERISTIC"].ToString().Trim() != "" || dr["CUSTOMER_EXP"].ToString().Trim() != "" || Convert.ToString(dr["SEVERITY"].ToString()).Trim() != "")
                        {
                            try
                            {
                                DDCI_CHAR ddci_char = (from o in DB.DDCI_CHAR
                                                       where o.CI_REFERENCE == rpdmodeldata.CI_REFERENCE && o.SLNO == Convert.ToDecimal(dr["SLNO"].ToString())
                                                       select o).SingleOrDefault<DDCI_CHAR>();
                                if (ddci_char == null)
                                {
                                    ddci_char = new DDCI_CHAR();
                                    ddci_char.SLNO = Convert.ToDecimal(inc);

                                    if (dr["SEVERITY"] != DBNull.Value && Convert.ToString(dr["SEVERITY"]).Length > 0)
                                    {
                                        ddci_char.SEVERITY = Convert.ToDecimal(dr["SEVERITY"].ToString());
                                        //ddci_char.SEVERITY = null;
                                    }
                                    else
                                    {
                                        ddci_char.SEVERITY = null;
                                    }

                                    ddci_char.CHARACTERISTIC = dr["CHARACTERISTIC"].ToString();
                                    ddci_char.CUSTOMER_EXP = dr["CUSTOMER_EXP"].ToString();
                                    ddci_char.CI_REFERENCE = rpdmodeldata.CI_REFERENCE;
                                    ddci_char.ROWID = Guid.NewGuid();
                                    DB.DDCI_CHAR.InsertOnSubmit(ddci_char);
                                    DB.SubmitChanges();
                                    ChangeSet cs = DB.GetChangeSet();
                                    inc = inc + 1;
                                    //   return cs.Updates.Count > 0 ? true : false;

                                }
                                else if (ddci_char != null)
                                {
                                    //  ddci_char = new DDCI_CHAR();
                                    //   ddci_char.SLNO = Convert.ToDecimal(dr["SLNO"].ToString());
                                    //ddci_char.SEVERITY = Convert.ToDecimal(dr["SEVERITY"].ToString());
                                    if (dr["SEVERITY"] != DBNull.Value && Convert.ToString(dr["SEVERITY"]).Length > 0)
                                    {
                                        ddci_char.SEVERITY = Convert.ToDecimal(dr["SEVERITY"].ToString());
                                        //ddci_char.SEVERITY = null;
                                    }
                                    else
                                    {
                                        ddci_char.SEVERITY = null;
                                    }
                                    ddci_char.CHARACTERISTIC = dr["CHARACTERISTIC"].ToString();
                                    ddci_char.CUSTOMER_EXP = dr["CUSTOMER_EXP"].ToString();
                                    //  ddci_char.CI_REFERENCE = RPDModelData.CI_REFERENCE;
                                    DB.SubmitChanges();
                                    ChangeSet cs = DB.GetChangeSet();
                                    // return cs.Inserts.Count > 0 ? true : false;
                                }
                            }
                            catch (System.Data.Linq.ChangeConflictException ex)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                // ProductCat.Status = "Record inserted successfully.";
                                return false;
                                throw ex.LogException();
                            }
                            catch (Exception ex)
                            {
                                DB.Transaction.Rollback();
                                throw ex.LogException();
                            }
                        }
                        else if (dr["CHARACTERISTIC"].ToString().Trim() != "")
                        {

                            //List<DDCI_CHAR> delschedsubdel = (from o in DB.DDCI_CHAR
                            //                               where o.CI_REFERENCE == rpdmodeldata.CI_REFERENCE && o.SLNO == Convert.ToDecimal(dr["SLNO"].ToString())
                            //                               select o).ToList<DDCI_CHAR>();
                            //DB.DDCI_CHAR.DeleteAllOnSubmit(delschedsubdel);
                            //DB.SubmitChanges();
                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                throw (ex.LogException());
            }
        }


        //  GetCirRefFromPartNo
        public string GetCirRefFromPartNo(string part_No)
        {
            try
            {
                // string ci_ref;
                PRD_CIREF ci_ref = (from c in DB.PRD_CIREF
                                    where c.PART_NO == part_No && c.CURRENT_CIREF == true
                                    orderby c.PART_NO ascending
                                    select c).FirstOrDefault<PRD_CIREF>();

                //ci_ref = ((from c in DB.PRD_CIREF
                //           where c.PART_NO == part_No
                //           orderby c.PART_NO ascending
                //           select c.CI_REF).FirstOrDefault<PRD_CIREF>());
                if (ci_ref != null)
                {
                    if (string.IsNullOrEmpty(ci_ref.CI_REF) == false)
                    {
                        return ci_ref.CI_REF;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public int GetCirRefIDPK(string ci_reference)
        {
            try
            {
                //int ci_idpk;

                DDCI_INFO ci_idpk = ((from c in DB.DDCI_INFO
                                      where c.CI_REFERENCE == ci_reference
                                      orderby c.CI_REFERENCE ascending
                                      select c).FirstOrDefault<DDCI_INFO>());
                if (ci_idpk != null)
                {
                    if (string.IsNullOrEmpty(ci_idpk.IDPK.ToString()) == false)
                    {
                        return ci_idpk.IDPK;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        //   CI_REFERENCE
    }

}




