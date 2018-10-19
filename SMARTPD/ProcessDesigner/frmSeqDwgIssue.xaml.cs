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
using ProcessDesigner.ViewModel;
using System.Data;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmSeqDwgIssue.xaml
    /// </summary>
    public partial class frmSeqDwgIssue : Window
    {
        public string LocCode = "";
        public string IssueNo = "";
        public bool IsChanged = false;
        public DateTime? IssueDate;
        public SeqDwgIssueViewmodel Vm;
        public frmSeqDwgIssue(UserInformation userInfo, string partNo)
        {
            InitializeComponent();
            Vm = new SeqDwgIssueViewmodel(userInfo, partNo);
            this.DataContext = Vm;
            this.Closing += Vm.CloseMethodWindow;
            if (Vm.CloseAction == null)
                Vm.CloseAction = new Action(() => this.Close());
        }

        private void dgvProdDwgMast_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            LoadSelectedData();

        }

        private void LoadSelectedData()
        {
            try
            {
                IsChanged = true;
                LocCode = ((DataRowView)dgvProdDwgMast.SelectedItem)["Loc_Code"].ToString();
                IssueNo = ((DataRowView)dgvProdDwgMast.SelectedItem)["issue_no"].ToString();
                if (((DataRowView)dgvProdDwgMast.SelectedItem)["issue_date"].ToString().Length > 0)
                    IssueDate = Convert.ToDateTime(((DataRowView)dgvProdDwgMast.SelectedItem)["issue_date"]);
                Vm.Closed = true;
                this.Close();
            }
            catch (Exception)
            {

            }

        }

        private void dgvProdDwgMast_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                LoadSelectedData();
            }
        }

    }
}
