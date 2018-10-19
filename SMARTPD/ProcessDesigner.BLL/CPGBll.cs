using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data;
using System.Reflection;

namespace ProcessDesigner.BLL
{
    public class CPGBll : Essential
    {
        public CPGBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool GetPccsComboValues(CPGModel cpgModel)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable((from o in DB.SPECIAL_CHARACTER
                                  select new { o.SPEC_CHAR }).ToList());
                cpgModel.SplChar = (dt != null) ? dt.DefaultView : null;

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetCPGAvailable(CPGModel cpgModel)
        {
            try
            {
                PROCESS_SHEET process_sheet = new PROCESS_SHEET();
                try
                {

                    // select opn_cd,opn_desc from process_sheet
                    process_sheet = (from o in DB.PROCESS_SHEET
                                     where o.PART_NO == cpgModel.PartNo && o.SEQ_NO == cpgModel.SeqNo && o.ROUTE_NO == cpgModel.RouteNo
                                     select o).FirstOrDefault<PROCESS_SHEET>();
                    if (process_sheet.IsNotNullOrEmpty())
                    {
                        cpgModel.OperCode = process_sheet.OPN_CD;
                        cpgModel.OperDesc = process_sheet.OPN_DESC;
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    process_sheet = null;
                    cpgModel.OperCode = 0;
                    cpgModel.OperDesc = "";
                }


                DataTable dt = new DataTable();
                DataSet dsMaster = new DataSet();
                decimal sno = 0;

                if (cpgModel.OperCode == 1020 || cpgModel.OperCode == 1030 || cpgModel.OperCode == 1040)
                {

                    List<StringBuilder> sbSQL = new List<StringBuilder>();
                    try
                    {
                        List<FEATURE_MASTER> featureDetails = new List<FEATURE_MASTER>();
                        sbSQL.Add(new StringBuilder("select ROW_NUMBER() OVER(ORDER BY feature_desc asc) AS SNO,* from (select  distinct M.FEATURE_CODE,FEATURE_DESC,MEASURING_TECHNIQUE,SAMPLE_SIZE,SAMPLE_FREQUENCY,  CONTROL_METHOD,REACTION_PLAN FROM FEATURE_MASTER M,FORGING_MASTER D,PRD_MAST P  WHERE M.FEATURE_CODE=D.FEATURE_CODE AND    ( P.TYPE =D.PRD_CODE OR P.HEAD_STYLE =D.PRD_CODE OR P.APPLICATION=D.PRD_CODE OR P.PRD_CLASS_CD =D.PRD_CODE OR P.PRD_GRP_CD =D.PRD_CODE OR P.ADDL_FEATURE=D.PRD_CODE  OR P.KEYWORDS = D.PRD_CODE ) AND PART_NO='" + cpgModel.PartNo + "')  sf order by feature_desc "));
                        dsMaster = Dal.GetDataSet(sbSQL);
                        if (dsMaster.Tables[0] != null)
                        {
                            cpgModel.AvailableCharacteristcsDetails = dsMaster.Tables[0].DefaultView;
                        }
                        else
                        {
                            cpgModel.AvailableCharacteristcsDetails = null;
                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex.LogException();
                    }
                }
                else
                {
                    List<FEATURE_MASTER> lstFEATURE_MASTER = (from o in DB.FEATURE_MASTER
                                                              where o.OPER_CODE == cpgModel.OperCode && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                              orderby o.FEATURE_DESC ascending
                                                              select o).Distinct().ToList();
                    dt = lstFEATURE_MASTER.Select((o, index) => new FEATURE_MASTER()
                    {
                        SNO = ++sno,
                        FEATURE_CODE = o.FEATURE_CODE,
                        FEATURE_DESC = o.FEATURE_DESC,
                        MEASURING_TECHNIQUE = o.MEASURING_TECHNIQUE,
                        SAMPLE_SIZE = o.SAMPLE_SIZE,
                        SAMPLE_FREQUENCY = o.SAMPLE_FREQUENCY,
                        CONTROL_METHOD = o.CONTROL_METHOD,
                        REACTION_PLAN = o.REACTION_PLAN
                    }).ToList().ToDataTable<FEATURE_MASTER>();

                    //, o.FEATURE_CODE, o.FEATURE_DESC, o.MEASURING_TECHNIQUE, o.SAMPLE_SIZE, o.SAMPLE_FREQUENCY, o.CONTROL_METHOD, o.REACTION_PLAN 
                    if (dt != null)
                    {
                        cpgModel.AvailableCharacteristcsDetails = dt.DefaultView;

                    }
                    else
                    {
                        cpgModel.AvailableCharacteristcsDetails = null;
                    }

                }
                if (cpgModel.AvailableCharacteristcsDetails.IsNotNullOrEmpty())
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp = cpgModel.AvailableCharacteristcsDetails.Table.Clone();
                    for (int i = 0; i < cpgModel.AvailableCharacteristcsDetails.Table.Rows.Count; i++)
                    {
                        dtTemp.DefaultView.RowFilter = "FEATURE_DESC='" + cpgModel.AvailableCharacteristcsDetails[i]["FEATURE_DESC"] + "'";
                        if (dtTemp.DefaultView.Count == 0)
                        {
                            dtTemp.ImportRow(cpgModel.AvailableCharacteristcsDetails[i].Row);
                            dtTemp.Rows[dtTemp.Rows.Count - 1]["SNO"] = dtTemp.Rows.Count;
                        }
                        dtTemp.DefaultView.RowFilter = string.Empty;
                    }
                    cpgModel.AvailableCharacteristcsDetails = dtTemp.DefaultView;
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public bool GetPccsExistingRecords(CPGModel cpgModel)
        {
            try
            {
                DataTable dt = new DataTable();
                double sno = 0;
                dt = ToDataTable((from o in DB.PCCS
                                  where o.PART_NO == cpgModel.PartNo && o.SEQ_NO == cpgModel.SeqNo && o.ROUTE_NO == cpgModel.RouteNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  orderby o.SNO ascending
                                  select new { o.SNO, o.ISR_NO, o.FEATURE, o.PROCESS_FEATURE, o.SPEC_MIN, o.SPEC_MAX, o.CTRL_SPEC_MIN, o.CTRL_SPEC_MAX, o.SPEC_CHAR, o.DEPT_RESP, o.FREQ_OF_INSP, o.GAUGES_USED, o.SAMPLE_SIZE, o.CONTROL_METHOD, o.REACTION_PLAN }).ToList());

                //dt = DB.PCCS.Where(o => o.PART_NO == cpgModel.PartNo && o.SEQ_NO == cpgModel.SeqNo).OrderBy(o => o.SNO).Select((o, i) => new PCCS { SNO = i + 1 }).ToList<PCCS>().ToDataTable<PCCS>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["SNO"] = i + 1;
                }
                if (dt != null)
                {
                    cpgModel.PccsDetails = dt.DefaultView;
                    // if (dt.DefaultView.Count > 0) dt.DefaultView.RowFilter = " FEATURE <> ''";
                    cpgModel.SplCharacteristcsDetails = dt.DefaultView;

                }
                else
                {
                    cpgModel.PccsDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetGrid3CharMeasuringTechniquesDetails(CPGModel cpgModel)
        {
            DataSet dsMaster = new DataSet();
            DataView dvTemp = new DataView();
            List<StringBuilder> sbSQL = new List<StringBuilder>();
            try
            {
                sbSQL.Add(new StringBuilder("select '' as SNO,FEATURE_DESC as FEATURE,MEASURING_TECHNIQUE,SAMPLE_SIZE,SAMPLE_FREQUENCY,  CONTROL_METHOD,REACTION_PLAN from feature_master m,forging_master d,prd_mast p  where m.feature_code=d.feature_code and    ( p.type =d.prd_code or p.head_style =d.prd_code or p.application=d.prd_code or P.prd_class_cd =d.prd_code or p.prd_grp_cd =d.prd_code or p.addl_feature=d.prd_code  or p.keywords = d.prd_code ) and part_no='" + cpgModel.PartNo + "' and feature_desc= '" + cpgModel.FeatureDesc + "'"));
                dsMaster = Dal.GetDataSet(sbSQL);
                if (dsMaster.Tables[0] != null)
                {
                    cpgModel.Grd3MeasuringTechniquesDetails = dsMaster.Tables[0].DefaultView;

                }
                else
                {
                    cpgModel.Grd3MeasuringTechniquesDetails = null;
                }

                if (cpgModel.AvailableCharacteristcsDetails.Count > 0)
                {
                    dvTemp = cpgModel.AvailableCharacteristcsDetails.Table.Copy().DefaultView;
                    dvTemp.RowFilter = "FEATURE_DESC= '" + cpgModel.FeatureDesc + "'";
                    if (dvTemp.Count > 0)
                    {
                        foreach (DataRowView item in dvTemp)
                        {
                            DataRowView drv = cpgModel.Grd3MeasuringTechniquesDetails.AddNew();
                            drv.BeginEdit();
                            drv["FEATURE"] = item["FEATURE_DESC"];
                            drv["MEASURING_TECHNIQUE"] = item["measuring_technique"];
                            drv["SAMPLE_SIZE"] = item["sample_size"];
                            drv["SAMPLE_FREQUENCY"] = item["sample_frequency"];
                            drv["CONTROL_METHOD"] = item["control_method"];
                            drv["REACTION_PLAN"] = item["reaction_plan"];
                            drv["SNO"] = cpgModel.Grd3MeasuringTechniquesDetails.Count;
                            drv.EndEdit();

                        }
                    }
                    else if (dvTemp.Count == 0)
                    {
                        DataRowView drv = cpgModel.Grd3MeasuringTechniquesDetails.AddNew();
                        drv.BeginEdit();
                        drv["FEATURE"] = cpgModel.FeatureDesc;
                        drv["MEASURING_TECHNIQUE"] = "";
                        drv["SAMPLE_SIZE"] = "0";
                        drv["SAMPLE_FREQUENCY"] = "";
                        drv["CONTROL_METHOD"] = "";
                        drv["REACTION_PLAN"] = "";
                        drv["SNO"] = cpgModel.Grd3MeasuringTechniquesDetails.Count;
                        drv.EndEdit();
                    }
                    // cpgModel.Grd3MeasuringTechniquesDetails = dvTemp;
                }
                else
                {
                    if (cpgModel.Grd3MeasuringTechniquesDetails != null)
                    {
                        DataRowView drv = cpgModel.Grd3MeasuringTechniquesDetails.AddNew();
                        drv.BeginEdit();
                        drv["FEATURE"] = cpgModel.FeatureDesc;
                        drv["MEASURING_TECHNIQUE"] = "";
                        drv["SAMPLE_SIZE"] = "0";
                        drv["SAMPLE_FREQUENCY"] = "";
                        drv["CONTROL_METHOD"] = "";
                        drv["REACTION_PLAN"] = "";
                        drv["SNO"] = 1;
                        drv.EndEdit();
                    }

                }
                //Remove duplicate and SNo

                if (cpgModel.Grd3MeasuringTechniquesDetails.Count > 0)
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp = cpgModel.Grd3MeasuringTechniquesDetails.Table.Clone();
                    for (int i = 0; i < cpgModel.Grd3MeasuringTechniquesDetails.Table.Rows.Count; i++)
                    {
                        dtTemp.DefaultView.RowFilter = "MEASURING_TECHNIQUE='" + cpgModel.Grd3MeasuringTechniquesDetails[i]["MEASURING_TECHNIQUE"] + "'";
                        if (dtTemp.DefaultView.Count == 0)
                        {
                            dtTemp.ImportRow(cpgModel.Grd3MeasuringTechniquesDetails[i].Row);
                            dtTemp.Rows[dtTemp.Rows.Count - 1]["SNO"] = dtTemp.Rows.Count;
                        }
                        dtTemp.DefaultView.RowFilter = string.Empty;
                    }
                    cpgModel.Grd3MeasuringTechniquesDetails = dtTemp.DefaultView;
                }

                //.RowFilter="FEATURE_DESC= feature_desc= '" + cpgModel.FeatureDesc + "'"
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
