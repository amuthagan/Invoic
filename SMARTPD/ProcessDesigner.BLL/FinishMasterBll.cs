using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class FinishMasterBll : Essential
    {
        public FinishMasterBll(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }
        public DataView GetFinishMaster()
        {
            //ddfinish_mast
            DataTable dataValue;
            dataValue = null;

            dataValue = ToDataTableWithType((from c in DB.DDFINISH_MAST.AsEnumerable()
                                             //where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             orderby c.FINISH_CODE ascending
                                             select new { OPER_CODE = c.FINISH_CODE, OPER_DESC = c.FINISH_DESC, c.COLORAPP, c.COATING_WEIGHT, c.COATING_THICKNESS, c.SALT_SPRAY_WHITE, c.SALT_SPRAY_RED, c.COF, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false), OPER_CODE_SORT = c.FINISH_CODE.ToDecimalValue() }).ToList());
            dataValue.DefaultView.Sort = "OPER_CODE_SORT asc";
            return dataValue.DefaultView;
        }

        public DataTable GetBaseCoating()
        {
            //ddfinish_mast
            DataTable dataValue;
            dataValue = null;

            dataValue = ToDataTableWithType((from c in DB.DDFINISH_MAST.AsEnumerable()
                                             where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             orderby c.FINISH_CODE ascending
                                             select c).ToList());
            return dataValue;
        }

        public bool AddNewFinishMaster(bool isActive, string operCode, string operDesc, string showInCost, string nextAction, string colorappearance, string coatingwight, string coatingthickness, string saltspraywhite, string saltsprayred, string cof, ref string message)
        {
            bool _status = false;

            DDFINISH_MAST ddFinish = (from c in DB.DDFINISH_MAST
                                      where (c.FINISH_CODE == operCode || operDesc.ToUpper() == c.FINISH_DESC.ToUpper())
                                      //   && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                      select c).FirstOrDefault<DDFINISH_MAST>();


            try
            {
                if (nextAction == "DELETE")
                {
                    nextAction = "EDIT";
                }
                if (nextAction == "ADD")
                {
                    if (ddFinish == null)
                    {
                        ddFinish = new DDFINISH_MAST();
                        ddFinish.FINISH_CODE = (string)operCode;
                        ddFinish.FINISH_DESC = (string)operDesc;
                        ddFinish.COLORAPP = colorappearance;
                        ddFinish.COATING_WEIGHT = coatingwight;
                        ddFinish.COATING_THICKNESS = coatingthickness;
                        ddFinish.SALT_SPRAY_WHITE = saltspraywhite;
                        ddFinish.SALT_SPRAY_RED = saltsprayred;
                        ddFinish.COF = cof;
                        ddFinish.DELETE_FLAG = isActive;
                        ddFinish.ENTERED_DATE = DateTime.Now;
                        ddFinish.ENTERED_BY = userInformation.UserName;
                        ddFinish.ROWID = Guid.NewGuid();
                        DB.DDFINISH_MAST.InsertOnSubmit(ddFinish);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                    }
                    else
                    {
                        if (CheckFinishEsxists(operCode) == true)
                        {
                            message = PDMsg.AlreadyExists("Finish Code");
                            return false;
                        }
                        else
                        {
                            message = PDMsg.AlreadyExists("Finish Desc");
                            return false;
                        }
                        //if (ddFinish.DELETE_FLAG != true)
                        //{
                        //    ddFinish.FINISH_DESC = (string)operDesc;
                        //    ddFinish.COLORAPP = colorappearance;
                        //    ddFinish.COATING_WEIGHT = coatingwight.ToDecimalValue();
                        //    ddFinish.COATING_THICKNESS = coatingthickness.ToDecimalValue();
                        //    ddFinish.SALT_SPRAY_WHITE = saltspraywhite.ToIntValue();
                        //    ddFinish.SALT_SPRAY_RED = saltsprayred.ToIntValue();
                        //    ddFinish.COF = cof.ToDecimalValue();
                        //    ddFinish.UPDATED_DATE = DateTime.Now;
                        //    ddFinish.DELETE_FLAG = isActive;
                        //    ddFinish.UPDATED_BY = userInformation.UserName;
                        //    DB.SubmitChanges();
                        //    message = PDMsg.SavedSuccessfully;
                        //}
                        //else
                        //{
                        //    if (CheckFinishEsxists(operCode) == true)
                        //    {
                        //        message = PDMsg.AlreadyExists("Finish Code");
                        //        return false;
                        //    }
                        //    else
                        //    {
                        //        message = PDMsg.AlreadyExists("Finish Desc");
                        //        return false;
                        //    }

                        //}
                        // message = "Finish Code already Exists";
                        // return false;
                    }

                }
                else if (nextAction == "EDIT")
                {
                    if (CheckFinishEsxistsDesc(operCode, operDesc) == true)
                    {
                        message = PDMsg.AlreadyExists("Finish Desc");
                        return false;
                    }
                    ddFinish.FINISH_DESC = (string)operDesc;
                    ddFinish.COLORAPP = colorappearance;
                    ddFinish.COATING_WEIGHT = coatingwight;
                    ddFinish.COATING_THICKNESS = coatingthickness;
                    ddFinish.SALT_SPRAY_WHITE = saltspraywhite;
                    ddFinish.SALT_SPRAY_RED = saltsprayred;
                    ddFinish.COF = cof;
                    ddFinish.UPDATED_DATE = DateTime.Now;
                    ddFinish.DELETE_FLAG = isActive;
                    ddFinish.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    message = PDMsg.UpdatedSuccessfully;
                }
                _status = true;
            }
            catch (Exception ex)
            {
                if (nextAction == "ADD")
                {
                    DB.DDFINISH_MAST.DeleteOnSubmit(ddFinish);
                }
                else if (nextAction == "EDIT")
                {
                    DB.DDFINISH_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddFinish);
                }
                ex.LogException();
            }
            return _status;
        }

        public bool DeletFinishCode(string operCode, string nextAction, ref string message)
        {
            DDFINISH_MAST ddFinish = (from c in DB.DDFINISH_MAST
                                      where c.FINISH_CODE == operCode
                                      //&& ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                      select c).SingleOrDefault<DDFINISH_MAST>();

            try
            {

                if (nextAction == "DELETE")
                {
                    if (ddFinish != null)
                    {

                        if (ddFinish.DELETE_FLAG == true)
                        {
                            ddFinish.DELETE_FLAG = false;
                        }
                        else
                        {
                            ddFinish.DELETE_FLAG = true;
                        }

                        // ddFinish.DELETE_FLAG = true;
                        ddFinish.UPDATED_DATE = DateTime.Now;
                        ddFinish.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                        //  ddFinish.FINISH_CODE  = operCode;
                        // DB.DDFINISH_MAST.DeleteOnSubmit(ddFinish);
                        DB.SubmitChanges();
                        message = PDMsg.DeletedSuccessfully;
                    }
                    else if (ddFinish == null)
                    {
                        message = PDMsg.NoRecordFound;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.DDFINISH_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddFinish);

            }
            return true;
        }

        public bool CheckFinishEsxists(string operCode)
        {
            try
            {

                DDFINISH_MAST ddFinish = (from c in DB.DDFINISH_MAST
                                          where c.FINISH_CODE == operCode
                                          //&& ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                          select c).SingleOrDefault<DDFINISH_MAST>();
                if (ddFinish != null)
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
                // throw ex.LogException();
                ex.LogException();
                return false;

            }
        }
        public bool CheckFinishEsxistsDesc(string operCode, string operDesc)
        {
            try
            {

                DDFINISH_MAST ddFinish = (from c in DB.DDFINISH_MAST
                                          where c.FINISH_CODE != operCode && c.FINISH_DESC == operDesc
                                          //&& ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                          select c).FirstOrDefault<DDFINISH_MAST>();
                if (ddFinish != null)
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
                // throw ex.LogException();
                ex.LogException();
                return false;

            }
        }

    }

}
