using Microsoft.AspNetCore.Mvc;
using Server.DA;
using Server.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ILogger<SeatController> _logger;

        public SeatController(ILogger<SeatController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{flightId}/{seatNumber}")]
        public async Task<IActionResult> GetSeatById([FromServices] AirportDB airportdb, int flightId, int seatNumber)
        {
            var seat = await airportdb.Seat.GetSeat(flightId, seatNumber);
            if (seat == null)
            {
                return NotFound();
            }
            return Ok(seat);
        }

        [HttpGet("{flightId}")]
        public async Task<IActionResult> GetSeats([FromServices] AirportDB airportdb, int flightId)
        {
            var seats = await airportdb.Seat.GetSeatsByFlightId(flightId);
            return Ok(seats);
        }

        [HttpPost]
        public async Task<IActionResult> AddSeat([FromServices] AirportDB airportdb, [FromBody] SeatDTO dto)
        {
            await airportdb.Seat.AddSeat(dto.FlightId, dto.SeatNumber, dto.isOccupied);
            return Ok(new { message = "Seat added successfully." });
        }

        [HttpDelete("{flightId}/{seatNumber}")]
        public async Task<IActionResult> DeleteSeat([FromServices] AirportDB airportdb, int flightId, int seatNumber)
        {
            await airportdb.Seat.DeleteSeat(flightId, seatNumber);
            return Ok(new { message = "Seat deleted successfully." });
        }
    }
}
