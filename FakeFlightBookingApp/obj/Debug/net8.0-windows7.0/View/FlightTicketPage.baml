   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      0    *FakeFlightBookingApp.View.FlightTicketPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5        N� Title$  Flight Tickets��5	      .��5      � 
������� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Available Flights��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $��0��5      $��10q�5      �� 5!   
   $��1��6   
������� �� Padding$ 10q�65   $��10q�6B   ._�5"      �� -   5         ��Name$ SearchQueryTextBlock��5"      $��16(�5#      $
��Wrap=�5$       �� TextAlignment$ Center=�5%      5&      �� 5)   
   -   5         ��Name$ FlightOffersListView��5)      $��2��61   $��10q�6>   ��5*      �� 5+      .R�5,      �� $��5q�6!   
�������.+�5-      �� $��Flight Number: ��6$   $
��Bold"�6;   �� 5.      ��6$   �� *FlightNumber+�� 50      $��Price: ��6$   $
��Bold"�63   �� 51      ��6$   �� *Price+�� 53      $��Departure Time: ��6$   $
��Bold"�6<   �� 54      ��6$   �� *DepartureTime+�� 56      $��Arrival Time: ��6$   $
��Bold"�6:   �� 57      ��6$   �� *ArrivalTime+�� 59      $��
Duration: ��6$   $
��Bold"�66   �� 5:      ��6$   �� *
Duration+�� 5<      -   5        $��
Buy Ticket��5<   !   $��0,5,0,0q�5>   !   5?      5@      5B      5C      5D      