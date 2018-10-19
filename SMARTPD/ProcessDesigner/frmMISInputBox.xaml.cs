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
    public partial class frmMISInputBox : Window
    {
        public bool BlnOk = false;

        public frmMISInputBox(string frmInputTitle, string lblCaption)
        {
            if (frmInputTitle == "Report Title")
            {
                InitializeComponent();
                this.Title = frmInputTitle;
                this.lblDisplay.Text = lblCaption;
                if (lblCaption != "") this.lblDisplay.Text = lblCaption;
                Txt_InputBox.Focus();
            }
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

        private void Txt_InputBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.Title != "InputBox")
            {
                TextBox box = (TextBox)sender;
                e.Handled = box.Text.Length > 5;
            }

        }

        private void Txt_InputBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BlnOk = true;
                this.Close();
            }
        }
    }
}
