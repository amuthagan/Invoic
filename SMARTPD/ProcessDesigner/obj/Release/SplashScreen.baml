   M S B A M L   `   `   `  ����   ProcessDesigner"    ProcessDesigner.SplashScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     $	��450��$	��200��  � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � ShowActivated .   � WindowStartupLocation$ CenterScreen=����	#FF0D3463 � Icon$ Images/logo.ico�� � 	Resources    Storyboard1 g� 
SpeedRatio �� TargetProperty �� 
TargetName	 g� RepeatBehavior
 g� Duration9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value   EasingFunction/  )System.Windows.Media.Animation.CircleEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode  Storyboard2  Storyboard3  Storyboard4  Storyboard5��%�  &	        &	 �     &	     &	 %    &	 <    �� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse��$	 Forever��$
 0:0:6D�.\�  $ 0:0:2.5��$	 240Q�   $ EaseOut=�  $ 0:0:4��$	 540Q�   $ EaseIn=�  $ 0:0:4��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse1��$	 Forever��$
 0:0:6D�.\�  $ 0:0:0.3��$ 0Q�  $ 0:0:2.5��$	 240Q�   $ EaseOut=�  $ 0:0:4.3��$	 540Q�   $ EaseIn=�  $ 0:0:4.3��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse2��$	 Forever��$
 0:0:6D�.\�  $ 0:0:0.6��$ 0Q�  $ 0:0:2.5��$	 240Q�   $ EaseOut=�  $ 0:0:4.6��$	 540Q�   $ EaseIn=�  $ 0:0:4.6��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse3��$	 Forever��$
 0:0:6D�.\�  $ 0:0:0.9��$ 0Q�  $ 0:0:2.5��$	 240Q�   $ EaseOut=�  $ 0:0:4.9��$	 540Q�   $ EaseIn=�  $ 0:0:4.9��$ 0Q��� $ 2Q�.��U� $8 2(UIElement.RenderTransform).(TranslateTransform.X)�$ ellipse4��$	 Forever��$
 0:0:6D�.\�  $ 0:0:1.2��$ 0Q�  $ 0:0:2.5��$	 240Q�   $ EaseOut=�  $ 0:0:5.2��$	 540Q�   $ EaseIn=�  $ 0:0:5.2��$ 0Q� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��.[��� #��[  �� #��[ �� #��[ �� #��[ �� #��[ .��� -    �Name$ 
LayoutRoot�� � RowDefinitions �� �� $��35 �.+��� $��SmartPD��
�������$��16(�$��10,10,0,136q�$��Italic$�$
��Bold"��� $��Loading��
�������$��16(�$��190,90,189,56q�$��Italic$�$
��Bold"� J� RenderTransformOrigin$ 0.984,0.352&�A� -    A�Name$ ellipse��
�������$
��Left=�$��
-8,130,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� A� -   $ ellipse1��
�������$
��Left=�$��-16,130,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� A� -   $ ellipse2��
�������$
��Left=�$��-24,130,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� A� -   $ ellipse3��
�������$
��Left=�$��-32,130,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� A� -   $ ellipse4��
�������$
��Left=�$��-40,130,0,0q�
�������$��5��$��5��$	��Top=�$ 0.5,0.5&�z�T� �� -    ��Name$ 
txtProcess��$��1��$��Version��
�������$��12(�$
��Left=�$
��Bold"�$
��10,0q�