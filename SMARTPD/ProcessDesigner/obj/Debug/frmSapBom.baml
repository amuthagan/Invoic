   M S B A M L   `   `   `  ����h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  5   	   E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl 5   	   J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon 5   	   W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5	   	   a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes 5
   	   qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	      ProcessDesigner.frmSapBom   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 5   	   qsi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 
 5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     5   	   ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5   	   5UICommon&clr-namespace:ProcessDesigner.UICommon 
 5   	   YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 5	   	   RtJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero 
 5
   	   ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 5   	   XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 5   	   -   5          >�Name$  sapBom��5      $	��600��5   %   $	��800��62   $	��800��6>      WindowBackground#��[  6M   .��5      � $��4q�6    � ColumnDefinitions 5   
   �� 5      $	��65* �6   6-   �� 5      $	��35* �6   6-    � RowDefinitions 5   
   �� 5      $��9* �6   �� 5      $��5* �6   �� 5      $	��80* �6   �� 5      $��5* �6   .+�5   
   �� $��1q�6   ._�5      � $��0��6   $��0��6     5      �� 5      $	��300 �6'   �� 5      $��* �6'    5      �� 5       �� 5!      .+�5#      �� $��
Part No : ��6   $��0��6/   $��0��6=   $��Center=�6M   $��Right=�6h    �� Padding$ 0,0,0,0q�6�   6�   �� 5$      $��	ROH No : ��6   $��1��6.   $��0��6;   $��Center=�6K   $��Right=�6f   $ 0,0,0,0q�6�   6�   .  (ProcessDesigner.UserControls.ComboBoxCus  5&      -   5          Name$ 	cmbPartNo��5&   �   
 � Tag$! Please enter/select Part No��6&      	MaxLength$ 40��6H   $	��160��6W      
IsReadOnly .  $��0��6v   $��1��6�   $��2,0,20,0q�6�      SelectedValuePath$ PART_NO��5'   �   	   
DataSource	 6   �� *SapModel.PartnoDetails+
 �� UpdateSourceTrigger$
 PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors .    SelectedItem 6�   �� *SelectedRowPart+$
 PropertyChanged=�   SelectedValue 6�   �� *SapModel.PartNo+$
 PropertyChanged=�   ColumnsHeader 6I  �� *DropDownItemsPart+$
 PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers 5(      /  )System.Windows.Interactivity.EventTrigger  5)         	EventName$ SelectedItemChanged��6)   .  (System.Windows.Interactivity.TriggerBase   Actions. 5*      6  0System.Windows.Interactivity.InvokeCommandAction     Command 64   �� * SelectChangeComboCommandPartNo+$
 PropertyChanged=�5+       J� InputBindings 5-      �� 5.      
 �� Key$ Enter��6%    �� Command 62   �� *EnterPartNumberCmb+50      5  /ProcessDesigner.UserControls.ComboBoxWithSearch  52      -   5          Name$ cmbROHNo��52   �   $ Please select ROH No��6-      	MaxLength$ 40��6I   $	��160��6X      
IsReadOnly . $��1��6v   $��1��6�   $��2,0,20,0q�6�      SelectedValuePath$	 ROH��55   -      DisplayValuePath$	 ROH��56   -      
DataSource 53      �� *SapModel.RohDetails+$
 PropertyChanged=� .  .    SelectedItem 54   -   �� *SelectedRowRoh+$
 PropertyChanged=�   SelectedValue 57   -   �� *SapModel.RohNo+$
 PropertyChanged=�   SelectedText 58   -   �� *SapModel.RohNo+$
 PropertyChanged=�    ColumnsHeader  59   -   �� *DropDownItemsROH+$
 PropertyChanged=� 5:        5;      $ SelectedItemChanged��6)   . 5<         64   �� *SelectChangeComboCommandRoh+$
 PropertyChanged=�5=      5?      5A      5B      �� 5C   
   $
��Bold"�6   $��Center=�6&   $��245,8.182,124.8,7.717q�6C   $��Number of Operations : ��6b   $��1��6�   $��0��6�   $��Center=�6�   �� 5D   
   $
��Bold"�6   $��Right=�6&   $	��110��6B   $��0,8,10,7q�6N   $��1��6�   $��0��6�   $��Center=�6�   ��6`   �� *SapModel.NoOfoperations+$
 PropertyChanged=�! �� Mode$! TwoWay=�6�   �� 5E   
   $0��*To Operate on the Grids - CTRL + E - EDIT ��6   $��3��6F   $��0��6S   $��Center=�6c   $
��Left=�6~   $��10,0,0,0q�6�   6�   � 5F   
   $��0��6   $��1��6   $
��Left=�6,    5G      �� 5H      $��* �6#   6/   �� 5I      $
��Auto �6#   62   �� 5J      $
��Auto �6#   62   �� 5K      $
��Auto �6#   62    5M      �� 5N      6"   .+�5P      �� -   5        $��
btnRefresh��5P   P   $��Refresh��6   $��1q�6'   $��1��62   $��1��6?   " � ToolTip$" Refresh��6b   -  'System.Windows.Input.KeyboardNavigation#   	IsTabStop# .  $��70��6�   $��50��6�   0  *ProcessDesigner.UserControls.AddedProperty$   Image$$ Images/Refresh.png��6�   ��6�   �� *OnRefreshCommand+6�   �� 5Q      $y�	Collapsed=�6   $��1q�6,   $��	Save Grid��67   $��1��6K   $
" Save��6X   $��70��6g   # .  $��50��6�   $$ Images/Save.png��6�   ��6�   �� *OnSaveCommand+6�   �� 5R      $��1��6   $��Export��6"   $��1q�63   $��2��6>   $" Export to Text��6N   # .  $��70��6�   $��50��6�   $$ Images/notepad.png��6�   $��Center=�6�   ��6�   �� *OnExportCommand+6�   �� 5S      $��Close��6   $��1��6%   $��1q�62   $��70��6>   $��3��6I   # .  $��50��6   $" 
Close Form��6�   $$ Images/Close.png��6�   ��6�   �� *OnCloseCommand+6�   5T      �� 5U   
   -   5        $��groupBox��5U   *   $��BOM Details :��6   $��2��6:   $��0��6G   $��2��6W   .��5V      &	   System.Windows.Controls.DataGrid	  -   5        $��dgvBom��5V      % 	  AutoGenerateColumns% .  & 	  
IsReadOnly& . ' 	  RowHeaderWidth$' 20Q�6r   ( 	  CanUserAddRows( .  $
��Left=�6�   $��2q�6  2
  ,ProcessDesigner.UICommon.ScrollToTopBehavior) 
  ScrollToTop) . ��6�   �� *SapModel.BomDetails+$
 PropertyChanged=�$! TwoWay=� 5W      �� 5X      $ F2��6"    6+   �� *BomEditCommand+* �� CommandParameter* 6N   �� + �� ElementName$+ dgvBom��, �� Path$, SelectedItem��� 5Y      $ E��6"   - �� 	Modifiers$- Controlf�6*    6>   �� *BomEditCommand+* 6a   �� $+ dgvBom��$, SelectedItem�. 	  Columns. 5[      0  *System.Windows.Controls.DataGridTextColumn  5\      ,  &System.Windows.Controls.DataGridColumn/   Header$/ Material Number��6)   0   Width5  /System.Windows.Controls.DataGridLengthConverter$	0 130 6�   1  +System.Windows.Controls.DataGridBoundColumn1   Binding1 6B   �� *Material_Number+$
 PropertyChanged=�$! TwoWay=�6�     5]      $/ Plant��6)   $0 60 6�   1 68   �� *Plant+$
 PropertyChanged=�$! TwoWay=�6�     5^      $/ Key Date��6)   $0 80 6�   2   
Visibility2 6;   �� *DataContext.KeyDateVisible+$+ sapBom��1 6�   �� *
Key_Date+$
 PropertyChanged=�$! TwoWay=�6�     5_      $/ Base
Quantity��6)   $	0 120 6�   1 6E   �� *Base_Quantity+$
 PropertyChanged=�$! TwoWay=�6�     5`      $/ BOM
Component��6)   $	0 130 6�   1 6E   �� *BOM_Component+$
 PropertyChanged=�$! TwoWay=�6�     5a      $/ Component Quantity��6)   $	0 150 6�   1 6E   �� *Component_Quantity+$
 PropertyChanged=�$! TwoWay=�6�     5b      $/ Component Unit
of Measure��6)   $	0 150 6�   1 6Q   �� *Component_Unit_Of_Measure+$
 PropertyChanged=�$! TwoWay=�6�     5c      $2 	Collapsed=�6)   $/ SeqNo��6@   $	0 150 6�   1 6O   �� *SeqNo+$
 PropertyChanged=�$! TwoWay=�6�     5d      $2 	Collapsed=�6)   $/ Input��6@   $	0 150 6�   1 6O   �� *Input+$
 PropertyChanged=�$! TwoWay=�6�   5f      5g      5h      5i      