   M S B A M L   `   `   `  ���� h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls *   $ProcessDesigner.frmDevelopmentReport   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 qsi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 RtJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero 
 ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 ]LookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 -   $	��600��$
��1000��$
��1000��   WindowBackground#��[    � 	Resources  0  *ProcessDesigner.UICommon.DateTimeConverter  DateTimeConverter��%   &	         .���� $��1q�
���   �$��10,10,10,10q�._�� -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=� � RowDefinitions �� $	��10* ��� $	��10* ��� $	��60* ��� $	��20* �.+��� $��0��$��0��$��2��._�`� .�c� -    c�Name$ Part1�� c� 	BandIndex$ 0��$��58��$
 None=�.@��� $��Add F3��$��0��$��0��$��73��$��50��   	IsTabStop .   � ToolTip$ Add New Record��0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Add.png������ *OnAddCommand+{��� *AddButtonIsEnable+�� $��Edit/View F5��$��0��$��1��$��80��$��50�� .  $ Edit or View Record��$ Images/Edit.png��{��� *EditButtonIsEnable+���� *OnEditViewCommand+�� $��Save Ctrl+S��$��0��$��3��$��69��$��50�� .  $ Save Record��$ Images/Save.png��{��� *SaveButtonIsEnable+���� *OnSaveCommand+�� $��	Print F8 ��$��50��$��2,0,2,0q� .  $��	10,0,10,0q�$ Images/Print.png��{��� *PrintButtonIsEnable+ �� UpdateSourceTrigger$ PropertyChanged=����� *OnPrintCommand+�� $��Search ��$��50��$��2,0,2,0q�$��	10,0,10,0q�$ Images/Search.png�� .  o� 	 c� OverflowMode$	 Always=�.@��� 
�������.+�m� $��Cost Sheet Search��
 m� Command
 �� *CostSheetSearchCommand+m� $��Product Search��
 �� *ProductSearchCommand+m� $��Tools Search��
 �� *ToolsSearchCommand+c� $��58��$
 None=�.@��� $��Undo ��$y�	Collapsed=�$��50�� .  $��2,0,2,0q�$��	10,0,10,0q�$ Images/Undo.png������ *OnUndoCommand+�� $��Show Related��$��50�� .  $��2,0,2,0q�$��	10,0,10,0q�$ Images/ShowRelated.png��o� $	 Always=�.@��� 
�������.+�m� $��Process Sheet��
 �� *ProcessSheetCommand+m� $��Control Plan��
 �� *ControlPlanCommandm+m� $��Drawings��
 �� *DrawingsCommand+m� $��Tool Schedule��
 �� *ToolScheduleCommand+�� m� $��Product Search��
 �� *ProductSearchCommand+m� $��Cost Sheet Search��
 �� *CostSheetSearchCommand+m� $��Tools Search��
 �� *ToolsSearchCommand+c� $��58��$
 None=�.@��� $��Close F9�� .  $��0��$��4��$��65��$��50��$ 
Close Form��$ Images/Close.png������ *OnCloseCommand+� $��1��$��2q� �� $	��47* ��� $	��53* � � ColumnDefinitions �� $	��13* ��� $	��10* ��� $��5* ��� $	��10* ��� $��9* ��� $��1* ��� $	��45* �.+��� -    ��Name$ 	lblPartNo��$��2��$	��100��$
��Left=�$��	Part No :��$��0��$��0��$��5,0,0,0q�$
��Bold"��� -   $ lblRunNo��$��	Run No : ��$��0��$��2���� -   $ lblDescription��$
��Bold"�$��Description :��$��0��$��4�� �� TextAlignment$ Right=�.  (ProcessDesigner.UserControls.ComboBoxCus  -     Name$ 	cmbPartNo��$
��Bold"�   CharacterCasingText$ Upper=�   	MaxLength$ 11��$	��160��   
IsEditable .    
IsReadOnly .  $
��Left=�$��0��$��2��
 � Tag$ Please select Part No.��$��66,0,0,0.286q�   SelectedValuePath$ PART_NO��-  'ProcessDesigner.UICommon.FocusExtension   	IsFocused �� *FocusButton+$ PropertyChanged=�   
DataSource �� *&$DevelopmentReportModel.PartNoDetails+$ PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors .    SelectedItem �� *SelectedRowPart+$ PropertyChanged=�   SelectedValue �� *DevelopmentReportModel.PartNo+$ PropertyChanged=�   ColumnsHeader �� *DropDownItemsPart+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6	  0System.Windows.Interactivity.InvokeCommandAction	   	  Command �� * SelectChangeComboCommandPartNo+$ PropertyChanged=�  J� InputBindings  �� 
! �� Key$! Enter��" �� Command" �� *EnterPartNumberCmb+5
  /ProcessDesigner.UserControls.ComboBoxWithSearch
  -   # 
 Name$# cmbFeatureCode��$��60��$
��Left=�$ 
  	MaxLength$$ 5��% 
  
IsEditable% .  $��0��$��3��$ Please select Run No.��& 
  SelectedValuePath$& 
DEV_RUN_NO��' 
  DisplayValuePath$' 
DEV_RUN_NO��( 
  
IsReadOnly( �� *IsReadOnlyRun+$ PropertyChanged=�) 
  ButtonVisibility) �� *ButtonEnable+* 
  
DataSource* �� *%#DevelopmentReportModel.RunNoDetails+$ PropertyChanged=�+ �� Mode$+ TwoWay=� .  . , 
  SelectedItem, �� *SelectedRowRunNo+- 
  SelectedValue- �� *DevelopmentReportModel.RunNo+$ PropertyChanged=�. 
  SelectedText. �� *DevelopmentReportModel.RunNo+$ PropertyChanged=�/ 
  ColumnsHeader/ �� *DropDownItemsRun+   $ SelectedItemChanged��. 	   �� *SelectChangeComboCommandRunNo+�� -   $ lblPartDescription��$
��Bold"�$��0��$��6��$
 Left=����� *#!DevelopmentReportModel.PartNoDesc+$ PropertyChanged=��� -	   $ lblDevelopmentDetails��$	��160��$
��Left=�$��Development Details : ��$��1��$��0��$��4��$
 Left=�$��2,0,0,0q�$��12(�$
��Bold"��� -
   $ 
lblRunDate��$��
Run Date :��$��1��$��4��$ Right=�0  *ProcessDesigner.UserControls.DatePickerCus  -   $��
txtRunDate��$��1��$ Please select Run Date��$��6��$��2,1,0,5q�$ Enter Cust. Approved Date��$
��Left=�$	��112��0   SelectedDate0 �� * DevelopmentReportModel.RunDate+$+ TwoWay=�$ PropertyChanged=��� -   1 ��Name$1 DEV_Inter_Tab��$��2��$��2q�.@��� $��Design Assumptions��$	��150��.��� $��0�� �� $	��50* ��� $	��30* �.+��� $��I.Design Assumptions:��$��0��.��&   System.Windows.Controls.DataGrid  -   2  Name$2 ssAssumtions��3   RowHeaderWidth$3 25Q�4   CanUserResizeRows4 .  $��0��$��0��5   AutoGenerateColumns5 .  6   CanUserAddRows6 .  7   CanUserDeleteRows7 .  2  ,ProcessDesigner.UICommon.ScrollToTopBehavior8   ScrollToTop8 . ���� *0.DevelopmentReportModel.DesignAssumptionDetails+$ PropertyChanged=�$+ TwoWay=�  �� $! Delete��" �� *DeleteCommandAssumtions+9 �� CommandParameter9 �� : �� ElementName$: ssAssumtions��; �� Path$; SelectedItem�   $ CurrentCellChanged��. 	   �� *&$RowEditEndingDesignAssumptionCommand+< 	  CommandParameter< �� $: ssAssumtions��$; SelectedItem�  $ BeginningEdit��. >   8Microsoft.Expression.Interactivity.Core.CallMethodAction  =   
MethodName$= OnBeginningEdit��>   TargetObject> �� ?   Columns? 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn@   Header$@ Part #��A   
Visibility$A Hidden=�1  +System.Windows.Controls.DataGridBoundColumnB   BindingB �� *	PART_NO+$ PropertyChanged=�$+ TwoWay=�  $@ Runnig #��$A Hidden=�B �� *
DEV_RUN_NO+$ PropertyChanged=�$+ TwoWay=�  $
@ S.No��C   Width5  /System.Windows.Controls.DataGridLengthConverter$	C 10* B �� *SNO+$ PropertyChanged=�$+ TwoWay=�D   ElementStyleD �� E �� 
TargetType"E ��.��� F �� PropertyF � ��G �� Value$G Center=�H   EditingElementStyleH �� "E ��.��� F �@��	MaxLength$G 2���� F � ��$G Center=�  $@ Design Assumptions��$	C 25* B �� *
CFT_DISS+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 200���� F � ��$G Center=�  $"@ Justification for Assumption��$	C 25* B �� *JUSTIFICATION+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 200���� F � ��$G Center=�  $@ TGR(Gone Right)��$	C 10* B �� *TGR+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 200���� F � ��$G Center=�  $@ TGW(Gone Wrong)��$	C 10* B �� *TWG+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 200���� F � ��$G Center=��� $#��II.Record of CFT Discussions:��$��3��$��0,211,0,0.329q�$��2��.���� -   I ��Name$I 	txtRecord��$��0��$��0��$��56��$��Stretch=�J �� TextAlignment$J Justify=�K �� AcceptsReturnK . L �� TextWrapping$
L Wrap=�"M �� VerticalScrollBarVisibility$M Visible=�$N �� HorizontalScrollBarVisibility$N Visible=�$	��Top=����� *.,DevelopmentReportModel.RecordOfCFTDiscussion+$ PropertyChanged=�.���� $	��LOG��$	��150��.��� $��0�� �� $	��80* ��� $	��20* �.+�) 
 #Microsoft.Windows.Controls.DataGrid  -   O  Name$O ssDevReportLog��P   RowHeaderWidth$P 25Q�Q   CanUserResizeRowsQ .  $# Enter Development Log Details��$��0��$��0��R   AutoGenerateColumnsR .  S   CanUserAddRowsS .  ���� *#!DevelopmentReportModel.LogDetails+$ PropertyChanged=�$+ TwoWay=�  �� $! Delete��" �� *DeleteCommandLog+9 �� $: ssDevReportLog��$; SelectedItem�   $ CurrentCellChanged��. 	   �� *RowEditEndingLogCommand+< �� $: ssDevReportLog��$; SelectedItem�  $ BeginningEdit��.   $= OnBeginningEditLog��> �� T   ColumnsT 3 
 -Microsoft.Windows.Controls.DataGridTextColumn  / 
 )Microsoft.Windows.Controls.DataGridColumnU   Header$U Part #��V   
Visibility$V Hidden=�4 
 .Microsoft.Windows.Controls.DataGridBoundColumnW   BindingW �� *	PART_NO+$ PropertyChanged=�$+ TwoWay=�  $U Runnig #��$V Hidden=�W �� *
DEV_RUN_NO+$ PropertyChanged=�$+ TwoWay=�  $U Seq No��X   Width8 
 2Microsoft.Windows.Controls.DataGridLengthConverter$X 6* W �� *
STAGE_NO+$ PropertyChanged=�$+ TwoWay=�Y   ElementStyleY �� "E ��.��� F � ��$G Center=�Z   EditingElementStyleZ �� "E ��.��� F �@��	MaxLength$G 2���� F � ��$G Center=��� 2  ,ProcessDesigner.UserControls.TextBoxIntsOnlyF �@ Mask$G UnSignedDec=�  $U Problem Faced��$	X 24* W �� *PROBLEM_FACED+$ PropertyChanged=�$+ TwoWay=�Y �� "E ��.��� F � ��$G Center=�Z �� "E ��.��� F �@��	MaxLength$
G 2000���� F � ��$G Center=�  $U Counter Measure��$	X 20* W �� *COUNTER_MEASURE+$ PropertyChanged=�$+ TwoWay=�Y �� "E ��.��� F � ��$G Center=�Z �� "E ��.��� F �@��	MaxLength$
G 2000���� F � ��$G Center=�&   BHCustCtrl.DataGridNumericColumn  $U Tool r/w due
 to design��$	X 15* [   MaxPrecision$[ 2��\   MaxScale$\ 0��W �� *REWORK_TOOL_DESIGN+$ PropertyChanged=�$+ TwoWay=�  $U r/w due to
tool mfg��$	X 15* $[ 2��$\ 0��W �� *REWORK_TOOL_MFG+$ PropertyChanged=�$+ TwoWay=�  $U Remarks��$	X 20* W �� *	REMARKS+$ PropertyChanged=�$+ TwoWay=�Y �� "E ��.��� F � ��$G Center=�Z �� "E ��.��� F �@��	MaxLength$
G 2000���� F � ��$G Center=�� $��1�� �� $	��20* ��� $	��20* � �� $	��15* ��� $	��13* ��� $	��15* ��� $	��25* �.+��� -   $ lblNoofforgoing0��$��0��$��0��$ Right=�$��Center=�$-��'Number of forging shifts for the run : ��$��2,5,0,0q��� -   $ lblDesignTools��$&�� Tool r/w due to Design Total  : ��$��0��$��2��$ Right=��� -   $ lblMsgTotal��$#��Tool r/w due to mfg Total  : ��$��1��$��2��$ Right=��� $7 1Please enter Number of forging shifts for the run��$��1��$��0��$��29��]   Mask$] UnSignedInt=�$��Center=�$��2,5,0,0q�$
��Left=�^ �� 	MaxLength$^ 2������ *(&DevelopmentReportModel.NoOfForginShift+$ PropertyChanged=��� -   $ lblDesignToolsValue��$��0��$��3��$
 Left=����� **(DevelopmentReportModel.ToolRWDesignTotal+$ PropertyChanged=��� -   $ lblMsgTotalValue��$��1��$��3��$
 Left=����� *'%DevelopmentReportModel.ToolRWMfgTotal+$ PropertyChanged=��� $��Short Closure report��$	��150��.��� $��0�� �� $	��50* �.+��� $3��-IV.Short closure report: (TGW to be updated )��$��0��.��  -   $2 ssShortClosure��$3 25Q�4 .  $! Enter Short Closure Details��$��0��$��0��5 .  6 .  ���� *,*DevelopmentReportModel.ShortClosureDetails+$ PropertyChanged=�$+ TwoWay=�  �� $! Delete��" �� *DeleteCommandShortClosure+9 �� $: ssShortClosure��$; SelectedItem�   $ CurrentCellChanged��. 	   �� *" RowEditEndingShortClosureCommand+< �� $: ssShortClosure��$; SelectedItem�  $ CellEditEnding��.   $= OnCellEditEndingLogIssue��> �� ?   $@ Part #��$A Hidden=�B �� *	PART_NO+$ PropertyChanged=�$+ TwoWay=�  $@ Runnig #��$A Hidden=�B �� *RUN_NO+$ PropertyChanged=�$+ TwoWay=�  $@ Reason��$	C 15* B �� *REASON+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 100���� F � ��$G Center=�  $	@ Why��$	C 15* B �� *WHY+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 100���� F � ��$G Center=�  $!@ Proposed Short
 Term Action��$	C 15* B �� *SHORT_ACTION+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 100���� F � ��$G Center=�4  .System.Windows.Controls.DataGridTemplateColumn  $@ Action Due
 Date��$	C 10* _   SortMemberPath$_ SHORT_ACTION_DATE��`   CellTemplate` �� .R��� ���� *SHORT_ACTION_DATE+a �� 	Converter#a [ $+ OneWay=�b   CellEditingTemplateb �� .R�  $��txtShortActionDate��$ Select Date��0 �� *SHORT_ACTION_DATE+$ PropertyChanged=�$+ TwoWay=�  $ @ Proposed Long
 Term Action��$	C 20* B �� *LONG_ACTION+$ PropertyChanged=�$+ TwoWay=�D �� "E ��.��� F � ��$G Center=�H �� "E ��.��� F �@��	MaxLength$	G 100���� F � ��$G Center=�  $@ Action Due
 Date��$	C 10* $_ LONG_ACTION_DATE��` �� .R��� ���� *LONG_ACTION_DATE+#a [ $+ OneWay=�b �� .R�  $��txtLongActionDate��$ Select Date��0 �� *LONG_ACTION_DATE+$ PropertyChanged=�$+ TwoWay=�  $@ 
Trial Date��$	C 10* $_ 
TRIAL_DATE��` �� .R��� ���� *
TRIAL_DATE+#a [ $+ OneWay=�b �� .R�  $��txtTrialDate��$ Select Date��0 �� *
TRIAL_DATE+$ PropertyChanged=�$+ TwoWay=�� $��3�� �� $	��15* ��� $	��20* ��� $	��25* ��� $	��25* � �� $	��15* ��� $��9* ��� $
��0.5* ��� $	��15* ��� $	��16* �.+��� -   $ lblDDRep��$	��Top=�$��0,7,0,0q�$��D&D Rep. : ��$��2��$��0��$ Right=��� -   $ 	lblZAPRep��$	��Top=�$��0,7,0,0q�$��ZAP Rep. : ��$��3��$��0��$ Right=��� -   $ lblComplaints��$ Right=�$(��"Do you have customer complaints?  ��$��0��$��3���� -   $ lblNatureofComplaint��$��Nature of Complaint��$��1��$��3��$
 Left=�$��Bottom=��� -   $I 	txtDndRep��$	��Top=�$��2��$% Please enter D&D representative��$^ 20��$��1��$ Enter D&D Representative��$��2,2,0,2q�$	��152��$
��Left=����� *DevelopmentReportModel.DADRep+$ PropertyChanged=�.��.���� -   $I 	txtZapRep��$% Please enter ZAP Representative��$	��152��$
��Left=�$	��Top=�$��3��$^ 20��$��1��$ Enter ZAP Representative��$��2q����� *DevelopmentReportModel.ZapRep+$ PropertyChanged=�.��.���� -   c ��Name$c chkComplaint��$��4��$��0������ */-DevelopmentReportModel.LabelNatureOfComplaint+$ PropertyChanged=�d d� 	IsCheckedd �� *1/DevelopmentReportModel.IsDoYouHaveCustComplaint+$ PropertyChanged=����� *OnAvailChkCommand+�� -   $I txtCustComplaint��$	��Top=�$��55��$��2��$��3��$��3��$��2��$&  Please enter Nature of Complaint��$��Stretch=�$J Justify=�K . $
L Wrap=�$M Visible=�$N Visible=�$��2q�$
^ 2000��$	��Top=����� **(DevelopmentReportModel.NatureOfComplaint+$ PropertyChanged=�e �� 
IsReadOnlye �� *IsReadonlyComp+$ PropertyChanged=�  �� $! F3��" �� *OnAddCommand+�� $! F5��" �� *OnEditViewCommand+�� $! S��f �� 	Modifiers$f Controlf�" �� *OnSaveCommand+�� $! F8��" �� *OnPrintCommand+�� $! F9��" �� *OnCloseCommand+