   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl !   ProcessDesigner.frmDrawings   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     Swfirms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions     8busy-http://schemas.microsoft.com/wpf/2008/toolkit 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 =
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl 
 ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 -   $	��500��$
��1000��   WindowBackground#��[    � 	Resources  0  *ProcessDesigner.UICommon.DateTimeConverter  DateTimeConverter/  )ProcessDesigner.UICommon.BooleanConverter  BooleanConverter0  *ProcessDesigner.UICommon.CurrencyConverter  CurrencyConverter/  )ProcessDesigner.UICommon.DecimalConverter  DecimalConverter��%<   &	       &	      &	 
     &	              .���� $��0q�-  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=�._��  � ColumnDefinitions �� $	��50* ��� $	��50* � � RowDefinitions �� $	��12* ��� $��8* ��� $��8* ��� $	��75* �.+��� $��0��$��0��$��2��._��  �� $	��50* � �� $��* �.+�`� .�c� $��0��$��0��$��58��.@��� $��Edit/View F5��$��50��$��2,0,2,0q�$��	10,0,10,0q�   	IsTabStop .  0   *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Edit.png��$y�	Collapsed=����� *OnEditCommand+{��� *
EditEnable+ �� UpdateSourceTrigger$ PropertyChanged=��� $��Save Ctrl+S��$��50��$��2,0,2,0q�$��	10,0,10,0q� .  $ Images/Save.png�� � ToolTip$ Save Record������ *OnSaveCommand+{��� *
SaveEnable+$ PropertyChanged=��� $��Search��$��50��$��2,0,2,0q�$��	10,0,10,0q� .  $ Images/Search.png������ *OnSearchCommand+o�  c� OverflowMode$ Always=�.@�]� .+�m� $��Cost Sheet Search��	 m� Command	 �� *CostSheetSearchClickCommand+m� $��Product Search��	 �� *ProductSearchClickCommand+m� $��Tools Search��	 �� *ToolSearchClickCommand+c� $��0��$��0��$��58��.@��� $��_Copy��$��50��$��2,0,2,0q�$��	10,0,10,0q� .  $ Images/Copy.png��$
 Copy������ *OnCopyCommand+�� $��	Close F9 ��$��50��$��2,0,2,0q�$��	10,0,10,0q� .  $ Images/Close.png��$ Close Current Active Form������ *OnCloseCommand+�� $	��ECN��$��30��$��50��$��1��$��2,0,2,0q�$��	10,0,10,0q�$y�	Collapsed=��� $	��MPS��$��30��$��50��$��2��$��2,0,2,0q�$��	10,0,10,0q�$y�	Collapsed=��� $��0��$��1��$��2,1,2,1q�
   TabIndex$
 0��$��1q�$��2��._�� $��0��$��1��$��5,5,5,5q�$
 0�� �� $	��50* � �� $��8* ��� $	��19* ��� $	��13* ��� $	��40* ��� $	��30* �.+��� $��
Part No : �� �� TextAlignment$ Right=�$��0��$��0��$
��Bold"�.   (ProcessDesigner.UserControls.ComboBoxCus  -   $��	cmbPartNo��$
��Bold"�   	MaxLength$ 11��   
IsReadOnly .     
IsEditable . 
 � Tag$ Please select Part No.��$	��160��$
��Left=�$��1��$��0��$	��2,0q�$ Select Part Number.��   SelectedValuePath$ PART_NO��-  'ProcessDesigner.UICommon.FocusExtension   	IsFocused �� *PartNumberIsFocused+$ PropertyChanged=� �� Mode$ TwoWay=�   ColumnsHeader �� *PartColumns+   ButtonVisibility �� *HasDropDownVisibility+$ PropertyChanged=�   
DataSource �� *DV_PART_MAST+$ TwoWay=�$ PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors .    SelectedItem �� *SelectedRow+   SelectedValue �� *DrwModel.PART_NO+$ PropertyChanged=�.	  (System.Windows.Interactivity.Interaction 	  Triggers /
  )System.Windows.Interactivity.EventTrigger
   
  	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� * OnselectChangeComboPartCommand+ J� InputBindings �� 
 �� Key$ Enter��  �� Command  �� *EnterPartNumberCmb+�� $�� Description : ��$ Right=�$	��120��$��2��$��0��$
��Bold"��� $��5,0,0,0q�$
 Left=�$��3��$��0��$
��Bold"����� *PART_DESCRIPTION+$ TwoWay=�$ PropertyChanged=��� $��
Page No : ��$ Right=�$��2��$��0��$y�	Collapsed=��� $��0��$��0��$��40��$��Center=�$y�	Collapsed=����� *	PAGE_NO+$ TwoWay=�$ PropertyChanged=��� $��0��$��0��$��25��$��30��$��Center=� .  $ Images/UpArrow.png��$y�	Collapsed=����� *OnPreviousCommand+�� $��0��$��0��$��25��$��30��$��Center=� .  $ Images/DownArrow.png��$y�	Collapsed=����� *OnNextCommand+�� $��Total Pages : ��$ Right=�$��0��$��0��$y�	Collapsed=��� $�� ��$
 Left=�$��0��$��0��$y�	Collapsed=��� $
 Left=�$��7��$��7��$��5��$y�	Collapsed=����� *TOTAL_PAGE_NO+$ TwoWay=�$ PropertyChanged=��� $��0��$��2��$��2,1,2,1q�$��1q�$
 1��$��2��._�� $��0��$��2��$��0,0,0,0q� �� $
��100* � �� $
��7.2* ��� $	��20* ��� $	��30* ��� $	��30* �! � ContextMenu! �� $��3��.@�m� $��Insert��	 �� *OnInsertImageCommand+" m� CommandParameter" �� # �� RelativeSource# �� *FindAncestor+$ �� AncestorType"$ ��% �� Path$"% PlacementTarget.SelectedItem�m� $
��Edit��	 �� *OnEditImageCommand+" �� # �� *FindAncestor+"$ ��$"% PlacementTarget.SelectedItem�m� $��Delete��	 �� *OnDeleteImageCommand+" �� # �� *FindAncestor+"$ ��$"% PlacementTarget.SelectedItem�.+��� $��Drwg Type : ��$ Right=�$��0��$��0��$
��Bold"�  -   $��
cmbDrwType��$! Please select Dwg Issue No.��$	��128��$
��Left=�$��1��$��0��$	��2,0q�$ Select Drawing Type��$ DWG_TYPE_DESC�� .  �� *ComboBoxMaxLength+ �� *
DwgColumns+ �� *ButtonEnable+ �� *DV_DWG_TYPE_MAST+$ TwoWay=�$ PropertyChanged=� .  .  �� *SelectedRow+ �� *DrwModel.DWG_TYPE_DESC+$ PropertyChanged=� .  .  
  $ SelectedItemChanged��.    �� *#!OnselectChangeComboDrawingCommand+�� -   & ��Name$& btnECN��$
��_ECN��$��0��$��2��$	��100��$��5,5,5,5q�$��Right=����� *ShowECNCommand+�� -   $& btnPCN��$
��_PCN��$��0��$��3��$	��100��$��5,5,5,5q�$
��Left=����� *ShowPCNCommand+�� $��0��$��3��$��2,1,2,1q�$��1q�$
 2��$��2��._�� $��0��$��3��$��5,5,5,5q� �� $��5* ��� $	��95* �.+��� $��0��$��1��$��	Revisions��.��&   System.Windows.Controls.DataGrid  -   $��dgvProdDwgMast��'   RowHeaderWidth$' 25Q�(   AutoGenerateColumns( .  )   CanUserResizeRows) .  *   CanUserAddRows* .  +   CanUserDeleteRows+ .  2  ,ProcessDesigner.UICommon.ScrollToTopBehavior,   ScrollToTop, . -   CanUserSortColumns- . ���� *DV_PROD_DWG_ISSUE+ �� $ Delete��  �� *OnDeleteIssueRevisionsCommand+. �� CommandParameter. �� / �� ElementName$/ dgvProdDwgMast��$% SelectedItem�0 	  	Behaviors0 / )ProcessDesigner.UICommon.DataGridBehavior   
  $ CurrentCellChanged��.    �� *RowEditIssueRevisionsCommand+1   CommandParameter1 �� $/ dgvProdDwgMast��$% SelectedItem�
  $ CellEditEnding��. >  8Microsoft.Expression.Interactivity.Core.CallMethodAction  2   
MethodName$#2 dgvProdDwgMast_CellEditEnding��3   TargetObject3 �� 
  $ Sorting��.   $2 dgrdProdDwgMast_Sorting��3 �� 4   Columns4 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn5   Header$5 Location��6   CellTemplate6 �� .R��� ���� *
LOC_CODE+7   CellEditingTemplate7 �� .R�5   /ProcessDesigner.UserControls.ComboBoxWithSearch  8   
IsReadOnly8 . 9   SelectedValuePath$9 LOC_CODE��:   DisplayValuePath$: LOC_CODE��$��Center=�;   
DataSource; �� # �� *FindAncestor+"$ >�$ % DataContext.LocationMaster�<   ColumnsHeader< �� # �� *FindAncestor+"$ >�$'% !DataContext.DropDownItemsLocation�=   SelectedText= �� *
LOC_CODE+$ TwoWay=�$ PropertyChanged=�>   SelectedValue> �� *
LOC_CODE+$ TwoWay=�$ PropertyChanged=�0  *System.Windows.Controls.DataGridTextColumn  ?   Width5  /System.Windows.Controls.DataGridLengthConverter$? 80 $5 Issue No��1  +System.Windows.Controls.DataGridBoundColumn@   Binding@ �� *
ISSUE_NO+$ PropertyChanged=�$ TwoWay=�A   EditingElementStyleA �� B �� 
TargetType"B ��.��� C �� PropertyC �@��	MaxLengthD �� Value$D 3���� 2   ,ProcessDesigner.UserControls.TextBoxIntsOnlyC �@ Mask$D UnSignedDec=�  $
5 Date��$? 90 6 �� .R��� ���� *
ISSUE_DATE+$ OneWay=�E �� 	Converter#E [ 7 �� .R�0   *ProcessDesigner.UserControls.DatePickerCus  $��txtIssueDate��$ Select Date��F   SelectedDateF �� *
ISSUE_DATE+$ PropertyChanged=�  $5 Alterations��$	? 40* @ �� *ISSUE_ALTER+$ PropertyChanged=�G   
IsReadOnlyG �� # �� *FindAncestor+"$  $% IsReadOnlyPrdCode�H   ElementStyleH �� "B ��.��� C � ��$D Center=�A �� "B ��.��� C � ��$D Center=��� C �@��	MaxLength$
D 1000��  $
5 Intl��$	? 120 @ �� *COMPILED_BY+$ PropertyChanged=�G �� # �� *FindAncestor+"$  $% IsReadOnlyPrdCode�H �� "B ��.��� C � ��$D Center=�A �� "B ��.��� C � ��$D Center=��� C �@��	MaxLength$D 10�� �� $ S��I �� 	Modifiers$I Controlf�  �� *OnSaveCommand+�� $ F9��  �� *OnCloseCommand+