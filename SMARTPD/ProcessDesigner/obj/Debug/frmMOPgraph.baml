   M S B A M L   `   `   `  ����|  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5   	   W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls 5   	    ProcessDesigner!   ProcessDesigner.frmMOPgraph   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   dControlPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls     5   	     � Title$  Measure Of Performance��5   	   $	��600��6(   $	��900��65    � WindowStartupLocation$ CenterScreen=�6C   $	��600��6h   $	��600��6x   $	��900��6�   $	��900��6�    � 
ResizeMode$ NoResize=�6�    � Icon$ Images/logo.ico��6�   .��5      � .+�5   
   �� ._�5	      �  � RowDefinitions 5
      �� 5      $	��10* �6$   63   �� 5      $	��90* �6$   63   .+�5      �  � ColumnDefinitions 5      �� 5      $	��100 �6+   6:   �� 5      $	��200 �6+   69   �� 5      $��80 �6+   68   �� 5      $	��140 �6+   6:   �� 5      $��80 �6+   68   �� 5      $��1* �6+   68   �� 5      $	��120 �6+   69   .+�5      �� $��Graph Type : ��6    $��0��65   $��0��6C    �� TextAlignment$ Right=�6S   $��Center=�6i   6�   �� 5      -   5        $��cmbGraphType��5   j   
 � Tag$ Please select Graph Type��6   $��0��6>   $��1��6K   $	��170��6]   $
��Left=�6~   $��Center=�6�    �� SelectedValuePath$
 Code��5   �   	 �� DisplayMemberPath$	 Description��6�   
 �� SelectedValue
 6   �� *	GraphType+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=���6s   �� *GraphTypeCombo+6�   �� 5      $��Start Date : ��6    $��0��65   $��2��6C   $ Right=�6S   $��Center=�6i   6�   0  *ProcessDesigner.UserControls.DatePickerCus  5      $��0��6,   $��3��69   $ Please select Start Date��6I   $
��Left=�6h   $��Center=�6�   $	��120��6�      SelectedDate 6�   �� *	StartDate+$ PropertyChanged=�$ TwoWay=�6  �� 5      $��End Date : ��6!   $��0��64   $��4��6B   $ Right=�6R   $��Center=�6h   6�     5      $��0��6-   $��5��6:   $ Please select End Date��6J   $
��Left=�6g   $��Center=�6�   $	��120��6�    6�   �� *	EndDate+$ PropertyChanged=�$ TwoWay=�6�   �� 5      -   5        $��
btnRefresh��5      $��_Refresh��6/   $��40��6B   $	��100��6O   $��0��6\   $��6��6j   ��6z   �� *RefreshGraphCommand+6�   5       �� 5!      $��1��6   $��1q�6&   y�6:   �� *SingleLegend+._�5"      >   8System.Windows.Controls.DataVisualization.Charting.Chart  -   5        $��columnChart��5"   H   $
��Left=�6-   $	��Top=�6x   $	��880��6�   $	��500��6�   $��0q�6�      Title 6[   �� *
GraphTitle+   LegendStyle 5$      �� 5%       �� 
TargetType" ��6$   .�5&   "   ��  �� Property � �� �� Value$ 0��6:   �� 5'   "    � ��$ 0��6;   5(         Axes 5+      C   =System.Windows.Controls.DataVisualization.Charting.LinearAxis  =   7System.Windows.Controls.DataVisualization.Charting.Axis   Orientation$ Y=�69   D   >System.Windows.Controls.DataVisualization.Charting.DisplayAxis   ShowGridLines .     Title 5,   )   �� *
YAxisTitle+$ PropertyChanged=�$ TwoWay=�5-   )   E   ?System.Windows.Controls.DataVisualization.Charting.CategoryAxis  52      $ X=�6;    6K   �� *
XAxisTitle+$ PropertyChanged=�$ TwoWay=�   AxisLabelStyle 53   "   �� 54   &   B   <System.Windows.Controls.DataVisualization.Charting.AxisLabel"  6,   .�55   *   ��  � �� 56   .   �� 57   2    �� 
TargetType"  6B   .R�58   6   ��    FormattedContent#��z 6@    � LayoutTransform 59   :   �� 5:   >    �� Angle$	 270Q�6N   5<   7   5=   3   5?   +   5@   '   5B         Series. 5D      E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  -   5        $��FIRST��5D   7   H	   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries 	  DependentValuePath$ Value��5F   1    	  IndependentValuePath$	 Key��5G   1    	  IsSelectionEnabled . ?
   9System.Windows.Controls.DataVisualization.Charting.Series  
  Title  5E   1   �� *
YAxisTitle+! 	  ItemsSource! 5H   1   �� *	GraphData+" 	  DataPointStyle" 5J      �� 5K   "   H   BSystem.Windows.Controls.DataVisualization.Charting.ColumnDataPoint"  6(   .�5L   &   ��  � ��
 �  ��6C   �� 5M   &    � ��
 �  ��6E   �� 5N   &    � ��$	 100q�6H   5O   #   5Q      5R      5S      �� 5T      $��1��6   $��1q�6&   y�6:   �� *DoubleLegend+._�5U        -   5        $��columnChart1��5U   H   $
��Left=�6-   $	��Top=�6y   $	��880��6�   $	��500��6�    6\   �� *
GraphTitle+ 5W      �� 5X      " ��6$   .�5Y   "   ��  � ��$ 0��6:   �� 5Z   "    � ��$ 0��6;   5[       5^        $ Y=�69    .   5_   )   �� *
YAxisTitle+$ PropertyChanged=�$ TwoWay=�5`   )     5e      $ X=�6;    6K   �� *
XAxisTitle+$ PropertyChanged=�$ TwoWay=� 5f   "   �� 5g   &   "  6,   .�5h   *   ��  � �� 5i   .   �� 5j   2   "  6B   .R�5k   6   �� #��z 6@    5l   :   �� 5m   >   $	 270Q�6N   5o   7   5p   3   5r   +   5s   '   5u      . 5w        -   5        $��FIRST1��5w   7   $ Value��5y   1   $	 Key��5z   1    .   5x   1   �� *
YAxisTitle+! 5{   1   �� *	GraphData+" 5}      �� 5~   "   "  6(   .�5   &   ��  � ��
 �  ��6C   �� 5�   &    � ��
 �  ��6E   �� 5�   &    � ��$	 100q�6H   5�   #   5�        5�      -   5        $��SECOND1��5�   7   $   ��5�   1   $ Value��5�   1   $	 Key��5�   1    . ! 5�   1   �� *
GraphData1+" 5�      �� 5�   "   "  6(   .�5�   &   ��  � ��
 � � �6C   �� 5�   &    � ��
 � � �6E   �� 5�   &    � ��$	 100q�6H   5�   #   5�      5�      5�      �� 5�      $��1��6   $��1q�6&   y�6:   �� *TripleLegend+._�5�        -   5        $��columnChart2��5�   H   $
��Left=�6-   $	��Top=�6y   $	��880��6�   $	��500��6�    6\   �� *
GraphTitle+ 5�        $ Y=�69    .   5�   )   �� *
YAxisTitle+$ PropertyChanged=�$ TwoWay=�5�   )     5�      $ X=�6;    6K   �� *
XAxisTitle+$ PropertyChanged=�$ TwoWay=� 5�   "   �� 5�   &   "  6,   .�5�   *   ��  � �� 5�   .   �� 5�   2   "  6B   .R�5�   6   �� #��z 6@    5�   :   �� 5�   >   $	 270Q�6N   5�   7   5�   3   5�   +   5�   '   5�      . 5�        -	   5        $��FIRST2��5�   7   $ Value��5�   1   $	 Key��5�   1    .   5�   1   �� *LegendTitle+! 5�   1   �� *	GraphData+" 5�      �� 5�   "   "  6(   .�5�   &   ��  � ��
 �  ��6C   �� 5�   &    � ��
 �  ��6E   �� 5�   &    � ��$	 100q�6H   5�   #   5�        5�      -
   5        $��SECOND2��5�   7   $ Value��5�   1   $	 Key��5�   1    .   5�   1   �� *LegendTitle1+! 5�   1   �� *
GraphData1+" 5�      �� 5�   "   "  6(   .�5�   &   ��  � ��
 � � �6C   �� 5�   &    � ��
 � � �6E   �� 5�   &    � ��$	 100q�6H   5�   #   5�        5�      -   5        $��THIRD2��5�   7   $ Value��5�   1   $	 Key��5�   1    .   5�   1   �� *LegendTitle2+! 5�   1   �� *
GraphData2+" 5�      �� 5�   "   "  6(   .�5�   &   ��  � ��
 ��  �6C   �� 5�   &    � ��
 ��  �6E   �� 5�   &    � ��$	 100q�6H   5�   #   5�        5�      -   5        $��FOURTH2��5�   7   $ Value��5�   1   $	 Key��5�   1    .    5�   1   �� *LegendTitle3+! 5�   1   �� *
GraphData3+" 5�      �� 5�   "   "  6(   .�5�   &   ��  � ��
 � ���6C   �� 5�   &    � ��
 � ���6E   �� 5�   &    � ��$	 100q�6H   5�   #   5�      5�      5�      5�      5�      5�      5�      