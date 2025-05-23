using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.DA;
using Server.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IHubContext<SeatHub> _hubContext;
        public BookingController(ILogger<BookingController> logger, IHubContext<SeatHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Get a booking by id.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="id"></param>
        /// <returns></returns>

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

        /// <summary>
        /// Get all bookings.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetBookings([FromServices] AirportDB airportdb)
        {
            var bookings = await airportdb.Booking.GetBookings();
            return Ok(bookings);
        }

        /// <summary>
        /// Get bookings by passenger id.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="PassportId"></param>
        /// <returns></returns>
        [HttpGet("getByPassportId/{PassportId}")]
        public async Task<IActionResult> GetBookingsByPassengerId([FromServices] AirportDB airportdb, int PassportId)
        {
            var bookings = await airportdb.Booking.GetBookingsByPassportId(PassportId);
            if (bookings == null)
            {
                return NotFound();
            }
            return Ok(bookings);
        }

        /// <summary>
        /// Add a new booking.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromServices] AirportDB airportdb, [FromBody] BookingCreateDTO dto)
        {
            //SeatDTO seat = await airportdb.Seat.GetSeat(dto.FlightId, dto.SeatNumber);
            //if (seat == null)
            //{
            //    return NotFound(new { message = "Seat not found." });
            //}
            //if (seat.isOccupied)
            //{
            //    return BadRequest(new { message = "Seat is already occupied." });
            //}
            //await airportdb.Seat.UpdateSeat(dto.FlightId, dto.SeatNumber, true);
            await airportdb.Booking.AddBooking(dto);
            return Ok(new { message = "Booking added successfully." });
        }

        /// <summary>
        /// Update an existing booking.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateBooking([FromServices] AirportDB airportdb, [FromBody] BookingUpdateDTO dto)
        {
            var booking = await airportdb.Booking.GetBooking(dto.Id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            if (booking.SeatNumber != null)
            {
                if (dto.SeatNumber != null && dto.FlightId != null)
                    return BadRequest(new { message = "Cannot change assigned seat." });
            }

                SeatDTO seat = await airportdb.Seat.GetSeat(dto.FlightId, (int)dto.SeatNumber);
                if (seat == null)
                {
                    return NotFound(new { message = "Seat not found." });
                }
                if (seat.isOccupied)
                {
                    return BadRequest(new { message = "Seat is unavailable." });
                }
                await airportdb.Seat.UpdateSeat(dto.FlightId, (int)dto.SeatNumber, true);
            
            await airportdb.Booking.UpdateBooking(dto);
            await _hubContext.Clients.Group($"flight-{dto.FlightId}")
                .SendAsync("ReceiveSeatBooked", dto.FlightId, dto.SeatNumber);

            return Ok(new { message = "Booking updated successfully." });
        }

        /// <summary>
        /// Delete a booking by id.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking([FromServices] AirportDB airportdb, int id)
        {
            var booking = await airportdb.Booking.GetBooking(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            if (booking.SeatNumber != null) {
                int seatNumber = (int)booking.SeatNumber;
                SeatDTO seat = await airportdb.Seat.GetSeat(booking.FlightId, seatNumber);
                if (seat == null)
                {
                    return NotFound(new { message = "Seat not found." });
                }
                await airportdb.Seat.UpdateSeat(booking.FlightId, seatNumber, false);
            }
         
            await airportdb.Booking.DeleteBooking(id);
            return Ok(new { message = "Booking deleted successfully." });
        }
    }
}
