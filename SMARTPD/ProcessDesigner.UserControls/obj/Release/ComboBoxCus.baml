   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem    ProcessDesigner.UserControls.   (ProcessDesigner.UserControls.ComboBoxCus   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6sys&clr-namespace:System;assembly=mscorlib     -   $��
ucComboBox��
�������  +� FocusedElement  ��  �� ElementName$ txtCombobox��.���  � ColumnDefinitions �� �� $
��Auto �.+��� -    ��Name$ 	ErrorTemp��$��1��y��� *VisibilityErrorTemp+$ 
ucComboBox�� �� UpdateSourceTrigger$ PropertyChanged=�.+���  �� Right$	 -12��
 �� Top$ 4��._��� $��*��$��15(�$��center=�$��center=�$
��Bold"����	#FF930F0F�� -   $��txtCombobox�� ^� ShowOnDisabled . ���� *SelectedValue+$ 
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
ucComboBox��$	 Tag�d� -   $��TogCombobox����.     ComboBoxToggleButton#���   $��Right=����� $ txtCombobox��$ Height�y��� *ButtonVisibility+$ 
ucComboBox��$ PropertyChanged=�� -   $��
popContent�� � 	StaysOpen .  ���� *	IsChecked+$ TogCombobox��.���� -   $��
bdrContent��$��1q� �� CornerRadius$ 4��
�������._�� -   $��	grdLookUp������ *DropDownCusWidth+$ 
ucComboBox��$ PropertyChanged=����� *DropDownCusHeight+$ 
ucComboBox��$ PropertyChanged=� � RowDefinitions �� $
��Auto ��� $��30 ��� $
��Auto ��� $��* �.+��� $��20��$��12(�  
HeaderText#��[ $	��Top=����� *
HeaderText+$ 
ucComboBox��$ PropertyChanged=�� $��1�� �� $��* � �� $
��68.5 ��� $	��50* ��� $	��130 ��� $	��50* �.+��� $��Search in : ��$��2,0,0,0q�$��0��$��Center=�$��Right=�$��0��$
��Bold"��� -   $��cmbSearchIn��$��1��$��0���� $��Enter Text or Value : ��$��2��$��0��$��Center=�$��Right=�$
��Bold"��� -	   $��txtSearchValue��$��3��$��0��$��0,0,10,0q�� $��2��$��0,0,0,2q�y��� *IsStatusBoxVisible+$ 
ucComboBox��$ PropertyChanged=� �� Mode$ TwoWay=� �� $��* � �� $
��68.5 ��� $	��25* ��� $	��120 ��� $	��50* �.+��� $��	Status : ��$��2,0,0,0q�$��0��$��Center=�$��Right=�$��0��$
��Bold"��� -
   $��	cmbStatus��$��1��$��0��.@��� .��Active�� .��	In-Active�� .��Both�� $��3��._�&   System.Windows.Controls.DataGrid  -   $��dgLookup��$��0��$��0��   SelectionUnit$ FullRow=�   RowHeaderWidth$ 20Q�   CanUserResizeRows .     CanUserAddRows .     SelectionMode$ Single=����� *
DataSource+$ 
ucComboBox��$ PropertyChanged=� �� *ColumnsHeader+$ 
ucComboBox��$ PropertyChanged=� �� SelectedItem �� *SelectedItem+$ 
ucComboBox��$ PropertyChanged=�   CanUserSortColumns �� *IsColumnSorting+$ 
ucComboBox��$ PropertyChanged=�