   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem    ProcessDesigner.UserControls5   /ProcessDesigner.UserControls.ComboBoxWithSearch   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6sys&clr-namespace:System;assembly=mscorlib     -   $��
ucComboBox��
�������  +� FocusedElement  ��  �� ElementName$ txtCombobox��.���  � ColumnDefinitions �� $��1* ��� $
��19.8 ��� $
��Auto �.+��� -    ��Name$ 	ErrorTemp��$��2��y��� *VisibilityErrorTemp+$ 
ucComboBox�� �� UpdateSourceTrigger$ PropertyChanged=�.+���  �� Right$	 -12��
 �� Top$ 4��._��� $��*��$��15(�$��center=�$��center=�$
��Bold"����	#FF930F0F�� -   $��txtCombobox��$��0�� ^� ShowOnDisabled . ���� *SelectedText+$ 
ucComboBox��$ PropertyChanged=� �� 
IsReadOnly �� *
IsReadOnly+$ 
ucComboBox��$ PropertyChanged=�{��� *
IsEditable+$ 
ucComboBox��$ PropertyChanged=�	 �� 	MaxLength	 �� *	MaxLength+$ 
ucComboBox��$ PropertyChanged=�
 � ToolTip
 �� $ txtCombobox�� �� Path$
 Text� �� CharacterCasing �� *CharacterCasingText+$ 
ucComboBox��$ PropertyChanged=�
 � Tag �� $ 
ucComboBox��$	 Tag�.��d� -   $��TogCombobox��$��1����.     ComboBoxToggleButton#���   $��Right=����� $ txtCombobox��$ Height�y��� *ButtonVisibility+$ 
ucComboBox��$ PropertyChanged=�� -   $��
popContent�� � 	StaysOpen .  ���� *	IsChecked+$ TogCombobox��.���� -   $��
bdrContent��$��1q� �� CornerRadius$ 4��
�������._�� -   $��	grdLookUp������ *DropDownWidth+$ 
ucComboBox��$ PropertyChanged=����� *DropDownHeight+$ 
ucComboBox��$ PropertyChanged=� � RowDefinitions �� $
��Auto ��� .+��  �� �� $
��Auto �.+��� -   $��txtSearchValue��$��4q�|�. �� -	   $��	WaterMark�� J� IsHitTestVisible .  $��Center=�$
��Left=�$��8,0,0,0q�
�����������  �� 
TargetType" ��.���  �� Property � y� �� Value$ 	Collapsed=� �� Triggers ��  �� Binding �� *Text+$ txtSearchValue�� �� Value  .a���  � y�$ Visible=��� $��1��
�������$��2q�$ 2��$��Right=�$��25��$��25��
���  ��$��0,0,3,0q�._��� -
   $��X��$��Center=�$��15(�$
��Bold"�
�������$
 Close���� $��1��._�&   System.Windows.Controls.DataGrid  -   $��dgLookup��$��0��$��0��   SelectionUnit$ FullRow=�   RowHeaderWidth$ 20Q�   CanUserResizeRows .     CanUserAddRows .     SelectionMode$ Single=����� *
DataSource+$ 
ucComboBox��$ PropertyChanged=� �� *ColumnsHeader+$ 
ucComboBox��$ PropertyChanged=� �� SelectedItem �� *SelectedItem+$ 
ucComboBox��$ PropertyChanged=�   CanUserSortColumns �� *IsColumnsSorting+$ 
ucComboBox��$ PropertyChanged=�� $��3��$y�	Collapsed=� �� $��* � �� $	��100 ��� $��* ��� $	��150 ��� $��* �.+��� $��Search in : ��$��0��$��Center=�$��Right=�$��0��$
��Bold"��� -   $��cmbSearchIn��$��1��$��0���� $��Enter Text or Value : ��$��2��$��0��$��Center=�$��Right=�$
��Bold"�