using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for frmCFTMeet.xaml
    /// </summary>
    public partial class frmCFTMeet : UserControl
    {
        public frmCFTMeet(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            //ci.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            //string newdateTimeFormat = ci.DateTimeFormat.ToFormattedDateAsString("dd-MMM-yyyy");
            // newdateTimeFormat.DateSeparator = "-";
            ci.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
            CFTMeetingReportViewModel fm = new CFTMeetingReportViewModel(userInformation);
            this.DataContext = fm;
            mdiChild.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => mdiChild.Close());
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)cmbPartNo.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        //private void DGVTime_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //DGVTime.CurrentCell.Column.
        //    DGVTime.Columns[2].IsReadOnly = false;
        //    DGVTime.Columns[3].IsReadOnly = false;
        //    DGVTime.Columns[4].IsReadOnly = false;
        //}

        //private void Border_IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{

        //}
    }
}
