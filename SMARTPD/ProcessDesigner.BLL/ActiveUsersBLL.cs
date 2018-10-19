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
    public class ActiveUsersBLL : Essential
    {
        public ActiveUsersBLL(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool LogIn(string username, string ipaddress, string hostname)
        {
            ActiveUser au = new ActiveUser();
            bool _status = false;
            au = (from a in DB.ActiveUser
                  where a.UserName == username && a.Login_Time != null && a.Logout_Time == null && a.IP_Address == ipaddress && a.Host_Name == hostname
                  select a).FirstOrDefault<ActiveUser>();
            if (au != null)
            {
                au.Logout_Time = DateTime.Now;
                DB.SubmitChanges();
            }

            try
            {
                au = (from a in DB.ActiveUser
                      where a.UserName == username && a.Login_Time == null
                      select a).FirstOrDefault<ActiveUser>();
                if (au == null)
                {
                    try
                    {
                        au = new ActiveUser();
                        au.UserName = username;
                        au.IP_Address = ipaddress;
                        au.Host_Name = hostname;
                        au.Login_Time = DateTime.Now;
                        au.Logout_Time = null;
                        DB.ActiveUser.InsertOnSubmit(au);
                        DB.SubmitChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.ActiveUser.DeleteOnSubmit(au);
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
                DB.ActiveUser.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, au);

            }
            return _status;
        }

        public bool LogOut(string username, string ipaddress, string hostname)
        {
            ActiveUser au = new ActiveUser();
            bool _status = false;
            try
            {
                au = (from a in DB.ActiveUser
                      where a.UserName == username && a.Login_Time != null && a.Logout_Time == null && a.IP_Address == ipaddress && a.Host_Name == hostname
                      select a).FirstOrDefault<ActiveUser>();
                if (au != null)
                {
                    try
                    {
                        au.Logout_Time = DateTime.Now;
                        DB.SubmitChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.ActiveUser.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, au);
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
                DB.ActiveUser.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, au);

            }
            return _status;
        }
    }
}
