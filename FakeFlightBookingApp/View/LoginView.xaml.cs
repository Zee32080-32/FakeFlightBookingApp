using FakeFlightBookingApp.Model;
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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace FakeFlightBookingApp.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        private readonly HttpClient _httpClient;

        public LoginView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)App.ServiceProvider.GetService(typeof(LoginViewModel));

            if (viewModel != null)
            {
                DataContext = viewModel;
            }
            else
            {
                MessageBox.Show("ViewModel could not be resolved.");
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = this.DataContext as LoginViewModel;
            if (viewModel != null)
            {
                viewModel.Password = passwordBox.Password;
            }
        }

    }
}
