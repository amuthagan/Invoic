   M S B A M L   `   `   `  ���� |  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  ProcessDesigner!   ProcessDesigner.frmMOPgraph   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     dControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls       � Title$  Measure Of Performance��$	��600��$	��900�� � WindowStartupLocation$ CenterScreen=�$	��600��$	��600��$	��900��$	��900�� � 
ResizeMode$ NoResize=� � Icon$ Images/logo.ico��.��� .+��� ._��  � RowDefinitions �� $	��10* ��� $	��90* �.+��  � ColumnDefinitions �� $	��100 ��� $	��200 ��� $��80 ��� $	��140 ��� $��80 ��� $��1* ��� $	��120 �.+��� $��Graph Type : ��$��0��$��0�� �� TextAlignment$ Right=�$��Center=��� -   $��cmbGraphType��
 � Tag$ Please select Graph Type��$��0��$��1��$	��170��$
��Left=�$��Center=� �� SelectedValuePath$
 Code��	 �� DisplayMemberPath$	 Description��
 �� SelectedValue
 �� *	GraphType+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=����� *GraphTypeCombo+�� $��Start Date : ��$��0��$��2��$ Right=�$��Center=�0  *ProcessDesigner.UserControls.DatePickerCus  $��0��$��3��$ Please select Start Date��$
��Left=�$��Center=�$	��120��   SelectedDate �� *	StartDate+$ PropertyChanged=�$ TwoWay=��� $��End Date : ��$��0��$��4��$ Right=�$��Center=�  $��0��$��5��$ Please select End Date��$
��Left=�$��Center=�$	��120�� �� *	EndDate+$ PropertyChanged=�$ TwoWay=��� -   $��
btnRefresh��$��_Refresh��$��40��$	��100��$��0��$��6������ *RefreshGraphCommand+�� $��1��$��1q�y��� *SingleLegend+._�>   8System.Windows.Controls.DataVisualization.Charting.Chart  -   $��columnChart��$
��Left=�$	��Top=�$	��880��$	��500��$��0q�   Title �� *
GraphTitle+   LegendStyle ��  �� 
TargetType" ��.���  �� Property � �� �� Value$ 0����  � ��$ 0��   Axes C   =System.Windows.Controls.DataVisualization.Charting.LinearAxis  =   7System.Windows.Controls.DataVisualization.Charting.Axis   Orientation$ Y=�D   >System.Windows.Controls.DataVisualization.Charting.DisplayAxis   ShowGridLines .     Title �� *
YAxisTitle+$ PropertyChanged=�$ TwoWay=�E   ?System.Windows.Controls.DataVisualization.Charting.CategoryAxis  $ X=� �� *
XAxisTitle+$ PropertyChanged=�$ TwoWay=�   AxisLabelStyle �� B   <System.Windows.Controls.DataVisualization.Charting.AxisLabel"  .���  � �� ��  �� 
TargetType"  .R���    FormattedContent#��z  � LayoutTransform ��  �� Angle$	 270Q�   Series. E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  -   $��FIRST��H	   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries 	  DependentValuePath$ Value�� 	  IndependentValuePath$	 Key�� 	  IsSelectionEnabled . ?
   9System.Windows.Controls.DataVisualization.Charting.Series  
  Title  �� *
YAxisTitle+! 	  ItemsSource! �� *	GraphData+" 	  DataPointStyle" �� H   BSystem.Windows.Controls.DataVisualization.Charting.ColumnDataPoint"  .���  � ��
 �  ����  � ��
 �  ����  � ��$	 100q��� $��1��$��1q�y��� *DoubleLegend+._�  -   $��columnChart1��$
��Left=�$	��Top=�$	��880��$	��500�� �� *
GraphTitle+ �� " ��.���  � ��$ 0����  � ��$ 0��   $ Y=� .   �� *
YAxisTitle+$ PropertyChanged=�$ TwoWay=�  $ X=� �� *
XAxisTitle+$ PropertyChanged=�$ TwoWay=� �� "  .���  � �� �� "  .R��� #��z  �� $	 270Q�.   -   $��FIRST1��$ Value��$	 Key�� .   �� *
YAxisTitle+! �� *	GraphData+" �� "  .���  � ��
 �  ����  � ��
 �  ����  � ��$	 100q�  -   $��SECOND1��$   ��$ Value��$	 Key�� . ! �� *
GraphData1+" �� "  .���  � ��
 � � ���  � ��
 � � ���  � ��$	 100q��� $��1��$��1q�y��� *TripleLegend+._�  -   $��columnChart2��$
��Left=�$	��Top=�$	��880��$	��500�� �� *
GraphTitle+   $ Y=� .   �� *
YAxisTitle+$ PropertyChanged=�$ TwoWay=�  $ X=� �� *
XAxisTitle+$ PropertyChanged=�$ TwoWay=� �� "  .���  � �� �� "  .R��� #��z  �� $	 270Q�.   -	   $��FIRST2��$ Value��$	 Key�� .   �� *LegendTitle+! �� *	GraphData+" �� "  .���  � ��
 �  ����  � ��
 �  ����  � ��$	 100q�  -
   $��SECOND2��$ Value��$	 Key�� .   �� *LegendTitle1+! �� *
GraphData1+" �� "  .���  � ��
 � � ���  � ��
 � � ���  � ��$	 100q�  -   $��THIRD2��$ Value��$	 Key�� .   �� *LegendTitle2+! �� *
GraphData2+" �� "  .���  � ��
 ��  ���  � ��
 ��  ���  � ��$	 100q�  -   $��FOURTH2��$ Value��$	 Key�� .    �� *LegendTitle3+! �� *
GraphData3+" �� "  .���  � ��
 � �����  � ��
 � �����  � ��$	 100q�