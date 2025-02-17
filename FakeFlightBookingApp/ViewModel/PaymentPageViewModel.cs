using FakeFlightBookingApp.Model;
using Newtonsoft.Json.Linq;
using SharedModels;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using FakeFlightBookingApp.Helpers;
using Microsoft.Web.WebView2.Wpf;
using FakeFlightBookingApp.View;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using Stripe;
using Application = System.Windows.Application;

namespace FakeFlightBookingApp.ViewModel
{
    public class PaymentPageViewModel : INotifyPropertyChanged
    {
        public event EventHandler PaymentSuccess;
        public event Action<string> ShowMessage;
        public ICommand MainPageCommand { get; }

        public event Action NavigateToMainPage;



        private string _flightDetails;
        private string _totalPrice;
        private FlightOfferDTO _flightOffer;
        private HttpClient _httpClient;

        public PaymentPageViewModel(FlightOfferDTO flightOffer, HttpClient httpClient)
        {
            FlightOffer = flightOffer;
            _httpClient = httpClient;
            FlightDetails = $"Flight Number: {FlightOffer.FlightNumber}";
            TotalPrice = $"${FlightOffer.Price}";
            ProceedToPaymentCommand = new RelayCommand(async () => await ProceedToPayment());
            MainPageCommand = new RelayCommand(ExecuteGoToMainPage);

        }


        private async void ExecuteGoToMainPage()
        {
            var MainPage = new MainPageView();
            Application.Current.MainWindow.Content = MainPage;


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

        public FlightOfferDTO FlightOffer
        {
            get => _flightOffer;
            set
            {
                _flightOffer = value;
                OnPropertyChanged();
            }
        }

        public string FlightDetails
        {
            get => _flightDetails;
            set
            {
                _flightDetails = value;
                OnPropertyChanged();
            }
        }

        public string TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public ICommand ProceedToPaymentCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal async Task<string> ProceedToPayment()
        {
            string apiUrl = "https://localhost:7186/api/payment/create-checkout-session";
            var requestData = new { Amount = FlightOffer.Price.ToString() };

            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);

                    string checkoutUrl = jsonResponse["session"].ToString();
                    return checkoutUrl;

                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    StatusMessage = $"Error: {errorContent}";
                    return null;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
                return null;

            }
        }


        public async Task HandlePaymentSuccess()
        {
            int userId = GetAuthenticatedUserId();

            var bookedFlight = new
            {
                CustomerId = userId,
                FlightNumber = FlightOffer.FlightNumber,
                AirlineName = FlightOffer.AirlineName?.Trim(),
                Origin = FlightOffer.Origin?.Trim(),
                Destination = FlightOffer.Destination?.Trim(),
                DepartureDateTime = DateTime.ParseExact(FlightOffer.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ArrivalDateTime = DateTime.ParseExact(FlightOffer.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                Price = decimal.Parse(FlightOffer.Price, CultureInfo.InvariantCulture),
                NumberOfTickets = FlightOffer.NumberOfTickets,
                ClassType = FlightOffer.ClassType?.Trim(),
                BookingDate = DateTime.UtcNow.ToString("o")
            };

            string apiUrl = "https://localhost:7186/api/BookedFlight/AddBookedFlight";

            try
            {
                var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(bookedFlight);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage = "Booking successful! Taking you to homepage";
                    NavigateToMainPage?.Invoke();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    StatusMessage = $"Failed to save booking: {errorContent} + Try again later please";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
            }
        }
        public void OnPaymentSuccess()
        {
            // Raise the PaymentSuccess event
            PaymentSuccess?.Invoke(this, EventArgs.Empty);
        }


      
        private int GetAuthenticatedUserId()
        {
            // Check if UserId exists in Application.Current.Properties
            if (Application.Current.Properties.Contains("UserId"))
            {
                return (int)Application.Current.Properties["UserId"];
            }
            else
            {
                return -1; 
            }

        }



    }
}
