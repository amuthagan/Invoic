using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for Testing.xaml
    /// </summary>
    public partial class Testing : Window
    {
        public Testing()
        {
            InitializeComponent();
        }

        private UserInformation user;
        public Testing(UserInformation userinfo)
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;
            user = userinfo;
        }

        DataTable dt = new DataTable();
        private void btnReferesh_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sqlQry = new StringBuilder();
            TestingBll bll = new TestingBll(user);
            int count = txtMonth.Text.ToIntValue();
            DateTime startdate = dpStartdate.SelectedDate.Value;
            DateTime first_month_date = startdate;

            List<PGCATOGORY> lstEntity = (from row in bll.DB.PGCATOGORY
                                          select row).ToList<PGCATOGORY>();
            if (lstEntity.Count > 0)
            {
                bll.DB.PGCATOGORY.DeleteAllOnSubmit(lstEntity);
                bll.DB.SubmitChanges();
            }

            List<PGCATOGORY_2> lstEntity1 = (from row in bll.DB.PGCATOGORY_2
                                             select row).ToList<PGCATOGORY_2>();
            if (lstEntity1.Count > 0)
            {
                bll.DB.PGCATOGORY_2.DeleteAllOnSubmit(lstEntity1);
                bll.DB.SubmitChanges();
            }

            startdate = startdate.AddMonths(-1);
            for (int i = 1; i <= count; i++)
            {
                startdate = startdate.AddMonths(1);
                sqlQry.Append("SELECT PG_CATEGORY PG,");
                sqlQry.Append("  DOC_REL_DT_ACTUAL,");
                sqlQry.Append("  PPAP_ACTUAL_DT ");
                sqlQry.Append("FROM mfm_mast a Right join prd_mast b on  a.part_no = b.part_no ");
                //sqlQry.Append("WHERE b.pg_category     IN ('1','2','3','3F') ");
                sqlQry.Append("WHERE B.Allot_date        IS NOT NULL ");
                sqlQry.Append("AND A.DOC_REL_DT_ACTUAL IS NOT NULL ");
                sqlQry.Append("AND samp_submit_date BETWEEN Convert(datetime,'" + startdate.ToFormattedDateAsString() + "',103) AND  Convert(datetime,'" + startdate.AddMonths(1).AddDays(-1).ToFormattedDateAsString() + "',103) ");
                sqlQry.Append("AND samp_submit_date IS NOT NULL ");
                if (txtPG.Text == "3")
                {
                    sqlQry.Append("AND b.pg_category in ('3','3F')");
                }
                else
                {
                    sqlQry.Append("AND b.pg_category = '" + txtPG.Text + "'");
                }
                dt = bll.GetDataTable(sqlQry);

                decimal leadtime = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!dr["DOC_REL_DT_ACTUAL"].IsNotNullOrEmpty() || !dr["PPAP_ACTUAL_DT"].IsNotNullOrEmpty())
                        {
                            switch (txtPG.Text)
                            {
                                case "1":
                                    leadtime = leadtime + 60;
                                    break;
                                case "2":
                                    leadtime = leadtime + 90;
                                    break;
                                case "3":
                                    leadtime = leadtime + 100;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            decimal days = Math.Abs(((Convert.ToDateTime(dr["PPAP_ACTUAL_DT"]) - Convert.ToDateTime(dr["DOC_REL_DT_ACTUAL"])).TotalDays).ToString().ToDecimalValue());
                            switch (txtPG.Text)
                            {
                                case "1":
                                    leadtime = leadtime + ((days > 60) ? 60 : days);
                                    break;
                                case "2":
                                    leadtime = leadtime + ((days > 90) ? 90 : days);
                                    break;
                                case "3":
                                    leadtime = leadtime + ((days > 150) ? 100 : days);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    leadtime = leadtime / dt.Rows.Count;
                }

                PGCATOGORY pgcat = new PGCATOGORY();
                pgcat.SNO = i;
                pgcat.PGCATOGORY1 = txtPG.Text.ToDecimalValue();
                pgcat.LEADTIME = Math.Round(leadtime, 2);
                pgcat.NO_OF_PRODUCTS = dt.Rows.Count;
                pgcat.PG_DATE = startdate;
                bll.DB.PGCATOGORY.InsertOnSubmit(pgcat);
                bll.DB.SubmitChanges();
            }

            for (int i = 1; i <= 4; i++)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select sum(leadtime) LEADTIME,sum(no_of_products) NO_OF_PRODUCTS,min(PG_DATE) PG_DATE from pgcatogory where  pgcatogory = '" + txtPG.Text + "' and PG_DATE BETWEEN Convert(datetime,'" + first_month_date.ToFormattedDateAsString() + "',103) AND  Convert(datetime,'" + first_month_date.AddMonths(6).AddDays(-1).ToFormattedDateAsString() + "',103) ");
                DataTable dt1 = bll.GetDataTable(sql);

                if (dt1.Rows.Count > 0)
                {
                    PGCATOGORY_2 pgcat2 = new PGCATOGORY_2();
                    pgcat2.SNO = i;
                    pgcat2.PGCATOGORY = txtPG.Text.ToDecimalValue();
                    pgcat2.LEADTIME = Math.Round((dt1.Rows[0]["LEADTIME"].ToString().ToDecimalValue() > 0) ? dt1.Rows[0]["LEADTIME"].ToString().ToDecimalValue() / 6 : dt1.Rows[0]["LEADTIME"].ToString().ToDecimalValue(), 2);
                    pgcat2.NO_OF_PRODUCTS = Math.Round((dt1.Rows[0]["NO_OF_PRODUCTS"].ToString().ToDecimalValue() > 0) ? dt1.Rows[0]["NO_OF_PRODUCTS"].ToString().ToDecimalValue() / 6 : dt1.Rows[0]["NO_OF_PRODUCTS"].ToString().ToDecimalValue(), 2);
                    pgcat2.START_DATE = first_month_date;
                    pgcat2.END_DATE = first_month_date.AddMonths(6).AddDays(-1);
                    bll.DB.PGCATOGORY_2.InsertOnSubmit(pgcat2);
                    bll.DB.SubmitChanges();
                }
                first_month_date = first_month_date.AddMonths(6);
            }

            StringBuilder sql2 = new StringBuilder();
            sql2.Append("select * from pgcatogory where  pgcatogory = '" + txtPG.Text + "' and PG_DATE > Convert(datetime,'" + first_month_date.ToFormattedDateAsString() + "',103) ");
            DataTable dt2 = bll.GetDataTable(sql2);
            int k = 4;
            foreach (DataRow dr in dt2.Rows)
            {
                k = k + 1;
                PGCATOGORY_2 pgcat2 = new PGCATOGORY_2();
                pgcat2.SNO = k;
                pgcat2.PGCATOGORY = txtPG.Text.ToDecimalValue();
                pgcat2.LEADTIME = dr["LEADTIME"].ToString().ToDecimalValue();
                pgcat2.NO_OF_PRODUCTS = dr["NO_OF_PRODUCTS"].ToString().ToDecimalValue();
                pgcat2.START_DATE = Convert.ToDateTime(dr["PG_DATE"]);
                pgcat2.END_DATE = Convert.ToDateTime(dr["PG_DATE"]);
                bll.DB.PGCATOGORY_2.InsertOnSubmit(pgcat2);
                bll.DB.SubmitChanges();
            }


            MessageBox.Show("Records Added Successfully.", "Tesing", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }

    partial class TestingBll : Essential
    {
        public TestingBll(UserInformation userInformation)
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

        public DataTable GetDataTable(StringBuilder sqlqry)
        {
            return Dal.GetDataTable(sqlqry);
        }
    }
}
