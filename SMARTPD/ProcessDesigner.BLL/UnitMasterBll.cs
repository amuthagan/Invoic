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
    public class UnitMasterBll : Essential
    {
        public UnitMasterBll(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }
        public DataView GetUnitMaster()
        {
            //ddfinish_mast
            DataTable dataValue;
            dataValue = null;


            dataValue = ToDataTableWithType((from c in DB.DDUNIT_MAST.AsEnumerable()
                                             // where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             orderby c.UNIT_CODE ascending
                                             select new { OPER_CODE = c.UNIT_CODE, OPER_DESC = c.UNIT_OF_MEAS, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false), OPER_CODE_SORT = c.UNIT_CODE.ToDecimalValue() }).ToList());
            dataValue.DefaultView.Sort = "OPER_CODE_SORT asc";

            return dataValue.DefaultView;
        }

        public bool AddNewUnitMaster(bool isActive, string operCode, string operDesc, string showInCost, string nextAction, ref string message)
        {
            bool _status = false;

            DDUNIT_MAST dduUnitMaster = (from c in DB.DDUNIT_MAST
                                         where (c.UNIT_CODE == operCode || operDesc.ToUpper() == c.UNIT_OF_MEAS.ToUpper())
                                         //                                         where c.UNIT_CODE == operCode
                                         //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                         select c).FirstOrDefault<DDUNIT_MAST>();
            try
            {
                if (nextAction == "DELETE")
                {
                    nextAction = "EDIT";
                }
                if (nextAction == "ADD")
                {
                    if (dduUnitMaster == null)
                    {
                        dduUnitMaster = new DDUNIT_MAST();
                        dduUnitMaster.UNIT_CODE = (string)operCode;
                        dduUnitMaster.UNIT_OF_MEAS = (string)operDesc;
                        dduUnitMaster.DELETE_FLAG = isActive;
                        dduUnitMaster.ENTERED_DATE = DateTime.Now;
                        dduUnitMaster.ENTERED_BY = userInformation.UserName;
                        dduUnitMaster.ROWID = Guid.NewGuid();
                        DB.DDUNIT_MAST.InsertOnSubmit(dduUnitMaster);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                    }
                    else
                    {
                        if (CheckUnitEsxists(operCode) == true)
                        {
                            message = PDMsg.AlreadyExists("Unit Code");
                            return false;
                        }
                        else
                        {
                            message = PDMsg.AlreadyExists("Unit Desc");
                            return false;
                        }
                        //if (dduUnitMaster.DELETE_FLAG != true)
                        //{
                        //    dduUnitMaster.UNIT_OF_MEAS = (string)operDesc;
                        //    dduUnitMaster.UPDATED_DATE = DateTime.Now;
                        //    dduUnitMaster.DELETE_FLAG = isActive;
                        //    dduUnitMaster.UPDATED_BY = userInformation.UserName;
                        //    DB.SubmitChanges();
                        //    message = PDMsg.SavedSuccessfully;
                        //}
                        //else
                        //{
                        //    if (CheckUnitEsxists(operCode) == true)
                        //    {
                        //        message = PDMsg.AlreadyExists("Unit Code");
                        //        return false;
                        //    }
                        //    else
                        //    {
                        //        message = PDMsg.AlreadyExists("Unit Desc");
                        //        return false;
                        //    }

                        //}

                        // message = "Finish Code already Exists";
                        // return false;
                    }

                }
                else if (nextAction == "EDIT")
                {
                    if (CheckUnitEsxistsDesc(operCode, operDesc) == true)
                    {
                        message = PDMsg.AlreadyExists("Unit Desc");
                        return false;
                    }
                    dduUnitMaster.UNIT_OF_MEAS = (string)operDesc;
                    dduUnitMaster.UPDATED_DATE = DateTime.Now;
                    dduUnitMaster.DELETE_FLAG = isActive;
                    dduUnitMaster.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    message = PDMsg.UpdatedSuccessfully;
                }
                _status = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                if (nextAction == "ADD")
                {
                    DB.DDUNIT_MAST.DeleteOnSubmit(dduUnitMaster);
                }
                else if (nextAction == "EDIT")
                {
                    DB.DDUNIT_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dduUnitMaster);
                }


            }
            return _status;
        }

        public bool DeletUnitCode(string operCode, string nextAction, ref string message)
        {

            DDUNIT_MAST ddUnitMaster = (from c in DB.DDUNIT_MAST
                                        where c.UNIT_CODE == operCode
                                        //&& ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                        select c).SingleOrDefault<DDUNIT_MAST>();
            try
            {
                if (nextAction == "DELETE")
                {
                    if (ddUnitMaster != null)
                    {

                        if (ddUnitMaster.DELETE_FLAG == true)
                        {
                            ddUnitMaster.DELETE_FLAG = false;
                        }
                        else
                        {
                            ddUnitMaster.DELETE_FLAG = true;
                        }

                        // ddUnitMaster.DELETE_FLAG = true;
                        ddUnitMaster.UPDATED_DATE = DateTime.Now;
                        ddUnitMaster.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                        // ddUnitMaster.UNIT_CODE = operCode;
                        // DB.DDUNIT_MAST.DeleteOnSubmit(ddUnitMaster);
                        DB.SubmitChanges();
                        message = PDMsg.DeletedSuccessfully;
                    }
                    else if (ddUnitMaster == null)
                    {
                        message = PDMsg.NoRecordFound;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.DDUNIT_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddUnitMaster);

            }
            return true;
        }
        public bool CheckUnitEsxists(string operCode)
        {
            try
            {

                DDUNIT_MAST ddUnitMaster = (from c in DB.DDUNIT_MAST
                                            where c.UNIT_CODE == operCode
                                            // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                            select c).FirstOrDefault<DDUNIT_MAST>();
                if (ddUnitMaster != null)
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
        public bool CheckUnitEsxistsDesc(string operCode, string operDesc)
        {
            try
            {

                DDUNIT_MAST ddUnitMaster = (from c in DB.DDUNIT_MAST
                                            where c.UNIT_CODE != operCode && c.UNIT_OF_MEAS == operDesc
                                            // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                            select c).FirstOrDefault<DDUNIT_MAST>();
                if (ddUnitMaster != null)
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
