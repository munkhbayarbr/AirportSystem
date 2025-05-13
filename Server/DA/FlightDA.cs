using Microsoft.Data.Sqlite;
using Server.DTO;

namespace Server.DA
{
    public class FlightDA
    {
        private readonly string _connectionString;
        public FlightDA(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public void AddFlight(string flightNumber, string status, string departure, string arrival, DateTime departureTime, DateTime arrivalTime)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO Flight (FlightNumber, Status, Departure, Arrival, DepartureTime, ArrivalTime)
                    VALUES ($flightNumber, $status, $departure, $arrival, $departureTime, $arrivalTime);
                ";
                command.Parameters.AddWithValue("$flightNumber", flightNumber);
                command.Parameters.AddWithValue("$status", status);
                command.Parameters.AddWithValue("$departure", departure);
                command.Parameters.AddWithValue("$arrival", arrival);
                command.Parameters.AddWithValue("$departureTime", departureTime);
                command.Parameters.AddWithValue("$arrivalTime", arrivalTime);
                command.ExecuteNonQuery();
            }
        }
        public void UpdateFlight(int flightId, string flightNumber, string status, string departure, string arrival, DateTime departureTime, DateTime arrivalTime)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    UPDATE Flight
                    SET FlightNumber = $flightNumber, Status = $status, Departure = $departure, Arrival = $arrival, DepartureTime = $departureTime, ArrivalTime = $arrivalTime
                    WHERE Id = $flightId;
                ";
                command.Parameters.AddWithValue("$flightId", flightId);
                command.Parameters.AddWithValue("$status", status);
                command.Parameters.AddWithValue("$flightNumber", flightNumber);
                command.Parameters.AddWithValue("$departure", departure);
                command.Parameters.AddWithValue("$arrival", arrival);
                command.Parameters.AddWithValue("$departureTime", departureTime);
                command.Parameters.AddWithValue("$arrivalTime", arrivalTime);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteFlight(int flightId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    DELETE FROM Flight
                    WHERE Id = $flightId;
                ";
                command.Parameters.AddWithValue("$flightId", flightId);
                command.ExecuteNonQuery();
            }
        }
        public IEnumerable<FlightReadDTO> GetAllFlights()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT *
                    FROM Flight;
                ";
                using (var reader = command.ExecuteReader())
                {
                    var flights = new List<FlightReadDTO>();
                    while (reader.Read())
                    {
                        flights.Add(new FlightReadDTO(
                            Id: Convert.ToInt32(reader["Id"]),
                            FlightNumber: (string)reader["FlightNumber"],
                            Status: (string)reader["Status"],
                            Departure: (string)reader["Departure"],
                            Arrival: (string)reader["Arrival"],
                            DepartureTime: DateTime.Parse(reader["DepartureTime"].ToString()),
                            ArrivalTime: DateTime.Parse(reader["ArrivalTime"].ToString())
                        ));
                    }
                    return flights;
                }
            }
        }
        public FlightReadDTO? GetFlight(int flightId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT *
                    FROM Flight
                    WHERE Id = $flightId;
                ";
                command.Parameters.AddWithValue("$flightId", flightId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new FlightReadDTO(
                            Id: Convert.ToInt32(reader["Id"]),
                            FlightNumber: (string)reader["FlightNumber"],
                            Status: (string)reader["Status"],
                            Departure: (string)reader["Departure"],
                            Arrival: (string)reader["Arrival"],
                            DepartureTime: DateTime.Parse(reader["DepartureTime"].ToString()),
                            ArrivalTime: DateTime.Parse(reader["ArrivalTime"].ToString())
                        );
                    }
                    return null;
                }
            }
        }
    }
}
