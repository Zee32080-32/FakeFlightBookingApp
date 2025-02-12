using CommunityToolkit.Mvvm.Input;
using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Services;
using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.View;
using Newtonsoft.Json;
using SharedModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace FakeFlightBookingApp.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        public event Action<Page> NavigateToPage;

        // Properties for data binding
        private string _flightFrom;
        public string FlightFrom
        {
            get => _flightFrom;
            set
            {
                _flightFrom = value;
                OnPropertyChanged();
            }
        }

        private string _flightTo;
        public string FlightTo
        {
            get => _flightTo;
            set
            {
                _flightTo = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _departureDate;
        public DateTime? DepartureDate
        {
            get => _departureDate;
            set
            {
                _departureDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _returnDate;
        public DateTime? ReturnDate
        {
            get => _returnDate;
            set
            {
                _returnDate = value;
                OnPropertyChanged();
            }
        }

        private string _numberOfTickets;
        public string NumberOfTickets
        {
            get => _numberOfTickets;
            set
            {
                _numberOfTickets = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTravelClass;
        public string SelectedTravelClass
        {
            get => _selectedTravelClass;
            set
            {
                _selectedTravelClass = value;
                OnPropertyChanged();
            }
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

        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));  // Notify UI when this changes
            }
        }

        public ObservableCollection<FlightOfferDTO> FlightOffersCollection { get; set; } = new();

        // Command bindings
        public ICommand SearchFlightsCommand { get; }
        public ICommand BuyTicketCommand { get; }
        public ICommand LoginSignCommand { get; }
        public ICommand ViewProfileCommand { get; }


        public MainPageViewModel(HttpClient httpClient)
        {
            int userId = GetAuthenticatedUserId();
            if (userId == -1)
            {
                IsLoggedIn = false;

            }
            else 
            {
                IsLoggedIn = true;
            }
            _httpClient = httpClient;

            SearchFlightsCommand = new RelayCommand(async () => await ExecuteSearchFlightsCommand());
            LoginSignCommand = new RelayCommand(ExecuteGoToLoginPage);
            ViewProfileCommand = new RelayCommand(ExecuteGoToProfilePage);

            // Make sure BuyTicketCommand is a RelayCommand<FlightOfferDTO> (not just RelayCommand)
            BuyTicketCommand = new CommunityToolkit.Mvvm.Input.RelayCommand<FlightOfferDTO>(BuyTicket);
        }

        private async void ExecuteGoToLoginPage()
        {
            var LoginView = new LoginView();
            Application.Current.MainWindow.Content = LoginView;

        }

        private async void ExecuteGoToProfilePage()
        {
            var ProfilePage = new ProfilePageView();
            Application.Current.MainWindow.Content = ProfilePage;

        }

        internal async Task ExecuteSearchFlightsCommand()
        {
            await FetchFlightOffers();

        }

        internal async Task FetchFlightOffers()
        {
            var flightOffersCollection = new List<FlightOfferDTO>();

            StatusMessage = "Searching for flights";

            string searchQuery = $"Flights from {FlightFrom} to {FlightTo} " +
                        $"| Leave on: {DepartureDate} " +
                        (string.IsNullOrEmpty(ReturnDate.ToString()) ? "" : $"| Return on: {ReturnDate} ") +
                        $"| Tickets: {NumberOfTickets} | Class: {SelectedTravelClass}";


            if (string.IsNullOrWhiteSpace(FlightFrom) || string.IsNullOrWhiteSpace(FlightTo) || !DepartureDate.HasValue)
            {
                StatusMessage = "Please fill in all required fields.";
                return;
            }

            try
            {
                string url = $"/api/Flights/search?From={FlightFrom}&To={FlightTo}&" +
                             $"DepartureDate={DepartureDate:yyyy-MM-dd}&ReturnDate={ReturnDate:yyyy-MM-dd}&" +
                             $"NumberOfTickets={NumberOfTickets}&TravelClass={SelectedTravelClass}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var flightOfferResponse = await response.Content.ReadFromJsonAsync<FlightOffersResponse>();

                    if (flightOfferResponse?.Data != null)
                    {
                        FlightOffersCollection.Clear();
                        foreach (var offer in flightOfferResponse.Data)
                        {
                            FlightOffersCollection.Add(new FlightOfferDTO
                            {
                                FlightNumber = offer.Itineraries?.FirstOrDefault()?.Segments?.FirstOrDefault()?.Number,
                                Price = offer.Price.Total,
                                DepartureTime = offer.Itineraries?.FirstOrDefault()?.Segments?.FirstOrDefault()?.Departure?.At.ToString("g"),
                                ArrivalTime = offer.Itineraries?.FirstOrDefault()?.Segments?.FirstOrDefault()?.Arrival?.At.ToString("g"),
                                Duration = offer.Itineraries?.FirstOrDefault()?.Duration,
                                NumberOfTickets = int.TryParse(NumberOfTickets, out var tickets) ? tickets : 0,
                                Origin = offer.Itineraries?.FirstOrDefault()?.Segments?.FirstOrDefault()?.Departure?.IataCode,
                                Destination = offer.Itineraries?.FirstOrDefault()?.Segments?.FirstOrDefault()?.Arrival?.IataCode,
                                AirlineName = offer.ValidatingAirlineCodes?.FirstOrDefault(),
                                ClassType = SelectedTravelClass
                            });
                        }

                        StatusMessage = "Search completed.";

                        NavigateToPage?.Invoke(new FlightTicketPage(FlightOffersCollection, searchQuery));

                    }
                    else
                    {
                        StatusMessage = "No flights found.";
                    }
                }
                else
                {
                    StatusMessage = $"Error: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
            }
        }


        private void BuyTicket(FlightOfferDTO selectedFlight)
        {
            if (selectedFlight == null)
            {
                StatusMessage = "Please select a flight to buy a ticket.";

                return;
            }
            NavigateToPage?.Invoke(new PaymentPage(selectedFlight));

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
                //MessageBox.Show("User not authenticated. Please log in.");
                return -1; // Return an invalid value or handle appropriately
            }

        }

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
