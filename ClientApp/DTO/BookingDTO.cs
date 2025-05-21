namespace ClientApp.DTO
{
    /// <summary>
    /// DTO for creating a booking.
    /// </summary>
    /// <param name="PassengerId"></param>
    /// <param name="FlightId"></param>
    /// <param name="BookingDate"></param>
    public record BookingCreateDTO(int PassengerId, int FlightId, DateTime BookingDate);

    /// <summary>
    /// DTO for reading a booking.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="PassengerId"></param>
    /// <param name="FlightId"></param>
    /// <param name="SeatNumber"></param>
    /// <param name="BookingDate"></param>
    public record BookingReadDTO(int Id, int PassengerId, int FlightId, int? SeatNumber, DateTime BookingDate);

    /// <summary>
    /// DTO for updating a booking.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="PassengerId"></param>
    /// <param name="FlightId"></param>
    /// <param name="SeatNumber"></param>
    /// <param name="BookingDate"></param>

    public record BookingUpdateDTO(int Id, int PassengerId, int FlightId, int? SeatNumber, DateTime BookingDate);
}
