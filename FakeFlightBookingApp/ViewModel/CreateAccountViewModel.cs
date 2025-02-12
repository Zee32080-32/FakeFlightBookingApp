using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using FakeFlightBookingApp.View;
using Newtonsoft.Json;

namespace FakeFlightBookingApp.ViewModel
{
    public class CreateAccountViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;


        // Properties bound to the View
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }


        private string _verificationCode;
        public string VerificationCode
        {
            get => _verificationCode;
            set { _verificationCode = value; OnPropertyChanged(); }
        }

        // Command for creating account
        public ICommand CreateAccountCommand { get; private set; }

        // Command for verifying email
        public ICommand VerifyEmailCommand { get; }

        public ICommand LoginPageCommand { get; }

        private async void ExecuteCreateAccountCommand()
        {
            await CreateAccountAsync();
        }

        private async void ExecuteVerifyEmailCommand()
        {
            await VerifyEmailAsync();
        }

        private async void ExecuteGoToLoginPage()
        {
            var LoginView = new LoginView();
            Application.Current.MainWindow.Content = LoginView;

    
        }

        public CreateAccountViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            CreateAccountCommand = new RelayCommand(ExecuteCreateAccountCommand);
            VerifyEmailCommand = new RelayCommand(ExecuteVerifyEmailCommand);
            LoginPageCommand = new RelayCommand(ExecuteGoToLoginPage);

        }

        // Create Account Logic
        internal async Task CreateAccountAsync()
        {
            var newCustomer = new SharedModels.Customer(FirstName, LastName, UserName, Email, Password, PhoneNumber);
            var response = await _httpClient.PostAsJsonAsync("initiate-registration", newCustomer);
            MessageBox.Show($"Response: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Verification email sent. Please check your email.");

            }
            else
            {
                MessageBox.Show("Error creating account.");


            }
        }

        // Email Verification Logic
        internal async Task VerifyEmailAsync()
        {
            var model = new FakeFlightBookingAPI.Models.EmailVerification
            {
                Email = Email,
                Code = VerificationCode
            };

            var response = await _httpClient.PostAsJsonAsync("verify-email-and-create-account", model);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Email successfully verified, and account created.");
            }
            else
            {
                MessageBox.Show("Invalid verification code.");
            }
        }

        // PropertyChanged event for data binding
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
