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
    public class ReportMISFeatureWise : Essential
    {
        public ReportMISFeatureWise(UserInformation userInformation)
        {
            try
            {
                this.userInformation = userInformation;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<PCCS> GetFeature(PCCS paramEntity = null)
        {

            List<PCCS> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.PART_NO.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PCCS
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.PART_NO == paramEntity.PART_NO
                                 select row).ToList<PCCS>();
                }
                else
                {

                    lstEntity = (from row in DB.PCCS
                                 where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                                 select row).Distinct<PCCS>().ToList<PCCS>();
                }
                if (lstEntity.IsNotNullOrEmpty())
                    lstEntity = lstEntity.GroupBy(row => row.FEATURE).Select(row => row.First()).OrderBy(row => row.FEATURE).Distinct<PCCS>().ToList<PCCS>();


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public DataTable GetFeatureDataTable(PCCS paramEntity = null)
        {

            DataTable lstEntity = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT DISTINCT FEATURE FROM PCCS ");

                List<StringBuilder> sqlList = new List<StringBuilder>() { sb };

                DataSet dsReport = Dal.GetDataSet(sqlList);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    lstEntity = dsReport.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<PCCS> GetFeature1(PCCS paramEntity = null)
        {
            return GetFeature(paramEntity);
        }

        public List<PCCS> GetFeature2(PCCS paramEntity = null)
        {
            return GetFeature(paramEntity);
        }

        public System.Data.DataSet GetAllFeatures(PCCS feature = null, PCCS feature1 = null, PCCS feature2 = null, PCCS specification = null, PRD_MAST productMaster = null)
        {

            System.Data.DataSet dsReport = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT DISTINCT 123456789012 AS SNO, A.PART_NO,   A.PART_DESC,  A.IDPK,  B.FEATURE,   ");
                sb.Append("B.SPEC_MIN,   B.SPEC_MAX,   A.QUALITY,   E.CUST_NAME,   ");
                sb.Append("(select TOP 1 p.CTRL_SPEC_MIN from pccs p where p.PART_NO=b.part_no   ");
                sb.Append("and (UPPER(p.FEATURE) LIKE 'HEAD HEI%'    ");
                sb.Append("or UPPER(p.FEATURE) LIKE 'HEAD TH%')   ");
                sb.Append(") CTRL_SPEC_MIN,      ");
                sb.Append("(select TOP 1 p.CTRL_SPEC_MAX from pccs p where p.PART_NO=b.part_no   ");
                sb.Append("and (UPPER(p.FEATURE) LIKE 'HEAD HEI%'    ");
                sb.Append("or UPPER(p.FEATURE) LIKE 'HEAD TH%')   ");
                sb.Append(") as CTRL_SPEC_MAX,      ");
                sb.Append("(select TOP 1 p.CTRL_SPEC_MIN from pccs p where p.PART_NO=b.part_no   ");
                sb.Append("and (UPPER(p.FEATURE) LIKE 'COLLAR DIA%'    ");
                sb.Append("or UPPER(p.FEATURE) LIKE 'FLANGE DIA%')   ");
                sb.Append(") AS COLLAR_FLANGE_DIA_MIN,      ");
                sb.Append("(select TOP 1 p.CTRL_SPEC_MAX from pccs p where p.PART_NO=b.part_no   ");
                sb.Append("and (UPPER(p.FEATURE) LIKE 'COLLAR DIA%'    ");
                sb.Append("or UPPER(p.FEATURE) LIKE 'FLANGE DIA%'))   ");
                 sb.Append("AS COLLAR_FLANGE_DIA_MAX,      ");
                sb.Append("(select TOP 1 p.CTRL_SPEC_MIN from pccs p where p.PART_NO=b.part_no   ");
                sb.Append("and (UPPER(p.FEATURE) LIKE 'COLLAR TH%'    ");
                sb.Append("or UPPER(p.FEATURE) LIKE 'FLANGE TH%')) AS COLLAR_FLANGE_TK_MIN,      ");
                sb.Append("(select TOP 1 p.CTRL_SPEC_MAX from pccs p where p.PART_NO=b.part_no   ");
                sb.Append("and (UPPER(p.FEATURE) LIKE 'COLLAR TH%'    ");
                sb.Append("or UPPER(p.FEATURE) LIKE 'FLANGE TH%')) AS COLLAR_FLANGE_TK_MAX,      ");
                sb.Append("(SELECT TOP 1 VFORG.CC_CODE FROM V_FORGING_COST_CENTER VFORG WHERE VFORG.PART_NO=B.PART_NO) AS FORGING_COST_CENTER,      ");
                sb.Append("(SELECT TOP 1 VFORG.WIRE_SIZE_MIN FROM V_FORGING_COST_CENTER VFORG WHERE VFORG.PART_NO=B.PART_NO) AS WIRE_DIA_MIN,      ");
                sb.Append("(SELECT TOP 1 VFORG.WIRE_SIZE_MAX FROM V_FORGING_COST_CENTER VFORG WHERE VFORG.PART_NO=B.PART_NO) AS WIRE_DIA_MAX    ");
                sb.Append("FROM PRD_MAST A,   PCCS B,      ");
                sb.Append("PRD_CIREF C,   DDCI_INFO D,      ");
                sb.Append("DDCUST_MAST E    ");
                sb.Append("WHERE A.PART_NO =B.PART_NO AND    ");
                sb.Append("A.PART_NO   =C.PART_NO AND    ");
                
                sb.Append("C.CI_REF    =D.CI_REFERENCE AND C.CURRENT_CIREF = 1 AND D.CUST_CODE =E.CUST_CODE     ");
                //sb.Append("C.CI_REF    =D.CI_REFERENCE AND D.CUST_CODE =E.CUST_CODE     ");

                string featureSql = "";
                if (feature.IsNotNullOrEmpty() && feature.FEATURE.IsNotNullOrEmpty())
                    featureSql = featureSql + "UPPER(B.FEATURE) LIKE '%" + sqlEncode(feature.FEATURE.ToUpper()) + "%' ";

                string feature1Sql = "";
                if (feature1.IsNotNullOrEmpty() && feature1.FEATURE.IsNotNullOrEmpty())
                    feature1Sql = feature1Sql + "UPPER(B.FEATURE) LIKE '%" + sqlEncode(feature1.FEATURE.ToUpper()) + "%' ";

                if (featureSql.IsNotNullOrEmpty() && feature1Sql.IsNotNullOrEmpty())
                    featureSql = featureSql + " OR " + feature1Sql;
                else if (!featureSql.IsNotNullOrEmpty() && feature1Sql.IsNotNullOrEmpty())
                    featureSql = feature1Sql;

                string feature2Sql = "";
                if (feature2.IsNotNullOrEmpty() && feature2.FEATURE.IsNotNullOrEmpty())
                    feature2Sql = feature2Sql + "UPPER(B.FEATURE) LIKE '%" + sqlEncode(feature2.FEATURE.ToUpper()) + "%' ";

                if (featureSql.IsNotNullOrEmpty() && feature2Sql.IsNotNullOrEmpty())
                    featureSql = featureSql + " OR " + feature2Sql;
                else if (!featureSql.IsNotNullOrEmpty() && feature2Sql.IsNotNullOrEmpty())
                    featureSql = feature2Sql;

                if (featureSql.IsNotNullOrEmpty() && featureSql.Trim().Length > 0)
                    sb.Append(" AND ( " + featureSql + ") ");

                if (productMaster.IsNotNullOrEmpty() && productMaster.PART_DESC.IsNotNullOrEmpty())
                    sb.Append(" AND UPPER(A.PART_DESC) LIKE '%" + sqlEncode(productMaster.PART_DESC.ToUpper()) + "%' ");

                if (specification.IsNotNullOrEmpty())
                {
                    if (specification.SPEC_MIN.IsNotNullOrEmpty() && specification.SPEC_MIN.IsNumeric())
                    {
                        sb.Append(" AND B.SPEC_MIN BETWEEN '" + sqlEncode(specification.SPEC_MIN) + "' AND '" + Convert.ToDouble(specification.SPEC_MIN) + 1 + "' ");
                    }
                    else if (specification.SPEC_MIN.IsNotNullOrEmpty())
                    {
                        sb.Append(" AND  UPPER(B.SPEC_MIN) LIKE '" + sqlEncode(specification.SPEC_MIN.ToUpper()) + "%' ");
                    }

                    if (specification.SPEC_MAX.IsNotNullOrEmpty() && specification.SPEC_MAX.IsNumeric() && 1 == 2)
                    {
                        sb.Append(" AND B.SPEC_MAX BETWEEN '" + sqlEncode(specification.SPEC_MAX) + "' AND '" + Convert.ToDouble(specification.SPEC_MAX) + 1 + "' ");
                    }
                    else if (specification.SPEC_MAX.IsNotNullOrEmpty())
                    {
                        sb.Append(" AND  UPPER(B.SPEC_MAX) = '" + sqlEncode(specification.SPEC_MAX.ToUpper()) + "' ");
                    }
                }
                sb.Append(" ORDER BY A.PART_NO ");
                
                List<StringBuilder> sqlList = new List<StringBuilder>() { sb };
                dsReport = Dal.GetDataSet(sqlList);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    dsReport.Tables[0].TableName = "FEATURE_WISE_REPORT";
                    long sno = 0;
                    foreach (DataRow dataRow in dsReport.Tables[0].Rows)
                    {
                        string part_no = dataRow["PART_NO"].ToValueAsString();
                        sno++;
                        dataRow["SNO"] = sno.ToValueAsString();
                    }
                    dsReport.Tables[0].AcceptChanges();
                }
                DataTable dtCompany = new DataTable();
                dtCompany.TableName = "CompanyName";
                dtCompany.Columns.Add("Name");
                dtCompany.Columns.Add("ShortName");
                dtCompany.Columns.Add("Phone");
                dtCompany.Columns.Add("Fax");
                dtCompany.Columns.Add("Mobile");
                dtCompany.Columns.Add("EMail");
                dtCompany.Columns.Add("Title");
                dtCompany.Columns.Add("ReportTitle");
                if (dsReport.IsNotNullOrEmpty())
                    dsReport.Tables.Add(dtCompany);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return dsReport;
        }

        public System.Data.DataSet GetAllFeaturesBackUp(PCCS feature = null, PCCS feature1 = null, PCCS feature2 = null, PCCS specification = null, PRD_MAST productMaster = null)
        {

            System.Data.DataSet dsReport = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT DISTINCT 123456789012 AS SNO, A.PART_NO, ");
                sb.Append("  A.PART_DESC, ");
                sb.Append("  B.FEATURE, ");
                sb.Append("  B.SPEC_MIN, ");
                sb.Append("  B.SPEC_MAX, ");
                sb.Append("  A.QUALITY, ");
                sb.Append("  E.CUST_NAME, ");
                sb.Append("  B.CTRL_SPEC_MIN, ");
                sb.Append("  B.CTRL_SPEC_MAX, ");

                sb.Append("  ' ' AS COLLAR_FLANGE_DIA_MIN, ");
                sb.Append("  ' ' AS COLLAR_FLANGE_DIA_MAX, ");

                sb.Append("  ' ' AS COLLAR_FLANGE_TK_MIN, ");
                sb.Append("  ' ' AS COLLAR_FLANGE_TK_MAX, ");

                sb.Append("  ' ' AS FORGING_COST_CENTER, ");

                sb.Append("  ' ' AS WIRE_DIA_MIN, ");
                sb.Append("  ' ' AS WIRE_DIA_MAX ");

                sb.Append("FROM PRD_MAST A, ");
                sb.Append("  PCCS B, ");
                sb.Append("  PRD_CIREF C, ");
                sb.Append("  DDCI_INFO D, ");
                sb.Append("  DDCUST_MAST E ");
                sb.Append("WHERE A.PART_NO =B.PART_NO ");
                sb.Append("AND A.PART_NO   =C.PART_NO ");

                sb.Append("AND C.CI_REF    =D.CI_REFERENCE AND C.CURRENT_CIREF = 1 ");
                //sb.Append("AND C.CI_REF    =D.CI_REFERENCE ");
                
                sb.Append("AND D.CUST_CODE =E.CUST_CODE ");
                //sb.Append("AND A.PART_NO   ='M03920' ");

                string featureSql = "";
                if (feature.IsNotNullOrEmpty() && feature.FEATURE.IsNotNullOrEmpty())
                    featureSql = featureSql + "UPPER(B.FEATURE) LIKE '%" + feature.FEATURE.ToUpper() + "%' ";

                string feature1Sql = "";
                if (feature1.IsNotNullOrEmpty() && feature1.FEATURE.IsNotNullOrEmpty())
                    feature1Sql = feature1Sql + "UPPER(B.FEATURE) LIKE '%" + feature1.FEATURE.ToUpper() + "%' ";

                if (featureSql.IsNotNullOrEmpty() && feature1Sql.IsNotNullOrEmpty())
                    featureSql = featureSql + " OR " + feature1Sql;
                else if (!featureSql.IsNotNullOrEmpty() && feature1Sql.IsNotNullOrEmpty())
                    featureSql = feature1Sql;

                string feature2Sql = "";
                if (feature2.IsNotNullOrEmpty() && feature2.FEATURE.IsNotNullOrEmpty())
                    feature2Sql = feature2Sql + "UPPER(B.FEATURE) LIKE '%" + feature2.FEATURE.ToUpper() + "%' ";

                if (featureSql.IsNotNullOrEmpty() && feature2Sql.IsNotNullOrEmpty())
                    featureSql = featureSql + " OR " + feature2Sql;
                else if (!featureSql.IsNotNullOrEmpty() && feature2Sql.IsNotNullOrEmpty())
                    featureSql = feature2Sql;

                if (featureSql.IsNotNullOrEmpty() && featureSql.Trim().Length > 0)
                    sb.Append(" AND ( " + featureSql + ") ");

                if (productMaster.IsNotNullOrEmpty() && productMaster.PART_DESC.IsNotNullOrEmpty())
                    sb.Append(" AND UPPER(A.PART_DESC) LIKE '%" + productMaster.PART_DESC.ToUpper() + "%' ");

                if (specification.IsNotNullOrEmpty())
                {
                    if (specification.SPEC_MIN.IsNotNullOrEmpty() && specification.SPEC_MIN.IsNumeric())
                    {
                        sb.Append(" AND B.SPEC_MIN BETWEEN '" + specification.SPEC_MIN + "' AND '" + Convert.ToDouble(specification.SPEC_MIN) + 1 + "' ");
                    }
                    else if (specification.SPEC_MIN.IsNotNullOrEmpty())
                    {
                        sb.Append(" AND  UPPER(B.SPEC_MIN) LIKE '" + specification.SPEC_MIN.ToUpper() + "%' ");
                    }

                    if (specification.SPEC_MAX.IsNotNullOrEmpty() && specification.SPEC_MAX.IsNumeric() && 1 == 2)
                    {
                        sb.Append(" AND B.SPEC_MAX BETWEEN '" + specification.SPEC_MAX + "' AND '" + Convert.ToDouble(specification.SPEC_MAX) + 1 + "' ");
                    }
                    else if (specification.SPEC_MAX.IsNotNullOrEmpty())
                    {
                        sb.Append(" AND  UPPER(B.SPEC_MAX) = '" + specification.SPEC_MAX.ToUpper() + "' ");
                    }
                }

                sb.Append(" ORDER BY A.PART_NO ");

                List<StringBuilder> sqlList = new List<StringBuilder>() { sb };

                dsReport = Dal.GetDataSet(sqlList);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    dsReport.Tables[0].TableName = "FEATURE_WISE_REPORT";

                    List<PCCS> lstAllPCCS = (from row in DB.PCCS
                                             where (
                                                   row.FEATURE.ToUpper().StartsWith("HEAD HEI") ||
                                                   row.FEATURE.ToUpper().StartsWith("HEAD TH") ||
                                                   row.FEATURE.ToUpper().StartsWith("COLLAR DIA") ||
                                                   row.FEATURE.ToUpper().StartsWith("FLANGE DIA") ||
                                                   row.FEATURE.ToUpper().StartsWith("COLLAR TH") ||
                                                   row.FEATURE.ToUpper().StartsWith("FLANGE TH"))
                                             select row).ToList<PCCS>();

                    List<PCCS> lstPCCS = null;


                    List<V_FORGING_COST_CENTER> lstAllFORGING_COST_CENTER = (from row in DB.V_FORGING_COST_CENTER
                                                                             select row).ToList<V_FORGING_COST_CENTER>();

                    List<V_FORGING_COST_CENTER> lstFORGING_COST_CENTER = null;

                    //SELECT PC.CC_CODE,
                    //  PC.PART_NO,
                    //  PC.WIRE_SIZE_MIN,
                    //  PC.WIRE_SIZE_MAX
                    //FROM PROCESS_CC PC
                    //WHERE PC.PART_NO = 'M92910'
                    //AND PC.SEQ_NO   IN
                    //  (SELECT a.SEQ_NO
                    //  FROM PROCESS_SHEET a
                    //  WHERE a.OPN_DESC LIKE 'FORG%'
                    //  AND A.PART_NO   = PC.PART_NO
                    //  AND A.PART_NO   = 'M92910'
                    //  AND a.ROUTE_NO IN
                    //    (SELECT DISTINCT(B.ROUTE_NO)
                    //    FROM PROCESS_MAIN B
                    //    WHERE B.CURRENT_PROC=1
                    //    AND B.PART_NO       =a.PART_NO
                    //    AND B.PART_NO       = PC.PART_NO
                    //    AND B.PART_NO       = 'M92910'
                    //    )
                    //  )
                    //AND PC.ROUTE_NO IN
                    //  (SELECT DISTINCT(C.ROUTE_NO)
                    //  FROM PROCESS_MAIN C
                    //  WHERE C.CURRENT_PROC=1
                    //  AND C.PART_NO       = PC.PART_NO
                    //  AND C.PART_NO       = 'M92910'
                    //  )
                    long sno = 0;
                    foreach (DataRow dataRow in dsReport.Tables[0].Rows)
                    {
                        string part_no = dataRow["PART_NO"].ToValueAsString();

                        sno++;
                        dataRow["SNO"] = sno.ToValueAsString();

                        lstPCCS = (from row in lstAllPCCS.AsEnumerable()
                                   where row.PART_NO == part_no && (row.FEATURE.ToUpper().StartsWith("HEAD HEI") ||
                                   row.FEATURE.ToUpper().StartsWith("HEAD TH"))
                                   select row).ToList<PCCS>();
                        if (lstPCCS.IsNotNullOrEmpty() && lstPCCS.Count > 0)
                        {
                            dataRow["CTRL_SPEC_MIN"] = lstPCCS[0].CTRL_SPEC_MIN;
                            dataRow["CTRL_SPEC_MAX"] = lstPCCS[0].CTRL_SPEC_MAX;

                        }

                        lstPCCS = (from row in lstAllPCCS.AsEnumerable()
                                   where row.PART_NO == part_no && (row.FEATURE.ToUpper().StartsWith("COLLAR DIA") ||
                                   row.FEATURE.ToUpper().StartsWith("FLANGE DIA"))
                                   select row).ToList<PCCS>();
                        if (lstPCCS.IsNotNullOrEmpty() && lstPCCS.Count > 0)
                        {
                            dataRow["COLLAR_FLANGE_DIA_MIN"] = lstPCCS[0].CTRL_SPEC_MIN;
                            dataRow["COLLAR_FLANGE_DIA_MAX"] = lstPCCS[0].CTRL_SPEC_MAX;
                        }

                        lstPCCS = (from row in lstAllPCCS.AsEnumerable()
                                   where row.PART_NO == part_no && (row.FEATURE.ToUpper().StartsWith("COLLAR TH") ||
                                   row.FEATURE.ToUpper().StartsWith("FLANGE TH"))
                                   select row).ToList<PCCS>();
                        if (lstPCCS.IsNotNullOrEmpty() && lstPCCS.Count > 0)
                        {
                            dataRow["COLLAR_FLANGE_TK_MIN"] = lstPCCS[0].CTRL_SPEC_MIN;
                            dataRow["COLLAR_FLANGE_TK_MAX"] = lstPCCS[0].CTRL_SPEC_MAX;
                        }

                        lstFORGING_COST_CENTER = (from row in lstAllFORGING_COST_CENTER.AsEnumerable()
                                                  where row.PART_NO == part_no
                                                  select row).ToList<V_FORGING_COST_CENTER>();
                        if (lstFORGING_COST_CENTER.IsNotNullOrEmpty() && lstFORGING_COST_CENTER.Count > 0)
                        {
                            dataRow["FORGING_COST_CENTER"] = lstFORGING_COST_CENTER[0].CC_CODE.ToValueAsString();
                            dataRow["WIRE_DIA_MIN"] = lstFORGING_COST_CENTER[0].WIRE_SIZE_MIN.ToValueAsString();
                            dataRow["WIRE_DIA_MAX"] = lstFORGING_COST_CENTER[0].WIRE_SIZE_MAX.ToValueAsString();
                        }

                        //sb = new StringBuilder();
                        //sb.Append("SELECT CC_CODE, ");
                        //sb.Append("  WIRE_SIZE_MIN, ");
                        //sb.Append("  WIRE_SIZE_MAX ");
                        //sb.Append("FROM PROCESS_CC ");
                        //sb.Append("WHERE PART_NO='" + part_no + "' ");
                        //sb.Append("AND SEQ_NO   = ");
                        //sb.Append("  (SELECT a.SEQ_NO ");
                        //sb.Append("  FROM PROCESS_SHEET a ");
                        //sb.Append("  WHERE a.OPN_DESC LIKE 'FORG%' ");
                        //sb.Append("  AND a.ROUTE_NO= ");
                        //sb.Append("    (SELECT DISTINCT(ROUTE_NO) ");
                        //sb.Append("    FROM PROCESS_MAIN B ");
                        //sb.Append("    WHERE B.CURRENT_PROC=1 ");
                        //sb.Append("    AND B.PART_NO       =a.PART_NO ");
                        //sb.Append("    AND B.PART_NO       ='" + part_no + "' ");
                        //sb.Append("    ) ");
                        //sb.Append("  ) ");
                        //sb.Append("AND ROUTE_NO= ");
                        //sb.Append("  (SELECT DISTINCT(ROUTE_NO) ");
                        //sb.Append("  FROM PROCESS_MAIN B ");
                        //sb.Append("  WHERE B.CURRENT_PROC=1 ");
                        //sb.Append("  AND B.PART_NO       =PART_NO ");
                        //sb.Append("  AND B.PART_NO       ='" + part_no + "' ");
                        //sb.Append("  )");
                        //sqlList = new List<StringBuilder>() { sb };

                        //DataSet dsResult = Dal.GetDataSet(sqlList);

                        //if (dsResult.IsNotNullOrEmpty() && dsResult.Tables.IsNotNullOrEmpty() && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                        //{
                        //    DataRow resultRow = dsResult.Tables[0].Rows[0];
                        //    dataRow["FORGING_COST_CENTER"] = resultRow["CC_CODE"].ToValueAsString();
                        //    dataRow["WIRE_DIA_MIN"] = resultRow["WIRE_SIZE_MIN"].ToValueAsString();
                        //    dataRow["WIRE_DIA_MAX"] = resultRow["WIRE_SIZE_MAX"].ToValueAsString();
                        //    dataRow.AcceptChanges();
                        //}

                        dataRow.AcceptChanges();
                    }
                    dsReport.Tables[0].AcceptChanges();
                }
                DataTable dtCompany = new DataTable();
                dtCompany.TableName = "CompanyName";
                dtCompany.Columns.Add("Name");
                dtCompany.Columns.Add("ShortName");
                dtCompany.Columns.Add("Phone");
                dtCompany.Columns.Add("Fax");
                dtCompany.Columns.Add("Mobile");
                dtCompany.Columns.Add("EMail");
                dtCompany.Columns.Add("Title");
                dtCompany.Columns.Add("ReportTitle");
                if (dsReport.IsNotNullOrEmpty())
                    dsReport.Tables.Add(dtCompany);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return dsReport;
        }


        private string sqlEncode(string sqlValue)
        {
            return sqlValue.Replace("'", "''");
        }

    }
}
