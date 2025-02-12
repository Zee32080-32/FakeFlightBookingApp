using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FakeFlightBookingApp.Helpers;
using System.Net.Http.Json;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using FakeFlightBookingApp.View;

namespace FakeFlightBookingApp.ViewModel
{
    public class LoginViewModel
    {
        private readonly HttpClient _httpClient;

        // Properties for binding
        public string UserName { get; set; }
        public string Password { get; set; }


 

        // Command for SignIn Button
        public ICommand SignInCommand { get; }
        public ICommand CreateAccountCommand { get; }
        public ICommand ResetPasswordCommand { get; }


        public LoginViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            SignInCommand = new RelayCommand(ExecuteSignInCommand);
            CreateAccountCommand = new RelayCommand(ExecuteCreateAccountCommand);
            ResetPasswordCommand = new RelayCommand(ExecuteResetPasswordCommand);

        }

        private async void ExecuteCreateAccountCommand()
        {
            var CreateAccount = new CreateAccountView();
            Application.Current.MainWindow.Content = CreateAccount;
        }

        private async void ExecuteResetPasswordCommand()
        {
            var ResetPassword = new ForgotPassword();
            Application.Current.MainWindow.Content = ResetPassword;
        }

        private async void ExecuteSignInCommand()
        {
            // Create a new LoginRequest object
            var loginRequest = new FakeFlightBookingAPI.Models.LoginRequest
            {
                UserName = UserName,
                Password = Password
            };

            // Get the response from the LoginUser method
            var response = await LoginUser(loginRequest);

            if (response.IsSuccessStatusCode)
            {
                // Show success message
                MessageBox.Show("Login successful.");

                // Navigate to the main page (ensure this line is executed after successful login)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Check if the current window or page is of type NavigationWindow
                    var navigationWindow = Application.Current.MainWindow as NavigationWindow;
                    if (navigationWindow != null)
                    {
                        // Navigate to the MainPage
                        navigationWindow.Navigate(new Uri("/View/MainPageView.xaml", UriKind.Relative));
                    }
                });
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        internal async Task<HttpResponseMessage> LoginUser(FakeFlightBookingAPI.Models.LoginRequest loginRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("CustomerLogin", loginRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var customer = await response.Content.ReadFromJsonAsync<CustomerDTO>();

                    if (customer != null)
                    {
                        // Store user data in application properties
                        Application.Current.Properties["UserId"] = customer.Id;
                        Application.Current.Properties["UserName"] = customer.UserName;
                        Application.Current.Properties["FirstName"] = customer.FirstName;
                        Application.Current.Properties["LastName"] = customer.LastName;
                        Application.Current.Properties["Email"] = customer.Email;

                        return response;

                    }
                    else
                    {
                        MessageBox.Show("Error: Invalid response data.");
                        return response;
                    }
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
