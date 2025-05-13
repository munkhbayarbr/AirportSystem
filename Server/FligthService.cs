using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.IO;


namespace Server;


public class FlightService
{
    public List<FlightInfo> Flights { get; } = new List<FlightInfo>();

    public FlightService()
    {

        byte[] airlineLogo = File.ReadAllBytes("AirlineLogos/japan-airlines.png");
        Flights.Add(new FlightInfo("Japan",airlineLogo,"FF123", "B7" ,DateTime.Now,"Boarding"));
        
    }

    public List<FlightInfo> GetFlights()
    {
        return Flights;
    }
}
