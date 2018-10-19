using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class GrinmachineBll : Essential
    {
        public GrinmachineBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool GetGrinMachine(GrinmachineModel grinMachine)
        {
            try
            {
                DDGRINDING_MAC ddnloc = (from o in DB.DDGRINDING_MAC
                                         where o.COST_CENT_CODE == grinMachine.COST_CENT_CODE
                                         select o).FirstOrDefault<DDGRINDING_MAC>();
                if (ddnloc != null)
                {
                    grinMachine.MIN_PROD_DIA = ddnloc.MIN_PROD_DIA;
                    grinMachine.MAX_PROD_DIA = ddnloc.MAX_PROD_DIA;
                    grinMachine.MAX_LEN_IN_PLUNGE = ddnloc.MAX_LEN_IN_PLUNGE;
                    grinMachine.MAX_GWHEEL_OD = ddnloc.MAX_GWHEEL_OD;
                    grinMachine.MAX_GWHEEL_THICKNESS = ddnloc.MAX_GWHEEL_THICKNESS;
                    grinMachine.GWHEEL_BORE = ddnloc.GWHEEL_BORE;
                    grinMachine.MAX_CWHEEL_OD = ddnloc.MAX_CWHEEL_OD;
                    grinMachine.CWHEEL_THICKNESS = ddnloc.CWHEEL_THICKNESS;
                    grinMachine.CWHEEL_BORE = ddnloc.CWHEEL_BORE;
                    grinMachine.MAX_WHEEL_TILT = ddnloc.MAX_WHEEL_TILT;
                    grinMachine.GWHEEL_CAM_REF_LEN = ddnloc.GWHEEL_CAM_REF_LEN;
                    grinMachine.CWHEEL_CAM_REF_LEN = ddnloc.CWHEEL_CAM_REF_LEN;
                    grinMachine.GWHEEL_CAM_LEN = ddnloc.GWHEEL_CAM_LEN;
                    grinMachine.CWHEEL_CAM_LENGTH = ddnloc.CWHEEL_CAM_LENGTH;
                    grinMachine.AUTO_DRESSING = ddnloc.AUTO_DRESSING;
                    grinMachine.FEED_SYSTEM = ddnloc.FEED_SYSTEM;
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

        public bool UpdateGrinMachine(GrinmachineModel grinMachine)
        {
            string mode = "";
            bool _status = false;

            DDGRINDING_MAC ddnloc = (from o in DB.DDGRINDING_MAC
                                     where o.COST_CENT_CODE == grinMachine.COST_CENT_CODE
                                     select o).FirstOrDefault<DDGRINDING_MAC>();
            try
            {
                if (ddnloc == null)
                {
                    ddnloc = new DDGRINDING_MAC();
                    mode = "New";
                    ddnloc.COST_CENT_CODE = grinMachine.COST_CENT_CODE;
                }

                ddnloc.MIN_PROD_DIA = grinMachine.MIN_PROD_DIA;
                ddnloc.MAX_PROD_DIA = grinMachine.MAX_PROD_DIA;
                ddnloc.MAX_LEN_IN_PLUNGE = grinMachine.MAX_LEN_IN_PLUNGE;
                ddnloc.MAX_GWHEEL_OD = grinMachine.MAX_GWHEEL_OD;
                ddnloc.MAX_GWHEEL_THICKNESS = grinMachine.MAX_GWHEEL_THICKNESS;
                ddnloc.GWHEEL_BORE = grinMachine.GWHEEL_BORE;
                ddnloc.MAX_CWHEEL_OD = grinMachine.MAX_CWHEEL_OD;
                ddnloc.CWHEEL_THICKNESS = grinMachine.CWHEEL_THICKNESS;
                ddnloc.CWHEEL_BORE = grinMachine.CWHEEL_BORE;
                ddnloc.MAX_WHEEL_TILT = grinMachine.MAX_WHEEL_TILT;
                ddnloc.GWHEEL_CAM_REF_LEN = grinMachine.GWHEEL_CAM_REF_LEN;
                ddnloc.CWHEEL_CAM_REF_LEN = grinMachine.CWHEEL_CAM_REF_LEN;
                ddnloc.GWHEEL_CAM_LEN = grinMachine.GWHEEL_CAM_LEN;
                ddnloc.CWHEEL_CAM_LENGTH = grinMachine.CWHEEL_CAM_LENGTH;
                ddnloc.AUTO_DRESSING = grinMachine.AUTO_DRESSING;
                ddnloc.FEED_SYSTEM = grinMachine.FEED_SYSTEM;

                if (mode == "New")
                {
                    ddnloc.ROWID = Guid.NewGuid();
                    DB.DDGRINDING_MAC.InsertOnSubmit(ddnloc);
                }

                DB.SubmitChanges();


                ddnloc = null;
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
                    DB.DDGRINDING_MAC.DeleteOnSubmit(ddnloc);
                }
                else
                {
                    DB.DDGRINDING_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddnloc);
                }
                _status = false;
                
            }
            return _status;
        }
    }
}
