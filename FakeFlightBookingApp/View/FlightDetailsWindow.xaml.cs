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
using System.Windows.Shapes;

namespace FakeFlightBookingApp.View
{
    /// <summary>
    /// Interaction logic for FlightDetailsWindow.xaml
    /// </summary>
    public partial class FlightDetailsWindow : Window
    {
        public FlightDetailsWindow(string flightDetails)
        {
            InitializeComponent();
            FlightInfoTextBlock.Text = flightDetails;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
