   M S B A M L   `   `   `  ���� h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity    ProcessDesigner.frmSapBom   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 qsi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 RtJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero 
 ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 -     >�Name$  sapBom��$	��600��$	��800��$	��800��   WindowBackground#��[  .��� $��4q� � ColumnDefinitions �� $	��65* ��� $	��35* � � RowDefinitions �� $��9* ��� $��5* ��� $	��80* ��� $��5* �.+��� $��1q�._�� $��0��$��0�� �� $	��300 ��� $��* � �� �� .+��� $��
Part No : ��$��0��$��0��$��Center=�$��Right=� �� Padding$ 0,0,0,0q��� $��	ROH No : ��$��1��$��0��$��Center=�$��Right=�$ 0,0,0,0q�.  (ProcessDesigner.UserControls.ComboBoxCus  -     Name$ 	cmbPartNo��
 � Tag$! Please enter/select Part No��   	MaxLength$ 40��$	��160��   
IsReadOnly .  $��0��$��1��$��2,0,20,0q�   SelectedValuePath$ PART_NO��	   
DataSource	 �� *SapModel.PartnoDetails+
 �� UpdateSourceTrigger$
 PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors .    SelectedItem �� *SelectedRowPart+$
 PropertyChanged=�   SelectedValue �� *SapModel.PartNo+$
 PropertyChanged=�   ColumnsHeader �� *DropDownItemsPart+$
 PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� * SelectChangeComboCommandPartNo+$
 PropertyChanged=� J� InputBindings �� 
 �� Key$ Enter�� �� Command �� *EnterPartNumberCmb+5  /ProcessDesigner.UserControls.ComboBoxWithSearch  -     Name$ cmbROHNo��$ Please select ROH No��   	MaxLength$ 40��$	��160��   
IsReadOnly . $��1��$��1��$��2,0,20,0q�   SelectedValuePath$	 ROH��   DisplayValuePath$	 ROH��   
DataSource �� *SapModel.RohDetails+$
 PropertyChanged=� .  .    SelectedItem �� *SelectedRowRoh+$
 PropertyChanged=�   SelectedValue �� *SapModel.RohNo+$
 PropertyChanged=�   SelectedText �� *SapModel.RohNo+$
 PropertyChanged=�    ColumnsHeader  �� *DropDownItemsROH+$
 PropertyChanged=�   $ SelectedItemChanged��.    �� *SelectChangeComboCommandRoh+$
 PropertyChanged=��� $
��Bold"�$��Center=�$��245,8.182,124.8,7.717q�$��Number of Operations : ��$��1��$��0��$��Center=��� $
��Bold"�$��Right=�$	��110��$��0,8,10,7q�$��1��$��0��$��Center=����� *SapModel.NoOfoperations+$
 PropertyChanged=�! �� Mode$! TwoWay=��� $0��*To Operate on the Grids - CTRL + E - EDIT ��$��3��$��0��$��Center=�$
��Left=�$��10,0,0,0q�� $��0��$��1��$
��Left=� �� $��* ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� .+��� -   $��
btnRefresh��$��Refresh��$��1q�$��1��$��1��" � ToolTip$" Refresh��-  'System.Windows.Input.KeyboardNavigation#   	IsTabStop# .  $��70��$��50��0  *ProcessDesigner.UserControls.AddedProperty$   Image$$ Images/Refresh.png������ *OnRefreshCommand+�� $y�	Collapsed=�$��1q�$��	Save Grid��$��1��$
" Save��$��70��# .  $��50��$$ Images/Save.png������ *OnSaveCommand+�� $��1��$��Export��$��1q�$��2��$" Export to Text��# .  $��70��$��50��$$ Images/notepad.png��$��Center=����� *OnExportCommand+�� $��Close��$��1��$��1q�$��70��$��3��# .  $��50��$" 
Close Form��$$ Images/Close.png������ *OnCloseCommand+�� -   $��groupBox��$��BOM Details :��$��2��$��0��$��2��.��&	   System.Windows.Controls.DataGrid	  -   $��dgvBom��% 	  AutoGenerateColumns% .  & 	  
IsReadOnly& . ' 	  RowHeaderWidth$' 20Q�( 	  CanUserAddRows( .  $
��Left=�$��2q�2
  ,ProcessDesigner.UICommon.ScrollToTopBehavior) 
  ScrollToTop) . ���� *SapModel.BomDetails+$
 PropertyChanged=�$! TwoWay=� �� $ F2�� �� *BomEditCommand+* �� CommandParameter* �� + �� ElementName$+ dgvBom��, �� Path$, SelectedItem��� $ E��- �� 	Modifiers$- Controlf� �� *BomEditCommand+* �� $+ dgvBom��$, SelectedItem�. 	  Columns. 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn/   Header$/ Material Number��0   Width5  /System.Windows.Controls.DataGridLengthConverter$	0 130 1  +System.Windows.Controls.DataGridBoundColumn1   Binding1 �� *Material_Number+$
 PropertyChanged=�$! TwoWay=�  $/ Plant��$0 60 1 �� *Plant+$
 PropertyChanged=�$! TwoWay=�  $/ Key Date��$0 80 2   
Visibility2 �� *DataContext.KeyDateVisible+$+ sapBom��1 �� *
Key_Date+$
 PropertyChanged=�$! TwoWay=�  $/ Base
Quantity��$	0 120 1 �� *Base_Quantity+$
 PropertyChanged=�$! TwoWay=�  $/ BOM
Component��$	0 130 1 �� *BOM_Component+$
 PropertyChanged=�$! TwoWay=�  $/ Component Quantity��$	0 150 1 �� *Component_Quantity+$
 PropertyChanged=�$! TwoWay=�  $/ Component Unit
of Measure��$	0 150 1 �� *Component_Unit_Of_Measure+$
 PropertyChanged=�$! TwoWay=�  $2 	Collapsed=�$/ SeqNo��$	0 150 1 �� *SeqNo+$
 PropertyChanged=�$! TwoWay=�  $2 	Collapsed=�$/ Input��$	0 150 1 �� *Input+$
 PropertyChanged=�$! TwoWay=�