   M S B A M L   `   `   `  ����h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  5   	   E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl 5   	   J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon 5   	   W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5   	   a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes 5	   	   qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5
   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	   *   $ProcessDesigner.frmSapProductVersion   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 5   	   qsi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 
 5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     5   	   ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5   	   5UICommon&clr-namespace:ProcessDesigner.UICommon 
 5   	   YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 5   	   RtJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero 
 5	   	   ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 5
   	   XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 5   	   -   5        $	��600��5   %   $	��700��62   $	��700��6>   .��5      �� $��4q�6   ._�5   
   �   � ColumnDefinitions  5      �� 5      $	��60* �6#   61   �� 5      $	��40* �6#   61    � RowDefinitions 5      �� 5      $��60 �6    �� 5      $��* �6    .+�5      �� $��1q�6   $��2q�6)   ._�5      �   5      �� 5      $	��70* �6+   69   �� 5      $	��10* �6+   69   �� 5      $	��40* �6+   69   �� 5      $	��10* �6+   69    5       �� 5!      $	��20* �6(   67   �� 5"      $	��40* �6(   67   �� 5#      $	��40* �6(   67   .+�5%      �� $��
Part No : ��6    $��1��62   $��0��6?   $��right=�6O   $
��Bold"�6k   .  (ProcessDesigner.UserControls.ComboBoxCus  5'      -   5          Name$ 	cmbPartNo��5'   �   
 � Tag$! Please enter/select Part No��6*      	MaxLength$ 40��6L   $	��160��6[      
IsReadOnly .  $��1��6{   $��3��6�   $��1��6�   $��	2,0,20,-1q�6�      SelectedValuePath$ PART_NO��5(   �      
DataSource 6   �� *SapModel.PartnoDetails+ �� UpdateSourceTrigger$ PropertyChanged=�	 �� NotifyOnValidationError	 . 
 �� ValidatesOnDataErrors
 .    SelectedItem 6�   �� *SelectedRowPart+$ PropertyChanged=�   SelectedValue 6�   �� *SapModel.PartNo+$ PropertyChanged=�   ColumnsHeader 6I  �� *DropDownItemsPart+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers 5)      /  )System.Windows.Interactivity.EventTrigger  5*         	EventName$ SelectedItemChanged��6-   .  (System.Windows.Interactivity.TriggerBase   Actions. 5+   "   6  0System.Windows.Interactivity.InvokeCommandAction     Command 68   �� * SelectChangeComboCommandPartNo+$ PropertyChanged=�5,       J� InputBindings 5.      �� 5/      
 �� Key$ Enter��6)    �� Command 66   �� *EnterPartNumberCmb+51      �� 52      -   5        $��txtNumberOfOperation��52   �   $��Number of Operations: ��6    $��Right=�6>   $	��140��6Z   $��3��6f   $��0,5.6,73.723,1q�6z   $��2��6�   $��0��6�   $��Center=�6�   $
��Bold"�6�   6�   �� 53      -   5        $��txtNumberOfOperation1��53   �   $��2��6z   $��2��6�   $��Center=�6�   $��25��6�   $��57,7,0,0q�6�   $
��Left=�6�   ��6    �� *SapModel.NoOfoperations+$ PropertyChanged=� �� Mode$ TwoWay=�6�   54      55      � 56      $��0��6   $��1��6    $
��Left=�60     57      �� 58      $	��25* �6'   65   �� 59      $	��25* �6'   65   �� 5:      $	��25* �6'   65   �� 5;      $	��25* �6'   65    5=      �� 5>      6&   .+�5@      �� -   5        $��
btnRefresh��5@   e   $��Refresh��6    � ToolTip$ Refresh��6+   $��1q�6=   $��1��6H   $��1��6U   -  'System.Windows.Input.KeyboardNavigation   	IsTabStop .  $��70��6�   $��50��6�   0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Refresh.png��6�   ��6�   �� *OnRefreshCommand+6�   �� 5A      $��1��6   $ Export to Text��6&   $��2��6?   $��1q�6O   $��Export��6Z   $��70��6k    .  $��50��6�   $ Images/notepad.png��6�   ��6�   �� *OnExportCommand+6�   �� 5B      $��Close��6   $��1��6)   $��3��66   $��70��6F   $��1q�6Q    .  $��50��6�   $ 
Close Form��6�   $ Images/Close.png��6�   ��6�   �� *OnCloseCommand+6�   5C      �� 5E      $��Product Version Details :��6   $��1��6:   $��0��6G   $��2��6W   $��0,10,0,0q�6k   .��5F      &   System.Windows.Controls.DataGrid  $   HorizontalScrollBarVisibility$ Visible=�6      RowHeaderWidth$ 20Q�6D      AutoGenerateColumns .     
IsReadOnly .    CanUserAddRows .  $
��Left=�6  $��2q�6"  2	  ,ProcessDesigner.UICommon.ScrollToTopBehavior 	  ScrollToTop . ��6�   �� *#!SapModel.ProductionVersionDetails+$ PropertyChanged=�$ TwoWay=�   Columns 5H      0
  *System.Windows.Controls.DataGridTextColumn
  5I      ,  &System.Windows.Controls.DataGridColumn    Header$  Plant��6-   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6<   �� *Plant+$ PropertyChanged=�$ TwoWay=�
  5J      $  Material��6-   ! 6?   �� *
Material+$ PropertyChanged=�$ TwoWay=�
  5K      $  Ver No��6-   ! 6=   �� *Ver_No+$ PropertyChanged=�$ TwoWay=�
  5L      $  Text1��6-   ! 6<   �� *Text1+$ PropertyChanged=�$ TwoWay=�
  5M      $  From Lot��6-   ! 6?   �� *
From_Lot+$ PropertyChanged=�$ TwoWay=�
  5N      $  To Lot��6-   ! 6=   �� *To_Lot+$ PropertyChanged=�$ TwoWay=�
  5O      $  
Start Date��6-   ! 6A   �� *
Start_Date+$ PropertyChanged=�$ TwoWay=�
  5P      $  End Date��6-   ! 6?   �� *
End_Date+$ PropertyChanged=�$ TwoWay=�
  5Q      $  PLNTY��6-   ! 6<   �� *PLNTY+$ PropertyChanged=�$ TwoWay=�
  5R      $  PLNNR��6-   ! 6<   �� *PLNNR+$ PropertyChanged=�$ TwoWay=�
  5S      $  PLNAL��6-   ! 6<   �� *PLNAL+$ PropertyChanged=�$ TwoWay=�
  5T      $  STLAL��6-   ! 6<   �� *STLAL+$ PropertyChanged=�$ TwoWay=�
  5U      $  STLAN��6-   ! 6<   �� *STLAN+$ PropertyChanged=�$ TwoWay=�
  5V      $  W.Cenre��6-   ! 6>   �� *	W_Cenre+$ PropertyChanged=�$ TwoWay=�
  5W      $  From SL��6-   ! 6>   �� *	From_SL+$ PropertyChanged=�$ TwoWay=�
  5X      $  To SL��6-   ! 6<   �� *To_SL+$ PropertyChanged=�$ TwoWay=�
  5Y      $  SERKZ��6-   ! 6<   �� *SERKZ+$ PropertyChanged=�$ TwoWay=�5\      5]      5^      5_      5a      