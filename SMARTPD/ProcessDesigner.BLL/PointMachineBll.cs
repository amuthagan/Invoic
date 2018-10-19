using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class PointMachineBll : Essential
    {
        public PointMachineBll(UserInformation userInfo)
        {
            this.userInformation = userInfo;
        }

        public bool GetPointMachine(PointMachineModel pointmach)
        {
            try
            {
                DDPOINTING_MAC ddPointMc = (from o in DB.DDPOINTING_MAC
                                            where o.COST_CENT_CODE == pointmach.COST_CENT_CODE
                                            select o).FirstOrDefault<DDPOINTING_MAC>();
                if (ddPointMc != null)
                {
                    pointmach.MAX_PROD_DIA = ddPointMc.MAX_PROD_DIA;
                    pointmach.MAX_PROD_LEN = ddPointMc.MAX_PROD_LEN;
                    pointmach.CUTTER_TYPE = ddPointMc.CUTTER_TYPE;

                    pointmach.NO_OF_CUTTERS = ddPointMc.NO_OF_CUTTERS;
                    pointmach.ADJUSTMENT_TYPE = ddPointMc.ADJUSTMENT_TYPE;
                    pointmach.FEED_TYPE = ddPointMc.FEED_TYPE;
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

        public bool UpdatePointMachine(PointMachineModel pointmach)
        {
            string mode = "";
            bool _status = false;

            DDPOINTING_MAC ddPointMc = (from o in DB.DDPOINTING_MAC
                                        where o.COST_CENT_CODE == pointmach.COST_CENT_CODE
                                        select o).SingleOrDefault<DDPOINTING_MAC>();
            try
            {

                if (ddPointMc == null)
                {
                    ddPointMc = new DDPOINTING_MAC();
                    mode = "New";
                    ddPointMc.COST_CENT_CODE = pointmach.COST_CENT_CODE;
                }
                ddPointMc.MAX_PROD_DIA = pointmach.MAX_PROD_DIA;
                ddPointMc.MAX_PROD_LEN = pointmach.MAX_PROD_LEN;
                ddPointMc.CUTTER_TYPE = pointmach.CUTTER_TYPE;

                ddPointMc.NO_OF_CUTTERS = pointmach.NO_OF_CUTTERS;
                ddPointMc.ADJUSTMENT_TYPE = pointmach.ADJUSTMENT_TYPE;
                ddPointMc.FEED_TYPE = pointmach.FEED_TYPE;

                if (mode == "New")
                {
                    ddPointMc.ROWID = Guid.NewGuid();
                    DB.DDPOINTING_MAC.InsertOnSubmit(ddPointMc);
                }

                DB.SubmitChanges();


                ddPointMc = null;
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
                    DB.DDPOINTING_MAC.DeleteOnSubmit(ddPointMc);
                }
                else
                {
                    DB.DDPOINTING_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddPointMc);
                }
                _status = false;
            }
            return _status;
        }
    }
}
