﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmSapHalb : UserControl
    {
        public frmSapHalb(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();
            SapViewModel fm = new SapViewModel(userInformation, "HALB");
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
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
    }
}