   M S B A M L   `   `   `  ����h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  5   	   E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl 5   	   J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon 5   	   W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5   	   a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes 5	   	   qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5
   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	       ProcessDesigner.frmSapHalb   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
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
 5   	   -   5        $	��550��5   %   $
��1020��62   $	��800��6@      WindowBackground#��[  6O   .��5      � $��4q�6     � ColumnDefinitions  5   
   �� 5      $	��60* �6   6-   �� 5      $	��40* �6   6-    � RowDefinitions 5   
   �� 5      $��58 �6   �� 5      $��* �6   .+�5   
   �� $��1q�6   ._�5      � $��0��6   $��0��6      5      �� 5      $	��400 �6'   65   �� 5      $��* �6'   63    5      �� 5      $	��70* �6$   63   �� 5      $	��30* �6$   63   .+�5!      �� $��
Part No : ��6   $��0��6.   $��0��6;   $��right=�6K   $
��Bold"�6g   6{   �� 5#      $
��Bold"�6   $	��150��6.   $
��Left=�6:   $��Number of Operations : ��6U   $��
343,37,0,5q�6t   $��2��6�   $��2��6�   �� 5$      $
��Bold"�6   $��1��6�   $��Bottom=�6�   $��Center=�6�   $��50��6�   $��	94,0,61,4q�6�   $��2��6�   ��6.   �� *SapModel.NoOfoperations+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�6�   .  (ProcessDesigner.UserControls.ComboBoxCus  5&      -   5          Name$ 	cmbPartNo��5&   �   
 � Tag$! Please enter/select Part No��6&      	MaxLength$ 40��6H   $	��160��6W      
IsReadOnly .  $��0��6w   $��1��6�   $��2,0,20,0q�6�      SelectedValuePath$ PART_NO��5'   �   	   
DataSource	 6   �� *SapModel.PartnoDetails+$ PropertyChanged=�
 �� NotifyOnValidationError
 .  �� ValidatesOnDataErrors .    SelectedItem 6�   �� *SelectedRowPart+$ PropertyChanged=�   SelectedValue 6�   �� *SapModel.PartNo+$ PropertyChanged=�   ColumnsHeader 6I  �� *DropDownItemsPart+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers 5(      /  )System.Windows.Interactivity.EventTrigger  5)         	EventName$ SelectedItemChanged��6)   .  (System.Windows.Interactivity.TriggerBase   Actions. 5*      6  0System.Windows.Interactivity.InvokeCommandAction     Command 64   �� * SelectChangeComboCommandPartNo+$ PropertyChanged=�5+       J� InputBindings 5-      �� 5.      
 �� Key$ Enter��6%    �� Command 62   �� *EnterPartNumberCmb+50      51      52      � 54   
   $��0��6   $��1��6   $
��Left=�6,   $��20,0,0,0q�6G     55      �� 56      $��* �6#   6/   �� 57      $
��Auto �6#   62   �� 58      $
��Auto �6#   62   �� 59      $
��Auto �6#   62    5;      �� 5<      6"   .+�5>      �� -   5        $��
btnRefresh��5>   a   $��Refresh��6   $��1q�6'    � ToolTip$ Refresh��62   $��1��6D   $��1��6Q   -  'System.Windows.Input.KeyboardNavigation   	IsTabStop .  $��70��6�   $��50��6�   0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Refresh.png��6�   ��6s   �� *OnRefreshCommand+6
  �� 5?      $y�	Collapsed=�6   $��1q�6,   $��	Save Grid��67   $��1��6K   $��1��6X   $��70��6h    .  $��50��6�   $ Images/Save.png��6�   ��6s   �� *OnSaveCommand+6�   �� 5@      $��1��6   $��2��6"   $��1q�62   $ Export to Text��6=   $��Export��6V    .  $��Center=�6�   $��70��6�   $��50��6�   $ Images/notepad.png��6�   ��6g   �� *OnExportCommand+6�   �� 5A      $��Close��6   $��1��6%   $��1q�62   $��3��6=   $��70��6M    .  $��50��6�   $ 
Close Form��6�   $ Images/Close.png��6�   ��6X   �� *OnCloseCommand+6�   5B      �� 5D   
   $��Halb Details :��6   $��1��6+   $��0��68   $��2��6H   $��0,10,0,0q�6\   .��5E      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     
IsReadOnly .    RowHeaderWidth$ 20Q�6E      CanUserAddRows .  $
��Left=�6�   $��2q�6�   2	  ,ProcessDesigner.UICommon.ScrollToTopBehavior 	  ScrollToTop . ��6p   �� *SapModel.HalbDetails+$ PropertyChanged=�$ TwoWay=�   Columns 5G      0
  *System.Windows.Controls.DataGridTextColumn
  5H      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Oper Desc��6)       Width5  /System.Windows.Controls.DataGridLengthConverter$	  100 6<   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6H   �� *
Opn_Desc+$ PropertyChanged=�$ TwoWay=�
  5I      $ Material Number��6)   ! 6C   �� *Material_Number+$ PropertyChanged=�$ TwoWay=�
  5J      $ Material Description��6)   ! 6H   �� *Material_Description+$ PropertyChanged=�$ TwoWay=�
  5K      $ Base Unit Of
Measure��6)   ! 6M   �� *Base_Unit_of_Measure+$ PropertyChanged=�$ TwoWay=�
  5L      $ Material
Group��6)   ! 6G   �� *Material_Group+$ PropertyChanged=�$ TwoWay=�
  5M      $ Division��6)   $	  125 6;   ! 6H   �� *
Division+$ PropertyChanged=�$ TwoWay=�
  5N      $ Gross
Weight��6)   $	  150 6D   ! 6Q   �� *Gross_weight+$ PropertyChanged=�$ TwoWay=�
  5O      $ Weight
Unit��6)   $	  100 6C   ! 6P   �� *Weight_Unit+$ PropertyChanged=�$ TwoWay=�
  5P      $ 
Net
Weight��6)   $	  100 6B   ! 6O   �� *
Net_weight+$ PropertyChanged=�$ TwoWay=�
  5Q      $ MRP
Controller��6)   $	  100 6F   ! 6S   �� *MRP_Controller+$ PropertyChanged=�$ TwoWay=�
  5R      $ Procurement
Type��6)   $	  100 6H   ! 6U   �� *Procurement_Type+$ PropertyChanged=�$ TwoWay=�
  5S      $ Special
Procurement��6)   $	  100 6K   ! 6X   �� *Special_Procurement_Type+$ PropertyChanged=�$ TwoWay=�
  5T      $ Production
Scheduler��6)   $	  100 6L   ! 6Y   �� *Production_scheduler+$ PropertyChanged=�$ TwoWay=�
  5U      $. (Group of Materials
For Transition Matrix��6)   ! 6a   �� **(Group_Of_Materials_For_Transition_Matrix+$ PropertyChanged=�$ TwoWay=�
  5V      $ Standard Price��6)   $	  100 6A   ! 6N   �� *Standard_Price+$ PropertyChanged=�$ TwoWay=�
  5W      $ Profit Center��6)   $	  100 6@   ! 6M   �� *Profit_Center+$ PropertyChanged=�$ TwoWay=�
  5X      $ Spl Proc Costing��6)   $	  100 6C   ! 6P   �� *Spl_Proc_Costing+$ PropertyChanged=�$ TwoWay=�5[      5\      5]      5^      