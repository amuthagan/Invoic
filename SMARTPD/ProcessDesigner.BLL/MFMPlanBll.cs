using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class MFMPlanBll : Essential
    {
        public MFMPlanBll(UserInformation userinfo)
        {
            this.userInformation = userinfo;
        }

        public bool GetProductMaster(MFMPlanModel mfmplan)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where o.DOC_REL_DATE != null && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                if (dt != null)
                {

                    mfmplan.DVProductMaster = dt.DefaultView;
                }
                else
                {
                    mfmplan.DVProductMaster = null;
                }

                dt = ToDataTable((from o in DB.DDUSER_LIST
                                  where (o.USER_GROUP == "Designer" || o.USER_GROUP == "Manager")
                                  select new { o.LOGIN, o.USER_FULL_NAME }).ToList());
                if (dt != null)
                {

                    mfmplan.DVUsers = dt.DefaultView;
                }
                else
                {
                    mfmplan.DVUsers = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool RetreieveCustomerName(MFMPlanModel mfmplan)
        {
            try
            {
                var innerQuery = (from o in DB.PRD_CIREF
                                  where o.PART_NO == mfmplan.PART_NO && o.CURRENT_CIREF == true
                                  select o.CI_REF).Distinct();

                var ciinfo = (from a in DB.DDCI_INFO
                              join b in DB.DDCUST_MAST on a.CUST_CODE equals b.CUST_CODE
                              where innerQuery.Contains(a.CI_REFERENCE)
                              select new { b.CUST_NAME }).FirstOrDefault();

                if (ciinfo != null)
                {
                    mfmplan.CUST_NAME = ciinfo.CUST_NAME;
                }
                else
                {
                    mfmplan.CUST_NAME = "";
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool RetrieveMFMmast(MFMPlanModel mfmplan)
        {
            try
            {
                mfmplan.MFM_MASTER = (from o in DB.MFM_MAST
                                      where o.PART_NO == mfmplan.PART_NO && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select o).FirstOrDefault<MFM_MAST>();

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool UpdateMFMPlan(MFMPlanModel mfmplan)
        {
            try
            {
                MFM_MAST mfm = (from o in DB.MFM_MAST
                                where o.PART_NO == mfmplan.PART_NO && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                select o).FirstOrDefault<MFM_MAST>();
                if (mfm != null)
                {
                    mfm.DOC_REL_DT_PLAN = mfmplan.DOC_REL_DT_PLAN;
                    mfm.DOC_REL_DT_ACTUAL = mfmplan.DOC_REL_DT_ACTUAL;
                    mfm.TIME_BOGAUGE_PLAN = mfmplan.TIME_BOGAUGE_PLAN;
                    mfm.TIME_BOGAUGE_ACTUAL = mfmplan.TIME_BOGAUGE_ACTUAL;
                    mfm.TOOLS_READY_DT_PLAN = mfmplan.TOOLS_READY_DT_PLAN;
                    mfm.TOOLS_READY_ACTUAL_DT = mfmplan.TOOLS_READY_ACTUAL_DT;
                    mfm.FORGING_PLAN_DT = mfmplan.FORGING_PLAN_DT;
                    mfm.FORGING_ACTUAL_DT = mfmplan.FORGING_ACTUAL_DT;
                    mfm.SECONDARY_PLAN_DT = mfmplan.SECONDARY_PLAN_DT;
                    mfm.SECONDARY_ACTUAL_DT = mfmplan.SECONDARY_ACTUAL_DT;
                    mfm.HEAT_TREATMENT_PLAN_DT = mfmplan.HEAT_TREATMENT_PLAN_DT;
                    mfm.HEAT_TREATMENT_ACTUAL = mfmplan.HEAT_TREATMENT_ACTUAL;
                    mfm.ISSR_PLAN_DT = mfmplan.ISSR_PLAN_DT;
                    mfm.ISSR_ACTUAL_DT = mfmplan.ISSR_ACTUAL_DT;
                    mfm.PPAP_PLAN = mfmplan.PPAP_PLAN;
                    mfm.PPAP_ACTUAL_DT = mfmplan.PPAP_ACTUAL_DT;
                    mfm.SAMPLE_QTY = mfmplan.SAMPLE_QTY.ToString().ToDecimalValue();
                    mfm.PSW_DATE = mfmplan.PSW_DATE;
                    mfm.RESP = mfmplan.RESP;
                    mfm.REMARKS = mfmplan.REMARKS;
                    mfm.UPDATED_BY = userInformation.UserName;
                    mfm.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                    DB.SubmitChanges();
                }
                mfm = null;
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}
