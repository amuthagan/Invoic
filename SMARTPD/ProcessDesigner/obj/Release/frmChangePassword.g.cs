﻿#pragma checksum "..\..\frmChangePassword.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B45F33DFBBE865021287EDAB1339B2CEB118C0C6"
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
    /// frmChangePassword
    /// </summary>
    public partial class frmChangePassword : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock errOld;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox txtOldPassword;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock errNew;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox txtNewPassword;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock errVer;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox txtVerifyPassword;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\frmChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/ProcessDesigner;component/frmchangepassword.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\frmChangePassword.xaml"
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
            
            #line 4 "..\..\frmChangePassword.xaml"
            ((ProcessDesigner.frmChangePassword)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.errOld = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.txtOldPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 20 "..\..\frmChangePassword.xaml"
            this.txtOldPassword.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtOldPassword_KeyUp);
            
            #line default
            #line hidden
            
            #line 20 "..\..\frmChangePassword.xaml"
            this.txtOldPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.txtOldPassword_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.errNew = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.txtNewPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 23 "..\..\frmChangePassword.xaml"
            this.txtNewPassword.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtNewPassword_KeyUp);
            
            #line default
            #line hidden
            
            #line 23 "..\..\frmChangePassword.xaml"
            this.txtNewPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.txtNewPassword_LostFocus);
            
            #line default
            #line hidden
            return;
            case 6:
            this.errVer = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.txtVerifyPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 26 "..\..\frmChangePassword.xaml"
            this.txtVerifyPassword.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtVerifyPassword_KeyUp);
            
            #line default
            #line hidden
            
            #line 26 "..\..\frmChangePassword.xaml"
            this.txtVerifyPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.txtVerifyPassword_LostFocus);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\frmChangePassword.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\frmChangePassword.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

