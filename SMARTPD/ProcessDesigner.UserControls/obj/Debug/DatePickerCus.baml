   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5        ProcessDesigner.UserControls0   *ProcessDesigner.UserControls.DatePickerCus   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6sys&clr-namespace:System;assembly=mscorlib     5      -   5        $��CusDatePicker��5   3   |�.   +� FocusedElement  6{   ��  �� ElementName$ txtDate��.��5	      �  � ColumnDefinitions 5
   
   �� 5      �� 5      $��23 �6   .+�5   
   �� -   5        $��txtDate��5       �� 	MaxLength$ 10��5   4   
 � Tag 6C   �� $ CusDatePicker�� �� Path$	 Tag��� 5   
    J� IsHitTestVisible .  $��Center=�6�   $
��Left=�6�   $��8,0,0,0q�6�   
���������6-   �� *	WaterMark+$ CusDatePicker�� �� UpdateSourceTrigger$ PropertyChanged=���5      �� 5       �� 
TargetType" ��6   .�5      �� 	 �� Property	 � y�
 �� Value$
 	Collapsed=�63    �� Triggers 5      �� 5       �� Binding 6&   �� *Text+$ txtDate�� �� Value  6T   .a�5      �� 	 � y�$
 Visible=�6;   5      5      5      �� 5   
   -   5         ��Name$ 	ErrorTemp��5   !   $��1��6   y�64   �� *VisibilityErrorTempDate+$ CusDatePicker��$ PropertyChanged=�.+�5      ��  �� Right$	 -12��6   
 �� Top$ 4��6(   ._�5      �� $��*��6   $��15(�6&   $��center=�64   $��center=�6O   $
��Bold"�5   $   ���	#FF930F0F66   5       5!      d� 5#   
   -   5        $��togDate��5#      $��1��6'   ��.  |�. $��20��6�   $��-1q�6�   ���	#FF2196F3��6Z   �� $ txtDate��$ Height���5$      �� 5%      5'      .��5)      � .+�5*      �� $��Images/Calendar.png��6   $
��Fill=�66   5+      5,      � 5-   
   -   5        $��
popContent��5-       � 	StaysOpen .  ��6#   �� *	IsChecked+$ togDate�� �� Mode$ TwoWay=�.��5.      �� -   5        $��
bdrContent��5.      
�������._�5/      �  � RowDefinitions 50      �� 51      �� 52      $��35 �6(   .+�54      &   System.Windows.Controls.Calendar  -   5        $��
calForDate��54         SelectedDate 61   �� *SelectedDate+$ CusDatePicker��$ PropertyChanged=�   DisplayDate 55      �� *SelectedDate+$ CusDatePicker��$ OneWay=�6h   �� 56      -   5        $��btnToday��56   0   |�.  $��1��6@   $��5,0,5,4q�6d   57      58      59      5:      5;      