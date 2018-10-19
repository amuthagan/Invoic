using ProcessDesigner.Model;
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
    /// Interaction logic for frmInputBox.xaml
    /// </summary>
    public partial class frmInputBox : Window
    {
        public bool BlnOk = false;
        public decimal BlnOkR = 0;
        public frmInputBox()
        {

            InitializeComponent();
            MaxLengthValue = 50;
            this.Title = "InputBox";
            this.lblDisplay.Text = "Enter The New Tool Code";
            Txt_InputBox.Focus();
        }
        public frmInputBox(string frmInputTitle)
        {
            InitializeComponent();
            MaxLengthValue = 6;
            this.Title = frmInputTitle + " Input";
            this.lblDisplay.Text = "Enter Finish Code";
            Txt_InputBox.Focus();
        }
        public frmInputBox(string frmInputTitle, string lblCaption)
        {
            InitializeComponent();
            MaxLengthValue = 6;
            this.Title = "BOM Input";
            this.lblDisplay.Text = "Unit";
            if (lblCaption != "") this.lblDisplay.Text = lblCaption;
            Txt_InputBox.Focus();
        }
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            BlnOk = true;
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            BlnOk = false;
            Txt_InputBox.Text = "";
            this.Close();
        }
        private int _maxLengthValue = 50;
        public int MaxLengthValue
        {
            get { return _maxLengthValue; }
            set
            {
                _maxLengthValue = value;

            }
        }

        private void Txt_InputBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.Title != "SmartPD - New Tool Code")
            {
                TextBox box = (TextBox)sender;
                e.Handled = box.Text.Length > 5;
            }

        }

        private void Txt_InputBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    Btn_Save.Focus();
            //    Btn_Save_Click(sender, e);
            //}


            //if (e.Key == Key.Enter)
            //{
            //    BlnOk = true;
            //    this.Close();
            //}
        }

        private void Txt_InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Btn_Save.Focus();
                Btn_Save_Click(sender, e);
            }
        }
    }
}
