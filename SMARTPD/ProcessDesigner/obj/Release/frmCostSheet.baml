   M S B A M L   `   `   `  ���� W  SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core J FProcessDesigner, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ProcessDesigner.UICommonProcessDesigner.UICommon "   ProcessDesigner.frmCostSheet   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml    	 _ucPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls    	 ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity    	 osi`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions    	 ;UICommon&clr-namespace:ProcessDesigner.UICommon    	   � Title$  
Cost Sheet��$	��550��$	��800��   WindowBackground#��[   � Icon$ Images\logo.ico�� � WindowStartupLocation$ CenterScreen=�.���� ._��  � RowDefinitions �� $	��100 ��� $��* ��� $��80 �.+��� $��0��$	��5,2q�$��2q�._�� $��0��$��	10,5,10,5q� �� $��* ��� $��* ��� $��* � � ColumnDefinitions �� $
��108* ��� $
��192* ��� $
��108* ��� $
��108* ��� $
��108* ��� $
��106* ��� $
��108* �.+��� $��
Part No : ��$��0��$��Center=�$��Right=�$��0��$
��Bold"�$��15��$��0,6,0.172,6.333q�$��57��.   (ProcessDesigner.UserControls.ComboBoxCus  -     Name$ 	cmbPartNo��$��0��
 � Tag$ Please select Part No��   SelectedValuePath$ PART_NO��   	MaxLength$ 9��$��2��$
��Auto��$��110,3,0.287,2.333q�$	��160��	   
IsReadOnly	 �� *PartNoIsReadOnly+
   ColumnsHeader
 �� *DropdownHeaders+   
DataSource �� *CostSheet.DVProductMaster+ �� UpdateSourceTrigger$ PropertyChanged=�   SelectedItem �� *PartNoSelectedItem+$ PropertyChanged=�   SelectedValue �� *CostSheet.PART_NO+$ PropertyChanged=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectedItemChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *OnPartNoSelectionChanged+  $ TextBox_LostFocus��.    �� *OnPartNoSelectionChanged+ J� InputBindings �� 
 �� Key$ Enter�� �� Command �� *OnPartNoSelectionChanged+�� $��Description : ��$��2��$��0��$��Center=�$��Right=�$��15��$��0,6,0.429,6.333q�$��70���� $��2��$��0��$
��Bold"�$��Center=�$
��Left=�$��15��$��107.713,6,0,6.333q�$��0������ *CostSheet.PART_DESC+�� $��Process No : ��$��0��$
��Bold"�$��Center=�$��Right=�$��1��$��14��$��0,6.667,0.172,6.667q�$��78��5   /ProcessDesigner.UserControls.ComboBoxWithSearch  -     Name$ 
cmbRouteNo��$��1��$ Please enter Process No��   
IsReadOnly .    SelectedValuePath$ ROUTE_NO��   DisplayValuePath$ ROUTE_NO��   	MaxLength$ 9��   DropDownWidth$	 140��$��2��$
��Auto��$��110,2.667,0.287,2.667q�$	��160��   ColumnsHeader �� *RouteNoDropdownHeaders+   
DataSource �� *CostSheet.DVProcessMain+$ PropertyChanged=�   SelectedItem �� *ProcessNoSelectedItem+$ PropertyChanged=�   SelectedValue �� *CostSheet.ROUTE_NO+$ PropertyChanged=�    SelectedText  �� *CostSheet.ROUTE_NO+$ PropertyChanged=�   $ SelectedItemChanged��.    �� *OnProcessNoSelectionChanged+�� $��Customer : ��$��2��$��1��$��Center=�$��Right=�$��14��$��0,6.667,0.429,6.667q�$��62���� $��2��$��1��$
��Bold"�$��Center=�$
��Left=�$��14��$��107.713,6.667,0,6.667q�$��0������ *CostSheet.CUSTOMER+�� $��RM : ��$��0��$��Center=�$��Right=�$��2��$��15��$��0,6.333,0.172,6q�$��28��  -   $ cmbRM��$��2��$ Please enter RM�� . $ RM_CODE��$ RM_CODE��$ 9��$��2��$
��Auto��$��110,2.333,0.287,3q�$	��160�� �� *RMDropdownHeaders+ �� *CostSheet.DVRMBasic+$ PropertyChanged=� �� *CostSheet.WIRE_ROD_CD+$ PropertyChanged=�  �� *CostSheet.WIRE_ROD_CD+$ PropertyChanged=�   $ SelectedItemChanged��.    �� *OnRMSelectionChanged+�� $��Cheese Wt : ��$��2��$��2��$��Center=�$��Right=�$��15��$��0,6.333,0.429,6q�$��68���� $��2��$��2��$
��Bold"�$��Center=�$
��Left=�$��15��$��107.713,6.333,0,6q�$��0������ *CostSheet.CHEESE_WT+! �� StringFormat$! F3���� $��Finish Wt : ��$��4��$��2��$��Center=�$��Right=�$��2��$��15��$��0,6.333,107.857,6q�$��61���� $��5��$��2��$
��Bold"�$��Center=�$
��Left=�$��15��$��0.285,6.333,0,6q�$��0������ *CostSheet.FINISH_WT+$! F3���� $��Export��$��Center=�$��6��$��2��$��15��$��0.143,6.333,0,6q�" d� 	IsChecked" �� *CostSheet.EXPORT+   $ Checked��.    �� *OnCheckedChanged+  $ 	Unchecked��.    �� *OnCheckedChanged+�� $��0��$��6��$��3��$��_Print��0   *ProcessDesigner.UserControls.AddedProperty#   Image$# Images/Print.png����.  $ � ToolTip$$ Print��$��0.143,0,0,27q����� *PrintCommand+�� $��1��$��1q�$	��5,2q�._�&   System.Windows.Controls.DataGrid  -   $��dgrdProcessSheet��$��1��%   AutoGenerateColumns% .  &   CanUserAddRows& .  2	  ,ProcessDesigner.UICommon.ScrollToTopBehavior' 	  ScrollToTop' . (   CanUserDeleteRows( .  )   RowHeaderWidth$) 20Q�*   CanUserResizeRows* .  +   SelectionUnit$+ FullRow=�,   SelectionMode$, Extended=����� *CostSheet.DVCostSheet+$ PropertyChanged=�- �� SelectedItem- �� *ProcessSheetSelectedItem+$ PropertyChanged=�.   Columns. 0
  *System.Windows.Controls.DataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn/   
IsReadOnly/ . 0   Header$0 Seq No��1   Width5  /System.Windows.Controls.DataGridLengthConverter$1 80 1  +System.Windows.Controls.DataGridBoundColumn2   Binding2 �� *SEQ_NO+$ PropertyChanged=�3   ElementStyle3 �� 4 �� 
TargetType"4 ��.��� 5 �� Property5 � ��6 �� Value$6 Center=�
  / . $0 Opn Code��$1 90 2 �� *OPN_CD+$ PropertyChanged=�3 �� "4 ��.��� 5 � ��$6 Center=�
  / . $0 	Operation��$	1 200 2 �� *
OPN_DESC+$ PropertyChanged=�3 �� "4 ��.��� 5 � ��$6 Center=�
  / . $0 CC Code��$1 90 2 �� *	CC_CODE+$ PropertyChanged=�3 �� "4 ��.��� 5 � ��$6 Center=�
  / . $0 Output��$1 90 2 �� *OUTPUT+$ PropertyChanged=�$! F2��3 �� "4 ��.��� 5 � ��$6 Center=�
  / . $0 Var Cost��$1 90 2 �� *
VAR_COST+$ PropertyChanged=�$! F2��3 �� "4 ��.��� 5 � ��$6 Center=�
  / . $0 Fix Cost��$1 90 2 �� *
FIX_COST+$ PropertyChanged=�$! F2��3 �� "4 ��.��� 5 � ��$6 Center=�
  / . $0 Spl Cost��$1 90 2 �� *
SPL_COST+$ PropertyChanged=�$! F2��3 �� "4 ��.��� 5 � ��$6 Center=�
  / . $0 Opn Cost��$1 90 2 �� *
OPN_COST+$ PropertyChanged=�$! F2��3 �� "4 ��.��� 5 � ��$6 Center=��� $��2��$��2q�$	��5,2q�._��  �� $	��50* ��� $	��50* � �� $	��30* ��� $	��20* ��� $	��30* ��� $	��20* �.+��� $��Raw Material Cost Rs :��$��0��$��0��$��Center=�$��Right=��� $$��Total Cost for 100 Pieces Rs :��$��0��$��1��$��Center=�$��Right=��� $��1��$��0��$
��Bold"�$��Center=�$
��Left=����� *CostSheet.RMCOST+$! F2���� $��1��$��1��$
��Bold"�$��Center=�$
��Left=����� *CostSheet.TOTAL+$! F2���� $)��#Conversion Cost for 100 pieces Rs :��$��2��$��0��$��Center=�$��Right=��� $��Realisation Rs :��$��2��$��1��$��Center=�$��Right=�$y�	Collapsed=��� $��3��$��0��$
��Bold"�$��Center=�$
��Left=����� *CostSheet.COST+$! F2���� $��3��$��1��$
��Bold"�$��Center=�$
��Left=�$y�	Collapsed=����� *CostSheet.REAL+$! F2��