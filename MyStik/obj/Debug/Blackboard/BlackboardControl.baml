   M S B A M L   `   `   `  ����
  MyStik&     myUserControls.BlackboardControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      e aMicrosoft.Surface.Presentation, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e351s)http://schemas.microsoft.com/surface/2008  5        � 	Resources  5         WindowBackground j� Stretch �� Opacity �� ImageSource  bbItemContainerStyle �� 
TargetType=  7Microsoft.Surface.Presentation.Controls.ScatterViewItem �� Property �� ValueG  AMicrosoft.Surface.Presentation.Controls.ScatterContentControlBase��%  &	        &	 n     �� 5   
   $
 None=�6.   $	 0.6Q�6=   $; 5pack://application:,,,/Resources/WindowBackground.jpg��6K   �� 5   
   "  6-   .�5      ��  � ��$	 320��6*   �� 5       � ��
 �����6+   �� 5       �@ SingleInputRotationMode$&  ProportionalToDistanceFromCenter=�68   �� 5       �@ Center 6'   �� *Center+�� 5       �@ Orientation 6,   �� *Orientation+�� 5       � ��$	 900��6*   �� 5       � ��$	 900��6)   5      5      .��5       � }�.   5)   
   ��%    5-      .+�5.   
   (�  J� Opacity$ .2Q�6   $
��1920��6    $��Stretch=�6-   $��Stretch=�6I    J� IsHitTestVisible .  .��5/      �� $��72(�6   $��	ExtraBold"�6"   
�������$��Stretch=�6P   $��Stretch=�6n   $��Stretch=�6�   $��Stretch=�6�   	 J� Effect	 50      3  -System.Windows.Media.Effects.DropShadowEffect  51      
   ShadowDepth$
 2Q�6'      
BlurRadius$ 10Q�67      Color$ Black��6G   6W   .��52       DEMO53      54      �� 55   
   -   5         ��Name$ 
stackpanel��55      $��20q�6)   $
��Left=�65   $��Bottom=�6P   ��56      �� 57      .��58      �  � Color$ DarkGray��6#    � Offset$ 1Q�64   6A   � 59      $ 
WhiteSmoke��6#   $	 0.2Q�66   6E   � 5:      $ DarkGray��6#   $ 0Q�64   6A   5;      	 5=        5>      $ 20Q�6#   65   .+�5A      �� -   5        $��	demoLabel��5A      $	��200��6%   $��50(�61   
���  ��$��Center=�6P   $
��Bold"�6t   	 5B        5C      $ 15Q�6'   $	 Red��67   6E   .��5D       DEMO6&   �� 5E      
���   �$	��200��6+   $��15(�67   $
��Wrap=�6E    �� TextAlignment$ Center=�6Y   trDir würde die Umsetzung dieser Funktion gefallen? Dann lege deine Karte auf und drücke den Gefällt mir!-Button!5H      ;  5Microsoft.Surface.Presentation.Controls.SurfaceButton  5I      -   5        $��
likeButton��5I      {�.  $	��200��6B   $��50��6N   $��20(�6Z   $��10q�6h   .��6�   Gefällt Mir!6�   5J      9  3Microsoft.Surface.Presentation.Controls.ScatterView  5K   
   -   5        $��blackboardScatterView��5K      #��[ 65   6r   �� 5L   
   $��Center=�6   $�� Bottom=�63    �� Orientation$ 
Horizontal=�6O   .+�5N      L  FMicrosoft.Surface.Presentation.Controls.Primitives.SurfaceToggleButton  -   5        $��button1��5N   $   
 � Tag$ 	sonstiges��63   
�������$	��150��6�   $��50��6�   .��5O      �� $��20(�6   .��6&   	Sonstiges61   5P        5Q      -   5        $��button2��5Q   $   $ 
lernmittel��63   
�������$	��150��6�   $��50��6�   .��5R      �� $��20(�6   .��6&   
Lernmittel62   5S        5T      -   5        $��button3��5T   $   $
 jobs��63   
�������$	��150��6�   $��50��6�   .��5U      �� $��20(�6   .��6&   Jobs6,   5V        5W      -   5        $��button4��5W   $   $ 	wohnungen��63   
�������$	��150��6�   $��50��6�   .��5X      �� $��20(�6   .��6&   
Immobilien62   5Y        5Z      -	   5        $��button5��5Z   $   $ 	nachhilfe��63   
�������$	��150��6�   $��50��6�   .��5[      �� $��20(�6   .��6&   	Nachhilfe61   5\        5^      -
   5        $��allesButton��5^   $   $ alles��67   
�������$	��150��6Y   $��50��6e   .��5_      �� $��20(�6   .��6&   Alles6-   5`      5c      5i      5j      