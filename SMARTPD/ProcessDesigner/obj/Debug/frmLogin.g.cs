﻿#pragma checksum "..\..\frmLogin.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E25BADF165D6D5B0B94ABB4310D1EB82E966459F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ProcessDesigner {
    
    
    /// <summary>
    /// frmLogin
    /// </summary>
    public partial class frmLogin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 66 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtOQA;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtDD;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Txtusername;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TxtPassword;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblPasswater;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdCapsLock;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
        #line default
        #line hidden
        
        
        #line 154 "..\..\frmLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtLastUpdate;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProcessDesigner;component/frmlogin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\frmLogin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\frmLogin.xaml"
            ((ProcessDesigner.frmLogin)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.Window_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 30 "..\..\frmLogin.xaml"
            ((System.Windows.Controls.Image)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Image_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.rbtOQA = ((System.Windows.Controls.RadioButton)(target));
            
            #line 66 "..\..\frmLogin.xaml"
            this.rbtOQA.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.rbtOQA_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.rbtDD = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.Txtusername = ((System.Windows.Controls.TextBox)(target));
            
            #line 77 "..\..\frmLogin.xaml"
            this.Txtusername.KeyUp += new System.Windows.Input.KeyEventHandler(this.Txtusername_KeyUp);
            
            #line default
            #line hidden
            return;
            case 6:
            this.TxtPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 91 "..\..\frmLogin.xaml"
            this.TxtPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.TxtPassword_PasswordChanged);
            
            #line default
            #line hidden
            
            #line 91 "..\..\frmLogin.xaml"
            this.TxtPassword.KeyUp += new System.Windows.Input.KeyEventHandler(this.TxtPassword_KeyUp);
            
            #line default
            #line hidden
            
            #line 91 "..\..\frmLogin.xaml"
            this.TxtPassword.GotFocus += new System.Windows.RoutedEventHandler(this.TxtPassword_GotFocus);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lblPasswater = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.grdCapsLock = ((System.Windows.Controls.Grid)(target));
            return;
            case 9:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 124 "..\..\frmLogin.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 126 "..\..\frmLogin.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.txtLastUpdate = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

