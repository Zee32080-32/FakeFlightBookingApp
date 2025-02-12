using FakeFlightBookingApp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using SharedModels;
using System.Globalization;
using FakeFlightBookingApp.ViewModel;
using Microsoft.Web.WebView2.Wpf;

namespace FakeFlightBookingApp.View
{
    public partial class PaymentPage : Page
    {
        private PaymentPageViewModel _viewModel;
        private WebView2 _paymentWebView;

        public FlightOfferDTO _flightOffer { get; set; }

        public PaymentPage(FlightOfferDTO flightOffer)
        {
            InitializeComponent();
            _flightOffer = flightOffer;


            // Bind flight details to UI
            //FlightDetailsTextBlock.Text = $"Flight Number: {FlightOffer.FlightNumber}";
            //TotalPriceTextBlock.Text = $"${FlightOffer.Price}";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new PaymentPageViewModel(_flightOffer, new HttpClient()); // Manually passing the dependency

            if (_viewModel != null)
            {
                // Subscribe to the PaymentSuccess event
                _viewModel.PaymentSuccess += ViewModel_PaymentSuccess;
                _viewModel.ShowMessage += OnShowMessage;
                _viewModel.NavigateToMainPage += OnNavigateToMainPage;
              

                DataContext = _viewModel; // Set the ViewModel as DataContext
            }
            else
            {
                MessageBox.Show("ViewModel could not be resolved.");
            }

        }

  

        private void PaymentWebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // Check if the URL contains the word "success" to confirm a successful payment
            string currentUrl = PaymentWebView.CoreWebView2.Source;

            if (currentUrl.Contains("success", StringComparison.OrdinalIgnoreCase))
            {
                // Raise the PaymentSuccess event
                _viewModel.OnPaymentSuccess();  // Trigger the event in the ViewModel
            }
        }

        // Event handler for the PaymentSuccess event from the ViewModel
        private async void ViewModel_PaymentSuccess(object sender, EventArgs e)
        {
            // Call the method in the ViewModel that will handle the API call to save the booking
            await _viewModel.HandlePaymentSuccess();
        }

        // Event handler to show the message box when the ViewModel triggers the event
        private void OnShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void OnNavigateToMainPage()
        {
            // Navigate to the main page
            NavigationService.Navigate(new MainPageView());
        }


        private async void ProceedToPayment_Click(object sender, RoutedEventArgs e)
        {
            var checkoutUrl = await _viewModel.ProceedToPayment();

            if (!string.IsNullOrEmpty(checkoutUrl))
            {
                // If the URL is valid, load it into the WebView2 control
                PaymentWebView.Source = new Uri(checkoutUrl);
            }
            else
            {
                MessageBox.Show("Payment URL could not be generated.");
            }
        }


    }
}
