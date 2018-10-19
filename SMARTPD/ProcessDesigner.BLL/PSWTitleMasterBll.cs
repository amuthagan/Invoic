using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using System.ComponentModel;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class PSWTitleMasterBll : Essential
    {
        public PSWTitleMasterBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public DataView GetPswNameList()
        {
            DataTable dataValue;
            // DB.PSW_WAR_TITLE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.PSW_WAR_TITLE);
            dataValue = ToDataTable((from c in DB.PSW_WAR_TITLE.AsEnumerable()
                                     //where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)) && (!string.IsNullOrEmpty(c.NAME))
                                     where (!string.IsNullOrEmpty(c.NAME))
                                     orderby c.NAME ascending
                                     select new { c.NAME, c.TITLE, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false) }).ToList());
            return dataValue.DefaultView;
        }

        public bool DeletePswTitle(string pswName, string pswTitle)
        {
            PSW_WAR_TITLE ddPswTitle = new PSW_WAR_TITLE();
            try
            {
                ddPswTitle = (from c in DB.PSW_WAR_TITLE
                              where c.NAME == pswName && c.TITLE == pswTitle
                              //   && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                              select c).FirstOrDefault<PSW_WAR_TITLE>();

                if (ddPswTitle != null)
                {
                    if (ddPswTitle.DELETE_FLAG == true)
                    {
                        ddPswTitle.DELETE_FLAG = false;
                    }
                    else
                    {
                        ddPswTitle.DELETE_FLAG = true;
                    }
                    //ddPswTitle.DELETE_FLAG = true;
                    ddPswTitle.UPDATED_DATE = DateTime.Now;
                    ddPswTitle.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    return true;
                }
                else if (ddPswTitle == null)
                {
                    return false;
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                // DB.PSW_WAR_TITLE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddPswTitle);

                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.PSW_WAR_TITLE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddPswTitle);

            }
            return true;
        }
        public bool SavePswMasterTitle(PSWTitleMasterModel psw, string name, string title, bool buttType, ref string type)
        {
            bool _status = false;
            PSW_WAR_TITLE ddPswTitle = new PSW_WAR_TITLE();
            try
            {
                ddPswTitle = (from c in DB.PSW_WAR_TITLE
                              where c.NAME == name
                              //    && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                              select c).FirstOrDefault<PSW_WAR_TITLE>();

                if (ddPswTitle == null)
                {
                    try
                    {
                        ddPswTitle = new PSW_WAR_TITLE();
                        ddPswTitle.NAME = (string)name;
                        ddPswTitle.TITLE = (string)title;
                        ddPswTitle.LOCATION = string.Empty;
                        ddPswTitle.DELETE_FLAG = psw.IsActive;
                        ddPswTitle.ENTERED_DATE = DateTime.Now;
                        ddPswTitle.ENTERED_BY = userInformation.UserName;
                        ddPswTitle.ROWID = Guid.NewGuid();
                        DB.PSW_WAR_TITLE.InsertOnSubmit(ddPswTitle);
                        DB.SubmitChanges();
                        type = PDMsg.SavedSuccessfully;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PSW_WAR_TITLE.DeleteOnSubmit(ddPswTitle);
                    }

                }
                else
                {
                    try
                    {
                        if (buttType == true)
                        {
                            PSW_WAR_TITLE pswTitle = (from c in DB.PSW_WAR_TITLE
                                                      where c.NAME == name
                                                      select c).FirstOrDefault<PSW_WAR_TITLE>();

                            if (pswTitle != null)
                            {
                                type = PDMsg.AlreadyExists("Name");
                                return false;
                            }
                        }

                        ddPswTitle.NAME = (string)name;
                        ddPswTitle.TITLE = (string)title;
                        ddPswTitle.DELETE_FLAG = psw.IsActive;
                        ddPswTitle.UPDATED_DATE = DateTime.Now;
                        ddPswTitle.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                        type = PDMsg.UpdatedSuccessfully;
                        return true;

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.PSW_WAR_TITLE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddPswTitle);
                    }

                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                //  DB.PSW_WAR_TITLE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddPswTitle);
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
