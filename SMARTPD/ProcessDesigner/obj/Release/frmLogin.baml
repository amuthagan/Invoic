   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem   ProcessDesigner   ProcessDesigner.frmLogin   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9System&clr-namespace:System;assembly=mscorlib     -     � Title$  frmLogin��$	��380��$	��500��
������� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � WindowStyle$
 None=� � Icon$ Images/logo.ico�� +� FocusedElement ��  �� ElementName$ Txtusername��.���� $��1q�   NormalBorderBrush#���   ._��  � ColumnDefinitions �� $	�� 40 ��� $�� * ��� $	�� 40 � � RowDefinitions �� $��45 ��� $	��60* ��� $��20 ��� $	��25* �.+��� $��3��._��� $��Center=�$��0��$��2��$��10,1,0,1q�$��Images/logo-04.png��$
��Left=�$	��180���� 
�������$��2��$��20��$��20���� -   $��2��$��Uniform=�$��30��$��25������ 	 �� 
TargetType"	 ��.��� 
 �� Property
 � �� �� Value$ Images/CloseLogin.png�� �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ��� 
 � ��$ Images/CloseHover.png���� $��Login�����	#FFF6FFFE$��1��$��1��$	��350��$	��210��$��Center=�$��
15,10,9,12q�$��2��.���  �� $��40 ��� $��70 ��� $��* ��� $��40 � �� $��50 ��� $��40 ��� $��30 ��� $��15 ��� $��40 ��� $��* �.+��� $��1q�$��4��$��	User Type��$��Center=�$��0,2,0,2q���Z� .���  �� $��* ��� $��* ��� $��* �.+�(� $��16��.��	� -   $��rbtOQA��$	��OQA�� d� 	IsChecked$ FalseY� 	� 	GroupName$ UserType��(� $��16��$��1��.��	� -   $��rbtDD��$��PD��$
 TrueY�$ UserType���� $��	User Name��$��1��$��Center=�$��1���� $��Password��$��1��$��Center=�$��2���� -    ��Name$ Txtusername��$��2��-  'System.Windows.Input.KeyboardNavigation   TabIndex$ 0��$��1��$��Normal$�$
��Bold"� �� 	MaxLength$ 15��$��Center=�$��Center=�$��22���� $��2��$��1�� J� IsHitTestVisible .  $��Enter User Name��$��Center=�$
��Left=�$��5,0,0,0q�
����������� "	 ��.��� 
 � y�$ 	Collapsed=� ��  �� Binding �� *Text+$ Txtusername�� �� Value  .a��� 
 � y�$ Visible=�G� -    G�Name$ TxtPassword��$��2��$ 1��$��2��$��Normal$�$
��Bold"� G� 	MaxLength$ 49��$��Center=�$��Center=�$��22���� -   $��lblPasswater��$��2��$��2�� .  $��Enter Password��$��Center=�$
��Left=�$��5,0,0,0q�
����������� "	 ��.��� 
 � y�$ 	Collapsed=� ��  �� *
Password+$ TxtPassword��  .a��� 
 � y�$ Visible=�� -   $��grdCapsLock��$��3��$��2�� �� $��15 ��� .+�� $��15��$��0��.+��  � Points �   @KL  �  �  � 
��� ���
��� ����� $��!��
���  ��$��4,0,0,0q�$��8(��� $��CapsLock is On��$��9(�$��1��� $��2��$��4��.+��  �� $	��50* ��� $	��50* �.+��� -	   $��btnOk��$��80��$��30��$��Login��$ 2�� � ToolTip$ Login���� -
   $��btnClose��$��80��$��30��$��Cancel��$ 3��$��1��$��0�� �� IsCancel . $ Cancel��$��	Condensed&�� $��3��$��3�� �� $	��50* ��� $	��25* ��� $	��25* � �� $��20 ��� $��5 ��� �� �� �� $��12 �.+��� $��3��$	��Top=� �� Stretch$
 Fill=�#���   	 �� X2$ 1��$��2���� $��10(�$��Bottom=�$��Last Updated Date:�� �� Padding$ 10,0,0,2q�$��0��$��0���� $
��Bold"�$��10(�$��Developed By :��$ 10,0,0,2q�$��2��$��0���� $��10(�$ ��Sundram Fasteners Limited,��$ 10,0,0,2q�$��3��$��0���� $��10(�$+��%Padi, Chennai & TVS Infotech,Chennai ��$ 10,0,0,2q�$��4��$��0���� $�� ��$ 10,0,0,2q�$��5��$��0���� $�� ��$ 10,0,0,2q�$��6��$��0���� -   $��txtLastUpdate��$��10(�$��Bottom=�$�� ��$ 	100,0,0,2q�$��2��$��0��$��0��