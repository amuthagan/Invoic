   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5        ProcessDesigner.UserControls5   /ProcessDesigner.UserControls.ComboBoxWithSearch   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6sys&clr-namespace:System;assembly=mscorlib     5      -   5        $��
ucComboBox��5   5   
�������  +� FocusedElement  5	      ��  �� ElementName$ txtCombobox��.��5
      �  � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $
��19.8 �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ 	ErrorTemp��5   !   $��2��6   y�64   �� *VisibilityErrorTemp+$ 
ucComboBox�� �� UpdateSourceTrigger$ PropertyChanged=�.+�5      ��  �� Right$	 -12��6   
 �� Top$ 4��6(   ._�5      �� $��*��6   $��15(�6&   $��center=�64   $��center=�6O   $
��Bold"�5   $   ���	#FF930F0F66   5      5      �� 5   
   -   5        $��txtCombobox��5      $��0��6%    ^� ShowOnDisabled . ��65   �� *SelectedText+$ 
ucComboBox��$ PropertyChanged=� �� 
IsReadOnly 5      �� *
IsReadOnly+$ 
ucComboBox��$ PropertyChanged=�{�6o   �� *
IsEditable+$ 
ucComboBox��$ PropertyChanged=�	 �� 	MaxLength	 5      �� *	MaxLength+$ 
ucComboBox��$ PropertyChanged=�
 � ToolTip
 5      �� $ txtCombobox�� �� Path$
 Text� �� CharacterCasing 6H   �� *CharacterCasingText+$ 
ucComboBox��$ PropertyChanged=�
 � Tag 5      �� $ 
ucComboBox��$	 Tag�.��6D   5      d� 5   
   -   5        $��TogCombobox��5      $��1��6+   ��.     ComboBoxToggleButton#���   6M   $��Right=�5      ��5   �   �� $ txtCombobox��$ Height�y�5   .   �� *ButtonVisibility+$ 
ucComboBox��$ PropertyChanged=�� 5   
   -   5        $��
popContent��5       � 	StaysOpen .  ��6"   �� *	IsChecked+$ TogCombobox��.��5       �� -   5        $��
bdrContent��5       $��1q�6(    �� CornerRadius$ 4��6<   
�������._�5!      � -   5        $��	grdLookUp��5!   �   ��6   �� *DropDownWidth+$ 
ucComboBox��$ PropertyChanged=���6s   �� *DropDownHeight+$ 
ucComboBox��$ PropertyChanged=� � RowDefinitions 5"      �� 5#      $
��Auto �6(   �� 5$      .+�5&      �  5'      �� 5(      �� 5)      $
��Auto �6/   .+�5+      �� -   5        $��txtSearchValue��5+   "   $��4q�6a   |�. 6�   �� 5,      -	   5        $��	WaterMark��5,   =    J� IsHitTestVisible .  $��Center=�6N   $
��Left=�6i   $��8,0,0,0q�6�   
���������5-      �� 5.   "    �� 
TargetType" ��6(   .�5/   &   ��  �� Property � y� �� Value$ 	Collapsed=�6C    �� Triggers 50   &   �� 51   *    �� Binding 66   �� *Text+$ txtSearchValue�� �� Value  6k   .a�52   .   ��  � y�$ Visible=�6K   53   +   55   #   57      �� 58      $��1��6!   
�������$��2q�6E   $ 2��6Y   $��Right=�6j   $��25��6�   $��25��6�   
���  ��$��0,0,3,0q�6�   ._�59      �� -
   5        $��X��59   (   $��Center=�62   $��15(�6O   $
��Bold"�6]   
�������$
 Close��6�   6�   5:      5;      �� 5=      $��1��6   ._�5>      &   System.Windows.Controls.DataGrid  -   5        $��dgLookup��5>   #   $��0��63   $��0��6C      SelectionUnit$ FullRow=�6�      RowHeaderWidth$ 20Q�6�      CanUserResizeRows .     CanUserAddRows .     SelectionMode$ Single=�5?   H   ��5>   P   �� *
DataSource+$ 
ucComboBox��$ PropertyChanged=� 5?   _   �� *ColumnsHeader+$ 
ucComboBox��$ PropertyChanged=� �� SelectedItem 5@   !   �� *SelectedItem+$ 
ucComboBox��$ PropertyChanged=�   CanUserSortColumns 6�   �� *IsColumnsSorting+$ 
ucComboBox��$ PropertyChanged=�5B      5C      � 5D      $��3��6   $y�	Collapsed=�6(    5E      �� 5F      $��* �6,   69    5H      �� 5I      $	��100 �6/   6=   �� 5J      $��* �6/   6;   �� 5K      $	��150 �6/   6=   �� 5L      $��* �6/   6;   .+�5N      �� $��Search in : ��6$   $��0��68   $��Center=�6H   $��Right=�6d   $��0��6�   $
��Bold"�6�   6�   �� 5O      -   5        $��cmbSearchIn��5O   #   $��1��66   $��0��6F   6�   �� 5P      $��Enter Text or Value : ��6$   $��2��6B   $��0��6R   $��Center=�6_   $��Right=�6z   $
��Bold"�6�   6�   5Q      5R      5S      5T      5U      5V      