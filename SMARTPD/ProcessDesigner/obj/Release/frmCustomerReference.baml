   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl *   $ProcessDesigner.frmCustomerReference   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     Swfirms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions     8busy-http://schemas.microsoft.com/wpf/2008/toolkit 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 =
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl 
 ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 -   $	��660��$	��900��   WindowBackground#��[  .���� $��0q�._��   � RowDefinitions  �� $
��8.5* ��� $��91.5* �.+��� $��0��$��0��$��2,2,2,2q�`� .�c� $��0��$��0��$��58��.@��� $��Add F3��$��50��$��2,0,2,0q�$��	10,0,10,0q�0   *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Add.png�� � ToolTip$ Add New Record������ *AddECNOrPCNCommand+{��� *	AddEnable+�� $��Edit/View F5��$��50��$��2,0,2,0q�$��	10,0,10,0q�-  'System.Windows.Input.KeyboardNavigation   	IsTabStop .  $ Images/Edit.png��$ Edit or view record������ *EditECNOrPCNCommand+{��� *
EditEnable+ �� UpdateSourceTrigger$ PropertyChanged=��� $��Save Ctrl+S��$��50��$��2,0,2,0q�$��	10,0,10,0q� .  $ Images/Save.png��$ Save record������ *SaveECNOrPCNCommand+{��� *
SaveEnable+$ PropertyChanged=��� $��Print F8��$��50��$��2,0,2,0q�$��	10,0,10,0q� .  $ Images/Print.png��$ Print details from screen������ *PrintECNOrPCNCommand+{��� *PrintEnable+$ PropertyChanged=��� $��	Close F9 ��$��50��$��2,0,2,0q�$��	10,0,10,0q� .  $ Images/Close.png��$ Close current active form������ *CloseCommand+� $��0��$��1��$��3q�   TabNavigation$ Cycle=�  �� $��0* ��� $	��44* ��� $��0* ��� $��4* ��� $	��19* �.+��� $��0��$��1��$	��0.5q�� $��1��$��2,4,2,2q�  ��  � ColumnDefinitions �� $	��50* ��� $	��50* �.+�� $��0��$��0��$��5,5,5,5q�  �� $��25 ��� $��25 ��� $��25 ��� $��25 ��� $��25 ��� $��25 ��� $��25 ��� $��25 ��� �� �� $��25 ��� $��25 � �� $	��38* ��� $	��25* ��� $	��30* �.+��� -   $��lblReferenceNo��$��Reference No. : �� �� TextAlignment$ Right=�$��0��$��0���� -   $��txtReferenceNo��$��1��$	��175��$
��Left=�$��0��
 � Tag$ Please enter Reference No��	   TabIndex$	 0��$��2��$	��2,0q�
 �� 	MaxLength$
 25������ * DDPCNECNModel.ECN_REFERENCE_NO+$ PropertyChanged=��� -   $��lblSFLPartNo��$��SFL Part No. : ��$ Right=�$��1��$��0��.   (ProcessDesigner.UserControls.ComboBoxCus  -   $��	cmbPartNo��$  Please select SFL Part No.��   CharacterCasingText$ Upper=�$	��160��$��1��$��1��   ButtonVisibility$ Visible=�$	��2,0q�$ Select Part Number.��   SelectedValuePath$ PART_NO��   
IsReadOnly .     
IsEditable . $��2��$
��Left=�-  'ProcessDesigner.UICommon.FocusExtension   	IsFocused �� *PartNumberIsFocused+$ PropertyChanged=�   	MaxLength �� *ComboBoxMaxLength+   
DataSource �� *
DvDD_PCN+ �� Mode$ TwoWay=�$ PropertyChanged=�   ColumnsHeader �� *DropDownItemsPartNo+   SelectedItem �� *SelectedRow+   SelectedValue �� *PartNo+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *SelectChangeComboPartCommand+  $ 	LostFocus��.    �� *LostFocusPartNoCommand+ J� InputBindings �� 
 �� Key$ Enter�� �� Command �� *EnterPartNumberCmb+�� -   $��txtCustomersPartNo��$��1��$��2��$	��160��$! Please enter Cust. Part No.��$��2,2,2,2q�$
 20������ *DDPCNECNModel.CUST_PART_NO+$ PropertyChanged=��� -   $��txtCustomersIssueNo��$��2��$��2��$��70��$ Please enter issue No.��$
��Left=�$��2,2,2,2q�2	   ,ProcessDesigner.UserControls.TextBoxIntsOnly 	  Mask$ UnSignedInt=�$
 10������ *DDPCNECNModel.CUST_ISSUE_NO+$ PropertyChanged=��� -   $��lblCustomersPartNo��$'��!Customer's Part No. & Issue No : ��$ Right=�$��2��$��0���� -	   $��lblCustomer��$��Customer : ��$ Right=�$��3��$��0���� -
   $��lblCustomerDrgNo��$%��Customer Drg No. & Issue No. : ��$ Right=�$��4��$��0���� -   $��lblProDrgIssueNoDate��$!��Pro.Drg.Issue No. & Date : ��$ Right=�$��5��$��0���� -   $��lblProSeqDrgIssueNoDate��$%��Pro.Seq.Drg.Issue No. & Date : ��$ Right=�$��6��$��0���� -   $��lblDescription��$��Description : ��$ Right=�$��7��$��0���� -   $��txtCustomer��$��1��$��3��$ Please enter Customer��$��2��$��2,2,2,2q�$
 30������ *DDPCNECNModel.CUST_NAME+$ PropertyChanged=��� -   $��txtCustomerDrgNo��$��1��$! Please select Cust. Drg No.��$��4��$��2,2,2,2q�$
 50������ *DDPCNECNModel.CUST_DWG_NO+$ PropertyChanged=��� -   $��txtCustomerIssueNo��$��2��$��4��$ Please enter Issue No.��$��70��$
��Left=�$��2,2,2,2q�$
 15������ *!DDPCNECNModel.CUST_DWG_NO_ISSUE+$ PropertyChanged=��� -   $��txtProDrgIssueNoDate��$��1��$ Pro.Drg.Issue.No.��$��5��$��85��$
��Left=�$��2,2,20,2q�$ UnSignedInt=�$
 10������ *SFLDrawIssueNo1+$ PropertyChanged=�0
   *ProcessDesigner.UserControls.DatePickerCus
  -   $��	dtpDrgSeq��$	��110��$ Please select date��$
��Left=�$��2��$��5��$��2,2,2,2q� 
  SelectedDate �� *#!DDPCNECNModel.SFL_DRAW_ISSUEDATE1+$ PropertyChanged=�$ TwoWay=��� -   $��txtProSeqDrgIssueNoDate��$��1��$��6��$( "Please enter Pro.Seq.Drg.Issue No.��$��85��$
��Left=�$��2,2,20,2q�$ UnSignedInt=�$
 10������ *SFLDrawIssueNo+$ PropertyChanged=�
  -   $��	dtpProSeq��$	��110��$ Please enter date��$
��Left=�$��2��$��6��$��2,2,2,2q� �� *#!DDPCNECNModel.SFL_DRAW_ISSUE_DATE+$ PropertyChanged=�$ TwoWay=��� -   $��txtDescription��$��1��$��7��$ Please enter Description��$��2��$��2,2,2,2q�$	
 100������ *DDPCNECNModel.PART_DESC+$ PropertyChanged=��� -   $��lblDetailsOfChange��$��Details Of Change : ��$ Right=�$��8��$��0���� -   $��txtDetailsOfChange��$��1��$��8��$��2��$��2,2,2,-1q�  �� TextWrapping$
  Wrap=�$$ Please enter Details of Change��$��52��$	��Top=�$

 1000��! �� AcceptsReturn! . ���� * DDPCNECNModel.NATURE_OF_CHANGE+$ PropertyChanged=��� -   $��lblReasonForChange��$��Reason For Change : ��$ Right=�$��9��$��0���� -   $��txtReasonForChange��$��1��$��9��$��2,2,2,-1q�$��2��$
  Wrap=�$$ Please enter Reason for Change��$��52��$	��Top=�$

 1000��! . ���� * DDPCNECNModel.RESON_FOR_CHANGE+$ PropertyChanged=��� $��Verification : ��$ Right=�$��10��$��0���� $��Validation : ��$ Right=�$��11��$��0���� -   $��txtVerification��$��1��$��10��$ Please enter Verification��$��2��$��2,2,2,2q�$	
 100������ *DDPCNECNModel.VERIFICATION+$ PropertyChanged=��� -   $��txtValidation��$��1��$��11��$ Please enter Validation��$��2��$��2,2,2,2q�$	
 100������ *DDPCNECNModel.VALIDATION+$ PropertyChanged=�� $��0��$��1��$��2��$��5,5,5,0q�  �� $
��2.2* ��� $	��14* ��� $��25.8* ��� $	��15* ��� $��4* � �� $	��50* ��� $	��50* �.+��� -   $��lblCost��$��Cost : ��$
 Left=�$	��Top=�$��0��$��0���� -   $��txtCost��$��0��$��1��$ Please enter Cost��$	��Top=�$��2,2,2,0q�$��80��$
  Wrap=�$	
 200��" �� TextAlignment$
" Left=�$	��Top=�! . ���� *DDPCNECNModel.COST_DESC+$ PropertyChanged=�$ TwoWay=��� -   $��lblManufacturingProcess��$��Manufacturing Process : ��$
 Left=�$	��Top=�$��0��$��2��$
��Wrap=��� -   $��txtManufacturingProcess��$��2��$��1��$	��Top=�$��2,2,2,0q�$��80��$
  Wrap=�$( "Please enter Manufacturing Process��$	
 200��$	��Top=�! . ���� *#!DDPCNECNModel.MANUFACTURE_PROCESS+$ PropertyChanged=�$ TwoWay=��� $��2��$��Documents Changed��$��0��$��2��$��2,1,2,1q�$	��150��$	��Top=�.���   �� $	��20* ��� $	��20* ��� $	��20* ��� $	��25* � �� $	��20* ��� $	��20* ��� $	��20* �.+��� -    $��chkprddrawing��$��PRODUCT DWG��$��Center=�$
��Left=�$��0��$��0��# d� 	IsChecked# �� *DDPCNECNModel.PRODUCT_DWG+$ PropertyChanged=��� $��SEQUENCE DWG��$��Center=�$
��Left=�$��1��$��0��# �� *DDPCNECNModel.SEQUENCE_DWG+$ PropertyChanged=��� $��TOOL DWG��$��Center=�$
��Left=�$��2��$��0��# �� *DDPCNECNModel.TOOL_DWG+$ PropertyChanged=��� $��	GAUGE DWG��$��Center=�$
��Left=�$��0��$��1��# �� *DDPCNECNModel.GAUGE_DWG+$ PropertyChanged=��� $��PFMEA��$��Center=�$
��Left=�$��1��$��1��# �� *DDPCNECNModel.PFMEA+$ PropertyChanged=��� $��CONTROL PLAN��$��Center=�$
��Left=�$��2��$��1��# �� *DDPCNECNModel.CONTROL_PLAN+$ PropertyChanged=��� $	��PFD��$��Center=�$
��Left=�$��0��$��2��# �� *DDPCNECNModel.PFD+$ PropertyChanged=��� $��OTHERS��$��Center=�$
��Left=�$��1��$��2��# �� *DDPCNECNModel.OTHERS+$ PropertyChanged=��� $��ROUTING TAG��$��Center=�$
��Left=�$��2��$��2��# �� *DDPCNECNModel.ROUTING_TAG+$ PropertyChanged=��� $��0��$��3��$��Center=�# �� *#!DDPCNECNModel.FINISH_CODE_SAP_DWG+$ PropertyChanged=�.���� �� $ �� TextWrapping$
$ Wrap=�$"��FINISH CODE LOCK IN SAP DWG.���� $��WORK INSTRUCTION��$��Center=�$
��Left=�$��1��$��3��# �� * DDPCNECNModel.WORK_INSTRUCTION+$ PropertyChanged=��� $��Center=�$
��Left=�$��3��$��3��# �� *(&DDPCNECNModel.SAP_SEQ_DWG_ISSUE_NO_UPD+$ PropertyChanged=�.���� �� $
$ Wrap=�$)��#IN SAP SEQ. DWG. ISSUE NO. UPDATION���� $��3��$��2��$��Proposed Implementation��$��2,2,2,1q�.���   �� $	��50* ��� $	��50* � �� $	��50* ��� $	��50* �.+��� $��ALREADY IN EFFECT��$��Center=�$
��Left=�$��0��$��0��# �� *!DDPCNECNModel.ALREADY_IN_EFFECT+$ PropertyChanged=��� $��	IMMEDIATE��$��Center=�$
��Left=�$��1��$��0��# �� *DDPCNECNModel.IMMEDIATE+$ PropertyChanged=��� $��FROM NEXT H.T. CYCLE��$��Center=�$
��Left=�$��0��$��1��$��0,0,0,5q�# �� *" DDPCNECNModel.FROM_NEXT_HT_CYCLE+$ PropertyChanged=��� $��FROM NEXT FORGING��$��Center=�$
��Left=�$��1��$��1��$��0,0,0,5q�# �� *!DDPCNECNModel.FROM_NEXT_FORGING+$ PropertyChanged=��� -!   $��	lblRePPAP��$��
Re-PPAP : ��$ Right=�$��4��$��0���� $��2��$��4��._��   �� $��* � �� $��8* ��� $	��20* �.+��� $	��YES��$��Center=�$
��Left=�$��0��$��2��# �� *	RE_PPAP+$ PropertyChanged=�$ TwoWay=��� $��NO��$��Center=�$
��Left=�$��1��$��2��# �� *NOT_RE_PPAP+$ PropertyChanged=�$ TwoWay=�� $��1��$��0��$��5,5,5,5q�  �� $��* � �� $	��30* ��� $	��70* �� $��2��$��0��$��5,5,5,5q�  �� $��* � �� $	��30* ��� $	��70* ��� $��2��$��.5q�._�� $��2��  �� $��* � �� $	��30* ��� $	��30* ��� $	��10* ��� $	��20* ��� $	��10* ��� $	��30* ��� $��3��$��.5q�._��   �� $��* � �� $	��17* ��� $	��23* ��� $	��20* ��� $	��23* ��� $	��17* ��� $	��23* ��� $	��25* ��� $	��15* �.+��� -"   $��lblCompiledBy��$��Compiled By : ��$ Right=�$��0��$��0��5   /ProcessDesigner.UserControls.ComboBoxWithSearch  $��0��$��1��$ Please select Compiled By��%   DropDownWidth$	% 300��&   SelectedValuePath$& 	FULL_NAME��'   DisplayValuePath$' 	FULL_NAME��(   
IsReadOnly( . $��Center=�)   
DataSource) �� *CompiledByCombo+*   SelectedValue* �� *DDPCNECNModel.COMPILED_BY+$ TwoWay=�$ PropertyChanged=�+   SelectedText+ �� *DDPCNECNModel.COMPILED_BY+$ TwoWay=�$ PropertyChanged=�,   ColumnsHeader, �� *DropDownHeaderCompiledBy+�� -#   $��lblApprovedBy��$��Approved By : ��$ Right=�$��0��$��2��  ( .  -   
IsEditable- . $ Please select Approved By��$��0��$��3��$	% 300��$& 	FULL_NAME��$' 	FULL_NAME��$��Center=�) �� *ApprovedByCombo+* �� *DDPCNECNModel.APPROVED_BY+$ TwoWay=�$ PropertyChanged=�+ �� *DDPCNECNModel.APPROVED_BY+$ TwoWay=�$ PropertyChanged=�, �� *DropDownHeaderCompiledBy+�� -$   $��lblApprovedDate��$��Date : ��$ Right=�$��0��$��4��
  -%   $��dtpAppBy��$��5��$��0��$ Please select date��$	��110�� �� *DDPCNECNModel.DATE_OF_PCN+$ PropertyChanged=�$ TwoWay=��� -&   $��lblEnggChangeNoteNo��$ Right=�$��2,2,2,2q�$��0��$��6������ *ECNOrPCNLabel+$ PropertyChanged=�$ TwoWay=��� -'   $��txtEnggChangeNoteNo��$��7��$��0��$��60��$
��Left=�$��2,2,7,2q�$
 20��. �� 
IsReadOnly. . / �� IsReadOnlyCaretVisible/ . ���� *
PCNSheetNo+$ PropertyChanged=�$ TwoWay=�� $��4��  �� $��* � �� $	��30* ��� $	��30* �.+��   �� $��* � �� $	��40* ��� $	��60* �.+��� -(   $#��lblActualChangeImplementation��$%��Actual Change Implementation : ��$ Right=�$��0��$��0���� $��1��$"��Actual Change Implementation��$��0��$��2��.���   �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* � �� $	��17* ��� $	��20* ��� $	��20* ��� $	��25* ��� $	��20* ��� $	��15* �.+��� -)   $��lblInfgg��$��Stock At : ��$ right=�$��0��$��0���� -*   $��lblInwip��$��Logistics : ��$ right=�$��1��$��0���� -+   $��lblInHeartreatment��$��Ware House : ��$ right=�$��2��$��0���� $��Work in Progress H.T:��$ right=�$��3��$��0���� -,   $��txtStockAtCode��$��1��$��0��$ Please enter Stock At��$��2,2,2,2q�$
 20������ *DDPCNECNModel.STOCK_AT_CODE+$ PropertyChanged=�$ TwoWay=��� --   $��txtStockAtDescription��$��2��$��0��$ Please enter Stock At��$��2,2,2,2q�$
 50������ *$"DDPCNECNModel.STOCK_AT_DESCRIPTION+$ PropertyChanged=�$ TwoWay=��� -.   $��txtLogisticsCode��$��1��$��1��$ Please enter Logistics��$��2,2,2,2q�$
 20������ *DDPCNECNModel.LOGISTICS_CODE+$ PropertyChanged=�$ TwoWay=��� -/   $��txtLogisticsDescription��$��2��$��1��$ Please enter Logistics��$��2,2,2,2q�$
 50������ *%#DDPCNECNModel.LOGISTICS_DESCRIPTION+$ PropertyChanged=�$ TwoWay=��� -0   $��txtWareHouseCode��$��1��$��2��$ Please enter Ware House��$��2,2,2,2q�$
 20������ *DDPCNECNModel.WARE_HOUSE_CODE+$ PropertyChanged=�$ TwoWay=��� -1   $��txtWareHouseDescription��$��2��$��2��$ Please enter Ware House��$��2,2,2,2q�$
 50������ *&$DDPCNECNModel.WARE_HOUSE_DESCRIPTION+$ PropertyChanged=�$ TwoWay=��� -2   $��txtWIPHTCode��$��1��$��3��$' !Please enter Work in Progress H.T��$��2,2,2,2q�$
 20������ *DDPCNECNModel.WIP_HT_CODE+$ PropertyChanged=�$ TwoWay=��� -3   $��txtWIPHTDescription��$��2��$��3��$' !Please enter Work in Progress H.T��$��2,    2,2,2q�$
 50������ *" DDPCNECNModel.WIP_HT_DESCRIPTION+$ PropertyChanged=�$ TwoWay=��� $��New : ��$ right=�$��4��$��0���� -4   $��
txtNewCode��$��1��$��4��$ Please enter New��$��2,2,2,2q�$
 20������ *DDPCNECNModel.NEW_CODE+$ PropertyChanged=�$ TwoWay=��� -5   $��txtNewDescription��$��2��$��4��$ Please enter New��$��2,2,2,2q�$
 50������ *DDPCNECNModel.NEW_DESCRIPTION+$ PropertyChanged=�$ TwoWay=��� $&�� Actual Date of Implementation : ��$ right=�$��0��$��3��$��NoWrap=�
  -6   $��dtpActualDate��$' !Please select Actual Date of Imp.��$��4��$��0��$	��110��$
��Left=� �� *&$DDPCNECNModel.DATE_OF_IMPLEMENTATION+$ PropertyChanged=�$ TwoWay=��� $��INVOICE / D.A No. : ��$ right=�$��1��$��3��$��WrapWithOverflow=��� -7   $��txtInvoiceNo��$��90��$
��Left=�$" Please enter Invoice/D.A No.��$��4��$��1��$��2,2,2,2q�$ Decimal=�$
 10������ *DDPCNECNModel.INVOICE_NO+$ PropertyChanged=�$ TwoWay=��� $��Date of Invoice /D.A. : ��$ right=�$��2��$��3��$��WrapWithOverflow=�
  -8   $��dtpInvoiceDate��$# Please select Date of Invoice��$��4��$��2��$	��110��$
��Left=� �� *DDPCNECNModel.INVOICE_DATE+$ PropertyChanged=�$ TwoWay=��� $��Sign : ��$ right=�$��3��$��3��$��WrapWithOverflow=��� -9   $��txtSign��$��4��$ Please enter Sign��$��3��$��2,2,2,2q�$	
 100������ *DDPCNECNModel.SIGNATURE_NAME+$ PropertyChanged=�$ TwoWay=��� $��Date : ��$ right=�$��4��$��3��$��WrapWithOverflow=�
  -:   $��dtpSignDate��$ Please select Date��$��4��$��4��$	��110��$
��Left=�$ 	Sign Date�� �� *DDPCNECNModel.SIGNATURE_DATE+$ PropertyChanged=�$ TwoWay=� �� $ F3�� �� *AddECNOrPCNCommand+�� $ F5�� �� *EditECNOrPCNCommand+�� $ S��0 �� 	Modifiers$0 Controlf� �� *SaveECNOrPCNCommand+�� $ F8�� �� *PrintECNOrPCNCommand+�� $ F9�� �� *CloseCommand+