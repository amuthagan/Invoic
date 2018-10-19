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
    public class ApplicationBll : Essential
    {
        public ApplicationBll(UserInformation userInfo)
        {
            this.userInformation = userInfo;
        }

        public int GetSerialNo(ApplicationModel appmodel)
        {
            int serialno = 1;
            List<APPLICATION> app = (from a in DB.APPLICATION
                                    where a.S_ == appmodel.SNo
                                    select a).ToList<APPLICATION>();

            if (app != null && app.Count > 0)
            {
                serialno = (from s in app
                            select s).Max(sno => sno.S_);
                serialno = serialno + 1;
            }           
            
            return serialno;
        }

        public bool AddEditApplication(ApplicationModel appmodel, ref string mode)
        {
            bool status = false;
            APPLICATION app = new APPLICATION();
            try
            {
                app = (from a in DB.APPLICATION
                       where a.S_ == appmodel.SNo
                       select a).FirstOrDefault<APPLICATION>();

                if (app == null)
                {
                    try
                    {
                        app = new APPLICATION();
                        app.S_ = appmodel.SNo;
                        app.NewApplication = appmodel.PSWApplication;
                        app.ENTERED_DATE = DateTime.Now;
                        app.ENTERED_BY = userInformation.UserName;
                        app.DELETE_FLAG = appmodel.Active;
                        DB.APPLICATION.InsertOnSubmit(app);
                        DB.SubmitChanges();
                        mode = "Add";
                        return true;
                    }
                    catch (Exception e)
                    {
                        e.LogException();
                        DB.APPLICATION.DeleteOnSubmit(app);
                    }
                }
                else
                {
                    try
                    {
                        app.NewApplication = appmodel.PSWApplication;
                        app.UPDATED_DATE = DateTime.Now;
                        app.UPDATED_BY = userInformation.UserName;
                        app.DELETE_FLAG = appmodel.Active;
                        DB.SubmitChanges();
                        mode = "Update";
                        return true;
                    }
                    catch (Exception e)
                    {
                        e.LogException();
                        DB.APPLICATION.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, app);
                    }
                }
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
            return status;
        }

        public DataView GetApplication()
        {
            DataTable dataValue;
            dataValue = ToDataTable((from c in DB.APPLICATION.AsEnumerable()
                                     where (!string.IsNullOrEmpty(c.NewApplication))
                                     orderby c.NewApplication ascending
                                     select new { c.S_, c.NewApplication, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false) }).ToList());
            return dataValue.DefaultView;
        }

        public bool DeletePswApplication(string application)
        {
            APPLICATION app = new APPLICATION();
            try
            {
                app = (from c in DB.APPLICATION
                       where c.NewApplication == application//   && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                       select c).FirstOrDefault<APPLICATION>();

                if (app != null)
                {
                    if (app.DELETE_FLAG == true)
                    {
                        app.DELETE_FLAG = false;
                    }
                    else
                    {
                        app.DELETE_FLAG = true;
                    }
                    //ddPswTitle.DELETE_FLAG = true;
                    app.UPDATED_DATE = DateTime.Now;
                    app.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    return true;
                }
                else if (app == null)
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
                DB.APPLICATION.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, app);

            }
            return true;
        }

        public bool ApplicationAddDuplicate(ApplicationModel app)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from a in DB.APPLICATION
                                  where a.NewApplication == app.PSWApplication && a.S_ != app.SNo
                                  //&& ((Convert.ToBoolean(Convert.ToInt16(a.DELETE_FLAG)) == false) || (a.DELETE_FLAG == null))
                                  select new { a.NewApplication }).ToList());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                        return false;
                }
                else
                {
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }

        public bool ApplicationEditDuplicate(ApplicationModel app)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from a in DB.APPLICATION
                                  where a.S_ != app.SNo && a.NewApplication == app.PSWApplication
                                  //&& ((Convert.ToBoolean(Convert.ToInt16(a.DELETE_FLAG)) == false) || (a.DELETE_FLAG == null))
                                  select new { a.NewApplication }).ToList());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                        return true;
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }
    }
}
