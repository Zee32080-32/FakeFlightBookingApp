using FakeFlightBookingAPI.Models;
using Newtonsoft.Json;
using SharedModels;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using System.Numerics;
using FakeFlightBookingApp.View;

namespace FakeFlightBookingApp.ViewModel
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private string _email;
        private string _code;
        private string _newPassword;
        private string _repeatPassword;
        private string _message;

        private Visibility _isCodeInputVisible = Visibility.Hidden;
        private Visibility _isPasswordChangeVisible = Visibility.Hidden;

        // Event to notify the view to show a message
        public event Action<string> ShowMessage;
        public ICommand LoginPageCommand { get; }
        public ICommand SendResetCodeCommand { get; }
        public ICommand VerifyCodeANDsaveNewPasswordCommand { get; }
        public ICommand MainPageCommand { get; }


        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged(); }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; OnPropertyChanged(); }
        }

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set { _repeatPassword = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }



        public Visibility IsCodeInputVisible
        {
            get { return _isCodeInputVisible; }
            set { _isCodeInputVisible = value; OnPropertyChanged(); }
        }

        public Visibility IsPasswordChangeVisible
        {
            get { return _isPasswordChangeVisible; }
            set { _isPasswordChangeVisible = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ForgotPasswordViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            SendResetCodeCommand = new RelayCommand(async () => await SendResetCodeAsync());
            VerifyCodeANDsaveNewPasswordCommand = new RelayCommand(async () => await VerifyCodeANDsaveNewPasswordAsync());
            MainPageCommand = new RelayCommand(ExecuteGoToMainPage);
            LoginPageCommand = new RelayCommand(ExecuteGoToLoginPage);

        }

        private async void ExecuteGoToMainPage()
        {
            var MainPageView = new MainPageView();
            Application.Current.MainWindow.Content = MainPageView;
        }
        private async void ExecuteGoToLoginPage()
        {
            var LoginView = new LoginView();
            Application.Current.MainWindow.Content = LoginView;

        }

        internal async Task SendResetCodeAsync()
        {
            
            var forgotPassword = new ForgotPassword_Poco
            {
                Email = Email,
                newPassword = string.Empty,
                code = string.Empty
            };

            if (string.IsNullOrEmpty(Email))
            {
                //ShowMessage?.Invoke("Please enter your email address.");
                StatusMessage = "Please enter your email address.";
                return;
            }

            var response = await _httpClient.PostAsJsonAsync("Send-Reset-Password-Email", forgotPassword);


            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "If this email is registered, you will receive a password reset link.";
                IsCodeInputVisible = Visibility.Visible; // Make code input visible
                IsPasswordChangeVisible = Visibility.Visible;

                // You can raise an event to make the fields visible in the View
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                StatusMessage = $"Error: {errorMessage}";
            }
        }

        internal async Task VerifyCodeANDsaveNewPasswordAsync()
        {
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(RepeatPassword))
            {
                //ShowMessage?.Invoke("Please fill in all fields");
                StatusMessage = "Please fill in all fields";
                return;
            }

            if (NewPassword != RepeatPassword)
            {
                StatusMessage = "New passwords do not match.";
                return;
            }

            var forgotPasswordData = new ForgotPassword_Poco
            {
                Email = Email,
                code = Code,
                newPassword = NewPassword
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7186/api/users/verify-code-and-change-password", forgotPasswordData);

            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Password reset successfully.";

            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                StatusMessage = $"Error: {errorMessage}";
            }
        }
      
    }
}
