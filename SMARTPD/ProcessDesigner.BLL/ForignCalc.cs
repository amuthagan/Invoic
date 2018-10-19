using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.Common;


namespace ProcessDesigner.BLL
{

    public class ForignCalc : Essential
    {

        public ForignCalc(UserInformation userInformation)
        {
            try
            {
                this.userInformation = userInformation;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        //private Exception ex = null;
        StringBuilder sqlQry = new StringBuilder();

        public int Getforging_calc(string partNo, ref DataTable dtPartNo)
        {

            try
            {

                sqlQry.Append("select part_no,min_wire_size,max_wire_size,double_cone,converged_cone,min_cone_dia_top," +
               " min_cone_dia_bot,single_cone,face_angle,cone_dia_factor,die_punch_gap,parallel_land, " +
               " face_angle_dim , closed_heading, open_heading, head_angle, head_jn_rad, head_chamfer_percent, " +
               " cost_centre_code,station_1,station_2,station_3,station_4,station_5 from forging_calc where part_no='" + partNo + "'");

                dtPartNo = Dal.GetDataTable(sqlQry, null);
                return 0;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -255;
                
            }
        }
        /// <summary>
        ///  Get PerDim Data From PRD_DIMN
        /// </summary>
        /// <param name="PartNo"></param>
        /// <param name="DtPartNo"></param>
        /// <returns></returns>
        public int Get_Prdim_Data(string partNo, ref DataTable dtPartNo)
        {

            try
            {
                sqlQry.Append("SELECT * FROM PRD_DIMN WHERE PART_NO = '" + partNo + "'");
                dtPartNo = Dal.GetDataTable(sqlQry, null);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
               
            }
        }
        /// <summary>
        /// Get Famil Data From 
        /// </summary>
        /// <param name="PartNo"></param>
        /// <param name="DtrsFamily"></param>
        /// <returns></returns>
        public int Get_Family_Data(string partNo, ref DataTable dtrsFamily)
        {

            try
            {
                sqlQry.Append("select family,head_style,type from prd_mast where part_no='" + partNo + "'");
                dtrsFamily = Dal.GetDataTable(sqlQry, null);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        /// <summary>
        ///  Calculation For Voume  
        /// </summary>
        /// <param name="Shape"></param>
        /// <param name="Param1"></param>
        /// <param name="Param2"></param>
        /// <param name="Param3"></param>
        /// <param name="Param4"></param>
        /// <returns></returns>

        double rad; // rad is replace for r
        double c;
        double alpha;
        double l;
        double angle;
        int mult = 1;
        const double PI = 3.1416;
        public double Volume(String shape, double param1, double param2, double param3, string param4)
        {

            try
            {
                mult = 1;
                switch (shape)
                {
                    case "Square Prism":
                        return (Math.Pow(param1, 2) * param1) * mult;
                    case "Cylinder":
                        return (PI * Math.Pow(param1, 2) * param2 / 4) * mult;
                    case "Hexagon Prism":
                        return (3.464 * Math.Pow(param1, 2) * param2 / 4) * mult;
                    case "Bi-Hex Prism":
                        return (3.314 * Math.Pow(param1, 2) * param2 / 4) * mult;
                    case "Cone Frustum":
                        return (0.2618 * param3 * (Math.Pow(param1, 2) + (param1 * param2) + Math.Pow(param2, 2))) * mult;
                    case "Spherical Segment":
                        return (PI * Math.Pow(param2, 2) * (param1 - (param2 / 3))) * mult;
                    case "D-Head":
                        return VolumeDHeadCalc(param1, param2, param3);
                    default: return 0.0;

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public double VolumeDHeadCalc(double param1, double param2, double param3)
        {
            try
            {
                if (param1 != 0 && param2 != 0)
                {
                    rad = param1 / 2;
                    c = 2 * Math.Pow((param2 * (param1 - param2)), 0.5);
                    angle = (rad - param2) / rad;
                    alpha = 2 * Math.Tan(-angle / Math.Sqrt(-angle * angle + 1)) + 1.5708;
                    l = 0.01745 * rad * alpha;
                    return ((PI * Math.Pow(param2, 2) / 4) - (0.5 * ((rad * l) - (c * (rad - param2)))) * param3) * mult;
                }
                return 0.0;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        public int Get_CostCenter_Data(string colLength, string wireSize, ref DataTable dtCostData)
        {

            try
            {
                sqlQry.Append("SELECT A.COST_CENT_CODE, A.COST_CENT_DESC FROM DDCOST_CENT_MAST A, DDFORGING_MAC B WHERE " +
                   "A.CATE_CODE = 4 AND B.MAX_CUTOFF_LEN > '" + colLength + "'  AND B.MAX_CUTOFF_DIA >  '" + wireSize + "' AND A.COST_CENT_CODE = B.COST_CENT_CODE ");
                dtCostData = Dal.GetDataTable(sqlQry, null);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

    }
}
