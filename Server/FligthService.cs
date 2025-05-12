namespace Server;


public class FlightService
{
    public List<FlightInfo> Flights { get; } = new List<FlightInfo>();

    public FlightService()
    {
        Flights.Add(new FlightInfo("FF123", "Boarding"));
        Flights.Add(new FlightInfo("AF777", "Boarding"));
        Flights.Add(new FlightInfo("SS137", "Boarding"));
    }

    public List<FlightInfo> GetFlights()
    {
        return Flights;
    }
}
