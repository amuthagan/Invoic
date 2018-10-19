using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using LinqCache;

using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{

    public class CircularRolling : Essential
    {
        public CircularRolling(UserInformation user)
        {
            this.userInformation = user;
        }
        public DDCIR_THREAD_ROLL_MAC getCircularRollings(string costCenterCode)
        {
            DB.DDCIR_THREAD_ROLL_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DB.DDCIR_THREAD_ROLL_MAC);

            return (from e in DB.DDCIR_THREAD_ROLL_MAC
                    where e.COST_CENT_CODE == costCenterCode && (e.DELETE_FLAG == false || e.DELETE_FLAG == null)
                    select e).FirstOrDefault();
        }
        public bool saveCircularRolling(DDCIR_THREAD_ROLL_MAC circularroll)
        {
            string mode = "";
            bool submit = false;
            bool insert = false;
            bool update = false;
            DDCIR_THREAD_ROLL_MAC cirroll = new DDCIR_THREAD_ROLL_MAC();
            try
            {

                cirroll = (from o in DB.DDCIR_THREAD_ROLL_MAC
                           where o.COST_CENT_CODE == circularroll.COST_CENT_CODE //&& o.DELETE_FLAG == false 
                           select o).SingleOrDefault();
                if (cirroll == null)
                {
                    cirroll = new DDCIR_THREAD_ROLL_MAC();
                    mode = "New";
                    cirroll.COST_CENT_CODE = circularroll.COST_CENT_CODE;
                    cirroll.ROWID = Guid.NewGuid();
                    circularroll.DELETE_FLAG = false;
                    circularroll.ENTERED_BY = userInformation.UserName;
                    circularroll.ENTERED_DATE = DateTime.Now;
                    insert = true;
                }
                else
                {
                    circularroll.UPDATED_BY = userInformation.UserName;
                    circularroll.UPDATED_DATE = DateTime.Now;
                    update = true;
                }

                cirroll.BACKLASH_ELIMINATOR = circularroll.BACKLASH_ELIMINATOR;
                cirroll.DIE_MOVEMENT_BOTH = circularroll.DIE_MOVEMENT_BOTH;
                cirroll.MOTOR_POWER = circularroll.MOTOR_POWER;
                cirroll.MAX_PROD_DIA = circularroll.MAX_PROD_DIA;
                cirroll.MIN_PITCH = circularroll.MIN_PITCH;
                cirroll.MAX_PITCH = circularroll.MAX_PITCH;
                cirroll.MAX_TILT_ANGLE = circularroll.MAX_TILT_ANGLE;
                cirroll.MAX_ROLL_DIA = circularroll.MAX_ROLL_DIA;
                cirroll.MIN_PROD_DIA = Convert.ToDecimal(circularroll.MIN_PROD_DIA);
                cirroll.OUTBOARD_BEARINGS = circularroll.OUTBOARD_BEARINGS;
                cirroll.ROLL_PRESSURE = circularroll.ROLL_PRESSURE;
                cirroll.THROUGH_FEED = circularroll.THROUGH_FEED;
                cirroll.TYPE_WORKREST_BLADE = circularroll.TYPE_WORKREST_BLADE;

                if (mode == "New")
                {
                    DB.DDCIR_THREAD_ROLL_MAC.InsertOnSubmit(cirroll);
                }
                submit = true;
                DB.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (insert == true)
                    {
                        DB.DDCIR_THREAD_ROLL_MAC.DeleteOnSubmit(cirroll);
                    }
                    if (update == true)
                    {
                        DB.DDCIR_THREAD_ROLL_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, cirroll);
                    }
                }
                ex.LogException();
            }
            return false;
        }
    }
}
