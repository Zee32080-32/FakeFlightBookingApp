# DESCRIPTION OF PROJECT
- Flight Booking app using WPF and ASP.Net Core
- stores user details on to Azure SQL
- can create an account
- can retireve password if forgotten using Sendgrid email API 
- Search for Flights using Amadeus API through IATA codes
- purchase ticket using Stripe API
- GUI is made using WPF
- APIs are made using ASP.Net Core

## How to deploy
- open the FakeFlightBookingAPP.sln file
- make sure all three projects are added
- create an account on sendgrid to get the api key
- create an account on stripe developer to get an api key
- create an amadeus account to get an api key
- create an azure account and set up a database with the smae name so you can use the password and userID
