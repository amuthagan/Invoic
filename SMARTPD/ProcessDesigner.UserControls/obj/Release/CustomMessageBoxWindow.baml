   M S B A M L   `   `   `  ����    ProcessDesigner.UserControls9   3ProcessDesigner.UserControls.CustomMessageBoxWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � WindowStartupLocation$  CenterScreen=� � WindowStyle$
 None=� � ShowInTaskbar .   � Topmost .  � 
ResizeMode$ NoResize=� � SizeToContent$ WidthAndHeight=�&   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�   TextRenderingMode$ 	ClearType=� � UseLayoutRounding . 	 � Title$	  ��$	��155��$	��470��$	��154��
 � 	Resources
  �� 
TargetType �� Property �� Value �� BasedOn��%�   '�       '�� !     �� " �.���  � ��# � ���� " �� Z� .���  � �� �	#FF443B3B��  � ��
 �������  � ��$ 12(���  � ��$ Trebuchet MS)���  � ��$ 3q���  � ��$ 50��.���� $��2q�
��� E��._��  � RowDefinitions �� $��* ��� $
��Auto �.+�� $��0��
�������$��69��.+�]� .+��� $	��Top=� �� Orientation$ 
Horizontal=�#��� ��.+��� -   $��TextBlock_Caption��$
��Wrap=�$
��Auto��$	��200��$
��Left=�$��Center=��� -   $��25��$��25��$
��Bold"�$��X��$��Right=� �� IsCancel .  � ToolTip$ Close the Application��$��	Condensed&��� -   $��Image_MessageBox��$��32��$��32��$
��Left=�$
��Left=�$��30,0,0,0q�$y�	Collapsed=��� -   $��TextBlock_Message��$
��Wrap=�$	��500��$
��Auto��$��Center=�$��12,20,41,15q�� $��1��#��� ��$��39�� � ColumnDefinitions �� $��2* ��� $	��23* �.+�]� $	��5,0q�$��2��.+��� -   $��Button_Cancel��$��88��$	��160��$��26��$	��5,0q�$��Right=�$y�	Collapsed=� . $��Right=�.���� -   $��Label_Cancel��$��0q�$
��10,0q�.��	_Cancel�� -   $��	Button_No��$��88��$	��160��$��26��$	��5,0q�$��Right=�$y�	Collapsed=�$��Right=�.���� -   $��Label_No��$��0q�$
��10,0q�.��_No�� -	   $��
Button_Yes��$��88��$	��160��$��26��$��35,0,5,0q�$��Right=�$y�	Collapsed=�$��Right=�.���� -
   $��	Label_Yes��$��0q�$
��10,0q�.��_Yes�� -   $��	Button_OK��$��88��$	��160��$��35,0,5,0q�$��Right=�$��26��.���� -   $��Label_Ok��$��0q�$
��10,0q�.��_OK