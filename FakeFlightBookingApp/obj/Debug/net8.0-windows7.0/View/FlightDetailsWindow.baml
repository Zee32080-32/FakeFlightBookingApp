   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5   	   3    -FakeFlightBookingApp.View.FlightDetailsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:FakeFlightBookingApp.View     5   	   $	��300��5   	   $	��400��5	   	     � WindowStartupLocation$  CenterOwner=�5
   	    � 
ResizeMode$ NoResize=�5   	   
����������	#FF274DC3$��3q�5   	   $��Segoe UI)�5   	   $��14(�5   	   ��5      �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ 	#FF46B859��6    � Offset$ 1Q�6-   � 5      $ 	#FF46B68E��6   $ 0.072Q�6-   5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       $��40 �6   .+�5$   
   �� $��Flight Details��6   $��18(�5%      $
��Bold"�5&      $��Center=�5'      $��0,0,0,10q�5(      �� 5,   
   -   5         ��Name$ FlightInfoTextBlock��5,      $��1��5-      $
��Wrap=�5.      $��5q�5/      �� 53   
   -   5        $��Close��53         RoundedButtonStyle#��[  54      $��5��55      $	��100��56      $��40��57      $��Center=�58      ���	#FF274DC3
�������$
��Bold"�5<      
������  � Cursor$
 Hand��5>      5@      5A      