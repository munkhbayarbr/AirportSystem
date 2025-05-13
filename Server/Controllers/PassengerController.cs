using Microsoft.AspNetCore.Mvc;
using Server.DA;
using Server.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassengerController
    {

        private readonly ILogger<PassengerController> _logger;

        public PassengerController(ILogger<PassengerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public PassengerReadDTO? GetPassengerById(AirportDB airportdb, int id)
        {
            return airportdb.Passenger.GetPassenger(id);
        }


        [HttpGet("")]
        public IEnumerable<PassengerReadDTO> GetPassengers(AirportDB airportdb)
        {
            return airportdb.Passenger.GetAllPassengers();
        }
    }
}
