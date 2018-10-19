using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
//using LinqCache;

namespace ProcessDesigner.BLL
{
    public class Tapmachine : Essential
    {
        public Tapmachine()
        {

        }
        public Tapmachine(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DDTAPPING_MAC getTapmachines(string costCenterCode)
        {

            return (from e in DB.DDTAPPING_MAC
                    where e.COST_CENT_CODE == costCenterCode && e.DELETE_FLAG == false
                    select e).FirstOrDefault();

        }
        public bool saveTapmachine(DDTAPPING_MAC tapmac)
        {
            string mode = "";

            DDTAPPING_MAC tapm = (from o in DB.DDTAPPING_MAC
                                  where o.COST_CENT_CODE == tapmac.COST_CENT_CODE && o.DELETE_FLAG == false
                                  select o).FirstOrDefault();
            try
            {
                if (tapm == null)
                {
                    tapm = new DDTAPPING_MAC();
                    mode = "New";
                    tapm.COST_CENT_CODE = tapmac.COST_CENT_CODE;
                    tapm.DELETE_FLAG = false;
                    tapm.ENTERED_BY = userInformation.UserName;
                    tapm.ENTERED_DATE = DateTime.Now;
                    tapm.ROWID = Guid.NewGuid();
                }
                else
                {
                    tapm.UPDATED_BY = userInformation.UserName;
                    tapm.UPDATED_DATE = DateTime.Now;
                }

                tapm.MIN_TAP_SIZE = tapmac.MIN_TAP_SIZE;
                tapm.MAX_TAP_SIZE = tapmac.MAX_TAP_SIZE;
                tapm.MOTOR_POWER = tapmac.MOTOR_POWER;
                tapm.NO_OF_SPINDLES = tapmac.NO_OF_SPINDLES;
                tapm.PUSH_STORKE_SHAFT_SPEED = tapmac.PUSH_STORKE_SHAFT_SPEED;

                if (mode == "New")
                {
                    DB.DDTAPPING_MAC.InsertOnSubmit(tapm);
                }

                DB.SubmitChanges();

                tapm = null;

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
                    DB.DDTAPPING_MAC.DeleteOnSubmit(tapm);
                }
                else
                {
                    DB.DDTAPPING_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, tapm);
                }
                
            }
            return false;
        }
    }
}
