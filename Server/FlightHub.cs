using Microsoft.AspNetCore.SignalR;
using Server.DA;
using Server.DTO;


namespace Server
{
    public class FlightHub : Hub
    {

        private readonly AirportDB _airportDb;

        public FlightHub(AirportDB airportDb)
        {
            _airportDb = airportDb;
        }
        public async Task UpdateFlightStatus(FlightUpdateDTO flight)
        {
            await Clients.All.SendAsync("ReceiveFlightStatusUpdate", flight);
        }

        public async Task SeatAssigned(string message, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveSeatMessage", message);
        }

        public async Task TestConcurrent(string message, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("TestMessage", message);
        }


        public async Task RequestFlightList()
        {
            IEnumerable<FlightReadDTO> flights = await _airportDb.Flight.GetAllFlights();

            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.Id}, From: {flight.FlightNumber}");
            }

            await Clients.Caller.SendAsync("ReceiveAllFlights", flights);
        }



    }

}