﻿#pragma checksum "..\..\frmProductCategoryMaster.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "37900FE8683BEA1D13D3C860BA63EA0079DF4EAD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Expression.Interactivity.Core;
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
    /// frmProductCategoryMaster
    /// </summary>
    public partial class frmProductCategoryMaster : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Edit;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Delete;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Save;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Close;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtProductCode;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ProcessDesigner.UserControls.ComboBoxCus cmbPrdCategory;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgrdType;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgrdTypeNew;
        
        #line default
        #line hidden
        
        
        #line 192 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgrdSubType;
        
        #line default
        #line hidden
        
        
        #line 247 "..\..\frmProductCategoryMaster.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgrdSubTypeNew;
        
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
            System.Uri resourceLocater = new System.Uri("/ProcessDesigner;component/frmproductcategorymaster.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\frmProductCategoryMaster.xaml"
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
            
            #line 11 "..\..\frmProductCategoryMaster.xaml"
            ((ProcessDesigner.frmProductCategoryMaster)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            
            #line 11 "..\..\frmProductCategoryMaster.xaml"
            ((ProcessDesigner.frmProductCategoryMaster)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.UserControl_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Btn_Edit = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.Btn_Delete = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.Btn_Save = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.Btn_Close = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.txtProductCode = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.cmbPrdCategory = ((ProcessDesigner.UserControls.ComboBoxCus)(target));
            return;
            case 8:
            this.dgrdType = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 9:
            this.dgrdTypeNew = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            this.dgrdSubType = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 11:
            this.dgrdSubTypeNew = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
