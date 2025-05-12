using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;


namespace Dashboard
{

    using Microsoft.AspNetCore.SignalR.Client;

    public class FlightHubService
    {
        private readonly HubConnection _hubConnection;

        public FlightHubService(NavigationManager navigationManager)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7132/flighthub")
                .WithAutomaticReconnect()
                .Build();
        }

        public async Task ConnectAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
            }
        }

        public async Task RequestFlightListAsync()
        {
            await _hubConnection.InvokeAsync("RequestFlightList");
        }

        public void OnReceiveFlights(Action<List<FlightInfo>> handler)
        {
            _hubConnection.On("ReceiveInfo", handler);
        }
    }



}
