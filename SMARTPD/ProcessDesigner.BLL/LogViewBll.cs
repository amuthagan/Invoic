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
using System.Net;

namespace ProcessDesigner.BLL
{
    public class LogViewBll : Essential
    {
        public DateTime Accdate_datetime;
        private UserInformation _userInformation;
        public LogViewBll(UserInformation userInformatio)
        {
            this.userInformation = userInformatio;
            _userInformation = userInformation;
        }

       //original
        //public bool GetLogDetails(LogModel lm, PCCSModel pm)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = ToDataTable((from n in DB.LOGS
        //                          where n.PART_NO == pm.PartNo
        //                         // orderby (n.ACC_DATE.ToDateTimeValue()) descending //new
        //                          select new { n.PART_NO, ACC_DATE = String.Format("{0:d/M/yyyy HH:mm:ss tt}", n.ACC_DATE), n.IPADDRESS, n.SHEET_ACCESSED, n.UNAME }).ToList());

        //        ////new
        //        //dt = ToDataTable((from n in DB.LOGS
        //        //                  where n.PART_NO == pm.PartNo

        //        //                  select new { n.PART_NO, ACC_DATE = String.Format("{0:d/M/yyyy HH:mm:ss tt}", n.ACC_DATE), n.IPADDRESS, n.SHEET_ACCESSED, n.UNAME }).ToList());


        //        ////end new
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            lm.LogDetails = dt.DefaultView;
        //        }
        //        else
        //        {
        //            lm.LogDetails = null;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //}
        //original


        //new
        public bool GetLogDetails(LogModel lm, PCCSModel pm)
        {
            try
            {

                DataTable dt = new DataTable();
                StringBuilder sbsql = new StringBuilder();
                //original
                sbsql.Append("select n.PART_NO as PART_NO, n.ACC_DATE as ACC_DATE, n.IPADDRESS as IPADDRESS, n.SHEET_ACCESSED as SHEET_ACCESSED, n.UNAME as UNAME from LOGS n where n.PART_NO ='" + pm.PartNo + "'order by convert(varchar,n.ACC_DATE,101) desc ");
                //new
                //sbsql.Append("select n.PART_NO as PART_NO,ACC_DATE = String.Format({0:d/M/yyyy HH:mm:ss tt}, n.ACC_DATE) as ACC_DATE, n.IPADDRESS as IPADDRESS, n.SHEET_ACCESSED as SHEET_ACCESSED, n.UNAME as UNAME from LOGS n where n.PART_NO ='" + pm.PartNo + "'order by convert(Datetime,n.ACC_DATE,104) desc ");
                dt = ToDataTable(DB.ExecuteQuery<LogNewModel>(sbsql.ToString()).ToList());
                if (dt != null && dt.Rows.Count > 0)
                {
                    lm.LogDetails = dt.DefaultView;
                }
                else
                {
                    lm.LogDetails = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }
        //end

        public bool SaveLog(string partno, string sheet_accessed)
        {
            LOGS logs = new LOGS();
            try
            {
                string hostname = Dns.GetHostName();
                string ipaddress = Dns.GetHostByName(hostname).AddressList[0].ToString();
                string userInformation = _userInformation.UserName;
                try
                {
                    logs = new LOGS();
                    logs.UNAME = userInformation;
                    logs.IPADDRESS = ipaddress;
                    logs.ACC_DATE = String.Format("{0:d/M/yyyy HH:mm:ss tt}", _userInformation.Dal.ServerDateTime); //original
                   
                    logs.PART_NO = partno;
                    logs.SHEET_ACCESSED = sheet_accessed;
                    DB.LOGS.InsertOnSubmit(logs);
                    DB.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    DB.LOGS.DeleteOnSubmit(logs);
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.LOGS.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, logs);
            }
            return true;
        }
    }
}
