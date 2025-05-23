using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
public class SeatHub : Hub
{
    public async Task JoinFlightGroup(int flightId)
    {
        string groupName = $"flight-{flightId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
}