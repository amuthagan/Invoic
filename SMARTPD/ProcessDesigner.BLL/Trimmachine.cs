using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class Trimmachine : Essential
    {
        public Trimmachine(UserInformation user)
        {
            this.userInformation = user;
        }
        public DDTRIMMING_MAC getTrimmachines(string costCenterCode)
        {

            return (from e in DB.DDTRIMMING_MAC
                    where e.COST_CENT_CODE == costCenterCode && (e.DELETE_FLAG == false || e.DELETE_FLAG == null)
                    select e).FirstOrDefault();
        }
        public bool saveTrimmachine(DDTRIMMING_MAC trimmac)
        {
            string mode = "";
            DDTRIMMING_MAC trimmc = (from o in DB.DDTRIMMING_MAC
                                     where o.COST_CENT_CODE == trimmac.COST_CENT_CODE// && o.DELETE_FLAG == false 
                                     select o).FirstOrDefault();
            try
            {

                if (trimmc == null)
                {
                    trimmc = new DDTRIMMING_MAC();
                    mode = "New";
                    trimmc.COST_CENT_CODE = trimmac.COST_CENT_CODE;
                    trimmc.ROWID = Guid.NewGuid();
                    trimmac.DELETE_FLAG = false;
                    trimmac.ENTERED_BY = userInformation.UserName;
                    trimmac.ENTERED_DATE = DateTime.Now;
                }
                else
                {
                    trimmac.UPDATED_BY = userInformation.UserName;
                    trimmac.UPDATED_DATE = DateTime.Now;
                }

                trimmc.FEED_TYPE = trimmac.FEED_TYPE;
                trimmc.MAX_AF = trimmac.MAX_AF;
                trimmc.MOTOR_POWER = trimmac.MOTOR_POWER;
                trimmc.MAX_PROD_DIA = trimmac.MAX_PROD_DIA;
                trimmc.MAX_SHANK_LEN = trimmac.MAX_SHANK_LEN;
                trimmc.TRIM_DIE_LEN = trimmac.TRIM_DIE_LEN;
                trimmc.TRIM_DIE_OD = trimmac.TRIM_DIE_OD;
                trimmc.TRIM_PUNCH_LEN = trimmac.TRIM_PUNCH_LEN;
                trimmc.TRIM_PUNCH_OD = trimmac.TRIM_PUNCH_OD;

                if (mode == "New")
                {
                    trimmc.ROWID = Guid.NewGuid();
                    DB.DDTRIMMING_MAC.InsertOnSubmit(trimmc);
                }

                DB.SubmitChanges();


                trimmc = null;

                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                
            }
            catch (Exception ex)
            {
                ex.LogException();
                if (mode == "New")
                {
                    DB.DDTRIMMING_MAC.DeleteOnSubmit(trimmc);
                }
                else
                {
                    DB.DDTRIMMING_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, trimmc);
                }
                
            }

            return false;
        }
    }
}
