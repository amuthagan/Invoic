   M S B A M L   `   `   `  ���� c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  W SProcessDesigner.UserControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullqPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControlsProcessDesigner.UserControls  ProcessDesigner#   ProcessDesigner.frmCopyStatus   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     cLookupPclr-namespace:ProcessDesigner.UserControls;assembly=ProcessDesigner.UserControls       � Title$  Copy Status��$	��500��$	��550��$	��700�� � Icon$ Images/logo.ico�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�   WindowBackground#��[  .���� $��1q�
���   �$��5,5,5,5q�._��  � RowDefinitions �� $	��20* ��� $	��40* ��� $	��10* � � ColumnDefinitions �� $	��70* ��� $	��30* �.+�� $��0��$��0�� �� $
��100* � �� $��* �.+��� $��Data to be copied��$��0��$��0��$��2,2,2,2q�.���� $��0��$��2,2,2,2q�$��1q� �� CornerRadius$ 7��._�� $��0��$��5,5,5,5q� �� $	��20* ��� $	��20* ��� $	��20* � �� $	��50* ��� $	��50* ��� $	��50* �.+��� -    ��Name$ 
chkPrdMast��$��0��$��0�� d� 	IsChecked �� *IschkPrdMast+{��� *En_chkPrdMast+.   (System.Windows.Interactivity.Interaction	   Triggers	 /   )System.Windows.Interactivity.EventTrigger  
   	EventName$
 Click��.   (System.Windows.Interactivity.TriggerBase   Actions. 6   0System.Windows.Interactivity.InvokeCommandAction     Command �� *OnCheckBoxClicked+ � LayoutTransform ��  �� ScaleX$	 1.5Q� �� ScaleY$	 1.5Q�.���� $��Product Master��$��8(��� -   $ chkProcessSheet��$��0��$��1�� �� *IschkProcessSheet+{��� *En_chkProcessSheet+	   $
 Click��.    �� *OnCheckBoxClicked+ �� $	 1.5Q�$	 1.5Q�.���� $��Process Sheet��$��8(��� -   $ chkControlPlan��$��0��$��2�� �� *IschkControlPlan+{��� *En_chkControlPlan+	   $
 Click��.    �� *OnCheckBoxClicked+ �� $	 1.5Q�$	 1.5Q�.���� $��Control Plan��$��8(��� -   $ chkDrawings��$��1��$��0�� �� *IschkDrawings+{��� *En_chkDrawings+	   $
 Click��.    �� *OnCheckBoxClicked+ �� $	 1.5Q�$	 1.5Q�.���� $��Drawings��$��8(��� -   $ chkToolSchedule��$��1��$��1�� �� *IschkToolSchedule+{��� *En_chkToolSchedule+	   $
 Click��.    �� *OnCheckBoxClicked+ �� $	 1.5Q�$	 1.5Q�.���� $��Tool Schedule��$��8(��� $��
Start Copy��$��0��$��3��$��2��$	��100��$	��100��$��Center=����� *CopyClickCommand+� $��0��$��1��$��2��.+��� $��Copy Status��$��0��$��0��$��3��$��2,2,2,2q�.��� $��0��$	��250��$��5,5,5,5q� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � �� $	��10* ��� $	��90* �.+��� $��0��$��0��$��Images/CHECKMRK.ICO��y��� *imgPrdMaster+�� $��1��$��0��$��/Images/CHECKMRK.ICO��y��� *ImgProcessSheetHeader+�� $��2��$��0��$��/Images/CHECKMRK.ICO��y��� *ImgProcessSheet+�� $��3��$��0��$��/Images/CHECKMRK.ICO��y��� *ImgProcessSheetCC+�� -    ��Name$ imgTSH��$��4��$��0��$��/Images/CHECKMRK.ICO��y��� *ImgToolScheduleHeader+�� -   $ imgTSD��$��5��$��0��$��/Images/CHECKMRK.ICO��y��� *ImgToolScheduleDetails+�� -   $ ImgCP��$��6��$��0��$��/Images/CHECKMRK.ICO��y��� *imgControlPlan+�� -	   $ ImgPD��$��7��$��0��$��/Images/CHECKMRK.ICO��y��� *imgProductDrawing+�� -
   $ ImgSD��$��8��$��0��$��/Images/CHECKMRK.ICO��y��� *imgSequenceDrawing+�� $��7��$��0���� $��8��$��0���� -   $��lblProductMaster��$��Product Master��$��0��$��1�� �� TextAlignment$
 Left=��� -   $��lblProcessSheetHeader��$��Process Sheet Header��$��1��$��1��$
 Left=��� -   $��lblProcessSheet��$��Process Sheet��$��2��$��1��$
 Left=��� -   $ ��lblProcessSheetCostCentres��$ ��Process Sheet Cost Centres��$��3��$��1��$
 Left=��� -   $��lblToolScheduleHeader��$��Tool Schedule Header��$��4��$��1��$
 Left=��� -   $��lblToolScheduleDetails��$��Tool Schedule Details��$��5��$��1��$
 Left=��� -   $��lblTControlPlan��$��Control Plan��$��6��$��1��$
 Left=��� -   $��lblProductDrawing��$��Product Drawing��$��7��$��1��$
 Left=��� -   $��lblSequenceDrawing��$��Sequence Drawing��$��8��$��1��$
 Left=�� $��1��$��0��.+��� $��Copy Parameters��$��0��$��0��$��3��$��2,2,2,2q�.���� $��0��$��2,2,2,2q�$��1q�$ 7��._�� $��0��$	��200��$��5,5,5,5q� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � �� $	��100 ��� $��* ��� $	��100 ��� $��* �.+��� -   $��lblOldPartNo��$��Old Part No : ��$��0��$��0��$ Right=��� -   $��lblOldRouteNo��$��Old Process No : ��$��1��$��0��$ Right=��� -   $��lblOldSeqNo��$��Old Seq No : ��$��2��$��0��$ Right=��� -   $��
lblOldCCNo��$��Old CC No : ��$��3��$��0��$ Right=��� -   $��
lblOldSHNo��$��Old SH No : ��$��4��$��0��$ Right=��� -   $��lblNewPartNo��$��New Part No : ��$��0��$��2��$ Right=��� -   $��lblNewRouteNo��$��New Process No : ��$��1��$��2��$ Right=��� -   $��lblNewSeqNo��$��New Seq No : ��$��2��$��2��$ Right=��� -   $��
lblNewCCNo��$��New CC No : ��$��3��$��2��$ Right=��� -   $��
lblNewSHNo��$��New SH No : ��$��4��$��2��$ Right=��� -    ��Name$ txtOldPartNo��
 � Tag$ Please enter Old Part No.��$	��120��$
��Left=�$��25��$��0��$��1�� �� 	MaxLength$ 9�� �� CharacterCasing$ Upper=����� *	OldPartNo+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� -   $ txtOldRouteNo��$��60��$" Please enter Old Process No.��$
��Left=�$��25��$��1��$��1��$ 2��2  ,ProcessDesigner.UserControls.TextBoxIntsOnly   Mask$ UnSignedInt=����� *
OldRouteNo+$ TwoWay=�$ PropertyChanged=� �� 
IsReadOnly �� *IsReadOnlyProcessNo+�� -    $ txtOldSeqNo��$��60��$ Please enter Old Seq No.��$
��Left=�$��25��$��2��$��1��$ 4��$ UnSignedInt=����� *
OldSeqNo+$ TwoWay=�$ PropertyChanged=� �� *IsReadOnlySeqNo+�� -!   $ 
txtOldCCNo��$��40��$ Please enter Old CC No.��$
��Left=�$��25��$��3��$��1��$ 3��$ UnSignedInt=����� *
OldCCSno+$ TwoWay=�$ PropertyChanged=� �� *IsReadOnlyCCSno+�� -"   $ 
txtOldSHNo��$ Please enter Old SH No.��$��50��$
��Left=�$��25��$��4��$��1��$ 4��$ UnSignedInt=����� *	OldShNo+$ TwoWay=�$ PropertyChanged=� �� *IsReadOnlySHNo+�� -#   $ txtNewPartNo��$ Please enter New Part No.��$	��120��$
��Left=�$��25��$��0��$��3��$ 11��$ Upper=����� *	NewPartNo+$ TwoWay=�$ PropertyChanged=��� -$   $ txtNewRouteNo��$��60��$" Please enter New Process No.��$
��Left=�$��25��$��1��$��3��$ 2��$ UnSignedInt=����� *
NewRouteNo+$ TwoWay=�$ PropertyChanged=� �� *IsReadOnlyProcessNo+�� -%   $ txtNewSeqNo��$��60��$ Please enter New Seq. No.��$
��Left=�$��25��$��2��$��3��$ 4��$ UnSignedInt=����� *
NewSeqNo+$ TwoWay=�$ PropertyChanged=� �� *IsReadOnlySeqNo+�� -&   $ 
txtNewCCNo��$ Please enter New CC No.��$��40��$
��Left=�$��25��$��3��$��3��$ 3��$ UnSignedInt=����� *
NewCCSno+$ TwoWay=�$ PropertyChanged=� �� *IsReadOnlyCCSno+�� -'   $ 
txtNewSHNo��$ Please enter New SH No.��$��50��$
��Left=�$��25��$��4��$��3��$ 4��$ UnSignedInt=����� *	NewShNo+$ TwoWay=�$ PropertyChanged=� �� *IsReadOnlySHNo+�� $��Close��$��2��$��1��$	��100��$��50��$��Center=� � ToolTip$ 
Close Form��0  *ProcessDesigner.UserControls.AddedProperty   Image$ Images/Close.png������ *OnCloseCommand+