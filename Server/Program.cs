using Microsoft.AspNetCore.SignalR;
using Server;
using Server.DA;





public class Program
{
    public static async Task Main(string[] args)
    {

        // builder обьектыг үүсгэж байна. ASP.NET core -н үндсэн тохиргоог үүсгэдэг.
        var builder = WebApplication.CreateBuilder(args);
        // Өгөгдлийн сантай холбох обьектыг singleton байдлаар бүртгэж байна.
        builder.Services.AddSingleton<AirportDB>(sp => new AirportDB("Datasource=airport.db;"));

        builder.Services.AddControllers();

        //SignalR бүртгэж байна.
        builder.Services.AddSignalR();

        //client бүртгэж байна. API - д хандах боломжтой болгоно.
        builder.Services.AddHttpClient();

        //Cross origin resource sharing тодорхой хостуудаас хандах боломжтой болгоно.
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {
                //Уг хаягаас хүсэлтүүдийг зөвшөөрнө.
                policy.WithOrigins("https://localhost:7163")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        //app обьектыг үүсгэж байна.

        var app = builder.Build();

        //http request -г чиглүүлэх route -г идэвхжүүлж байна.
        app.UseRouting();
        // middleware идэвхжүүлж байна.
        app.UseAuthorization();
        //Cors хэрэглэж зөвшөөрсөн хостуудыг хэрэгжүүлнэ.
        app.UseCors("AllowBlazorClient");

        //Endpoint буюу хаягуудыг бүртгэж Controller болон signalR hub тай холбож өгнө.
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<FlightHub>("/flighthub");
        });



        var serviceProvider = app.Services;

        // client үүсгэхэд хэрэгдэх client factory
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        //хэрэглэгчдэд мэдээлэл дамжуулах SignalR HUB context
        var hubContext = serviceProvider.GetRequiredService<IHubContext<FlightHub>>();

        //socket server үүсгэж асинхрон байдлаар эхлүүлнэ. client болон hub context -г дамжуулна.
        var socketServer = new TcpSocketServer(httpClientFactory.CreateClient(), hubContext);

        _ = Task.Run(async () =>
        {
            await socketServer.Start("127.0.0.1", 6000);
        });


        //ASP.NET aпп зогсох үед дуудагдах event listener
        var lifetime = app.Lifetime;
        lifetime.ApplicationStopping.Register(() =>
        {

            //app зогсох үед TCP socket server -г аюулгүй зогсооно.
            Console.WriteLine("Shutting down TCP socket server...");
            socketServer.Stop();
        });

        //апп -г асинхрон байдлаар ажиллуулна.
        await app.RunAsync();

    }
}

