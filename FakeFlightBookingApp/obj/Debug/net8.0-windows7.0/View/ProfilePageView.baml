   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      O,clr-namespace:FakeFlightBookingApp.ViewModelFakeFlightBookingApp.ViewModel  5      /    )FakeFlightBookingApp.View.ProfilePageView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5      ;vm,clr-namespace:FakeFlightBookingApp.ViewModel     5      -   5           PageBackgroundStyle#��[  5        N� Title$  ProfilePage��5       � 	Resources 5        DataGridHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value��%�   &	       �� 5   
   "  6,   .�5      ��  � �� �	#FF2E7D326+   �� 5       � ��
 �����6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ Center=�6;   5      5      .��5      � $��10q�6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��3* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5"   
   �� $��0��6   $��0��6"   .+�5#      �� $��Profile Information��6   $��24(�63   
�������$
��Bold"�6T   5$      �� 5&   
   $��20q�6   $��1��6!   $��0��6.   .+�5'      �� $��
Username: ��6   
�������$
��Bold"�6=   �� 5(        RoundedTextBoxStyle#��[ 6    �� 
IsReadOnly$
 True��6]   ��6C   �� *
Username+6]   �� 5)      $��First Name: ��6   
�������$
��Bold"�6@   �� 5*      #��[ 6   $
 True��6^   ��6C   �� *	FirstName+6^   �� 5+      $��Last Name: ��6   
�������$
��Bold"�6>   �� 5,      #��[ 6   $
 True��6]   ��6C   �� *
LastName+6]   �� 5-      $��Email Address: ��6   
�������$
��Bold"�6B   �� 5.      #��[ 6   $
 True��6Z   ��6C   �� *Email+6Z   �� 52      -   5         ��Name$ StatusLabel��52      
�������$��14(�55      $��10q�56      $��10q�57      $
��Left=�6   $��Bottom=�69   ��53      �� *StatusMessage+57   9   59      &   System.Windows.Controls.DataGrid  5<   
   $��1��6   $��1��6    	   ColumnHeaderStyle#	 [ 60   
   AutoGenerateColumns$
 False��6�   $��10q�6�   ���	#FF46B859��6i   �� *BookedFlights+   Columns 5=      0  *System.Windows.Controls.DataGridTextColumn  5>      ,  &System.Windows.Controls.DataGridColumn   Header$ Flight Number��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *FlightNumber+  5?      $ Booking Date��6%    6;   �� *BookingDate+4  .System.Windows.Controls.DataGridTemplateColumn  5@      $ Actions��6)      CellTemplate 5A      �� 5B      .R�5C      �� -   5        $��View Flight Details��5C   %   6C   5D      5F      5H      �� 5K   
   $��2��6   $��2��6"    �� Orientation$ 
Horizontal=�66   $��Center=�6O   $��10q�6l   .+�5L      ��   RoundedButtonStyle#��[ 6   $��Delete Account��6A   
���  ��
�������$	��120��6~   $��5q�6�   ��6�   �� *DeleteAccountCommand+�� 5M      #��[ 6   $��Go to Main Page��6A   $	��120��6[   $��5q�6g   ��6r   �� *MainPageCommand+5N      5O      5P      