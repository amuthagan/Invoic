   M S B A M L   `   `   `  ����W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core 5   	   J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon 5	   	   qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  5
   	   E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl 5   	   $   ProcessDesigner.frmSeqDwgIssue   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     5   	   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   	   Swfirms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions     5   	   8busy-http://schemas.microsoft.com/wpf/2008/toolkit 
 5   	   5UICommon&clr-namespace:ProcessDesigner.UICommon 
 5	   	   ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 5
   	   =
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl 
 5   	   ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5   	     � Title$  Sequence Drawing Issue��5   	   $	��200��6(   $	��500��65    � 
ResizeMode$ NoResize=�6A      WindowBackground#��[  6W    � WindowStartupLocation$ CenterScreen=�6�    � Icon$ Images/logo.ico��6�    � 	Resources 5      0  *ProcessDesigner.UICommon.DateTimeConverter  DateTimeConverter/  )ProcessDesigner.UICommon.BooleanConverter  BooleanConverter0  *ProcessDesigner.UICommon.CurrencyConverter  CurrencyConverter/  )ProcessDesigner.UICommon.DecimalConverter  DecimalConverter��%t   &	       &	      &	 &     &	 9       5   
   6%     5   
   6$     5   
   6%     5   
   6$   5      .��5      � $��5q�6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��dgvProdDwgMast��5   -     AutoGenerateColumns .     CanUserDeleteRows .     CanUserAddRows .     CanUserResizeRows .  	   RowHeaderWidth$	 20Q�6z   
   SelectionMode$
 Single=�6�      SelectionUnit$ FullRow=�6     
IsReadOnly . 2  ,ProcessDesigner.UICommon.ScrollToTopBehavior   ScrollToTop . ��6C  �� *DV_PROD_DWG_ISSUE+   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Location Code��6)      CellTemplate 5      �� 5      .R�5      �� ��6(   �� *
LOC_CODE+5         CellEditingTemplate 5      �� 5      .R�5      5	   /ProcessDesigner.UserControls.ComboBoxWithSearch	   	  
IsReadOnly .  	  SelectedValuePath$ LOC_CODE��5$   .    	  DisplayValuePath$ LOC_CODE��5%   .   $��Center=�5&   .    	  
DataSource 5    -   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >� �� Path$  DataContext.LocationMaster� 	  ColumnsHeader 5!   .   ��  �� *FindAncestor+" >�$' !DataContext.DropDownItemsLocation� 	  SelectedText 5"   .   �� *
LOC_CODE+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� 	  SelectedValue 5#   .   �� *
LOC_CODE+$ TwoWay=�$ PropertyChanged=�5'      5(      5*      0
  *System.Windows.Controls.DataGridTextColumn
  5+         Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6%   $ Issue No��60   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *
ISSUE_NO+$ PropertyChanged=�$ TwoWay=�    EditingElementStyle  5,      �� 5-      ! �� 
TargetType"! ��6    .�5.      �� " �� Property" �@��	MaxLength# �� Value$# 3��6:   �� 5/      2   ,ProcessDesigner.UserControls.TextBoxIntsOnly" �@ Mask$# UnSignedDec=�6M   6c   50      52        53      $
 Date��6)   $ * 67   $   SortMemberPath$$ 
ISSUE_DATE��6A    54      �� 55      .R�56      �� ��6(   �� *
ISSUE_DATE+% �� 	Converter#% [ $ OneWay=�57       59      �� 5:      .R�5;      0   *ProcessDesigner.UserControls.DatePickerCus  $��txtIssueDate��65   & � ToolTip$& Select Date��6J   '   SelectedDate' 5<   &   �� *
ISSUE_DATE+$ PropertyChanged=�$ TwoWay=�5=      5>      5@      
  5A      $ Alterations��6%   $ * 6:    6E   �� *ISSUE_ALTER+$ PropertyChanged=�(   
IsReadOnly( 6�   ��  �� *FindAncestor+" 
 $ IsReadOnlyPrdCode�)   ElementStyle) 5B      �� 5C      "! ��6    .�5D      �� " � ��$# Center=�6B   5E        5G      �� 5H      "! ��6    .�5I      �� " � ��$# Center=�6I   �� 5J      " �@��	MaxLength$
# 1000��6:   6I   5K      5M      
  5N      $
 Intl��6%   $ * 63    6>   �� *COMPILED_BY+$ PropertyChanged=�( 6�   ��  �� *FindAncestor+" 
 $ IsReadOnlyPrdCode�) 5O      �� 5P      "! ��6    .�5Q      �� " � ��$# Center=�6B   5R        5T      �� 5U      "! ��6    .�5V      �� " � ��$# Center=�6I   �� 5W      " �@��	MaxLength$# 10��6:   6G   5X      5Z      5\      5^      5_      