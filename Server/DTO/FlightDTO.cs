namespace Server.DTO
{
    public record FlightCreateDTO(string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime);
    public record FlightReadDTO(int Id, string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime);

    public record FlightUpdateDTO(int Id, string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime);
}
