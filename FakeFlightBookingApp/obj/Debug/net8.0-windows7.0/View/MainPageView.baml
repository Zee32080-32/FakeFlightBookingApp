   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      K*clr-namespace:FakeFlightBookingApp.HelpersFakeFlightBookingApp.Helpers  5      ,    &FakeFlightBookingApp.View.MainPageView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5      >helpers*clr-namespace:FakeFlightBookingApp.Helpers     5      -   5           PageBackgroundStyle#��[  5
        N� Title$  MainPageView��5       � 	Resources 5      ?   9FakeFlightBookingApp.Helpers.BooleanToVisibilityConverter   BooleanToVisibilityConverter  RoundedComboBoxStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Padding �� 	ClickMode	 d� 	IsChecked
 �� RelativeSource �� Mode �� SelectionBoxItem �� IsDropDownOpen � AllowsTransparency K� IsItemsHost��%Q  &	       &	        5   
   6/   �� 5   
   " ��6-   .�5      ��  � ��
 �����6+   �� 5       � ��
 �����6,   �� 5       � ��$ 1q�60   �� 5       � ��$ 5q�6(   �� 5       � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $��Border��6%   #��z��63   #��z��5      #��z��5      $ 10��5      $ 2q�6/   ._�5    "   � .+�5!   &   d� $��ToggleButton��63   $��2��5"   +   $|�false��5#   +   $ Press=�5%   +   
������ $��0q�5'   +   	 5$   +   �� *IsDropDownOpen+
 �� *TemplatedParent+$ TwoWay=�.��5(   *   �� $��ContentPresenter��6;   $��4q�5)   3   $��Center=�5*   3   $
��Left=�5+   3   #��z 5,   3   5-   '   5.   #   5/      � 52      $��Popup��6$   $��Bottom=�53      #��z 54      $
 True��55      $|�False��56      $��Slide=�57      .��58   "   �� 
�������$ 10��6<   
�������$��1q�6a   ._�59   &   �� $��4q�63   $��0q�6>   .��5:   *   �� $
 True��65   5;   '   5<   #   5=      5>      5?      5A      5B      5C      .��5E      � $��10q�6    � RowDefinitions 5F   
   �� 5G      $
��Auto �6   �� 5H      $
��Auto �6   �� 5I      $��* �6   .+�5M   
   � $��0��6   $��0,0,0,10q�6    � ColumnDefinitions 5N      �� 5O      $��* �6#   �� 5P      $
��Auto �6#   .+�5S      �� $��Fake Flights��6   $
��Left=�5T      $	��Top=�5U      $��24(�5V      $
��Bold"�5W      
�������$��0��5Y      �� 5[       �� Orientation$ 
Horizontal=�6   $��Right=�62   .+�5]      �� $��Login/SignIn��6     RoundedButtonStyle#��[ 5_      ��5^      �� *LoginSignCommand+y�5`      �� *
IsLoggedIn+ �� 	Converter# [  �� ConverterParameter$ invert��5a      �� 5c      $ Vertical=�6   $��Right=�60   .+�5e      �� -   5        $��75��5f      $��75��6    $��0,0,0,10q�6,   $��UniformToFill=�5h       � Cursor$
 Hand��6-   ��5e      �� *UserProfileImage+y�5g      �� *
IsLoggedIn+# [  �� FallbackValue$ 	Collapsed�� J� Clip 5i      @� 5j       @� Center$ 	37.5,37.5&�6*    @� RadiusX$
 37.5Q�6=    @� RadiusY$
 37.5Q�6L   5l      �� 5o      $��14(�66   
�������$��Center=�6�   $��Center=�6�   ��6   �� *
Username+y�6W   �� *
IsLoggedIn+# [ $ 	Collapsed��6�   5p      5q      �� 5s   
   $��1��6   $��Center=�6"   $��5q�6?   .+�5t      �� $ 
Horizontal=�6   $��Center=�62   $��5q�6O   .+�5v      �� $ Vertical=�6   $��5q�64   .+�5w      �� 
�������$��Center=�6/   $��15(�6L   .��6Z   From (IATA Code)6l   �� 5x        RoundedTextBoxStyle#��[ 6   $	��100��6g   ��6K   �� *
FlightFrom+6g   5y      �� 5|      $ Vertical=�6   $��5q�64   .+�5}      �� 
�������$��Center=�6/   $��15(�6L   .��6Z   To (IATA Code)6j   �� 5~      #��[ 6   $	��100��6e   ��6K   �� *
FlightTo+6e   5      �� 5�      $ Vertical=�6   $��5q�64   .+�5�      �� 
�������$��Center=�6/   $��15(�6L   .��6Z   	Departure6e   (  "System.Windows.Controls.DatePicker  5�      $	��120��6H      SelectedDate 6!   �� *DepartureDate+6H   5�      �� 5�      $ Vertical=�6   $��5q�64   .+�5�      �� 
�������$��Center=�6/   $��15(�6L   .��6Z   Return6b     5�      $	��120��6E    6!   �� *
ReturnDate+6E   5�      �� 5�      $ Vertical=�6   $��5q�64   .+�5�      �� 
�������$��Center=�6/   $��15(�6L   .��6Z   	Tickets6c   �� 5�      #��[ 6   $��50��6l   ��6K   �� *NumberOfTickets+6l   5�      �� 5�      $ Vertical=�6   $��5q�64   .+�5�      �� 
�������$��Center=�6/   $��15(�6L   .��6Z   Class6a   �� 5�      #��[ 6   $	��120��6z    �� SelectedItem 6M   �� *SelectedTravelClass+.@�5�      �� $��ECONOMY��6'   �� 5�      $��BUSINESS��6'   �� 5�      $��FIRST��6'   5�      5�      �� 5�      $��Search Flight��6   #��[ 5�      $��	10,20,0,0q�5�      $��15(�5�      $��43��6   ��5�      �� *SearchFlightsCommand+5�      5�      �� 5�      -   5         ��Name$ StatusLabel��5�      
�������$
��Left=�5�   '   $��20(�6B   $��5,5,5,5q�6P   ��5�   "   �� *StatusMessage+5�      �� 5�   
   $��2��6   $��10q�6"   $��Center=�6.   $	��Top=�6K   .+�5�      �� -   5         ��Name$ TravelImage��5�      $	��900��5�      $	��400��6   $��Uniform=�5�      $
 Hand��5�      ��5�      �� *RandomTravelImage+5�      5�      �� 5�   
   $��3��6   $��10q�6"   $��Bottom=�6.   .+�5�      �� $9��3Fake Flights - The best place to book fake flights!��6   
�������$��16(�5�   #   $��Center=�61   �� 5�      $ 
Horizontal=�6   $��Center=�62   $��10q�6O   .+�5�      �� $��About Us��6   #��[ 5�      $��	10,0,10,0q�5�      ��5�      �� *AboutUsCommand+5�      �� 5�      $��Privacy Policy��6   #��[ 5�      ��5�      �� *PrivacyPolicyCommand+5�      5�      5�      5�      5�      