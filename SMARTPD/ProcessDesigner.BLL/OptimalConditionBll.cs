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
    public class OptimalConditionBll : Essential
    {
        public OptimalConditionBll(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }
        public DataTable GetOptimalMaster(string cost_cent_code)
        {
            DataTable dataValue;



            dataValue = ToDataTable((from C in DB.DDOPTIMAL_COND.AsEnumerable()
                                     where C.COST_CENT_CODE == cost_cent_code
                                     //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                     orderby C.COST_CENT_CODE ascending
                                     select new { C.COST_CENT_CODE, C.SER_NO, C.PART_REP_ADJ, C.AREA, C.OPTIMAL_COND, C.RESP, C.FREQUENCY, C.NORMAL, C.COUNTER_MEAS }).ToList());
            return dataValue;
        }

        public List<DDOPTIMAL_COND> GetOptimalMaster1(string cost_cent_code)
        {
            //new { C.COST_CENT_CODE, C.SER_NO, C.PART_REP_ADJ, C.AREA, C.OPTIMAL_COND, C.RESP, C.FREQUENCY, C.NORMAL, C.COUNTER_MEAS }


            return (from C in DB.DDOPTIMAL_COND
                    where C.COST_CENT_CODE == cost_cent_code
                    //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                    orderby C.COST_CENT_CODE ascending
                    select C).ToList<DDOPTIMAL_COND>();

        }
        public bool AddNewOptimalCondition(DataTable masterdata, string cost_cent_code, ref string message)
        {
            bool _status = false;
            string serNo = "0";
            try
            {

                foreach (DataRow row in masterdata.Rows)
                {
                    if (row["SER_NO"].ToString().Trim().Length == 0)
                    {
                        serNo = "0";
                    }
                    else
                    {
                        serNo = row["SER_NO"].ToString().Trim();
                    }
                    DDOPTIMAL_COND ddOptimalMaster = (from c in DB.DDOPTIMAL_COND
                                                      where c.COST_CENT_CODE == cost_cent_code && c.SER_NO == Convert.ToDecimal(serNo)
                                                      //    && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                                      //((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || c.DELETE_FLAG =null)
                                                      select c).SingleOrDefault<DDOPTIMAL_COND>();
                    //TextBox1.Text = row["ImagePath"].ToString();

                    if (ddOptimalMaster != null)
                    {
                        try
                        {
                            ddOptimalMaster.PART_REP_ADJ = row["PART_REP_ADJ"].ToString();
                            ddOptimalMaster.AREA = row["AREA"].ToString();
                            ddOptimalMaster.OPTIMAL_COND = row["OPTIMAL_COND"].ToString();
                            ddOptimalMaster.RESP = row["RESP"].ToString();
                            ddOptimalMaster.FREQUENCY = row["FREQUENCY"].ToString();
                            ddOptimalMaster.NORMAL = row["NORMAL"].ToString();
                            ddOptimalMaster.COUNTER_MEAS = row["COUNTER_MEAS"].ToString();
                            DB.SubmitChanges();
                            message = PDMsg.SavedSuccessfully;
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DDOPTIMAL_COND.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddOptimalMaster);

                        }



                    }
                    else if (ddOptimalMaster == null)
                    {
                        try
                        {
                            ddOptimalMaster = new DDOPTIMAL_COND();
                            ddOptimalMaster.COST_CENT_CODE = cost_cent_code;
                            ddOptimalMaster.SER_NO = Convert.ToDecimal(row["SER_NO"].ToString());
                            ddOptimalMaster.PART_REP_ADJ = row["PART_REP_ADJ"].ToString();
                            ddOptimalMaster.AREA = row["AREA"].ToString();
                            ddOptimalMaster.OPTIMAL_COND = row["OPTIMAL_COND"].ToString();
                            ddOptimalMaster.RESP = row["RESP"].ToString();
                            ddOptimalMaster.FREQUENCY = row["FREQUENCY"].ToString();
                            ddOptimalMaster.NORMAL = row["NORMAL"].ToString();
                            ddOptimalMaster.COUNTER_MEAS = row["COUNTER_MEAS"].ToString();
                            ddOptimalMaster.ROWID = Guid.NewGuid();
                            DB.DDOPTIMAL_COND.InsertOnSubmit(ddOptimalMaster);
                            DB.SubmitChanges();
                            message = PDMsg.SavedSuccessfully;
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            DB.DDOPTIMAL_COND.DeleteOnSubmit(ddOptimalMaster);

                        }

                    }
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
            return _status;
        }

    }
}
