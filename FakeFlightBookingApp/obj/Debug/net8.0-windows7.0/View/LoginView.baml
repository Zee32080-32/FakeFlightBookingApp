   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      O,clr-namespace:FakeFlightBookingApp.ViewModelFakeFlightBookingApp.ViewModel  5      )    #FakeFlightBookingApp.View.LoginView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5      ;vm,clr-namespace:FakeFlightBookingApp.ViewModel     5      -   5           PageBackgroundStyle#��[  5
        N� Title$  	LoginView��5      .��5      � $��10q�6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��0.5* �6   �� 5      $��0.75* �6   �� 5      $��0.75* �6   �� 5      $
��0.5* �6   �� 5      $��* �6   .+�5   
   �� $��Go back to Main Page��6   $
��Left=�5      $��Center=�5         RoundedButtonStyle#��[ 6,   $��10q�5!      $��0��5"      $��0��5#      ��5!      �� *MainPageCommand+5#      �� 5&   
   $��24(�6   $��3��5'      $��Center=�5(      $��Welcome Back Please Login��5)      
�������5*      �� 5-   
   -   5         ��Name$ UserName_label��5-      $��1��5.      $��1��6   $	��Top=�5/      
�������$
��Left=�50      $��	User Name��51      �� 53   
     RoundedTextBoxStyle#��[ 54      $��1��55      $��1��6   4  .System.Windows.Automation.AutomationProperties   HelpText$ Enter Your User name��6*   $��Center=�56      ��54   :   �� *
UserName+56      �� 59   
   -   5        $ Password_label��59      
�������$��2��5:       $��Password��5;      $��1��5<      $	��Top=�6   G� 5>   
   -   5        $��2��5?      $��1��6     RoundedPasswordBoxStyle#��[ 5@      $ Enter Your Password��5A      $��Center=�5B   ;   �� 5F   
   -   5        $ StatusLabel��5F      
�������$��14(�5I      $��1��5J      $��2��5K      $��10q�5L      $
��Left=�6   $��Bottom=�65   ��5G      �� *StatusMessage+5L   5   �� 5O   
   $��1��6   $��Reset Password��5P      #��[ 5Q      $��3��5R   6   $��Center=�5S      $	��Top=�6*   ��5R      �� *ResetPasswordCommand+5S   *   � 5X   
   $��1��6   $��4��6    $��0,10,0,0q�6-   $	��Top=�6?   $��Center=�6W    5Y      �� 5Z      $��* �6#   �� 5\      $
��Auto �6#   �� 5^      $��50 �6#   �� 5`      $
��Auto �6#   �� 5b      $��* �6#   .+�5f      �� $��Sign In��5g      $��1��5h      #��[ 6!   $��20q�5i   3   $	��100��6@   ��6   �� *SignInCommand+6@   �� 5l      $��Create Account��5m      $��3��5n      #��[ 6!   ��5o      �� *CreateAccountCommand+5r      5s      5t      