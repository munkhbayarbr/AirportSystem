namespace Server.DTO
{
    public record BookingCreateDTO(int PassengerId, int FlightId, int SeatNumber, DateTime BookingDate);

    public record BookingReadDTO(int Id, int PassengerId, int FlightId, int SeatNumber, DateTime BookingDate);

    public record BookingUpdateDTO(int Id, int PassengerId, int FlightId, int SeatNumber, DateTime BookingDate);
}
