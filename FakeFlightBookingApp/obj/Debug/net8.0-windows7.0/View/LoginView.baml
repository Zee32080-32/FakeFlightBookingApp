   M S B A M L   `   `   `  ����O  KFakeFlightBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:FakeFlightBookingApp.ViewFakeFlightBookingApp.View  5      O,clr-namespace:FakeFlightBookingApp.ViewModelFakeFlightBookingApp.ViewModel  5      )    #FakeFlightBookingApp.View.LoginView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:FakeFlightBookingApp.View     5      ;vm,clr-namespace:FakeFlightBookingApp.ViewModel     5      -   5          N� Title$  	LoginView��5      .��5      �� 
���   �$��1q�6!   
�������._�5   
   � $	��600��6   $	��400��6    � RowDefinitions 5      �� 5      $
��0.5* �6    �� 5      $
��0.5* �6    �� 5      $
��0.5* �6    �� 5      $
��0.5* �6    �� 5      $
��0.5* �6    .+�5      �� -   5         ��Name$ Login_Label��5      
������ $��36(�6B   $��Center=�6P   $��5,5,5,5q�6t   .��6�   Welcome Back Please Login6�   �� 5      -   5        $ UserName_label��5      
������ $��1��6E   $��1��6R   $	��Top=�6b   $��5,5,5,5q�6z   .��6�   	User Name6�   �� 5      
������ $��1��6I   $��1��6V   $��Bottom=�6f   $��5,5,5,5q�6�   4  .System.Windows.Automation.AutomationProperties   HelpText$ Enter Your User name��6�   ��6   �� *
UserName+6�   �� 5      -   5        $ Password_label��5      
������ $��2��6E   $��1��6R   $	��Top=�6b   $��5,5,5,5q�6z   .��6�   
Password6�   G� 5      -   5        
������ $��2��5      $��1��6   $��Bottom=�5       $��5,5,5,5q�6,   $ Enter Your Password��5!      5"      �� 5%      $��Reset Password��6   $��3��6X   $
��Left=�6e   $	��Top=�6�   $��5,0,0,0q�6�   
������ 
����  ���6/   �� *ResetPasswordCommand+6�   �� 5&      $��Sign In��6   $��Center=�6I   $��3��6f   $��Center=�6s   $��1��6�   $��	0,0,200,0q�6�   $��20(�6�   ��6'   �� *SignInCommand+6�   �� 5'      $��Create Account��6   $��Center=�6W   $��3��6t   $��Center=�6�   $��1��6�   $��	200,0,0,0q�6�   $��20(�6�   ��6.   �� *CreateAccountCommand+6�   5(      5)      5*      