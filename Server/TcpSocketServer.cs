using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR;
using System.IO;
using System;

namespace Server
{
    public class TcpSocketServer
    {
        private TcpListener _listener;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<FlightHub> _hubContext;
        private bool _isRunning;

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
                    Console.WriteLine("New client connected");

                    var clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                }
            }
        }




        private async void HandleClient(TcpClient client)
        {
            using var stream = client.GetStream();
            var buffer = new byte[4096];

            try
            {
                while (client.Connected)
                {
                    int type = stream.ReadByte();
                    if (type == -1)
                        break;

                    if (type == 1)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        int numberOfInts = bytesRead / sizeof(int);
                        int[] receivedNumbers = new int[numberOfInts];

                        for (int i = 0; i < numberOfInts; i++)
                        {
                            receivedNumbers[i] = BitConverter.ToInt32(buffer, i * sizeof(int));
                        }

                        Console.WriteLine("Received Integers:");
                        foreach (var num in receivedNumbers)
                        {
                            Console.WriteLine(num);
                        }

                        string message = $"Row: {receivedNumbers[0]} Column: {receivedNumbers[1]} taken.";
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "123", message);
                    }
                    else if (type == 2)
                    {
                        byte[] lengthBuffer = new byte[4];
                        await stream.ReadAsync(lengthBuffer, 0, 4);
                        int totalStringBytes = BitConverter.ToInt32(lengthBuffer, 0);

                        byte[] stringDataBuffer = new byte[totalStringBytes];
                        await stream.ReadAsync(stringDataBuffer, 0, totalStringBytes);

                        string allStrings = Encoding.UTF8.GetString(stringDataBuffer);
                        string[] strings = allStrings.Split('|');

                        Console.WriteLine("Received Strings:");
                        foreach (var str in strings)
                        {
                            Console.WriteLine(str);
                        }

                        await _hubContext.Clients.All.SendAsync("ReceiveFlightStatusUpdate", strings[0], strings[1]);
                    }
                    else
                    {
                        Console.WriteLine($"Unknown message type received: {type}");
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
