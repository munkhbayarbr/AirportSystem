namespace ClientApp.DTO
{
    /// <summary>
    /// DTO for a seat.
    /// </summary>
    /// <param name="FlightId"></param>
    /// <param name="SeatNumber"></param>
    /// <param name="isOccupied"></param>
    public record SeatDTO(int FlightId, int SeatNumber, bool isOccupied);
}
