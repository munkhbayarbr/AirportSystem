using Microsoft.AspNetCore.Mvc;
using Server.DA;
using Server.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;

        public FlightController(ILogger<FlightController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightById([FromServices] AirportDB airportdb, int id)
        {
            var flight = await airportdb.Flight.GetFlight(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetFlights([FromServices] AirportDB airportdb)
        {
            var flights = await airportdb.Flight.GetAllFlights();
            return Ok(flights);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight([FromServices] AirportDB airportdb, [FromBody] FlightCreateDTO dto)
        {
            FlightReadDTO flight = await airportdb.Flight.AddFlight(dto.FlightNumber, dto.Status, dto.Departure, dto.Arrival, dto.DepartureTime, dto.ArrivalTime, dto.SeatCount);

            for (int i = 1; i <= dto.SeatCount; i++)
            {
                await airportdb.Seat.AddSeat(flight.Id, i, false);
            }

            return Ok(new { message = "Flight added successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight([FromServices] AirportDB airportdb, int id)
        {
            await airportdb.Flight.DeleteFlight(id);

            var seats = await airportdb.Seat.GetSeatsByFlightId(id);
            foreach (var seat in seats)
            {
                await airportdb.Seat.DeleteSeat(id, seat.SeatNumber);
            }
            return Ok(new { message = "Flight deleted successfully." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFlight([FromServices] AirportDB airportdb, [FromBody] FlightUpdateDTO dto)
        {
            await airportdb.Flight.UpdateFlight(dto.Id, dto.FlightNumber, dto.Status, dto.Departure, dto.Arrival, dto.DepartureTime, dto.ArrivalTime, dto.SeatCount);
            return Ok(new { message = "Flight updated successfully." });
        }
    }
}
