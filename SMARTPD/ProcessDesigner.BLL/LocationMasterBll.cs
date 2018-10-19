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
    public class LocationMasterBll : Essential
    {
        public LocationMasterBll(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }

        public DataView GetLocationMaster()
        {
            DataTable dataValue;

            dataValue = ToDataTable((from c in DB.DDLOC_MAST.AsEnumerable()
                                     where c.LOC_CODE.ToValueAsString().Trim() != ""
                                     && c.LOCATION.ToValueAsString().Trim() != ""
                                     orderby c.LOC_CODE ascending
                                     select new { c.LOC_CODE, c.LOCATION, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false) }).ToList());
            return dataValue.DefaultView;
        }

        public bool AddNewLocationMaster(bool isActive, string operCode, string operDesc, string nextAction, ref string message)
        {
            bool _status = false;

            DDLOC_MAST ddLocMaster = (from c in DB.DDLOC_MAST
                                      //   where c.LOC_CODE == operCode
                                      where (c.LOC_CODE == operCode && operDesc.ToUpper() == operDesc.ToUpper())
                                      //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                      select c).FirstOrDefault<DDLOC_MAST>();

            try
            {

                if (nextAction == "ADD")
                {
                    if (ddLocMaster == null)
                    {
                        ddLocMaster = new DDLOC_MAST();
                        ddLocMaster.LOC_CODE = (string)operCode;
                        ddLocMaster.LOCATION = (string)operDesc;
                        ddLocMaster.DELETE_FLAG = isActive;
                        ddLocMaster.ENTERED_DATE = DateTime.Now;
                        ddLocMaster.ENTERED_BY = userInformation.UserName;
                        ddLocMaster.ROWID = Guid.NewGuid();
                        DB.DDLOC_MAST.InsertOnSubmit(ddLocMaster);
                        DB.SubmitChanges();
                        message = PDMsg.SavedSuccessfully;
                        //"Records Added sucessfully";
                    }
                    else
                    {
                        //if (ddLocMaster.DELETE_FLAG != true)
                        //{
                        //    ddLocMaster.LOCATION = (string)operDesc;
                        //    ddLocMaster.UPDATED_DATE = DateTime.Now;
                        //    ddLocMaster.DELETE_FLAG = isActive;
                        //    ddLocMaster.UPDATED_BY = userInformation.UserName;
                        //    DB.SubmitChanges();
                        //    message = PDMsg.SavedSuccessfully;
                        //}
                        //else
                        //{


                        DDLOC_MAST ddLocMaster1 = (from c in DB.DDLOC_MAST
                                                   where c.LOC_CODE == operCode
                                                   //    where (c.LOC_CODE == operCode || operDesc.ToUpper().Replace(" ", "") == c.LOCATION.ToUpper().Replace(" ", ""))
                                                   //  && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                   select c).FirstOrDefault<DDLOC_MAST>();

                        if (ddLocMaster1 != null)
                        {
                            message = PDMsg.AlreadyExists("Location Code");
                            return false;
                        }
                        //else
                        //{
                        //    message = PDMsg.AlreadyExists("Location Desc");
                        //    return false;
                        //}

                    }

                }
                else if (nextAction == "EDIT")
                {
                    DDLOC_MAST ddLocMaster1 = (from c in DB.DDLOC_MAST
                                               where c.LOC_CODE != operCode && c.LOCATION == operDesc
                                               select c).FirstOrDefault<DDLOC_MAST>();

                    //if (ddLocMaster1 != null)
                    //{
                    //    message = PDMsg.AlreadyExists("Location Desc");
                    //    return false;
                    //}

                    ddLocMaster.LOCATION = (string)operDesc;
                    ddLocMaster.UPDATED_DATE = DateTime.Now;
                    ddLocMaster.DELETE_FLAG = isActive;
                    ddLocMaster.UPDATED_BY = userInformation.UserName;
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
                    DB.DDLOC_MAST.DeleteOnSubmit(ddLocMaster);
                }
                else if (nextAction == "EDIT")
                {
                    DB.DDLOC_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddLocMaster);
                }

            }
            return _status;
        }

        public bool DeleteLocationCode(string operCode, string nextAction, ref string message)
        {

            DDLOC_MAST ddLocMaster = (from c in DB.DDLOC_MAST
                                      where c.LOC_CODE == operCode
                                      // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                      select c).SingleOrDefault<DDLOC_MAST>();
            try
            {

                if (nextAction == "DELETE")
                {
                    if (ddLocMaster != null)
                    {

                        if (ddLocMaster.DELETE_FLAG == true)
                        {
                            ddLocMaster.DELETE_FLAG = false;
                        }
                        else
                        {
                            ddLocMaster.DELETE_FLAG = true;
                        }

                        //ddLocMaster.DELETE_FLAG = true;
                        ddLocMaster.UPDATED_DATE = DateTime.Now;
                        ddLocMaster.UPDATED_BY = userInformation.UserName;
                        // DB.DDLOC_MAST.DeleteOnSubmit(ddLocMaster);
                        DB.SubmitChanges();
                        message = PDMsg.DeletedSuccessfully;
                    }
                    else if (ddLocMaster == null)
                    {
                        message = PDMsg.NoRecordFound;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.DDLOC_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddLocMaster);

            }
            return true;
        }
    }
}
