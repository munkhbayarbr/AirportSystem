using ClientApp.DTO;
using ClinetApp.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ClientApp
{
    public static class ConnectionService
    {
        private static HubConnection _connection;
        private static TcpClient _tcpClient;
        
        public static HubConnection Connection => _connection;
        public static TcpClient TcpClient => _tcpClient;
        public delegate void FlightStatusUpdateHandler(FlightReadDTO flight);
        public static event FlightStatusUpdateHandler FlightStatusUpdate;

        public delegate void AllFlightHandler(IEnumerable<FlightReadDTO> flights);
        public static event AllFlightHandler AllFlight;

        public static async Task Start() {
            _tcpClient = new TcpClient();
            _connection = new HubConnectionBuilder().WithUrl("https://localhost:7132/flighthub").WithAutomaticReconnect().Build();



            RegisterEvents();

            await _connection.StartAsync();
            await _connection.InvokeAsync("RequestFlightList");
        }

        public static void RegisterEvents()
        {
            _connection.On < IEnumerable<FlightReadDTO>>("ReceiveAllFlights", (flights) =>
            {
                AllFlight?.Invoke(flights);
            });


            _connection.On <FlightReadDTO>("ReceiveUpdatedFlight", (flight) =>
            {
                FlightStatusUpdate?.Invoke(flight);
            });
        }



        public static async Task Stop()
        {
            try
            {
                if (_connection != null)
                {
                    await _connection.StopAsync();
                    await _connection.DisposeAsync();
                    _connection = null;
                }

                if (_tcpClient != null && _tcpClient.Connected)
                {
                    _tcpClient.Close();
                    _tcpClient.Dispose();
                    _tcpClient = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while closing connection: {ex.Message}");
            }
        }





    }
}
