   M S B A M L   `   `   `  ���� h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity     ProcessDesigner.frmSapHalb   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 qsi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 RtJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero 
 ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 -   $	��550��$
��1020��$	��800��   WindowBackground#��[  .��� $��4q�  � ColumnDefinitions  �� $	��60* ��� $	��40* � � RowDefinitions �� $��58 ��� $��* �.+��� $��1q�._�� $��0��$��0��  �� $	��400 ��� $��* � �� $	��70* ��� $	��30* �.+��� $��
Part No : ��$��0��$��0��$��right=�$
��Bold"��� $
��Bold"�$	��150��$
��Left=�$��Number of Operations : ��$��
343,37,0,5q�$��2��$��2���� $
��Bold"�$��1��$��Bottom=�$��Center=�$��50��$��	94,0,61,4q�$��2������ *SapModel.NoOfoperations+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�.  (ProcessDesigner.UserControls.ComboBoxCus  -     Name$ 	cmbPartNo��
 � Tag$! Please enter/select Part No��   	MaxLength$ 40��$	��160��   
IsReadOnly .  $��0��$��1��$��2,0,20,0q�   SelectedValuePath$ PART_NO��	   
DataSource	 �� *SapModel.PartnoDetails+$ PropertyChanged=�
 �� NotifyOnValidationError
 .  �� ValidatesOnDataErrors .    SelectedItem �� *SelectedRowPart+$ PropertyChanged=�   SelectedValue �� *SapModel.PartNo+$ PropertyChanged=�   ColumnsHeader �� *DropDownItemsPart+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� * SelectChangeComboCommandPartNo+$ PropertyChanged=� J� InputBindings �� 
 �� Key$ Enter�� �� Command �� *EnterPartNumberCmb+� $��0��$��1��$
��Left=�$��20,0,0,0q�  �� $��* ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� .+��� -   $��
btnRefresh��$��Refresh��$��1q� � ToolTip$ Refresh��$��1��$��1��-  'System.Windows.Input.KeyboardNavigation   	IsTabStop .  $��70��$��50��0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Refresh.png������ *OnRefreshCommand+�� $y�	Collapsed=�$��1q�$��	Save Grid��$��1��$��1��$��70�� .  $��50��$ Images/Save.png������ *OnSaveCommand+�� $��1��$��2��$��1q�$ Export to Text��$��Export�� .  $��Center=�$��70��$��50��$ Images/notepad.png������ *OnExportCommand+�� $��Close��$��1��$��1q�$��3��$��70�� .  $��50��$ 
Close Form��$ Images/Close.png������ *OnCloseCommand+�� $��Halb Details :��$��1��$��0��$��2��$��0,10,0,0q�.��&   System.Windows.Controls.DataGrid     AutoGenerateColumns .     
IsReadOnly .    RowHeaderWidth$ 20Q�   CanUserAddRows .  $
��Left=�$��2q�2	  ,ProcessDesigner.UICommon.ScrollToTopBehavior 	  ScrollToTop . ���� *SapModel.HalbDetails+$ PropertyChanged=�$ TwoWay=�   Columns 0
  *System.Windows.Controls.DataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn   Header$ 	Oper Desc��    Width5  /System.Windows.Controls.DataGridLengthConverter$	  100 1  +System.Windows.Controls.DataGridBoundColumn!   Binding! �� *
Opn_Desc+$ PropertyChanged=�$ TwoWay=�
  $ Material Number��! �� *Material_Number+$ PropertyChanged=�$ TwoWay=�
  $ Material Description��! �� *Material_Description+$ PropertyChanged=�$ TwoWay=�
  $ Base Unit Of
Measure��! �� *Base_Unit_of_Measure+$ PropertyChanged=�$ TwoWay=�
  $ Material
Group��! �� *Material_Group+$ PropertyChanged=�$ TwoWay=�
  $ Division��$	  125 ! �� *
Division+$ PropertyChanged=�$ TwoWay=�
  $ Gross
Weight��$	  150 ! �� *Gross_weight+$ PropertyChanged=�$ TwoWay=�
  $ Weight
Unit��$	  100 ! �� *Weight_Unit+$ PropertyChanged=�$ TwoWay=�
  $ 
Net
Weight��$	  100 ! �� *
Net_weight+$ PropertyChanged=�$ TwoWay=�
  $ MRP
Controller��$	  100 ! �� *MRP_Controller+$ PropertyChanged=�$ TwoWay=�
  $ Procurement
Type��$	  100 ! �� *Procurement_Type+$ PropertyChanged=�$ TwoWay=�
  $ Special
Procurement��$	  100 ! �� *Special_Procurement_Type+$ PropertyChanged=�$ TwoWay=�
  $ Production
Scheduler��$	  100 ! �� *Production_scheduler+$ PropertyChanged=�$ TwoWay=�
  $. (Group of Materials
For Transition Matrix��! �� **(Group_Of_Materials_For_Transition_Matrix+$ PropertyChanged=�$ TwoWay=�
  $ Standard Price��$	  100 ! �� *Standard_Price+$ PropertyChanged=�$ TwoWay=�
  $ Profit Center��$	  100 ! �� *Profit_Center+$ PropertyChanged=�$ TwoWay=�
  $ Spl Proc Costing��$	  100 ! �� *Spl_Proc_Costing+$ PropertyChanged=�$ TwoWay=�