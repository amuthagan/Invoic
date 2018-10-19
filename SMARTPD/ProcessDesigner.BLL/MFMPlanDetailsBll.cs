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
    public class MFMPlanDetailsBll : Essential
    {
        public MFMPlanDetailsBll(UserInformation userinfo)
        {
            this.userInformation = userinfo;
        }

        public bool GetMFMPlanDetils(MFMPlanDetailsModel mfmPlanDetails)
        {

            try
            {
                string getQuery = " select CASE SUBSTRING(loc_code,1,1) WHEN 'M' THEN 'PADI' WHEN 'K' THEN 'KPM' WHEN 'Y' THEN 'PONDY' ELSE '' END as Location,module,cc_abbr ,cust_name ,part_no,part_desc,pg_category,resp,remarks from mfm_mast_view "
                                + " where ( cancel_status = 0 or cancel_status is null ) and ( hold_me =0 or hold_me is null ) and (( part_no is null and part_desc is null )";

                if (mfmPlanDetails.GroupHeader == "Awaiting")
                {
                    if (mfmPlanDetails.IsDocumentation)
                    {
                        getQuery = " select CASE SUBSTRING(c.loc_code,1,1) WHEN 'M' THEN 'PADI' WHEN 'K' THEN 'KPM' WHEN 'Y' THEN 'PONDY' ELSE '' END as Location,f.cust_name,a.part_no,b.part_desc,b.pg_category,a.resp,a.remarks "
                                 + " from mfm_mast A,prd_mast b,ddloc_mast c,prd_ciref d ,ddci_info e,ddcust_mast f"
                                 + " where a.doc_rel_dt_actual is null and a.doc_rel_dt_plan is not null "
                                 + " and ( b.cancel_status =0 or b.cancel_status is null ) and ( b.hold_me =0 or b.hold_me is null ) and ( allot_date > convert(datetime, '31/03/1999', 103) and "
                                 
                                 + " ( b.part_no like 'M%' or b.part_no like 'S%' )  and a.part_no=b.part_no and b.bif_proj =C.loc_code and b.part_no=d.part_no  AND d.CURRENT_CIREF = 1 and d.ci_ref=e.ci_reference and e.cust_code=f.cust_code and samp_submit_date is null  ";
                                 //+ " ( b.part_no like 'M%' or b.part_no like 'S%' )  and a.part_no=b.part_no and b.bif_proj =C.loc_code and b.part_no=d.part_no and d.ci_ref=e.ci_reference and e.cust_code=f.cust_code and samp_submit_date is null  ";
                    }

                    if (mfmPlanDetails.IsTools)
                    {
                        getQuery = getQuery + " or ( tools_ready_actual_dt is null and doc_rel_dt_actual > convert(datetime, '31/03/1999', 103) and allot_date > convert(datetime, '31/03/1999', 103) and samp_submit_date is null) ";
                    }

                    if (mfmPlanDetails.IsForging)
                    {
                        getQuery = getQuery + " or ( forging_actual_dt is null and tools_ready_actual_dt is not null and doc_rel_dt_actual > convert(datetime, '31/03/1999', 103) and allot_date > convert(datetime, '31/03/1999', 103) and samp_submit_date is null) ";
                    }

                    if (mfmPlanDetails.IsSecondary)
                    {
                        getQuery = getQuery + " or ( forging_actual_dt is not null and  secondary_actual_dt is null and heat_treatment_actual is null and doc_rel_dt_actual > convert(datetime, '31/03/1999', 103) and allot_date > convert(datetime, '31/03/1999', 103) and samp_submit_date is null ) ";
                    }

                    if (mfmPlanDetails.IsPPAP)
                    {
                        getQuery = getQuery + " or ( forging_actual_dt is not null and tools_ready_actual_dt is not null and  secondary_actual_dt is not null and heat_treatment_actual is not null and ppap_actual_dt is null and doc_rel_dt_actual > convert(datetime, '31/03/1999', 103) and allot_date > convert(datetime, '31/03/1999', 103) and samp_submit_date is null and ppap_actual_dt is null ) ";
                    }

                    if (mfmPlanDetails.IsAwaitingPSW)
                    {
                        getQuery = getQuery + " or ( samp_submit_date > convert(datetime, '31/03/1999', 103) and PSW_ST='NO') ";
                    }
                }
                else
                {
                    if (mfmPlanDetails.IsDocumentation)
                    {
                        getQuery = getQuery + " OR (DOC_REL_DT_ACTUAL IS NULL AND SUBSTRING(convert(varchar(10), DOC_REL_DT_PLAN, 112),1,6) = '" + mfmPlanDetails.Month + "')";
                    }

                    if (mfmPlanDetails.IsTools)
                    {
                        getQuery = getQuery + " OR (TOOLS_READY_ACTUAL_DT IS NULL AND SUBSTRING(convert(varchar(10), TOOLS_READY_DT_PLAN, 112),1,6) = '" + mfmPlanDetails.Month + "')";
                    }

                    if (mfmPlanDetails.IsForging)
                    {
                        getQuery = getQuery + " OR (FORGING_ACTUAL_DT IS NULL AND SUBSTRING(convert(varchar(10), FORGING_PLAN_DT, 112),1,6) = '" + mfmPlanDetails.Month + "')";
                    }

                    if (mfmPlanDetails.IsSecondary)
                    {
                        getQuery = getQuery + " OR (SECONDARY_ACTUAL_DT IS NULL AND SUBSTRING(convert(varchar(10), SECONDARY_PLAN_DT, 112),1,6) = '" + mfmPlanDetails.Month + "')";
                    }

                    if (mfmPlanDetails.IsPPAP)
                    {
                        getQuery = getQuery + " OR (PPAP_ACTUAL_DT IS NULL AND SUBSTRING(convert(varchar(10), PPAP_PLAN, 112),1,6) = '" + mfmPlanDetails.Month + "')";
                    }
                }

                if (mfmPlanDetails.IsApprovedPSW)
                {
                    getQuery = getQuery + " OR (PSW_ST='YES' AND PSW_DATE IS NOT NULL)";
                }

                getQuery = getQuery + ")";

                DataTable getprocess = ToDataTable(DB.ExecuteQuery<MFMPlanDetailsGrid>(getQuery).ToList());

                if (getprocess == null)
                {
                    mfmPlanDetails.DVMFMPlanDetails = null;
                    mfmPlanDetails.ProductCount = "0";
                }
                else
                {
                    mfmPlanDetails.DVMFMPlanDetails = getprocess.DefaultView;
                    mfmPlanDetails.ProductCount = mfmPlanDetails.DVMFMPlanDetails.Count.ToString();
                }
                return true;
            }
            catch
            {
                mfmPlanDetails.DVMFMPlanDetails = null;
                mfmPlanDetails.ProductCount = "0";
                return false;
            }
        }
    }
}
