using FakeFlightBookingApp.Helpers;
using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.ViewModel;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FakeFlightBookingApp.View
{
    /// <summary>
    /// Interaction logic for FlightTicketPage.xaml
    /// </summary>
    public partial class FlightTicketPage : Page
    {
        public IEnumerable<FlightOfferDTO> _flightOffers { get; set; }
        public string _searchQuery { get; set; }
        public FlightTicketPage(IEnumerable<FlightOfferDTO> flightOffers, string searchQuery)
        {
            InitializeComponent();

            _flightOffers = flightOffers;
            _searchQuery = searchQuery;

            // Set the DataContext for binding
            this.DataContext = new FlightTicketPageViewModel(flightOffers, searchQuery);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new FlightTicketPageViewModel(_flightOffers, _searchQuery);

            if (viewModel != null)
            {
                viewModel.ShowMessage += ViewModel_ShowMessage;
                viewModel.NavigateToPaymentPage += ViewModel_NavigateToPaymentPage;


                DataContext = viewModel; // Set the ViewModel as DataContext
            }
            else
            {
                MessageBox.Show("ViewModel could not be resolved.");
            }
        }

        private void ViewModel_ShowMessage(string message)
        {
            MessageBox.Show(message);  // Show the message in the view
        }

        private void ViewModel_NavigateToPaymentPage(FlightOfferDTO selectedFlight)
        {
            // Handle navigation to the PaymentPage when triggered from the ViewModel
            var paymentPage = new PaymentPage(selectedFlight);
            var navigationWindow = Application.Current.MainWindow as NavigationWindow;
            navigationWindow?.Navigate(paymentPage);
        }
    }
}
