   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  ProcessDesigner   ProcessDesigner.frmTfc   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml    	 cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls    	 eEnterKeyPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls    	 ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity    	 osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 -   $	��550��$
��1024��   WindowBackground#��[  .���� ._�� $��2,0,2,0q�  � ColumnDefinitions  �� $	��18* ��� $	��24* ��� $	��13* ��� $	��27* ��� $	��20* � � RowDefinitions �� $��30 ��� $��30 ��� $��30 ��� $��1* ��� $��40 ��� $	��100 �.+��� -   $��lblSFLPartNo��$��SFL Part No : ��$��0��$��0�� �� TextAlignment$ Right=�5   /ProcessDesigner.UserControls.ComboBoxWithSearch  -     Name$ 	cmbPartNo��   	MaxLength$ 40��$	��160��$
��Left=�$��0��$��1��   DisplayValuePath$ PART_NO��$��Center=�   
IsReadOnly .     SelectedValuePath$ PART_NO��$	��0,4q�
 � Tag$' !Please select the SFL Part Number��	   
DataSource	 �� *TFCMODEL.PartNoDetails+
 �� UpdateSourceTrigger$
 PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors .    SelectedItem �� *SelectedRowPart+$
 PropertyChanged=�   SelectedText �� *TFCMODEL.PartNo+$
 PropertyChanged=� �� Mode$ TwoWay=�   SelectedValue �� *TFCMODEL.PartNo+$
 PropertyChanged=�$ TwoWay=�   ColumnsHeader �� *DropDownItemsPart+$
 PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� * SelectChangeComboCommandPartNo+$
 PropertyChanged=� J� InputBindings �� 
 �� Key$ Enter�� �� Command �� * SelectChangeComboCommandPartNo+�� -   $��lblCustomerPartNo��$��Customer Part Number : ��$��0��$��2��$ Right=��� -   $��txtCustomerPartNo�� �� 
IsReadOnly . $��0��$��3������ *TFCMODEL.CUSTOMERPARTNO+$
 PropertyChanged=��� -   $��lblPartName��$��Part Name : ��$��1��$��0��$ Right=��� -   $��txtPartName�� . $��1��$��1������ *TFCMODEL.PartDesc+$
 PropertyChanged=�.���� -   $��lblCustomerName��$��Customer Name : ��$��1��$��2��$ Right=��� -	   $��txtCustomerName�� . $��1��$��3������ *TFCMODEL.CUSTOMERNAME+$
 PropertyChanged=��� $��2��$��0�� �� Orientation$ 
Horizontal=�$��5,0,0,0q�.+��� -
   $��lblIsitAutoPart��$��Is it Auto Part ? ��$
 Left=��� -    ��Name$ chkAutoPartYes��$��Yes     ��$��Center=����� *OnAvailChkYesCommand+ d� 	IsChecked �� *TFCMODEL.AUTOPARTYES+$
 PropertyChanged=��� -   $��chkAutoPartNo��$��No��$��Center=����� *OnAvailChkNoCommand+ �� *TFCMODEL.AUTOPARTNO+$
 PropertyChanged=��� -   $��
lblCFTDate��$��CFT Date : ��$��60��$
��Left=�$��2��$��1���� -   $��
txtCFTDate��$��2��$��1��$	��155��$��35,0,0,0q� . ���� *TFCMODEL.DATE+$
 PropertyChanged=��� -   $��lblApplication��$��Application : ��$��2��$��2��$ Right=��� -   $��txtApplication��$��2��$��3�� . ���� *TFCMODEL.APPLICATION+$
 PropertyChanged=��� $��Customer Programme��$��0��$��4��$��3��$��5,0,5,0q�.��� $��0�� �� $	��30* ��� $	��30* ��� $	��30* �  �� $	��16* �.+��� -   $��chkPrototype��$��0��$��0��$�� Proto-type��$��Center=�$��0,0,0,3q����� *OnAvailChkPtCommand+ �� *TFCMODEL.PROTOTYPE+$
 PropertyChanged=��� -   $��chkPrelaunch��$��1��$��0��$�� Pre-launch��$��Center=�$��0,0,0,3q����� *OnAvailChkPlCommand+ �� *TFCMODEL.PRELAUNCH+$
 PropertyChanged=��� -   $��chkProduction��$��2��$��0��$�� Production��$��Center=�$��0,0,0,3q����� *OnAvailChkPrCommand+ �� *TFCMODEL.PRODUCTION+$
 PropertyChanged=��� $��3��$��4��$	��0,5q�.@��� $��Consideration��.��&   System.Windows.Controls.DataGrid  -   $��dgConsideration��   
IsReadOnly .    RowHeaderWidth$ 20Q�   AutoGenerateColumns .  $��3��$��0��$��4��$��5,0,0,0q����� *TFCMODEL.GRIDLOAD+$
 PropertyChanged=�  �� SelectedItem  �� *SelectedGrid+$
 PropertyChanged=�   $ MouseLeftButtonUp��.    �� *MouseComboCommand+$
 PropertyChanged=�  $ 
LoadingRow��. >  8Microsoft.Expression.Interactivity.Core.CallMethodAction  !   
MethodName$ ! dgConsideration_LoadingRow��"   TargetObject" ��   $ Loaded��.   $! dgConsideration_Loaded��" �� #   Columns# 0  *System.Windows.Controls.DataGridTextColumn  ,	  &System.Windows.Controls.DataGridColumn$ 	  Header$$ Consideration��% 	  Width5
  /System.Windows.Controls.DataGridLengthConverter$% 1*
 1  +System.Windows.Controls.DataGridBoundColumn&   Binding& �� *Consideration+$
 PropertyChanged=�'   ElementStyle' �� ( �� 
TargetType"( ��.��� ) �� Property) � ��* �� Value$* Center=�  $	$ Y/N��$	% 100
 & �� *Yes_No+$
 PropertyChanged=�' �� "( ��.��� ) � ��$* Center=��� ) � ��$* Center=��� $��Risk Analysis��.��   .  $��4��$��0��$��4��$��5,0,0,0q�+   CanUserAddRows+ .  ,   CanUserResizeColumns, . -   CanUserDeleteRows- .  ���� *TFCMODEL.GRIDLOADRISK+$
 PropertyChanged=�  �� *SelectedRowPart+$
 PropertyChanged=�#   $
$ S.No��$% 60
 . 	  
IsReadOnly. . & �� *S_NO+$
 PropertyChanged=�' �� "( ��.��� ) � ��$* Center=��� ) � ��$* Center=�  $$ Aspects��$% 2*
 . . & �� *	Aspects+$
 PropertyChanged=�' �� "( ��.��� ) � ��$* Center=�  $"$ If risk, then Impact of Risk��$% 1*
 & �� *ImpactofRisk+$
 PropertyChanged=�/   EditingElementStyle/ �� "( ��.��� ) �@��	MaxLength$	* 500���� ) � ��$* Center=�  $$ Remarks��$% 1*
 & �� *	Remarks+$
 PropertyChanged=�/ �� "( ��.��� ) �@��	MaxLength$	* 500���� ) � ��$* Center=��� -   $��lblNote��$e��_Note:- If Status is Marked as 'X' then NO will be Ticked. If left Blank then Yes will be Ticked��$��4��$��0��$��4��$��Center=�� $��5��$��0��$��5��  �� $	��10* ��� $	��40* ��� $��1* ��� $	��15* ��� $��1* ��� $	��15* � �� $	��20* ��� $	��20* ��� $	��20* ��� $	��20* �.+��� $��
Conclusion��$��0��$��0��$��4��$��2��$��5,0,0,0q�.��� $��0�� �� $	��30* ��� $	��30* ��� $	��30* �  �� $��5* ��� $	��16* �.+��� -   $��chkFeasibleProduct��$��Feasible��$��Center=�$��0��$��0�� �� *TFCMODEL.FEASIBLEPRODUCT+$
 PropertyChanged=����� *OnAvailChkF1Command+�� -   $��chkFeasibleChange��$��Feasible��$��Center=�$��1��$��0�� �� *TFCMODEL.FEASIBLECHANGE+$
 PropertyChanged=����� *OnAvailChkF2Command+�� -   $��chkNotFeasible��$��Not Feasible��$��Center=�$��2��$��0�� �� *TFCMODEL.NOTFEASIBLE+$
 PropertyChanged=����� *OnAvailChkNfCommand+�� -   $��lblFeasibleProduct��$=��7Product can be produced as specified with no revisions.��$��0��$��1���� -   $��lblFeasibleChange��$'��!Change recommended(see attached).��$��1��$��1���� -   $��lblNotFeasible��$T��NDesign revision required to produce product within the specified requirements.��$��2��$��1���� -   $��lblProcessNo��$��Please Enter Process no. ��$��2��$��3��$��2���� -   $��txtProcessNo��$! Please enter the Process no��2   ,ProcessDesigner.UserControls.TextBoxIntsOnly0   Mask$0 Integer=�$��2��$��3��$��2��$��Right=�$��40��1 �� 	MaxLength$1 2������ *TFCMODEL.ROUTENO+$
 PropertyChanged=��� -   $��btnSave��$��_Save��$��0��$��5��$��2��$��2,5,10,5q����� *SaveClickCommand+�� -   $��btnPrint��$��_Print��$��2��$��5��$��2��$��2,5,10,5q����� *PrintClickCommand+