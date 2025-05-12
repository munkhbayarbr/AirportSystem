using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class FlightHub : Hub
    {
        public async Task UpdateFlightStatus(string flightId, string newStatus)
        {
            await Clients.All.SendAsync("ReceiveFlightStatusUpdate", flightId, newStatus);
        }

        public async Task SeatAssigned(string flightId, string message)
        {
            Console.WriteLine("in hub");
            await Clients.All.SendAsync("ReceiveMessage", flightId, message);
        }

        public async Task RequestFlightList()
        {
            await Clients.Caller.SendAsync("ReceiveInfo", Data.FlightService.GetFlights());
        }

        public async Task RequestMessage()
        {
            await Clients.Caller.SendAsync("ReceiveHello", "Hello from hub.");
        }

    }

}


