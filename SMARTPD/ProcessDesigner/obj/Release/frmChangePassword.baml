   M S B A M L   `   `   `  ����   ProcessDesigner'    !ProcessDesigner.frmChangePassword   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  SmartPD - Change Password��$	��190��$	��400��   WindowBackground#��[   � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � Icon$ Images/logo.ico��.���� $��5q�$��Change Password��.��� -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=� � RowDefinitions �� $	��25* ��� $	��25* ��� $	��25* ��� $	��25* � � ColumnDefinitions �� $	��35* ��� $	��50* ��� $	��15* �.+��� $��Old Password: ��$��0��$��Center=�$��Right=�$��0���� -   $��errOld��$��*��$��5,10,5,5q�$��15(�$��3��$
��Left=�$
��Bold"����	#FF930F0FG� -    G�Name$ txtOldPassword�� G� 	MaxLength$ 49��$��1��$��0��
	 � Tag$	 Please enter Old Password���� $��New Password: ��$��0��$��Center=�$��Right=�$��1���� -   $��errNew��$��*��$��5,10,5,5q�$��15(�$��3��$��1��$
��Left=�$
��Bold"����	#FF930F0FG� -   $ txtNewPassword��$ 49��$��1��$��1��$	 Please enter New Password���� $��Verify Password: ��$��0��$��Center=�$��Right=�$��2���� -   $��errVer��$��*��$��5,10,5,5q�$��15(�$��3��$��2��$
��Left=�$
��Bold"����	#FF930F0FG� -   $ txtVerifyPassword��$ 49��$��1��$��2��$	 Please Re-enter password���� -   
 ��Name$
 btnOk�� � ToolTip$ OK��$	��_OK��$��5,0,0,0q�$��3��$��1��$��85��$��28��$��Center=�$
��Left=��� -	   $
 	btnCancel��$ Cancel��$��_Cancel��$��95,0,0,0q�$��3��$��2��$��1��$��85��$��28��$
��Left=�