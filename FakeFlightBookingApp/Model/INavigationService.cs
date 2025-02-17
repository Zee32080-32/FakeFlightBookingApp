using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeFlightBookingApp.Model
{
    public interface INavigationService
    {
        void NavigateToMainPage();
        void NavigateToLoginPage();
    }
}
