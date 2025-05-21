using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
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
        public async Task TcpSocketServer_ShouldAcceptClientAndProcessBookSeat()
        {
            var mockHubContext = new Mock<IHubContext<FlightHub>>();
            var mockClients = new Mock<IHubClients>();
            var mockAllClients = new Mock<IClientProxy>();
            var mockSingleClient = new Mock<ISingleClientProxy>();

            mockClients.Setup(c => c.All).Returns(mockAllClients.Object);
            mockClients.Setup(c => c.Client(It.IsAny<string>())).Returns(mockSingleClient.Object);
            mockHubContext.Setup(h => h.Clients).Returns(mockClients.Object);


            var httpClient = new HttpClient(new FakeHttpHandler());

            var server = new TcpSocketServer(httpClient, mockHubContext.Object);
            var serverTask = server.Start("127.0.0.1", 7001);

            await Task.Delay(500);

            using var client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 7001);
            using var stream = client.GetStream();

            string json = @"
            {
                ""action"": ""bookSeat"",
                ""data"": {
                    ""Id"": 1,
                    ""PassengerId"": 101,
                    ""FlightId"": 202,
                    ""SeatNumber"": 12,
                    ""BookingDate"": ""2024-05-01T12:00:00""
                }
            }";

            byte[] data = Encoding.UTF8.GetBytes(json);
            await stream.WriteAsync(data, 0, data.Length);

            await Task.Delay(1000);

            mockAllClients.Verify(c =>
            c.SendCoreAsync("ReceiveSeatUpdate", It.IsAny<object[]>(), default), Times.Once);

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
}
