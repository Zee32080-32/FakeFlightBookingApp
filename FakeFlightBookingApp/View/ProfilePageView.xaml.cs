using FakeFlightBookingApp.ViewModel;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for ProfilePageView.xaml
    /// </summary>
    public partial class ProfilePageView : Page
    {

        public ProfilePageView()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (ProfilePageViewModel)App.ServiceProvider.GetService(typeof(ProfilePageViewModel));

            if (viewModel != null)
            {
                DataContext = viewModel;
                viewModel.LoadBookedFlightsCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("ViewModel could not be resolved.");
            }
        }

        private void ViewFlightDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BookedFlight bookedFlight)
            {
                // Get the ViewModel from DataContext
                if (DataContext is ProfilePageViewModel viewModel)
                {
                    viewModel.ShowFlightDetails(bookedFlight);
                }
            }
        }

    }
}
