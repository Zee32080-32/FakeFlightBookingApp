   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      O,clr-namespace:FakeFlightBookingApp.ViewModelFakeFlightBookingApp.ViewModel  5      /    )FakeFlightBookingApp.View.ProfilePageView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5      ;vm,clr-namespace:FakeFlightBookingApp.ViewModel     5      -   5          N� 
Background
  ����� N� Title$ ProfilePage��5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��3* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��20q�6   $��0��6!   $��0��6.   .+�5      �� $��Profile Information��6   $��20(�63   $
��Bold"�6A   5      �� 5   
   $��20q�6   $��1��6!   $��0��6.   .+�5      �� $��
Username: ��6   $
��Bold"�6*   �� 5        �� 
IsReadOnly$
 True��60   ��6   �� *
Username+60   �� 5!      $��First Name: ��6   $
��Bold"�6,   �� 5"      $
 True��61   ��6   �� *	FirstName+61   �� 5#      $��Last Name: ��6   $
��Bold"�6+   �� 5$      $
 True��60   ��6   �� *
LastName+60   �� 5%      $��Email Address: ��6   $
��Bold"�6/   �� 5&      $
 True��6-   ��6   �� *Email+6-   5(      &   System.Windows.Controls.DataGrid  5+   
   $��1��6   $��1��6       AutoGenerateColumns$ False��6V   $��20q�6r   ��60   �� *BookedFlights+   Columns 5,      0  *System.Windows.Controls.DataGridTextColumn  5-      ,  &System.Windows.Controls.DataGridColumn   Header$ Flight Number��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *FlightNumber+  5.      $ Booking Date��6%    6;   �� *BookingDate+4  .System.Windows.Controls.DataGridTemplateColumn  5/      $ Actions��6)   	   CellTemplate	 50      �� 51      .R�52      �� -   5        $��View Flight Details��52   %   6C   53      55      57      �� 5:   
   $��2��6   $��2��6"   
 �� Orientation$
 
Horizontal=�66   $��Center=�6O   $��10q�6l   .+�5;      �� $��Delete Account��6   
���  ��
�������$	��120��6R   $��5q�6^   ��6i   �� *DeleteAccountCommand+�� 5<      $��Go to Main Page��6   $	��120��6/   $��5q�6;   ��6F   �� *MainPageCommand+5=      5>      5?      