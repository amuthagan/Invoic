   M S B A M L   `   `   `  ����    ProcessDesigner.UserControls3    -ProcessDesigner.UserControls.ProcessingWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     $	��450��$��60��  � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � ShowActivated .  ���	#FF0D3463 � 	Resources    Storyboard1 g� 
SpeedRatio �� TargetProperty �� 
TargetName g� RepeatBehavior	 g� Duration9  3System.Windows.Media.Animation.EasingDoubleKeyFrame
 O� KeyTime O� Value   EasingFunction/  )System.Windows.Media.Animation.CircleEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode  Storyboard2  Storyboard3  Storyboard4  Storyboard5��%�  &	        &	 �     &	     &	 %    &	 <    �� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse��$ Forever��$	 0:0:6D�.\�  $
 0:0:2.5��$	 240Q�   $ EaseOut=�  $
 0:0:4��$	 540Q�   $ EaseIn=�  $
 0:0:4��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse1��$ Forever��$	 0:0:6D�.\�  $
 0:0:0.3��$ 0Q�  $
 0:0:2.5��$	 240Q�   $ EaseOut=�  $
 0:0:4.3��$	 540Q�   $ EaseIn=�  $
 0:0:4.3��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse2��$ Forever��$	 0:0:6D�.\�  $
 0:0:0.6��$ 0Q�  $
 0:0:2.5��$	 240Q�   $ EaseOut=�  $
 0:0:4.6��$	 540Q�   $ EaseIn=�  $
 0:0:4.6��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse3��$ Forever��$	 0:0:6D�.\�  $
 0:0:0.9��$ 0Q�  $
 0:0:2.5��$	 240Q�   $ EaseOut=�  $
 0:0:4.9��$	 540Q�   $ EaseIn=�  $
 0:0:4.9��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse4��$ Forever��$	 0:0:6D�.\�  $
 0:0:1.2��$ 0Q�  $
 0:0:2.5��$	 240Q�   $ EaseOut=�  $
 0:0:5.2��$	 540Q�   $ EaseIn=�  $
 0:0:5.2��$ 0Q� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��.[��� #��[  �� #��[ �� #��[ �� #��[ �� #��[ .��� -    �Name$ 
LayoutRoot�� � RowDefinitions �� �� $��35 �.+�A� -    A�Name$ ellipse��
�������$
��Left=�$��	-8,20,0,0q�
�������$��5��$��5��$	��Top=� J� RenderTransformOrigin$ 0.5,0.5&�z�T� A� -   $ ellipse1��
�������$
��Left=�$��
-16,20,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� A� -   $ ellipse2��
�������$
��Left=�$��
-24,20,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� A� -   $ ellipse3��
�������$
��Left=�$��
-32,20,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� A� -   $ ellipse4��
�������$
��Left=�$��
-40,20,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� �� -   $��
txtProcess��$��1��$��
Processing��
�������$��15(�$��Center=�$
��Bold"�