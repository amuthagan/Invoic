   M S B A M L   `   `   `  ���� h  dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core  E ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes qPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity *   $ProcessDesigner.frmSapProductVersion   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U
 QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 qsi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 
 R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit 
 5UICommon&clr-namespace:ProcessDesigner.UICommon 
 YucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 RtJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero 
 ^ControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 -   $	��600��$	��700��$	��700��.���� $��4q�._��   � ColumnDefinitions  �� $	��60* ��� $	��40* � � RowDefinitions �� $��60 ��� $��* �.+��� $��1q�$��2q�._��   �� $	��70* ��� $	��10* ��� $	��40* ��� $	��10* � �� $	��20* ��� $	��40* ��� $	��40* �.+��� $��
Part No : ��$��1��$��0��$��right=�$
��Bold"�.  (ProcessDesigner.UserControls.ComboBoxCus  -     Name$ 	cmbPartNo��
 � Tag$! Please enter/select Part No��   	MaxLength$ 40��$	��160��   
IsReadOnly .  $��1��$��3��$��1��$��	2,0,20,-1q�   SelectedValuePath$ PART_NO��   
DataSource �� *SapModel.PartnoDetails+ �� UpdateSourceTrigger$ PropertyChanged=�	 �� NotifyOnValidationError	 . 
 �� ValidatesOnDataErrors
 .    SelectedItem �� *SelectedRowPart+$ PropertyChanged=�   SelectedValue �� *SapModel.PartNo+$ PropertyChanged=�   ColumnsHeader �� *DropDownItemsPart+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� * SelectChangeComboCommandPartNo+$ PropertyChanged=� J� InputBindings �� 
 �� Key$ Enter�� �� Command �� *EnterPartNumberCmb+�� -   $��txtNumberOfOperation��$��Number of Operations: ��$��Right=�$	��140��$��3��$��0,5.6,73.723,1q�$��2��$��0��$��Center=�$
��Bold"��� -   $��txtNumberOfOperation1��$��2��$��2��$��Center=�$��25��$��57,7,0,0q�$
��Left=����� *SapModel.NoOfoperations+$ PropertyChanged=� �� Mode$ TwoWay=�� $��0��$��1��$
��Left=�  �� $	��25* ��� $	��25* ��� $	��25* ��� $	��25* � �� .+��� -   $��
btnRefresh��$��Refresh�� � ToolTip$ Refresh��$��1q�$��1��$��1��-  'System.Windows.Input.KeyboardNavigation   	IsTabStop .  $��70��$��50��0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Refresh.png������ *OnRefreshCommand+�� $��1��$ Export to Text��$��2��$��1q�$��Export��$��70�� .  $��50��$ Images/notepad.png������ *OnExportCommand+�� $��Close��$��1��$��3��$��70��$��1q� .  $��50��$ 
Close Form��$ Images/Close.png������ *OnCloseCommand+�� $��Product Version Details :��$��1��$��0��$��2��$��0,10,0,0q�.��&   System.Windows.Controls.DataGrid  $   HorizontalScrollBarVisibility$ Visible=�   RowHeaderWidth$ 20Q�   AutoGenerateColumns .     
IsReadOnly .    CanUserAddRows .  $
��Left=�$��2q�2	  ,ProcessDesigner.UICommon.ScrollToTopBehavior 	  ScrollToTop . ���� *#!SapModel.ProductionVersionDetails+$ PropertyChanged=�$ TwoWay=�   Columns 0
  *System.Windows.Controls.DataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn    Header$  Plant��1  +System.Windows.Controls.DataGridBoundColumn!   Binding! �� *Plant+$ PropertyChanged=�$ TwoWay=�
  $  Material��! �� *
Material+$ PropertyChanged=�$ TwoWay=�
  $  Ver No��! �� *Ver_No+$ PropertyChanged=�$ TwoWay=�
  $  Text1��! �� *Text1+$ PropertyChanged=�$ TwoWay=�
  $  From Lot��! �� *
From_Lot+$ PropertyChanged=�$ TwoWay=�
  $  To Lot��! �� *To_Lot+$ PropertyChanged=�$ TwoWay=�
  $  
Start Date��! �� *
Start_Date+$ PropertyChanged=�$ TwoWay=�
  $  End Date��! �� *
End_Date+$ PropertyChanged=�$ TwoWay=�
  $  PLNTY��! �� *PLNTY+$ PropertyChanged=�$ TwoWay=�
  $  PLNNR��! �� *PLNNR+$ PropertyChanged=�$ TwoWay=�
  $  PLNAL��! �� *PLNAL+$ PropertyChanged=�$ TwoWay=�
  $  STLAL��! �� *STLAL+$ PropertyChanged=�$ TwoWay=�
  $  STLAN��! �� *STLAN+$ PropertyChanged=�$ TwoWay=�
  $  W.Cenre��! �� *	W_Cenre+$ PropertyChanged=�$ TwoWay=�
  $  From SL��! �� *	From_SL+$ PropertyChanged=�$ TwoWay=�
  $  To SL��! �� *To_SL+$ PropertyChanged=�$ TwoWay=�
  $  SERKZ��! �� *SERKZ+$ PropertyChanged=�$ TwoWay=�