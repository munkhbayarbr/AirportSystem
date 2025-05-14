using Microsoft.Data.Sqlite;
using Server.DTO;

namespace Server.DA
{
    public class SeatDB
    {
        private readonly string _connectionString;
        public SeatDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddSeat(int flightId, int seatNumber, bool isOccupied)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Seats (FlightId, SeatNumber, isOccupied)
                VALUES ($flightId, $seatNumber, $isOccupied);
            ";
            command.Parameters.AddWithValue("$flightId", flightId);
            command.Parameters.AddWithValue("$seatNumber", seatNumber);
            command.Parameters.AddWithValue("$isOccupied", isOccupied);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateSeat(int flightId, int seatNumber, bool isOccupied)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE Seats
                SET isOccupied = $isOccupied                
                WHERE SeatNumber = $seatNumber AND FlightId = $flightId;
            ";
            command.Parameters.AddWithValue("$flightId", flightId);
            command.Parameters.AddWithValue("$seatNumber", seatNumber);
            command.Parameters.AddWithValue("$isOccupied", isOccupied);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteSeat(int flightId, int seatNumber)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM Seats
                WHERE SeatNumber = $seatNumber AND FlightId = $flightId;
            ";
            command.Parameters.AddWithValue("$flightId", flightId);
            command.Parameters.AddWithValue("$seatNumber", seatNumber);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Бүх суудлын мэдээллийг FlightId-ээр нь авчирна.
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SeatDTO>> GetSeatsByFlightId(int flightId)
        {
            var seats = new List<SeatDTO>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM Seats
                WHERE FlightId = $flightId;
            ";
            command.Parameters.AddWithValue("$flightId", flightId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                seats.Add(new SeatDTO(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetBoolean(2)
                ));
            }

            return seats;
        }

        /// <summary>
        /// Суудлын мэдээллийг FlightId болон SeatNumber-ээр нь авчирна.
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="seatNumber"></param>
        /// <returns></returns>
        public async Task<SeatDTO?> GetSeat(int flightId, int seatNumber)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM Seats
                WHERE FlightId = $flightId AND SeatNumber = $seatNumber;
            ";
            command.Parameters.AddWithValue("$flightId", flightId);
            command.Parameters.AddWithValue("$seatNumber", seatNumber);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new SeatDTO(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetBoolean(2)
                );
            }

            return null;
        }
    }
}
