using CommunityToolkit.Mvvm.Input;
using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;
using SharedModels;


namespace FakeFlightBookingApp.ViewModel
{
    public class FlightTicketPageViewModel : INotifyPropertyChanged
    {
        public IEnumerable<FlightOfferDTO> FlightOffers { get; set; }
        public string SearchQuery { get; set; }

        public ICommand BuyTicketCommand { get; }
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


        // Event to notify the view to show a message
        public event Action<string> ShowMessage;
        // Event to notify the view to navigate (no direct navigation code in ViewModel)
        public event Action<FlightOfferDTO> NavigateToPaymentPage;
        public FlightTicketPageViewModel(IEnumerable<FlightOfferDTO> flightOffers, string searchQuery)
        {
            FlightOffers = flightOffers;
            SearchQuery = searchQuery;

            BuyTicketCommand = new RelayCommand<FlightOfferDTO>(ExecuteBuyTicketCommand);
            MainPageCommand = new RelayCommand(ExecuteGoToMainPage);

        }

        private async void ExecuteGoToMainPage()
        {
            var MainPageView = new MainPageView();
            Application.Current.MainWindow.Content = MainPageView;


        }



        private void ExecuteBuyTicketCommand(FlightOfferDTO selectedFlight)
        {
            int userId = GetAuthenticatedProperty<int>("UserId");

            if (selectedFlight != null && userId != -1)
            {
                //ShowMessage?.Invoke($"Buying ticket for flight {selectedFlight.FlightNumber}");

                // Trigger the event for the view to handle the navigation
                NavigateToPaymentPage?.Invoke(selectedFlight);
            }
            else
            {
               StatusMessage =  "Please sign in";
            }
        }

        private T GetAuthenticatedProperty<T>(string propertyKey)
        {
            // Check if the property exists in Application.Current.Properties
            if (Application.Current.Properties.Contains(propertyKey))
            {
                return (T)Application.Current.Properties[propertyKey];
            }
            else
            {
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

