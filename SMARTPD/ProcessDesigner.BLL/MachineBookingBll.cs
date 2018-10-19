using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
namespace ProcessDesigner.BLL
{
    public class MachineBookingBll : Essential
    {
        public MachineBookingBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataSet GetMachineBookingRange(string plantName, DateTime? start_Date, DateTime? end_Date)
        {
            DateTime startDate = Convert.ToDateTime(start_Date);
            DateTime endDate = Convert.ToDateTime(end_Date);

            DataSet dsData = new DataSet();
            string sqlPlant = "";
            string sqlLoc = "";
            StringBuilder sqlMachine = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            try
            {
                switch (plantName)
                {
                    case "PADI":
                        sqlPlant = " AND M.BIF_PROJ LIKE 'M%'";
                        sqlLoc = "AND LOC_CODE LIKE 'M%'";
                        break;
                    case "KPM-BOLT":
                        sqlPlant = " AND M.BIF_PROJ LIKE 'K%' AND TYPE_NUT_BOLT='BOLT'";
                        sqlLoc = "AND LOC_CODE LIKE 'K%' AND TYPE_NUT_BOLT='BOLT'";
                        break;
                    case "KPM-NUT":
                        sqlPlant = " AND M.BIF_PROJ LIKE 'K%' AND TYPE_NUT_BOLT='NUT'";
                        sqlLoc = "AND LOC_CODE LIKE 'K%' AND TYPE_NUT_BOLT='NUT'";
                        break;
                    default:
                        sqlPlant = " AND M.BIF_PROJ LIKE 'Y%'";
                        sqlLoc = "AND LOC_CODE LIKE 'Y%'";
                        break;
                }

                sql.Append(" SELECT DISTINCT M.PART_NO,S.MACHINE_NAME,M.PG_CATEGORY,S.COST_CENT_CODE,");
                sql.Append(" Q.PPAP_PLAN,Q.DOC_REL_DT_ACTUAL,Q.TOOLS_READY_ACTUAL_DT,Q.FORGING_ACTUAL_DT,Q.SECONDARY_ACTUAL_DT,M.SAMP_SUBMIT_DATE ");
                sql.Append(" FROM PRD_MAST M,PROCESS_CC  P,PROCESS_MAIN R,MFM_MAST Q,DDCOST_CENT_MAST S,PROCESS_SHEET PS ");
                sql.Append(" WHERE ");
                sql.Append(" M.PART_NO = P.PART_NO  AND   P.PART_NO = R.PART_NO AND R.PART_NO = Q.PART_NO AND ");
                sql.Append(" Q.PPAP_PLAN >=convert(date,'" + startDate.ToString("dd-MMM-yyyy") + "',103)  AND Q.PPAP_PLAN <=convert(date,'" + endDate.ToString("dd-MMM-yyyy") + "',103) ");
                sql.Append(" AND M.pg_category IN ('1','2','3') ");
                sql.Append(sqlPlant);
                sql.Append(" AND P.CC_CODE COLLATE DATABASE_DEFAULT = S.COST_CENT_CODE COLLATE DATABASE_DEFAULT AND S.MACHINE_NAME IS NOT NULL");
                sql.Append(" AND r.CURRENT_PROC = 1");
                sql.Append(" AND P.ROUTE_NO = R.ROUTE_NO");
                sql.Append(" AND Q.STATUS IS NULL AND M.PART_NO = PS.PART_NO AND P.ROUTE_NO = PS.ROUTE_NO AND P.SEQ_NO = PS.SEQ_NO AND ");
                sql.Append(" PS.OPN_DESC LIKE 'FORGE%' AND P.CC_SNO = 1 ");
                sql.Append(" ORDER BY S.MACHINE_NAME ,M.PG_CATEGORY,Q.PPAP_PLAN ");

                sqlMachine.Append(" SELECT  DISTINCT COST_CENT_CODE as COLUMN0,COST_CENT_DESC as COLUMN1,MACHINE_NAME COLUMN2  FROM  DDCOST_CENT_MAST  WHERE MACHINE_NAME IS NOT NULL  ");
                sqlMachine.Append(sqlLoc);
                sqlMachine.Append(" Order by MACHINE_NAME asc ");

                DataTable dtData = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sqlMachine.ToString()).ToList());
                if (dtData.IsNotNullOrEmpty())
                {
                    dtData.TableName = "MACHINE_NAME";
                    dsData.Tables.Add(dtData.Copy());
                }

                DataTable dtData1 = new DataTable();
                //dtData1 = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sql.ToString().Trim()).ToList());
                dtData1 = Dal.GetDataTable(sql);
                if (dtData1.IsNotNullOrEmpty())
                {
                    dtData1.TableName = "MACHINE_BOOKING_DATA";
                    dsData.Tables.Add(dtData1.Copy());
                }

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
