   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem    ProcessDesigner.UserControls0   *ProcessDesigner.UserControls.DatePickerCus   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6sys&clr-namespace:System;assembly=mscorlib     -   $��CusDatePicker��|�.   +� FocusedElement  ��  �� ElementName$ txtDate��.���  � ColumnDefinitions �� �� $��23 �.+��� -   $��txtDate�� �� 	MaxLength$ 10��
 � Tag �� $ CusDatePicker�� �� Path$	 Tag���  J� IsHitTestVisible .  $��Center=�$
��Left=�$��8,0,0,0q�
����������� *	WaterMark+$ CusDatePicker�� �� UpdateSourceTrigger$ PropertyChanged=�����  �� 
TargetType" ��.��� 	 �� Property	 � y�
 �� Value$
 	Collapsed=� �� Triggers ��  �� Binding �� *Text+$ txtDate�� �� Value  .a��� 	 � y�$
 Visible=��� -    ��Name$ 	ErrorTemp��$��1��y��� *VisibilityErrorTempDate+$ CusDatePicker��$ PropertyChanged=�.+���  �� Right$	 -12��
 �� Top$ 4��._��� $��*��$��15(�$��center=�$��center=�$
��Bold"����	#FF930F0Fd� -   $��togDate��$��1����.  |�. $��20��$��-1q����	#FF2196F3���� $ txtDate��$ Height����� .��� .+��� $��Images/Calendar.png��$
��Fill=�� -   $��
popContent�� � 	StaysOpen .  ���� *	IsChecked+$ togDate�� �� Mode$ TwoWay=�.���� -   $��
bdrContent��
�������._��  � RowDefinitions �� �� $��35 �.+�&   System.Windows.Controls.Calendar  -   $��
calForDate��   SelectedDate �� *SelectedDate+$ CusDatePicker��$ PropertyChanged=�   DisplayDate �� *SelectedDate+$ CusDatePicker��$ OneWay=��� -   $��btnToday��|�.  $��1��$��5,0,5,4q�