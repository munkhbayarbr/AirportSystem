using Microsoft.AspNetCore.Mvc;
using Server.DA;
using Server.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassengerController : ControllerBase
    {
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(ILogger<PassengerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPassengerById([FromServices] AirportDB airportdb, int id)
        {
            var passenger = await airportdb.Passenger.GetPassenger(id);
            if (passenger == null)
            {
                return NotFound();
            }

            return Ok(passenger);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPassengers([FromServices] AirportDB airportdb)
        {
            var passengers = await airportdb.Passenger.GetAllPassengers();
            return Ok(passengers);
        }
    }
}
