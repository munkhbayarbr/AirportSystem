using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Test
{
    [TestClass]
    public sealed class SocketTest
    {

        [TestMethod]
        public async Task TcpSocketServer_ShouldHandleConcurrentSeatBookingWithSignalR()
        {
            // Arrange
            var mockHubContext = new Mock<IHubContext<FlightHub>>();
            var mockClients = new Mock<IHubClients>();
            var mockSingleClient1 = new Mock<ISingleClientProxy>();
            var mockSingleClient2 = new Mock<ISingleClientProxy>();

            mockClients.Setup(c => c.Client("client1")).Returns(mockSingleClient1.Object);
            mockClients.Setup(c => c.Client("client2")).Returns(mockSingleClient2.Object);
            mockHubContext.Setup(h => h.Clients).Returns(mockClients.Object);

            string client1Message = null;
            string client2Message = null;


            mockSingleClient1.Setup(c => c.SendCoreAsync("TestMessage", It.IsAny<object[]>(), default))
                .Callback<string, object[], CancellationToken>((method, args, token) =>
                {
                    client1Message = $"{method}: {args[0]}";
                })
                .Returns(Task.CompletedTask);

            mockSingleClient2.Setup(c => c.SendCoreAsync("TestMessage", It.IsAny<object[]>(), default))
                .Callback<string, object[], CancellationToken>((method, args, token) =>
                {
                    client2Message = $"{method}: {args[0]}";
                })
                .Returns(Task.CompletedTask);

            //var httpClient = new HttpClient(new FakeHttpHandler());
            var server = new TcpSocketServerMock(mockHubContext.Object);

            server.Start("127.0.0.1", 7001);


            await Task.Delay(2000); 

            var json1 = new JObject
            {
                ["connectionId"] = "client1",
                ["action"] = "bookSeat",
                ["data"] = new JObject
                {
                    ["key"] = "1",
                    ["status"] = "client1"
                } 
            };

            var json2 = new JObject
            {
                ["connectionId"] = "client2",
                ["action"] = "bookSeat",
                ["data"] = new JObject
                {
                    ["key"] = "1",
                    ["status"] = "client2"
                }
            };


            using var client1 = new TcpClient();
            using var client2 = new TcpClient();

            await client1.ConnectAsync("127.0.0.1", 7001);
            await client2.ConnectAsync("127.0.0.1", 7001);

            using var stream1 = client1.GetStream();
            using var stream2 = client2.GetStream();


            byte[] jsonBytes1 = Encoding.UTF8.GetBytes(json1.ToString());

            byte[] jsonBytes2 = Encoding.UTF8.GetBytes(json2.ToString());
            await stream1.WriteAsync(jsonBytes1, 0, jsonBytes1.Length);
            await stream2.WriteAsync(jsonBytes2, 0, jsonBytes2.Length);

            await Task.Delay(1000);

            Console.WriteLine("\nClient 1 Message:");
            Console.WriteLine(client1Message);
            Console.WriteLine("\nClient 2 Message:");
            Console.WriteLine(client2Message);

            Assert.IsTrue((client1Message?.Contains("client1") ?? false) ||
                         (client2Message?.Contains("client1") ?? false));
            Assert.IsTrue((client1Message?.Contains("client2") ?? false) ||
                         (client2Message?.Contains("client2") ?? false));

            //mockAllClients.Verify(c =>
            //    c.SendCoreAsync("ReceiveSeatUpdate", It.IsAny<object[]>(), default),
            //    Times.Once);

            mockSingleClient1.Verify(c =>
                c.SendCoreAsync("TestMessage", It.IsAny<object[]>(), default),
                Times.Once);

            mockSingleClient2.Verify(c =>
                c.SendCoreAsync("TestMessage", It.IsAny<object[]>(), default),
                Times.Once);

            server.Stop();
        }
    }

    public class FakeHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(@"{ ""message"": ""Test booking success"" }")
            };
            return Task.FromResult(response);
        }
    }


    public class TcpSocketServerMock
    {
        private TcpListener _listener;
        private readonly IHubContext<FlightHub> _hubContext;
        private bool _isRunning;
        private readonly ConcurrentDictionary<TcpClient, string> _clientConnections = new();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> SeatLocks = new();
        public static string value = "success";

        public TcpSocketServerMock(IHubContext<FlightHub> hubContext)
        {
            _hubContext = hubContext;
            _isRunning = true;
        }

        public async Task Start(string ipAddress, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _listener.Start();
            Console.WriteLine($"TCP Socket Server started at {ipAddress}:{port}");

            _ = Task.Run(() => ListenForClients());
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    var client = _listener.AcceptTcpClient();
                    string connectionId = Guid.NewGuid().ToString();
                    _clientConnections.TryAdd(client, connectionId);
                    Console.WriteLine("New client connected");

                    _ = Task.Run(() => HandleClient(client, connectionId));
                }
                
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                }
            }
        }

        private async Task HandleClient(TcpClient client, string connectionId)
        {
            using var stream = client.GetStream();
            var buffer = new byte[1024];
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                while (client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    string currentMessage = messageBuilder.ToString();
                    int lastBraceIndex = currentMessage.LastIndexOf('}');
                    if (lastBraceIndex != -1 && lastBraceIndex == currentMessage.Length - 1)
                    {
                        string jsonString = currentMessage;
                        messageBuilder.Clear();

                        if (string.IsNullOrEmpty(jsonString))
                        {
                            Console.WriteLine("Received empty or null JSON string.");
                            continue;
                        }

                        JObject obj = JObject.Parse(jsonString);
                        string action = obj["action"]?.ToString();
                        string connectionIds = obj["connectionId"]?.ToString();

                        Console.WriteLine(connectionIds);
                        if (action == "bookSeat")
                        {
                            var seatBooking = obj["data"];
                            string key = seatBooking["key"].Value<string>();
                            string status = seatBooking["status"].Value<string>();
                            string seatKey = $"{key}";

                            //await _hubContext.Clients.Client(connectionIds).SendAsync("TestMessage", value);
                            //value = status;

                            var seatLock = SeatLocks.GetOrAdd(seatKey, _ => new SemaphoreSlim(1, 1));
                            await seatLock.WaitAsync();
                            try
                            {
                                value = status;

                                await _hubContext.Clients.Client(connectionIds).SendAsync("TestMessage", value);
                            }
                            finally
                            {
                                seatLock.Release();
                            }





                        }

                    }
                    else if (bytesRead > 0 && currentMessage.Length > 4096 * 2)
                    {
                        Console.WriteLine("Message buffer growing too large, discarding partial message.");
                        messageBuilder.Clear();
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Stream error (client disconnected?): {ex.Message}");
            }
            
            finally
            {
                _clientConnections.TryRemove(client, out _);
                client.Close();
                Console.WriteLine($"Client connection closed. Connection ID: {connectionId}");
            }
        }

        

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
        }
    }
}
