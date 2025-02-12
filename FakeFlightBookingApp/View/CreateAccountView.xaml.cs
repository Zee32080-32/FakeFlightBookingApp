//using Microsoft.VisualBasic.ApplicationServices;
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
using SharedModels;
using System.Net.Http; // Include for HttpClient
using Newtonsoft.Json;
using System.Net.Http.Json;
using FakeFlightBookingApp.ViewModel; // Include for Json serialization


namespace FakeFlightBookingApp.View
{
    public partial class CreateAccountView : Page
    {

        public CreateAccountView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (CreateAccountViewModel)App.ServiceProvider.GetService(typeof(CreateAccountViewModel));

            if (viewModel != null)
            {
                DataContext = viewModel;
            }
            else
            {
                MessageBox.Show("ViewModel could not be resolved.");
            }
        }
        // Handle the PasswordChanged event to update the ViewModel's Password property
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is CreateAccountViewModel viewModel)
            {
                viewModel.Password = passwordBox.Password;
            }
        }

        // Handle the ConfirmPasswordBox_PasswordChanged event to update the ViewModel's ConfirmPassword property
        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is CreateAccountViewModel viewModel)
            {
                viewModel.ConfirmPassword = passwordBox.Password;
            }
        }


    }
}
