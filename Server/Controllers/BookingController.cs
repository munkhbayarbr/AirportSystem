using Microsoft.AspNetCore.Mvc;
using Server.DA;
using Server.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;

        public BookingController(ILogger<BookingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById([FromServices] AirportDB airportdb, int id)
        {
            var booking = await airportdb.Booking.GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetBookings([FromServices] AirportDB airportdb)
        {
            var bookings = await airportdb.Booking.GetBookings();
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromServices] AirportDB airportdb, [FromBody] BookingCreateDTO dto)
        {         
            SeatDTO seat = await airportdb.Seat.GetSeat(dto.FlightId, dto.SeatNumber);
            if (seat == null)
            {
                return NotFound(new { message = "Seat not found." });
            }
            if (seat.isOccupied)
            {
                return BadRequest(new { message = "Seat is already occupied." });
            }
            await airportdb.Seat.UpdateSeat(dto.FlightId, dto.SeatNumber, true);
            await airportdb.Booking.AddBooking(dto);
            return Ok(new { message = "Booking added successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking([FromServices] AirportDB airportdb, int id)
        {
            var booking = await airportdb.Booking.GetBooking(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }
            int seatNumber = booking.SeatNumber;
            SeatDTO seat = await airportdb.Seat.GetSeat(id, seatNumber);
            if (seat == null)
            {
                return NotFound(new { message = "Seat not found." });
            }
            await airportdb.Seat.UpdateSeat(id, seatNumber, false);
            await airportdb.Booking.DeleteBooking(id);
            return Ok(new { message = "Booking deleted successfully." });
        }
    }
}
