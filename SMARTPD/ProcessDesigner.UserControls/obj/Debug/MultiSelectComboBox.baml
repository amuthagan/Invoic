   M S B A M L   `   `   `  ����   ProcessDesigner.UserControls6    0ProcessDesigner.UserControls.MultiSelectComboBox   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � 	Resources  5         NormalBorderBrush �� EndPoint �� 
StartPoint  NormalBrushGradient1 � Color  NormalBrushGradient3 � Offset��%�   &	        7  7  �� 5   
   $ 0.5,0&�68   $ 0.5,1&�6I   .��5      � 8   6   � 5      8  $ 1Q�6I   5      5	      .��5
      �� -   5         ��Name$ MultiSelectCombo��5   	    J� SnapsToDevicePixels .  � OverridesDefaultStyle . $
��Auto=�5   	   $
��Auto=�5   	   ��. $ �� IsSynchronizedWithCurrentItem$
 TrueY�5   	   ��5   
   �� 5      .R�5      �� -   5        ��5      �� *Title+	 d� 	IsChecked	 5      �� 
 �� Path$
 
IsSelected� �� Mode$ TwoWay=�
 � Tag 5      �� *FindAncestor+ �� AncestorType" ��5      5      ��5   
   �� 5       �� 
TargetType" ��6   .R�5      � .+�5      d�  d�Name$ ToggleButton��5      $��2��5       |�.   �� 	ClickMode$ Press=�5"      $
��Left=�6+   	 5    (   �� $
 IsDropDownOpen�$ TwoWay=� �� RelativeSource �� *TemplatedParent+��5#      �� 5$      .R�5%   "   �  � ColumnDefinitions 5&   &   �� 5'   *   $��* �6;   �� 5(   *   $��18 �6;   .+�5*   &   ��  ��Name$ Border��5+      $��2��5,       �� CornerRadius$ 2��5-      
�������#��[  5/      $��1,1,1,1q�50      �� 51   &   $ 
BorderComp��52      $��0��53      $ 2��54      $��1q�55      
�������#��[  57      $��0,0,0,0q�58      ._�59   *   �� 
������� �� Padding$ 3q�6�   ��64   �� $

 Text� ��  �� Mode$ FindAncestor=�$ UserControlL�6�   5:   '   F� 5;   &    F�Name$ Arrow��5<      $��1��5=      
���   �$��Center=�5?      $��Center=�5@      "����        � 	=  	= � z     	5A      5B   #   5C      5E      � 5F      $��Popup��5G      $��Bottom=�5H       � AllowsTransparency . |�.   �� IsDropDownOpen#��z 5J   ,   $��Slide=�5K      .��5L      � $��DropDown��5M   #    .  � ActualWidth#��z 5O       �� MaxDropDownHeight#��z 5P   #   .+�5Q      �� $ DropDownBorder��5R   %   $��1q�5S   $   
�������#��[  5T   %   �� 5U      $��4,6,4,6q�6+    .  � DataContext 6W   �� .��5V   "   ��  K� IsItemsHost . -  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation$ 	Contained=�6@   5W      5X      5Y      5Z       �� Triggers 5[      P� 5\        P� Property  �@��HasItems! P� Value! .  . �5]      �� " �� 
TargetName$" DropDownBorder��6!   # �� Property# � ��$ �� Value$$ 95��6R   5^      P� 5_      % P� 
SourceName$% Popup��6     �@�AllowsTransparency! . . �5`      �� $" DropDownBorder��6!   # �@��CornerRadius$$ 4��6U   �� 5a      $" DropDownBorder��6!   # � ��$$ 0,2,0,0q�6O   5b      5d      5f      5g      