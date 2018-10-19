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
    public class CopyTurningMacBll : Essential
    {
        public CopyTurningMacBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public bool GetCopyTurningMac(CopyTurningMacModel copyturning)
        {
            try
            {
                DDCOPY_TURN_MAC ddRolling = (from o in DB.DDCOPY_TURN_MAC
                                             where o.COST_CENT_CODE == copyturning.COST_CENT_CODE
                                             select o).SingleOrDefault<DDCOPY_TURN_MAC>();
                if (ddRolling != null)
                {
                    copyturning.MACHINE_TYPE = ddRolling.MACHINE_TYPE;
                    copyturning.WORKHOLDER_TYPE = ddRolling.WORKHOLDER_TYPE;
                    copyturning.MAX_MANDREL_LEN = ddRolling.MAX_MANDREL_LEN.ToString().ToDecimalValue();
                    copyturning.MIN_PROD_DIA = ddRolling.MIN_PROD_DIA.ToString().ToDecimalValue();
                    copyturning.MAX_PROD_DIA = ddRolling.MAX_PROD_DIA.ToString().ToDecimalValue();
                    copyturning.MAX_PROD_LEN = ddRolling.MAX_PROD_LEN.ToString().ToDecimalValue();
                    copyturning.MIN_PROD_LEN = ddRolling.MIN_PROD_LEN.ToString().ToDecimalValue();
                    copyturning.SPINDLE_SPEED = ddRolling.SPINDLE_SPEED.ToString().ToDecimalValue();
                    copyturning.FEED_RESTRICTIONS = ddRolling.FEED_RESTRICTIONS;
                    copyturning.TURRET_STATIONS = ddRolling.TURRET_STATIONS.ToString().ToDecimalValue();
                    copyturning.COOLANT = ddRolling.COOLANT;
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

        public bool UpdateCopyTurningMac(CopyTurningMacModel copyturning)
        {
            string mode = "";
            bool _status = false;
            DDCOPY_TURN_MAC ddRolling = new DDCOPY_TURN_MAC();

            try
            {

                ddRolling = (from o in DB.DDCOPY_TURN_MAC
                             where o.COST_CENT_CODE == copyturning.COST_CENT_CODE
                             select o).SingleOrDefault<DDCOPY_TURN_MAC>();
                if (ddRolling == null)
                {
                    ddRolling = new DDCOPY_TURN_MAC();
                    mode = "New";
                    ddRolling.COST_CENT_CODE = copyturning.COST_CENT_CODE;
                }

                ddRolling.MACHINE_TYPE = copyturning.MACHINE_TYPE;
                ddRolling.WORKHOLDER_TYPE = copyturning.WORKHOLDER_TYPE;
                ddRolling.MAX_MANDREL_LEN = copyturning.MAX_MANDREL_LEN.ToString().ToDecimalValue();
                ddRolling.MIN_PROD_DIA = copyturning.MIN_PROD_DIA.ToString().ToDecimalValue();
                ddRolling.MAX_PROD_DIA = copyturning.MAX_PROD_DIA.ToString().ToDecimalValue();
                ddRolling.MAX_PROD_LEN = copyturning.MAX_PROD_LEN.ToString().ToDecimalValue();
                ddRolling.MIN_PROD_LEN = copyturning.MIN_PROD_LEN.ToString().ToDecimalValue();
                ddRolling.SPINDLE_SPEED = copyturning.SPINDLE_SPEED.ToString().ToDecimalValue();
                ddRolling.FEED_RESTRICTIONS = copyturning.FEED_RESTRICTIONS;
                ddRolling.TURRET_STATIONS = copyturning.TURRET_STATIONS.ToString().ToDecimalValue();
                ddRolling.COOLANT = copyturning.COOLANT;

                if (mode == "New")
                {
                    ddRolling.ROWID = Guid.NewGuid();
                    DB.DDCOPY_TURN_MAC.InsertOnSubmit(ddRolling);
                }
                DB.SubmitChanges();

                _status = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                if (mode == "New")
                {
                    DB.DDCOPY_TURN_MAC.DeleteOnSubmit(ddRolling);
                }
                else if (mode != "New")
                {
                    DB.DDCOPY_TURN_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddRolling);
                }
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                
            }
            catch (Exception ex)
            {
                ex.LogException();
                if (mode == "New")
                {
                    DB.DDCOPY_TURN_MAC.DeleteOnSubmit(ddRolling);
                }
                else if (mode != "New")
                {
                    DB.DDCOPY_TURN_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddRolling);
                }
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.OverwriteCurrentValues);


                
            }
            return _status;
        }
    }
}
