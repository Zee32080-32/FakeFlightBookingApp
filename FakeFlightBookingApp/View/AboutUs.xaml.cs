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
    /// Interaction logic for AboutUs.xaml
    /// </summary>
    public partial class AboutUs : Page
    {
        public AboutUs()
        {
            InitializeComponent();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            var MainPage = new MainPageView();
            Application.Current.MainWindow.Content = MainPage;
        }
    }
}
