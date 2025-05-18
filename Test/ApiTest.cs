using Server;
using Server.DA;
using Server.Controllers;
using Server.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace Test
{
    [TestClass]
    public sealed class ApiTest
    {
        public TestContext TestContext { get; set; }
        public String Server_HostAddress = "http://localhost:5106";


        [TestMethod]
        public async Task GetPassengers()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Server_HostAddress + "/passenger");
            var content = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(content);
            Assert.AreEqual(200, (int)response.StatusCode);
            var passengers = System.Text.Json.JsonSerializer.Deserialize<List<PassengerReadDTO>>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsInstanceOfType(passengers, typeof(IEnumerable<PassengerReadDTO>));
        }

        [TestMethod]
        public async Task GetPassengerById()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Server_HostAddress + "/passenger/1");
            var content = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(content);
            Assert.AreEqual(200, (int)response.StatusCode);
            var passenger = System.Text.Json.JsonSerializer.Deserialize<PassengerReadDTO>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsInstanceOfType(passenger, typeof(PassengerReadDTO));
        }

        [TestMethod]
        public async Task GetFlights()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Server_HostAddress + "/flight");
            var content = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(content);
            Assert.AreEqual(200, (int)response.StatusCode);
            var flights = System.Text.Json.JsonSerializer.Deserialize<List<FlightReadDTO>>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsInstanceOfType(flights, typeof(IEnumerable<FlightReadDTO>));
        }

        [TestMethod]
        public async Task GetFlightById()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Server_HostAddress + "/flight/2");
            var content = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(content);
            Assert.AreEqual(200, (int)response.StatusCode);
            var flight = System.Text.Json.JsonSerializer.Deserialize<FlightReadDTO>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsInstanceOfType(flight, typeof(FlightReadDTO));
        }

        [TestMethod]
        public async Task GetBookings()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Server_HostAddress + "/booking");
            var content = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(content);
            Assert.AreEqual(200, (int)response.StatusCode);
            var bookings = System.Text.Json.JsonSerializer.Deserialize<List<BookingReadDTO>>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsInstanceOfType(bookings, typeof(IEnumerable<BookingReadDTO>));
        }

        [TestMethod]
        public async Task GetBookingsByPassengerId()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Server_HostAddress + "/booking/getByPassportId/1");
            var content = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(content);
            Assert.AreEqual(200, (int)response.StatusCode);
            var bookings = System.Text.Json.JsonSerializer.Deserialize<List<BookingReadDTO>>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsInstanceOfType(bookings, typeof(IEnumerable<BookingReadDTO>));
        }

        [TestMethod]
        public async Task PostRequestFlight()
        {
            using var client = new HttpClient();
            var flight = new FlightCreateDTO(
                FlightNumber: "FL123",
                Status: "On Time",
                Departure: "New York",
                Arrival: "Paris",
                DepartureTime: DateTime.Now,
                ArrivalTime: DateTime.Now.AddHours(2),
                SeatCount: 1
            );
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(flight), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Server_HostAddress + "/flight", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(responseContent);
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateFlight()
        {
            using var client = new HttpClient();
            var flight = new FlightUpdateDTO(
                Id: 1,
                FlightNumber: "FL123",
                Status: "On Time",
                Departure: "New York",
                Arrival: "Paris",
                DepartureTime: DateTime.Now,
                ArrivalTime: DateTime.Now.AddHours(2),
                SeatCount: 1
            );
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(flight), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync(Server_HostAddress + "/flight", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(responseContent);
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public async Task PostRequestBooking()
        {
            using var client = new HttpClient();
            var booking = new BookingCreateDTO(
                PassengerId: 2,
                FlightId: 2,
                BookingDate: DateTime.Now
            );
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(booking), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Server_HostAddress + "/booking", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(responseContent);
            Assert.AreEqual(200, (int)response.StatusCode);
        }


        [TestMethod]
        public async Task GetSeatsByFlightId()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Server_HostAddress + "/seat/1");
            var content = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(content);
            Assert.AreEqual(200, (int)response.StatusCode);
            var seats = System.Text.Json.JsonSerializer.Deserialize<List<int>>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsInstanceOfType(seats, typeof(IEnumerable<int>));
        }
    }
}
