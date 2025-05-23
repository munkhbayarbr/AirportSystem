using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace Server;
public class SeatHub : Hub
{
    public async Task JoinFlightGroup(int flightId)
    {
        string groupName = $"flight-{flightId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task NotifySeatBooked(int flightId, int seatNumber)
    {
        string groupName = $"flight-{flightId}";
        await Clients.Group(groupName).SendAsync("ReceiveSeatBooked", flightId, seatNumber);
    }
}
