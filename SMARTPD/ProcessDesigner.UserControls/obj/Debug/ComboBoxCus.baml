   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5        ProcessDesigner.UserControls.   (ProcessDesigner.UserControls.ComboBoxCus   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6sys&clr-namespace:System;assembly=mscorlib     5      -   5        $��
ucComboBox��5   6   
�������  +� FocusedElement  5	      ��  �� ElementName$ txtCombobox��.��5
      �  � ColumnDefinitions 5   
   �� 5      �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ 	ErrorTemp��5   !   $��1��6   y�64   �� *VisibilityErrorTemp+$ 
ucComboBox�� �� UpdateSourceTrigger$ PropertyChanged=�.+�5      ��  �� Right$	 -12��6   
 �� Top$ 4��6(   ._�5      �� $��*��6   $��15(�6&   $��center=�64   $��center=�6O   $
��Bold"�5   $   ���	#FF930F0F66   5      5      �� 5   
   -   5        $��txtCombobox��5       ^� ShowOnDisabled . ��6%   �� *SelectedValue+$ 
ucComboBox��$ PropertyChanged=� �� 
IsReadOnly 5      �� *
IsReadOnly+$ 
ucComboBox��$ PropertyChanged=�{�6o   �� *
IsEditable+$ 
ucComboBox��$ PropertyChanged=�	 �� 	MaxLength	 5      �� *	MaxLength+$ 
ucComboBox��$ PropertyChanged=�
 � ToolTip
 5      �� $ txtCombobox�� �� Path$
 Text� �� CharacterCasing 6n   �� *CharacterCasingText+$ 
ucComboBox��$ PropertyChanged=�
 � Tag 5      �� $ 
ucComboBox��$	 Tag�d� 5   
   -   5        $��TogCombobox��5      ��.     ComboBoxToggleButton#���   6>   $��Right=�5      ��5   q   �� $ txtCombobox��$ Height�y�5   .   �� *ButtonVisibility+$ 
ucComboBox��$ PropertyChanged=�� 5   
   -   5        $��
popContent��5       � 	StaysOpen .  ��6"   �� *	IsChecked+$ TogCombobox��.��5      �� -   5        $��
bdrContent��5      $��1q�6(    �� CornerRadius$ 4��6<   
�������._�5      � -   5        $��	grdLookUp��5   �   ��6   �� *DropDownCusWidth+$ 
ucComboBox��$ PropertyChanged=���6u   �� *DropDownCusHeight+$ 
ucComboBox��$ PropertyChanged=� � RowDefinitions 5      �� 5       $
��Auto �6(   68   �� 5!      $��30 �6(   66   �� 5"      $
��Auto �6(   68   �� 5#      $��* �6(   65   .+�5%      �� $��20��6x   $��12(�6�     
HeaderText#��[ 6�   $	��Top=�6�   ��6    �� *
HeaderText+$ 
ucComboBox��$ PropertyChanged=�6�   � 5*      $��1��6    5+      �� 5,      $��* �6,   69    5.      �� 5/      $
��68.5 �6/   6>   �� 50      $	��50* �6/   6=   �� 51      $	��130 �6/   6=   �� 52      $	��50* �6/   6=   .+�54      �� $��Search in : ��6$   $��2,0,0,0q�68   $��0��6I   $��Center=�6Y   $��Right=�6u   $��0��6�   $
��Bold"�6�   6�   �� 55      -   5        $��cmbSearchIn��55   #   $��1��67   $��0��6G   6�   �� 56      $��Enter Text or Value : ��6$   $��2��6B   $��0��6R   $��Center=�6_   $��Right=�6z   $
��Bold"�6�   6�   �� 57      -	   5        $��txtSearchValue��57   "   $��3��68   $��0��6I   $��0,0,10,0q�6V   6�   58      � 59      $��2��6   $��0,0,0,2q�6(   y�69   �� *IsStatusBoxVisible+$ 
ucComboBox��$ PropertyChanged=� �� Mode$ TwoWay=� 5:      �� 5;      $��* �6,   69    5=      �� 5>      $
��68.5 �6/   6>   �� 5?      $	��25* �6/   6=   �� 5@      $	��120 �6/   6=   �� 5A      $	��50* �6/   6=   .+�5C      �� $��	Status : ��6$   $��2,0,0,0q�65   $��0��6F   $��Center=�6V   $��Right=�6r   $��0��6�   $
��Bold"�6�   6�   �� 5D      -
   5        $��	cmbStatus��5D   #   $��1��6_   $��0��6o   .@�5E      �� .��6+   Active63   �� 5F      .��6+   	In-Active66   �� 5G      .��6+   Both61   5H      5I      �� 5J      $��3��6   ._�5K      &   System.Windows.Controls.DataGrid  -   5        $��dgLookup��5K   #   $��0��63   $��0��6C      SelectionUnit$ FullRow=�6�      RowHeaderWidth$ 20Q�6�      CanUserResizeRows .     CanUserAddRows .     SelectionMode$ Single=�5L   I   ��5K   P   �� *
DataSource+$ 
ucComboBox��$ PropertyChanged=� 5L   `   �� *ColumnsHeader+$ 
ucComboBox��$ PropertyChanged=� �� SelectedItem 5M   !   �� *SelectedItem+$ 
ucComboBox��$ PropertyChanged=�   CanUserSortColumns 6�   �� *IsColumnSorting+$ 
ucComboBox��$ PropertyChanged=�5O      5P      5Q      5R      5S      5T      5U      