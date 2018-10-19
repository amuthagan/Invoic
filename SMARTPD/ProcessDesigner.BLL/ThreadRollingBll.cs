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
    public class ThreadRollingBll : Essential
    {
        public ThreadRollingBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool GetThreadRollingMachine(ThreadRollingModel threadrolling)
        {
            try
            {
                DDFLAT_THREAD_ROLL_MAC ddRolling = (from o in DB.DDFLAT_THREAD_ROLL_MAC
                                                    where o.COST_CENT_CODE == threadrolling.COST_CENT_CODE
                                                    select o).FirstOrDefault<DDFLAT_THREAD_ROLL_MAC>();
                if (ddRolling != null)
                {

                    threadrolling.COST_CENT_CODE = ddRolling.COST_CENT_CODE;
                    threadrolling.MAX_THREAD_DIA = ddRolling.MAX_THREAD_DIA;
                    threadrolling.MAX_THREAD_LEN = ddRolling.MAX_THREAD_LEN;
                    threadrolling.MAX_SHANK_LEN = ddRolling.MAX_SHANK_LEN;
                    threadrolling.MOVING_DIE_LEN = ddRolling.MOVING_DIE_LEN;
                    threadrolling.MOVING_DIE_WIDTH = ddRolling.MOVING_DIE_WIDTH;
                    threadrolling.MOVING_DIE_HEIGHT = ddRolling.MOVING_DIE_HEIGHT;
                    threadrolling.FIXED_DIE_LEN = ddRolling.FIXED_DIE_LEN;
                    threadrolling.FIXED_DIE_WIDTH = ddRolling.FIXED_DIE_WIDTH;
                    threadrolling.FIXED_DIE_HEIGHT = ddRolling.FIXED_DIE_HEIGHT;
                    threadrolling.DIE_POCKET_DEPTH = ddRolling.DIE_POCKET_DEPTH;
                    threadrolling.MAX_HARDNESS = ddRolling.MAX_HARDNESS;
                    threadrolling.MIN_HARDNESS = ddRolling.MIN_HARDNESS;
                    threadrolling.COOLANT = ddRolling.COOLANT;
                    threadrolling.FEED_METHOD = ddRolling.FEED_METHOD;
                    threadrolling.FEEDING_MECHANISH = ddRolling.FEEDING_MECHANISH;
                    threadrolling.MOTOR_POWER = ddRolling.MOTOR_POWER;
                    threadrolling.REMARKS = ddRolling.REMARKS;
                    return true;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool UpdateThreadRollingMachine(ThreadRollingModel threadrolling)
        {
            string mode = "";
            bool _status = false;
            DDFLAT_THREAD_ROLL_MAC ddRolling = (from o in DB.DDFLAT_THREAD_ROLL_MAC
                                                where o.COST_CENT_CODE == threadrolling.COST_CENT_CODE
                                                select o).SingleOrDefault<DDFLAT_THREAD_ROLL_MAC>();
            try
            {
                if (ddRolling == null)
                {
                    ddRolling = new DDFLAT_THREAD_ROLL_MAC();
                    mode = "New";
                    ddRolling.COST_CENT_CODE = threadrolling.COST_CENT_CODE;
                }


                ddRolling.COST_CENT_CODE = threadrolling.COST_CENT_CODE;
                ddRolling.MAX_THREAD_DIA = threadrolling.MAX_THREAD_DIA;

                ddRolling.MAX_THREAD_LEN = threadrolling.MAX_THREAD_LEN;
                ddRolling.MAX_SHANK_LEN = threadrolling.MAX_SHANK_LEN;
                ddRolling.MOVING_DIE_LEN = threadrolling.MOVING_DIE_LEN;
                ddRolling.MOVING_DIE_WIDTH = threadrolling.MOVING_DIE_WIDTH;
                ddRolling.MOVING_DIE_HEIGHT = threadrolling.MOVING_DIE_HEIGHT;
                ddRolling.FIXED_DIE_LEN = threadrolling.FIXED_DIE_LEN;
                ddRolling.FIXED_DIE_WIDTH = threadrolling.FIXED_DIE_WIDTH;
                ddRolling.FIXED_DIE_HEIGHT = threadrolling.FIXED_DIE_HEIGHT;
                ddRolling.DIE_POCKET_DEPTH = threadrolling.DIE_POCKET_DEPTH;
                ddRolling.MAX_HARDNESS = threadrolling.MAX_HARDNESS;
                ddRolling.MIN_HARDNESS = threadrolling.MIN_HARDNESS;
                ddRolling.COOLANT = threadrolling.COOLANT;
                ddRolling.FEED_METHOD = threadrolling.FEED_METHOD;
                ddRolling.FEEDING_MECHANISH = threadrolling.FEEDING_MECHANISH;
                ddRolling.MOTOR_POWER = threadrolling.MOTOR_POWER;
                ddRolling.REMARKS = threadrolling.REMARKS;

                if (mode == "New")
                {
                    ddRolling.ROWID = Guid.NewGuid();
                    DB.DDFLAT_THREAD_ROLL_MAC.InsertOnSubmit(ddRolling);
                }

                DB.SubmitChanges();

                ddRolling = null;

                _status = true;
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
                    DB.DDFLAT_THREAD_ROLL_MAC.DeleteOnSubmit(ddRolling);
                }
                else
                {
                    DB.DDFLAT_THREAD_ROLL_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddRolling);
                }
                
            }
            return _status;
        }
    }
}

//ddflat_thread_roll_mac


