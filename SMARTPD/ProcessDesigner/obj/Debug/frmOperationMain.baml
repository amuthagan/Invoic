   M S B A M L   `   `   `  ����E  ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl  5   	   W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5   	   h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core 5   	   J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null?$clr-namespace:ProcessDesigner.CommonProcessDesigner.Common 5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5	   	   C&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon 5
   	   &    ProcessDesigner.frmOperationMain   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U	 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     	 R
 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml    
 5   	   C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl    
 5   	   cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls    
 5   	   osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    
 5   	   9Enterkey$clr-namespace:ProcessDesigner.Common    
 5   	   ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 	 5   	   XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 	 5	   	   2local&clr-namespace:ProcessDesigner.UICommon 	 5
   	   -   5        $	��160��5   	   $	��360��6   .  (System.Windows.Interactivity.Interaction    Triggers  5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *LoadCommand+5       � 	Resources 5      /  )ProcessDesigner.UICommon.BooleanConverter   BooleanConverter��%   &	          5   
   6!   5      ��5      �� 5   
    �� ResourceKey$ WindowBackground��6   .��5      �� ._�5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      � $��0��6   .+�5       c� .@�5!      �� -   5         ��Name$ Btn_Add��5!   d   $��Add F3��6   $��0��60   $��0��6=   $��40��6M   $��55��6Y    � ToolTip$ Add New Record��6�   	 J� RenderTransformOrigin$	 	0.5,0.583&�6�   0  *ProcessDesigner.UserControls.AddedProperty
   Image$
 Images/Add.png��6  ��6u   �� *AddClickCommand+{�6�   �� *	AddEnable+ �� UpdateSourceTrigger$ PropertyChanged=�6  �� 5"      -   5        $ Btn_Edit��5"   S   $��Edit/View F5��6   $��0��66   $��1��6C   $��40��6e   $ Edit or View Record��6�   $��80��6�   $
 Images/Edit.png��6  ��6q   �� *EditClickCommand+{�6�   �� *
EditEnable+$ PropertyChanged=�6  �� 5#      -   5        $ Btn_Save��5#   f   $��Save Ctrl+S��6   $��0��63   $ Save Record��6@   $��3��6V   $��40��6x   $��70��6�   $
 Images/Save.png��6  ��6�   �� *UpdateOperMastCommand+{�6�   �� *
SaveEnable+$ PropertyChanged=�6  �� 5$      -   5        $ 	Btn_Close��5$      $��Close F9��61   $ 
Close Form��6D   $��0��6Y   $��4��6f   $��40��6w   $��60��6�   $
 Images/Close.png��6�   ��6�   �� *OnCloseCommand+6�   �� 5%      -   5        $ 
Btn_Delete��5%   �   $��	Delete F6��6   $y�	Collapsed=�61   $��0��6I   $ Delete a Record��6V   $��2��6p   $��40��6�   $��60��6�   $
 Images/Delete.png��6  {�6�   �� *DeleteEnable+$ PropertyChanged=���6�   �� *DeleteClickCommand+6  5&      5'      �� 5(      $��2q�6   $��1��6)   $��5q�66   ._�5)      � -   5         �Name$ GrdOperation��5)      -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=�6.    5*      �� 5+      $��* �6(   �� 5,      $��* �6(   �� 5-      $��* �6(   �� 5.      $��* �6(    � ColumnDefinitions 50      �� 51      $	��120 �6+   �� 52      $	��120 �6+   �� 53      $��* �6+   .+�55      �� -   5         ��Name$ lbl_Oper_Code��55       $��0��67   $��Center=�6G   $��Right=�6   $��0,0,10,0q�6�   $��0��6�   ��6c   �� *	LabelCode+6�   �� 56      -	   5        $ lbl_Oper_Desc��56       $��0��67   $��Center=�6G   $��Right=�6}   $��0,0,10,0q�6�   $��1��6�   ��6b   �� *	LabelDesc+6�   .  (ProcessDesigner.UserControls.ComboBoxCus  57      -
   5          Name$ cmbOperMaster��57   r  $��0,0,25,0q�6)   $��90��6;   $
��Left=�6F   $��2��6a      IsStatusBoxVisible$ Visible=�6u   $��Bottom=�6�      FirstAllowZero .     TabIndex$ 0��6  $��1��6T  $��0��6d     Mask$ UnSignedInt=�6�     SelectedValuePath$ 	OPER_CODE��58   s      
IsReadOnly 57   �   �� *TxtReadOnly+   	MaxLength 6�   �� *ComboBoxMaxLength+   ColumnsHeader 62  �� *	Columns+   ButtonVisibility 6�  �� *ButtonEnable+   
DataSource 58      �� *
DtDataview+$ PropertyChanged=�   SelectedItem 6M   �� *SelectedRow+   SelectedValue 6�   �� *
OperCode+$ PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors . 
 � Tag 59      �� *LabelCodeStatusMsg+  5:        5;      $ SelectedItemChanged��6-   . 5<   "      68   �� *SelectChangeComboCommand+5=      5@      �� 5A      -   5          ��Name$  txt_operDesc��5A   �   $��1��6   $	��Top=�6.   $��2��6F   $ 1��6�   $��1��5B      $��0,2,25,0q�6+   -	  'ProcessDesigner.UICommon.FocusExtension! 	  	IsFocused! 5A   [   �� *
FocusCombo+" �� 
IsReadOnly" 6�   �� *TxtDecReadOnly+��6�   �� *
OperDesc+$ PropertyChanged=� .  . # �� 	MaxLength# 6j  �� *DescTextboxMaxLength+ 5B   =   �� *LabelDescStatusMsg+�� 5C      $��Show in Costing��6   $ 2��6^   $��1��6   $��2��6�   {�6:   �� *CheckReadOnly+y�6�   �� *CostVisible+$ d� 	IsChecked$ 6�   �� *ShowInCaset+% �� 	Converter#% [  ]� 5D      $��1��6    $��2��60   $��3��6D   $��Center=�6Q   $��0,3,0,0q�6l   $	��140��6}   $
��Left=�6�   y�6�   �� *IsVisibilityDelete+$ PropertyChanged=�& �� Mode$& TwoWay=�{�6�   �� *IsDeleteEnable+$ PropertyChanged=�$& TwoWay=�.+�5E      	� $��Active��6u   $��60��6�   $ 6&   �� *
IsActive+$ PropertyChanged=�$& TwoWay=�6�   	� 5F      $��	In-Active��6w   $��70��6�   $ 6&   �� *
IsInActive+$ PropertyChanged=�$& TwoWay=�6�   5G      5H      5I      5J      5K      ' J� InputBindings' 5L      �� 5M   
   
( �� Key$( F3��6   ) �� Command) 6   �� *AddClickCommand+�� 5N   
   $( F5��6   ) 6   �� *EditClickCommand+�� 5O   
   $( F6��6   ) 6   �� *DeleteClickCommand+�� 5P   
   $( S��6   * �� 	Modifiers$* Controlf�6   ) 61   �� *UpdateOperMastCommand+�� 5Q   
   $( F9��6   ) 6   �� *OnCloseCommand+5U      