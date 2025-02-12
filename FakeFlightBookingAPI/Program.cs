using Microsoft.EntityFrameworkCore;
using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Data;
using FakeFlightBookingAPI.Services;
using System.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Microsoft.Win32;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

//Register EmailService for Dependency Injection (DI) with Scoped lifetime
builder.Services.AddScoped<IEmailService, EmailService>();

//Register interface with PaymentService
builder.Services.AddSingleton<IPaymentService, PaymentService>();

//Register interface with FlightOffersSearchService
builder.Services.AddScoped<IFlightOffersSearchService, FlightOffersSearchService>();

// Register HttpCliennt for this class
builder.Services.AddHttpClient<FlightOffersSearchService>();

//Register the interface with AirportLookupService
builder.Services.AddScoped<IAirportLookupService, AirportLookupService>();

//Register HttpClient for AirportLookupService and configure the client with AmadeusOptions
builder.Services.AddHttpClient<AirportLookupService>()
    .ConfigureHttpClient((sp, client) =>
    {
        // Inject AmadeusOptions to configure the base URL and other settings
        var amadeusOptions = sp.GetRequiredService<IOptions<AmadeusOptions>>().Value;
        client.BaseAddress = new Uri(amadeusOptions.BaseUrl);
    });

//Configue SendGridOptions from appsettings.Json
builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGrid"));

//Configure StripeOptions from appsettings.json and set the Stripe API key
builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection("Stripe"));
Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Configure AmadeusOptions from appsettings.json
builder.Services.Configure<AmadeusOptions>(builder.Configuration.GetSection("Amadeus"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
