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
using System.Diagnostics;
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

        private string _randomTravelImage;
        public string RandomTravelImage
        {
            get { return _randomTravelImage; }
            set
            {
                _randomTravelImage = value;
                OnPropertyChanged(nameof(RandomTravelImage));
            }
        }

        private string _userProfileImage;
        public string UserProfileImage
        {
            get => _userProfileImage;
            set
            {
                _userProfileImage = value;
                OnPropertyChanged(nameof(UserProfileImage));
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ObservableCollection<FlightOfferDTO> FlightOffersCollection { get; set; } = new();

        private readonly List<string> _travelImages = new List<string>
        {
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Colombia/Cartegena.jpg",
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Colombia/Food.jpg",

            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Mexico/Aztec.png",
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Mexico/StreetFood.jpg",
            
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Pakistan/Building.jpg",
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Pakistan/NationalMonument.jpg",
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Pakistan/River.jpg",
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Pakistan/SaltMine.jpg",
            
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Romania/Building.jpg",
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Romania/Waterfall.jpg",

            
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Senegal/PinkLake.jpg",
            "pack://application:,,,/FakeFlightBookingApp;component/Pics/Senegal/Touba.jpg"
            
        };

        // Command bindings
        public ICommand SearchFlightsCommand { get; }
        public ICommand BuyTicketCommand { get; }
        public ICommand LoginSignCommand { get; }
        public ICommand ViewProfileCommand { get; }
        public ICommand TravelGuideCommand { get; }
        public ICommand AboutUsCommand { get; }
        public ICommand PrivacyPolicyCommand { get; }



        public MainPageViewModel(HttpClient httpClient)
        {
            SetRandomTravelImage();

            int userId = GetAuthenticatedProperty<int>("UserId");

            if (userId == -1)
            {
                IsLoggedIn = false;

            }
            else 
            {
                IsLoggedIn = true;
                Username = GetAuthenticatedProperty<string>("UserName");
                UserProfileImage = "pack://application:,,,/FakeFlightBookingApp;component/Pics/DefaultUserLogo.jpg";

            }
            _httpClient = httpClient;

            SearchFlightsCommand = new RelayCommand(async () => await ExecuteSearchFlightsCommand());
            ViewProfileCommand = new RelayCommand(ExecuteGoToProfilePage);
            LoginSignCommand = new RelayCommand(ExecuteGoToLoginPage);
            AboutUsCommand = new RelayCommand(ExecuteGoToAboutUsPage);
            PrivacyPolicyCommand = new RelayCommand(ExecuteGoToPrivacyPolicyPage);


            // Make sure BuyTicketCommand is a RelayCommand<FlightOfferDTO> (not just RelayCommand)
            BuyTicketCommand = new CommunityToolkit.Mvvm.Input.RelayCommand<FlightOfferDTO>(BuyTicket);


            // Command to open travel guide
            TravelGuideCommand = new RelayCommand(ExecuteTravelGuide);
            
        }


        private void SetRandomTravelImage()
        {
            if (_travelImages.Count > 0)
            {
                int index = new Random().Next(_travelImages.Count);
                RandomTravelImage = _travelImages[index];

            }
            else
            {
                RandomTravelImage = "pack://application:,,,/FakeFlightBookingApp;component/Pics/Pakistan/Building.jpg";

            }
        }


        private void ExecuteTravelGuide()
        {
            try
            {
                if (string.IsNullOrEmpty(RandomTravelImage))
                {
                    Debug.WriteLine("RandomTravelImage is empty.");
                    return;
                }

                // Extract the country/folder name from the image path
                string[] parts = RandomTravelImage.Split('/');
                if (parts.Length < 6) // Ensure the path has enough parts
                {
                    Debug.WriteLine("Invalid image path: " + RandomTravelImage);
                    return;
                }

                string country = parts[parts.Length - 2]; // Second last part is the country folder

                // Define the mapping of countries to Lonely Planet URLs
                var travelGuideUrls = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Pakistan", "https://www.lonelyplanet.com/pakistan" },
                    { "Romania", "https://www.lonelyplanet.com/romania" },
                    { "Senegal", "https://www.lonelyplanet.com/senegal" },
                    { "Mexico", "https://www.lonelyplanet.com/mexico" },
                    { "Colombia", "https://www.lonelyplanet.com/colombia" }
                };

                if (travelGuideUrls.TryGetValue(country, out string url))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    Debug.WriteLine("No travel guide found for: " + country);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error opening travel guide: " + ex.Message);
            }
        }


        private async void ExecuteGoToLoginPage()
        {
            var LoginView = new LoginView();
            Application.Current.MainWindow.Content = LoginView;

        }

        private async void ExecuteGoToAboutUsPage()
        {
            var aboutUs = new AboutUs();
            Application.Current.MainWindow.Content = aboutUs;

        }

        private async void ExecuteGoToPrivacyPolicyPage()
        {
            var privacyPolicy = new PrivacyPolicy();
            Application.Current.MainWindow.Content = privacyPolicy;

        }
        public async void ExecuteGoToProfilePage()
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
        private T GetAuthenticatedProperty<T>(string propertyKey)
        {
            if (Application.Current.Properties.Contains(propertyKey))
            {
                return (T)Application.Current.Properties[propertyKey];
            }
            else
            {
                // Return a custom default value based on the type
                if (typeof(T) == typeof(int))
                {
                    return (T)(object)-1;  // Return 0 for int
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)(object)"";  // Return an empty string for string
                }
                else
                {
                    return default(T);  // Return the default value for other types
                }
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
