   M S B A M L   `   `   `  ����E  ABHCustCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:BHCustCtrl;assembly=BHCustCtrl
BHCustCtrl  5   
    ProcessDesigner   ProcessDesigner.frmSNO   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   C
bhCustCtrl,clr-namespace:BHCustCtrl;assembly=BHCustCtrl     5   
   ;toolKit-http://schemas.microsoft.com/wpf/2008/toolkit  5   	     � Title$
  SNO ��5   	   $	��300��6   $	��300��6#    � 	Resources 5         LocationViewSource �� Source �� Path  LocationView��%n   &	        &	 )     �� 5	   
    6:   �� $ DtData��� 5
   
    64   �� $ LstRawMaterial�5      .��5      �� ._�5   
   �  � RowDefinitions 5      �� 5      $	��10* �6    6/   �� 5      $	��90* �6    6/   .+�5      � $��0��6    5      �� 5      $��* �6$   61    � ColumnDefinitions 5      �� 5      $	��20* �6'   65   �� 5      $	��80* �6'   65   .+�5      �� $��Find : ��6   $��0��6+   $��Center=�6;   $��Right=�6W   $��0��6s   6�   �� 5      
 � Tag$ Please enter Find Value��6   $��1��68   $��0��6H   6X   5      � 5       $��1��6    5!      �� 5"      $��* �6$   61    5$      �� 5%      $��* �6'   63   .+�5'      )  #Microsoft.Windows.Controls.DataGrid  $��0��6%   $��0��65   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6      AutoGenerateColumns .  	   	RowHeight$	 25Q�5(   C   $��6,6,6,35q�6R   $��3��6d   ��5'   B   �� 
 �� Source#
 [    Columns 5)      +   %BHCustCtrl.CustDataGridComboBoxColumn  5*      -   5        custComboBoxCol��/  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$	 100 5*   [      Header$ Employee��6g   7  1Microsoft.Windows.Controls.DataGridComboBoxColumn   SelectedValuePath$ 	OPER_CODE��5,   E      DisplayMemberPath$ 	OPER_CODE��5-   E      SelectedValueBinding 5+   B   �� *	OPER_CODE+   ItemsSource 5.   E   �� #
 [     Columns. 5/      3  -Microsoft.Windows.Controls.DataGridTextColumn  $ ContactName��69   4  .Microsoft.Windows.Controls.DataGridBoundColumn   Binding 6N   �� *	OPER_CODE+ �� Mode$ Default=�  50      $ CompanyName��69    6N   �� *	OPER_DESC+$ Default=�51        52      $
 Name��65   $	 550 6o    6C   �� *
RM_DIA_MIN+$ TwoWay=�6o     53      $	 Age��65   $	 450 6n    6B   �� *
RM_DIA_MAX+$ TwoWay=�6n   55      57      58      59      5:      