using Microsoft.AspNetCore.Mvc;
using Server.DA;
using Server.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController: ControllerBase
    {
        private readonly ILogger<FlightController> _logger;

        public FlightController(ILogger<FlightController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public FlightReadDTO? GetFlightById(AirportDB airportdb, int id)
        {
            return airportdb.Flight.GetFlight(id);
        }

        [HttpGet("")]
        public IEnumerable<FlightReadDTO> GetFlights(AirportDB airportdb)
        {
            return airportdb.Flight.GetAllFlights();
        }

        [HttpPost]
        public IActionResult AddFlight(AirportDB airportdb, [FromBody] FlightCreateDTO dto)
        {
            airportdb.Flight.AddFlight(dto.FlightNumber, dto.Status, dto.Departure, dto.Arrival, dto.DepartureTime, dto.ArrivalTime);
            return Ok(new { message = "Flight added successfully." });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFlight(AirportDB airportdb, int id) { 
            airportdb.Flight.DeleteFlight(id);
            return Ok(new { message = "Flight deleted successfully." });
        }

        [HttpPut]
        public IActionResult UpdateFlight(AirportDB airportdb, [FromBody] FlightUpdateDTO dto)
        {
            airportdb.Flight.UpdateFlight(dto.Id, dto.FlightNumber, dto.Status, dto.Departure, dto.Arrival, dto.DepartureTime, dto.ArrivalTime);
            return Ok(new { message = "Flight updated successfully." });
        }
    }
}
