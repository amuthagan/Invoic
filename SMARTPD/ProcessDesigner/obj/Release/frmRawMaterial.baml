   M S B A M L   `   `   `  ���� c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core �bclr-namespace:Microsoft.Expression.Interactivity.Layout;assembly=Microsoft.Expression.Interactions)Microsoft.Expression.Interactivity.Layout J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl C&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon $   ProcessDesigner.frmRawMaterial   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U	 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     	 R
 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml    
 ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity    
 cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls    
 oic`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    
 qilbclr-namespace:Microsoft.Expression.Interactivity.Layout;assembly=Microsoft.Expression.Interactions    
 ;UICommon&clr-namespace:ProcessDesigner.UICommon    
 dControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls    
 C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl    
 ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 	 3Common&clr-namespace:ProcessDesigner.UICommon 	 -   $	��500��$	��800��   WindowBackground#��[  
  � Tag$  RawMaterial�� � 	Resources   LocationViewSource �� Source �� Path��%8   &	       ��  �� $ RawMaterialLocationMaster�.���� ._�� -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=� � RowDefinitions �� $��6* ��� $	��11* ��� $	��40* ��� $��5* �.+�c� $��0��.@��� -    ��Name$ AddNew��$��40��$��65��$��Add F3��   	IsTabStop .  0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Add.png��{��� *AddButtonIsEnable+���� *AddNewClickCommand+�� -   $
 Edit��$��40��$��75��$��Edit/View F5�� .  $ Images/Edit.png��{��� *EditButtonIsEnable+���� *EditClickCommand+�� -   $
 Save��$��40��$��70��$��1q�$��Save Ctrl+S�� .  $ Images/Save.png��{��� *SaveButtonIsEnable+���� *SaveClickCommand+�� -   $ Close��$��40��$��65��$��Close F9�� .  $ Images/Close.png������ *CloseClickCommand+�� $��1��$��1q�$��2,2,2,2q�._�� $��1��$��	10,5,10,0q� �� $��* ��� $��* ��� $��* �	 � ColumnDefinitions	 �� $
��Auto ��� $��3 ��� �� $��10 �.+��� $��
RM Code : ��$��0��$��Center=�$��Right=�$��0���� $��
RM Desc : ��$��0��$��Center=�$��Right=�$��1���� $��Class : ��$��0��$��Center=�$��Right=�$��2��.  (ProcessDesigner.UserControls.ComboBoxCus  -   
  Name$
 	ltbRmCode��   DropDownCusWidth$	 500��$��0��$��2��$
��Left=�$  Please enter RM Code��$��1��$��95��   SelectedValuePath$ RM_CODE��   	MaxLength$ 10��   TabIndex$ 1��   
DataSource �� *RawMaterials+   
IsReadOnly �� *RMCodeIsReadonly+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�   SelectedItem �� *SelectedRow+   SelectedValue �� *MandatoryFields.RM_CODE+$ PropertyChanged=�$ TwoWay=�   ButtonVisibility �� *HasDropDownVisibility+   ColumnsHeader �� *DropDownItems+.   (System.Windows.Interactivity.Interaction   Triggers /   )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.   (System.Windows.Interactivity.TriggerBase   Actions. 6   0System.Windows.Interactivity.InvokeCommandAction     Command �� *SelectedItemChangedCommand+  $ 	LostFocus��.    �� *TextBoxValueChanged+   CommandParameter �� *Input+ J� InputBindings �� 
 �� Key$ Enter�� �� Command �� *TextBoxValueChanged+�� -    ��Name$ 	txtRmDesc��$��2��$	��155��$
��Left=�$��1��$!  Please enter RM Description������ *MandatoryFields.RM_DESC+$ PropertyChanged=�$ TwoWay=�-  'ProcessDesigner.UICommon.FocusExtension    	IsFocused  �� *RMDescIsFocused+.��!   	Behaviors! 8	  2ProcessDesigner.UICommon.TabOnEnterTextBoxBehavior	  .���� -   " ��Name$" lblClass��$��2��$��Center=�$
��Left=�$��2������ *CLASS_DESCRIPTION+$ TwoWay=��� $��2��$��1q�$��2,2,2,2q�._�� $��2��$��5,2,5,0q�.+�)
 	 #Microsoft.Windows.Controls.DataGrid
  -	   # 
 Name$# ssRawMaterial��$ 
  CanUserAddRows$ . % 
  RowHeaderWidth$% 20Q�& 
  CanUserDeleteRows& . ' 
  CanUserResizeColumns' . ( 
  CanUserReorderColumns( .  ) 
  CanUserResizeRows) .  * 
  CanUserSortColumns* .  + 
  AutoGenerateColumns+ .  , � ToolTip$ , Enter Raw Material Details��2  ,ProcessDesigner.UICommon.ScrollToTopBehavior-   ScrollToTop- . $��1��$��1������ $ RawMaterialsSize�$ PropertyChanged=�! 6 0ProcessDesigner.UICommon.DataGridBehaviorToolkit     $ CellEditEnding��.    �� *TextBoxMIN_DIA_ValueChanged+ �� . �� ElementName$. ssRawMaterial��$ CurrentCell�/ 
  Columns/ 3 	 -Microsoft.Windows.Controls.DataGridTextColumn  / 	 )Microsoft.Windows.Controls.DataGridColumn0   Header$0 RM Code��1   
Visibility$1 Hidden=�2   Width8 	 2Microsoft.Windows.Controls.DataGridLengthConverter$	2 20* 2  ,ProcessDesigner.UserControls.TextBoxIntsOnly3   Mask$	3 Any=�4 	 .Microsoft.Windows.Controls.DataGridBoundColumn4   Binding4 �� *	RM_CODE+&   BHCustCtrl.DataGridNumericColumn  $0 Minimum Dia��$	2 20* 5   MaxPrecision$5 10��6   MaxScale$6 2��4 �� *
RM_DIA_MIN+$ PropertyChanged=�$ TwoWay=�  $0 Maximum Dia��$	2 20* $5 10��$6 2��4 �� *
RM_DIA_MAX+$ PropertyChanged=�$ TwoWay=�7 	 1Microsoft.Windows.Controls.DataGridTemplateColumn  $0 Location Code��$	2 20* 7   CellTemplate7 �� .R��� ���� *
LOC_CODE+8   CellEditingTemplate8 �� .R�5  /ProcessDesigner.UserControls.ComboBoxWithSearch  9   
IsReadOnly9 . :   SelectedValuePath$: LOC_CODE��;   DisplayValuePath$; LOC_CODE��$��Center=�<   
DataSource< �� = �� RelativeSource= �� *FindAncestor+> �� AncestorType"> >�$+ %DataContext.RawMaterialLocationMaster�?   ColumnsHeader? �� = �� *FindAncestor+"> >�$' !DataContext.DropDownItemsLocation�@   SelectedText@ �� *
LOC_CODE+$ TwoWay=�$ PropertyChanged=�A   SelectedValueA �� *
LOC_CODE+$ TwoWay=�$ PropertyChanged=�B � ContextMenuB �� .@�m� $��Delete��C m� CommandC �� *DeleteClickCommand+D m� CommandParameterD �� = �� *FindAncestor+"> ��$" PlacementTarget.SelectedItem��� $��3��$��1q�$��2,2,2,2q�._�� $��3��$��	10,5,10,2q� �� $��* �	 �� $	��20* ��� $	��11* ��� $��1* ��� $	��20* ��� $	��20* ��� $	��11* ��� $��1* ��� $	��10* �.+��� $��Cost for Domestic : ��$��0��$��Center=�$��Right=�$��0���� $��Per Kg.��$��3��$��Center=�$
��Left=�$��0���� $��Cost for Export : ��$��4��$��Center=�$��Right=�$��0��1  +ProcessDesigner.UserControls.NumericTextBox  -
   E  Name$E txtDomesticCost��$��1��$��0��$   Please enter Domestic Cost��$ 4��$��25��$��90��$
��Left=�F   MaxPrecision$F 10��G   MaxScale$G 2��H �� TextAlignment$
H Left=����� *ActiveEntity.LOC_COST+$ TwoWay=�$ PropertyChanged=�I �� StringFormat$I F2��1  +ProcessDesigner.UserControls.DecimalTextBox  -   J  Name$J txtExportCost��$��5��$��0��$  Please enter Export Cost��$��90��$ 5��$��25��K   AllowNegativeK .  L   MaxDecimalPlaces$L 2��$
H Left=�M �� 	MaxLength$M 13������ *ActiveEntity.EXP_COST+$ TwoWay=�$ PropertyChanged=�$I F2��.��! 	  .���� $��Per Kg.��$��7��$��Center=�$
��Left=�$��0�� �� $ F3�� �� *AddNewClickCommand+�� $ F5�� �� *EditClickCommand+�� $ S��N �� 	Modifiers$N Controlf� �� *SaveClickCommand+�� $ F9�� �� *CloseClickCommand+