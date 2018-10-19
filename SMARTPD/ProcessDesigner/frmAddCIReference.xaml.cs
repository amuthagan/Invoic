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
using System.Data;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmAddCIReference.xaml
    /// </summary>
    public partial class frmAddCIReference : Window
    {
        //public string cIRefNo { get; set; }
        private string _cIRefNo;
        public string CIRefNo
        {
            get { return _cIRefNo; }
            set { this._cIRefNo = value; }
        }
        private string _cIRIssue_No;
        public string CIRIssue_No
        {
            get { return _cIRIssue_No; }
            set { this._cIRIssue_No = value; }
        }
        private string _cIRIssue_Date;
        public string CIRIssue_Date
        {
            get { return _cIRIssue_Date; }
            set { this._cIRIssue_Date = value; }
        }
        private string _cIRIPart_No;
        public string CIRIPart_No
        {
            get { return _cIRIPart_No; }
            set { this._cIRIPart_No = value; }
        }
        private string _cIRICustomer;
        public string CIRICustomer
        {
            get { return _cIRICustomer; }
            set { this._cIRICustomer = value; }
        }
        private string _cIRFinish;
        public string CIRFinish
        {
            get { return _cIRFinish; }
            set { this._cIRFinish = value; }
        }

        DateTime dt;
        DataTable dtFinish;
        DataTable dtCoating;
        public DateTime ltbEnquiredDate { get; set; }
        DataTable dtIrRefNo = new DataTable();
        DataTable dtForecastLocation = new DataTable();
        DataTable dtCustomer = new DataTable();
        CirReference bllReference = null;
        ProductInformation bllProInfo = null;
        private UserInformation _userInformation;

        public frmAddCIReference(UserInformation userInformation)
        {
            InitializeComponent();
            _userInformation = userInformation;
            bllProInfo = new ProductInformation(userInformation);
            bllReference = new CirReference(userInformation);
           
           // _userInformation.StatusBarDetails = this.stMain;
            dt = DateTime.Now.Date;
            dateRecd.SelectedDate = dt;
            //String.Format("{0:MM/dd/yyyy}", dt);  
            loadZone();
            loadAllCombo();
        }
        private void loadZone()
        {
            string[] zOne = new string[] { "N", "E", "S", "W", "O", "U", "P", "I", "X" };
            cmbZone.ItemsSource = zOne;
            ltbEnquiredDate = DateTime.Now.Date;
        }
        private void loadAllCombo()
        {
            if (bllReference.getdtFinish(ref dtFinish) != 0) return;
            bllReference.dtFinish = dtFinish;
            DataContext = bllReference;
            if (bllReference.getdtCoating(ref dtCoating) != 0) return;
            bllReference.dtCoating = dtCoating;
            if (bllProInfo.GetForecastLocation(ref dtForecastLocation) != 0) return;
            bllReference.ForecastLocation = dtForecastLocation;
            if (bllReference.getCustomer(ref dtCustomer) != 0) return;
            DataContext = bllReference;
            cmbCustomer.DataSource = dtCustomer.DefaultView;


        }
        private void cmbZone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string zOne = cmbZone.SelectedValue.ToString();
            bllReference.getCIRRefNo(ref dtIrRefNo, zOne);
            this.CIRefNo = dtIrRefNo.Rows[0]["New_ci_reference"].ToString();
            lbl_CrRefNo.Text = CIRefNo;

        }

        private void CreateCIR_REF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbZone.Text.ToString()))
                {
                    MessageBox.Show("Please Select Zone..");
                    return;
                }



                this.CIRFinish = cmbFinish.Text.ToString() != "" ? cmbFinish.Text.ToString().ToString() : "";
                CIRIssue_No = txtissueNo.Text.ToString() != "" ? txtissueNo.Text.ToString() : "";
                CIRIPart_No = txtPartNo.Text.ToString() != "" ? txtPartNo.Text.ToString() : "";
                CIRIssue_Date = dp_issue_date.Text.ToString() != "" ? dp_issue_date.Text.ToString() : "";
                CIRICustomer = cmbCustomer.SelectedValue.ToString() != "" ? cmbCustomer.SelectedValue.ToString() : "";
                CIRefNo = lbl_CrRefNo.Text.ToString();
                this.Close();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
