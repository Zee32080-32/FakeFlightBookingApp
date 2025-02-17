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
using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.View;
using Newtonsoft.Json;
using SharedModels;

namespace FakeFlightBookingApp.ViewModel
{
    public class CreateAccountViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;

        private Visibility _isVerificationVisible = Visibility.Hidden;
        public Visibility IsVerificationVisible
        {
            get { return _isVerificationVisible; }
            set { _isVerificationVisible = value; OnPropertyChanged(); }
        }

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


        private string _homePageImage;
        public string HomePageImage
        {
            get => _homePageImage;
            set
            {
                _homePageImage = value;
                OnPropertyChanged(nameof(HomePageImage));
            }
        }

        // Command for creating account
        public ICommand CreateAccountCommand { get; private set; }

        // Command for verifying email
        public ICommand VerifyEmailCommand { get; }
        public ICommand LoginPageCommand { get; }
        public ICommand MainPageCommand { get; }

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



        private readonly INavigationService _navigationService;
        public CreateAccountViewModel(HttpClient httpClient, INavigationService navigationService)
        {
            _httpClient = httpClient;
            _navigationService = navigationService;  
            CreateAccountCommand = new RelayCommand(ExecuteCreateAccountCommand);
            VerifyEmailCommand = new RelayCommand(ExecuteVerifyEmailCommand);
            LoginPageCommand = new RelayCommand(ExecuteGoToLoginPage);
            MainPageCommand = new RelayCommand(ExecuteGoToMainPage);

        }

        private async void ExecuteGoToMainPage()
        {
            var MainPageView = new MainPageView();
            Application.Current.MainWindow.Content = MainPageView;
        }

        // Create Account Logic
        internal async Task CreateAccountAsync()
        {

            var newCustomer = new SharedModels.Customer(FirstName, LastName, UserName, Email, Password, PhoneNumber);
            if (Password != ConfirmPassword)
            {
                StatusMessage = "passwords do not match.";
                return;
            }

            var response = await _httpClient.PostAsJsonAsync("initiate-registration", newCustomer);
            StatusMessage = $"Response: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}";

            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Verification email sent. Please check your email.";
                IsVerificationVisible = Visibility.Visible;
            }
            else
            {
                StatusMessage = "Error creating account.";


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
                // Create a new LoginRequest object
                var loginRequest = new FakeFlightBookingAPI.Models.LoginRequest
                {
                    UserName = UserName,
                    Password = Password
                };

                StatusMessage = "Email successfully verified, and account created. Taking you to homepage";

                var loginResponse = await _httpClient.PostAsJsonAsync("CustomerLogin", loginRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (loginResponse.IsSuccessStatusCode)
                {
                    var customer = await loginResponse.Content.ReadFromJsonAsync<CustomerDTO>();

                    if (customer != null)
                    {
                        // Store user data in application properties
                        Application.Current.Properties["UserId"] = customer.Id;
                        Application.Current.Properties["UserName"] = customer.UserName;
                        Application.Current.Properties["FirstName"] = customer.FirstName;
                        Application.Current.Properties["LastName"] = customer.LastName;
                        Application.Current.Properties["Email"] = customer.Email;

                        GoToMainPage();


                    }
                    else
                    {
                        StatusMessage = "Error: Invalid response data.";
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                StatusMessage = "Invalid verification code.";
            }
        }

        // PropertyChanged event for data binding
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GoToMainPage()
        {
            _navigationService.NavigateToMainPage();
        }
    }
}
