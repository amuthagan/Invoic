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
    /// Interaction logic for frmCCMaster.xaml
    /// </summary>
    public partial class frmCCMaster : UserControl
    {       
        public frmCCMaster(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                CostCenterMasterDet costcenter = new CostCenterMasterDet(userInformation);
                InitializeComponent();
                ViewModel.CCMasterViewModel ccmvm = new ViewModel.CCMasterViewModel(userInformation);
                this.DataContext = ccmvm;
                mdiChild.Closing += ccmvm.CloseMethod;
                if (ccmvm.CloseAction == null)
                    ccmvm.CloseAction = new Action(() => mdiChild.Close());
                List<ProcessDesigner.Model.V_TABLE_DESCRIPTION> lstTableDescription = costcenter.GetTableColumnsSize("DDCOST_CENT_MAST");
                this.SetColumnLength<TextBox>(lstTableDescription);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)usrCostCentreCode.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void FocusTextBoxOnLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.Clear();
                var textbox = sender as TextBox;
                if (textbox == null) return;
                textbox.Focus();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }


        }

        private void tbFixCost_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var textbox = sender as TextBox;
                textbox.Text = Convert.ToDouble(textbox.Text).ToString("0.00");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        private void tbFixCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumericTextBox(sender);
        }

        private void NumericTextBox(object sender)
        {
            try
            {
                int charAt;
                ProcessDesigner.UserControls.DecimalTextBox textbox = sender as ProcessDesigner.UserControls.DecimalTextBox;
                if (textbox.Text.Trim() != "")
                {
                    if (textbox.Text.IndexOf(".") < 0)
                    {
                        textbox.Text = Convert.ToDouble(textbox.Text).ToString("0.00");
                        textbox.SelectionStart = textbox.Text.IndexOf(".");
                    }

                    charAt = textbox.Text.IndexOf(".");
                    if (charAt > textbox.MaxLength - textbox.MaxDecimalPlaces)
                    {
                        textbox.Text = Convert.ToDecimal(textbox.Text.Substring(0, charAt - 1)).ToString("0.00");
                    }



                    if (textbox.Text.IndexOf(".") == textbox.Text.Length - 1)
                    {
                        textbox.Text = Convert.ToDouble(textbox.Text).ToString("0.00");
                        textbox.SelectionStart = textbox.Text.IndexOf(".");
                    }

                    charAt = textbox.Text.IndexOf(".");
                    if (charAt >= 0)
                    {
                        if (textbox.Text.Trim().Substring(charAt + 1).Length < 2 || charAt == 0)
                        {
                            textbox.Text = Convert.ToDouble(textbox.Text).ToString("0.00");
                            textbox.SelectionStart = textbox.Text.IndexOf(".") + 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
