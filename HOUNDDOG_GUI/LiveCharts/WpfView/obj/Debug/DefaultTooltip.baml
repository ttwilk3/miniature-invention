   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8df/clr-namespace:LiveCharts.WpfLiveCharts.Wpf  5       #    LiveCharts.Wpf.DefaultTooltip   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,wpfclr-namespace:LiveCharts.Wpf     5       -   5          >�Name$  Control��5#       � 	Resources 5$       �� 
TargetType �� Property �� Value$   LiveCharts.Wpf.SharedConverter   SharedConverter.   (LiveCharts.Wpf.SharedVisibilityConverter  SharedVisibilityConverter-   'LiveCharts.Wpf.ChartPointLabelConverter  ChartPointLabelConverter5   /LiveCharts.Wpf.ParticipationVisibilityConverter $  ParticipationVisibilityConverter  Bvc��%�   '��       &	  \     &	 o     &	 �     &	 �     &	 �     �� 5%   
   " ��6   .�5&      ��  � �� 6+   �� *
Foreground+6J   5'        5(   
   6     5)   
   6(     5*   
   6'     5+   
   6/   �� 5,   
   65   5-      ��5.      �� 5/   
   .R�50      � $��Center=�6   $��Center=�6.   .+�51      �� $��1q�6;   ��6   �� *
Background+ J� Effect 6O   �� *Effect+ �� CornerRadius 6i   �� *CornerRadius+��52      �� *Width+��61   �� *Height+�� 53      ��6   �� *
Background+ 6;   �� *CornerRadius+��54      �� *BorderThickness+ �� Padding 6E   �� *	Padding+��55      �� *BorderBrush+��56      �� *Width+��61   �� *Height+._�57      ��  �� Orientation$ Vertical=�6!   .+�58      �� $��Center=�6f   $
��Bold"�6�   ��6$   �� *Data+	 �� 	Converter#	 [  y�59      `� 5:   "   
 `� 	Converter#
 [ 6/   .1�5;   &   ��  �� Path$
 Data�6.   6<   �� 5<   &   $ 	ShowTitle�6.   6A   5=   #   5?      �� 5@       � IsSharedSizeScope . ��6'   �� *Data.Points+��5A      �� 5B   "    �� DataType'   !LiveCharts.Wpf.DataPointViewModel"  6/   .R�5C   &   � $��2q�6+   $��Center=�66   $��Center=�6Q    � ColumnDefinitions 5D   *   �� 5E   .   $
��Auto �6?   �� 5F   .   $
��Auto �6?    z� SharedSizeGroup$ Title��6L   �� 5G   .   $
��Auto �6?   $ Label��6L   �� 5H   .   $
��Auto �6?   $ Participation��6L   .+�5J   *   F�  �� Stretch$
 Fill=�5N   .   ��5J   /   �� *
BulletSize+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorLevel$ 1�� �� AncestorType" >���5K   .   �� *
BulletSize+ �� $ FindAncestor=�$ 1��" >���5L   .   �� *Series.StrokeThickness+��5M   .   �� *Series.Stroke+��6O   �� *Series.Fill+��5N   =   �� *Series.PointGeometry+y�5O   .   �� *
ShowSeries+ �� ElementName$ Control��#	 [ �� 5P   *   $��1��64   $��Center=�6b   $��5 0 5 0q�6}   ��6D   �� *Series.Title+y�5Q   4   �� *
ShowSeries+$ Control��#	 [ �� 5S   *   $��2��64   $��Center=�6�   ��6D   �� *
ChartPoint+#	 [ 6�   �� 5U   *   $��3��64   $��Center=�5V   0   $��5 0 0 0q�6K   ��5U   D   �� *ChartPoint.Participation+ �� StringFormat$ {0:P}��y�5W   0   �� *DataContext.Data+ �� *FindAncestor+" ��#	 [ 5Z   '   5[   #   5]      5^      5_      5`      5a      5c      