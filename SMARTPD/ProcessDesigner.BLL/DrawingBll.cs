using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class DrawingBll : Essential
    {
        public DrawingBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataView GetPartNumberDetails()
        {

            //string getquery = "select part_no,part_desc from prd_mast where delete_flag is null or delete_flag = 0 ";
            //var getcollection = ToDataTable(DB.ExecuteQuery<DrawingModel>(getquery).ToList());
            //return getcollection.DefaultView;

            DataTable dt = new DataTable();
            dt = ToDataTable((from o in DB.PRD_MAST
                              where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                              select new { o.PART_NO, o.PART_DESC }).ToList());
            return dt.DefaultView;
        }

        public DataView GetDrawingTypeDetails()
        {
            //string getquery = "select dwg_type_desc from dwg_type;";
            //var getcollection = ToDataTable(DB.ExecuteQuery<DrawingModel>(getquery).ToList());
            //return getcollection.DefaultView;

            DataTable dt = new DataTable();
            dt = ToDataTable((from o in DB.DWG_TYPE
                              select new { o.DWG_TYPE_DESC }).ToList());
            return dt.DefaultView;
        }

        public DataView GetProductDrawingDetails(string partNo, string drwType)
        {
            //string GetQuery = "select pd.part_no,pdis.dwg_type,pdis.issue_no,pdis.issue_date,pdis.issue_alter,pd.PAGE_NO,pd.PRD_DWG,pdis.compiled_by"
            //+ " from prd_dwg_issue pdis "
            //+ " join prod_drawing pd on pd.PART_NO = pdis.PART_NO "
            //+ " where pd.part_no ='" + partNo + "' and pdis.dwg_type ='" + drwType + "'";   
            string getquery = "select part_no,dwg_type,PAGE_NO,PRD_DWG,PROD_DRW_IMAGE from prod_drawing "
            + " where part_no ='" + partNo + "' and dwg_type ='" + drwType + "'";
            var getcollection = ToDataTable(DB.ExecuteQuery<DrawingModel>(getquery).ToList());
            return getcollection.DefaultView;
        }

        public DataView GetRevisionDetails(string partNo, string drwType)
        {
            string getquery = "select part_no,dwg_type,issue_no,issue_date,issue_alter,compiled_by,Loc_Code from prd_dwg_issue "
             + " where part_no ='" + partNo + "' and dwg_type ='" + drwType + "'";
            var getcollection = ToDataTableWithType(DB.ExecuteQuery<DrawingModel>(getquery).ToList());
            DataRow drRow = getcollection.NewRow();
            getcollection.Rows.Add(drRow);
            return getcollection.DefaultView;
        }

        public DataView GetRevisionDetailsWithOutNewRow(string partNo, string drwType)
        {
            string getquery = "select part_no,dwg_type,issue_no,issue_date,issue_alter,compiled_by,Loc_Code from prd_dwg_issue "
             + " where part_no ='" + partNo + "' and dwg_type ='" + drwType + "'";
            var getcollection = ToDataTableWithType(DB.ExecuteQuery<DrawingModel>(getquery).ToList());
            return getcollection.DefaultView;
        }
        public DataView GetProductDrawing(string partNo, string drwType, int page_no)
        {
            string getquery = "select part_no,dwg_type,PAGE_NO,PRD_DWG,PROD_DRW_IMAGE from prod_drawing "
           + " where part_no ='" + partNo + "' and dwg_type ='" + drwType + "'";
            if (page_no > 0)
            {
                getquery += " and PAGE_NO ='" + page_no + "'";
            }
            var getcollection = ToDataTable(DB.ExecuteQuery<DrawingModel>(getquery).ToList());
            return getcollection.DefaultView;
        }

        public System.IO.MemoryStream ShowImage(string partNo, decimal drwType, int issueNo)
        {

            PROD_DRAWING ddDrawing = null;

            byte[] photosource = null;
            System.IO.MemoryStream strm;
            try
            {
                ddDrawing = (from c in DB.PROD_DRAWING
                             where c.PART_NO == partNo && c.DWG_TYPE == drwType && c.PAGE_NO == issueNo
                             select c).SingleOrDefault<PROD_DRAWING>();
                if (ddDrawing.IsNotNullOrEmpty())
                {
                    if (ddDrawing.PROD_DRW_IMAGE != null)
                    {
                        photosource = ddDrawing.PROD_DRW_IMAGE.ToArray();
                        strm = new System.IO.MemoryStream(photosource);
                        return strm;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return null;

            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.PROD_DRAWING.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddDrawing);

                return null;
            }
        }

        public bool DeleteDrawingDetails(DrawingModel1 drwgModel)
        {
            bool _status = false;
            drwgModel.Status = "";
            try
            {
                if (drwgModel.PART_NO != "")
                {
                    int dwg_type = (drwgModel.DWG_TYPE_DESC == "Sequence Drawing") ? 1 : 0;
                    PROD_DRAWING prddwgIssues = (from o in DB.PROD_DRAWING
                                                 where o.PART_NO == drwgModel.PART_NO && o.DWG_TYPE == dwg_type && o.PAGE_NO == drwgModel.PAGE_NO
                                                 select o).SingleOrDefault<PROD_DRAWING>();

                    if (prddwgIssues != null)
                    {
                        DB.PROD_DRAWING.DeleteOnSubmit(prddwgIssues);
                        DB.SubmitChanges();
                        drwgModel.Status = PDMsg.DeletedSuccessfully;
                        _status = true;
                    }

                    List<PROD_DRAWING> updatePage = (from o in DB.PROD_DRAWING
                                                     where o.PART_NO == drwgModel.PART_NO && o.DWG_TYPE == dwg_type
                                                     select o).OrderBy(item => item.PAGE_NO).ToList();
                    int pageNo = 1;
                    foreach (PROD_DRAWING dr in updatePage)
                    {
                        //PROD_DRAWING updatePageNumber = (from o in DB.PROD_DRAWING
                        //                                 where o.PART_NO == dr.PART_NO && o.DWG_TYPE == dr.DWG_TYPE && o.PAGE_NO == dr.PAGE_NO
                        //                                 select o).FirstOrDefault<PROD_DRAWING>();
                        //updatePageNumber.PAGE_NO = pageNo;
                        //DB.SubmitChanges();
                        string query = "UPDATE PROD_DRAWING set PAGE_NO='" + pageNo + "' WHERE PART_NO='" + dr.PART_NO + "' AND DWG_TYPE='" + dr.DWG_TYPE + "' AND PAGE_NO='" + dr.PAGE_NO + "'";
                        IEnumerable<int> result = DB.ExecuteQuery<int>(query);
                        DB.SubmitChanges();
                        pageNo++;
                    }
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                _status = true;

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return _status;
        }

        public bool DeleteIssueDetails(DrawingModel1 drwgModel)
        {
            bool _status = false;
            drwgModel.Status = "";
            try
            {
                if (drwgModel.SelectedItem["ISSUE_NO"].ToString() != "")
                {
                    int dwg_type = (drwgModel.DWG_TYPE_DESC == "Sequence Drawing") ? 1 : 0;
                    PRD_DWG_ISSUE prddwgIssues = (from o in DB.PRD_DWG_ISSUE
                                                  where o.ISSUE_NO == Convert.ToDecimal(drwgModel.SelectedItem["ISSUE_NO"].ToString()) && o.PART_NO == drwgModel.PART_NO && o.DWG_TYPE == dwg_type
                                                  select o).SingleOrDefault<PRD_DWG_ISSUE>();

                    if (prddwgIssues != null)
                    {
                        DB.PRD_DWG_ISSUE.DeleteOnSubmit(prddwgIssues);
                        DB.SubmitChanges();
                        _status = true;
                    }
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                _status = true;

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return _status;
        }

        public bool GetDrawingModel(DrawingModel1 drwgModel)
        {
            try
            {
                DataTable dt = new DataTable();
                string getquery = "select pd.part_no,pdis.dwg_type,pdis.issue_no,pdis.issue_date,pdis.issue_alter,pd.PAGE_NO,pd.PRD_DWG"
                                   + " from prd_dwg_issue pdis "
                                   + " join prod_drawing pd on pd.PART_NO = pdis.PART_NO "
                                   + " where pd.part_no ='0' and pdis.dwg_type ='0'";

                dt = ToDataTable(DB.ExecuteQuery<DrawingModel>(getquery).ToList());
                if (dt != null)
                {
                    drwgModel.DVType = dt.DefaultView;
                    drwgModel.DVType.AddNew();
                }
                else
                {
                    drwgModel.DVType = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool InsertDrawingDetails(DrawingModel1 drawingModel)
        {
            try
            {
                if (drawingModel.Mode == "Add")
                {
                    PROD_DRAWING fastnersM = (from o in DB.PROD_DRAWING
                                              where o.PART_NO == drawingModel.PART_NO
                                              select o).FirstOrDefault<PROD_DRAWING>();
                    int dwg_type = (drawingModel.DWG_TYPE_DESC == "Sequence Drawing") ? 1 : 0;
                    if (fastnersM == null)
                    {
                        fastnersM = new PROD_DRAWING();
                        fastnersM.PART_NO = drawingModel.PART_NO;
                        fastnersM.PRD_DWG = drawingModel.FilePath;
                        fastnersM.DWG_TYPE = dwg_type;
                        fastnersM.PAGE_NO = 1;
                        fastnersM.PROD_DRW_IMAGE = drawingModel.ImageByte;
                        fastnersM.ROWID = Guid.NewGuid();
                        DB.PROD_DRAWING.InsertOnSubmit(fastnersM);
                        DB.SubmitChanges();
                        fastnersM = null;
                        drawingModel.Status = PDMsg.SavedSuccessfully;
                    }
                    else
                    {
                        PROD_DRAWING objProdDwg = new PROD_DRAWING();
                        //var duplicates = (from o in DB.PROD_DRAWING
                        //                  where o.PART_NO == drawingModel.PART_NO && o.DWG_TYPE == dwg_type
                        //                  select o).AsEnumerable().GroupBy(i => new { PAGE_NO = i.PAGE_NO }).Where(g => g.Count() > 0).Select(g => new { g.Key.PAGE_NO }).ToList();
                        string query = "SELECT Count(*) As Counts FROM prod_drawing WHERE PART_NO='" + drawingModel.PART_NO + "' AND DWG_TYPE='" + dwg_type + "'";
                        IEnumerable<int> result = DB.ExecuteQuery<int>(query).ToList();
                        DB.SubmitChanges();
                        if (result != null && result.Count() > 0)
                        {
                            int get = result.Max();
                            objProdDwg.PAGE_NO = get + 1;
                        }
                        else
                        {
                            objProdDwg.PAGE_NO = 1;
                        }
                        objProdDwg.PART_NO = drawingModel.PART_NO;
                        objProdDwg.PRD_DWG = drawingModel.FilePath;
                        objProdDwg.DWG_TYPE = dwg_type;
                        objProdDwg.PROD_DRW_IMAGE = drawingModel.ImageByte;
                        objProdDwg.ROWID = Guid.NewGuid();
                        DB.PROD_DRAWING.InsertOnSubmit(objProdDwg);
                        DB.SubmitChanges();
                        objProdDwg = null;
                        drawingModel.Status = PDMsg.SavedSuccessfully;
                    }
                }

                if (drawingModel.Mode == "Edit")
                {
                    PROD_DRAWING prdDwgIssue = (from o in DB.PROD_DRAWING
                                                where o.PART_NO == drawingModel.PART_NO && o.DWG_TYPE == drawingModel.DWG_TYPE && o.PAGE_NO == drawingModel.PAGE_NO
                                                select o).FirstOrDefault<PROD_DRAWING>();
                    if (prdDwgIssue != null)
                    {
                        prdDwgIssue.PART_NO = drawingModel.PART_NO;
                        prdDwgIssue.DWG_TYPE = drawingModel.DWG_TYPE;
                        prdDwgIssue.PAGE_NO = drawingModel.PAGE_NO;
                        prdDwgIssue.PRD_DWG = drawingModel.FilePath;
                        prdDwgIssue.PROD_DRW_IMAGE = drawingModel.ImageByte;
                        DB.SubmitChanges();
                        prdDwgIssue = null;
                        drawingModel.Status = PDMsg.UpdatedSuccessfully;
                    }
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                drawingModel.Status = PDMsg.UpdatedSuccessfully;
                return false;

            }
            catch (Exception ex)
            {
                DB.Transaction.Rollback();
                ex.LogException();
                return false;

            }
            drawingModel.Status = PDMsg.UpdatedSuccessfully;
            return true;
        }

        public bool InsertDrawingRevisionDetails(DrawingModel1 drawingModel, DataView dV_PROD_DWG_ISSUE)
        {
            PRD_DWG_ISSUE prdDwgIssue = new PRD_DWG_ISSUE();
            bool insert = false;
            bool update = false;
            bool submit = false;

            try
            {
                int dwg_type = (drawingModel.DWG_TYPE_DESC == "Sequence Drawing") ? 1 : 0;

                List<PRD_DWG_ISSUE> lstexistingDatas = new List<PRD_DWG_ISSUE>();
                lstexistingDatas = ((from o in DB.PRD_DWG_ISSUE
                                     where o.PART_NO == drawingModel.PART_NO && o.DWG_TYPE == dwg_type
                                     select o).ToList());
                if (lstexistingDatas.Count > 0)
                {
                    DB.PRD_DWG_ISSUE.DeleteAllOnSubmit(lstexistingDatas);
                    DB.SubmitChanges();
                }

                foreach (DataRow dr in dV_PROD_DWG_ISSUE.ToTable().AsEnumerable())
                {
                    if (dr["ISSUE_NO"].ToString() != "")
                    {

                        prdDwgIssue = (from o in DB.PRD_DWG_ISSUE
                                       where o.ISSUE_NO == Convert.ToDecimal(dr["issue_No"].ToString()) && o.PART_NO == drawingModel.PART_NO && o.DWG_TYPE == dwg_type
                                       select o).FirstOrDefault<PRD_DWG_ISSUE>();
                        if (prdDwgIssue == null)
                        {
                            prdDwgIssue = new PRD_DWG_ISSUE();
                            prdDwgIssue.PART_NO = drawingModel.PART_NO;
                            prdDwgIssue.DWG_TYPE = dwg_type;
                            prdDwgIssue.Loc_Code = dr["Loc_Code"].ToValueAsString();
                            prdDwgIssue.ISSUE_NO = Convert.ToDecimal(dr["issue_No"]);
                            prdDwgIssue.ISSUE_DATE = Convert.ToDateTime(dr["ISSUE_DATE"]);
                            prdDwgIssue.ISSUE_ALTER = dr["issue_alter"].ToString();
                            prdDwgIssue.COMPILED_BY = dr["COMPILED_BY"].ToString();
                            insert = true;
                            DB.PRD_DWG_ISSUE.InsertOnSubmit(prdDwgIssue);
                            submit = true;
                            DB.SubmitChanges();
                        }
                        else
                        {
                            prdDwgIssue.PART_NO = drawingModel.PART_NO;
                            prdDwgIssue.DWG_TYPE = dwg_type;
                            prdDwgIssue.Loc_Code = dr["Loc_Code"].ToValueAsString();
                            prdDwgIssue.ISSUE_NO = Convert.ToDecimal(dr["issue_No"]);
                            prdDwgIssue.ISSUE_DATE = Convert.ToDateTime(dr["ISSUE_DATE"]);
                            prdDwgIssue.ISSUE_ALTER = dr["issue_alter"].ToString();
                            prdDwgIssue.COMPILED_BY = dr["COMPILED_BY"].ToString();
                            update = true;
                            submit = true;
                            DB.SubmitChanges();
                        }
                    }
                }
                //DB.Transaction.Commit();              
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                drawingModel.Status = PDMsg.UpdatedSuccessfully;
                return false;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (insert == true)
                    {
                        DB.PRD_DWG_ISSUE.DeleteOnSubmit(prdDwgIssue);
                    }
                    if (update == true)
                    {
                        DB.PRD_DWG_ISSUE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, prdDwgIssue);
                    }
                }
                //DB.Transaction.Rollback();
                ex.LogException();
                return false;
            }
            drawingModel.Status = PDMsg.UpdatedSuccessfully;
            return true;
        }

        #region "ECN"
        /// <summary>
        /// Get the list of users where designation != 'MANAGER' and 'GM'
        /// </summary>
        /// <returns></returns>
        public DataView GetCompiledByCombo()
        {
            DataView dvEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return dvEntity;
                dvEntity = ToDataTable((from row in DB.SEC_USER_MASTER
                                        where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                        && row.DESIGNATION.ToUpper().Trim() != "MANAGER" && row.DESIGNATION.ToUpper().Trim() != "GM"
                                        select row).ToList()).DefaultView;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return null;

            }
            catch (Exception ex1)
            {
                ex1.LogException();
            }
            return dvEntity;
        }

        public DataView GetApprovedByCombo()
        {
            DataView dvEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return dvEntity;
                dvEntity = ToDataTable((from row in DB.SEC_USER_MASTER
                                        where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                                        && row.DESIGNATION.ToUpper().Trim() == "SENIOR MANAGER" || row.DESIGNATION.ToUpper().Trim() == "AGM-DND"
                                        select row).ToList()).DefaultView;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return null;

            }
            catch (Exception ex1)
            {
                ex1.LogException();
                return null;
            }
            return dvEntity;
        }

        /// <summary>
        /// ACTUAL_CHANGE_IMP Column to be updated
        /// </summary>
        /// <returns></returns>
        public DataTable GetActualChangeImplementCombo()
        {
            DataTable dtEntity = new DataTable();
            try
            {
                dtEntity.Columns.Add("ACTUAL_CHANGE_IMP");
                dtEntity.Rows.Add("STD");
                dtEntity.Rows.Add("SPL");
                dtEntity.Rows.Add("SEC");
                dtEntity.Rows.Add("S/C");
                dtEntity.Rows.Add("PROCESS");
                dtEntity.Rows.Add("DESPATCH");
            }
            catch (Exception ex1)
            {
                ex1.LogException();
            }
            return dtEntity;
        }

        /// <summary>
        /// CHANGE_EFFECTIVE Column to be updated
        /// </summary>
        /// <returns></returns>
        public DataTable GetProposedImplementCombo()
        {
            //List<String> lstEntity = new List<String>();
            DataTable dtEntity = new DataTable();
            try
            {
                dtEntity.Columns.Add("CHANGE_EFFECTIVE");
                dtEntity.Rows.Add("IMMEDIATE");
                dtEntity.Rows.Add("FROM NEXT FORGING");
                dtEntity.Rows.Add("ALREADY IN EFFECT");
                dtEntity.Rows.Add("FROM NEXT H.T.CYCLE");

            }
            catch (Exception ex1)
            {
                ex1.LogException();
            }
            return dtEntity;
        }


        /// <summary>
        /// type="ECN" - CUSTOMER  , type="PCN"   SFL
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DD_PCN> GetECNMPSDetails(decimal sNo, string screentype, ref string referenceNo)
        {
            List<DD_PCN> lstEntity = new List<DD_PCN>();
            // DD_PCN lstEntity = null;
            string requestedBy = "";
            try
            {
                requestedBy = (screentype == "MPS" ? "SFL" : "CUSTOMER");
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                IsDefaultSubmitRequired = false;
                lstEntity = (from row in DB.DD_PCN
                             where
                             row.SNO == sNo
                             && row.REQUSTED_BY.ToUpper() == requestedBy
                             select row).ToList<DD_PCN>();
                DB.DD_PCN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstEntity);
                if (lstEntity == null || lstEntity.Count == 0)
                {
                    lstEntity = null;
                    DD_PCN lstPCN = new DD_PCN();
                    lstEntity = new List<DD_PCN>();
                    lstPCN.PART_NO = "";
                    lstPCN.ACTUAL_CHANGE_IMP = "";
                    lstPCN.APPROVED_BY = "";
                    lstPCN.CHANGE_EFFECTIVE = "";
                    lstPCN.COMPILED_BY = "";
                    lstPCN.CONTROL_PLAN = false;
                    lstPCN.COST_DESC = "";
                    lstPCN.CUST_NAME = "";
                    lstPCN.CUST_PART_NO = "";
                    lstPCN.DATE_OF_PCN = null;
                    lstPCN.DATE_OF_SIGN = null;
                    lstPCN.DISPOSITION = "";
                    lstPCN.GAUGE_DWG = null;
                    lstPCN.INFGG_INITIAL = "";
                    lstPCN.INFGG_QTY = "";
                    lstPCN.INHEAT_TREATMENT_INITIAL = "";
                    lstPCN.INHEAT_TREATMENT_QTY = "";
                    lstPCN.INWIP_INITIAL = "";
                    lstPCN.INWIP_QTY = "";
                    lstPCN.MANUFACTURE_PROCESS = "";
                    lstPCN.NATURE_OF_CHANGE = "";
                    lstPCN.OTHERS = false;
                    lstPCN.PART_DESC = "";
                    lstPCN.PFD = false;
                    lstPCN.PFMEA = false;
                    lstPCN.PRODUCT_CHANGE_NO = "";
                    lstPCN.PRODUCT_DWG = false;
                    lstPCN.RE_PPAP = false;
                    lstPCN.RESON_FOR_CHANGE = "";
                    lstPCN.SEQUENCE_DWG = false;
                    lstPCN.SFL_DRAW_ISSUE_DATE = null;
                    lstPCN.SFL_DRAW_ISSUEDATE1 = null;
                    lstPCN.SFL_DRAW_ISSUENO = null;
                    lstPCN.SFL_DRAW_ISSUENO1 = null;
                    //lstPCN.SNO = ;
                    lstPCN.TOOL_DWG = false;
                    lstEntity.Add(lstPCN);

                }
                GetCustomerDetails(lstEntity[0]);
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return null;

            }
            catch (Exception ex1)
            {
                DB.DD_PCN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstEntity[0]);
                ex1.LogException();
            }
            return lstEntity;
        }

        /// <summary>
        /// type="ECN" - CUSTOMER  , type="PCN"   SFL
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataView GetDDPCNList(string screentype)
        {
            DataView dvEntity = new DataView();
            string requestedBy = "";
            try
            {
                requestedBy = (screentype == "MPS" ? "SFL" : "CUSTOMER");
                if (!DB.IsNotNullOrEmpty()) return dvEntity;
                dvEntity = ToDataTable((from row in DB.DD_PCN
                                        where
                                        row.REQUSTED_BY.ToUpper().Trim() == requestedBy.ToUpper().Trim()
                                        orderby row.PART_NO, row.PRODUCT_CHANGE_NO
                                        select new { row.PART_NO, row.PRODUCT_CHANGE_NO, SFL_DRAW_ISSUEDATE1 = row.SFL_DRAW_ISSUEDATE1.ToFormattedDateAsString("dd/MM/yyyy"), row.SNO, row.PART_DESC }).ToList()).DefaultView;
                return dvEntity;
                //part_no || product_change_no,part_no as PartNo, product_change_no as ChangeNoteNo,sfl_draw_issuedate1 as ECN_Date
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return null;

            }
            catch (Exception ex1)
            {
                ex1.LogException();
            }
            return dvEntity;
        }


        public bool SaveECNMPSDetails(DD_PCN lstpcn, string partNo, string screentype, OperationMode actionMode)
        {
            DD_PCN lstEntity = new DD_PCN();
            string requestedBy = "";
            bool add = false;
            bool edit = false;
            bool submit = false;
            try
            {
                requestedBy = (screentype == "MPS" ? "SFL" : "CUSTOMER");
                if (actionMode == OperationMode.Edit)
                {
                    edit = true;
                    lstEntity = (from row in DB.DD_PCN
                                 where
                                 row.SNO == lstpcn.SNO
                                 && row.REQUSTED_BY.ToUpper() == requestedBy.ToUpper()
                                 select row).FirstOrDefault();
                    if (lstEntity == null)
                    {
                        var maxId = DB.DD_PCN.Max(x => x.SNO);
                        maxId = maxId + 1;
                        lstEntity = new DD_PCN();
                        lstEntity.SNO = maxId;
                        lstEntity.PART_NO = partNo.Trim();
                        lstEntity.REQUSTED_BY = requestedBy;
                        edit = false;
                        add = true;
                    }
                }
                else
                {
                    //var maxId = DB.DD_PCN.Max(x => x.SNO);
                    var maxId = DB.DD_PCN.Max(x => x.SNO);
                    maxId = maxId + 1;
                    lstEntity = new DD_PCN();
                    lstEntity.SNO = maxId;
                    lstEntity.PART_NO = lstpcn.PART_NO;
                    lstEntity.REQUSTED_BY = requestedBy;
                    edit = false;
                    add = true;
                }
                lstEntity.ACTUAL_CHANGE_IMP = lstpcn.ACTUAL_CHANGE_IMP.ToValueAsString().Trim();
                lstEntity.APPROVED_BY = lstpcn.APPROVED_BY.ToValueAsString().Trim();
                lstEntity.CHANGE_EFFECTIVE = lstpcn.CHANGE_EFFECTIVE;
                lstEntity.COMPILED_BY = lstpcn.COMPILED_BY.ToValueAsString().Trim();
                lstEntity.CONTROL_PLAN = lstpcn.CONTROL_PLAN;
                lstEntity.COST_DESC = lstpcn.COST_DESC.ToValueAsString().Trim();
                lstEntity.CUST_NAME = lstpcn.CUST_NAME.ToValueAsString().Trim();
                lstEntity.CUST_PART_NO = lstpcn.CUST_PART_NO.ToValueAsString().Trim();
                lstEntity.DATE_OF_PCN = lstpcn.DATE_OF_PCN;
                lstEntity.DATE_OF_SIGN = lstpcn.DATE_OF_SIGN;
                lstEntity.DISPOSITION = lstpcn.DISPOSITION.ToValueAsString().Trim();
                lstEntity.GAUGE_DWG = lstpcn.GAUGE_DWG;
                lstEntity.INFGG_INITIAL = lstpcn.INFGG_INITIAL.ToValueAsString().Trim();
                lstEntity.INFGG_QTY = lstpcn.INFGG_QTY.ToValueAsString().Trim();
                lstEntity.INHEAT_TREATMENT_INITIAL = lstpcn.INHEAT_TREATMENT_INITIAL.ToValueAsString().Trim();
                lstEntity.INHEAT_TREATMENT_QTY = lstpcn.INHEAT_TREATMENT_QTY.ToValueAsString().Trim();
                lstEntity.INWIP_INITIAL = lstpcn.INWIP_INITIAL.ToValueAsString().Trim();
                lstEntity.INWIP_QTY = lstpcn.INWIP_QTY;
                lstEntity.MANUFACTURE_PROCESS = lstpcn.MANUFACTURE_PROCESS.ToValueAsString().Trim();
                lstEntity.NATURE_OF_CHANGE = lstpcn.NATURE_OF_CHANGE.ToValueAsString().Trim();
                lstEntity.OTHERS = lstpcn.OTHERS;
                lstEntity.PART_DESC = lstpcn.PART_DESC.ToValueAsString().Trim();
                lstEntity.PFD = lstpcn.PFD;
                lstEntity.PFMEA = lstpcn.PFMEA;
                lstEntity.PRODUCT_CHANGE_NO = lstpcn.PRODUCT_CHANGE_NO.ToValueAsString().Trim();
                lstEntity.PRODUCT_DWG = lstpcn.PRODUCT_DWG;
                lstEntity.RE_PPAP = lstpcn.RE_PPAP;
                lstEntity.RESON_FOR_CHANGE = lstpcn.RESON_FOR_CHANGE.ToValueAsString().Trim();
                lstEntity.SEQUENCE_DWG = lstpcn.SEQUENCE_DWG;
                lstEntity.SFL_DRAW_ISSUE_DATE = lstpcn.SFL_DRAW_ISSUE_DATE;
                lstEntity.SFL_DRAW_ISSUEDATE1 = lstpcn.SFL_DRAW_ISSUEDATE1;
                lstEntity.SFL_DRAW_ISSUENO = lstpcn.SFL_DRAW_ISSUENO;
                lstEntity.SFL_DRAW_ISSUENO1 = lstpcn.SFL_DRAW_ISSUENO1;
                lstEntity.TOOL_DWG = lstpcn.TOOL_DWG;
                lstEntity.CUST_ISSUE_NO = lstpcn.CUST_ISSUE_NO;
                lstEntity.CUST_DWG_NO = lstpcn.CUST_DWG_NO.ToValueAsString().Trim();
                lstEntity.CUST_DWG_NO_ISSUE = lstpcn.CUST_DWG_NO_ISSUE.ToValueAsString().Trim();
                lstEntity.ROUTING_TAG = lstpcn.ROUTING_TAG;
                lstEntity.FINISH_CODE_SAP_DWG = lstpcn.FINISH_CODE_SAP_DWG;
                lstEntity.WORK_INSTRUCTION = lstpcn.WORK_INSTRUCTION;
                lstEntity.SAP_SEQ_DWG_ISSUE_NO_UPD = lstpcn.SAP_SEQ_DWG_ISSUE_NO_UPD;
                lstEntity.ALREADY_IN_EFFECT = lstpcn.ALREADY_IN_EFFECT;
                lstEntity.IMMEDIATE = lstpcn.IMMEDIATE;
                lstEntity.ALREADY_IN_EFFECT = lstpcn.ALREADY_IN_EFFECT;
                lstEntity.FROM_NEXT_HT_CYCLE = lstpcn.FROM_NEXT_HT_CYCLE;
                lstEntity.FROM_NEXT_FORGING = lstpcn.FROM_NEXT_FORGING;
                lstEntity.STOCK_AT_CODE = lstpcn.STOCK_AT_CODE.ToValueAsString().Trim();
                lstEntity.STOCK_AT_DESCRIPTION = lstpcn.STOCK_AT_DESCRIPTION.ToValueAsString().Trim();
                lstEntity.WARE_HOUSE_CODE = lstpcn.WARE_HOUSE_CODE.ToValueAsString().Trim();
                lstEntity.LOGISTICS_DESCRIPTION = lstpcn.LOGISTICS_DESCRIPTION.ToValueAsString().Trim();
                lstEntity.WARE_HOUSE_DESCRIPTION = lstpcn.WARE_HOUSE_DESCRIPTION.ToValueAsString().Trim();
                lstEntity.WIP_HT_CODE = lstpcn.WIP_HT_CODE.ToValueAsString().Trim();
                lstEntity.NEW_CODE = lstpcn.NEW_CODE.ToValueAsString().Trim();
                lstEntity.NEW_DESCRIPTION = lstpcn.NEW_DESCRIPTION.ToValueAsString().Trim();
                lstEntity.DATE_OF_IMPLEMENTATION = lstpcn.DATE_OF_IMPLEMENTATION;
                lstEntity.INVOICE_NO = lstpcn.INVOICE_NO.ToValueAsString().Trim();
                lstEntity.INVOICE_DATE = lstpcn.INVOICE_DATE;
                lstEntity.ECN_REFERENCE_NO = lstpcn.ECN_REFERENCE_NO.ToValueAsString().Trim();
                lstEntity.VERIFICATION = lstpcn.VERIFICATION.ToValueAsString().Trim();
                lstEntity.VALIDATION = lstpcn.VALIDATION.ToValueAsString().Trim();
                lstEntity.LOGISTICS_CODE = lstpcn.LOGISTICS_CODE;
                lstEntity.WIP_HT_DESCRIPTION = lstpcn.WIP_HT_DESCRIPTION;
                lstEntity.SIGNATURE_NAME = lstpcn.SIGNATURE_NAME;
                lstEntity.SIGNATURE_DATE = lstpcn.SIGNATURE_DATE;
                if (add == true)
                {
                    DB.DD_PCN.InsertOnSubmit(lstEntity);
                }
                submit = true;
                DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return false;

            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (add == true)
                    {
                        DB.DD_PCN.DeleteOnSubmit(lstEntity);
                    }
                    if (edit == true)
                    {
                        DB.DD_PCN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstEntity);
                    }
                }
                DB.DD_PCN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstEntity);
                ex.LogException();
                return false;
            }
        }
        public void GetCustomerDetails(DD_PCN lstpcn)
        {
            try
            {
                var lstEntity = (from A in DB.PRD_CIREF
                                 from B in DB.DDCI_INFO
                                 from C in DB.DDCUST_MAST
                                 from E in DB.PRD_MAST
                                 where B.CI_REFERENCE == A.CI_REF && A.CURRENT_CIREF == true &&
                                 B.CUST_CODE == C.CUST_CODE &&
                                 A.PART_NO.Trim() == lstpcn.PART_NO.Trim() &&
                                 E.PART_NO.Trim() == lstpcn.PART_NO.Trim()
                                 select new { A.CI_REF, B.CUST_DWG_NO, B.CUST_CODE, C.CUST_NAME, E.PART_DESC, E.PART_NO }).FirstOrDefault();
                if (lstEntity != null)
                {
                    // lstpcn.ECN_REFERENCE_NO = lstEntity.CI_REF;
                    if (lstpcn.CUST_PART_NO.ToValueAsString() == "")
                    {
                        lstpcn.CUST_PART_NO = lstEntity.CUST_DWG_NO;
                    }
                    if (lstpcn.CUST_NAME.ToValueAsString() == "")
                    {
                        if (lstEntity.CUST_NAME.Length > 30)
                        {
                            lstpcn.CUST_NAME = lstEntity.CUST_NAME.Substring(0, 30);
                        }
                        else
                        {
                            lstpcn.CUST_NAME = lstEntity.CUST_NAME;
                        }

                    }
                }
                else
                {
                    lstpcn.CUST_PART_NO = "";
                    lstpcn.CUST_NAME = "";
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.DD_PCN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstpcn);

                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                DB.DD_PCN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, lstpcn);
                ex.LogException();
            }
            //SELECT A.CI_REF,B.cust_dwg_no,B.cust_code,C.cust_name,E.PART_DESC ,E.PART_NO  FROM  PRD_ciref A ,ddci_info B,ddcust_mast c ,PRD_MAST E 
            //WHERE A.PART_NO = '" & ltbSflPartNo.text & "' AND E.PART_NO='" & ltbSflPartNo.text & "' AND B. ci_reference=A.CI_REF  AND B.CUST_CODE=C.CUST_CODE
        }

        public Int32 GetMaxSheetNo(string partNo, string screentype)
        {
            string requestedBy;
            requestedBy = (screentype == "MPS" ? "SFL" : "CUSTOMER");
            try
            {
                List<DD_PCN> lstPCN = (from row in DB.DD_PCN
                                       where row.PART_NO.Trim() == partNo.Trim()
                                       && row.REQUSTED_BY.ToUpper().Trim() == requestedBy.ToUpper().Trim()
                                       select row).ToList<DD_PCN>();
                List<DD_PCN> lstValue = new List<DD_PCN>();

                foreach (DD_PCN row in lstPCN)
                {
                    try
                    {
                        DD_PCN pcn = new DD_PCN();
                        pcn.SNO = getChangeNo(row.PRODUCT_CHANGE_NO);
                        lstValue.Add(pcn);
                    }
                    catch (Exception ex)
                    {

                    }
                    //lstValue.Add(getChangeNo(row.PRODUCT_CHANGE_NO));
                }
                var maxProductChangeNo = lstValue.Max(x => x.SNO);
                maxProductChangeNo = maxProductChangeNo + 1;
                return Convert.ToInt32(maxProductChangeNo);
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        private Int32 getChangeNo(string productChangeNo)
        {
            try
            {
                string changeNo;
                int pos = -1;
                productChangeNo = productChangeNo.ToValueAsString().Trim();
                pos = productChangeNo.IndexOf("/");
                if (pos > -1)
                {
                    changeNo = productChangeNo.Substring(pos + 1);
                }
                else
                {
                    changeNo = productChangeNo;
                }
                return Convert.ToInt16(changeNo);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public DataView GetPartDetails()
        {
            return null;
        }


        #endregion
    }
}
