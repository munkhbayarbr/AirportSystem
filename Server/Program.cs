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
        //client бүртгэж байна. API - д хандах боломжтой болгоно.
        //Cross origin resource sharing тодорхой хостуудаас хандах боломжтой болгоно.
        //Уг хаягаас хүсэлтүүдийг зөвшөөрнө.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSignalR();
        builder.Services.AddHttpClient();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {

                policy.WithOrigins("https://localhost:7163")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });


        //app обьектыг үүсгэж байна.
        //http request -г чиглүүлэх route -г идэвхжүүлж байна.
        // middleware идэвхжүүлж байна.
        //Cors хэрэглэж зөвшөөрсөн хостуудыг хэрэгжүүлнэ.
        //Endpoint буюу хаягуудыг бүртгэж Controller болон signalR hub тай холбож өгнө.
    
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseAuthorization();
        app.UseCors("AllowBlazorClient");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<FlightHub>("/flighthub");

        });

        app.MapHub<SeatHub>("/seathub");
        // client үүсгэхэд хэрэгдэх client factory
        //хэрэглэгчдэд мэдээлэл дамжуулах SignalR HUB context
        //socket server үүсгэж асинхрон байдлаар эхлүүлнэ. client болон hub context -г дамжуулна.
        var serviceProvider = app.Services;

        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        var hubContext = serviceProvider.GetRequiredService<IHubContext<FlightHub>>();

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

