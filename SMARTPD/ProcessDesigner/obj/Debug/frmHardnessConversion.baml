   M S B A M L   `   `   `  ����  ProcessDesigner+    %ProcessDesigner.frmHardnessConversion   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  frmHardnessConversion��5   	   $	��500��6'   $	��700��64      WindowBackground#��[  6@    � 	Resources 5       �� 
TargetType  RadioListboxItem �� BasedOn��%0   '��       7  �� 5   
   " ��6   8   6)   5      .��5      �  � ColumnDefinitions 5	   
   �� 5
      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      $	��80* �6   6+   �� 5      $	��20* �6   6,   .+�5   
   �� $��0��6   $��0��6    �� Padding$ 4q�6/   ._�5      �� $��Input Hardness Type��6   .��5      �� -   5        $��LstBox1��5      $��2,5,0,10q�6)   5      5      �� 5   
   $��0��6   $��1��6   $ 4q�6/   ._�5      �� $��Conversion Hardness Type��6   $��0��6:   $��1��6G   .��5      �� -   5        $��LstBox2��5      $��2,5,0,10q�6&   5      5      � 5   
   $��1��6   $��0��6    5      �� 5      6%   �� 5      6%    5!      �� 5"      6"   �� 5#      6"   .+�5%      �� $��Input Value :��6    �� TextAlignment$ Center=�6-   $��0��6D   $��0��6Q   $��Right=�6a    �� Padding$ 0,0,10,0q�6}   6�   �� 5&      $��Range :��6   $ Center=�6'   $��1��6>   $��0��6K   $��Right=�6[   $ 0,0,10,0q�6w   6�   �� 5'      	 �� TextAlignment$
	 Left=�6   $��0��6-   $��1��6:   

 � Tag$
 Please enter Input Value��6J   6k   �� 5(      $
	 Left=�6   $��1��6-   $��1��6:   $
 Please enter Range��6J   6e   5)      � 5*   
   $��1��6   $��1��6   .+�5,      �� $��Equivalent value : ��6   $
 Left=�63   $��Center=�6H   $��1��6d   $��0��6q   $ 10,0,0,0q�6�   6�   5-      5/      51      