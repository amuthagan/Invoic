   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon     ProcessDesigner.frmGrinmac   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ccommonPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     ;UICommon&clr-namespace:ProcessDesigner.UICommon       � Title$  Fastner Process Designer��$	��355��$	��600��   WindowBackground#��[   � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � 	Resources /  )ProcessDesigner.UICommon.DecimalConverter  DecimalConverter��%   &	         .���� $��5q�$��1q�._��  � RowDefinitions �� $��25 ��� $��25 ��� $��* ��� $��40 �.+��� $��Grinding Machine��  
HeaderText#��[ $	��Top=�� $��1�� � ColumnDefinitions �� $	��130 ��� .+��� $��Cost Centre Code :��$��0��  NormalHeaderText#��[ $��Right=�$��0,0,10,0q��� $��1��#��[ ���� *GrinMachine.COST_CENT_CODE+ �� UpdateSourceTrigger$ PropertyChanged=��� $��2��$��1q� �� Padding$ 0,0,10,0q�$��5q�._�� $��2q� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* � �� $	��150 ��� $��* ��� $	��150 ��� $��* �.+��� $��Min Product Dia : ��$��0��$��Center=�$��Right=�$��0��1   +ProcessDesigner.UserControls.DecimalTextBox  $��90��$
��Left=�$��1��$��0��
 � Tag$# Please enter Min. Product Dia��	 �� 	MaxLength$	 13��
   AllowNegative
 .     MaxDecimalPlaces$ 2������ *GrinMachine.MIN_PROD_DIA+$ PropertyChanged=� �� StringFormat$ F2���� $��Max Product Dia : ��$��2��$��0��$��Center=�$��Right=�  $��90��$# Please enter Max. Product Dia��$
��Left=�$��3��$��0��$	 13��
 .  $ 2������ *GrinMachine.MAX_PROD_DIA+$ PropertyChanged=�$ F2���� $��Max Length in Plunge : ��$��0��$��1��$��Center=�$��Right=�  $��90��$
��Left=�$( "Please enter Max. Length in Plunge��$��1��$��1��$	 13��
 .  $ 2������ *GrinMachine.MAX_LEN_IN_PLUNGE+$ PropertyChanged=�$ F2���� $��Max Ghweel OD : ��$��2��$��1��$��Center=�$��Right=�  $��3��$��90��$! Please enter Max. Gwheel OD��$
��Left=�$��1��$	 13��
 .  $ 2������ *GrinMachine.MAX_GWHEEL_OD+$ PropertyChanged=�$ F2���� $��Max Ghweel Thickness : ��$��0��$��2��$��Center=�$��Right=�  $��1��$��90��$' !Please enter Max. Gweel Thickness��$
��Left=�$��2��$	 13��
 .  $ 2������ *" GrinMachine.MAX_GWHEEL_THICKNESS+$ PropertyChanged=�$ F2���� $��Gwheel Bore : ��$��2��$��2��$��Center=�$��Right=�  $��3��$��90��$ Please enter Gwheel Bore��$
��Left=�$��2��$	 13��
 .  $ 2������ *GrinMachine.GWHEEL_BORE+$ PropertyChanged=�$ F2���� $��Max Cwheel OD : ��$��0��$��3��$��Center=�$��Right=�  $��1��$  Please enter Max. Cweel OD��$��90��$
��Left=�$��3��$	 13��
 .  $ 2������ *GrinMachine.MAX_CWHEEL_OD+$ PropertyChanged=�$ F2���� $��Cwheel Thickness : ��$��2��$��3��$��Center=�$��Right=�  $��3��$��90��$
��Left=�$��3��$# Please enter Cwheel Thickness��$	 13��
 .  $ 2������ *GrinMachine.CWHEEL_THICKNESS+$ PropertyChanged=�$ F2���� $��Cwheel Bore : ��$��0��$��4��$��Center=�$��Right=�  $��1��$ Please enter Cwheel Bore��$��4��$��90��$
��Left=�$	 13��
 .  $ 2������ *GrinMachine.CWHEEL_BORE+$ PropertyChanged=�$ F2���� $��Max Wheel Tilt : ��$��2��$��4��$��Center=�$��Right=�  $��3��$��4��$! Please enter Max Wheel Tilt��$��90��$
��Left=�$	 13��
 .  $ 2������ *GrinMachine.MAX_WHEEL_TILT+$ PropertyChanged=�$ F2���� $��Gwheel Cam Ref Length : ��$��0��$��5��$��Center=�$��Right=�  $' !Please enter Gwheel cam Ref. Len.��$��1��$��90��$
��Left=�$��5��$	 13��
 .  $ 2������ * GrinMachine.GWHEEL_CAM_REF_LEN+$ PropertyChanged=�$ F2���� $��Cwheel Cam Ref Length : ��$��2��$��5��$��Center=�$��Right=�  $��3��$��90��$&  Please enter Cwheel Cam Ref Len.��$
��Left=�$��5��$	 13��
 .  $ 2������ * GrinMachine.CWHEEL_CAM_REF_LEN+$ PropertyChanged=�$ F2���� $��Gwheel Cam Length : ��$��0��$��6��$��Center=�$��Right=�  $��1��$" Please enter Gwheel cam Len.��$��90��$
��Left=�$��6��$	 13��
 .  $ 2������ *GrinMachine.GWHEEL_CAM_LEN+$ PropertyChanged=�$ F2���� $��Cwheel Cam Length  : ��$��2��$��6��$��Center=�$��Right=�  $��3��$��90��$" Please enter Cwheel cam Len.��$
��Left=�$��6��$	 13��
 .  $ 2������ *GrinMachine.CWHEEL_CAM_LENGTH+$ PropertyChanged=�$ F2���� $��Auto Dressing : ��$��0��$��7��$��Center=�$��Right=��� $��1��$��7��$  Please enter Auto Dressing��$	 1��$��15��$
��Left=����� *GrinMachine.AUTO_DRESSING+$ PropertyChanged=��� $��Feed System : ��$��0��$��8��$ Please enter Feed System��$��Center=�$��Right=��� $��1��$��8��$��3��$	 50������ *GrinMachine.FEED_SYSTEM+$ PropertyChanged=�� $��3��$��2��$��2��$��0,0,5,0q� �� �� $
��Auto �.+��� $��30��$��80��$��Right=�$��_Save��$��0,0,10,0q����� *OnSubmitCommand+�� $��1��$��30��$��80��$��Right=�$��_Cancel������ *OnCacelCommand+