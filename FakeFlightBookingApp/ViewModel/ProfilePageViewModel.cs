using CommunityToolkit.Mvvm.Input;
using FakeFlightBookingApp.View;
using Newtonsoft.Json;
using SharedModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Application = System.Windows.Application;

namespace FakeFlightBookingApp.ViewModel
{
    public class ProfilePageViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private ObservableCollection<BookedFlight> _bookedFlights;

        public ICommand DeleteAccountCommand { get; }
        public ICommand LoadBookedFlightsCommand { get; }

        public ICommand MainPageCommand { get; }


        private string _username;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _statusMessage;



        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public ObservableCollection<BookedFlight> BookedFlights
        {
            get => _bookedFlights;
            set
            {
                _bookedFlights = value;
                OnPropertyChanged(nameof(BookedFlights));
            }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public ProfilePageViewModel(HttpClient httpClient) 
        {
            _httpClient = httpClient;
            _bookedFlights = new ObservableCollection<BookedFlight>();
            DeleteAccountCommand = new RelayCommand(async () => await ExecuteDeleteAccountCommand());

            LoadBookedFlightsCommand = new RelayCommand(async () => await LoadBookedFlights());
            MainPageCommand = new RelayCommand(ExecuteMainPageCommand);

            // Check if UserId exists in Application.Current.Properties
            if (Application.Current.Properties.Contains("UserId"))
            {
                _username = (string)Application.Current.Properties["UserName"];
                _firstName = (string)Application.Current.Properties["FirstName"];
                _lastName = (string)Application.Current.Properties["LastName"];
                _email = (string)Application.Current.Properties["Email"];
            }
            else
            {
                _username = string.Empty; 
                _firstName = string.Empty;
                _lastName = string.Empty;
                _email = string.Empty;
               
            }

        }

        private async void ExecuteMainPageCommand()
        {
            var MainPage = new MainPageView();
            Application.Current.MainWindow.Content = MainPage;
        }

        public void ShowFlightDetails(BookedFlight bookedFlight)
        {
            if (bookedFlight == null)
            {
                StatusMessage = "Could not retrieve flight details.";
                return;
            }

            string classType = bookedFlight.ClassType;

            if (classType.Contains("System.Windows.Controls.ComboBoxItem:"))
            {
                classType = classType.Split(':')[1].Trim();
            }

            string flightDetails = $"Price: {bookedFlight.Price:C}\n" +
                                   $"Arrival Date: {bookedFlight.ArrivalDateTime}\n" +
                                   $"Number of Tickets: {bookedFlight.NumberOfTickets}\n" +
                                   $"Destination: {bookedFlight.Destination}\n" +
                                   $"Class Type: {classType}";

            // Show the new popup window
            FlightDetailsWindow detailsWindow = new FlightDetailsWindow(flightDetails);
            detailsWindow.Owner = Application.Current.MainWindow;
            detailsWindow.ShowDialog(); // Opens as a modal dialog
        }

        private async Task ExecuteDeleteAccountCommand()
        {
            int userId = GetAuthenticatedUserId();
            if (userId == -1)
            {
                MessageBox.Show("User not authenticated.");
            }
            

            var response = await _httpClient.DeleteAsync($"delete-account?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Account deleted successfully.";
                ExecuteMainPageCommand();
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                StatusMessage = $"Failed to delete account: {errorMessage}";
            }

        }
        private async Task LoadBookedFlights()
        {
            int userId = GetAuthenticatedUserId();
            if (userId == -1)
            {
                MessageBox.Show("User not authenticated.");
                return;
            }

            string apiUrl = $"https://localhost:7186/api/BookedFlight/GetBookedFlightByID?id={userId}";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                string responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var flights = JsonConvert.DeserializeObject<List<BookedFlight>>(responseText);
                    if (flights != null)
                    {
                        BookedFlights = new ObservableCollection<BookedFlight>(flights);
                    }
                }
                else
                {
                    StatusMessage = "Failed to fetch booked flights.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error fetching flights: {ex.Message}";
            }
        }

        private int GetAuthenticatedUserId()
        {
            if (Application.Current.Properties.Contains("UserId"))
            {
                return (int)Application.Current.Properties["UserId"];
            }

            return -1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
