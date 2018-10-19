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
    public class CostSheetBll : Essential
    {
        public CostSheetBll(UserInformation userinfo)
        {
            this.userInformation = userinfo;
        }


        public bool GetProductMaster(CostSheetModel costSheet)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                if (dt != null)
                {

                    costSheet.DVProductMaster = dt.DefaultView;
                }
                else
                {
                    costSheet.DVProductMaster = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetProcessMain(CostSheetModel costSheet, int currentproc = 0)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable((from o in DB.PROCESS_MAIN
                                  where o.PART_NO == costSheet.PART_NO && o.CURRENT_PROC == ((currentproc > 0) ? currentproc : o.CURRENT_PROC) && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  orderby o.ROUTE_NO
                                  select new { o.ROUTE_NO }).ToList());
                if (dt != null)
                {

                    costSheet.DVProcessMain = dt.DefaultView;
                }
                else
                {
                    costSheet.DVProcessMain = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public bool GetDropDownSource(CostSheetModel costSheet)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.DDRM_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  orderby o.RM_CODE
                                  select new { o.RM_CODE, o.RM_DESC }).ToList());

                if (dt != null)
                {
                    costSheet.DVRMBasic = dt.Copy().DefaultView;
                }
                else
                {
                    costSheet.DVRMBasic = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetCostSheetDetails(CostSheetModel costSheet)
        {
            try
            {
                DataTable dt = new DataTable();

                PROCESS_MAIN pm = (from o in DB.PROCESS_MAIN
                                   where o.PART_NO == costSheet.PART_NO && o.ROUTE_NO == costSheet.ROUTE_NO && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                   select o).FirstOrDefault<PROCESS_MAIN>();

                if (pm != null)
                {
                    costSheet.WIRE_ROD_CD = pm.WIRE_ROD_CD;
                }
                else
                {
                    costSheet.WIRE_ROD_CD = "";
                    costSheet.RMCOST = null;
                }
                pm = null;


                var q = (from a in DB.PRD_MAST
                         join b in DB.PROCESS_MAIN on a.PART_NO equals b.PART_NO
                         join c in DB.PRD_CIREF on a.PART_NO equals c.PART_NO
                         join d in DB.DDCI_INFO on c.CI_REF equals d.CI_REFERENCE
                         join e in DB.DDCUST_MAST on d.CUST_CODE equals e.CUST_CODE
                         join f in DB.DDFINISH_MAST on d.FINISH_CODE equals f.FINISH_CODE
                         where a.PART_NO == costSheet.PART_NO && b.ROUTE_NO == costSheet.ROUTE_NO && b.CURRENT_PROC == 1 && c.CURRENT_CIREF == true
                         select new
                         {
                             a.PART_DESC,
                             a.FINISH_WT,
                             a.BIF_PROJ,
                             CUSTOMER = (d.CUST_CODE + "-" + e.CUST_NAME),
                             CUST = e.CUST_CODE.ToString().Substring(0, 1),
                             b.CHEESE_WT,
                             FINISH_DESC = (d.FINISH_CODE + '-' + f.FINISH_DESC)
                         }).FirstOrDefault();

                if (q != null)
                {                    
                    costSheet.FINISH_WT = q.FINISH_WT;
                    costSheet.LOCATION = (q.BIF_PROJ.IsNotNullOrEmpty()) ? q.BIF_PROJ : "0";
                    costSheet.CUSTOMER = q.CUSTOMER;
                    costSheet.CUSTCODE = q.CUST;
                    costSheet.CHEESE_WT = q.CHEESE_WT;
                    costSheet.FINISH_DESC = q.FINISH_DESC;
                }
                else
                {                   
                    costSheet.FINISH_WT = null;
                    costSheet.LOCATION = "0";
                    costSheet.CUSTOMER = "";
                    costSheet.CUSTCODE = "";
                    costSheet.CHEESE_WT = null;
                    costSheet.FINISH_DESC = "";
                }

                if (!costSheet.ExportIsClicked)
                {
                    if (costSheet.CUSTCODE.IsNotNullOrEmpty() && costSheet.CUSTCODE == "8")
                    {
                        costSheet.EXPORT = true;
                    }
                    else
                    {
                        costSheet.EXPORT = false;
                    }
                }

                costSheet.DVCostSheet = ToDataTableWithType((from a in DB.PROCESS_SHEET
                                                             join b in DB.PROCESS_CC on new { a.PART_NO, a.ROUTE_NO, a.SEQ_NO } equals new { b.PART_NO, b.ROUTE_NO, b.SEQ_NO }
                                                             join c in DB.PROCESS_MAIN on a.PART_NO equals c.PART_NO
                                                             where a.PART_NO == costSheet.PART_NO && b.ROUTE_NO == costSheet.ROUTE_NO && c.CURRENT_PROC == 1 && b.CC_SNO == 1
                                                             select new
                                                             {
                                                                 a.SEQ_NO,
                                                                 a.OPN_CD,
                                                                 a.OPN_DESC,
                                                                 b.CC_CODE,
                                                                 b.OUTPUT,
                                                                 VAR_COST = 0.0,
                                                                 FIX_COST = 0.0,
                                                                 SPL_COST = 0.0,
                                                                 OPN_COST = 0.0
                                                             }).ToList()).DefaultView;


                if (costSheet.DVCostSheet != null)
                {
                    foreach (DataRowView cs in costSheet.DVCostSheet)
                    {
                        cs.BeginEdit();
                        switch (cs["OPN_CD"].ToString().ToIntValue())
                        {
                            case 1000:
                                var innerQuery = (from o in DB.DDRM_PROCESS
                                                  where o.LOC_CODE == costSheet.LOCATION && o.RM_CODE == costSheet.WIRE_ROD_CD
                                                  select o.COST_CENT_CODE).Distinct();

                                DDCC_OPER cco = (from o in DB.DDCC_OPER
                                                 where innerQuery.Contains(o.COST_CENT_CODE) && o.OPN_CODE == 1000
                                                 select o).FirstOrDefault<DDCC_OPER>();

                                CostSheetGridUpdate(cs, cco, costSheet.CHEESE_WT.ToValueAsString().ToDoubleValue(), costSheet.FINISH_WT.ToValueAsString().ToDoubleValue());                                
                                break;
                            case 2600:
                                DDCC_OPER cco1 = new DDCC_OPER();
                                switch (costSheet.LOCATION)
                                {
                                    case "MM":
                                        if (costSheet.EXPORT)
                                        {
                                            cco1 = (from o in DB.DDCC_OPER
                                                    where o.OPN_CODE == 2600 && o.COST_CENT_CODE == "EXP PKG"
                                                    select o).FirstOrDefault<DDCC_OPER>();
                                        }
                                        else
                                        {
                                            cco1 = (from o in DB.DDCC_OPER
                                                    where o.OPN_CODE == 2600 && o.COST_CENT_CODE == "OE PKG"
                                                    select o).FirstOrDefault<DDCC_OPER>();
                                        }
                                        break;
                                    case "KK":
                                        if (costSheet.EXPORT)
                                        {
                                            cco1 = (from o in DB.DDCC_OPER
                                                    where o.OPN_CODE == 2600 && o.COST_CENT_CODE == "EXP PKG - K"
                                                    select o).FirstOrDefault<DDCC_OPER>();
                                        }
                                        else
                                        {
                                            cco1 = (from o in DB.DDCC_OPER
                                                    where o.OPN_CODE == 2600 && o.COST_CENT_CODE == "OE PKG - K"
                                                    select o).FirstOrDefault<DDCC_OPER>();
                                        }
                                        break;
                                }
                                CostSheetGridUpdate(cs, cco1, costSheet.CHEESE_WT.ToValueAsString().ToDoubleValue(), costSheet.FINISH_WT.ToValueAsString().ToDoubleValue());                                
                                break;
                            default:
                                DDCC_OPER cco2 = (from o in DB.DDCC_OPER
                                                  where o.OPN_CODE == cs["OPN_CD"].ToString().ToDecimalValue() && o.COST_CENT_CODE == cs["CC_CODE"].ToString()
                                                  select o).FirstOrDefault<DDCC_OPER>();

                                CostSheetGridUpdate(cs, cco2, costSheet.CHEESE_WT.ToValueAsString().ToDoubleValue(), costSheet.FINISH_WT.ToValueAsString().ToDoubleValue());                                
                                break;
                        }
                        cs.EndEdit();
                    }
                                        
                    costSheet.COST = costSheet.DVCostSheet.Table.Compute("SUM(OPN_COST)", "").ToString().ToDecimalValue();
                }
                else
                {
                    costSheet.DVCostSheet = null;
                    costSheet.COST = 0;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void CostSheetGridUpdate(DataRowView cs, DDCC_OPER cco, double cheeseWt, double finishWt)
        {
            double vco = 0, fco = 0;
            if (cco != null)
            {
                switch (cco.UNIT_CODE)
                {
                    case "1":
                        if (cs["OUTPUT"].ToString().ToDoubleValue() != 0)
                        {
                            vco = cco.VAR_COST.ToValueAsString().ToDoubleValue() * 100 / cs["OUTPUT"].ToString().ToDoubleValue();
                            fco = cco.FIX_COST.ToValueAsString().ToDoubleValue() * 100 / cs["OUTPUT"].ToString().ToDoubleValue();
                        }
                        else
                        {
                            vco = 0;
                            fco = 0;
                        }
                        break;
                    case "2":
                        vco = cco.VAR_COST.ToValueAsString().ToDoubleValue() * 100;
                        fco = cco.FIX_COST.ToValueAsString().ToDoubleValue() * 100;
                        break;
                    case "3":
                        vco = cco.VAR_COST.ToValueAsString().ToDoubleValue() * finishWt;
                        fco = cco.FIX_COST.ToValueAsString().ToDoubleValue() * finishWt;
                        break;
                    case "4":
                        vco = cco.VAR_COST.ToValueAsString().ToDoubleValue() * cheeseWt * 1.05;
                        fco = cco.FIX_COST.ToValueAsString().ToDoubleValue() * cheeseWt * 1.05;
                        break;
                    default:
                        vco = 0;
                        fco = 0;
                        break;
                }
            }

            cs["VAR_COST"] = vco;
            cs["FIX_COST"] = fco;
            cs["OPN_COST"] = vco + fco;
        }

    }
}
