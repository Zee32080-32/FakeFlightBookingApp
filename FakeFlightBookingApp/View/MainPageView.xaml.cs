using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FakeFlightBookingApp.View
{
    public partial class MainPageView : Page
    {
        //private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7186") };
        //public ObservableCollection<FlightOfferDTO> FlightOffersCollection { get; set; } = new ObservableCollection<FlightOfferDTO>();

        public MainPageView()
        {
            InitializeComponent();
            
            //FlightOffers.ItemsSource = FlightOffersCollection;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainPageViewModel)App.ServiceProvider.GetService(typeof(MainPageViewModel));

            if (viewModel != null)
            {
                viewModel.NavigateToPage += ViewModel_NavigateToPage;

                DataContext = viewModel;
            }
            else
            {
                MessageBox.Show("ViewModel could not be resolved.");
            }
        }

        private void ViewModel_NavigateToPage(Page page)
        {
            var navigationWindow = Application.Current.MainWindow as NavigationWindow;
            navigationWindow?.Navigate(page);
        }

        private void TravelImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainPageViewModel mainPageViewModel)
            {
                mainPageViewModel.TravelGuideCommand.Execute(null);
            }
        }

        private void ProfileImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainPageViewModel mainPageViewModel)
            {
                mainPageViewModel.ViewProfileCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("Data context is null");
            }
        }
    }
}
