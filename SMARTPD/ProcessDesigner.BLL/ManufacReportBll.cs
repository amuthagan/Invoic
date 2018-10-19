using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{

    public class ManufacReportBll : Essential
    {

        public ManufacReportBll(UserInformation userInformation)
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
        public bool GetTabGridDetails(ManufacReportModel manuModel)
        {
            try
            {
                DataTable dt = new DataTable();
                if (manuModel.PartNo.IsNotNullOrEmpty())
                {
                    dt = ToDataTableWithType((from o in DB.MFG_DESIGN
                                              where o.PART_NO == manuModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.SNO, o.ASSUMPTIONS, o.STATUS, o.REMARKS, o.ROWID }).ToList());
                    if (dt != null)
                    {
                        manuModel.DVDesign = dt.DefaultView;
                        DataRowView drv = manuModel.DVDesign.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = manuModel.DVDesign.Count;
                        drv.EndEdit();
                    }
                    else
                    {
                        manuModel.DVDesign = null;
                    }


                    dt = ToDataTableWithType((from o in DB.MFG_DIFFICULTIES
                                              where o.PART_NO == manuModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.SNO, o.DIFFICULTIES, o.ACTION, o.STATUS, o.ROWID }).ToList());
                    if (dt != null)
                    {
                        manuModel.DVDifficulties = dt.DefaultView;
                        DataRowView drv = manuModel.DVDifficulties.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = manuModel.DVDifficulties.Count;
                        drv.EndEdit();
                    }
                    else
                    {
                        manuModel.DVDifficulties = null;
                    }

                    dt = ToDataTableWithType((from o in DB.MFG_PRE_QUAL
                                              where o.PART_NO == manuModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.SNO, o.ISSUES, o.ROWID }).ToList());
                    if (dt != null)
                    {
                        manuModel.DVPreQual = dt.DefaultView;
                        DataRowView drv = manuModel.DVPreQual.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = manuModel.DVPreQual.Count;
                        drv.EndEdit();
                    }
                    else
                    {
                        manuModel.DVPreQual = null;
                    }

                    dt = ToDataTableWithType((from o in DB.MFG_PROCESS
                                              where o.PART_NO == manuModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.SNO, o.MFG_PROCESS_NO, o.ISSUES_FACED, o.ROOT_CAUSE, o.CORRECTIVE_ACTION, o.STATUS, o.ROWID }).ToList());
                    if (dt != null)
                    {
                        manuModel.DVProcess = dt.DefaultView;
                        DataRowView drv = manuModel.DVProcess.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = manuModel.DVProcess.Count;
                        drv.EndEdit();
                    }
                    else
                    {
                        manuModel.DVProcess = null;
                    }

                    dt = ToDataTableWithType((from o in DB.MFG_OUTPUT
                                              where o.PART_NO == manuModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                              select new { o.PART_NO, o.SNO, o.MFG_PROCESS_NO, o.REJECTION, o.REASON, o.CORRECTIVE_ACTION, o.STATUS, o.ROWID }).ToList());
                    if (dt != null)
                    {
                        manuModel.DVOutput = dt.DefaultView;
                        DataRowView drv = manuModel.DVOutput.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = manuModel.DVOutput.Count;
                        drv.EndEdit();
                    }
                    else
                    {
                        manuModel.DVOutput = null;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataView GetProcessSheetCCode(ManufacReportModel manuModel)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = ToDataTableWithType((from o in DB.PROCESS_CC
                                          where o.PART_NO == manuModel.PartNo
                                          select new { o.ROWID, o.PART_NO, o.ROUTE_NO, o.SEQ_NO, o.CC_SNO, COST_CENT_CODE = o.CC_CODE, o.WIRE_SIZE_MIN, o.WIRE_SIZE_MAX, o.OUTPUT }).ToList());

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return dt.DefaultView;

        }
        public DataView GetProcessSheetDetils(ManufacReportModel manuModel)
        {
            try
            {
                DataTable dt = new DataTable();
                DataView dv = new DataView();

                dt = ToDataTable((from o in DB.PROCESS_MAIN
                                  where o.PART_NO == manuModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.ROWID, o.PART_NO, o.ROUTE_NO, o.CURRENT_PROC, o.TKO_CD, o.AJAX_CD, o.RM_CD, o.ALT_RM_CD, o.ALT_RM_CD1, o.WIRE_ROD_CD, o.ALT_WIRE_ROD_CD, o.ALT_WIRE_ROD_CD1, o.CHEESE_WT, o.CHEESE_WT_EST, o.ACTIVE_PART, o.RM_WT }).ToList());

                if (dt != null)
                {
                    dv = dt.DefaultView;
                }
                else
                {
                    dv = null;
                }

                return dv;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<DDCI_INFO> GetCIRefernceByPartNumber(ManufacReportModel manuModelnull)
        {

            List<PRD_CIREF> lstEntity = null;
            List<DDCI_INFO> lstCiEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return null;
                if (manuModelnull.IsNotNullOrEmpty() && manuModelnull.PartNo.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.PRD_CIREF
                                 where row.PART_NO == manuModelnull.PartNo
                                 select row).ToList<PRD_CIREF>();
                    if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                    {
                        lstCiEntity = (from row in DB.DDCI_INFO
                                       where row.CI_REFERENCE == lstEntity[0].CI_REF
                                       select row).ToList<DDCI_INFO>();
                    }
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return lstCiEntity;
        }


        public bool GetManufacturingDetails(ManufacReportModel manuModel)
        {
            try
            {
                MFG_REPORT_MAIN mfgMain = (from o in DB.MFG_REPORT_MAIN
                                           where o.PART_NO == manuModel.PartNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                           select o).FirstOrDefault<MFG_REPORT_MAIN>();

                if (mfgMain != null)
                {
                    manuModel.ActionMode = OperationMode.Edit;
                    manuModel.MACHINE = mfgMain.MACHINE;
                    manuModel.CUST_NAME = mfgMain.CUST_NAME;
                    manuModel.MATERIAL = mfgMain.MATERIAL;
                    manuModel.RM_CD = mfgMain.RM_CD;
                    manuModel.WIRE_SIZE = mfgMain.WIRE_SIZE;
                    manuModel.ROD_DIA = mfgMain.ROD_DIA;
                    manuModel.UTS_YP = mfgMain.UTS_YP;
                    manuModel.HEAT_NO = mfgMain.HEAT_NO;
                    manuModel.COATING = mfgMain.COATING;
                    manuModel.QTY_PLANNED = mfgMain.QTY_PLANNED;
                    manuModel.QTY_FORGED = mfgMain.QTY_FORGED;
                    manuModel.SETTING_SCRAP = mfgMain.SETTING_SCRAP;
                    manuModel.START_DATE = mfgMain.START_DATE;
                    manuModel.END_DATE = mfgMain.END_DATE;
                    manuModel.DURATION = mfgMain.DURATION;
                    manuModel.POST_APPROVAL = mfgMain.POST_APPROVAL;

                    if (mfgMain.POST_APPROVAL.ToString() == "Y")
                    {
                        manuModel.POST_APPR_YES = true;
                    }
                    else if (mfgMain.POST_APPROVAL.ToString() == "N")
                    {
                        manuModel.POST_APPR_NO = true;
                    }
                    else if (mfgMain.POST_APPROVAL.ToString() == "NA")
                    {
                        manuModel.POST_APPR_NA = true;
                    }
                    else
                    {
                        manuModel.POST_APPR_YES = false;
                        manuModel.POST_APPR_NO = false;
                        manuModel.POST_APPR_NA = false;
                    }

                    manuModel.BULK_PRODUCTION = mfgMain.BULK_PRODUCTION;
                    if (mfgMain.BULK_PRODUCTION.ToString() == "Y")
                    {
                        manuModel.BULK_PROD_YES = true;
                    }
                    else if (mfgMain.BULK_PRODUCTION.ToString() == "N")
                    {
                        manuModel.BULK_PROD_NO = true;
                    }
                    else if (mfgMain.BULK_PRODUCTION.ToString() == "NA")
                    {
                        manuModel.BULK_PROD_NA = true;
                    }
                    else
                    {
                        manuModel.BULK_PROD_YES = false;
                        manuModel.BULK_PROD_NO = false;
                        manuModel.BULK_PROD_NA = false;
                    }

                    manuModel.PREPARED_DD = mfgMain.PREPARED_DD;
                    manuModel.FORGING = mfgMain.FORGING;
                    manuModel.TOOL_MANAGEMENT = mfgMain.TOOL_MANAGEMENT;
                    manuModel.QUALITY_ASSURANCE = mfgMain.QUALITY_ASSURANCE;
                    manuModel.OTHERS = mfgMain.OTHERS;

                    manuModel.ActionMode = OperationMode.Edit;
                }
                else
                {
                    manuModel.ActionMode = OperationMode.AddNew;
                    manuModel.MACHINE = "";
                    manuModel.CUST_NAME = "";
                    manuModel.MATERIAL = "";
                    manuModel.RM_CD = "";
                    manuModel.WIRE_SIZE = null;
                    manuModel.ROD_DIA = "";
                    manuModel.UTS_YP = "";
                    manuModel.HEAT_NO = null;
                    manuModel.COATING = "";
                    manuModel.QTY_PLANNED = "";
                    manuModel.QTY_FORGED = "";
                    manuModel.SETTING_SCRAP = "";
                    manuModel.START_DATE = null;
                    manuModel.END_DATE = null;
                    manuModel.DURATION = "";
                    manuModel.POST_APPROVAL = "";
                    manuModel.BULK_PRODUCTION = "";
                    manuModel.PREPARED_DD = "";
                    manuModel.FORGING = "";
                    manuModel.TOOL_MANAGEMENT = "";
                    manuModel.QUALITY_ASSURANCE = "";
                    manuModel.OTHERS = "";
                    manuModel.POST_APPR_YES = false;
                    manuModel.POST_APPR_NO = false;
                    manuModel.POST_APPR_NA = false;
                    manuModel.BULK_PROD_YES = false;
                    manuModel.BULK_PROD_NO = false;
                    manuModel.BULK_PROD_NA = false;
                    manuModel.ActionMode = OperationMode.AddNew;
                }

                GetTabGridDetails(manuModel);
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetPartNoDetails(ManufacReportModel manuModel)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());

                if (dt != null)
                {
                    manuModel.PartNoDetails = dt.DefaultView;
                }
                else
                {
                    manuModel.PartNoDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<DDCUST_MAST> GetCustomerDetails()
        {

            List<DDCUST_MAST> lstEntity = null;
            try
            {
                lstEntity = (from row in DB.DDCUST_MAST
                             where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null)
                             orderby row.CUST_CODE ascending
                             select row).ToList<DDCUST_MAST>();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDRM_MAST> GetRawMaterial()
        {

            List<DDRM_MAST> lstEntity = null;
            try
            {
                lstEntity = (from row in DB.DDRM_MAST
                             where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.RM_CODE.Trim() != ""
                             orderby row.RM_CODE
                             select row).ToList<DDRM_MAST>();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public List<DDCOST_CENT_MAST> GetCCMaster()
        {

            List<DDCOST_CENT_MAST> lstEntity = null;
            try
            {
                lstEntity = (from row in DB.DDCOST_CENT_MAST
                             where (Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false || row.DELETE_FLAG == null) && row.COST_CENT_CODE.Trim() != ""
                             orderby row.COST_CENT_CODE
                             select row).ToList<DDCOST_CENT_MAST>();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public bool SaveManufacturingRpt(ManufacReportModel manuModel)
        {
            bool _status = false;

            try
            {
                DataView dvDesign = manuModel.DVDesign.ToTable().Copy().DefaultView;
                DataView dvDifficulties = manuModel.DVDifficulties.ToTable().Copy().DefaultView;
                DataView dvPreQual = manuModel.DVPreQual.ToTable().Copy().DefaultView;
                DataView dvProcess = manuModel.DVProcess.ToTable().Copy().DefaultView;
                DataView dvOutput = manuModel.DVOutput.ToTable().Copy().DefaultView;

                if (manuModel.PartNo.IsNotNullOrEmpty())
                {

                    #region DevelopmentRptMain

                    MFG_REPORT_MAIN manuRptMain = (from c in DB.MFG_REPORT_MAIN
                                                   where c.PART_NO == manuModel.PartNo
                                                   select c).FirstOrDefault<MFG_REPORT_MAIN>();

                    if (manuRptMain == null)
                    {
                        try
                        {
                            manuRptMain = new MFG_REPORT_MAIN()
                            {
                                PART_NO = manuModel.PartNo,
                                PART_DESC = manuModel.PartNoDesc,
                                MACHINE = manuModel.MACHINE,
                                CUST_NAME = manuModel.CUST_NAME,
                                MATERIAL = manuModel.MATERIAL,
                                RM_CD = manuModel.RM_CD,
                                WIRE_SIZE = manuModel.WIRE_SIZE,
                                ROD_DIA = manuModel.ROD_DIA,
                                UTS_YP = manuModel.UTS_YP,
                                HEAT_NO = manuModel.HEAT_NO,
                                COATING = manuModel.COATING,
                                QTY_PLANNED = manuModel.QTY_PLANNED,
                                QTY_FORGED = manuModel.QTY_FORGED,
                                SETTING_SCRAP = manuModel.SETTING_SCRAP,
                                START_DATE = manuModel.START_DATE,
                                END_DATE = manuModel.END_DATE,
                                DURATION = manuModel.DURATION,
                                POST_APPROVAL = manuModel.POST_APPROVAL,
                                BULK_PRODUCTION = manuModel.BULK_PRODUCTION,
                                PREPARED_DD = manuModel.PREPARED_DD,
                                FORGING = manuModel.FORGING,
                                TOOL_MANAGEMENT = manuModel.TOOL_MANAGEMENT,
                                QUALITY_ASSURANCE = manuModel.QUALITY_ASSURANCE,
                                OTHERS = manuModel.OTHERS,
                                DELETE_FLAG = false,
                                ENTERED_DATE = DateTime.Now,
                                ENTERED_BY = userInformation.UserName,
                                ROWID = Guid.NewGuid()
                            };

                            if (manuModel.POST_APPR_YES)
                            {
                                manuRptMain.POST_APPROVAL = "Y";
                            }
                            else if (manuModel.POST_APPR_NO)
                            {
                                manuRptMain.POST_APPROVAL = "N";
                            }
                            else if (manuModel.POST_APPR_NA)
                            {
                                manuRptMain.POST_APPROVAL = "NA";
                            }

                            if (manuModel.BULK_PROD_YES)
                            {
                                manuRptMain.BULK_PRODUCTION = "Y";
                            }
                            else if (manuModel.BULK_PROD_NO)
                            {
                                manuRptMain.BULK_PRODUCTION = "N";
                            }
                            else if (manuModel.BULK_PROD_NA)
                            {
                                manuRptMain.BULK_PRODUCTION = "NA";
                            }

                            DB.MFG_REPORT_MAIN.InsertOnSubmit(manuRptMain);
                            DB.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.MFG_REPORT_MAIN.DeleteOnSubmit(manuRptMain);
                        }
                    }
                    else
                    {
                        if (manuRptMain != null)
                        {
                            try
                            {
                                manuRptMain.PART_DESC = manuModel.PartNoDesc;
                                manuRptMain.MACHINE = manuModel.MACHINE;
                                manuRptMain.CUST_NAME = manuModel.CUST_NAME;
                                manuRptMain.MATERIAL = manuModel.MATERIAL;
                                manuRptMain.RM_CD = manuModel.RM_CD;
                                manuRptMain.WIRE_SIZE = manuModel.WIRE_SIZE;
                                manuRptMain.ROD_DIA = manuModel.ROD_DIA;
                                manuRptMain.UTS_YP = manuModel.UTS_YP;
                                manuRptMain.HEAT_NO = manuModel.HEAT_NO;
                                manuRptMain.COATING = manuModel.COATING;
                                manuRptMain.QTY_PLANNED = manuModel.QTY_PLANNED;
                                manuRptMain.QTY_FORGED = manuModel.QTY_FORGED;
                                manuRptMain.SETTING_SCRAP = manuModel.SETTING_SCRAP;
                                manuRptMain.START_DATE = manuModel.START_DATE;
                                manuRptMain.END_DATE = manuModel.END_DATE;
                                manuRptMain.DURATION = manuModel.DURATION;
                                manuRptMain.POST_APPROVAL = manuModel.POST_APPROVAL;
                                manuRptMain.BULK_PRODUCTION = manuModel.BULK_PRODUCTION;
                                manuRptMain.PREPARED_DD = manuModel.PREPARED_DD;
                                manuRptMain.FORGING = manuModel.FORGING;
                                manuRptMain.TOOL_MANAGEMENT = manuModel.TOOL_MANAGEMENT;
                                manuRptMain.QUALITY_ASSURANCE = manuModel.QUALITY_ASSURANCE;
                                manuRptMain.OTHERS = manuModel.OTHERS;
                                manuRptMain.DELETE_FLAG = false;
                                manuRptMain.UPDATED_DATE = DateTime.Now;
                                manuRptMain.UPDATED_BY = userInformation.UserName;

                                if (manuModel.POST_APPR_YES)
                                {
                                    manuRptMain.POST_APPROVAL = "Y";
                                }
                                else if (manuModel.POST_APPR_NO)
                                {
                                    manuRptMain.POST_APPROVAL = "N";
                                }
                                else if (manuModel.POST_APPR_NA)
                                {
                                    manuRptMain.POST_APPROVAL = "NA";
                                }

                                if (manuModel.BULK_PROD_YES)
                                {
                                    manuRptMain.BULK_PRODUCTION = "Y";
                                }
                                else if (manuModel.BULK_PROD_NO)
                                {
                                    manuRptMain.BULK_PRODUCTION = "N";
                                }
                                else if (manuModel.BULK_PROD_NA)
                                {
                                    manuRptMain.BULK_PRODUCTION = "NA";
                                }

                                DB.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                                DB.MFG_REPORT_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, manuRptMain);
                            }

                        }
                    }
                    _status = true;
                    #endregion

                    #region DesignAssumptionDetails

                    List<MFG_DESIGN> lstexistingDatas = new List<MFG_DESIGN>();
                    lstexistingDatas = ((from c in DB.MFG_DESIGN
                                         where c.PART_NO == manuModel.PartNo
                                         select c).ToList());
                    if (lstexistingDatas.Count > 0)
                    {
                        DB.MFG_DESIGN.DeleteAllOnSubmit(lstexistingDatas);
                        DB.SubmitChanges();
                    }

                    dvDesign.RowFilter = "ASSUMPTIONS <> '' OR STATUS <>'' OR REMARKS <> ''";
                    int rown = 1;
                    foreach (DataRowView drv in dvDesign)
                    {
                        MFG_DESIGN design = null;
                        try
                        {

                            design = new MFG_DESIGN()
                                  {
                                      PART_NO = manuModel.PartNo.ToString(),
                                      SNO = rown,
                                      ASSUMPTIONS = drv["ASSUMPTIONS"].ToString(),
                                      STATUS = drv["STATUS"].ToString(),
                                      REMARKS = drv["REMARKS"].ToString(),
                                      DELETE_FLAG = false,
                                      ENTERED_DATE = DateTime.Now,
                                      ENTERED_BY = userInformation.UserName,
                                      ROWID = Guid.NewGuid()
                                  };
                            DB.MFG_DESIGN.InsertOnSubmit(design);
                            DB.SubmitChanges();
                            rown = rown + 1;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.MFG_DESIGN.DeleteOnSubmit(design);
                            rown = rown + 1;
                        }

                    }
                    _status = true;
                    #endregion

                    #region DifficultiesDetails

                    List<MFG_DIFFICULTIES> lstexistingDifficulties = new List<MFG_DIFFICULTIES>();
                    lstexistingDifficulties = ((from c in DB.MFG_DIFFICULTIES
                                                where c.PART_NO == manuModel.PartNo
                                                select c).ToList());
                    if (lstexistingDifficulties.Count > 0)
                    {
                        DB.MFG_DIFFICULTIES.DeleteAllOnSubmit(lstexistingDifficulties);
                        DB.SubmitChanges();
                    }

                    dvDifficulties.RowFilter = "DIFFICULTIES <> '' OR ACTION <>'' OR STATUS <> ''";
                    rown = 1;
                    foreach (DataRowView drv in dvDifficulties)
                    {
                        MFG_DIFFICULTIES difficulties = null;
                        try
                        {
                            difficulties = new MFG_DIFFICULTIES()
                            {
                                PART_NO = manuModel.PartNo.ToString(),
                                SNO = rown,
                                DIFFICULTIES = drv["DIFFICULTIES"].ToString(),
                                ACTION = drv["ACTION"].ToString(),
                                STATUS = drv["STATUS"].ToString(),
                                DELETE_FLAG = false,
                                ENTERED_DATE = DateTime.Now,
                                ENTERED_BY = userInformation.UserName,
                                ROWID = Guid.NewGuid()
                            };
                            DB.MFG_DIFFICULTIES.InsertOnSubmit(difficulties);
                            DB.SubmitChanges();
                            rown = rown + 1;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.MFG_DIFFICULTIES.DeleteOnSubmit(difficulties);
                            rown = rown + 1;
                        }

                    }
                    _status = true;
                    #endregion

                    #region PreQualDetails

                    List<MFG_PRE_QUAL> lstexistingPreQual = new List<MFG_PRE_QUAL>();
                    lstexistingPreQual = ((from c in DB.MFG_PRE_QUAL
                                           where c.PART_NO == manuModel.PartNo
                                           select c).ToList());
                    if (lstexistingPreQual.Count > 0)
                    {
                        DB.MFG_PRE_QUAL.DeleteAllOnSubmit(lstexistingPreQual);
                        DB.SubmitChanges();
                    }

                    dvPreQual.RowFilter = "ISSUES <> ''";
                    rown = 1;
                    foreach (DataRowView drv in dvPreQual)
                    {
                        MFG_PRE_QUAL prequal = null;
                        try
                        {
                            prequal = new MFG_PRE_QUAL()
                            {
                                PART_NO = manuModel.PartNo.ToString(),
                                SNO = rown,
                                ISSUES = drv["ISSUES"].ToString(),
                                DELETE_FLAG = false,
                                ENTERED_DATE = DateTime.Now,
                                ENTERED_BY = userInformation.UserName,
                                ROWID = Guid.NewGuid()
                            };
                            DB.MFG_PRE_QUAL.InsertOnSubmit(prequal);
                            DB.SubmitChanges();
                            rown = rown + 1;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.MFG_PRE_QUAL.DeleteOnSubmit(prequal);
                            rown = rown + 1;
                        }

                    }
                    _status = true;
                    #endregion

                    #region ProcessDetails

                    List<MFG_PROCESS> lstexistingProcess = new List<MFG_PROCESS>();
                    lstexistingProcess = ((from c in DB.MFG_PROCESS
                                           where c.PART_NO == manuModel.PartNo
                                           select c).ToList());
                    if (lstexistingProcess.Count > 0)
                    {
                        DB.MFG_PROCESS.DeleteAllOnSubmit(lstexistingProcess);
                        DB.SubmitChanges();
                    }

                    dvProcess.RowFilter = "ISSUES_FACED <> '' OR ROOT_CAUSE <>'' OR CORRECTIVE_ACTION <> '' OR STATUS <> ''";
                    rown = 1;
                    foreach (DataRowView drv in dvProcess)
                    {
                        MFG_PROCESS process = null;
                        try
                        {
                            process = new MFG_PROCESS()
                            {
                                PART_NO = manuModel.PartNo.ToString(),
                                SNO = rown,
                                MFG_PROCESS_NO = drv["MFG_PROCESS_NO"].ToString(),
                                ISSUES_FACED = drv["ISSUES_FACED"].ToString(),
                                ROOT_CAUSE = drv["ROOT_CAUSE"].ToString(),
                                CORRECTIVE_ACTION = drv["CORRECTIVE_ACTION"].ToString(),
                                STATUS = drv["STATUS"].ToString(),
                                DELETE_FLAG = false,
                                ENTERED_DATE = DateTime.Now,
                                ENTERED_BY = userInformation.UserName,
                                ROWID = Guid.NewGuid()
                            };
                            DB.MFG_PROCESS.InsertOnSubmit(process);
                            DB.SubmitChanges();
                            rown = rown + 1;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.MFG_PROCESS.DeleteOnSubmit(process);
                            rown = rown + 1;
                        }

                    }
                    _status = true;
                    #endregion

                    #region OutputDetails

                    List<MFG_OUTPUT> lstexistingOutput = new List<MFG_OUTPUT>();
                    lstexistingOutput = ((from c in DB.MFG_OUTPUT
                                          where c.PART_NO == manuModel.PartNo
                                          select c).ToList());
                    if (lstexistingOutput.Count > 0)
                    {
                        DB.MFG_OUTPUT.DeleteAllOnSubmit(lstexistingOutput);
                        DB.SubmitChanges();
                    }

                    dvOutput.RowFilter = "REJECTION <> '' OR REASON <>'' OR CORRECTIVE_ACTION <> '' OR STATUS <> ''";
                    rown = 1;
                    foreach (DataRowView drv in dvOutput)
                    {
                        MFG_OUTPUT output = null;
                        try
                        {
                            output = new MFG_OUTPUT()
                            {
                                PART_NO = manuModel.PartNo.ToString(),
                                SNO = rown,
                                MFG_PROCESS_NO = drv["MFG_PROCESS_NO"].ToString(),
                                REJECTION = drv["REJECTION"].ToString(),
                                REASON = drv["REASON"].ToString(),
                                CORRECTIVE_ACTION = drv["CORRECTIVE_ACTION"].ToString(),
                                STATUS = drv["STATUS"].ToString(),
                                DELETE_FLAG = false,
                                ENTERED_DATE = DateTime.Now,
                                ENTERED_BY = userInformation.UserName,
                                ROWID = Guid.NewGuid()
                            };
                            DB.MFG_OUTPUT.InsertOnSubmit(output);
                            DB.SubmitChanges();
                            rown = rown + 1;

                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.MFG_OUTPUT.DeleteOnSubmit(output);
                            rown = rown + 1;
                        }

                    }
                    _status = true;
                    #endregion

                }
                return _status;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        #region PrintReport

        public DataSet GetManufactrptDetails(string partno)
        {
            try
            {
                DataSet dsManufactrpt = new DataSet("ManufactDetails");
                DataTable dtMainReport = new DataTable("MainReport");
                DataTable dtDesign = new DataTable("Design");
                DataTable dtDifficulties = new DataTable("Difficulties");
                DataTable dtPreQual = new DataTable("PreQual");
                DataTable dtProcess = new DataTable("Process");
                DataTable dtOutput = new DataTable("Output");

                dtMainReport = ToDataTableWithType((from o in DB.MFG_REPORT_MAIN
                                                    where o.PART_NO == partno && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                    select o).ToList());
                dtMainReport.TableName = "MainReport";


                dtDesign = ToDataTableWithType((from o in DB.MFG_DESIGN
                                                where o.PART_NO == partno && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                select new { o.PART_NO, o.SNO, o.ASSUMPTIONS, o.STATUS, o.REMARKS, o.ROWID }).ToList());
                dtDesign.TableName = "Design";

                dtDifficulties = ToDataTableWithType((from o in DB.MFG_DIFFICULTIES
                                                      where o.PART_NO == partno && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                      select new { o.PART_NO, o.SNO, o.DIFFICULTIES, o.ACTION, o.STATUS, o.ROWID }).ToList());
                dtDifficulties.TableName = "Difficulties";

                dtPreQual = ToDataTableWithType((from o in DB.MFG_PRE_QUAL
                                                 where o.PART_NO == partno && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                 select new { o.PART_NO, o.SNO, o.ISSUES, o.ROWID }).ToList());
                dtPreQual.TableName = "PreQual";

                dtProcess = ToDataTableWithType((from o in DB.MFG_PROCESS
                                                 where o.PART_NO == partno && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                 select new { o.PART_NO, o.SNO, o.MFG_PROCESS_NO, o.ISSUES_FACED, o.ROOT_CAUSE, o.CORRECTIVE_ACTION, o.STATUS, o.ROWID }).ToList());
                dtProcess.TableName = "Process";

                dtOutput = ToDataTableWithType((from o in DB.MFG_OUTPUT
                                                where o.PART_NO == partno && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                                select new { o.PART_NO, o.SNO, o.MFG_PROCESS_NO, o.REJECTION, o.REASON, o.CORRECTIVE_ACTION, o.STATUS, o.ROWID }).ToList());
                dtOutput.TableName = "Output";

                dsManufactrpt.Tables.Add(dtDesign);
                dsManufactrpt.Tables.Add(dtDifficulties);
                dsManufactrpt.Tables.Add(dtMainReport);
                dsManufactrpt.Tables.Add(dtOutput);
                dsManufactrpt.Tables.Add(dtPreQual);
                dsManufactrpt.Tables.Add(dtProcess);

                return dsManufactrpt;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        #endregion

    }
}
