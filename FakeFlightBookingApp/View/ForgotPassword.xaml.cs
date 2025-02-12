using FakeFlightBookingAPI.Models;
using FakeFlightBookingApp.ViewModel;
using Newtonsoft.Json;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
using MessageBox = System.Windows.MessageBox;

namespace FakeFlightBookingApp.View
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Page
    {


        public ForgotPassword()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (ForgotPasswordViewModel)App.ServiceProvider.GetService(typeof(ForgotPasswordViewModel));

            if (viewModel != null)
            {
                viewModel.ShowMessage += ViewModel_ShowMessage;

                DataContext = viewModel;
            }
            else
            {
                MessageBox.Show("ViewModel could not be resolved.");
            }
        }

        // Event handler to show the message box when the ViewModel triggers the event
        private void ViewModel_ShowMessage(string message)
        {
            MessageBox.Show(message);
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is ForgotPasswordViewModel viewModel)
            {
                viewModel.NewPassword = passwordBox.Password;
            }
        }

        private void PasswordBox_PasswordRetype(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is ForgotPasswordViewModel viewModel)
            {
                viewModel.RepeatPassword = passwordBox.Password;
            }
        }

    }

}
