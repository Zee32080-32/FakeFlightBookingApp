   M S B A M L   `   `   `  ����e  aMicrosoft.Web.WebView2.Wpf, Version=1.0.2903.40, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf  5       FakeFlightBookingApp+   %FakeFlightBookingApp.View.PaymentPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      awebview2Lclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5           PageBackgroundStyle#��[  5
        N� Title$  PaymentPage��5      .��5      �� $��10q�6   .+�5   
   �� 
�������$ ��Review Your Ticket and Pay��6'   $��24(�6I   $
��Bold"�6W   )   #Microsoft.Web.WebView2.Wpf.WebView2  5   
   -   5          Name$ PaymentWebView��5      $	��400��64   $	��600��6A   $��0,20,0,0q�6M   6`   �� 5   
   $��15(�6   
�������$
��Bold"�65   $
��Left=�6G   $��Center=�6b    �� Padding$ 10q�6}   Flight Details: �� 5      �6"   �� *FlightDetails+5      �� 5   
   
�������$��15(�6'   $
��Bold"�65   $
��Left=�6G   $ 10q�6b   Total Price: �� 5      �6   �� *
TotalPrice+5      �� 5   
   -   5          RoundedButtonStyle#��[ 5      $��Proceed to Payment��6=   $��10q�6y   $��Center=�6�   $��Center=�6�   �� 5   
   #��[ 6   $&�� Go to Main Page and cancel order��6=   $��10q�6�   $��Center=�6�   ��6h   �� *MainPageCommand+6�   �� 5!   
   -   5         ��Name$ StatusLabel��5!      
�������$��14(�5$      $
��Left=�5%      ��5"      �� *StatusMessage+5%      5(      5)      