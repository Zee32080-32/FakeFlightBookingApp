using FakeFlightBookingAPI.Services;
using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace FakeFlightBookingApp
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } // Make this static
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Build configuration from appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            // Register services
            //serviceCollection.AddHttpClient<CreateAccountViewModel>();
            serviceCollection.AddHttpClient<FlightOffersSearchService>();
            serviceCollection.AddHttpClient<AirportLookupService>();
            serviceCollection.Configure<AmadeusOptions>(Configuration.GetSection("Amadeus"));
            serviceCollection.AddSingleton<INavigationService, NavigationService>();

            serviceCollection.AddTransient<FlightTicketPageViewModel>();

            serviceCollection.AddTransient<PaymentPageViewModel>();
            serviceCollection.AddHttpClient<PaymentPageViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7186/api/users/"); 
            });

            serviceCollection.AddTransient<ForgotPasswordViewModel>();
            serviceCollection.AddHttpClient<ForgotPasswordViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7186/api/users/");
            });

            // Register HttpClient for CreateAccountViewModel with BaseAddress
            serviceCollection.AddHttpClient<CreateAccountViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7186/api/users/"); //
            });

            serviceCollection.AddSingleton<LoginViewModel>(); 
            serviceCollection.AddHttpClient<LoginViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7186/api/users/");
            });

            serviceCollection.AddSingleton<ProfilePageViewModel>();
            serviceCollection.AddHttpClient<ProfilePageViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7186/api/users/");
            });


            // Register HttpClient for MainPage with BaseAddress
            serviceCollection.AddHttpClient<MainPageViewModel>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7186/api/users/"); 
            });

            ServiceProvider = serviceCollection.BuildServiceProvider(); // Assign to static property

            base.OnStartup(e);
        }
    }
}


