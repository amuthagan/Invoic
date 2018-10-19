using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class CPMMasterBll : Essential
    {
        public CPMMasterBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public decimal GenerateSno(string dept)
        {
            decimal cpmSno = 0;
            try
            {
                cpmSno = (from o in DB.CONTROL_PLAN_MEMBER where o.DEPT == dept select o.SNO).Max();
                cpmSno = (Convert.ToInt32(cpmSno + 1));
                return cpmSno;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataView GetCPMMasterDeptList()
        {
            try
            {

                DataTable dataValue;
                //  DB.CONTROL_PLAN_MEMBER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.CONTROL_PLAN_MEMBER);

                dataValue = ToDataTableWithType((from c in DB.CONTROL_PLAN_MEMBER
                                                 where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)) && (c.DEPT != "")
                                                 orderby c.DEPT ascending
                                                 select new { c.DEPT }).Distinct().ToList());
                return dataValue.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataView GetCPMMMemberList(string dept)
        {
            DataTable dataValue;
            //  DB.CONTROL_PLAN_MEMBER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.CONTROL_PLAN_MEMBER);
            dataValue = ToDataTableWithType((from c in DB.CONTROL_PLAN_MEMBER
                                             // where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)) && (c.DEPT == dept) && (c.DEPT != "")
                                             where (c.DEPT == dept) && (c.DEPT != "")
                                             //orderby c.DEPT ascending
                                             orderby c.MEMBER ascending
                                             select new { c.MEMBER, c.SNO, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false) }).ToList());
            dataValue.Columns.Add("SNOT", typeof(int));
            for (int i = 0; i < dataValue.Rows.Count; i++)
            {
                dataValue.Rows[i]["SNOT"] = i + 1;
            }
            if (dataValue.Rows.Count > 0) dataValue.DefaultView.Sort = "SNOT asc";

            return dataValue.DefaultView;
        }
        public bool SaveCpmMaster(CPMMasterModel cpm, string dept, string member, decimal sno, ref string typ)
        {
            CONTROL_PLAN_MEMBER ddCpm = new CONTROL_PLAN_MEMBER();
            bool _status = false;
            try
            {
                ddCpm = (from c in DB.CONTROL_PLAN_MEMBER
                         where c.DEPT == dept && c.SNO == sno
                         //   && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                         select c).FirstOrDefault<CONTROL_PLAN_MEMBER>();


                if (ddCpm == null)
                {
                    try
                    {
                        ddCpm = new CONTROL_PLAN_MEMBER();
                        ddCpm.DEPT = dept;
                        ddCpm.MEMBER = member;
                        ddCpm.LOCATION = string.Empty;
                        ddCpm.SNO = sno;
                        ddCpm.DELETE_FLAG = cpm.IsActive;
                        ddCpm.ENTERED_DATE = DateTime.Now;
                        ddCpm.ENTERED_BY = userInformation.UserName;
                        ddCpm.ROWID = Guid.NewGuid();
                        DB.CONTROL_PLAN_MEMBER.InsertOnSubmit(ddCpm);
                        DB.SubmitChanges();
                        typ = "INS";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.CONTROL_PLAN_MEMBER.DeleteOnSubmit(ddCpm);
                    }

                }
                else
                {
                    try
                    {
                        ddCpm.DEPT = dept;
                        ddCpm.MEMBER = member;
                        ddCpm.LOCATION = string.Empty;
                        ddCpm.DELETE_FLAG = cpm.IsActive;
                        ddCpm.UPDATED_DATE = DateTime.Now;
                        ddCpm.UPDATED_BY = userInformation.UserName;
                        DB.SubmitChanges();
                        typ = "UPD";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.CONTROL_PLAN_MEMBER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddCpm);
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
                DB.CONTROL_PLAN_MEMBER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddCpm);

            }
            return _status;
        }

        public bool DeleteCpmTitle(string dept, string member)
        {
            CONTROL_PLAN_MEMBER ddCpmMaster = new CONTROL_PLAN_MEMBER();
            try
            {
                ddCpmMaster = (from c in DB.CONTROL_PLAN_MEMBER
                               where c.DEPT == dept && c.MEMBER == member
                               // && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                               select c).FirstOrDefault<CONTROL_PLAN_MEMBER>();

                if (ddCpmMaster != null)
                {

                    if (ddCpmMaster.DELETE_FLAG == true)
                    {
                        ddCpmMaster.DELETE_FLAG = false;
                    }
                    else
                    {
                        ddCpmMaster.DELETE_FLAG = true;
                    }

                    ddCpmMaster.UPDATED_DATE = DateTime.Now;
                    ddCpmMaster.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    return true;
                }
                else if (ddCpmMaster == null)
                {
                    return false;
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {

                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {

                throw ex.LogException();
            }
            return true;
        }
    }
}
