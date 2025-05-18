using Microsoft.Data.Sqlite;
using Server.DTO;

namespace Server.DA
{
    public class FlightDA
    {
        private readonly string _connectionString;

        /// <summary>
        /// FLight Data Access class constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public FlightDA(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Add a flight
        /// </summary>
        /// <param name="flightNumber"></param>
        /// <param name="status"></param>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="departureTime"></param>
        /// <param name="arrivalTime"></param>
        /// <param name="seatCount"></param>
        /// <returns></returns>
        public async Task<FlightReadDTO> AddFlight(string flightNumber, string status, string departure, string arrival, DateTime departureTime, DateTime arrivalTime, int seatCount)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Flight (FlightNumber, Status, Departure, Arrival, DepartureTime, ArrivalTime, SeatCount)
                VALUES ($flightNumber, $status, $departure, $arrival, $departureTime, $arrivalTime, $seatCount);
            ";
            command.Parameters.AddWithValue("$flightNumber", flightNumber);
            command.Parameters.AddWithValue("$status", status);
            command.Parameters.AddWithValue("$departure", departure);
            command.Parameters.AddWithValue("$arrival", arrival);
            command.Parameters.AddWithValue("$departureTime", departureTime);
            command.Parameters.AddWithValue("$arrivalTime", arrivalTime);
            command.Parameters.AddWithValue("$seatCount", seatCount);

            await command.ExecuteNonQueryAsync();

            // Id - oloh
            var idCommand = connection.CreateCommand();
            idCommand.CommandText = "SELECT last_insert_rowid();";
            var flightId = Convert.ToInt32(await idCommand.ExecuteScalarAsync());
            return new FlightReadDTO(
                Id: flightId,
                FlightNumber: flightNumber,
                Status: status,
                Departure: departure,
                Arrival: arrival,
                DepartureTime: departureTime,
                ArrivalTime: arrivalTime,
                SeatCount: seatCount
            );
        }

        /// <summary>
        /// Update a flight
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="flightNumber"></param>
        /// <param name="status"></param>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="departureTime"></param>
        /// <param name="arrivalTime"></param>
        /// <param name="seatCount"></param>
        /// <returns></returns>
        public async Task UpdateFlight(int flightId, string flightNumber, string status, string departure, string arrival, DateTime departureTime, DateTime arrivalTime, int seatCount)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE Flight
                SET FlightNumber = $flightNumber,
                    Status = $status,
                    Departure = $departure,
                    Arrival = $arrival,
                    DepartureTime = $departureTime,
                    ArrivalTime = $arrivalTime,
                    SeatCount = $seatCount
                WHERE Id = $flightId;
            ";
            command.Parameters.AddWithValue("$flightId", flightId);
            command.Parameters.AddWithValue("$status", status);
            command.Parameters.AddWithValue("$flightNumber", flightNumber);
            command.Parameters.AddWithValue("$departure", departure);
            command.Parameters.AddWithValue("$arrival", arrival);
            command.Parameters.AddWithValue("$departureTime", departureTime);
            command.Parameters.AddWithValue("$arrivalTime", arrivalTime);
            command.Parameters.AddWithValue("$seatCount", seatCount);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Delete a flight
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns></returns>
        public async Task DeleteFlight(int flightId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM Flight
                WHERE Id = $flightId;
            ";
            command.Parameters.AddWithValue("$flightId", flightId);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Get all flights
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FlightReadDTO>> GetAllFlights()
        {
            var flights = new List<FlightReadDTO>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM Flight;
            ";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                flights.Add(new FlightReadDTO(
                    Id: Convert.ToInt32(reader["Id"]),
                    FlightNumber: (string)reader["FlightNumber"],
                    Status: (string)reader["Status"],
                    Departure: (string)reader["Departure"],
                    Arrival: (string)reader["Arrival"],
                    DepartureTime: DateTime.Parse(reader["DepartureTime"].ToString() ?? string.Empty),
                    ArrivalTime: DateTime.Parse(reader["ArrivalTime"].ToString() ?? string.Empty),
                    SeatCount: Convert.ToInt32(reader["SeatCount"])
                ));
            }

            return flights;
        }

        /// <summary>
        /// Get a flight by ID
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns></returns>
        public async Task<FlightReadDTO?> GetFlight(int flightId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM Flight
                WHERE Id = $flightId;
            ";
            command.Parameters.AddWithValue("$flightId", flightId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new FlightReadDTO(
                    Id: Convert.ToInt32(reader["Id"]),
                    FlightNumber: (string)reader["FlightNumber"],
                    Status: (string)reader["Status"],
                    Departure: (string)reader["Departure"],
                    Arrival: (string)reader["Arrival"],
                    DepartureTime: DateTime.Parse(reader["DepartureTime"].ToString() ?? string.Empty),
                    ArrivalTime: DateTime.Parse(reader["ArrivalTime"].ToString() ?? string.Empty),
                    SeatCount: Convert.ToInt32(reader["SeatCount"])
                );
            }

            return null;
        }
    }
}
