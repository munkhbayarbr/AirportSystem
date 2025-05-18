using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DA;
using Server.DTO;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Server
{
    public class TcpSocketServer
    {
        private TcpListener _listener;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<FlightHub> _hubContext;
        private bool _isRunning;
        private FlightHub _flighthub;
        private Dictionary<TcpClient, string> _clientConnections = new Dictionary<TcpClient, string>();
        private static readonly ConcurrentDictionary<string, object> SeatLocks = new();

        public TcpSocketServer(HttpClient httpClient, IHubContext<FlightHub> hubContext)
        {
            _httpClient = httpClient;
            _hubContext = hubContext;
            _isRunning = true;


        }

        public async Task Start(string ipAddress, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _listener.Start();
            Console.WriteLine($"TCP Socket Server started at {ipAddress}:{port}");

            Task.Run(() => ListenForClients());
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    var client = _listener.AcceptTcpClient();
                    string connectionId = Guid.NewGuid().ToString();
                    _clientConnections.Add(client, connectionId);
                    Console.WriteLine("New client connected");

                    var clientThread = new Thread(() => HandleClient(client, connectionId));
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                }
            }
        }




        private async void HandleClient(TcpClient client, string connectionId)
        {
            using var stream = client.GetStream();
            var buffer = new byte[4096];
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                while (client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                        if (messageBuilder.ToString().EndsWith("}"))
                        {
                            string jsonString = messageBuilder.ToString();
                            Console.WriteLine($"Received JSON: {jsonString}");
                            if (string.IsNullOrEmpty(jsonString))
                            {
                                Console.WriteLine("Received empty or null JSON string.");
                                continue;
                            }

                            try
                            {
                                JObject obj = JObject.Parse(jsonString);
                                string action = obj["action"]?.ToString();

                                if (action == "updateFlight")
                                {
                                    var flightData = obj["data"];
                                    if (flightData == null)
                                    {
                                        Console.WriteLine("Flight data is null.");
                                        continue;
                                    }

                                    var flightUpdateDTO = new FlightUpdateDTO(
                                        flightData["Id"].Value<int>(),
                                        flightData["FlightNumber"].Value<string>(),
                                        flightData["Status"].Value<string>(),
                                        flightData["Departure"].Value<string>(),
                                        flightData["Arrival"].Value<string>(),
                                        flightData["DepartureTime"].Value<DateTime>(),
                                        flightData["ArrivalTime"].Value<DateTime>(),
                                        flightData["SeatCount"].Value<int>()
                                    );

                                    var jsonContent = new StringContent(
                                        Newtonsoft.Json.JsonConvert.SerializeObject(flightUpdateDTO),
                                        Encoding.UTF8,
                                        "application/json"
                                    );

                                    var apiUrl = "http://localhost:5106/flight";
                                    var response = await _httpClient.PutAsync(apiUrl, jsonContent);
                                    var responseContent = await response.Content.ReadAsStringAsync();

                                    await _flighthub.UpdateFlightStatus(flightUpdateDTO);
                                }
                                else if (action == "bookSeat")
                                {
                                    var seatBooking = obj["data"];
                                    if (seatBooking == null)
                                    {
                                        Console.WriteLine("seatBooking is null.");
                                        continue;
                                    }


                                    int flightId = seatBooking["FlightId"].Value<int>();
                                    int seatNumber = seatBooking["SeatNumber"].Value<int>();

                                    string seatLockKey = $"{flightId}_{seatNumber}";

                                    var seatLock = SeatLocks.GetOrAdd(seatLockKey, new object());

                                    lock (seatLock)
                                    {
                                        var seatDTO = new SeatDTO(
                                            flightId,
                                            seatNumber,
                                            seatBooking["isOccupied"].Value<bool>()
                                        );

                                        var jsonContent = new StringContent(
                                            Newtonsoft.Json.JsonConvert.SerializeObject(seatDTO),
                                            Encoding.UTF8,
                                            "application/json"
                                        );

                                        var apiUrl = "http://localhost:5106/booking";
                                        var response = _httpClient.PutAsync(apiUrl, jsonContent).Result; 
                                        var responseContent = response.Content.ReadAsStringAsync().Result;

                                        var json = JObject.Parse(responseContent);
                                        string message = json["message"]?.ToString() ?? "No message found.";

                                        Task.Run(async () =>
                                        {
                                            await _flighthub.SeatAssigned(message, connectionId);
                                        }).Wait();
                                    }

                                }
                            }
                            catch (JsonReaderException jex)
                            {
                                Console.WriteLine($"JSON parsing error: {jex.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unexpected error: {ex.Message}");
                            }

                            messageBuilder.Clear();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Stream error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Client connection closed.");
            }
        }






        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }
    }
}