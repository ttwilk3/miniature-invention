   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8df/clr-namespace:LiveCharts.WpfLiveCharts.Wpf  #    LiveCharts.Wpf.DefaultTooltip   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,wpfclr-namespace:LiveCharts.Wpf     -     >�Name$  Control�� � 	Resources  �� 
TargetType �� Property �� Value$   LiveCharts.Wpf.SharedConverter   SharedConverter.   (LiveCharts.Wpf.SharedVisibilityConverter  SharedVisibilityConverter-   'LiveCharts.Wpf.ChartPointLabelConverter  ChartPointLabelConverter5   /LiveCharts.Wpf.ParticipationVisibilityConverter $  ParticipationVisibilityConverter  Bvc��%�   '��       &	  2     &	 7     &	 <     &	 A     &	 F     �� " ��.���  � �� �� *
Foreground+        �� ���� .R�� $��Center=�$��Center=�.+��� $��1q����� *
Background+ J� Effect �� *Effect+ �� CornerRadius �� *CornerRadius+���� *Width+���� *Height+�� ���� *
Background+ �� *CornerRadius+���� *BorderThickness+ �� Padding �� *	Padding+���� *BorderBrush+���� *Width+���� *Height+._���  �� Orientation$ Vertical=�.+��� $��Center=�$
��Bold"����� *Data+	 �� 	Converter#	 [  y�`� 
 `� 	Converter#
 [ .1���  �� Path$
 Data��� $ 	ShowTitle���  � IsSharedSizeScope . ���� *Data.Points+����  �� DataType'   !LiveCharts.Wpf.DataPointViewModel"  .R�� $��2q�$��Center=�$��Center=� � ColumnDefinitions �� $
��Auto ��� $
��Auto � z� SharedSizeGroup$ Title���� $
��Auto �$ Label���� $
��Auto �$ Participation��.+�F�  �� Stretch$
 Fill=����� *
BulletSize+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorLevel$ 1�� �� AncestorType" >����� *
BulletSize+ �� $ FindAncestor=�$ 1��" >����� *Series.StrokeThickness+���� *Series.Stroke+���� *Series.Fill+���� *Series.PointGeometry+y��� *
ShowSeries+ �� ElementName$ Control��#	 [ �� $��1��$��Center=�$��5 0 5 0q����� *Series.Title+y��� *
ShowSeries+$ Control��#	 [ �� $��2��$��Center=����� *
ChartPoint+#	 [ �� $��3��$��Center=�$��5 0 0 0q����� *ChartPoint.Participation+ �� StringFormat$ {0:P}��y��� *DataContext.Data+ �� *FindAncestor+" ��#	 [ 