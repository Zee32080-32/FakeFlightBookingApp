using FakeFlightBookingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakeFlightBookingApp.Model
{
    public class NavigationService : INavigationService
    {
        public void NavigateToMainPage()
        {
            var mainPage = new MainPageView();
            Application.Current.MainWindow.Content = mainPage;
        }

        public void NavigateToLoginPage()
        {
            var loginPage = new LoginView();
            Application.Current.MainWindow.Content = loginPage;
        }
    }
}
