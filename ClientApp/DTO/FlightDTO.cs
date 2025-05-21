namespace ClinetApp.DTO
{
    /// <summary>
    /// DTO for creating a flight.
    /// </summary>
    /// <param name="FlightNumber"></param>
    /// <param name="Status"></param>
    /// <param name="Departure"></param>
    /// <param name="Arrival"></param>
    /// <param name="DepartureTime"></param>
    /// <param name="ArrivalTime"></param>
    /// <param name="SeatCount"></param>
    public record FlightCreateDTO(string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime, int SeatCount);


    /// <summary>
    /// DTO for reading a flight.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="FlightNumber"></param>
    /// <param name="Status"></param>
    /// <param name="Departure"></param>
    /// <param name="Arrival"></param>
    /// <param name="DepartureTime"></param>
    /// <param name="ArrivalTime"></param>
    /// <param name="SeatCount"></param>
    public record FlightReadDTO(int Id, string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime, int SeatCount);

    /// <summary>
    /// DTO for updating a flight.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="FlightNumber"></param>
    /// <param name="Status"></param>
    /// <param name="Departure"></param>
    /// <param name="Arrival"></param>
    /// <param name="DepartureTime"></param>
    /// <param name="ArrivalTime"></param>
    /// <param name="SeatCount"></param>
    public record FlightUpdateDTO(int Id, string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime, int SeatCount);
}
