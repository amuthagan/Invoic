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
    public class CustomerMaset : Essential
    {
        public CustomerMaset(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }
        public DataView GetCustomerMaster()
        {
            //ddcust_mast
            DataTable dataValue;

            dataValue = ToDataTableWithType((from c in DB.DDCUST_MAST.AsEnumerable()
                                             // where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                             orderby c.CUST_CODE ascending
                                             select new { OPER_CODE = c.CUST_CODE, OPER_DESC = c.CUST_NAME, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false) }).ToList());
            return dataValue.DefaultView;
        }

        public bool AddNewCustomerMaster(bool isActive, decimal operCode, string operDesc, string showInCost, string nextAction, ref string message)
        {
            bool _status = false;

            DDCUST_MAST ddCustMaster = (from c in DB.DDCUST_MAST
                                        where c.CUST_CODE == operCode
                                        // where (c.CUST_CODE == operCode || operDesc.ToUpper().Replace(" ", "") == c.CUST_NAME.ToUpper().Replace(" ", ""))
                                        // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                        select c).FirstOrDefault<DDCUST_MAST>();

            try
            {
                if (nextAction == "DELETE")
                {
                    nextAction = "EDIT";
                }

                if (nextAction == "ADD")
                {
                    if (ddCustMaster == null)
                    {
                        ddCustMaster = new DDCUST_MAST();
                        ddCustMaster.CUST_CODE = (decimal)operCode;
                        ddCustMaster.CUST_NAME = (string)operDesc;
                        ddCustMaster.DELETE_FLAG = isActive;
                        ddCustMaster.ENTERED_DATE = DateTime.Now;
                        ddCustMaster.ENTERED_BY = userInformation.UserName;
                        ddCustMaster.ROWID = Guid.NewGuid();
                        DB.DDCUST_MAST.InsertOnSubmit(ddCustMaster);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                    }
                    else
                    {
                        if (CheckCodeisEsxists(operCode) == true)
                        {
                            message = PDMsg.AlreadyExists("Customer Code");
                            return false;
                        }
                        else
                        {
                            message = PDMsg.AlreadyExists("Customer Code");
                            return false;
                        }
                        //if (ddCustMaster.DELETE_FLAG != true)
                        //{
                        //    ddCustMaster.CUST_NAME = (string)operDesc;
                        //    ddCustMaster.UPDATED_DATE = DateTime.Now;
                        //    ddCustMaster.DELETE_FLAG = isActive;
                        //    ddCustMaster.UPDATED_BY = userInformation.UserName;
                        //    DB.SubmitChanges();
                        //    message = PDMsg.SavedSuccessfully;
                        //}
                        //else
                        //{
                        //    //if (CheckCodeisEsxists(operCode) == true)
                        //    //{
                        //    //    message = PDMsg.AlreadyExists("Customer Code");
                        //    //    return false;
                        //    //}
                        //    //else
                        //    //{
                        //    message = PDMsg.AlreadyExists("Customer Code");
                        //    return false;
                        //    //  }

                        //}

                        // message = "Customer Code already Exists";
                        // return false;
                    }

                }
                else if (nextAction == "EDIT")
                {
                    ddCustMaster.CUST_NAME = (string)operDesc;
                    ddCustMaster.UPDATED_DATE = DateTime.Now;
                    ddCustMaster.DELETE_FLAG = isActive;
                    ddCustMaster.UPDATED_BY = userInformation.UserName;
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
                    DB.DDCUST_MAST.DeleteOnSubmit(ddCustMaster);
                }
                else if (nextAction == "EDIT")
                {
                    DB.DDCUST_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddCustMaster);
                }
            }
            return _status;
        }

        public bool DeleteCustomerCode(decimal operCode, string nextAction, ref string message)
        {
            DDCUST_MAST ddCustMaster = (from c in DB.DDCUST_MAST
                                        where c.CUST_CODE == operCode
                                        //    && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                        select c).SingleOrDefault<DDCUST_MAST>();

            try
            {

                if (nextAction == "DELETE")
                {
                    if (ddCustMaster != null)
                    {

                        if (ddCustMaster.DELETE_FLAG == true)
                        {
                            ddCustMaster.DELETE_FLAG = false;
                        }
                        else
                        {
                            ddCustMaster.DELETE_FLAG = true;
                        }

                        ddCustMaster.CUST_CODE = operCode;
                        // ddCustMaster.DELETE_FLAG = true;
                        ddCustMaster.UPDATED_DATE = DateTime.Now;
                        ddCustMaster.UPDATED_BY = userInformation.UserName;
                        //  DB.DDCUST_MAST.DeleteOnSubmit(ddCustMaster);
                        DB.SubmitChanges();
                        message = PDMsg.DeletedSuccessfully;
                    }
                    else if (ddCustMaster == null)
                    {
                        message = PDMsg.NoRecordFound;
                    }
                }
            }

            catch (Exception ex)
            {
                ex.LogException();
                DB.DDCUST_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddCustMaster);

            }
            return true;
        }

        public bool CheckCodeisEsxists(decimal operCode)
        {
            try
            {


                DDCUST_MAST ddCustMaster = (from c in DB.DDCUST_MAST
                                            where c.CUST_CODE == operCode
                                             //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                            //Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false
                                            select c).SingleOrDefault<DDCUST_MAST>();
                if (ddCustMaster != null)
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
