   M S B A M L   `   `   `  ����   ProcessDesigner&     ProcessDesigner.frmThreadDetails   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Thread Details��$	��400��$	��800��$	��800��   WindowBackground#��[   � 
ResizeMode$ NoResize=�.����  �� Padding$ 5q�._��  � ColumnDefinitions �� ��  � RowDefinitions �� �� .+��  �� ��  �� $	��40* ��� $��5* ��� $	��55* �.+��� $��0��$��0��$��2��$��Thread Type��.���  �� $	��60* ��� $	��40* � �� $	��20* ��� $	��60* ��� $	��20* �.+�	� -   $��rdbThreadType1��$��External��$��1��$��Center=�$
��Left=�	� -   $��rdbThreadType2��$��Internal��$��1��$��Center=�$��Right=��� $��	Standards��$��2��$��0��$��2��.���  �� $	��69* ��� $	��65* � �� $	��10* ��� $	��30* ��� $	��30* ��� $	��10* �.+�	� -   $��rdbStd1��$��IS��$��1��$��Center=�$
��Left=�	� -   $��rdbStd2��$��BS��$��1��$��Center=�$
��Left=�$��1��	� -   $��rdbStd3��$	��ISO��$��2��$��Center=�$
��Left=�	� -   $��rdbStd4��$	��DIN��$��2��$��Center=�$
��Left=�$��1��� $��1�� �� $	��35* ��� $	��45* ��� $	��20* � �� $	��15* ��� $	��15* ��� $	��15* ��� $	��15* ��� $��5* ��� $	��35* �.+��� $��	Series : ��$��0�� �� TextAlignment$ Right=�$��0���� $��Nominal Dia : ��$��1��$ Right=�$��0���� $��Fit : ��$��2��$ Right=�$��0���� $��Pitch : ��$��3��$ Right=�$��0���� $��Inches��$
��Bold"�$��3��$��2�� �� Padding$ 4,2,0,0q��� $
��Show��$
��Bold"�$��5��$��1���� $��0��$��1��
 � Tag$ Please select Series���� $��1��$��1��$ Please select Normal Dia���� $��2��$��1��$ Please select Fit ���� $��3��$��1��$ Please select Pitch��� $��1��$��2��.+��� $ 0,5,0,0q�._��� $��Three Dimension Output��.���  �� $��1* ��� $	��12* ��� $	��12* ��� $	��12* ��� $	��12* ��� $	��12* ��� $	��12* ��� $	��12* ��� $	��12* ��� $��1* � �� $��2* ��� $	��18* ��� $	��22* ��� $	��22* ��� $	��10* ��� $	��22* ��� $��3* �.+��� -   $��lblHMajarDia��$��1��$��1��$��	Majar Dia��$��2��$
��Bold"�$ Center=��� -   $��lblHPitchDia��$��3��$��1��$��	Pitch Dia��$��2��$
��Bold"�$ Center=��� -	   $��lblHMinorDia��$��5��$��1��$��	Minor Dia��$��2��$
��Bold"�$ Center=��� -
   $��lblHThreadRollingDia��$��7��$��1��$
��Wrap=�$��Thread Rolling Dia��$��2��$
��Bold"�$ Center=��� -   $��
lblMDUpper��$��1��$��2��$��Upper Limit : ��$ Right=��� -   $��
lblMDlower��$��1��$��3��$��Lower Limit : ��$ Right=��� -   $��
lblPDUpper��$��3��$��2��$��Upper Limit : ��$ Right=��� -   $��
lblPDLower��$��3��$��3��$��Lower Limit : ��$ Right=��� -   $��lblMiDUpper��$��5��$��2��$��Upper Limit : ��$ Right=��� -   $��lblMiDLower��$��5��$��3��$��Lower Limit : ��$ Right=��� -   $��lblTRDUpper��$��7��$��2��$��Upper Limit : ��$ Right=��� -   $��lblTRDLower��$��7��$��3��$��Lower Limit : ��$ Right=��� -   $��
txtMDUpper��$��2��$��2��$��25��$( "Please enter Major Dia Upper Limit���� -   $��
txtMDLower��$��2��$��3��$��25��$( "Please enter Major Dia Lower Limit���� -   $��
txtPDUpper��$��4��$��2��$��25��$( "Please enter Pitch Dia Upper Limit���� -   $��
txtPDLower��$��4��$��3��$��25��$( "Please enter Pitch Dia Lower Limit���� -   $��txtMiDUpper��$��6��$��2��$��25��$( "Please enter Minor Dia Upper Limit���� -   $��txtMiDLower��$��6��$��3��$��25��$( "Please enter Minor Dia Lower Limit���� -   $��txtTRDUpper��$��8��$��2��$��25��$2 ,Please enter Thread Rolling Dia Upper Limit ���� -   $��txtTRDLower��$��8��$��3��$��25��$1 +Please enter Thread Rolling Dia Lower Limit���� -   $��
lblRemarks��$��1��$��5��$��
Remarks : ��$ Right=��� -   $��
txtRemarks��$��2��$��5��$��7��$��25��$ Please enter Remarks��