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
    public class OperationMasterBll : Essential
    {
        public string DeleteFlag = "N";
        public OperationMasterBll(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;

        }
        public DataView GetOpertionMaster()
        {
            DataTable dataValue;
            dataValue = ToDataTableWithType((from c in DB.DDOPER_MAST.AsEnumerable()
                                             //where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             orderby c.OPER_CODE ascending
                                             select new { c.OPER_CODE, c.OPER_DESC, c.SHOW_IN_COST, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false) }).ToList());

            return dataValue.DefaultView;
        }

        public bool CheckOperCodeEsxists(decimal operCode)
        {
            try
            {

                DDOPER_MAST ddOperMast = (from c in DB.DDOPER_MAST
                                          where c.OPER_CODE == operCode
                                          && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                          select c).SingleOrDefault<DDOPER_MAST>();
                if (ddOperMast != null)
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

        public bool DeletOperCode(decimal operCode, string nextAction, ref string message)
        {
            DDOPER_MAST ddOperMast = (from c in DB.DDOPER_MAST
                                      where c.OPER_CODE == operCode
                                      // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                      select c).SingleOrDefault<DDOPER_MAST>();
            try
            {

                //  UpdateHistory(operCode);

                if (nextAction == "DELETE")
                {
                    if (ddOperMast != null)
                    {
                        if (ddOperMast.DELETE_FLAG == true)
                        {
                            ddOperMast.DELETE_FLAG = false;
                        }
                        else
                        {
                            ddOperMast.DELETE_FLAG = true;
                        }


                        ddOperMast.UPDATED_DATE = DateTime.Now;
                        ddOperMast.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                        //DB.Refresh(System.Data.Linq.RefreshMode.KeepChanges, ddOperMast);
                        DB.DDOPER_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddOperMast);

                        //ddOperMast.OPER_CODE = operCode;
                        //DB.DDOPER_MAST.DeleteOnSubmit(ddOperMast);
                        //DB.SubmitChanges();
                        message = PDMsg.DeletedSuccessfully;
                    }
                    else if (ddOperMast == null)
                    {
                        message = PDMsg.NoRecordFound;
                    }
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.DDOPER_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddOperMast);

            }
            return true;
        }

        public void UpdateHistory(decimal operCode)
        {
            DDOPER_MAST ddOperMast = (from c in DB.DDOPER_MAST
                                      where c.OPER_CODE == operCode
                                        && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                      select c).SingleOrDefault<DDOPER_MAST>();
            try
            {

                REPL_DDOPER_MAST ddReplHistory = new REPL_DDOPER_MAST();
                if (ddOperMast != null)
                {

                    ddReplHistory.OPER_CODE = ddOperMast.OPER_CODE;
                    ddReplHistory.OPER_DESC = ddOperMast.OPER_DESC;
                    ddReplHistory.OPTIONAL_OPER = ddOperMast.OPTIONAL_OPER;
                    ddReplHistory.TAG_APPREVIATION = ddOperMast.TAG_APPREVIATION;
                    ddReplHistory.SHOW_IN_COST = ddOperMast.SHOW_IN_COST;
                    ddReplHistory.FLAG = "D";
                    ddReplHistory.UPDATEON = DateTime.Now;

                    DB.REPL_DDOPER_MAST.InsertOnSubmit(ddReplHistory);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.DDOPER_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddOperMast);

            }
        }

        public bool AddNewOperationMaster(bool isActive, decimal operCode, string operDesc, string showInCost, string nextAction, ref string message)
        {

            bool _status = false;

            DDOPER_MAST ddOperMast = (from c in DB.DDOPER_MAST
                                      //where (c.OPER_CODE == operCode || operDesc.ToUpper().Replace(" ", "") == c.OPER_DESC.ToUpper().Replace(" ", ""))
                                      where (c.OPER_CODE == operCode)
                                      // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                      select c).FirstOrDefault<DDOPER_MAST>();
            try
            {
                string mode = "";
                if (nextAction == "DELETE")
                {
                    nextAction = "EDIT";
                }

                if (nextAction == "ADD")
                {
                    if (ddOperMast == null)
                    {
                        ddOperMast = new DDOPER_MAST();
                        mode = "New";
                        ddOperMast.OPER_CODE = (decimal)operCode;
                        ddOperMast.OPER_DESC = (string)operDesc;
                        ddOperMast.DELETE_FLAG = isActive;
                        ddOperMast.ENTERED_DATE = DateTime.Now;
                        ddOperMast.ENTERED_BY = userInformation.UserName;

                        if (string.IsNullOrEmpty(showInCost))
                        {
                            ddOperMast.SHOW_IN_COST = "0";
                        }
                        else
                        {
                            ddOperMast.SHOW_IN_COST = showInCost;
                        }
                        ddOperMast.ROWID = Guid.NewGuid();
                        DB.DDOPER_MAST.InsertOnSubmit(ddOperMast);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                    }
                    else
                    {
                        message = PDMsg.AlreadyExists("Operation Code");
                        return false;
                        ////----- Check Delete Flag
                        //if (ddOperMast.DELETE_FLAG != true)
                        //{
                        //    ddOperMast.OPER_DESC = operDesc;
                        //    ddOperMast.SHOW_IN_COST = showInCost;
                        //    ddOperMast.DELETE_FLAG = isActive;
                        //    ddOperMast.UPDATED_DATE = DateTime.Now;
                        //    ddOperMast.UPDATED_BY = userInformation.UserName;
                        //    DB.SubmitChanges();
                        //    message = PDMsg.SavedSuccessfully;
                        //}
                        //else
                        //{
                        //    //if (CheckOperCodeEsxists(operCode) == true)
                        //    //{
                        //    //    message = PDMsg.AlreadyExists("Operation Code");
                        //    //    return false;
                        //    //}
                        //    //else
                        //    //{
                        //    message = PDMsg.AlreadyExists("Operation Code");
                        //    return false;

                        //    //  }

                        //}

                        //

                    }

                }
                else if (nextAction == "EDIT")
                {
                    ddOperMast.OPER_DESC = operDesc;
                    ddOperMast.SHOW_IN_COST = showInCost;
                    ddOperMast.DELETE_FLAG = isActive;
                    ddOperMast.UPDATED_DATE = DateTime.Now;
                    ddOperMast.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    message = PDMsg.UpdatedSuccessfully;
                }
                _status = true;
            }
            catch (Exception ex)
            {
                if (nextAction == "ADD")
                {
                    DB.DDOPER_MAST.DeleteOnSubmit(ddOperMast);
                }
                else if (nextAction == "EDIT")
                {
                    DB.DDOPER_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddOperMast);
                }
                ex.LogException();
            }
            return _status;
        }



    }



}
