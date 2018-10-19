using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class SapExportPDBll : Essential
    {
        public SapExportPDBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public int Update_Remove_SapRoh()
        {
            List<ROH_SAP_MASTER> lstexistingDatas = new List<ROH_SAP_MASTER>();
            try
            {

                lstexistingDatas = ((from c in DB.ROH_SAP_MASTER
                                     where 1 == 1
                                     select c).ToList());
                if (lstexistingDatas.Count > 0)
                {
                    sql = "delete from ROH_SAP_MASTER";
                    DB.ExecuteCommand(sql);
                }

            }
            catch (Exception ex)
            {
                return lstexistingDatas.Count;
            }
            return lstexistingDatas.Count;
        }
        public int Update_AfterInsertCount_SapRoh()
        {
            List<ROH_SAP_MASTER> lstexistingDatas = new List<ROH_SAP_MASTER>();
            try
            {

                lstexistingDatas = ((from c in DB.ROH_SAP_MASTER
                                     where 1 == 1
                                     select c).ToList());
                if (lstexistingDatas.Count > 0)
                {
                    return lstexistingDatas.Count;
                }
                else
                {
                    return lstexistingDatas.Count;
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
                return lstexistingDatas.Count;
            }
        }

        public Int32? Update_Insert_SapRoh(string rohValue)
        {
            string sql = "";
            Int32? status = -1;
            try
            {
                if (rohValue != "")
                {
                    sql = "insert into ROH_SAP_MASTER values('" + rohValue + "')";
                    status = DB.ExecuteCommand(sql);
                }
                return status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return status;
            }

        }

        public Int32? Update_Insert_Sap_Ccvsoprncc(string sapCcode, string sapBaseCode, string ccCode)
        {
            string sql = "";
            Int32? status = -1;
            try
            {
                if (ccCode != "")
                {
                    sql = "UPDATE DDCOST_CENT_MAST SET SAP_CCCODE='" + sapCcode + "' , SAP_BASE_QUANTITY='" + sapBaseCode + "' where COST_CENT_CODE='" + ccCode + "'";
                    status = DB.ExecuteCommand(sql);
                }
                return status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return status;
            }

        }
        public Int32? Update_Insert_Sap_Matgrvspd_Oprncode(string sapno, string operCode)
        {
            string sql = "";
            Int32? status = -1;
            try
            {
                if (operCode != "")
                {
                    sql = "UPDATE DDOPER_MAST SET  SAP_NO='" + sapno + "'  where OPER_CODE='" + operCode + "'";
                    status = DB.ExecuteCommand(sql);
                }
                return status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return status;
            }

        }
        public Int32? Update_Insert_Sap_Matgrvssap_Routshorttext(string shortText, string sapno)
        {
            string sql = "";
            Int32? status = -1;
            try
            {
                if (sapno != "")
                {
                    sql = "UPDATE DDOPER_MAST SET  SHORT_TEXT='" + shortText + "' WHERE  SAP_NO ='" + sapno + "'";
                    status = DB.ExecuteCommand(sql);
                }
                return status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return status;
            }

        }
        public Int32? Update_Insert_Sap_Nutboltstdsplcostcenre(string typeNutBolt, string ccCode)
        {
            string sql = "";
            Int32? status = -1;
            try
            {
                if (ccCode != "")
                {
                    sql = "UPDATE DDCOST_CENT_MAST SET  TYPE_NUT_BOLT='" + typeNutBolt.ToUpper() + "' WHERE  COST_CENT_CODE ='" + ccCode + "'";
                    status = DB.ExecuteCommand(sql);
                }
                return status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return status;
            }

        }
        public Int32? Update_Insert_Sapno_Vs_Unitofmeasureupdate(string uoMeasure, string sapNo)
        {
            string sql = "";
            Int32? status = -1;
            try
            {
                if (sapNo != "")
                {
                    sql = "UPDATE DDOPER_MAST SET UNIT_OF_MEASURE='" + uoMeasure + "' WHERE  SAP_NO ='" + sapNo + "'";
                    status = DB.ExecuteCommand(sql);
                }
                return status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return status;
            }

        }
        public Int32? Update_Insert_Sap_Matgrvsconfirmationpoints(string splProcessment, string sapNo)
        {
            string sql = "";
            Int32? status = -1;
            try
            {
                if (sapNo != "")
                {
                    sql = "UPDATE DDOPER_MAST SET SPECIAL_PROCUREMENT = '" + splProcessment + "' WHERE  SAP_NO ='" + sapNo + "'";
                    status = DB.ExecuteCommand(sql);
                }
                return status;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return status;
            }

        }
    }
}
