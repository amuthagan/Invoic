using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class NylocmachineBll : Essential
    {
        public NylocmachineBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public bool GetNylocmachine(NylocmacModel nylocmach)
        {
            try
            {
                DDNYLOC_MAC ddnloc = (from o in DB.DDNYLOC_MAC
                                      where o.COST_CENT_CODE == nylocmach.COST_CENT_CODE
                                      select o).FirstOrDefault<DDNYLOC_MAC>();
                if (ddnloc != null)
                {
                    if (ddnloc.MIN_AF == null) ddnloc.MIN_AF = 0;
                    if (ddnloc.MAX_AF == null) ddnloc.MAX_AF = 0;
                    if (ddnloc.MAX_THICKNESS == null) ddnloc.MAX_THICKNESS = 0;
                    if (ddnloc.MAX_DIA == null) ddnloc.MAX_DIA = 0;
                    nylocmach.MIN_AF = ddnloc.MIN_AF;
                    nylocmach.MAX_AF = ddnloc.MAX_AF;
                    nylocmach.MAX_THICKNESS = ddnloc.MAX_THICKNESS;
                    nylocmach.MAX_DIA = ddnloc.MAX_DIA;
                    nylocmach.PROD_HEAD_TYPE = ddnloc.PROD_HEAD_TYPE;
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

        public bool UpdateNylocmachine(NylocmacModel nylocmach)
        {
            string mode = "";
            bool _status = false;

            DDNYLOC_MAC ddnloc = (from o in DB.DDNYLOC_MAC
                                  where o.COST_CENT_CODE == nylocmach.COST_CENT_CODE
                                  select o).SingleOrDefault<DDNYLOC_MAC>();
            try
            {


                if (ddnloc == null)
                {
                    ddnloc = new DDNYLOC_MAC();
                    mode = "New";
                    ddnloc.COST_CENT_CODE = nylocmach.COST_CENT_CODE;
                }

                ddnloc.COST_CENT_CODE = nylocmach.COST_CENT_CODE;
                ddnloc.MIN_AF = nylocmach.MIN_AF;
                ddnloc.MAX_AF = nylocmach.MAX_AF;
                ddnloc.MAX_THICKNESS = nylocmach.MAX_THICKNESS;
                ddnloc.MAX_DIA = nylocmach.MAX_DIA;
                ddnloc.PROD_HEAD_TYPE = nylocmach.PROD_HEAD_TYPE;

                if (mode == "New")
                {
                    ddnloc.ROWID = Guid.NewGuid();
                    DB.DDNYLOC_MAC.InsertOnSubmit(ddnloc);
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
                    DB.DDNYLOC_MAC.DeleteOnSubmit(ddnloc);
                }
                else
                {
                    DB.DDNYLOC_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddnloc);
                }
                _status = false;

            }
            return _status;
        }

    }
}
