   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      O,clr-namespace:FakeFlightBookingApp.ViewModelFakeFlightBookingApp.ViewModel  5      0    *FakeFlightBookingApp.View.FlightTicketPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5      ;vm,clr-namespace:FakeFlightBookingApp.ViewModel     5      -   5           PageBackgroundStyle#��[  5
        N� Title$  Flight Tickets��5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��0.25* �6   .+�5   
   �� $��Available Flights��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $��0��5      $��10q�5      
�������6    �� 5    
   $��1��6    �� Padding$ 10q�6   $��10q�6,   ._�5!      �� $��16(�6   $
��Wrap=�6&    �� TextAlignment$ Center=�6:   
���������6Q   �� *SearchQuery+6n   5"      �� 5%   
   $��2��68   $��10q�6E   ��6   �� *FlightOffers+��6Q   Z� ��5&      �� 5'      .R�5(      �� $��5q�6!   .+�5)      �� $��Flight Number: ��6$   $
��Bold"�6;   
�������6N   �� 5*      
���������6$   �� *FlightNumber+6B   �� 5,      $��Price: ��6$   $
��Bold"�63   
�������6F   �� 5-      
���������6$   �� *Price+6<   �� 5/      $��Departure Time: ��6$   $
��Bold"�6<   
�������6O   �� 50      
���������6$   �� *DepartureTime+6C   �� 52      $��Arrival Time: ��6$   $
��Bold"�6:   
�������6M   �� 53      
���������6$   �� *ArrivalTime+6B   �� 55      $��
Duration: ��6$   $
��Bold"�66   
�������6I   �� 56      
���������6$   �� *
Duration+6?   �� 58      $��
Buy Ticket��6!     RoundedButtonStyle#��[ 5:      $��0,5,0,0q�5<      ��59      �� *DataContext.BuyTicketCommand+ �� RelativeSource ��  �� AncestorType Page��5;      �� *.+5<      5>      5?      5A      �� 5B   
   $��Go back to Main Page��6   $
��Left=�60   $��Center=�6K   #��[ 6f   $��10q�6�   ��6�   �� *MainPageCommand+�� 5D   
   -   5         ��Name$ StatusLabel��5D      
�������$��14(�5G      $
��Left=�5H      $��3��5I      ��5E      �� *StatusMessage+5I      5K      5M      