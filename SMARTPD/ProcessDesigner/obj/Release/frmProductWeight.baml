   M S B A M L   `   `   `  ���� E  ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core C&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon &    ProcessDesigner.frmProductWeight   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 =
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl 
 0sys&clr-namespace:System;assembly=mscorlib 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 isi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions 
 3Common&clr-namespace:ProcessDesigner.UICommon 
 -   $��ProductWeight��$	��600��$	��900��  � 	Resources  /  )ProcessDesigner.UICommon.DecimalConverter   DecimalConverter��%   &	          .��� .+��� ._��  � RowDefinitions �� $��60 ��� $
��Auto ��� $��* ��� $��60 �.+��� $��1q�$��0,4,0,4q�._��  � ColumnDefinitions �� $	��50* ��� $	��50* ��� $	��50* �.+��� $��CI Reference : ��$��0��$��0�� �� TextAlignment$ Right=�$
��Bold"��� $��0��$��1��$
 Left=�$
��Bold"����� *ProductWeight.CIreference+�� -   $��btnCalculate��$��
_Calculate��$��40��$��0��$��2��$	��100������ *CalculateCommand+�� $��Weight Calculation  : ��$��1��  
HeaderText#��[ �� $��All Dimensions in mm��$��1��$��1��$��Right=�$ Right=�#��[ $��Center=�$��10(�) 
 #Microsoft.Windows.Controls.DataGrid  -     Name$ dgvWeightCalc��$��2��$��0��   AutoGenerateColumns .  $��Stretch=�   SelectionMode$ Extended=�   SelectionUnit$ FullRow=�$��Stretch=�   RowHeaderWidth$ 20Q�	   CanUserAddRows	 .  
   CanUserDeleteRows
 .  2  ,ProcessDesigner.UICommon.ScrollToTopBehavior   ScrollToTop . ���� *ProductWeight.DVShapeDetails+ �� UpdateSourceTrigger$ PropertyChanged=� �� SelectedItem �� *ProductWeightSelectedItem+ �� Mode$ TwoWay=�$ PropertyChanged=�   CurrentCell �� *CurrentCell+$ TwoWay=�$ PropertyChanged=� � ContextMenu �� .@�m� $��Delete��{��� *DeleteButtonIsEnable+ m� Command �� *DeleteCommand+ m� CommandParameter ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" �� �� Path$" PlacementTarget.SelectedItem� J� InputBindings �� 
 �� Key$ Delete�� �� Command �� *DeleteCommand+ �� CommandParameter ��  �� ElementName$ dgvWeightCalc��$ SelectedItem�.  (System.Windows.Interactivity.Interaction   	Behaviors 6 0ProcessDesigner.UICommon.DataGridBehaviorToolkit     Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ RowEditEnding��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *RowEditEndingCommand+    CommandParameter  �� $ dgvWeightCalc��$ SelectedItem�  $ BeginningEdit��. >	  8Microsoft.Expression.Interactivity.Core.CallMethodAction	  ! 	  
MethodName$! OnBeginningEdit��" 	  TargetObject" ��   $ CurrentCellChanged��. 	  $#! WeightCalc_CurrentCellChanged��" ��   $ CellEditEnding��. 	  $! OnCellEditEnding��" �� #   Columns# 7
 
 1Microsoft.Windows.Controls.DataGridTemplateColumn
  / 
 )Microsoft.Windows.Controls.DataGridColumn$   Header$$ 
Shape Code��%   Width8 
 2Microsoft.Windows.Controls.DataGridLengthConverter$	% 100 &   SortMemberPath$& 
SHAPE_CODE��' 
  CellTemplate' �� .R��� ���� *
SHAPE_CODE+$ PropertyChanged=�( 
  CellEditingTemplate( �� .R�5  /ProcessDesigner.UserControls.ComboBoxWithSearch  )   SelectedValuePath$) 
SHAPE_CODE��*   DisplayValuePath$* 
SHAPE_CODE��+   
IsReadOnly+ . ,   DropDownWidth$	, 300��-   DropDownHeight$	- 200��$��Center=�.   
DataSource. ��  �� *FindAncestor+" >�$' !DataContext.ProductWeight.DVShape�/   SelectedValue/ �� *
SHAPE_CODE+$ TwoWay=�$ PropertyChanged=�0   SelectedText0 �� *
SHAPE_CODE+$ TwoWay=�$ PropertyChanged=�1   SelectedItem1 ��  �� *FindAncestor+" >�$( "DataContext.ShapeCode_SelectedItem�$ OneWayToSource=�$ PropertyChanged=�2 �� NotifyOnTargetUpdated2 . 3   ColumnsHeader3 ��  �� *FindAncestor+" >�$! DataContext.DropdownHeaders�   $ SelectedItemChanged��.    �� $ ProductWeight��$) #DataContext.OnShapeSelectionChanged�3 
 -Microsoft.Windows.Controls.DataGridTextColumn  $$ Dimension - I��$
% 1.5* 4 
 .Microsoft.Windows.Controls.DataGridBoundColumn4   Binding4 �� *HEAD1+$ PropertyChanged=�5   ElementStyle5 �� 6 �� 
TargetType"6 ��.��� 7 �� Property7 � ��8 �� Value$8 Center=�9   EditingElementStyle9 �� "6 ��.��� 7 �@��
IsReadOnly8 . &    BHCustCtrl.DataGridNumericColumn  $$ Value��$% 1* :   MaxPrecision$: 10��;   MaxScale$; 2��4 �� *VAL1+$ PropertyChanged=�$ TwoWay=�  $$ Dimension - II��$
% 1.5* 4 �� *HEAD2+$ PropertyChanged=�$ TwoWay=�5 �� "6 ��.��� 7 � ��$8 Center=��� 7 � ��8 �� *HEAD2+$ PropertyChanged=�$ TwoWay=�9 �� "6 ��.��� 7 �@��
IsReadOnly8 .   $$ Value��$% 1* $: 10��$; 2��4 �� *VAL2+$ PropertyChanged=�$ TwoWay=�  $$ Dimension - III��$
% 1.5* 4 �� *HEAD3+$ PropertyChanged=�5 �� "6 ��.��� 7 � ��$8 Center=�9 �� "6 ��.��� 7 �@��
IsReadOnly8 .   $$ Value��$% 1* $: 10��$; 2��4 �� *VAL3+$ PropertyChanged=�$ TwoWay=�  $$ Volume��$% 1* 4 �� *VOLUME+$ PropertyChanged=�< �� 	Converter#< [  5 �� "6 ��.��� 7 � ��$8 Center=��� 7 �@��TextAlignment$8 Right=�9 �� "6 ��.��� 7 �@��TextAlignment$8 Right=��� 7 �@��
IsReadOnly8 . �� 2  ,ProcessDesigner.UserControls.TextBoxIntsOnly7 �@ Mask$8 Decimal=�  $	$ +/-��$% 50 4 �� *SIGN+$ PropertyChanged=�5 �� "6 ��.��� 7 � ��$8 Center=��� 7 � ��$8 Center=�9 �� "6 ��.��� 7 �@��TextAlignment$8 Center=��� 7 �@��	MaxLength$8 1���� $��3��$��1q�$��0,4,0,4q�._��  �� $	��100 ��� $��* ��� $	��160 ��� $��* ��� $	��110 ��� $	��150 ��� $	��150 �.+��� $��0��$ Right=�$
��Bold"����� *WeightOptionText+1  +ProcessDesigner.UserControls.DecimalTextBox  -   $��txtCheeseWeight��$��1��= �� 	MaxLength$= 13��>   AllowNegative> . ?   MaxDecimalPlaces$? 2��$��95��$
��Left=�@ �� 
IsReadOnly@ �� *IsReadOnlyCheeseWeight+���� *ProductWeight.CheesWeight+$ PropertyChanged=�A �� StringFormat$A F2��-  'ProcessDesigner.UICommon.FocusExtensionB   	IsFocusedB �� *CheeseWtIsFocus+.��   $ 	LostFocus��.    �� *LostFocusCheeseWeightCommand+.���� $��Kgs / 100 pcs��$��2��$
 Left=�$��10,0,0,0q�$��Italic$�  -   $��txtTotal��$��95��$
��Left=�@ . $��3��$= 13��> . $? 2������ *ProductWeight.Total+$ PropertyChanged=�$A F2���� $��Lbs / 100 pcs��$��4��$
 Left=�$��10,0,0,0q�$��Italic$��� -   $��btnOk��$	��_OK��$��40��$��5��$	��100��$��Right=�$��0,0,2,0q����� *SaveCommand+�� -   $��	btnCancel��$��_Cancel��$��40��$��6��$	��100��$
��Left=����� *CloseCommand+