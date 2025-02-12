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
            if (selectedFlight != null)
            {
                ShowMessage?.Invoke($"Buying ticket for flight {selectedFlight.FlightNumber}");

                // Trigger the event for the view to handle the navigation
                NavigateToPaymentPage?.Invoke(selectedFlight);
            }
            else
            {
                ShowMessage?.Invoke("Please select a flight to buy a ticket.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

