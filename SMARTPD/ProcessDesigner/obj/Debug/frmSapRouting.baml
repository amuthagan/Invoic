   M S B A M L   `   `   `  ����h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  5   	   E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl 5   	   J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon 5   	   W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5   	   a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes 5	   	   qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5
   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	   #   ProcessDesigner.frmSapRouting   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 5      qsi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 
 5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     5   	   ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5   	   5UICommon&clr-namespace:ProcessDesigner.UICommon 
 5   	   YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 5   	   RtJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero 
 5	   	   ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 5
   	   XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 5   	   -   5        $	��600��5   %   $	��700��62   $	��700��6?      WindowBackground#��[  6N   .��5      �� $��4q�6   ._�5   
   �   � ColumnDefinitions  5      �� 5      $	��60* �6#   61   �� 5      $	��40* �6#   61    � RowDefinitions 5      �� 5      $��60 �6    �� 5      $��* �6    .+�5      �� $��1q�6   $��2q�6)   ._�5      �   5      �� 5      $	��70* �6+   69   �� 5      $	��10* �6+   69   �� 5      $	��40* �6+   69   �� 5      $	��10* �6+   69    5       �� 5!      $	��50* �6(   67   �� 5"      $	��40* �6(   67   �� 5#      $	��10* �6(   67   .+�5&      �� $��
Part No : ��6    $��0��62   $��0��6?   $��right=�6P   $
��Bold"�6l   6�   .  (ProcessDesigner.UserControls.ComboBoxCus  5(      -   5          Name$ 	cmbPartNo��5(   �   
 � Tag$" Please enter/select Part No ��6*      	MaxLength$ 40��6M   $	��160��6\      
IsReadOnly .  $��0��6{   $��3��6�   $��1��6�   $��2,0,20,0q�6�      SelectedValuePath$ PART_NO��5)   �      
DataSource 6   �� *SapModel.PartnoDetails+ �� UpdateSourceTrigger$ PropertyChanged=�	 �� NotifyOnValidationError	 . 
 �� ValidatesOnDataErrors
 .    SelectedItem 6�   �� *SelectedRowPart+$ PropertyChanged=�   SelectedValue 6�   �� *SapModel.PartNo+$ PropertyChanged=�   ColumnsHeader 6I  �� *DropDownItemsPart+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers 5*      /  )System.Windows.Interactivity.EventTrigger  5+         	EventName$ SelectedItemChanged��6-   .  (System.Windows.Interactivity.TriggerBase   Actions. 5,   "   6  0System.Windows.Interactivity.InvokeCommandAction     Command 68   �� * SelectChangeComboCommandPartNo+$ PropertyChanged=�5-       J� InputBindings 5/      �� 50      
 �� Key$ Enter��6)    �� Command 66   �� *EnterPartNumberCmb+52      �� 54      -   5        $��txtNumberOfOperation��54   �   $
��Bold"�6    $��Number of Operations : ��62   $��1��6Q   $��0��6^   $��3��6n   $��Center=�6�   $��Right=�6�   $��0,0,69.723,7.6q�6�   6�   �� 55      -   5        $��txtNumberOfOperation1��55   �   $
��Bold"�6    $��1��6�   $��2��6�   $��50��6�   $��Right=�6�   $��0,0,15,7q�6�   ��62   �� *SapModel.NoOfoperations+$ PropertyChanged=� �� Mode$ TwoWay=�6  56      57      � 5:      $��0��6   $��1��6    $
��Left=�60     5;      �� 5<      $��* �6'   63   �� 5=      $
��Auto �6'   66   �� 5>      $
��Auto �6'   66   �� 5?      $
��Auto �6'   66    5A      �� 5B      6&   .+�5D      �� -   5        $��
btnRefresh��5D   e   $��Refresh��6   $��1q�6+    � ToolTip$ Refresh��66   $��1��6H   $��1��6U   -  'System.Windows.Input.KeyboardNavigation   	IsTabStop .  $��70��6�   $��50��6�   0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Refresh.png��6�   ��6�   �� *OnRefreshCommand+6�   �� 5E      $��1��6   $��1q�6'   $ Export to Text��62   $��2��6K   $	��100��6[   $��Export to Text��6g    .  $��70��6�   $ Images/notepad.png��6�   ��6�   �� *OnExportCommand+6�   �� 5F      $��Close��6   $��1��6)   $��1q�67   $��3��6B   $��70��6R    .  $��50��6�   $ 
Close Form��6�   $ Images/Close.png��6�   ��6�   �� *OnCloseCommand+6�   5G      �� 5I      $��1��6   $��0��6$   $��2��64   $��0,10,0,0q�6H   $��Routing Details :��6Z   .��5J      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     
IsReadOnly .    RowHeaderWidth$ 20Q�6J      CanUserAddRows .  $
��Left=�6�   $��2q�6�   2	  ,ProcessDesigner.UICommon.ScrollToTopBehavior 	  ScrollToTop . ��6u   �� *SapModel.RoutingDetails+$ PropertyChanged=�$ TwoWay=�   Columns 5L      0
  *System.Windows.Controls.DataGridTextColumn
  5M      ,  &System.Windows.Controls.DataGridColumn   Header$ Material Number��6-   1  +System.Windows.Controls.DataGridBoundColumn    Binding  6F   �� *Material_Number+$ PropertyChanged=�$ TwoWay=�
  5N      $ Plant��6-     6<   �� *Plant+$ PropertyChanged=�$ TwoWay=�
  5O      $ Work Center��6-     6B   �� *Work_Center+$ PropertyChanged=�$ TwoWay=�
  5P      $ Standard Text Key��6-     6H   �� *Standard_text_key+$ PropertyChanged=�$ TwoWay=�
  5Q      $ Base Quantity��6-     6D   �� *Base_Quantity+$ PropertyChanged=�$ TwoWay=�
  5R      $ CC-UOM��6-     6=   �� *CC-UOM+$ PropertyChanged=�$ TwoWay=�
  5S      $ 	Qty in Pc��6-     6@   �� *	Qty_in_Pc+$ PropertyChanged=�$ TwoWay=�
  5T      $ 	Qty in Kg��6-   !   Width5  /System.Windows.Controls.DataGridLengthConverter$	! 125 6@     6L   �� *	Qty_in_Kg+$ PropertyChanged=�$ TwoWay=�5V      5W      5X      5Y      5Z      