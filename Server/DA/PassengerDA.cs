using Microsoft.Data.Sqlite;
using Server.DTO;

namespace Server.DA
{
    public class PassengerDA
    {
        private readonly string _connectionString;

        public PassengerDA(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddPassenger(PassengerCreateDTO passenger)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Passenger (FirstName, LastName, Birthday, PhoneNumber, PassportId)
                VALUES ($firstName, $lastName, $birthday, $phoneNumber, $passportId);
            ";
            command.Parameters.AddWithValue("$firstName", passenger.Firstname);
            command.Parameters.AddWithValue("$lastName", passenger.Lastname);
            command.Parameters.AddWithValue("$email", passenger.Birthday);
            command.Parameters.AddWithValue("$phoneNumber", passenger.PhoneNumber);
            command.Parameters.AddWithValue("$passportId", passenger.PassportId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePassenger(PassengerUpdateDTO passenger)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE Passenger
                SET FirstName = $firstName, LastName = $lastName, Birthday = $birthday, PhoneNumber = $phoneNumber, PassportId = $passportId
                WHERE Id = $passengerId;
            ";
            command.Parameters.AddWithValue("$firstName", passenger.Firstname);
            command.Parameters.AddWithValue("$lastName", passenger.Lastname);
            command.Parameters.AddWithValue("$email", passenger.Birthday);
            command.Parameters.AddWithValue("$phoneNumber", passenger.PhoneNumber);
            command.Parameters.AddWithValue("$passportId", passenger.PassportId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeletePassenger(int passengerId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM Passenger
                WHERE Id = $passengerId;
            ";
            command.Parameters.AddWithValue("$passengerId", passengerId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<PassengerReadDTO?> GetPassenger(int passengerId)
        {
            PassengerReadDTO? passenger = null;

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT * FROM Passenger
                WHERE Id = $passengerId;
            ";
            command.Parameters.AddWithValue("$passengerId", passengerId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                passenger = new PassengerReadDTO(
                    Id: Convert.ToInt32(reader["Id"]),
                    Firstname: (string)reader["FirstName"],
                    Lastname: (string)reader["LastName"],
                    Birthday: DateTime.Parse(reader["Birthday"].ToString() ?? string.Empty),
                    PhoneNumber: (string)reader["PhoneNumber"],
                    PassportId: Convert.ToInt32(reader["PassportId"])
                );
            }

            return passenger;
        }

        public async Task<IEnumerable<PassengerReadDTO>> GetAllPassengers()
        {
            var passengers = new List<PassengerReadDTO>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT * FROM Passenger;
            ";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var passenger = new PassengerReadDTO(
                    Id: Convert.ToInt32(reader["Id"]),
                    Firstname: (string)reader["FirstName"],
                    Lastname: (string)reader["LastName"],
                    Birthday: DateTime.Parse(reader["Birthday"].ToString() ?? string.Empty),
                    PhoneNumber: (string)reader["PhoneNumber"],
                    PassportId: Convert.ToInt32(reader["PassportId"])
                );

                passengers.Add(passenger);
            }

            return passengers;
        }
    }
}
