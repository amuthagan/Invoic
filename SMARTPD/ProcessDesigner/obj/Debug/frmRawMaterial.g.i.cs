﻿#pragma checksum "..\..\frmRawMaterial.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BF3C79446C24E2B1A49878A1DDC9E2C46B1B0DC2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BHCustCtrl;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Expression.Interactivity.Layout;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using ProcessDesigner.UICommon;
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
using System.Windows.Interactivity;
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
    /// frmRawMaterial
    /// </summary>
    public partial class frmRawMaterial : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddNew;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Edit;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Save;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Close;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ProcessDesigner.UserControls.ComboBoxCus ltbRmCode;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtRmDesc;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblClass;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.DataGrid ssRawMaterial;
        
        #line default
        #line hidden
        
        
        #line 177 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ProcessDesigner.UserControls.NumericTextBox txtDomesticCost;
        
        #line default
        #line hidden
        
        
        #line 190 "..\..\frmRawMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ProcessDesigner.UserControls.DecimalTextBox txtExportCost;
        
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
            System.Uri resourceLocater = new System.Uri("/ProcessDesigner;component/frmrawmaterial.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\frmRawMaterial.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 11 "..\..\frmRawMaterial.xaml"
            ((ProcessDesigner.frmRawMaterial)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.UserControl_KeyDown);
            
            #line default
            #line hidden
            
            #line 11 "..\..\frmRawMaterial.xaml"
            ((ProcessDesigner.frmRawMaterial)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AddNew = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.Edit = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.Save = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.Close = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.ltbRmCode = ((ProcessDesigner.UserControls.ComboBoxCus)(target));
            return;
            case 7:
            this.txtRmDesc = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.lblClass = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.ssRawMaterial = ((Microsoft.Windows.Controls.DataGrid)(target));
            
            #line 113 "..\..\frmRawMaterial.xaml"
            this.ssRawMaterial.MouseMove += new System.Windows.Input.MouseEventHandler(this.ssRawMaterial_MouseMove);
            
            #line default
            #line hidden
            return;
            case 10:
            this.txtDomesticCost = ((ProcessDesigner.UserControls.NumericTextBox)(target));
            
            #line 179 "..\..\frmRawMaterial.xaml"
            this.txtDomesticCost.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtDomesticCost_TextChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.txtExportCost = ((ProcessDesigner.UserControls.DecimalTextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
