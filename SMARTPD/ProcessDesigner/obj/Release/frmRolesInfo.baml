   M S B A M L   `   `   `  ����   ProcessDesigner"    ProcessDesigner.frmRolesInfo   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Roles Information��$	��150��$	��300�� � 
ResizeMode$ NoResize=� � Icon$ Images/logo.ico��   WindowBackground#��[   � WindowStartupLocation$ CenterScreen=�.���� $��4q�$��1q� �� CornerRadius$ 3��._�� -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=� � ColumnDefinitions �� $	��100 ��� $��* � � RowDefinitions �� $	��60* ��� $	��40* �.+��� $��1��$��1��
�������$��0,1,0,0q��� $��1��$��0��
�������$��0,1,0,0q��� $��0��$��0�� �� Padding$ 0,0,0,0q�$��Right=��� $��_Role Name : ���� -   $��
tbRoleName��
	 � Tag$	 Please enter Role Name ��$��0��$��25��$	��115��$
��Left=�
 �� 	MaxLength$
 15��$��
2,16,26,15q�$��1�� � ToolTip$ Enter the role name������ *	Role_Name+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� NotifyOnValidationError .  �� ValidatesOnDataErrors . � $��1��$��1�� �� $��* ��� $��* � �� .+��� -   $��btnSave��$��0��$��0��$��70��$��30��$��Right=�$��_Save��$
 Save������ *OnSaveCommand+�� -   $��	btnCancel��$��0��$��1��$��70��$��30��$
��Left=�$��_Cancel��$ Cancel������ *OnCancelCommand+