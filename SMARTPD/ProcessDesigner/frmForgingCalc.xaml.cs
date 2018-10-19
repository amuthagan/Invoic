using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
//using System


namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmLibrarySearch.xaml
    /// </summary>
    public partial class frmForgingCalc : Window
    {
        #region "Variable Declaration"
        public string Sql { get; set; }
        public string varPartNo { get; set; }
        public string varPartDesc { get; set; }
        const double PI = 3.14159265;
        DataTable dtPartNo = new DataTable();
        public DataTable dtCostCenterData { get; set; }

        public double x { get; set; }
        public string Message { get; set; }
        public int yorn { get; set; }
        public int looping { get; set; }


        //Rem Product Details

        public double vMaxAcrossFlat { get; set; }
        public double vMaxAcrossCorner { get; set; }
        public double vHeadHeight { get; set; }
        public double vMinAcrossFlat { get; set; }
        public double vMinHeadHeight { get; set; }
        public double vMeanAcrossFlat { get; set; }
        public double vMeanHeadHeight { get; set; }
        public double vBFDia { get; set; }
        public double vBFHeight { get; set; }
        public double vShankDia { get; set; }
        public double vOAL { get; set; }
        public double vThdLength { get; set; }
        public double vTRDia { get; set; }
        public double vShankLength { get; set; }
        public double vCollarDia { get; set; }
        public double vCollarHeight { get; set; }
        public double vCollarAngle { get; set; }
        public double vReinforceHeight { get; set; }
        public double vWrenchHeight { get; set; }
        public double vMinHeadDia { get; set; }
        public double vMaxHeadDia { get; set; }
        public double vMeanHeadDia { get; set; }
        public double vMinCutLen { get; set; }
        public double vMaxCutLen { get; set; }
        public double vMeanCutLen { get; set; }


        // Rem Calculated Basic Information

        public double vWireSize { get; set; }
        public double vMaxWiresize { get; set; }
        public double HeadVol { get; set; }
        public double BodyVol { get; set; }
        public double vShankVol { get; set; }
        public double vThdVol { get; set; }
        public double vChamferVol { get; set; }
        public double vHeadwoChamferVol { get; set; }
        public double vCollarVol { get; set; }
        public double vReinforceVol { get; set; }

        // Rem Cone Details
        public double ParallelLandDia { get; set; }
        public double vParallelLandHeight { get; set; }
        public double vDiePunchGap { get; set; }

        public double ParallelLandVol { get; set; }
        public double DiePunchGapVol { get; set; }
        public double ConeVol { get; set; }

        public double vConeHeight { get; set; }
        public double vCutOffLength { get; set; }
        public double vLbyD { get; set; }
        public double vSeverityRatio { get; set; }
        public double vHeightRedn { get; set; }
        public double vWireConeSeverityRatio { get; set; }
        public double vMaxConeDia { get; set; }
        public double vMinConeDia { get; set; }
        public double vMeanConeDia { get; set; }
        public double vConeDiaFactor { get; set; }
        public double vMinConeDiaBot { get; set; }
        public double vFaceAngleHeight { get; set; }
        public double ConeAngle { get; set; }

        public string vRemarks { get; set; }

        public double vCConeAngle { get; set; }
        public double vCConeTopDia { get; set; }
        public double vCConeHeight { get; set; }
        public double vCConeVolume { get; set; }
        public double FaceAngleVolume { get; set; }
        public double vFaceAngle { get; set; }
        public double temp1 { get; set; }

        //Rem Heading Details
        public double vChamferSmallDia { get; set; }
        public double vChamferBigDia { get; set; }
        public double vHeadBottomDia { get; set; }
        public double vHeadChamferHeight { get; set; }
        public double vHeadHeightwoChamfer { get; set; }
        public double vHeadChamferAngle { get; set; }
        public double vHeadChamferPercent { get; set; }
        public double vHeadAngle { get; set; }

        //Rem Extrusion Details
        public double vShankReduction { get; set; }
        public double vTRDReduction { get; set; }

        //Rem Process Details
        public string vStation1 { get; set; }
        public string vStation2 { get; set; }
        public string vStation3 { get; set; }
        public string vStation4 { get; set; }
        public string vStation5 { get; set; }

        // Rem Cone Tool - Punch Side Details

        public int vCone_Tool_Type { get; set; }

        public double vCTP_Dia { get; set; }
        public double vCTP_Length { get; set; }
        public double vCTP_Reference { get; set; }
        public double vCT_Case_OD_Big { get; set; }

        public double vCT_Case_OD_Small { get; set; }
        public double vCT_Case_OAL { get; set; }
        public double vCT_Case_Step_Length { get; set; }
        public double vCT_Insert_OD_Big { get; set; }
        public double pvCT_Insert_OD_Small { get; set; }
        public double vCT_Insert_OAL { get; set; }
        public double vCT_Insert_Step_Length { get; set; }

        public string vCT_Case_Code { get; set; }
        public string vCT_Insert_Code { get; set; }
        public string vCTP_Code { get; set; }
        #endregion

        ForignCalc bllForigncal = null;

        public frmForgingCalc(UserInformation userInformation)
        {
            InitializeComponent();
            bllForigncal = new ForignCalc(userInformation);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            varPartNo = "M70410";
            lbl_FAH.Text = "Face Angle Ht :";
            lblExtrusion.Text = "Extrusion Parameters";
            lblPartDescription.Text = varPartDesc;

            if (varPartNo.Length == 0) return;

            ForgingCalc(varPartNo);
            Get_Data(varPartNo);
            //  Set LtbCostCentre.Recordset = GetRS("select cost_cent_code,cost_cent_desc from ddcost_cent_mast where 1=2 ")
        }
        private void cmdCalculation_Click(object sender, RoutedEventArgs e)
        {
            Calculation();
        }

        /// <summary>
        ///  Get ForgingCalc Data From forging_calc Table
        /// </summary>
        /// <param name="varPartNo1"></param>
        public void ForgingCalc(string varPartNo1)
        {
            int iRet = 0;
            try
            {
                if (varPartNo1.Length == 0) return;

                // dtPartNo=new DataTable();

                iRet = bllForigncal.Getforging_calc(varPartNo1, ref dtPartNo);
                if (iRet < 0)
                {
                    MessageBox.Show("Error Get Data");
                }
                if ((dtPartNo.Rows.Count == 0) || (dtPartNo == null))
                {
                    // Insert Function Calling for forging_calc Table 
                    MessageBox.Show("No Records Founs!..");
                    return;
                }
                else
                {
                    FillDataValues(dtPartNo);
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void FillDataValues(DataTable dtPartNo)
        {
            int double_cone;
            txtPartNo.Text = dtPartNo.Rows[0]["part_no"].ToValueAsString().ToDoubleAsString();
            txt_minwiresize.Text = dtPartNo.Rows[0]["min_wire_size"].ToValueAsString().ToDoubleAsString();
            txt_maxwiresize.Text = dtPartNo.Rows[0]["max_wire_size"].ToValueAsString().ToDoubleAsString();
            double_cone = dtPartNo.Rows[0]["double_cone"].ToValueAsString().ToIntValue();

            if (double_cone == 1)
            {
                chk_DOUBLE_CONE.IsChecked = true;
            }
            else
            {
                chk_DOUBLE_CONE.IsChecked = false;
            }


            if (int.Parse(dtPartNo.Rows[0]["single_cone"].ToString()) == 1)
            {
                chk_SINGLE_CONE.IsChecked = true;
            }
            else
            {
                chk_SINGLE_CONE.IsChecked = false;
            }


            if (int.Parse(dtPartNo.Rows[0]["open_heading"].ToString()) == 1)
            {
                chk_OpenHeading.IsChecked = true;
            }
            else
            {
                chk_OpenHeading.IsChecked = false;
            }


            if (int.Parse(dtPartNo.Rows[0]["closed_heading"].ToString()) == 1)
            {
                chk_ClosedHeading.IsChecked = true;
            }
            else
            {
                chk_ClosedHeading.IsChecked = false;
            }

            if (int.Parse(dtPartNo.Rows[0]["face_angle"].ToString()) == 1)
            {
                chk_FACE_ANGLE.IsChecked = true;
            }
            else
            {
                chk_FACE_ANGLE.IsChecked = false;
            }


            txt_MinConeDiaTop.Text = dtPartNo.Rows[0]["min_cone_dia_top"].ToValueAsString().ToDoubleAsString();
            txt_MinConeDiaBot.Text = dtPartNo.Rows[0]["min_cone_dia_bot"].ToValueAsString().ToDoubleAsString();
            txt_ConeDiaFactor.Text = dtPartNo.Rows[0]["cone_dia_factor"].ToValueAsString().ToDoubleAsString();
            txt_FaceAngle.Text = dtPartNo.Rows[0]["face_angle_dim"].ToValueAsString().ToDoubleAsString();
            txt_DiePunchGap.Text = dtPartNo.Rows[0]["die_punch_gap"].ToValueAsString().ToDoubleAsString();
            txt_ParallelLand.Text = dtPartNo.Rows[0]["parallel_land"].ToValueAsString().ToDoubleAsString();
            txt_HeadAngle.Text = dtPartNo.Rows[0]["head_angle"].ToValueAsString().ToDoubleAsString();
            txt_HeadJnRad.Text = dtPartNo.Rows[0]["head_jn_rad"].ToValueAsString().ToDoubleAsString();
            txt_HeadChamferPercent.Text = dtPartNo.Rows[0]["head_chamfer_percent"].ToValueAsString().ToDoubleAsString();

            ltbCostCentre.Text = dtPartNo.Rows[0]["cost_centre_code"].ToValueAsString();
            stage_1.Text = dtPartNo.Rows[0]["station_1"].ToValueAsString();
            stage_2.Text = dtPartNo.Rows[0]["station_2"].ToValueAsString();
            stage_3.Text = dtPartNo.Rows[0]["station_3"].ToValueAsString();
            stage_4.Text = dtPartNo.Rows[0]["station_4"].ToValueAsString();
            stage_5.Text = dtPartNo.Rows[0]["station_5"].ToValueAsString();
        }


        private void chk_SINGLE_CONE_Checked(object sender, RoutedEventArgs e)
        {

            if (chk_SINGLE_CONE.IsChecked == true)
            {
                chk_FACE_ANGLE.IsEnabled = true;
            }
            else
            {
                chk_FACE_ANGLE.IsEnabled = false;
            }

        }

        private void chk_ClosedHeading_Checked(object sender, RoutedEventArgs e)
        {
            if (chk_ClosedHeading.IsChecked == true)
            {
                chk_OpenHeading.IsChecked = false;
            }
            else
            {
                chk_OpenHeading.IsChecked = true;
            }

        }

        private void chk_OpenHeading_Checked(object sender, RoutedEventArgs e)
        {
            if (chk_OpenHeading.IsChecked == true)
            {
                chk_ClosedHeading.IsChecked = false;
            }
            else
            {
                chk_ClosedHeading.IsChecked = true;
            }

        }
        /// <summary>
        ///  Get Data
        /// </summary>
        private void Get_Data(string varPartNo)
        {
            int iRet = 0;
            string familyName;
            string familyType;
            DataTable dtrsPrdDimn = new DataTable();
            DataTable dDtrsFamily = new DataTable();
            try
            {
                if (varPartNo.Length == 0) return;

                iRet = bllForigncal.Get_Prdim_Data(varPartNo, ref dtrsPrdDimn);

                if ((dtrsPrdDimn.Rows.Count == 0) || (dtrsPrdDimn == null))
                {
                    // Insert Function Calling for PART_NO Table 
                }
                else
                {

                }

                iRet = bllForigncal.Get_Family_Data(varPartNo, ref dDtrsFamily);

                if (dDtrsFamily.Rows.Count > 0)
                {
                    familyName = dDtrsFamily.Rows[0]["Family"].ToString() + "" + dDtrsFamily.Rows[0]["head_style"].ToString();
                    familyType = dDtrsFamily.Rows[0]["Type"].ToString();
                }
                else
                {

                    return;
                }

                FillFamilyData(dDtrsFamily);
                FindFamilyName(familyName.Trim(), familyType.Trim(), dtrsPrdDimn);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        /// <summary>
        /// Fill Damily Data to TextBox
        /// </summary>
        /// <param name="dtrsFamilyData"></param>
        public void FillFamilyData(DataTable dtrsFamilyData)
        {
            try
            {
                if (txt_minwiresize.Text == "0" || txt_minwiresize.Text == "")
                {

                    txt_MinConeDiaTop.Text = Convert.ToString(double.Parse(txt_maxwiresize.Text) + 0.1);
                    txt_MinConeDiaBot.Text = txt_maxwiresize.Text;
                }

                vWireSize = double.Parse(txt_minwiresize.Text);
                vDiePunchGap = double.Parse(txt_DiePunchGap.Text);
                vConeDiaFactor = double.Parse(txt_ConeDiaFactor.Text);
                vParallelLandHeight = double.Parse(txt_ParallelLand.Text);
                vFaceAngle = double.Parse(txt_FaceAngle.Text);
                vHeadAngle = double.Parse(txt_HeadAngle.Text);
                vMinConeDia = double.Parse(txt_MinConeDiaTop.Text);
                vMinConeDiaBot = double.Parse(txt_MinConeDiaBot.Text);
                vHeadChamferPercent = double.Parse(txt_HeadChamferPercent.Text);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void FindFamilyName(string familyName, string familyType, DataTable rsPrdDimn)
        {
            try
            {
                switch (familyName)
                {
                    case "02Hex":
                    case "01Hex":
                    case "01Bi-Hex":
                    case "02Bi-Hex":
                        vMinAcrossFlat = double.Parse(rsPrdDimn.Rows[0]["p001"].ToString());
                        vMaxAcrossFlat = double.Parse(rsPrdDimn.Rows[0]["p002"].ToString());
                        vMeanAcrossFlat = (vMaxAcrossFlat + vMinAcrossFlat) / 2;
                        vMinHeadHeight = double.Parse(rsPrdDimn.Rows[0]["p005"].ToString());
                        vHeadHeight = double.Parse(rsPrdDimn.Rows[0]["p006"].ToString());
                        vMeanHeadHeight = (vMinHeadHeight + vHeadHeight) / 2;
                        vHeadChamferAngle = double.Parse(rsPrdDimn.Rows[0]["p007"].ToString());
                        vBFDia = double.Parse(rsPrdDimn.Rows[0]["p010"].ToString());
                        vBFHeight = double.Parse(rsPrdDimn.Rows[0]["p009"].ToString());
                        vShankDia = double.Parse(rsPrdDimn.Rows[0]["p014"].ToString());
                        vTRDia = double.Parse(rsPrdDimn.Rows[0]["p016"].ToString());
                        vOAL = double.Parse(rsPrdDimn.Rows[0]["p026"].ToString());
                        vThdLength = rsPrdDimn.Rows[0]["p022"].ToString() == "" ? 0.0 : double.Parse(rsPrdDimn.Rows[0]["p022"].ToString());
                        vMaxAcrossCorner = vMaxAcrossFlat * 1.154;
                        vShankLength = vOAL - vThdLength;
                        vChamferSmallDia = vHeadChamferPercent * vMaxAcrossFlat + 0.1;
                        vChamferBigDia = vMaxAcrossCorner + 0.4;
                        vHeadChamferHeight = ((vChamferBigDia - vChamferSmallDia) / 2) * Math.Tan(vHeadChamferAngle * PI / 180);

                        vChamferVol = bllForigncal.Volume("Cone Frustum", vChamferBigDia, vChamferSmallDia, vHeadChamferHeight, "+");
                        vShankVol = bllForigncal.Volume("Cylinder", vShankDia, vShankLength, 0, "+");
                        vThdVol = bllForigncal.Volume("Cylinder", vTRDia, vThdLength, 0, "+");

                        // Inter Family Type Switch 
                        FamilyTpeSwitch(familyName, familyType, rsPrdDimn);
                        // End

                        break;
                    case "1Circular":
                    case "2Circular":

                        vMinHeadDia = double.Parse(rsPrdDimn.Rows[0]["p001"].ToString());
                        vMaxHeadDia = double.Parse(rsPrdDimn.Rows[0]["p002"].ToString());
                        vMeanHeadDia = (vMaxHeadDia + vMinHeadDia) / 2;
                        vMinHeadHeight = double.Parse(rsPrdDimn.Rows[0]["p003"].ToString());
                        vHeadHeight = double.Parse(rsPrdDimn.Rows[0]["p004"].ToString());
                        vMeanHeadHeight = (vMinHeadHeight + vHeadHeight) / 2;
                        vHeadChamferAngle = double.Parse(rsPrdDimn.Rows[0]["p005"].ToString());
                        vHeadChamferHeight = double.Parse(rsPrdDimn.Rows[0]["p007"].ToString());
                        vBFDia = double.Parse(rsPrdDimn.Rows[0]["p010"].ToString());
                        vBFHeight = double.Parse(rsPrdDimn.Rows[0]["p010"].ToString());
                        vShankDia = double.Parse(rsPrdDimn.Rows[0]["p014"].ToString());
                        vTRDia = double.Parse(rsPrdDimn.Rows[0]["p016"].ToString());
                        vOAL = double.Parse(rsPrdDimn.Rows[0]["p026"].ToString());
                        vThdLength = double.Parse(rsPrdDimn.Rows[0]["p022"].ToString());
                        vShankLength = vOAL - vThdLength;
                        vChamferBigDia = vMeanHeadDia;
                        vChamferSmallDia = vChamferBigDia - ((vHeadChamferHeight * Math.Tan(vHeadChamferAngle * PI / 180)) * 2);
                        vChamferVol = bllForigncal.Volume("Cone Frustum", vChamferBigDia, vChamferSmallDia, vHeadChamferHeight, "+");
                        vShankVol = bllForigncal.Volume("Cylinder", vShankDia, vShankLength, 0, "+");
                        vThdVol = bllForigncal.Volume("Cylinder", vTRDia, vThdLength, 0, "+");
                        vHeadHeightwoChamfer = vHeadHeight - vHeadChamferHeight;
                        vHeadBottomDia = vMeanHeadDia;
                        vHeadwoChamferVol = bllForigncal.Volume("Cylinder", vMeanHeadDia, vHeadHeightwoChamfer, 0, "+");
                        HeadVol = vHeadwoChamferVol + vChamferVol;
                        break;
                    case "1D-Head":
                    case "2D-Head":

                        vMinHeadDia = double.Parse(rsPrdDimn.Rows[0]["p001"].ToString());
                        vMaxHeadDia = double.Parse(rsPrdDimn.Rows[0]["p002"].ToString());
                        vMeanHeadDia = (vMaxHeadDia + vMinHeadDia) / 2;
                        vMinCutLen = double.Parse(rsPrdDimn.Rows[0]["p003"].ToString());
                        vMaxCutLen = double.Parse(rsPrdDimn.Rows[0]["p004"].ToString());
                        vMeanCutLen = (vMaxCutLen + vMinCutLen) / 2;
                        vMinHeadHeight = double.Parse(rsPrdDimn.Rows[0]["p005"].ToString());
                        vHeadHeight = double.Parse(rsPrdDimn.Rows[0]["p006"].ToString());
                        vMeanHeadHeight = (vMinHeadHeight + vHeadHeight) / 2;
                        vHeadChamferAngle = double.Parse(rsPrdDimn.Rows[0]["p007"].ToString());
                        vBFDia = double.Parse(rsPrdDimn.Rows[0]["p010"].ToString());
                        vBFHeight = double.Parse(rsPrdDimn.Rows[0]["p009"].ToString());
                        vShankDia = double.Parse(rsPrdDimn.Rows[0]["p014"].ToString());
                        vTRDia = double.Parse(rsPrdDimn.Rows[0]["p016"].ToString());
                        vOAL = double.Parse(rsPrdDimn.Rows[0]["p026"].ToString());
                        vThdLength = double.Parse(rsPrdDimn.Rows[0]["p022"].ToString());
                        vMaxAcrossCorner = vMaxAcrossFlat * 1.154;
                        vShankLength = vOAL - vThdLength;
                        vChamferSmallDia = vMeanHeadDia - (2 * vHeadChamferHeight);
                        vChamferBigDia = vMeanHeadDia;
                        vHeadChamferHeight = ((vChamferBigDia - vChamferSmallDia) / 2) * Math.Tan(vHeadChamferAngle * PI / 180);
                        vChamferVol = bllForigncal.Volume("Cone Frustum", vChamferBigDia, vChamferSmallDia, vHeadChamferHeight, "+");
                        vShankVol = bllForigncal.Volume("Cylinder", vShankDia, vShankLength, 0, "+");
                        vThdVol = bllForigncal.Volume("Cylinder", vTRDia, vThdLength, 0, "+");
                        vHeadHeightwoChamfer = vHeadHeight - vHeadChamferHeight;
                        vHeadBottomDia = vMeanHeadDia;
                        vHeadwoChamferVol = bllForigncal.Volume("Cylinder", vMeanHeadDia, vMeanHeadHeight - vHeadChamferHeight, 0, "+");
                        HeadVol = bllForigncal.Volume("Cylinder", vMeanHeadDia, vHeadHeightwoChamfer, 0, "+");
                        break;

                    default:
                        MessageBox.Show("Automation not completed for " + familyName);
                        break;
                }

                // txt_HeadVolume.Text = double.Parse(HeadVol);

                txt_HeadVolume.Text = Convert.ToString(Math.Round(HeadVol, 2));
                txt_BodyVolume.Text = Convert.ToString(Math.Round((vShankVol + vThdVol), 2));
                lbl_TopChamferDia.Text = Convert.ToString(Math.Round(vChamferSmallDia, 2));
                lbl_botChamferDia.Text = Convert.ToString(Math.Round(vChamferBigDia, 2));
                lbl_ChamferHeight.Text = Convert.ToString(Math.Round(vHeadChamferHeight, 2));
                lbl_headBottomDia.Text = Convert.ToString(Math.Round(vHeadBottomDia, 2));
                lbl_HeadHeightBelowChamfer.Text = Convert.ToString(Math.Round(vHeadHeightwoChamfer, 2));

                //txt_HeadVolume.Text = Format$(HeadVol, "Fixed");
                //txt_BodyVolume.Text = string.Format(vShankVol + vThdVol, "Fixed");
                //lbl_TopChamferDia.Text  = Format$(vChamferSmallDia, "Fixed");
                //lbl_botChamferDia.Text = Format$(vChamferBigDia, "Fixed");
                //lbl_ChamferHeight.Text = Format$(vHeadChamferHeight, "Fixed");
                //lbl_headBottomDia.Text = Format$(vHeadBottomDia, "Fixed");
                //lbl_HeadHeightBelowChamfer.Text = Format$(vHeadHeightwoChamfer, "Fixed");
                if (txt_minwiresize.Text == "")
                {
                    txt_minwiresize.Text = Convert.ToString(vShankDia - 0.2);
                }


                // If txt_minwiresize.text = "" Then 
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void FamilyTpeSwitch(string familyName, string familyType, DataTable rsPrdDimn)
        {
            try
            {
                switch (familyType)
                {
                    case "Plain":
                        vHeadHeightwoChamfer = vHeadHeight - vHeadChamferHeight;
                        vHeadBottomDia = 2 * (vHeadHeightwoChamfer * Math.Tan(vHeadAngle * PI / 180)) + vChamferBigDia;
                        vHeadwoChamferVol = bllForigncal.Volume("Cone Frustum", vHeadBottomDia, vChamferBigDia, vHeadHeightwoChamfer, "+");
                        HeadVol = vChamferVol + vHeadwoChamferVol;
                        break;
                    case "Collar":
                        vCollarDia = double.Parse(rsPrdDimn.Rows[0]["p030"].ToString());
                        vCollarHeight = double.Parse(rsPrdDimn.Rows[0]["p032"].ToString());
                        vHeadHeightwoChamfer = vHeadHeight - vHeadChamferHeight - vCollarHeight;
                        vHeadBottomDia = 2 * (vHeadHeightwoChamfer * Math.Tan(vHeadAngle * PI / 180)) + vChamferBigDia;
                        vHeadwoChamferVol = bllForigncal.Volume("Cone Frustum", vHeadBottomDia, vChamferBigDia, vHeadHeightwoChamfer, "+");
                        vCollarVol = bllForigncal.Volume("Cylinder", vCollarDia + 1, vCollarHeight, 0, "+");
                        HeadVol = vChamferVol + vHeadwoChamferVol + vCollarVol;
                        break;
                    case "Flanged":
                        vCollarDia = double.Parse(rsPrdDimn.Rows[0]["p030"].ToString());
                        vCollarHeight = double.Parse(rsPrdDimn.Rows[0]["p032"].ToString());
                        vCollarAngle = double.Parse(rsPrdDimn.Rows[0]["p033"].ToString());
                        vWrenchHeight = double.Parse(rsPrdDimn.Rows[0]["p034"].ToString());
                        vHeadHeightwoChamfer = vWrenchHeight - vHeadChamferHeight;
                        vHeadBottomDia = 2 * (vHeadHeightwoChamfer * Math.Tan(vHeadAngle * PI / 180)) + vChamferBigDia;
                        vHeadwoChamferVol = bllForigncal.Volume("Cone Frustum", vHeadBottomDia, vChamferBigDia, vHeadHeightwoChamfer, "+");
                        vCollarVol = bllForigncal.Volume("Cylinder", vCollarDia + 1, vCollarHeight, 0, "+");
                        vReinforceHeight = ((vCollarDia - vHeadBottomDia) / 2) * Math.Tan(vCollarAngle * PI / 180);
                        vReinforceVol = bllForigncal.Volume("Cone Frustum", vCollarDia, vHeadBottomDia, vReinforceHeight, "+");
                        HeadVol = vChamferVol + vHeadwoChamferVol + vCollarVol + vReinforceVol;
                        break;
                    default:
                        MessageBox.Show("Automation not completed for " + familyName);
                        break;
                    // MsgBox Message
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        public void Calculation()
        {


            //  fraCone.Visible = True
            //fraHeading.Visible = True
            //fraExtrusion.Visible = True
            //lblCone.Visible = True
            //lblHeading.Visible = True
            //lblExtrusion.Visible = True
            //shaCalculation.Visible = True

            if (txt_minwiresize.Text == "")
            {
                vWireSize = vShankDia - 0.2;
            }
            else
            {
                vWireSize = double.Parse(txt_minwiresize.Text);
                vMaxWiresize = double.Parse(txt_maxwiresize.Text);
            }
            Get_Data(txtPartNo.Text);

            lbl_TotCOLength.Text = Convert.ToString((HeadVol + vShankVol + vThdVol) / (PI * (Math.Pow(vWireSize, 2)) / 4));

            if (vMaxWiresize > (vChamferSmallDia - 0.5))
            {
                chk_CONVERGED_CONE.IsChecked = true;
            }
            else
            {
                chk_CONVERGED_CONE.IsChecked = false;
            }

            Cone_Design();

            Extrusion_Design();

            LoadCostCenterCombo();

            //  Sql = "SELECT A.COST_CENT_CODE, A.COST_CENT_DESC FROM DDCOST_CENT_MAST A, DDFORGING_MAC B WHERE " _
            //        & "A.CATE_CODE = 4 AND B.MAX_CUTOFF_LEN > " & lbl_TotCOLength.text _
            //        & " AND B.MAX_CUTOFF_DIA > " & txt_maxwiresize.text _
            //        & " AND A.COST_CENT_CODE = B.COST_CENT_CODE"

            //Set ltbCostCentre.Recordset = GetRS(Sql)
            //    ltbCostCentre.SetColWidth "0-2000"
            //    ltbCostCentre.DisplayField = 0
        }
        /// <summary>
        /// Cone Design
        /// </summary>
        public void Cone_Design()
        {
            try
            {
                int yorn2;
                yorn2 = 0;
                if (looping == 1)
                {
                    vMinConeDia = vMaxWiresize + 0.1;
                    vMinConeDiaBot = vMaxWiresize + 0.05;
                }
                vMaxConeDia = vConeDiaFactor * vMinConeDia;

                vCutOffLength = HeadVol / ((PI * Math.Pow(vWireSize, 2)) / 4);
                vLbyD = (vCutOffLength) / vWireSize;
                vFaceAngleHeight = 0;
                FaceAngleVolume = 0;
                if (vLbyD <= 1.5) { }
                Message = ("The l/d ratio is only " + vLbyD + ". Coning not necessary. The head can be formed in one blow. Do you want to Cone?");

                if (MessageBox.Show(Message, "Process Design", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    yorn2 = 0;
                //return;
                yorn2 = 6;
                vLbyD = (vCutOffLength - vParallelLandHeight) / vWireSize;

                if (chk_CONVERGED_CONE.IsChecked == true)
                {
                    vCConeAngle = 15;
                    Message = "The Wire Diameter is bigger than the Top Chamfer Dia of the head. Should I vary Parallel Land Top Dia or Parallel Land Height. Click Yes to vary Height. Click No to vary Top Diameter.  Converged Cone";
                    if (MessageBox.Show(Message, "Process Design", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        vCConeTopDia = vChamferSmallDia - 0.5;
                        vCConeHeight = ((vMaxWiresize - vCConeTopDia) / 2) / (Math.Tan(vCConeAngle * PI / 180));
                    }
                    else
                    {
                        vCConeHeight = vParallelLandHeight;
                        vCConeTopDia = vMinConeDia - (2 * (vParallelLandHeight * Math.Tan(vCConeAngle * PI / 180)));
                    }
                    vCConeVolume = bllForigncal.Volume("Cone Frustum", vMaxWiresize, vCConeTopDia, vCConeHeight, "+");
                    if (vCConeHeight <= 0)
                    {
                        MessageBox.Show("Converged Cone Height is negative. Cannot use Converged Cone.");
                        chk_CONVERGED_CONE.IsChecked = false;
                        vCConeVolume = 0;
                        vCConeHeight = 0;
                    }
                }
                else
                {
                    vCConeVolume = 0;
                    vCConeHeight = 0;
                }
                if (vLbyD > 1.5 || yorn2 == 6)
                {
                    vLbyD = (vCutOffLength - vParallelLandHeight) / vWireSize;
                    ParallelLandVol = PI * (Math.Pow(vMinConeDia, 2)) * vParallelLandHeight / 4;
                    DiePunchGapVol = bllForigncal.Volume("Cone Frustum", vMaxConeDia, vMinConeDia, vDiePunchGap, "+");

                    if (chk_FACE_ANGLE.IsEnabled == true && chk_FACE_ANGLE.IsChecked == true)
                    {
                        vFaceAngle = double.Parse(txt_FaceAngle.Text);
                        vFaceAngleHeight = ((vMaxConeDia - vMinConeDiaBot) / 2) / Math.Tan(vFaceAngle * PI / 180);
                        FaceAngleVolume = bllForigncal.Volume("Cone Frustum", vMaxConeDia, vMinConeDia, vFaceAngleHeight, "+");
                    }

                    if (chk_DOUBLE_CONE.IsChecked == true)
                    {
                        vFaceAngle = double.Parse(txt_FaceAngle.Text);
                        vFaceAngleHeight = ((vMaxConeDia - vMinConeDiaBot) / 2) / Math.Tan(vFaceAngle * PI / 180);
                        FaceAngleVolume = bllForigncal.Volume("Cone Frustum", vMaxConeDia, vMinConeDia, vFaceAngleHeight, "+");
                    }
                    ConeVol = HeadVol - (ParallelLandVol + DiePunchGapVol + FaceAngleVolume + vCConeVolume);

                    temp1 = (PI * ((Math.Pow(vMaxConeDia, 3)) - (Math.Pow(vMinConeDia, 3)))) / (24 * ConeVol);
                    ConeAngle = Math.Atan(temp1) * 180 / PI;

                    vConeHeight = (vMaxConeDia - vMinConeDia) / (2 * Math.Tan(ConeAngle * PI / 180));

                    vMeanConeDia = Math.Pow(((Math.Pow(vMaxConeDia, 2) + Math.Pow(vMinConeDia, 2)) / 2), 0.5);

                    vSeverityRatio = (vConeHeight + vDiePunchGap + (vCConeHeight == 0 ? 0 : vParallelLandHeight) + vFaceAngleHeight + vCConeHeight - vHeadHeight) / (vConeHeight + vDiePunchGap + (vCConeHeight == 0 ? 0 : vParallelLandHeight) + vFaceAngleHeight + vCConeHeight);
                    vWireConeSeverityRatio = vCutOffLength - (vConeHeight + vDiePunchGap + (vCConeHeight == 0 ? 0 : vParallelLandHeight) + vFaceAngleHeight + vCConeHeight) / vCutOffLength;
                }

                if (vConeHeight <= 0)
                {
                    MessageBox.Show("MsgBox Cannot Cone with these parameters. Cone Height negative.");
                    return;
                }

                if (vLbyD <= 1.5 && yorn2 == 6 || vLbyD > 1.5)
                {
                    lbl_ConeAngle.Text = Convert.ToString(ConeAngle);
                    lbl_ConeHeight.Text = Convert.ToString(vConeHeight);
                    lbl_ConeHeightWithLand.Text = Convert.ToString(vConeHeight + (vCConeHeight == 0 ? 0 : vParallelLandHeight) + vCConeHeight);
                    lbl_TotalConeHeight.Text = Convert.ToString(vConeHeight + (vCConeHeight == 0 ? 0 : vParallelLandHeight) + vDiePunchGap + vFaceAngleHeight + vCConeHeight);
                    lbl_MaxConeDia.Text = Convert.ToString(vMaxConeDia);
                    lbl_MeanConeDia.Text = Convert.ToString(vMeanConeDia);
                    lbl_CutOffLength.Text = Convert.ToString(vCutOffLength);

                    if (chk_DOUBLE_CONE.IsChecked == true)
                    {
                        lbl_FAH.Text = "Bot Cone Height:";
                    }
                    else
                    {
                        lbl_FAH.Text = "Face Angle Ht:";
                    }
                    lbl_FaceAngleHeight.Text = Convert.ToString(vFaceAngleHeight);
                    lbl_LbyD.Text = Convert.ToString(vLbyD);

                    if (vLbyD <= 1.5)
                    {
                        vRemarks = "Sliding Cone Tool is not necessary, but can be used.";
                    }
                    else if (vLbyD > 1.5)
                    {
                        if (vLbyD <= 5) vRemarks = "Sliding Cone Tool must be used.";
                    }
                    else
                    {
                        vRemarks = "Operation is not possible";
                    }
                    // lbl_Remarks. = vRemarks;

                    if (chk_CONVERGED_CONE.IsChecked == true)
                    {
                        lbl_ConvergedConeDia.Visibility = Visibility.Visible;
                        lbl_ConvergedConeHeight.Visibility = Visibility.Visible;
                        lbl_ConvergedConeDia.Text = Convert.ToString(vCConeTopDia);
                        lbl_ConvergedConeHeight.Text = Convert.ToString(vCConeHeight);
                    }
                    else
                    {
                        lbl_ConvergedConeDia.Visibility = Visibility.Hidden;
                        lbl_ConvergedConeHeight.Visibility = Visibility.Hidden;
                    }
                    lbl_WireConeSevRatio.Text = Convert.ToString(vWireConeSeverityRatio + "%").ToString();
                    lbl_SeverityRatio.Text = Convert.ToString((vSeverityRatio * 100) + "%").ToString();
                    lbl_CollarDia90.Text = Convert.ToString(0.9 * vCollarDia).ToString();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }

        /// <summary>
        /// Extrusion Design
        /// </summary>
        public void Extrusion_Design()
        {
            // lbl_ExtrusionRemarks.Text = " ";
            Chk_Extrusion.IsChecked = false;
            Message = vWireSize + "," + vShankDia + "," + vTRDia;

            vShankReduction = (Math.Pow(vWireSize, 2) - Math.Pow(vShankDia, 2)) / Math.Pow(vWireSize, 2);
            vTRDReduction = (Math.Pow(vShankDia, 2) - Math.Pow(vTRDia, 2)) / Math.Pow(vShankDia, 2);
            lbl_ShankReduction.Text = Convert.ToString((vShankReduction * 100) + "%").ToString();
            lbl_TRDReduction.Text = Convert.ToString((vTRDReduction * 100) + "%").ToString();

            if (vShankReduction < 0.01)
            {
                //  lbl_ExtrusionRemarks.Text = "Extrusion not required for Shank.";
            }
            else
            {
                Chk_Extrusion.IsChecked = false;
                if (vShankReduction <= 0.3)
                {
                    // lbl_ExtrusionRemarks.Text = "Open Extrusion for Shank." ;
                }
                else
                {
                    if (vShankReduction <= 0.6)
                    {
                        //lbl_ExtrusionRemarks.Caption = "Trapped Extrusion for Shank." ;
                    }
                    else
                    {
                        //lbl_ExtrusionRemarks.Caption = "Shank Extrusion not possible with this wire dia in one step."
                        Chk_Extrusion.IsChecked = false;
                        return;
                    }
                }
            }

            if (vTRDReduction < 0.01)
            {
                //lbl_ExtrusionRemarks.Text = lbl_ExtrusionRemarks.Text & "Extrusion not required for TRD.";
            }
            else
            {
                Chk_Extrusion.IsChecked = true;
                if (vTRDReduction <= 0.3)
                {
                    //lbl_ExtrusionRemarks.Text = lbl_ExtrusionRemarks.Text & "Open Extrusion for TRD.";
                }
                else
                {
                    if (vTRDReduction <= 0.6)
                    {
                        // lbl_ExtrusionRemarks.Text = lbl_ExtrusionRemarks.Text & "Trapped Extrusion for TRD.";
                    }
                    else
                    {
                        // lbl_ExtrusionRemarks.Text = lbl_ExtrusionRemarks.Text & "TRD Extrusion not possible with this wire dia in one step.";
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadCostCenterCombo()
        {
            // dtCostCenterData = new DataTable();
            // bllForigncal.Get_CostCenter_Data(lbl_TotCOLength.Text, txt_maxwiresize.Text, ref dtCostCenterData);

        }

    }
}
