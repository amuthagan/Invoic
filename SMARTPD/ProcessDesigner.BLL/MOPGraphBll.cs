using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.Model;
namespace ProcessDesigner.BLL
{
    public class MOPGraphBll : Essential
    {
        StringBuilder sbsql = new StringBuilder();
        public MOPGraphBll(UserInformation userInformation)
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

        public List<KeyValuePair<string, int>> FeasibilityReportCount(string startDate, string endDate)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                sbsql.Append(" select convert(varchar(6),fr_cs_date,112) as COLUMN0,  Convert(varchar(12),Count(ci_reference)) as COLUMN1 ");
                sbsql.Append(" from ddci_info where ");
                sbsql.Append(" ci_reference not like 'O%' and ");
                sbsql.Append(" pending = '0' ");
                sbsql.Append(" and ");
                sbsql.Append(" fr_cs_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103) ");
                sbsql.Append(" group by convert(varchar(6),fr_cs_date,112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());

                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<KeyValuePair<string, int>> DevelopmentPlanAdherence(string startDate, string endDate)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                sbsql.Append("select convert(varchar(6),doc_rel_date,112) as COLUMN0, Convert(varchar(12),Count(part_no)) as COLUMN1 ");
                sbsql.Append("from prd_mast where (HOLD_ME = 0 OR HOLD_ME IS NULL) and (CANCEL_STATUS = 0 OR CANCEL_STATUS IS NULL) and ");
                sbsql.Append("doc_rel_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103) ");
                sbsql.Append(" group by convert(varchar(6),doc_rel_date,112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<KeyValuePair<string, int>> LeadTimeAdherence(string startDate, string endDate, string difference)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                sbsql.Append("select convert(varchar(6),doc_rel_date,112) as COLUMN0, Convert(varchar(12),count(*)) as COLUMN1 from ( ");
                sbsql.Append("select part_no, doc_rel_date, allot_date from prd_mast where ");
                sbsql.Append("doc_rel_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103)) as reportdata ");
                //sbsql.Append(" Where decode(sign(21 - (doc_rel_date - allot_date)), 1, 1, 0, 1, -1, 0) = 0 ");
                sbsql.Append(" where (case when (21 - datediff(day,allot_date,doc_rel_date)) >= 0 then 1 else 0 end ) = " + difference);
                sbsql.Append(" group by convert(varchar(6),doc_rel_date,112) ");

                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<KeyValuePair<string, int>> DesignEffectiveness(string startDate, string endDate)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                //select to_char(fr_cs_date,'yymm'), Count(ci_reference) from ddci_info where ci_reference not like 'O%' and pending = '0' and fr_cs_date between to_date('01-Apr-15', 'dd-mon-yy') and to_date('05-Jan-16', 'dd-mon-yy') group by to_char(fr_cs_date,'yymm')
                sbsql.Append("select convert(varchar(6),fr_cs_date,112) as COLUMN0, Convert(varchar(12),count(ci_reference)) as COLUMN1  ");
                sbsql.Append("from ddci_info where ci_reference not like 'O%' and pending = '0' and ");
                sbsql.Append("fr_cs_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103)  ");
                sbsql.Append(" group by convert(varchar(6),fr_cs_date,112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public List<KeyValuePair<string, int>> ConformanceToSpecification(string startDate, string endDate)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                //select to_char(fr_cs_date,'yymm'), Count(ci_reference) from ddci_info where ci_reference not like 'O%' and pending = '0' and fr_cs_date between to_date('01-Apr-15', 'dd-mon-yy') and to_date('05-Jan-16', 'dd-mon-yy') group by to_char(fr_cs_date,'yymm')
                sbsql.Append("select convert(varchar(6),fr_cs_date,112) as COLUMN0, Convert(varchar(12),count(ci_reference)) as COLUMN1  ");
                sbsql.Append("from ddci_info where ci_reference not like 'O%' and pending = '0' and ");
                sbsql.Append("fr_cs_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103)  ");
                sbsql.Append(" group by convert(varchar(6),fr_cs_date,112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<KeyValuePair<string, int>> CostEffectiveness(string startDate, string endDate)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                //select to_char(fr_cs_date,'yymm'), Count(ci_reference) from ddci_info where ci_reference not like 'O%' and pending = '0' and fr_cs_date between to_date('01-Apr-15', 'dd-mon-yy') and to_date('05-Jan-16', 'dd-mon-yy') group by to_char(fr_cs_date,'yymm')
                sbsql.Append("select convert(varchar(6),fr_cs_date,112) as COLUMN0, Convert(varchar(12),count(ci_reference)) as COLUMN1  ");
                sbsql.Append("from ddci_info where ci_reference not like 'O%' and pending = '0' and ");
                sbsql.Append("fr_cs_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103)  ");
                sbsql.Append(" group by convert(varchar(6),fr_cs_date,112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<KeyValuePair<string, int>> RequestVsAgreedAndCompleted(string startDate, string endDate)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                //select to_char(fr_cs_date,'yymm'), Count(ci_reference) from ddci_info where ci_reference not like 'O%' and pending = '0' and fr_cs_date between to_date('01-Apr-15', 'dd-mon-yy') and to_date('05-Jan-16', 'dd-mon-yy') group by to_char(fr_cs_date,'yymm')
                sbsql.Append("select convert(varchar(6),fr_cs_date,112) as COLUMN0, Convert(varchar(12),count(ci_reference)) as COLUMN1  ");
                sbsql.Append("from ddci_info where ci_reference not like 'O%' and pending = '0' and ");
                sbsql.Append("fr_cs_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103) ");
                sbsql.Append(" and convert(date,'" + endDate + "',103)  ");
                sbsql.Append(" group by convert(varchar(6),fr_cs_date,112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        /// <summary>
        /// Product group category for document released
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="scategory"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, int>> PGCForDocumentReleased(string startDate, string endDate, string scategory)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                sbsql.Append("select convert(varchar(6),doc_rel_date,112) as COLUMN0, Convert(varchar(12),count(*)) as COLUMN1 from ");
                sbsql.Append("(select part_no, doc_rel_date from prd_mast where ");
                if (scategory == "TOTAL")
                {
                    sbsql.Append("(pg_category ='1' or pg_category ='2' or pg_category ='3') ");
                }
                else
                {
                    sbsql.Append("(pg_category = '" + scategory + "') ");
                }
                sbsql.Append(" and (HOLD_ME = 0 OR HOLD_ME IS NULL) and (CANCEL_STATUS = 0 OR CANCEL_STATUS IS NULL) ");
                sbsql.Append(" and doc_rel_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103)  ");
                sbsql.Append(" and convert(date,'" + endDate + "',103))  as cnt ");
                sbsql.Append(" group by convert(varchar(6),doc_rel_date,112) ");
                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Product group category for sample submitted products
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="scategory"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, int>> PGCForSampleSubmittedProducts(string startDate, string endDate, string scategory)
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            try
            {
                sbsql = new StringBuilder();
                sbsql = new StringBuilder();
                sbsql.Append("select convert(varchar(6),samp_submit_date,112) as COLUMN0, Convert(varchar(12),count(*)) as COLUMN1 from ");
                sbsql.Append("(select part_no, samp_submit_date from prd_mast where ");
                if (scategory == "TOTAL")
                {
                    sbsql.Append("(pg_category ='1' or pg_category ='2' or pg_category ='3') ");
                }
                else
                {
                    sbsql.Append("(pg_category = '" + scategory + "') ");
                }
                sbsql.Append(" and (HOLD_ME = 0 OR HOLD_ME IS NULL) and (CANCEL_STATUS = 0 OR CANCEL_STATUS IS NULL) ");
                sbsql.Append(" and samp_submit_date between ");
                sbsql.Append(" convert(date,'" + startDate + "',103)  ");
                sbsql.Append(" and convert(date,'" + endDate + "',103))  as cnt ");
                sbsql.Append(" group by convert(varchar(6),samp_submit_date,112) ");

                var getcollection = ToDataTable(DB.ExecuteQuery<DDPerformanceOneModel>(sbsql.ToString()).ToList());
                foreach (DataRow drRow in getcollection.Rows)
                {
                    valueList.Add(new KeyValuePair<string, int>(drRow["COLUMN0"].ToValueAsString().Substring(2), drRow["COLUMN1"].ToValueAsString().ToIntValue()));
                }
                return valueList;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


    }
}
