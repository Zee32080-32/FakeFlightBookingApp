   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      .    (FakeFlightBookingApp.View.ForgotPassword   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5      -   5           PageBackgroundStyle#��[  5
        N� Title$  ForgotPassword��5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��Go back to Main Page��6   $
��Left=�5      $��0��5      $��Center=�5        RoundedButtonStyle#��[ 6(   ��6T   �� *MainPageCommand+�� 5   
   $��Password Reset��6   $��24(�6*   $
��Bold"�68   $��Center=�6J   $��0��6g   
�������$��Center=�6�   �� 5"   
   -   5         ��Name$ SendResetCode_Label��5#      
�������$��1��5$      $,��&Enter E-mail for a reset password link��5%      $	��Top=�5&      $
��Left=�6%   �� 5(   
   -   5         ��Name$ EnterEmail_TextBox��5)        RoundedTextBoxStyle#��[ 6)   $��1��5*   $   $��Center=�61   $
��Left=�6L   $	��400��5+      
���   ���5*      �� *Email+5,      �� 5.   
   -   5         ��Name$ SendResetCode_Button��5.      #��[ 5/      $��1��51      $��Send Reset Code��52      $��Center=�6+   $��Center=�6H   $��10,0,0,0q�6c   ��50      �� *SendResetCodeCommand+52   c   �� 54   
   -   5        $ CodeVerify_Label��55      $4��.Enter your Reset Code Below to Change Password��6'   $��2��56      $
��Left=�6   $	��Top=�57      
�������y�59      �� *IsCodeInputVisible+�� 5;   
   -   5        $ CodeVerify_TextBox��5<      #��[ 6)   $��2��5=      $
��Left=�6   $��Center=�5>      
���   �$	��400��5@      ��5<   V   �� *Code+y�5A      �� *IsCodeInputVisible+�� 5C   
   -   5        $ ChangePassword_Label��5D      $*��$Enter your new Password to Change it��6+   $��3��5E      $
��Left=�6   
�������$	��Top=�5F      y�6%   �� *IsPasswordChangeVisible+G� 5H   
   -   5         G�Name$ ChangePassword_TextBox��5I        RoundedPasswordBoxStyle#��[ 6-   $��3��5J      $��Center=�6   $
��Left=�65   $	��400��5K      
���   �y�5M      �� *IsPasswordChangeVisible+�� 5O   
   -	   5        $ RetypePassword_Label��5P      $*��$Enter your new Password again please��6+   $��4��5Q      $
��Left=�6   $	��Top=�65   
�������y�5R      �� *IsPasswordChangeVisible+5S      G� 5U   
   -
   5        $ RetypePassword_TextBox��5V      #��[ 6-   $��4��5W      $
��Left=�6   
���   �$	��400��5Z      $��Center=�5[      y�5X      �� *IsPasswordChangeVisible+5[      � 5^   
   $��5��6    � ColumnDefinitions 5_      �� 5a      $
��Auto �6#   �� 5c      $��10 �6#   �� 5e      $��* �6#   �� 5g      $��10 �6#   �� 5i      $��* �6#   .+�5n      �� -   5        $ StatusLabel��5n      
�������$��14(�5q      $��0��5r      $��10q�5s      $��Center=�6   $��Center=�6;   ��5o      �� *StatusMessage+5s   ;   �� 5w      -   5        $ SaveNewPassword_Button��5x      $��2��5y      #��[ 5z      $+��%Verify Reset Code and Change Password��5|      $��Right=�5~      $��Center=�5      $��1��5�      ��5{      �� *%#VerifyCodeANDsaveNewPasswordCommand+y�5}      �� *IsPasswordChangeVisible+5�      �� 5�      -   5        $ GoToLoginPage��5�      $��4��5�      #��[ 5�      $��Go Back to Login��5�      $��Center=�5�      $
��Left=�5�      ��5�      �� *LoginPageCommand+5�      5�      5�      5�      