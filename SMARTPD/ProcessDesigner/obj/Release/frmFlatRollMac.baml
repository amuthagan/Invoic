   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon $   ProcessDesigner.frmFlatRollMac   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ccommonPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     8UICom&clr-namespace:ProcessDesigner.UICommon       � Title$!  Flat Thread Rolling Machine��$	��375��$	��365��$	��600��$	��630�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�   WindowBackground#��[   � 	Resources /  )ProcessDesigner.UICommon.DecimalConverter  DecimalConverter/ )ProcessDesigner.UICommon.IntegerConverter  IntegerConverter��%   &	       &	          .���� $��5q�$��1q�._��  � RowDefinitions �� $��25 ��� $��20 ��� $��* ��� $��40 �.+��� $!��Flat Thread Rolling Machine��  
HeaderText#��[ $	��Top=�� $��0��� $��1�� � ColumnDefinitions �� $	��14* ��� $	��50* �.+��� $��Cost Centre Code : ��$
��Bold"�$��0��$��Center=�$
��Left=�$��1��$��5,5,5,5q��� $��1��$��Center=�$
��Left=�$��1������ *ThreadRolling.COST_CENT_CODE+�� $��2��$��1q� �� Padding$ 0,0,10,0q�$��5q�._�� $��2�� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* � �� $	��20* ��� $	��30* ��� $	��20* ��� $	��30* �.+��� $��Max Thread Dia : ��$��0��$��Center=�$��Right=�$��0��1   +ProcessDesigner.UserControls.DecimalTextBox  $��1��$��2q�$��0��
 � Tag$# Please enter Max. Thread Dia.��   MaxDecimalPlaces$ 2��	   AllowNegative	 .  
 �� 	MaxLength$
 13��$��90��$
��Left=����� *ThreadRolling.MAX_THREAD_DIA+ �� UpdateSourceTrigger$ PropertyChanged=� �� StringFormat$ F2���� $��Max Thread Length : ��$��2��$��0��$��Center=�$��Right=�  $��3��$% Please enter Max. Thread Length��$��2q�$��0��$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.MAX_THREAD_LEN+$ PropertyChanged=�$ F2���� $��Max Shank Length : ��$��0��$��1��$��Center=�$��Right=�  $��1��$��1��$��2q�$$ Please enter Max. Shank Length��$ 2��$��90��$
��Left=�	 .  $
 13������ *ThreadRolling.MAX_SHANK_LEN+$ PropertyChanged=�$ F2���� $��Moving Die Length : ��$��2��$��1��$��Center=�$��Right=�  $��3��$��1��$$ Please enter Moving Die Length��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.MOVING_DIE_LEN+$ PropertyChanged=�$ F2���� $��Moving Die Width : ��$��0��$��2��$��Center=�$��Right=�  $��1��$��2��$��2q�$# Please enter Moving Die Width��$ 2��	 .  $
 13��$��90��$
��Left=����� * ThreadRolling.MOVING_DIE_WIDTH+$ PropertyChanged=�$ F2���� $��Moving Die Height : ��$��2��$��2��$��Center=�$��Right=�  $��3��$��2��$$ Please enter Moving Die Height��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� *!ThreadRolling.MOVING_DIE_HEIGHT+$ PropertyChanged=�$ F2���� $��Fixed Die Length : ��$��0��$��3��$��Center=�$��Right=�  $��1��$# Please enter Fixed die length��$��3��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.FIXED_DIE_LEN+$ PropertyChanged=�$ F2���� $��Fixed Die Width : ��$��2��$��3��$��Center=�$��Right=�  $��3��$��3��$" Please enter Fixed Die Width��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.FIXED_DIE_WIDTH+$ PropertyChanged=�$ F2���� $��Fixed Die Height : ��$��0��$��4��$��Center=�$��Right=�  $��1��$��4��$# Please enter Fixed Die Height��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� * ThreadRolling.FIXED_DIE_HEIGHT+$ PropertyChanged=�$ F2���� $��Die Pocket Depth : ��$��2��$��4��$��Center=�$��Right=�  $��3��$��4��$# Please enter Die Pocket Depth��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� * ThreadRolling.DIE_POCKET_DEPTH+$ PropertyChanged=�$ F2���� $��Max Hardness : ��$��0��$��5��$��Center=�$��Right=�  $��1��$��5��$  Please enter Max. Hardness��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.MAX_HARDNESS+$ PropertyChanged=�$ F2���� $��Min Hardness  : ��$��2��$��5��$��Center=�$��Right=�  $��3��$��5��$  Please enter Min. Hardness��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.MIN_HARDNESS+$ PropertyChanged=�$ F2���� $��
Coolant : ��$��0��$��6��$��Center=�$��Right=��� $��1��$��6��$ Please enter Coolant��$
 50��$��2q����� *ThreadRolling.COOLANT+�� $��Method of Feed : ��$��2��$��6��$��Center=�$��Right=�  $��3��$��6��$��2q�$! Please enter Method of Feed��$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.FEED_METHOD+$ PropertyChanged=�$ F2���� $��Feeding Mechanism : ��$��0��$��7��$��Center=�$��Right=��� $��1��$��7��$$ Please enter Feeding Mechanism��$
 50��$��2q����� *!ThreadRolling.FEEDING_MECHANISH+$ PropertyChanged=��� $��Motor Power : ��$��2��$��7��$��Center=�$��Right=�  $��3��$��7��$ Please enter Motor Power��$��2q�$ 2��	 .  $
 13��$��90��$
��Left=����� *ThreadRolling.MOTOR_POWER+$ PropertyChanged=�$ F2���� $��
Remarks : ��$��0��$��8��$��Center=�$��Right=��� $��1��$	
 200��$ Please enter Remarks��$��8��$��2q�$��3������ *ThreadRolling.REMARKS+$ PropertyChanged=�� $��3��$��2��$��2��$��0,0,5,0q� �� �� $
��Auto �.+��� $��30��$��80��$��Right=�$��_Save��$��0,0,10,0q����� *OnSubmitCommand+�� $��1��$��30��$��80��$��Right=�$��_Cancel������ *OnCacelCommand+