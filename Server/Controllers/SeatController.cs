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

        /// <summary>
        /// Get a seat by flight id and seat number.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="flightId"></param>
        /// <param name="seatNumber"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all seats by flight id.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="flightId"></param>
        /// <returns></returns>
        [HttpGet("{flightId}")]
        public async Task<IActionResult> GetSeats([FromServices] AirportDB airportdb, int flightId)
        {
            var seats = await airportdb.Seat.GetSeatsByFlightId(flightId);
            return Ok(seats);
        }
        /// <summary>
        /// Add a seat.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddSeat([FromServices] AirportDB airportdb, [FromBody] SeatDTO dto)
        {
            await airportdb.Seat.AddSeat(dto.FlightId, dto.SeatNumber, dto.isOccupied);
            return Ok(new { message = "Seat added successfully." });
        }
        /// <summary>
        /// Update a seat.
        /// </summary>
        /// <param name="airportdb"></param>
        /// <param name="flightId"></param>
        /// <param name="seatNumber"></param>
        /// <returns></returns>
        [HttpDelete("{flightId}/{seatNumber}")]
        public async Task<IActionResult> DeleteSeat([FromServices] AirportDB airportdb, int flightId, int seatNumber)
        {
            await airportdb.Seat.DeleteSeat(flightId, seatNumber);
            return Ok(new { message = "Seat deleted successfully." });
        }
    }
}
