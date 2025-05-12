using Microsoft.AspNetCore.SignalR;
using Server;





public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
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

        var app = builder.Build();
        app.UseRouting();
        app.UseAuthorization();
        app.UseCors("AllowBlazorClient");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<FlightHub>("/flighthub");
        });



        var serviceProvider = app.Services;
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var hubContext = serviceProvider.GetRequiredService<IHubContext<FlightHub>>();
        var socketServer = new TcpSocketServer(httpClientFactory.CreateClient(), hubContext);

        _ = Task.Run(async () =>
        {
            await socketServer.Start("127.0.0.1", 6000);
        });

        await app.RunAsync();

    }
}

