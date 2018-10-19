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
    public class Coatingmaster : Essential
    {
        public Coatingmaster(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }
        public DataView GetcoatingnMaster()
        {
            DataTable dataValue;

            dataValue = ToDataTableWithType((from c in DB.DDCOATING_MAST.AsEnumerable()
                                             //where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             orderby c.COATING_CODE ascending
                                             select new { OPER_CODE = c.COATING_CODE, OPER_DESC = c.COATING_DESC, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false), c.COLORAPP, c.SALT_SPRAY_WHITE, c.SALT_SPRAY_RED, c.COF, OPER_CODE_SORT = c.COATING_CODE.ToDecimalValue() }).ToList());
            dataValue.DefaultView.Sort = "OPER_CODE_SORT asc";
            return dataValue.DefaultView;
        }
        public DataTable GetTopCoatingMaster()
        {
            DataTable dataValue;

            dataValue = ToDataTableWithType((from c in DB.DDCOATING_MAST.AsEnumerable()
                                             where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             orderby c.COATING_CODE ascending
                                             select new { OPER_CODE = c.COATING_CODE, OPER_DESC = c.COATING_DESC, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false), c.COLORAPP, c.SALT_SPRAY_WHITE, c.SALT_SPRAY_RED, c.COF, OPER_CODE_SORT = c.COATING_CODE.ToDecimalValue() }).ToList());
            //dataValue.DefaultView.Sort = "OPER_CODE_SORT asc";
            return dataValue;
        }


        public bool AddNewCoatingMaster(bool isActive, string operCode, string operDesc, string showInCost, string nextAction, string colorapp, string salt_spray_white, string salt_spray_red, string cof, ref string message)
        {
            bool _status = false;

            if (nextAction == "ADD" && CheckCoatingEsxists(operCode) == true)
            {
                message = PDMsg.AlreadyExists("Coating Code");
                return false;
            }

            DDCOATING_MAST ddCoattMaster = (from c in DB.DDCOATING_MAST
                                            where c.COATING_CODE == operCode
                                            //where (c.COATING_CODE == operCode || operDesc.ToUpper() == c.COATING_DESC.ToUpper())
                                            // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                            //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || c.DELETE_FLAG =null)
                                            select c).FirstOrDefault<DDCOATING_MAST>();

            try
            {               

                if (nextAction == "DELETE")
                {
                    nextAction = "EDIT";
                }

                if (nextAction == "ADD")
                {
                    if (ddCoattMaster == null)
                    {
                        ddCoattMaster = new DDCOATING_MAST();
                        ddCoattMaster.COATING_CODE = (string)operCode;
                        ddCoattMaster.COATING_DESC = (string)operDesc;
                        ddCoattMaster.DELETE_FLAG = isActive;
                        ddCoattMaster.COLORAPP = colorapp;
                        ddCoattMaster.SALT_SPRAY_WHITE = salt_spray_white;
                        ddCoattMaster.SALT_SPRAY_RED = salt_spray_red;
                        ddCoattMaster.COF = cof;
                        ddCoattMaster.ENTERED_DATE = DateTime.Now;
                        ddCoattMaster.ENTERED_BY = userInformation.UserName;
                        ddCoattMaster.ROWID = Guid.NewGuid();
                        DB.DDCOATING_MAST.InsertOnSubmit(ddCoattMaster);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                    }
                    //else
                    //{
                    //    if (CheckCoatingEsxists(operCode) == true)
                    //    {
                    //        message = PDMsg.AlreadyExists("Coating Code");
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        message = PDMsg.AlreadyExists("Coating Desc");
                    //        return false;
                    //    }
                    //    ////----- Check Delete Flag
                    //    //if (ddCoattMaster.DELETE_FLAG != true)
                    //    //{
                    //    //    ddCoattMaster.COATING_DESC = (string)operDesc;
                    //    //    ddCoattMaster.COLORAPP = colorapp;
                    //    //    ddCoattMaster.SALT_SPRAY_WHITE = salt_spray_white.ToIntValue();
                    //    //    ddCoattMaster.SALT_SPRAY_RED = salt_spray_red.ToIntValue();
                    //    //    ddCoattMaster.COF = cof.ToDecimalValue();
                    //    //    ddCoattMaster.UPDATED_DATE = DateTime.Now;
                    //    //    ddCoattMaster.DELETE_FLAG = isActive;
                    //    //    ddCoattMaster.UPDATED_BY = userInformation.UserName;
                    //    //    DB.SubmitChanges();
                    //    //    message = PDMsg.SavedSuccessfully;
                    //    //}
                    //    //else
                    //    //{

                    //    //    if (CheckCoatingEsxists(operCode) == true)
                    //    //    {
                    //    //        message = PDMsg.AlreadyExists("Coating Code");
                    //    //        return false;
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        message = PDMsg.AlreadyExists("Coating Desc");
                    //    //        return false;
                    //    //    }

                    //    //}
                    //}

                }
                else if (nextAction == "EDIT")
                {
                    if (CheckCoatingEsxistsDesc(operCode, operDesc) == true)
                    {
                        message = PDMsg.AlreadyExists("Coating Desc");
                        return false;
                    }
                    ddCoattMaster.COATING_DESC = (string)operDesc;
                    ddCoattMaster.COLORAPP = colorapp;
                    ddCoattMaster.SALT_SPRAY_WHITE = salt_spray_white;
                    ddCoattMaster.SALT_SPRAY_RED = salt_spray_red;
                    ddCoattMaster.COF = cof;
                    ddCoattMaster.UPDATED_DATE = DateTime.Now;
                    ddCoattMaster.DELETE_FLAG = isActive;
                    ddCoattMaster.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    message = PDMsg.UpdatedSuccessfully;
                }
                _status = true;
            }
            catch (Exception ex)
            {
                if (nextAction == "ADD")
                {
                    DB.DDCOATING_MAST.DeleteOnSubmit(ddCoattMaster);
                }
                else if (nextAction == "EDIT")
                {
                    DB.DDCOATING_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddCoattMaster);
                }

                throw ex.LogException();
            }
            return _status;
        }

        public bool DeletcoatingCode(string operCode, string nextAction, ref string message)
        {
            DDCOATING_MAST ddCoatMaster = (from c in DB.DDCOATING_MAST
                                           where c.COATING_CODE == operCode
                                           //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                           //Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false
                                           select c).SingleOrDefault<DDCOATING_MAST>();
            try
            {

                if (nextAction == "DELETE")
                {
                    if (ddCoatMaster != null)
                    {
                        if (ddCoatMaster.DELETE_FLAG == true)
                        {
                            ddCoatMaster.DELETE_FLAG = false;
                        }
                        else
                        {
                            ddCoatMaster.DELETE_FLAG = true;
                        }

                        //   ddCoatMaster.DELETE_FLAG = true;
                        ddCoatMaster.UPDATED_DATE = DateTime.Now;
                        ddCoatMaster.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                        // ddCoatMaster.COATING_CODE = operCode;
                        // DB.DDCOATING_MAST.DeleteOnSubmit(ddCoatMaster);
                        DB.SubmitChanges();
                        message = PDMsg.DeletedSuccessfully;
                    }
                    else if (ddCoatMaster == null)
                    {
                        message = PDMsg.NoRecordFound;
                    }
                }
            }

            catch (Exception ex)
            {
                ex.LogException();
                DB.DDCOATING_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddCoatMaster);

            }
            return true;
        }

        public bool CheckCoatingEsxists(string operCode)
        {
            try
            {
                DDCOATING_MAST ddCoatMaster = (from c in DB.DDCOATING_MAST
                                               where c.COATING_CODE == operCode
                                               // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                               select c).SingleOrDefault<DDCOATING_MAST>();
                if (ddCoatMaster != null)
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
                ex.LogException();
                return false;
            }
        }
        public bool CheckCoatingEsxistsDesc(string operCode, string operDesc)
        {
            try
            {
                DDCOATING_MAST ddCoatMaster = (from c in DB.DDCOATING_MAST
                                               where c.COATING_CODE != operCode && c.COATING_DESC == operDesc
                                               // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                               select c).FirstOrDefault<DDCOATING_MAST>();
                if (ddCoatMaster != null)
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
                ex.LogException();
                return false;
            }
        }
    }

}
