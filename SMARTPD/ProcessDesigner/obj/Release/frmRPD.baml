   M S B A M L   `   `   `  ���� )  %BHCustrowEditEndingSubTypeCommandCtrlVGclr-namespace:BHCustCtrl;assembly=BHCustrowEditEndingSubTypeCommandCtrl
BHCustCtrl  h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null?$clr-namespace:ProcessDesigner.CommonProcessDesigner.Common =#clr-namespace:ProcessDesigner.ModelProcessDesigner.Model C&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity    ProcessDesigner.frmRPD   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^
bhCustCtrlGclr-namespace:BHCustCtrl;assembly=BHCustrowEditEndingSubTypeCommandCtrl     osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions     6sys&clr-namespace:System;assembly=mscorlib     cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     9Enterkey$clr-namespace:ProcessDesigner.Common     2vm#clr-namespace:ProcessDesigner.Model     ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 2local&clr-namespace:ProcessDesigner.UICommon 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 -   $	��610��$	��980��   WindowBackground#��[    � 	Resources  ��%     J� InputBindings ��  �� 	Modifiers$ Controlf�
 �� Key$ P�� �� Command �� *PrintCommand+�� $ Controlf�$ S�� �� *UpdateCommand+.���� $��0q�$��0,0,0,0q�._�� -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=� � ColumnDefinitions �� $
��132* ��� $	��65* � � RowDefinitions �� $	��186 ��� $	��166 ��� $��* �.+�� $��0��$��0��$��2�� �� $��30 ��� $��1* � �� $	��10* ��� $	��10* ��� $	��10* ��� $	��11* ��� $	��12* �.+��� $��CI Ref No : �� �� TextAlignment$ Right=�$��0��$��5,5,5,5q��� $��SFL Part No :��$ Right=�$��2��$��5,5,5,5q�.  (ProcessDesigner.UserControls.ComboBoxCus  -   	  Name$	 cmbCIRefference��

 � Tag$
 Please select CI Ref No��   
IsReadOnly . $	��110��$
��Left=�   TabIndex$ 0��$��0��$��1��   SelectedValuePath$ CI_REFERENCE��   	MaxLength �� *MaxLen+   ColumnsHeader �� *	Columns+   
DataSource �� *DtCIReference+ �� UpdateSourceTrigger$ PropertyChanged=�   SelectedItem �� *SelectedRow+   SelectedValue �� *Model_Rpd.CI_REFERENCE+$ PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors . .  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *SelectChangeCIRNo+ �� $ Enter�� �� *LostFocusCirNo+  -   $	 	cmbPartNo��$
 Please select SFL Part No��|�. $	��160��$
��Left=� . $ 1��$��0��$��3��$ PART_NO�� �� *PartColumns+ �� *
DtPartNo+$ PropertyChanged=� �� *SelectedRowPart+ �� *Model_Rpd.PART_NO+$ PropertyChanged=� .  .    $ SelectedItemChanged��.    �� *SelectChangePartNo+�� $��I.Part Details ��$��1��$��0��$��4��$��2��$��5,5,0,5q�.���  �� $	��23* ��� $	��27* ��� $	��26* ��� $	��23* � �� $
��1.5* ��� $
��1.6* ��� $��1* ��� $
��1.4* ��� $��* ��� $
��1.6* ��� $
��1.4* ��� $��60 �.+��� $��Cust Part No  : ��$ Right=�$��0��$��0��$��5,5,0,5.443q�$
��Left=�$��98���� $��Customer Drg    
No  : ��$ Right=�$��0��$��5,23,0,3.311q�$��2��$
��Left=�$��98���� $��Issue Date  : ��$ Right=�$��0��$��2��$��5q��� $��Similar to : ��$ Right=�$��0��$��3��$��5q�0  *ProcessDesigner.UserControls.DatePickerCus  $
 Please select Issue Date��$��1��$��2��$��2,2,15,2q�   SelectedDate �� *Model_Rpd.CUST_STD_DATE+ �� Mode$ TwoWay=�$ PropertyChanged=��� $��Part Desc : ��$ Right=�$��Right=�$��2��$��0��$��5,5,5,5q��� $��Customer : ��$ Right=�$��Right=�$��2��$��1��$��5,5,5,5q��� $��Issue No : ��$ Right=�$��Right=�$��2��$��2��$��5,5,5,5q��� $��
Remarks : ��$ Right=�$��Right=�$��2��$��3��$��5,5,5,5q��� $
 Please enter Part Desc��$	��310��$
��Left=�$��3�� �� 	MaxLength$ 50��$��0��$��6��$��2,2,15,2q�-  'ProcessDesigner.UICommon.FocusExtension   	IsFocused �� *Focus_PROD_DESC+���� *Model_Rpd.PROD_DESC+$ TwoWay=�$ PropertyChanged=�  -   $	 cmbCustomer��$
 Please select Customer ��$��2.046,0,15.113,4.311q� . $��5��$ 0��$��1��$��3��$	��394��$
��left=�$ 	CUST_NAME��$��Bottom=� �� *Focus_CUST_NAME+ �� *PartCustomerColumns+ �� *
DtCustomer+$ PropertyChanged=� �� *SelectedRowCustomer+ �� *Model_Rpd.CUST_NAME+$ PropertyChanged=� .  .    $ SelectedItemChanged��.    �� *SelectCustomerCode+�� $
 Please enter issue No��$��3��$��2��$ 15��$��2,2,15,2q� �� *Focus_CUST_DWG_NO_ISSUE+���� *Model_Rpd.CUST_DWG_NO_ISSUE+$ TwoWay=�$ PropertyChanged=�  $
 Please select ATP Date��$��5��$��2��$��2,2,15,2q� �� *Model_Rpd.ATP_DATE+$ TwoWay=�$ PropertyChanged=��� $��ATP Date : ��$ Right=�$��4��$��2��$��5,5,5,5q��� $��10,0,0,0q�$��Type : ��$
 Left=�$
��Left=�$��6��$��2���� -   $��
CbDomestic��$
 Please select Type ��$��2��$��78��$��19,2,0,2q�$��2��$��6�� �� *Focus_EXPORT+ �� Text �� *Model_Rpd.EXPORT+$ TwoWay=�.@��� $��Domestic���� $��Export���� $��Retail���� $	��Wef���� $
 Please enter Remarks��$��3��$��3��$ 60��$��6��$	��340��$
��Left=�$��2,2,15,2q� �� *Focus_GENERAL_REMARKS+���� *Model_Rpd.GENERAL_REMARKS+$ TwoWay=�$ PropertyChanged=��� $"
 Please enter Customer Drg No��$��1��$ 50��$��1��$��2,2,15,2q� �� *Focus_CUST_STD_NO+���� *Model_Rpd.CUST_STD_NO+$ TwoWay=�$ PropertyChanged=��� $#
 Please enter Similar to Value��$��1��$ 9��$��3��$��2,2,15,2q�$��70��$
��Left=� �� *Focus_SIMILAR_PART_NO+���� *Model_Rpd.SIMILAR_PART_NO+$ TwoWay=��� $ 
 Please enter Cust Part No ��$��1��$��0��$ 50��$��2,2,15,2q� �� *Focus_CUST_DWG_NO+���� *Model_Rpd.CUST_DWG_NO+$ TwoWay=�$ PropertyChanged=�� $��5��$��0��$��3��$��0,0,10,0q� �� $	��30* ��� $	��70* � �� $	��60* ��� $	��40* �.+��� $��II.Volumes ��$��1��$��2��$��0��$��5,6,5,5q�.���  �� $	��40* ��� $	��60* � �� $	��67* ��� $	��40* �.+��� $��Annual Requirement : ��$ Right=�$��0��$��0��$��0,5,2,5q��� $#��Likely plan quantity/ month :��$��WrapWithOverflow=�$ Right=�$��0��$��1��$��5,5,5,5q��� $%
 Please enter Annual Requirement��$��60��$
��Left=�$��1��$��0��$ 8��2	  ,ProcessDesigner.UserControls.TextBoxIntsOnly 	  Mask$ UnSignedInt=�$��2,2,2,2q� �� *Focus_POTENTIAL+���� *Model_Rpd.POTENTIAL+$ TwoWay=�$ PropertyChanged=��� $-
 'Please enter Likely plan quantity/month��$��60��$
��Left=�$��1��$��1��$ 8��$ UnSignedInt=�$��2,2,2,2q� �� *Focus_MONTHLY+���� *Model_Rpd.MONTHLY+$ TwoWay=�$ PropertyChanged=��� $
��Save��    	IsTabStop  .  0
  *ProcessDesigner.UserControls.AddedProperty! 
  Image$! Images/Save.png��$��50��$��80��$��0��$��2,0,2,0q����� *UpdateCommand+{��� *
SaveEnable+$ PropertyChanged=��� $��Print��  .  $��50��$��80��$��1��$��2,0,2,0q�$��0,0,0,0q�$! Images/Print.png������ *PrintCommand+{��� *PrintEnable+$ PropertyChanged=�� $��0��$��1��$��2�� �� $	��45* ��� $	��55* �.+��� $��III.Product Use Details ��$��1��$��0��$��5,5,5,5q�.��� $��0��$��1�� �� $��27 ��� $��* � �� $	��35* ��� $	��30* ��� $	��60* �.+��� $��Application of Part : ��$
 left=�$��0��$��5,0,0,0q��� $&
  Please enter Application of Part��$	��280��$
��Left=�$��1��$��0��$ 50��$��2,2,2,2q�$��2�� �� *Focus_APPLICATION+���� *Model_Rpd.APPLICATION+$ TwoWay=�$ PropertyChanged=��� $��
Auto Part?��$��1��$��0��$��5,5,5,2q�.��� $��0��$��0�� �� $��* ��� $��* �.+�	� $	��Yes��$��0��$��0��$��Bottom=�$��5q�" d� 	IsChecked" �� *Model_Rpd.AutoPart_Yes+$ TwoWay=�$ PropertyChanged=�	� $��No��$��0��$��1��$	��Top=�$��5q�" �� *Model_Rpd.AutoPart_No+$ TwoWay=�$ PropertyChanged=��� $��Safety Part?��$��1��$��1��$��5,5,5,2q�.��� $��0��$��0�� �� $��* ��� $��* �.+�	� $	��Yes��$��0��$��0��$��Bottom=�$��5q�" �� *Model_Rpd.Safety_Yes+$ TwoWay=�$ PropertyChanged=�	� $��No��$��0��$��1��$	��Top=�$��5q�" �� *Model_Rpd.Safety_No+$ TwoWay=�$ PropertyChanged=��� $��Status at Customer end��$��1��$��2��$��5,5,15,2q�.��� $��0��$��0�� �� $��* ��� $��* ��� $��* �.+�	� $��	Prototype��$��0��$��0��" �� *Model_Rpd.Opt_Prototype+$ TwoWay=�$ PropertyChanged=�	� $��
Pre-Launch��$��0��$��1��" �� *Model_Rpd.Opt_PreLaunch+$ TwoWay=�$ PropertyChanged=�	� $��
Production��$��0��$��2��" �� *Model_Rpd.Opt_Production+$ TwoWay=�$ PropertyChanged=��� $��IV.Submission Details ��$��1��$��1��$��0,5,10,5q�.��� $��0��$��0�� �� $��38 ��� $��28 ��� $��28 ��� $��* � �� $��10.5* ��� $	��14* ��� $	��14* ��� $	��11* ��� $��2* �.+��� $��Customer 
 Needed Date : ��$ Right=�$��0��$��Right=�$	��5,0q�  $(
 "Please select Customer Needed Date��$��22��$��1��$��0��$��0,0,15,0q� �� *Model_Rpd.CUSTOMER_NEED_DT+$ TwoWay=�$ PropertyChanged=��� $#��Marketing 
 Committed Date : ��$ Right=�$��2��$	��5,0q�  $-
 'Please select Marketing Committed Date ��$��22��$��3��$��0�� �� *Model_Rpd.MKTG_COMMITED_DT+$ TwoWay=�$ PropertyChanged=��� $'��!Development Methodology Suggested��$��1��$��0��$��2��$��4��$��5,5,0,5q�.���  �� $��* ��� $��* �.+�	� $��Prototype(Knife and Fork)��$��0��$��0��" �� *Model_Rpd.Opt_Devlp_Proto+	� $"��PreLaunch(Fullprodn.tooling)��$��0��$��1��" �� *Model_Rpd.Opt_Devlp_Prelaunch+�� $��PPAP Level : ��$ Right=�$��2��$��1��$��5,5,5,5q��� $
 Please select PPAP Level��$��3��$��1��$��70��$
��Left=�$��0,3,0,0q�$	��Top=� �� *Model_Rpd.PPAP_LEVEL+$ TwoWay=�$ PropertyChanged=�.@��� .��1�� .��2�� .��3�� .��Waived�� $��PPAP Forging quantity : ��$ Right=�$��2��$��2��$��5,5,5,5q��� $(
 "Please enter PPAP Forging quantity��$��65��$
��Left=�$��3��$��2��$ 8��$ UnSignedInt=� �� *Focus_PPAP_FORGING+���� *Model_Rpd.PPAP_FORGING+$ TwoWay=�$ PropertyChanged=��� $��PPAP Sample quantity : ��$ Right=�$��2��$��3��$��5,5,5,5q��� $'
 !Please enter PPAP Sample quantity��$��65��$
��Left=�$��3��$��3��$ 8��$ UnSignedInt=� �� *Focus_PPAP_SAMPLE+���� *Model_Rpd.PPAP_SAMPLE+$ TwoWay=�$ PropertyChanged=�� $��0��$��2��$��2��.+��� $'��!V.Customer specific requirements ��$��1��$��0��$��5,5,10,5q�.��� $��0��$��1�� �� $��47 ��� $��57 ��� $��77 ��� $��* � �� $	��20* ��� $	��15* ��� $	��55* ��� $	��40* �.+��� $��	Packaging��$��0��$��0��$��2��$��5,5,5,5q�.��� $��0��$��1�� �� $��* ��� $��* �.+�	� $��Special��$��2,2,2,2q�$��0��$��0������ *SpeicalClickCommand+" �� *Model_Rpd.Opt_Special+	� $��Standard��$��2,2,2,2q�$��0��$��1������ *StandardClickCommand+" �� *Model_Rpd.Opt_Stand+�� $��Special Charact/.��$��2��$��0��$��5,5,5,5q�.��� $��0��$��1�� �� $��* � �� �� .+�	� $	��Yes��$��2,2,2,2q�$��0��$��0������ *SpeicalGridClickCommandYes+" �� *Model_Rpd.Opt_Special_Yes+	� $��No��$��2,2,2,2q�$��1��$��0������ *SpeicalGridClickCommandNo+" �� *Model_Rpd.Opt_Special_No+�� $��Nature of Packing:��$ Right=�$��1��$��5,5,5,5q�y��� *
LblPacking+�� $%
 Please enter Nature of Packing ��$��2��$��0��$	��300��$
��Left=�$ 50��$��2,2,2,2q�y��� *
TxtPacking+ �� *Focus_NATURE_PACKING+���� *Model_Rpd.NATURE_PACKING+$ TwoWay=�$ PropertyChanged=�� $��1��$��1��$��2��$��2�� �� $��3* ��� ��  �� .+�) 
 #Microsoft.Windows.Controls.DataGrid  -   #  Name$# rpdDataGrid��$��5,0,0,0q�$   RowHeaderWidth$$ 20Q�$��0��%   CanUserAddRows% .  $��0��$��2��$��2��&   AutoGenerateColumns& .  y��� *GridVisible+���� *Model_Rpd.GridData+$ PropertyChanged=�$ TwoWay=� �� $ Delete�� �� *GridDeleteCommand+' �� CommandParameter' �� ( �� ElementName$( rpdDataGrid��) �� Path$) SelectedItem�* � ContextMenu* �� .@�m� $��
Delete Row��+ m� Command+ �� *GridDeleteCommand+, m� CommandParameter, �� - �� RelativeSource- �� *FindAncestor+. �� AncestorType". ��$") PlacementTarget.SelectedItem�   $ RowEditEnding��.    �� *RowEditEndingSubTypeCommand+/   CommandParameter/ �� $( rpdDataGrid��$) SelectedItem�  $ CellEditEnding��.    �� *GridLostFocus+/ �� $( rpdDataGrid��$) SelectedItem�0   Columns0 3 
 -Microsoft.Windows.Controls.DataGridTextColumn  / 
 )Microsoft.Windows.Controls.DataGridColumn1   
IsReadOnly1 . 2   Header$
2 S.NO��3   Width8 
 2Microsoft.Windows.Controls.DataGridLengthConverter$3 60 4 
 .Microsoft.Windows.Controls.DataGridBoundColumn4   Binding4 �� *SLNO+$ TwoWay=�$ PropertyChanged=�  $	3 150 $2 Characteristic��4 �� *CHARACTERISTIC+$ TwoWay=�$ PropertyChanged=�5   EditingElementStyle5 �� 6 �� 
TargetType"6 ��.��� 7 �� Property7 �@��	MaxLength8 �� Value$8 50���� 7 �@�ToolTip8 �� *CHARACTERISTIC+  $	3 110 $2 Severity��4 �� *
SEVERITY+$ TwoWay=�$ PropertyChanged=�5 �� "6 ��.��� 7 �@��	MaxLength$8 2���� 7 �@�ToolTip8 �� *
SEVERITY+�� 7 �@	 Mask$8 UnSignedDec=��� 7 �@	 	IsFocused8 �� *	firstCell+  $	3 192 $2 Customer Expectation��4 �� *CUSTOMER_EXP+$ TwoWay=�$ PropertyChanged=�5 �� "6 ��.��� 7 �@��	MaxLength$8 50���� 7 �@�ToolTip8 �� *CUSTOMER_EXP+�� $(��"Severity rating : Between 5 to 10 ��$ Center=�$��1��$��5,5,5,5q�$��3��$��2���� $,��&Other Customer Specific requirement : ��$��Right=�$��2��$ Center=�$��0��$��5,5,5,5q�$��4���� $7
 1Please enter Other Customer Specific requirement ��$��2��$	 200��$��4��$��2,2,2,2q�$��3������ *Model_Rpd.OTHER_CUST_REQ+$ TwoWay=�$ PropertyChanged=�� $��3��$��0��$��3��$��10,0,0,0q� �� $	��11* ��� $	��10* ��� $	��17* ��� $	��11* ��� $	��11* ��� $	��11* ��� $	��12* �.+��� $&�� Customer Expectations:(Example) ��$
 Left=�$��0��$��5,5,5,5q�$��0���� $��1.Submit FMEA with PPAP��$
 Left=�$��0��$��5,5,5,5q�$��1���� $8��22.SPC during prodn run and charts to be maintained��$
��Wrap=�$
 Left=�$��0��$��5,5,5,5q�$��2���� $ ��3.Initial capability > 2.0��$
 Left=�$��0��$��5,5,5,5q�$��3���� $)��#4.Customer approval of control plan��$
 Left=�$��0��$��5,5,5,5q�$��4���� $��5.100% Inspection  ��$
 Left=�$��0��$��5,5,5,5q�$��5���� $.��(6.Final Inspection required (Sampling)  ��$
 Left=�$��0��$��5,5,5,5q�$��6��