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

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmHardnessConversion.xaml
    /// </summary>
    public partial class frmHardnessConversion : Window
    {
        public frmHardnessConversion()
        {
            InitializeComponent();
            LoadDataToLst();
        }
        public void LoadDataToLst()
        {
            string rowData = "Diamond Pyramid,BH Standard Ball,BH Hultgren Ball,BH Tungsten Ball,RockWell A--60,RockWell B--100,RockWell C--150,RockWell D--100,RockkSuperficial 15N,RockkSuperficial 30N,RockkSuperficial 45N,Shorescleroscope,Tensile Strength";
            this.LstBox1.ItemsSource = null;
            this.LstBox1.Items.Clear();
            this.LstBox2.ItemsSource = null;
            this.LstBox2.Items.Clear();
            this.LstBox1.ItemsSource = rowData.Split(',');
            this.LstBox2.ItemsSource = rowData.Split(',');
           
        }
    }
   
}
