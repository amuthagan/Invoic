   M S B A M L   `   `   `  ����W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  5       ProcessDesigner!   ProcessDesigner.frmInputBox   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     5        � Title$  InputBox��5   	   $	��150��6   $	��350��6'    � Icon$ Images/logo.ico��64    � 
ResizeMode$ NoResize=�6K    � WindowStartupLocation$ CenterScreen=�6a   .��5      �  � RowDefinitions 5   
   �� 5	      $��* �6   �� 5
      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��240 �6   �� 5      $	��100 �6   .+�5   
   �� -   5        $��
lblDisplay��5   b   $��0��6   $��5,0,0,0q�6"   $��0��63   $��Enter The New Tool Code��6C   6b   �� 5   
   -   5         ��Name$ Btn_Save��5   y   $��OK��6   $��0��6   $��1��6+    � ToolTip$ OK��6<   $��Right=�6I   $��0,0,10,0q�6e   $��30��6�   $��80��6�   6�   �� 5   
   -   5        $ 
Btn_Cancel��5   �   $��Cancel��6   $��1��6"   $��1��6/   $ Cancel��6B   $��Right=�6S   $��0,0,10,0q�6o   $��30��6�   $��80��6�   6�   �� 5   
   -   5         ��Name$ Txt_InputBox��5       $��2��6   
	 � Tag$ 	 Please enter New Tool Code��66   $
��Left=�6W   $��2��6�   $��10,0,0,0q�6I  $��0��6\  $	��320��6l  
 �� 	MaxLength
 6�   �� *MaxLengthValue+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�6l  5      5      