   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls   ProcessDesigner   ProcessDesigner.frmTfcECN   [LookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls  ViPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml    	 osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006    	 @d2http://schemas.microsoft.com/expression/blend/2008    	 eEnterKeyPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls    	 -   $	��480��$	��750��   WindowBackground#��[  .��� .+��� ._�� $��2,0,2,0q�  � ColumnDefinitions  �� $��9* ��� $	��20* ��� $	��11* ��� $	��20* ��� $��1* � � RowDefinitions �� $��28 ��� $��28 ��� $��28 ��� $��28 ��� $��* ��� $��30 ��� $	��100 �.+��� -   $��lblSFLPartNo��$��SFL Part Number : ��$��0��$��0�� �� TextAlignment$ Right=�5   /ProcessDesigner.UserControls.ComboBoxWithSearch  -     Name$ 	cmbPartNo��
 � Tag$* $Please enter/select SFL Part Number ��   	MaxLength$ 40��$	��160��$
��Left=�$��0��$��1��   DisplayValuePath$ PART_NO��$��Center=�   
IsReadOnly .     SelectedValuePath$ PART_NO��	   
DataSource	 �� *TFCMODEL.PartNoDetails+
 �� UpdateSourceTrigger$
 PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors .    SelectedItem �� *SelectedRowPart+$
 PropertyChanged=�   SelectedText �� *TFCMODEL.PartNo+$
 PropertyChanged=� �� Mode$ TwoWay=�   SelectedValue �� *TFCMODEL.PartNo+$
 PropertyChanged=�$ TwoWay=�   ColumnsHeader �� *DropDownItemsPart+$
 PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� * SelectChangeComboCommandPartNo+$
 PropertyChanged=� J� InputBindings �� 
 �� Key$ Enter�� �� Command �� * SelectChangeComboCommandPartNo+�� -   $��lblCustomerName��$��Customer Name : ��$��0��$��2��$ Right=��� -   $��txtCustomerName��$  �� �� 
IsReadOnly . $��0��$��3������ *TFCMODEL.CUSTOMERNAME+$
 PropertyChanged=��� -   $��lblPartName��$��Description : ��$��1��$��0��$ Right=��� -   $��txtPartName��$  �� . $��1��$��1������ *TFCMODEL.PartDesc+$
 PropertyChanged=�.���� -   $��lblCustomerPartNo��$��Customer Part No. : ��$��1��$��2��$ Right=��� -	   $��txtCustomerPartNo��$  �� . $��1��$��3������ *TFCMODEL.CUSTOMERPARTNO+$
 PropertyChanged=��� -
   $��lbldrgissueNo��$��Drg Issue no. : ��$��2��$��0��$ Right=��� -   $��txtdrgIssueNo��$  ��$��2��$��1�� . ���� *TFCMODEL.PRD_ISSUE_NO+$
 PropertyChanged=��� -   $��lblCustDrgrevNo��$��Customer drg.rev.no. : ��$��2��$��2��$ Right=��� -   $��txtCustDrgrevNo��$  ��$��2��$��3�� . ���� *TFCMODEL.CUST_DWG_ISSUE_NO+$
 PropertyChanged=��� -   $��
lbldrgDate��$��Drg Date : ��$��3��$��0��$ Right=��� -   $��
txtDrgDate��$  ��$��3��$��1�� . ���� *TFCMODEL.PRD_ISSUE_DATE+$
 PropertyChanged=��� -   $��lblCustDrgDate��$��Customer drg.date : ��$��3��$��2��$ Right=��� -   $��txtCustDrgDate��$  ��$��3��$��3�� . ���� *TFCMODEL.CUST_STD_DATE+$
 PropertyChanged=��� $��4��$��4��.@��� $��Consideration��.��&   System.Windows.Controls.DataGrid     
IsReadOnly .    AutoGenerateColumns .     RowHeaderWidth$ 20Q�$��4��$��0��$��4��$��5,0,0,0q����� *TFCMODEL.GRIDLOAD+$
 PropertyChanged=� �� SelectedItem �� *SelectedGrid+$
 PropertyChanged=�   $ MouseLeftButtonUp��.    �� *MouseComboCommand+$
 PropertyChanged=�  $ 
LoadingRow��. >  8Microsoft.Expression.Interactivity.Core.CallMethodAction     
MethodName$  dgConsideration_LoadingRow��   TargetObject ��   $ Loaded��.   $ dgConsideration_Loaded�� ��     Columns  0  *System.Windows.Controls.DataGridTextColumn  ,	  &System.Windows.Controls.DataGridColumn! 	  Header$! Consideration��" 	  Width5
  /System.Windows.Controls.DataGridLengthConverter$" 1*
 1  +System.Windows.Controls.DataGridBoundColumn#   Binding# �� *Consideration+$
 PropertyChanged=�$   ElementStyle$ �� % �� 
TargetType"% ��.��� & �� Property& � ��' �� Value$' Center=�  $	! Y/N��$" 70
 # �� *Yes_No+$
 PropertyChanged=�$ �� "% ��.��� & � ��$' Center=��� & � ��$' Center=��� $��Risk Analysis��.��   .  $��4��$��0��$��4��$��5,0,0,0q�(   CanUserAddRows( .  )   CanUserResizeColumns) . *   CanUserDeleteRows* .  ���� *TFCMODEL.GRIDLOADRISK+$
 PropertyChanged=� �� *SelectedRowPart+$
 PropertyChanged=�    + 	  
IsReadOnly+ . $
! S.No��$" 60
 # �� *S_NO+$
 PropertyChanged=�$ �� "% ��.��� & � ��$' Center=��� & � ��$' Center=�  + . $! Aspects��$" 2*
 # �� *	Aspects+$
 PropertyChanged=�$ �� "% ��.��� & � ��$' Center=�  $"! If risk, then Impact of Risk��$" 1*
 # �� *ImpactofRisk+$
 PropertyChanged=�,   EditingElementStyle, �� "% ��.��� & �@��	MaxLength$	' 500���� & � ��$' Center=�  $! Remarks��$" 1*
 # �� *	Remarks+$
 PropertyChanged=�, �� "% ��.��� & �@��	MaxLength$	' 500���� & � ��$' Center=��� -   $��lblNote��$e��_Note:- If Status is Marked as 'X' then NO will be Ticked. If left Blank then Yes will be Ticked��$��5��$��0��$��4��$��Center=�� $��7��$��0��$��5��  �� $��6* ��� $��* ��� $	��180 ��� $��4* ��� $��4* ��� $��1* � �� $	��20* ��� $	��20* ��� $	��20* ��� $	��20* �.+��� $��
Conclusion��$��0��$��0��$��4��$��5,0,30,5q�.��� $��0�� �� $	��30* ��� $	��30* �  �� $��5* �.+��� -   $��chkFeasibleProduct��$��Feasible��$��Center=�$��0��$��0��- d� 	IsChecked- �� *TFCMODEL.FEASIBLEPRODUCT+$
 PropertyChanged=����� *OnAvailChkFeasibleCommand+�� -   $��chkNotFeasible��$��Not Feasible��$��Center=�$��1��$��0��- �� *TFCMODEL.NOTFEASIBLE+$
 PropertyChanged=����� *OnAvailChkNotFeasibleCommand+�� -   $��lblProcessNo��$��Please Enter Process no. ��$��2��$��2��$��2��$��Center=��� -   $��txtProcessNo��$ Please enter Process no��2   ,ProcessDesigner.UserControls.TextBoxIntsOnly.   Mask$. Integer=�$��2��$��2��$��2��$��40��$��Right=����� *TFCMODEL.ROUTENO+$
 PropertyChanged=��� -   $��btnSave��$��_Save��0   *ProcessDesigner.UserControls.AddedProperty/   Image$/ Images/Save.png��$��45��$��1��$��3��$��3��$	��100��$��20,0,1,0q����� *SaveClickCommand+�� -   $��btnPrint��$��_Print��$/ Images/Print.png��$��45��$��1��$��4��$��3��$	��100��$��1,0,20,0q����� *PrintClickCommand+