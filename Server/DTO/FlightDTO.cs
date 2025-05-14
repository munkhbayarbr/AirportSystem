namespace Server.DTO
{
    public record FlightCreateDTO(string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime, int SeatCount);
    public record FlightReadDTO(int Id, string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime, int SeatCount);

    public record FlightUpdateDTO(int Id, string FlightNumber, string Status, string Departure, string Arrival, DateTime DepartureTime, DateTime ArrivalTime, int SeatCount);
}
