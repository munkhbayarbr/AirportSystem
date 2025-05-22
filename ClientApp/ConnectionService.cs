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

    /// <summary>
    /// Server <-> Client харилцааг хариуцаж ажиллах класс 
    /// </summary>
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

        public delegate void SeatUpdateHandler(int flightId, int seatId);
        public static event SeatUpdateHandler SeatUpdate;

        /// <summary>
        /// Client үүсгэж SignalR тай холболт үүсгэнэ.
        /// </summary>
        /// <returns></returns>

        public static async Task Start() {

            await Task.Delay(5000);
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync("127.0.0.1", 6000);
            _connection = new HubConnectionBuilder().WithUrl("https://localhost:7132/flighthub").WithAutomaticReconnect().Build();



            RegisterEvents();
            await _connection.StartAsync();
            
            
        }

        public static async Task ReqFlight()
        {
            await _connection.InvokeAsync("RequestFlightList");
        }


        /// <summary>
        /// SignalR аас ирэх message үүдийг хариуцах handler үүдийг үүсгэнэ.
        /// </summary>
        public static void RegisterEvents()
        {
            _connection.On < IEnumerable<FlightReadDTO>>("ReceiveAllFlights", (flights) =>
            {
                AllFlight?.Invoke(flights);
            });


            _connection.On <FlightReadDTO>("ReceiveFlightStatusUpdate", (flight) =>
            {
                FlightStatusUpdate?.Invoke(flight);
            });


            _connection.On<int, int>("ReceiveSeatUpdate", (flightId, seatNumber) =>
            {
                SeatUpdate?.Invoke(flightId, seatNumber);
            });
        }


        /// <summary>
        /// Server лүү message илгээх функц
        /// </summary>
        /// <param name="json"></param>
        public static void SendUpdateToServer(string json)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                Console.WriteLine("TCP client is not connected.");
                return;
            }

            try
            {
                NetworkStream stream = _tcpClient.GetStream();

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                stream.Write(jsonBytes, 0, jsonBytes.Length);

                stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send flight status: {ex.Message}");
            }
        }


        
        /// <summary>
        /// Closes connection
        /// </summary>
        /// <returns></returns>


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
