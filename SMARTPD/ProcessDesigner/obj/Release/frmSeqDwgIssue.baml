   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl $   ProcessDesigner.frmSeqDwgIssue   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     Swfirms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions     8busy-http://schemas.microsoft.com/wpf/2008/toolkit 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 =
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl 
 ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
   � Title$  Sequence Drawing Issue��$	��200��$	��500�� � 
ResizeMode$ NoResize=�   WindowBackground#��[   � WindowStartupLocation$ CenterScreen=� � Icon$ Images/logo.ico�� � 	Resources 0  *ProcessDesigner.UICommon.DateTimeConverter  DateTimeConverter/  )ProcessDesigner.UICommon.BooleanConverter  BooleanConverter0  *ProcessDesigner.UICommon.CurrencyConverter  CurrencyConverter/  )ProcessDesigner.UICommon.DecimalConverter  DecimalConverter��%<   &	       &	      &	 
     &	              .��� $��5q�.+�&   System.Windows.Controls.DataGrid  -   $��dgvProdDwgMast��   AutoGenerateColumns .     CanUserDeleteRows .     CanUserAddRows .     CanUserResizeRows .  	   RowHeaderWidth$	 20Q�
   SelectionMode$
 Single=�   SelectionUnit$ FullRow=�   
IsReadOnly . 2  ,ProcessDesigner.UICommon.ScrollToTopBehavior   ScrollToTop . ���� *DV_PROD_DWG_ISSUE+   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Location Code��   CellTemplate �� .R��� ���� *
LOC_CODE+   CellEditingTemplate �� .R�5	   /ProcessDesigner.UserControls.ComboBoxWithSearch	   	  
IsReadOnly .  	  SelectedValuePath$ LOC_CODE�� 	  DisplayValuePath$ LOC_CODE��$��Center=� 	  
DataSource ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >� �� Path$  DataContext.LocationMaster� 	  ColumnsHeader ��  �� *FindAncestor+" >�$' !DataContext.DropDownItemsLocation� 	  SelectedText �� *
LOC_CODE+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� 	  SelectedValue �� *
LOC_CODE+$ TwoWay=�$ PropertyChanged=�0
  *System.Windows.Controls.DataGridTextColumn
     Width5  /System.Windows.Controls.DataGridLengthConverter$ * $ Issue No��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
ISSUE_NO+$ PropertyChanged=�$ TwoWay=�    EditingElementStyle  �� ! �� 
TargetType"! ��.��� " �� Property" �@��	MaxLength# �� Value$# 3���� 2   ,ProcessDesigner.UserControls.TextBoxIntsOnly" �@ Mask$# UnSignedDec=�  $
 Date��$ * $   SortMemberPath$$ 
ISSUE_DATE�� �� .R��� ���� *
ISSUE_DATE+% �� 	Converter#% [ $ OneWay=� �� .R�0   *ProcessDesigner.UserControls.DatePickerCus  $��txtIssueDate��& � ToolTip$& Select Date��'   SelectedDate' �� *
ISSUE_DATE+$ PropertyChanged=�$ TwoWay=�
  $ Alterations��$ *  �� *ISSUE_ALTER+$ PropertyChanged=�(   
IsReadOnly( ��  �� *FindAncestor+" 
 $ IsReadOnlyPrdCode�)   ElementStyle) �� "! ��.��� " � ��$# Center=�  �� "! ��.��� " � ��$# Center=��� " �@��	MaxLength$
# 1000��
  $
 Intl��$ *  �� *COMPILED_BY+$ PropertyChanged=�( ��  �� *FindAncestor+" 
 $ IsReadOnlyPrdCode�) �� "! ��.��� " � ��$# Center=�  �� "! ��.��� " � ��$# Center=��� " �@��	MaxLength$# 10��