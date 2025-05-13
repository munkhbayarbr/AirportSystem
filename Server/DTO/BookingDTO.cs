namespace Server.DTO
{
    public record BookingCreateDTO(PassengerReadDTO Passenger, FlightReadDTO Flight, DateTime BookingDate);

    public record BookingReadDTO(int Id, PassengerReadDTO Passenger, FlightReadDTO Flight, DateTime BookingDate);
}
