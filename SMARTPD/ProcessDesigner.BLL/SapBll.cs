using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using ProcessDesigner.Common;
using ProcessDesigner.Model;


namespace ProcessDesigner.BLL
{
    public class SapBll : Essential
    {
        public SapBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public bool GetPartNoDetails(SapModel sapModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                if (dt != null)
                {
                    sapModel.PartnoDetails = dt.DefaultView;
                }
                else
                {
                    sapModel.PartnoDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetRohNoDetails(SapModel sapModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.ROH_SAP_MASTER
                                  //where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.ROH }).ToList());
                if (dt != null)
                {
                    sapModel.RohDetails = dt.DefaultView;
                }
                else
                {
                    sapModel.RohDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool IsProcessSheetAvailableDetails(SapModel sapModel)
        {
            try
            {
                DataTable dt = new DataTable();
                string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,A.GROSS_WEIGHT,G.UNIT_OF_MEASURE,G.SAP_NO,G.SPECIAL_PROCUREMENT,G.SHORT_TEXT,A.NET_WEIGHT,"
               + "B.BIF_PROJ,C.RM_CD,D.RM_DESC,A.OPN_DESC,A.OPN_CD,E.CC_CODE,F.MODULE, E.WIRE_SIZE_MIN,F.SAP_CCCODE,F.SAP_BASE_QUANTITY,F.TYPE_NUT_BOLT"
               + " FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G "
               + " WHERE A.PART_NO= '" + sapModel.PartNo + "' AND E.PART_NO = '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO = '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE COLLATE DATABASE_DEFAULT =E.CC_CODE COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetBomMasterDetails(SapModel sapModel, ref string statusMsg, ref string[] arr)
        {
            DataTable dt = new DataTable();
            string str = "01";
            string sql = "";
            string strLocation = "";
            string[] bOMMaster = new string[] { "MATNR", "WERKS", "DATUV", "BMENG", "IDNRK_01", "MENGE_01", "MEINS_01" };
            arr = bOMMaster;
            DataTable tempBomTable = new DataTable();
            tempBomTable.Columns.Add("Material_Number", typeof(string));
            tempBomTable.Columns.Add("Plant", typeof(string));
            tempBomTable.Columns.Add("Key_Date", typeof(string));
            tempBomTable.Columns.Add("Base_Quantity", typeof(string));
            tempBomTable.Columns.Add("BOM_Component", typeof(string));
            tempBomTable.Columns.Add("Component_Quantity", typeof(string));
            tempBomTable.Columns.Add("Component_Unit_Of_Measure", typeof(string));
            tempBomTable.Columns.Add("SeqNo", typeof(string));
            tempBomTable.Columns.Add("Input", typeof(string));
            try
            {
                string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,A.GROSS_WEIGHT,G.UNIT_OF_MEASURE,G.SAP_NO,G.SPECIAL_PROCUREMENT,G.SHORT_TEXT,A.NET_WEIGHT,"
                + "B.BIF_PROJ,C.RM_CD,D.RM_DESC,A.OPN_DESC,A.OPN_CD,E.CC_CODE,F.MODULE, E.WIRE_SIZE_MIN,F.SAP_CCCODE,F.SAP_BASE_QUANTITY,F.TYPE_NUT_BOLT"
                + " FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G "
                + " WHERE A.PART_NO= '" + sapModel.PartNo + "' AND E.PART_NO = '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO = '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE COLLATE DATABASE_DEFAULT =E.CC_CODE COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        statusMsg = PDMsg.NoRecordFound;
                        return false;
                    }
                    else
                    {
                        sapModel.ProcessSheetDetails = dt.DefaultView;
                        sapModel.FinishPosition = GetFinishPosition(sapModel);
                        sapModel.NoOfOperations = GetNoofOperations(sapModel);
                        if (sapModel.ProcessSheetDetails.Count > 0)
                        {
                            sapModel.WireSize = string.Join("", sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToString().Where(char.IsDigit)).ToIntValue();
                            sapModel.Location = sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString();
                            DDLOC_MAST ddLocMaster = (from c in DB.DDLOC_MAST
                                                      // where c.LOC_CODE == sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim()
                                                      where c.LOC_CODE == sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString()
                                                      select c).FirstOrDefault<DDLOC_MAST>();
                            sapModel.Plant = ddLocMaster.Plant;
                            if (sapModel.Plant == 1000)
                            {
                                strLocation = "1";
                            }
                            else if (sapModel.Plant == 2000)
                            {
                                strLocation = "2";
                            }
                            else if (sapModel.Plant == 3000)
                            {
                                strLocation = "0";
                            }
                            sapModel.BomDetails = tempBomTable.DefaultView;
                            for (int i = 0; i < sapModel.ProcessSheetDetails.Count; i++)
                            {
                                str = (i + 1).ToString("00");
                                //tempBomTable.Rows[i]["MATNR"]

                                if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "25")
                                {
                                    sql = "RM2" + sapModel.ProcessSheetDetails[i]["RM_CD"].ToString().Substring(1) + string.Format("{0:0000}", sapModel.WireSize) + "\t" + sapModel.Plant;
                                }
                                else
                                {
                                    sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + str + strLocation + "\t" + sapModel.Plant;
                                }

                                if (sapModel.FinishPosition != -1)
                                    if ((sapModel.FinishPosition < Convert.ToInt16(str)) && (sapModel.FinishPosition != 0))
                                    {
                                        sql = "";
                                        sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "-" + str + strLocation + "\t" + sapModel.Plant;
                                    }
                                if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "45")
                                {
                                    sql = "";
                                    sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "\t" + sapModel.Plant;

                                }
                                sql = sql + "\t" + DateTime.Now.Date.ToString("dd.MM.yyyy") + "\t";
                                if (Convert.ToInt16(str) == 1)
                                {
                                    sql = sql + "1" + "\t" + sapModel.RohNo + "\t" + "1" + "\t" + "KG";

                                }
                                else
                                {
                                    string strTemp = "";
                                    if (sapModel.ProcessSheetDetails[i]["UNIT_OF_MEASURE"].ToString() == "PC")
                                    {
                                        sql = sql + "100" + "\t";
                                    }
                                    else
                                    {
                                        sql = sql + sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"].ToString() + "\t";
                                    }

                                    if (sapModel.BomDetails.Count > 0)
                                    {
                                        if (sapModel.BomDetails[i - 1][0].IsNotNullOrEmpty())
                                        {
                                            strTemp = sapModel.BomDetails[i - 1][0].ToString();
                                        }
                                        else
                                        {
                                            strTemp = "";
                                        }
                                    }
                                    else
                                    {
                                        strTemp = "";
                                    }
                                    sql = sql + strTemp + "\t" + sapModel.ProcessSheetDetails[i]["UNIT_OF_MEASURE"].ToString() + "\t";

                                }
                                sql = sql + "\t" + sapModel.ProcessSheetDetails[i]["SEQ_NO"].ToString();
                                char[] delimiters = new char[] { '\t' };
                                string[] result = sql.Split(delimiters);
                                sapModel.BomDetails.AddNew();
                                for (int j = 0; j < result.Length; j++)
                                {
                                    sapModel.BomDetails[i][j] = result[j];
                                }
                                Console.WriteLine(result.Count().ToString());
                                if (sapModel.BomDetails != null)
                                {

                                }
                                if (Convert.ToInt16(str) == 1 && sapModel.BomDetails[i][0].ToString().Substring(0, 2) == "RM")
                                {
                                    sapModel.BomDetails[i][6] = "KG";
                                    sapModel.BomDetails[i][5] = "1";
                                    sapModel.BomDetails[i][3] = "1";

                                }


                            }
                            for (int i = 1; i < sapModel.BomDetails.Count; i++)
                            {
                                sapModel.BomDetails[i][6] = sapModel.ProcessSheetDetails[i - 1]["UNIT_OF_MEASURE"].ToString();

                            }
                            if (sapModel.ProcessSheetDetails[0]["SAP_NO"].ToString() == "25")
                            {
                                //rsProcessSheet.MoveNext
                            }
                            for (int i = 1; i < sapModel.BomDetails.Count; i++)
                            {
                                if (sapModel.BomDetails[i][6].ToString() == "PC")
                                {
                                    sapModel.BomDetails[i][5] = "100";
                                }
                                else
                                {
                                    sapModel.BomDetails[i][5] = ((!sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"].IsNotNullOrEmpty()) ? "" : sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"]);
                                }
                            }

                        }
                    }
                }
                else
                {
                    sapModel.RohDetails = null;
                    statusMsg = PDMsg.NoRecordFound;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetFertMasterDetails(SapModel sapModel, ref string statusMsg, ref string[] arr)
        {

            DataTable dt = new DataTable();

            string str = "01";
            string sql = "", division = "";
            string strLocation = "";
            int countSub = 0;
            string plant = "", profitCenter = "", strModule = "", strModule1 = "", strModule2 = "", procurement_type = "", spe_procur_type = "", spe_procur_Cost = "", production_Sheduler = "";
            bool isnut_bolt, subc = true;
            DataTable tempFertMTable = new DataTable();
            DataTable tempFertKTable = new DataTable();
            DataTable tempFertYTable = new DataTable();

            try
            {
                string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,A.GROSS_WEIGHT,G.UNIT_OF_MEASURE,G.SAP_NO,G.SPECIAL_PROCUREMENT,G.SHORT_TEXT,A.NET_WEIGHT,"
                + "B.BIF_PROJ,C.RM_CD,D.RM_DESC,A.OPN_DESC,A.OPN_CD,E.CC_CODE,F.MODULE, E.WIRE_SIZE_MIN,F.SAP_CCCODE,F.SAP_BASE_QUANTITY,F.TYPE_NUT_BOLT"
                + " FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G "
                + " WHERE A.PART_NO= '" + sapModel.PartNo + "' AND E.PART_NO = '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO = '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE COLLATE DATABASE_DEFAULT =E.CC_CODE COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        statusMsg = PDMsg.NoRecordFound;
                        return false;
                    }
                    else
                    {
                        sapModel.ProcessSheetDetails = dt.DefaultView;
                        sapModel.FinishPosition = GetFinishPosition(sapModel);
                        // sapModel.NoOfOperations = Convert.ToInt32(GetNoofOperations(sapModel)) - 1;
                        subc = false;
                        isnut_bolt = false;
                        if (sapModel.ProcessSheetDetails.Count > 0)
                        {
                            sapModel.WireSize = string.Join("", sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToString().Where(char.IsDigit)).ToIntValue();
                            sapModel.Location = sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString();
                            DDLOC_MAST ddLocMaster = (from c in DB.DDLOC_MAST
                                                      // where c.LOC_CODE == sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim()
                                                      where c.LOC_CODE == sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString()
                                                      select c).FirstOrDefault<DDLOC_MAST>();
                            sapModel.Plant = ddLocMaster.Plant;
                            if (sapModel.Plant == 1000)
                            {
                                strLocation = "1";
                            }
                            else if (sapModel.Plant == 2000)
                            {
                                strLocation = "2";
                            }
                            else if (sapModel.Plant == 3000)
                            {
                                strLocation = "0";
                            }

                            for (int i = 0; i < sapModel.ProcessSheetDetails.Count; i++)
                            {
                                str = (i + 1).ToString("00");

                                switch (sapModel.ProcessSheetDetails[i]["CC_CODE"].ToString().ToString())
                                {
                                    case "010801":
                                    case "199 K":
                                    case "SS":
                                    case "010301":
                                    case "199 Y":
                                    case "199":
                                    case "009999":
                                        strModule = "SUB";
                                        procurement_type = "F";
                                        spe_procur_type = "30";
                                        spe_procur_Cost = "";
                                        production_Sheduler = "SPL";
                                        countSub = countSub + 1;
                                        subc = true;
                                        break;
                                    default:
                                        break;
                                }

                                switch (sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim())
                                {
                                    case "M":
                                        strLocation = "1";
                                        plant = "10";
                                        if (tempFertMTable.Columns.Count == 0)
                                        {
                                            tempFertMTable.Columns.Add("Material_Number", typeof(string));
                                            tempFertMTable.Columns.Add("Material_Description", typeof(string));
                                            tempFertMTable.Columns.Add("Division", typeof(string));
                                            tempFertMTable.Columns.Add("Gross_weight", typeof(string));
                                            tempFertMTable.Columns.Add("Net_weight", typeof(string));
                                            tempFertMTable.Columns.Add("Profit_Center", typeof(string));
                                            tempFertMTable.Columns.Add("MRP_Controller", typeof(string));
                                            tempFertMTable.Columns.Add("Production_scheduler", typeof(string));
                                            tempFertMTable.Columns.Add("Gr_of_Materials_for_tra_matrix", typeof(string));
                                            tempFertMTable.Columns.Add("Price_Control", typeof(string));
                                            tempFertMTable.Columns.Add("Standard_Price", typeof(string));
                                        }
                                        string[] fertMMaster = new string[] { "MATNR", "MAKTX", "SPART", "BRGEW", "NTGEW", "PRCTR", "DISPO", "FEVOR", "MATGR", "VPRSV", "STPRS" };
                                        arr = fertMMaster;
                                        statusMsg = "M";
                                        sapModel.FertMDetails = tempFertMTable.DefaultView;
                                        if (subc == false)
                                        {
                                            if ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Trim().Length > 0) && ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Substring(0, 1).ToString().Trim()) == "M"))
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "/";
                                                spe_procur_Cost = "";
                                            }
                                            else
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "50";
                                                spe_procur_Cost = "S1";
                                            }
                                        }
                                        break;
                                    case "K":
                                        strLocation = "2";
                                        plant = "20";
                                        string[] fertKMaster = new string[] { "MATNR", "MAKTX", "SPART", "BRGEW", "NTGEW", "PRCTR", "DISPO", "FEVOR", "MATGR", "STPRS" };
                                        arr = fertKMaster;
                                        if (tempFertKTable.Columns.Count == 0)
                                        {
                                            tempFertKTable.Columns.Add("Material_Number", typeof(string));
                                            tempFertKTable.Columns.Add("Material_Description", typeof(string));
                                            tempFertKTable.Columns.Add("Division", typeof(string));
                                            tempFertKTable.Columns.Add("Gross_weight", typeof(string));
                                            tempFertKTable.Columns.Add("Net_weight", typeof(string));
                                            tempFertKTable.Columns.Add("Profit_Center", typeof(string));
                                            tempFertKTable.Columns.Add("MRP_Controller", typeof(string));
                                            tempFertKTable.Columns.Add("Production_scheduler", typeof(string));
                                            tempFertKTable.Columns.Add("Gr_of_Materials_for_tra_matrix", typeof(string));
                                            tempFertKTable.Columns.Add("Standard_Price", typeof(string));
                                        }

                                        statusMsg = "K";
                                        sapModel.FertKDetails = tempFertKTable.DefaultView;
                                        if (subc == false)
                                        {
                                            if (sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Trim().Length > 0 && ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Substring(1, 1).ToString().Trim()) == "K"))
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "/";
                                                spe_procur_Cost = "";
                                            }
                                            else
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "50";
                                                spe_procur_Cost = "S1";
                                            }
                                        }
                                        break;
                                    case "Y":
                                        strLocation = "0";
                                        plant = "30";

                                        string[] fertYMaster = new string[] { "MATNR", "MAKTX", "SPART", "BRGEW", "NTGEW", "PRCTR", "DISMM", "DISPO", "FEVOR", "MATGR", "VPRSV" };
                                        arr = fertYMaster;
                                        if (tempFertYTable.Columns.Count == 0)
                                        {
                                            tempFertYTable.Columns.Add("Material_Number", typeof(string));
                                            tempFertYTable.Columns.Add("Material_Description", typeof(string));
                                            tempFertYTable.Columns.Add("Division", typeof(string));
                                            tempFertYTable.Columns.Add("Gross_weight", typeof(string));
                                            tempFertYTable.Columns.Add("Net_weight", typeof(string));
                                            tempFertYTable.Columns.Add("Profit_Center", typeof(string));
                                            tempFertYTable.Columns.Add("MRP_type", typeof(string));
                                            tempFertYTable.Columns.Add("MRP_Controller", typeof(string));
                                            tempFertYTable.Columns.Add("Production_scheduler", typeof(string));
                                            tempFertYTable.Columns.Add("Gr_of_Materials_for_tra_matrix", typeof(string));
                                            tempFertYTable.Columns.Add("Price_Control", typeof(string));
                                        }

                                        statusMsg = "Y";
                                        sapModel.FertYDetails = tempFertYTable.DefaultView;
                                        if (subc == false)
                                        {
                                            if (sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Trim().Length > 0 && ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Substring(2, 1).ToString().Trim()) == "Y"))
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "/";
                                                spe_procur_Cost = "";
                                            }
                                            else
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "50"; spe_procur_Cost = "S1";
                                            }
                                        }
                                        break;
                                    default:
                                        fertKMaster = new string[] { "MATNR", "MAKTX", "SPART", "BRGEW", "NTGEW", "PRCTR", "DISPO", "FEVOR", "MATGR", "STPRS" };
                                        arr = fertKMaster;
                                        if (tempFertKTable.Columns.Count == 0)
                                        {
                                            tempFertKTable.Columns.Add("Material_Number", typeof(string));
                                            tempFertKTable.Columns.Add("Material_Description", typeof(string));
                                            tempFertKTable.Columns.Add("Division", typeof(string));
                                            tempFertKTable.Columns.Add("Gross_weight", typeof(string));
                                            tempFertKTable.Columns.Add("Net_weight", typeof(string));
                                            tempFertKTable.Columns.Add("Profit_Center", typeof(string));
                                            tempFertKTable.Columns.Add("MRP_Controller", typeof(string));
                                            tempFertKTable.Columns.Add("Production_scheduler", typeof(string));
                                            tempFertKTable.Columns.Add("Gr_of_Materials_for_tra_matrix", typeof(string));
                                            tempFertKTable.Columns.Add("Standard_Price", typeof(string));
                                        }

                                        statusMsg = "K-O";
                                        sapModel.FertKDetails = tempFertKTable.DefaultView;

                                        break;
                                }

                                switch (sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString())
                                {
                                    case "MN":
                                        division = "10"; strModule = "HNF"; production_Sheduler = "HNF";
                                        break;
                                    case "MM":
                                    case "KK":
                                    case "YX":
                                        division = "15";
                                        switch (strLocation)
                                        {
                                            case "0":
                                                strModule = "YX"; production_Sheduler = "YX";
                                                break;
                                            case "1":
                                                isnut_bolt = true;
                                                strModule = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "";
                                                production_Sheduler = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "";
                                                if (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty())
                                                {
                                                    strModule1 = strModule;
                                                }
                                                break;
                                            case "2":
                                                isnut_bolt = true;
                                                strModule = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "";
                                                production_Sheduler = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "";
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    case "MS":
                                        division = "20";
                                        strModule = "SSU";
                                        production_Sheduler = "SSU";
                                        break;
                                    case "YS":
                                        division = "30";
                                        switch (strLocation)
                                        {
                                            case "0":
                                                strModule = "YS"; production_Sheduler = "YS";
                                                break;
                                            case "2":
                                                strModule = "EXP"; production_Sheduler = "EXP";
                                                break;
                                        }
                                        break;
                                    case "YY":
                                    case "KS":
                                        division = "35";
                                        switch (strLocation)
                                        {
                                            case "0":
                                                strModule = "YY"; production_Sheduler = "YY";
                                                break;
                                            case "2":
                                                strModule = "EXP"; production_Sheduler = "EXP";
                                                break;
                                        }
                                        break;
                                    //case "MD":
                                    //    division = "25";
                                    //    production_Sheduler = "DEL";
                                    //    break;
                                    default:
                                        division = "40";
                                        break;
                                }

                                profitCenter = plant + division + "00";
                                if (i == 1)
                                    strModule2 = strModule;
                                if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "45")
                                {
                                    if (strModule == "")
                                    {
                                        strModule = strModule2;
                                        production_Sheduler = strModule2;
                                    }

                                    switch (strLocation)
                                    {
                                        case "1":
                                            sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "\t";
                                            sql = sql + sapModel.ProcessSheetDetails[i]["PART_DESC"];
                                            sql = sql + "\t" + division + "\t" + String.Format("{0:0.0#}", (Convert.ToDecimal((sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"].IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"] : "0") * 10));
                                            sql = sql + "\t" + String.Format("{0:0.0#}", (Convert.ToDecimal((sapModel.ProcessSheetDetails[i]["NET_WEIGHT"].IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["NET_WEIGHT"] : "0") * 10)) + "\t" + profitCenter;
                                            if (sapModel.Location == "MM")
                                            {
                                                sql = sql + "\t" + strModule1 + "\t" + strModule1 + "\t" + sapModel.ProcessSheetDetails[i]["SAP_CCCODE"];
                                            }
                                            else
                                            {
                                                sql = sql + "\t" + strModule + "\t" + production_Sheduler + "\t" + sapModel.ProcessSheetDetails[i]["SAP_CCCODE"];

                                            }
                                            sql = sql + "\t" + "S" + "\t" + "1";
                                            break;
                                        case "2":
                                            sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "\t";
                                            sql = sql + sapModel.ProcessSheetDetails[i]["PART_DESC"];
                                            sql = sql + "\t" + division + "\t" + String.Format("{0:0.0#}", (Convert.ToDecimal((sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"].IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"] : "0") * 10));
                                            sql = sql + "\t" + String.Format("{0:0.0#}", (Convert.ToDecimal((sapModel.ProcessSheetDetails[i]["NET_WEIGHT"].IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["NET_WEIGHT"] : "0") * 10)) + "\t" + profitCenter;
                                            sql = sql + "\t" + strModule + "\t" + production_Sheduler + "\t" + sapModel.ProcessSheetDetails[i]["SAP_CCCODE"];
                                            sql = sql + "\t" + "1";
                                            break;
                                        default:
                                            sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "\t";
                                            sql = sql + sapModel.ProcessSheetDetails[i]["PART_DESC"];
                                            sql = sql + "\t" + division + "\t" + String.Format("{0:0.0#}", (Convert.ToDecimal((sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"].IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"] : "0") * 10));
                                            sql = sql + "\t" + String.Format("{0:0.0#}", (Convert.ToDecimal((sapModel.ProcessSheetDetails[i]["NET_WEIGHT"].IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["NET_WEIGHT"] : "0") * 10)) + "\t" + profitCenter;
                                            sql = sql + "\t" + "PD" + "\t" + strModule + "\t" + strModule + "\t" + sapModel.ProcessSheetDetails[i]["SAP_CCCODE"];
                                            sql = sql + "\t" + "1";
                                            break;
                                    }


                                    char[] delimiters = new char[] { '\t' };
                                    string[] result = sql.Split(delimiters);
                                    switch (strLocation)
                                    {
                                        case "1":
                                            sapModel.FertMDetails.AddNew();
                                            for (int j = 0; j < result.Length; j++)
                                            {
                                                sapModel.FertMDetails[sapModel.FertMDetails.Count - 1][j] = result[j];
                                            }
                                            break;
                                        case "2":
                                            sapModel.FertKDetails.AddNew();
                                            for (int j = 0; j < result.Length; j++)
                                            {
                                                sapModel.FertKDetails[sapModel.FertKDetails.Count - 1][j] = result[j];
                                            }
                                            break;
                                        case "0":
                                            sapModel.FertYDetails.AddNew();
                                            for (int j = 0; j < result.Length; j++)
                                            {
                                                sapModel.FertYDetails[sapModel.FertYDetails.Count - 1][j] = result[j];
                                            }
                                            break;
                                        default:
                                            sapModel.FertKDetails.AddNew();
                                            for (int j = 0; j < result.Length; j++)
                                            {
                                                sapModel.FertKDetails[sapModel.FertKDetails.Count - 1][j] = result[j];
                                            }
                                            break;
                                    }

                                    //    switch (sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim())
                                    //    {
                                    //        case "M":
                                    //            sapModel.FertMDetails.AddNew();
                                    //            for (int j = 0; j < result.Length; j++)
                                    //            {
                                    //                sapModel.FertMDetails[sapModel.FertMDetails.Count - 1][j] = result[j];
                                    //            }
                                    //            break;
                                    //        case "K":
                                    //            sapModel.FertKDetails.AddNew();
                                    //            for (int j = 0; j < result.Length; j++)
                                    //            {
                                    //                sapModel.FertKDetails[sapModel.FertKDetails.Count - 1][j] = result[j];
                                    //            }
                                    //            break;
                                    //        case "Y":
                                    //            sapModel.FertYDetails.AddNew();
                                    //            for (int j = 0; j < result.Length; j++)
                                    //            {
                                    //                sapModel.FertYDetails[sapModel.FertYDetails.Count - 1][j] = result[j];
                                    //            }
                                    //            break;
                                    //    }
                                }
                                strLocation = ""; profitCenter = "";
                                subc = false;

                            }


                            if (sapModel.Location == "MM")
                            {
                                if (countSub > 3)
                                {
                                    for (int i = 2; i < sapModel.HalbDetails.Count - 1; i++)
                                    {
                                        switch (sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim())
                                        {
                                            case "M":
                                                sapModel.FertMDetails[i][7] = "SUB";
                                                sapModel.FertMDetails[i][6] = "SUB";
                                                break;
                                            case "K":
                                                sapModel.FertKDetails[i][7] = "SUB";
                                                sapModel.FertKDetails[i][6] = "SUB";
                                                break;
                                            case "Y":
                                                sapModel.FertYDetails[i][7] = "SUB";
                                                sapModel.FertYDetails[i][6] = "SUB";
                                                break;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    sapModel.RohDetails = null;
                    statusMsg = PDMsg.NoRecordFound;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetHalbMasterDetails(SapModel sapModel, ref string statusMsg, ref string[] arr)
        {

            DataTable dt = new DataTable();

            string str = "01";
            string sql = "", division = "";
            string strLocation = "";
            int countSub = 0;
            string plant = "", profitCenter = "", strModule = "", procurement_type = "", spe_procur_type = "", spe_procur_Cost = "", production_Sheduler = "";
            bool isnut_bolt, subc = true;
            string[] halb = new string[] { "MATNR", "MAKTX", "MEINS", "MATKL", "SPART", "BRGEW", "GEWEI", "NTGEW", "DISPO", "BESKZ", "SOBSL", "FEVOR", "MATGR", "STPRS", "PRCTR", "SOBSK" };
            arr = halb;
            DataTable tempBomTable = new DataTable();
            tempBomTable.Columns.Add("Opn_Desc", typeof(string));
            tempBomTable.Columns.Add("Material_Number", typeof(string));
            tempBomTable.Columns.Add("Material_Description", typeof(string));
            tempBomTable.Columns.Add("Base_Unit_of_Measure", typeof(string));
            tempBomTable.Columns.Add("Material_Group", typeof(string));
            tempBomTable.Columns.Add("Division", typeof(string));
            tempBomTable.Columns.Add("Gross_weight", typeof(string));
            tempBomTable.Columns.Add("Weight_Unit", typeof(string));
            tempBomTable.Columns.Add("Net_weight", typeof(string));
            tempBomTable.Columns.Add("MRP_Controller", typeof(string));
            tempBomTable.Columns.Add("Procurement_Type", typeof(string));
            tempBomTable.Columns.Add("Special_Procurement_Type", typeof(string));
            tempBomTable.Columns.Add("Production_scheduler", typeof(string));
            tempBomTable.Columns.Add("Group_Of_Materials_For_Transition_Matrix", typeof(string));
            tempBomTable.Columns.Add("Standard_Price", typeof(string));
            tempBomTable.Columns.Add("Profit_Center", typeof(string));
            tempBomTable.Columns.Add("Spl_Proc_Costing", typeof(string));
            tempBomTable.Columns.Add("SeqNo", typeof(string));
            try
            {
                string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,A.GROSS_WEIGHT,G.UNIT_OF_MEASURE,G.SAP_NO,G.SPECIAL_PROCUREMENT,G.SHORT_TEXT,A.NET_WEIGHT,"
                + "B.BIF_PROJ,C.RM_CD,D.RM_DESC,A.OPN_DESC,A.OPN_CD,E.CC_CODE,F.MODULE, E.WIRE_SIZE_MIN,F.SAP_CCCODE,F.SAP_BASE_QUANTITY,F.TYPE_NUT_BOLT"
                + " FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G "
                + " WHERE A.PART_NO= '" + sapModel.PartNo + "' AND E.PART_NO = '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO = '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE COLLATE DATABASE_DEFAULT =E.CC_CODE COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        statusMsg = PDMsg.NoRecordFound;
                        return false;
                    }
                    else
                    {
                        sapModel.ProcessSheetDetails = dt.DefaultView;
                        sapModel.FinishPosition = GetFinishPosition(sapModel);
                        // sapModel.NoOfOperations = Convert.ToInt32(GetNoofOperations(sapModel)) - 1;
                        subc = false;
                        isnut_bolt = false;
                        if (sapModel.ProcessSheetDetails.Count > 0)
                        {
                            //                            Format(Int(rsProcessSheet.Fields("WIRE_SIZE_MIN")), "00") & Format((rsProcessSheet.Fields("WIRE_SIZE_MIN") - Int(rsProcessSheet.Fields("WIRE_SIZE_MIN"))) * 100, "00")
                            if (sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].IsNotNullOrEmpty())
                                sapModel.WireSize = string.Join("", sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToString().Where(char.IsDigit)).ToIntValue();
                            sapModel.Location = sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString();
                            DDLOC_MAST ddLocMaster = (from c in DB.DDLOC_MAST
                                                      // where c.LOC_CODE == sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim()
                                                      where c.LOC_CODE == sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString()
                                                      select c).FirstOrDefault<DDLOC_MAST>();
                            sapModel.Plant = ddLocMaster.Plant;
                            if (sapModel.Plant == 1000)
                            {
                                strLocation = "1";
                            }
                            else if (sapModel.Plant == 2000)
                            {
                                strLocation = "2";
                            }
                            else if (sapModel.Plant == 3000)
                            {
                                strLocation = "0";
                            }

                            sapModel.HalbDetails = tempBomTable.DefaultView;

                            for (int i = 0; i < sapModel.ProcessSheetDetails.Count; i++)
                            {
                                str = (i + 1).ToString("00");
                                if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "45") goto Loop2;
                                switch (sapModel.ProcessSheetDetails[i]["CC_CODE"].ToString().ToString())
                                {
                                    case "010801":
                                    case "199 K":
                                    case "SS":
                                    case "010301":
                                    case "199 Y":
                                    case "199":
                                    case "009999":
                                        strModule = "SUB";
                                        procurement_type = "F";
                                        spe_procur_type = "30";
                                        spe_procur_Cost = "";
                                        production_Sheduler = "SPL";
                                        countSub = countSub + 1;
                                        subc = true;
                                        break;
                                    default:
                                        break;
                                }

                                switch (sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim())
                                {
                                    case "M":
                                        strLocation = "1";
                                        plant = "10";
                                        if (subc == false)
                                        {
                                            if ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Trim().Length > 0) && ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Substring(0, 1).ToString().Trim()) == "M"))
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "/";
                                                spe_procur_Cost = "";
                                            }
                                            else
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "50";
                                                spe_procur_Cost = "S1";
                                            }
                                        }
                                        break;
                                    case "K":
                                        strLocation = "2";
                                        plant = "20";
                                        if (subc == false)
                                        {
                                            if (sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Trim().Length > 0 && ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Substring(1, 1).ToString().Trim()) == "K"))
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "/";
                                                spe_procur_Cost = "";
                                            }
                                            else
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "50";
                                                spe_procur_Cost = "S1";
                                            }
                                        }
                                        break;
                                    case "Y":
                                        strLocation = "0";
                                        plant = "30";
                                        if (subc == false)
                                        {
                                            if (sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Trim().Length > 0 && ((sapModel.ProcessSheetDetails[i]["SPECIAL_PROCUREMENT"].ToString().Substring(2, 1).ToString().Trim()) == "Y"))
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "/";
                                                spe_procur_Cost = "";
                                            }
                                            else
                                            {
                                                procurement_type = "E";
                                                spe_procur_type = "50"; spe_procur_Cost = "S1";
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                switch (sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString())
                                {
                                    case "MN":
                                        division = "10"; strModule = "HNF"; production_Sheduler = "HNF";
                                        break;
                                    case "MM":
                                    case "KK":
                                    case "YX":
                                        division = "15";
                                        switch (strLocation)
                                        {
                                            case "0":
                                                strModule = "YX"; production_Sheduler = "YX";
                                                break;
                                            case "1":
                                                isnut_bolt = true;
                                                strModule = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "SUB";
                                                production_Sheduler = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "";
                                                break;
                                            case "2":
                                                isnut_bolt = true;
                                                strModule = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "";
                                                production_Sheduler = (sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["TYPE_NUT_BOLT"].ToString().ToUpper() : "";
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    case "MS":
                                        division = "20";
                                        strModule = "SSU";
                                        production_Sheduler = "SSU";
                                        break;
                                    case "YS":
                                        division = "30";
                                        switch (strLocation)
                                        {
                                            case "0":
                                                strModule = "YS"; production_Sheduler = "YS";
                                                break;
                                            case "2":
                                                strModule = "EXP"; production_Sheduler = "EXP";
                                                break;
                                        }
                                        break;
                                    case "YY":
                                    case "KS":
                                        division = "35";
                                        switch (strLocation)
                                        {
                                            case "0":
                                                strModule = "YY"; production_Sheduler = "YY";
                                                break;
                                            case "2":
                                                strModule = "EXP"; production_Sheduler = "EXP";
                                                break;
                                        }
                                        break;
                                    case "MD":
                                        division = "25";
                                        production_Sheduler = "DEL";
                                        break;
                                    default:
                                        division = "40";
                                        break;
                                }
                                profitCenter = plant + division + "00";
                                if (sapModel.ProcessSheetDetails.Count > 0)
                                {
                                    sql = sapModel.ProcessSheetDetails[i]["OPN_DESC"].ToString() + "\t";
                                    if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "25")
                                    {
                                        // string wireSiz = sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToValueAsString();
                                        //wireSiz = String.Format("{0:00}", Convert.ToInt32(wireSiz)) + String.Format("{0:00}", (Convert.ToDecimal(String.Format("{0:00}", wireSiz)) - wireSiz.ToIntValue()) * 100);
                                        //(String.Format("{0:00}", Convert.ToInt32(sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"])) + String.Format("{0:00}", (sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToString().ToDecimalValue() - sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToString().ToIntValue() * 100)))
                                        sql = sql + "RM2" + sapModel.ProcessSheetDetails[i]["RM_CD"].ToString().Substring(1) + ((!sapModel.WireSize.IsNotNullOrEmpty() || (sapModel.WireSize == 0)) ? "0000" : string.Format("{0:0000}", sapModel.WireSize)) + "\t";
                                        sql = sql + sapModel.ProcessSheetDetails[i]["WIRE_SIZE_MIN"].ToString() + " " + "DIA.WIRE [" + " " + sapModel.ProcessSheetDetails[i]["RM_DESC"].ToString() + "]" + "";
                                        production_Sheduler = "RM"; strModule = "RM";
                                    }
                                    else
                                    {

                                        sql = sql + sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + str + strLocation + "\t";
                                        sql = sql + sapModel.ProcessSheetDetails[i]["PART_DESC"];
                                    }

                                }

                                /////////
                                if (sapModel.FinishPosition != -1)
                                    if ((sapModel.FinishPosition < Convert.ToInt16(str)) && (sapModel.FinishPosition != 0))
                                    {
                                        sql = "";
                                        sql = sapModel.ProcessSheetDetails[i]["OPN_DESC"] + "\t" + sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "-" + str + strLocation + "\t" + sapModel.ProcessSheetDetails[i]["PART_DESC"];
                                    }
                                sql = sql + "\t" + sapModel.ProcessSheetDetails[i]["UNIT_OF_MEASURE"] + "\t" + String.Format("{0:00}", (Convert.ToInt32(sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString())));
                                sql = sql + "\t" + division + "\t" + sapModel.ProcessSheetDetails[i]["GROSS_WEIGHT"] + "\t";
                                sql = sql + "KG" + "\t" + sapModel.ProcessSheetDetails[i]["NET_WEIGHT"] + "\t" + strModule + "\t";
                                sql = sql + procurement_type + "\t" + spe_procur_type + "\t" + production_Sheduler + "\t";
                                sql = sql + sapModel.ProcessSheetDetails[i]["SAP_CCCODE"] + "\t" + "1" + "\t" + profitCenter + "\t" + spe_procur_Cost;
                                sql = sql + "\t" + sapModel.ProcessSheetDetails[i]["SEQ_NO"].ToString();

                                char[] delimiters = new char[] { '\t' };
                                string[] result = sql.Split(delimiters);
                                sapModel.HalbDetails.AddNew();
                                for (int j = 0; j < result.Length; j++)
                                {
                                    sapModel.HalbDetails[i][j] = result[j];
                                }

                                strLocation = ""; profitCenter = "";
                                subc = false;

                            }
                        Loop2:
                            if (isnut_bolt == true)
                            {
                                for (int i = 1; i < sapModel.HalbDetails.Count; i++)
                                {
                                    if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() != "25")
                                    {
                                        sapModel.HalbDetails[i][9] = sapModel.HalbDetails[1][9].ToValueAsString().ToUpper();
                                        sapModel.HalbDetails[i][12] = sapModel.HalbDetails[1][9].ToValueAsString().ToUpper();
                                    }
                                }
                            }

                            if (sapModel.Location == "MM" || sapModel.Location == "MD")
                            {
                                if (countSub > 3)
                                {
                                    for (int i = 1; i < sapModel.HalbDetails.Count; i++)
                                    {
                                        sapModel.HalbDetails[i][9] = "SUB";
                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    sapModel.RohDetails = null;
                    statusMsg = PDMsg.NoRecordFound;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetPrdVersionMasterDetails(SapModel sapModel, ref string statusMsg, ref string[] arr)
        {

            DataTable dt = new DataTable();

            string str = "01";
            string sql = "";
            string strLocation = "";
            string plant = "";
            string[] prdVer = new string[] { "WERKS", "MATNR", "VERID", "TEXT1", "BSTMI", "BSTMA", "ADATU", "BDATU", "PLNTY", "PLNNR", "PLNAL", "STLAL", "STLAN", "MDV01", "ELPRO", "ALORT", "SERKZ" };
            arr = prdVer;
            DataTable tempPrdVerTable = new DataTable();
            tempPrdVerTable.Columns.Add("Plant", typeof(string));
            tempPrdVerTable.Columns.Add("Material", typeof(string));
            tempPrdVerTable.Columns.Add("Ver_No", typeof(string));
            tempPrdVerTable.Columns.Add("Text1", typeof(string));
            tempPrdVerTable.Columns.Add("From_Lot", typeof(string));
            tempPrdVerTable.Columns.Add("To_Lot", typeof(string));
            tempPrdVerTable.Columns.Add("Start_Date", typeof(string));
            tempPrdVerTable.Columns.Add("End_Date", typeof(string));
            tempPrdVerTable.Columns.Add("PLNTY", typeof(string));
            tempPrdVerTable.Columns.Add("PLNNR", typeof(string));
            tempPrdVerTable.Columns.Add("PLNAL", typeof(string));
            tempPrdVerTable.Columns.Add("STLAL", typeof(string));
            tempPrdVerTable.Columns.Add("STLAN", typeof(string));
            tempPrdVerTable.Columns.Add("W_Cenre", typeof(string));
            tempPrdVerTable.Columns.Add("From_SL", typeof(string));
            tempPrdVerTable.Columns.Add("To_SL", typeof(string));
            tempPrdVerTable.Columns.Add("SERKZ", typeof(string));
            try
            {
                string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,A.GROSS_WEIGHT,G.UNIT_OF_MEASURE,G.SAP_NO,G.SPECIAL_PROCUREMENT,G.SHORT_TEXT,A.NET_WEIGHT,"
                + "B.BIF_PROJ,C.RM_CD,D.RM_DESC,A.OPN_DESC,A.OPN_CD,E.CC_CODE,F.MODULE, E.WIRE_SIZE_MIN,F.SAP_CCCODE,F.SAP_BASE_QUANTITY,F.TYPE_NUT_BOLT"
                + " FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G "
                + " WHERE A.PART_NO= '" + sapModel.PartNo + "' AND E.PART_NO = '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO = '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE COLLATE DATABASE_DEFAULT =E.CC_CODE COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        statusMsg = PDMsg.NoRecordFound;
                        return false;
                    }
                    else
                    {
                        sapModel.ProcessSheetDetails = dt.DefaultView;
                        sapModel.FinishPosition = GetFinishPosition(sapModel);
                        if (sapModel.ProcessSheetDetails.Count > 0)
                        {
                            sapModel.WireSize = string.Join("", sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToString().Where(char.IsDigit)).ToIntValue();
                            sapModel.Location = sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString();
                            DDLOC_MAST ddLocMaster = (from c in DB.DDLOC_MAST
                                                      where c.LOC_CODE == sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString()
                                                      select c).FirstOrDefault<DDLOC_MAST>();
                            sapModel.Plant = ddLocMaster.Plant;
                            if (sapModel.Plant == 1000)
                            {
                                strLocation = "1";
                            }
                            else if (sapModel.Plant == 2000)
                            {
                                strLocation = "2";
                            }
                            else if (sapModel.Plant == 3000)
                            {
                                strLocation = "0";
                            }

                            sapModel.ProductionVersionDetails = tempPrdVerTable.DefaultView;

                            for (int i = 0; i < sapModel.ProcessSheetDetails.Count; i++)
                            {
                                str = (i + 1).ToString("00");
                                switch (sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString().Substring(0, 1).ToString().Trim())
                                {
                                    case "M":
                                        strLocation = "1";
                                        plant = "1000";
                                        break;
                                    case "K":
                                        strLocation = "2";
                                        plant = "2000";
                                        break;
                                    case "Y":
                                        strLocation = "0";
                                        plant = "3000";
                                        break;
                                    default:
                                        break;
                                }

                                if (sapModel.ProcessSheetDetails.Count > 0)
                                {
                                    sql = "";
                                    if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "25")
                                    {
                                        sql = "RM2" + sapModel.ProcessSheetDetails[i]["RM_CD"].ToString().Substring(1) + string.Format("{0:0000}", sapModel.WireSize) + "\t";

                                    }
                                    else
                                    {
                                        sql = sql + sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + str + strLocation + "\t";
                                    }

                                }

                                if (sapModel.FinishPosition != -1)
                                    if ((sapModel.FinishPosition < Convert.ToInt16(str)) && (sapModel.FinishPosition != 0))
                                    {
                                        sql = "";
                                        sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "-" + str + strLocation + "\t";
                                    }

                                if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "45")
                                {
                                    sql = "";
                                    sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "\t";
                                }

                                sql = plant + "\t" + sql;
                                sql = sql + "0001" + "\t" + "Version1" + "\t" + "1" + "\t" + "99999999" + "\t" + DateTime.Now.Date.ToString("dd.MM.yyyy") + "\t";
                                sql = sql + "31.12.9999" + "\t" + "N" + "\t" + "" + "\t" + "1" + "\t" + "1" + "\t" + "1" + "\t";
                                sql = sql + sapModel.ProcessSheetDetails[i]["SAP_CCCODE"] + "\t" + "0007" + "\t";
                                if (sapModel.ProcessSheetDetails.Count - 1 == i)
                                {
                                    sql = sql + "0900";
                                }
                                else
                                {
                                    sql = sql + "0007";
                                }
                                sql = sql + "\t" + "X";

                                char[] delimiters = new char[] { '\t' };
                                string[] result = sql.Split(delimiters);
                                sapModel.ProductionVersionDetails.AddNew();
                                for (int j = 0; j < result.Length; j++)
                                {
                                    sapModel.ProductionVersionDetails[i][j] = result[j];
                                }
                                sql = "";
                                strLocation = "";
                            }
                        }
                    }
                }
                else
                {
                    sapModel.RohDetails = null;
                    statusMsg = PDMsg.NoRecordFound;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public bool GetRoutingMasterDetails(SapModel sapModel, ref string statusMsg, ref string[] arr)
        {

            DataTable dt = new DataTable();
            bool finish = false;
            string str = "01";
            string sql = "";
            string strLocation = "";
            string[] routingMaster = new string[] { "MATNR", "WERKS", "ARBPL_01", "KTSCH_01", "BMSCH", "MEINH", "UMREZ", "UMREN" };
            arr = routingMaster;
            DataTable tempRoutingTable = new DataTable();
            tempRoutingTable.Columns.Add("Material_Number", typeof(string));
            tempRoutingTable.Columns.Add("Plant", typeof(string));
            tempRoutingTable.Columns.Add("Work_Center", typeof(string));
            tempRoutingTable.Columns.Add("Standard_text_key", typeof(string));
            tempRoutingTable.Columns.Add("Base_Quantity", typeof(string));
            tempRoutingTable.Columns.Add("CC-UOM", typeof(string));
            tempRoutingTable.Columns.Add("Qty_in_Pc", typeof(string));
            tempRoutingTable.Columns.Add("Qty_in_Kg", typeof(string));
            try
            {
                string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,A.GROSS_WEIGHT,G.UNIT_OF_MEASURE,G.SAP_NO,G.SPECIAL_PROCUREMENT,G.SHORT_TEXT,A.NET_WEIGHT,"
                + "B.BIF_PROJ,C.RM_CD,D.RM_DESC,A.OPN_DESC,A.OPN_CD,E.CC_CODE,F.MODULE, E.WIRE_SIZE_MIN,F.SAP_CCCODE,F.SAP_BASE_QUANTITY,F.TYPE_NUT_BOLT"
                + " FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G "
                + " WHERE A.PART_NO= '" + sapModel.PartNo + "' AND E.PART_NO = '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO = '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE COLLATE DATABASE_DEFAULT =E.CC_CODE COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        statusMsg = PDMsg.NoRecordFound;
                        return false;
                    }
                    else
                    {
                        sapModel.ProcessSheetDetails = dt.DefaultView;
                        sapModel.FinishPosition = GetFinishPositionRouting(sapModel);
                        sapModel.NoOfOperations = GetNoofOperations(sapModel);
                        if (sapModel.ProcessSheetDetails.Count > 0)
                        {
                            sapModel.WireSize = string.Join("", sapModel.ProcessSheetDetails[0]["WIRE_SIZE_MIN"].ToString().Where(char.IsDigit)).ToIntValue();
                            sapModel.Location = sapModel.ProcessSheetDetails[0]["BIF_PROJ"].ToString();
                            DDLOC_MAST ddLocMaster = (from c in DB.DDLOC_MAST
                                                      where c.LOC_CODE == sapModel.Location
                                                      select c).FirstOrDefault<DDLOC_MAST>();
                            sapModel.Plant = ddLocMaster.Plant;

                            if (sapModel.Plant == 1000)
                            {
                                strLocation = "1";
                            }
                            else if (sapModel.Plant == 2000)
                            {
                                strLocation = "2";
                            }
                            else if (sapModel.Plant == 3000)
                            {
                                strLocation = "0";
                            }

                            sapModel.RoutingDetails = tempRoutingTable.DefaultView;

                            for (int i = 0; i < sapModel.ProcessSheetDetails.Count; i++)
                            {
                                str = (i + 1).ToString("00");

                                if (sapModel.ProcessSheetDetails.Count > 0)
                                {
                                    sapModel.Location = sapModel.ProcessSheetDetails[i]["BIF_PROJ"].ToString();
                                    ddLocMaster = (from c in DB.DDLOC_MAST
                                                   where c.LOC_CODE == sapModel.Location
                                                   select c).FirstOrDefault<DDLOC_MAST>();
                                    sapModel.Plant = ddLocMaster.Plant;

                                    if (sapModel.Plant == 1000)
                                    {
                                        strLocation = "1";
                                    }
                                    else if (sapModel.Plant == 2000)
                                    {
                                        strLocation = "2";
                                    }
                                    else if (sapModel.Plant == 3000)
                                    {
                                        strLocation = "0";
                                    }
                                    else
                                    {
                                        sapModel.Plant = 0;
                                    }
                                    // sql = sapModel.ProcessSheetDetails[i]["OPN_DESC"].ToString() + "\t";
                                    if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "25")
                                    {
                                        sql = "RM2" + sapModel.ProcessSheetDetails[i]["RM_CD"].ToString().Substring(1) + string.Format("{0:0000}", sapModel.WireSize) + "\t" + ((sapModel.Plant.ToValueAsString() != "0") ? sapModel.Plant.ToValueAsString() : "").ToValueAsString();

                                    }
                                    else
                                    {

                                        sql = sql + sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + str + strLocation + "\t" + ((sapModel.Plant.ToValueAsString() != "0") ? sapModel.Plant.ToValueAsString() : "").ToValueAsString();
                                    }


                                    /////////
                                    if (sapModel.FinishPosition != -1)
                                        if ((sapModel.FinishPosition < Convert.ToInt16(str)) && (sapModel.FinishPosition != 0))
                                        {
                                            sql = "";
                                            sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "-" + str + strLocation + "\t" + ((sapModel.Plant.ToValueAsString() != "0") ? sapModel.Plant.ToValueAsString() : "").ToValueAsString();
                                        }

                                    if (sapModel.ProcessSheetDetails[i]["SAP_NO"].ToString() == "45")
                                    {
                                        sql = sapModel.ProcessSheetDetails[i]["PART_NO"] + "-" + sapModel.OperCode + "\t" + ((sapModel.Plant.ToValueAsString() != "0") ? sapModel.Plant.ToValueAsString() : "").ToValueAsString();
                                    }

                                    string tempSql1, tempSql2, tempSql3, kgDetails = "";
                                    double kgvalue = 0;
                                    string[] arrwcValues;
                                    bool getWCCode = false;


                                    tempSql1 = (sapModel.ProcessSheetDetails[i]["SAP_CCCODE"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["SAP_CCCODE"].ToString() : "";
                                    tempSql2 = (sapModel.ProcessSheetDetails[i]["SAP_BASE_QUANTITY"].ToString().IsNotNullOrEmpty()) ? sapModel.ProcessSheetDetails[i]["SAP_BASE_QUANTITY"].ToString() : "";
                                    sql = sql + "\t" + tempSql1 + "\t" + sapModel.ProcessSheetDetails[i]["SHORT_TEXT"].ToString() + "\t" + tempSql2;

                                    tempSql3 = "900900/900800/900700/900600/900500/900000/450001/440015/440010/161010/155010/154010/1530100/153010/112000/110000/";
                                    arrwcValues = tempSql3.Split('/');
                                    for (int l = 0; l < arrwcValues.Length; l++)
                                    {
                                        if (sapModel.ProcessSheetDetails[i]["SAP_CCCODE"].ToString().ToUpper() == arrwcValues[l].ToUpper())
                                        {
                                            getWCCode = true;
                                        }
                                    }

                                    if (getWCCode == true)
                                    {
                                        if (sapModel.ProcessSheetDetails[i]["SAP_CCCODE"].ToString().ToUpper() == "153010")
                                        {
                                            kgDetails = "KG" + "\t" + 1 + "\t" + 1;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                kgvalue = (10 * 100) / sapModel.ProcessSheetDetails[i]["NET_WEIGHT"].ToString().ToDoubleValue();
                                                kgvalue = Double.IsInfinity(kgvalue) ? 0 : kgvalue;
                                            }
                                            catch (Exception)
                                            {

                                            }
                                            kgDetails = "KG" + "\t" + Math.Round(kgvalue) + "\t" + 10;
                                        }
                                    }
                                    else
                                    {
                                        kgDetails = "PC" + "\t" + 1 + "\t" + 1;
                                    }
                                    sql = sql + "\t" + kgDetails;

                                    char[] delimiters = new char[] { '\t' };
                                    string[] result = sql.Split(delimiters);
                                    sapModel.RoutingDetails.AddNew();
                                    for (int j = 0; j < result.Length; j++)
                                    {
                                        sapModel.RoutingDetails[i][j] = result[j];
                                    }
                                    sql = "";
                                    strLocation = "";
                                }
                            }

                        }
                    }
                }
                else
                {
                    statusMsg = PDMsg.NoRecordFound;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
        public int GetFinishPosition(SapModel sapModel)
        {
            int finshNo = 0;
            try
            {

                DataTable dt = new DataTable();
                string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,G.SAP_NO FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G WHERE A.PART_NO=  '" + sapModel.PartNo + "' AND E.PART_NO =  '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO =  '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE  COLLATE DATABASE_DEFAULT =E.CC_CODE  COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE order by SEQ_NO";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {

                        return 0;
                    }
                    else
                    {
                        finshNo = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int sapnum = dt.Rows[i]["SAP_NO"].ToString().ToIntValue();
                            switch (sapnum)
                            {
                                case 6:
                                case 13:
                                case 16:
                                //case 20:
                                case 24:
                                case 32:
                                case 48:
                                //case 72:
                                case 74:
                                case 75:
                                case 81:
                                //case 82:
                                //case 83:
                                    {
                                        return finshNo;

                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            finshNo++;
                        }
                    }

                }
                return finshNo;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return finshNo;
            }
        }
        public int GetFinishPositionRouting(SapModel sapModel)
        {
            int finshNo = 0;
            try
            {

                DataTable dt = new DataTable();
                //string report = "SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,G.SAP_NO FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G WHERE A.PART_NO=  '" + sapModel.PartNo + "' AND E.PART_NO =  '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO =  '" + sapModel.PartNo + "'  AND C.PART_NO= '" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE  COLLATE DATABASE_DEFAULT =E.CC_CODE  COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE order by SEQ_NO";
                string report = " SELECT DISTINCT B.PART_NO,B.PART_DESC,A.SEQ_NO,G.SAP_NO FROM PRD_MAST B,PROCESS_SHEET A,PROCESS_MAIN C,DDRM_MAST D ,PROCESS_CC E,DDCOST_CENT_MAST F ,DDOPER_MAST G WHERE A.PART_NO= '" + sapModel.PartNo + "' AND E.PART_NO = '" + sapModel.PartNo + "' AND CC_SNO= 1 AND A.SEQ_NO=E.SEQ_NO AND B.PART_NO = '" + sapModel.PartNo + "'  AND C.PART_NO='" + sapModel.PartNo + "' AND  CURRENT_PROC=1 AND D.RM_CODE=C.RM_CD AND C.ROUTE_NO=E.ROUTE_NO AND A.ROUTE_NO=C.ROUTE_NO AND F.COST_CENT_CODE COLLATE DATABASE_DEFAULT =E.CC_CODE COLLATE DATABASE_DEFAULT AND A.OPN_CD=G.OPER_CODE ";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {

                        return 0;
                    }
                    else
                    {
                        finshNo = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int sapnum = dt.Rows[i]["SAP_NO"].ToString().ToIntValue();
                            switch (sapnum)
                            {
                                case 6:
                                case 13:
                                case 16:
                                //case 20: // Jeyan: Removed as per CKMs request mail date 01/Sep/2016
                                case 24:   // Jeyan: added as per CKMs request mail date 01/Sep/2016
                                case 32:
                                case 46:   // Jeyan: added as per CKMs request mail date 01/Sep/2016
                                case 48:
                                //case 72: // Jeyan: Removed as per CKMs request mail date 01/Sep/2016
                                case 74:
                                case 75:
                                case 81:
                                //case 82: // Jeyan: Removed as per CKMs request mail date 01/Sep/2016
                                //case 83: // Jeyan: Removed as per CKMs request mail date 01/Sep/2016
                                    {
                                        return finshNo;

                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            finshNo++;
                        }
                    }

                }
                return finshNo;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return finshNo;
            }
        }
        public int GetNoofOperations(SapModel sapModel)
        {
            try
            {
                DataTable dt = new DataTable();
                string report = "SELECT DISTINCT SEQ_NO FROM PROCESS_SHEET WHERE  PART_NO=  '" + sapModel.PartNo + "'";
                dt = Dal.GetDataTable(report);
                if (dt != null)
                {
                    sapModel.NoOfoperations = dt.Rows.Count.ToString();
                }
                else
                {
                    sapModel.NoOfoperations = "0";
                }
                return sapModel.NoOfoperations.ToIntValue();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return sapModel.NoOfoperations.ToIntValue();
            }
        }
        public int GetNoofOperationsHalb(SapModel sapModel)
        {
            try
            {
                DataTable dt = new DataTable();
                string report = "SELECT DISTINCT SEQ_NO FROM PROCESS_SHEET WHERE  PART_NO=  '" + sapModel.PartNo + "'";
                dt = Dal.GetDataTable(report);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sapModel.NoOfoperations = Convert.ToInt32(dt.Rows.Count - 1).ToString();
                }
                else
                {
                    sapModel.NoOfoperations = "0";
                }
                return sapModel.NoOfoperations.ToIntValue();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return sapModel.NoOfoperations.ToIntValue();
            }
        }

    }
}
