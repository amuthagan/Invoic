using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class StandardConditionBll : Essential
    {
        public string DeleteFlag = "N";
        public StandardConditionBll(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }

        public DataTable GetStandard_Main(string costCenterCode, string categoryId)
        {
            DataTable dataValue;

          

            dataValue = ToDataTable((from C in DB.DDSTAND_MAIN.AsEnumerable()
                                     where C.COST_CENT_CODE == costCenterCode && C.CATEGORY == categoryId
                                     //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                     orderby C.COST_CENT_CODE ascending
                                     select new { C.COST_CENT_CODE, C.CATEGORY, C.DATE_MADE, C.VALID_UPTO, C.REVISION_NO, C.REVISION_DATE }).ToList());
            return dataValue;
        }

        public DataTable GetStandSubMaster(string costCenterCode, string categoryId)
        {
            DataTable dataValue;

          

            dataValue = ToDataTable((from C in DB.DDSTAND_SUB.AsEnumerable()
                                     where C.COST_CENT_CODE == costCenterCode && C.CATE_CODE == categoryId
                                     //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                     orderby C.COST_CENT_CODE ascending
                                     select new { C.COST_CENT_CODE, C.CATE_CODE, SER_NO = Convert.ToInt32(C.SER_NO), C.ILLUSTRATION, C.AREA, C.REQD_COND, C.METHOD, C.RESP, C.FREQUENCY, C.TIME_ALLOWED }).ToList());
            return dataValue;
        }
        public bool AddNewDDStandMain(DateTime? date_made, DateTime? valid_upto, DateTime? revision_date, string revision_no, string costCenterCode, string categoryId, ref string message)
        {
            bool _status = false;
            try
            {
                DDSTAND_MAIN stndMain = (from c in DB.DDSTAND_MAIN
                                             where c.COST_CENT_CODE == costCenterCode && c.CATEGORY == Convert.ToString(categoryId.ToString())
                                             //    && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || c.DELETE_FLAG =null)
                                             select c).FirstOrDefault<DDSTAND_MAIN>();
                if (stndMain != null)
                {
                    try
                    {
                        stndMain.DATE_MADE = (date_made);
                        stndMain.VALID_UPTO = (valid_upto);
                        stndMain.REVISION_DATE = (revision_date);
                        stndMain.REVISION_NO = (string)(revision_no);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                        _status = true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDSTAND_MAIN.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, stndMain);
                    }
                  

                }
                else if (stndMain == null)
                {
                    try
                    {
                        stndMain = new DDSTAND_MAIN();
                        stndMain.COST_CENT_CODE = costCenterCode;
                        stndMain.CATEGORY = categoryId;
                        if (date_made.IsNotNullOrEmpty()) stndMain.DATE_MADE = (DateTime)(date_made);
                        if (valid_upto.IsNotNullOrEmpty()) stndMain.VALID_UPTO = (DateTime)(valid_upto);
                        if (revision_date.IsNotNullOrEmpty()) stndMain.REVISION_DATE = (DateTime)(revision_date);
                        stndMain.REVISION_NO = (string)(revision_no);
                        stndMain.ROWID = Guid.NewGuid();
                        DB.DDSTAND_MAIN.InsertOnSubmit(stndMain);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                        _status = true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDSTAND_MAIN.DeleteOnSubmit(stndMain);
                    }
                   
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return _status;
        }
        public bool AddNewOptimalCondition(DataTable masterdata, string costCenterCode, string categoryId, ref string message)
        {
            bool _status = false;
            string serNo = "0";
            try
            {

                foreach (DataRow row in masterdata.Rows)
                {
                    if (row["SER_NO"].ToString().Trim().Length == 0)
                    {
                        serNo = "0";
                    }
                    else
                    {
                        serNo = row["SER_NO"].ToString().Trim();
                    }
                    DDSTAND_SUB ddStandMaster = (from c in DB.DDSTAND_SUB
                                                 where c.COST_CENT_CODE == costCenterCode && c.SER_NO == Convert.ToDecimal(serNo) && c.CATE_CODE == Convert.ToString(categoryId.ToString())
                                                 //    && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                 //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || c.DELETE_FLAG =null)
                                                 select c).SingleOrDefault<DDSTAND_SUB>();
                    //TextBox1.Text = row["ImagePath"].ToString();

                    if (ddStandMaster != null)
                    {

                        ddStandMaster.ILLUSTRATION = row["ILLUSTRATION"].ToString();
                        //ddStandMaster.SER_NO = (decimal) Convert.ToDecimal(row["SER_NO"].ToString());
                        ddStandMaster.AREA = row["AREA"].ToString();
                        ddStandMaster.REQD_COND = row["REQD_COND"].ToString();
                        ddStandMaster.METHOD = row["METHOD"].ToString();
                        ddStandMaster.RESP = row["RESP"].ToString();
                        ddStandMaster.FREQUENCY = row["FREQUENCY"].ToString();
                        ddStandMaster.TIME_ALLOWED = row["TIME_ALLOWED"].ToString();
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                    }
                    else if (ddStandMaster == null)
                    {
                        ddStandMaster = new DDSTAND_SUB();
                        ddStandMaster.COST_CENT_CODE = costCenterCode;
                        ddStandMaster.CATE_CODE = categoryId;
                        ddStandMaster.SER_NO = Convert.ToDecimal(row["SER_NO"].ToString());
                        ddStandMaster.ILLUSTRATION = row["ILLUSTRATION"].ToString();
                        ddStandMaster.AREA = row["AREA"].ToString();
                        ddStandMaster.REQD_COND = row["REQD_COND"].ToString();
                        ddStandMaster.METHOD = row["METHOD"].ToString();
                        ddStandMaster.RESP = row["RESP"].ToString();
                        ddStandMaster.FREQUENCY = row["FREQUENCY"].ToString();
                        ddStandMaster.TIME_ALLOWED = row["TIME_ALLOWED"].ToString();
                        ddStandMaster.ROWID = Guid.NewGuid();
                        DB.DDSTAND_SUB.InsertOnSubmit(ddStandMaster);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                    }
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return _status;
        }
    }
}
