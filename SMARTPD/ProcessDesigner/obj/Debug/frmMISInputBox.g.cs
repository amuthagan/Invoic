﻿#pragma checksum "..\..\frmMISInputBox.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CEF55E1714A8F39EB035D4BF3EE3D83E8DD1B398"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ProcessDesigner.UserControls;
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
    /// frmMISInputBox
    /// </summary>
    public partial class frmMISInputBox : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\frmMISInputBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblDisplay;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\frmMISInputBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Save;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\frmMISInputBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Cancel;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\frmMISInputBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Txt_InputBox;
        
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
            System.Uri resourceLocater = new System.Uri("/ProcessDesigner;component/frmmisinputbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\frmMISInputBox.xaml"
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
            this.lblDisplay = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.Btn_Save = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\frmMISInputBox.xaml"
            this.Btn_Save.Click += new System.Windows.RoutedEventHandler(this.Btn_Save_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Btn_Cancel = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\frmMISInputBox.xaml"
            this.Btn_Cancel.Click += new System.Windows.RoutedEventHandler(this.Btn_Cancel_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Txt_InputBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 20 "..\..\frmMISInputBox.xaml"
            this.Txt_InputBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.Txt_InputBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 20 "..\..\frmMISInputBox.xaml"
            this.Txt_InputBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.Txt_InputBox_KeyUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

