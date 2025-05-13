using Microsoft.Data.Sqlite;
using Server.DTO;

namespace Server.DA
{
    public class PassengerDA
    {
        private readonly string _connectionString;
        public PassengerDA(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public void AddPassenger(string firstName, string lastName, string email, string phoneNumber)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO Passenger (FirstName, LastName, Email, PhoneNumber)
                    VALUES ($firstName, $lastName, $email, $phoneNumber);
                ";
                command.Parameters.AddWithValue("$firstName", firstName);
                command.Parameters.AddWithValue("$lastName", lastName);
                command.Parameters.AddWithValue("$email", email);
                command.Parameters.AddWithValue("$phoneNumber", phoneNumber);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void UpdatePassenger(int passengerId, string firstName, string lastName, string email, string phoneNumber)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
               
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    UPDATE Passenger
                    SET FirstName = $firstName, LastName = $lastName, Email = $email, PhoneNumber = $phoneNumber
                    WHERE Id = $passengerId;
                ";
                command.Parameters.AddWithValue("$passengerId", passengerId);
                command.Parameters.AddWithValue("$firstName", firstName);
                command.Parameters.AddWithValue("$lastName", lastName);
                command.Parameters.AddWithValue("$email", email);
                command.Parameters.AddWithValue("$phoneNumber", phoneNumber);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void DeletePassenger(int passengerId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
               
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    DELETE FROM Passenger
                    WHERE Id = $passengerId;
                ";
                command.Parameters.AddWithValue("$passengerId", passengerId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public PassengerReadDTO? GetPassenger(int passengerId)
        {
            PassengerReadDTO passenger = null;
            using (var connection = new SqliteConnection(_connectionString))
            {
                
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT * FROM Passenger
                    WHERE Id = $passengerId;
                ";
                command.Parameters.AddWithValue("$passengerId", passengerId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        passenger =  new PassengerReadDTO(
                            Id: Convert.ToInt32(reader["Id"]),
                            Firstname: (string)reader["FirstName"],
                            Lastname: (string)reader["LastName"],
                            Birthday: DateTime.Parse(reader["Birthday"].ToString()),
                            PhoneNumber: (string)reader["PhoneNumber"],
                            PassportId: Convert.ToInt32(reader["PassportId"])
                        );
                    }
                }
                
            }
            return passenger;
        }
            public IEnumerable<PassengerReadDTO> GetAllPassengers()
            {
                var passengers = new List<PassengerReadDTO>();
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT * FROM Passenger;
                    ";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var passenger = new PassengerReadDTO(
                                Id: Convert.ToInt32(reader["Id"]),
                                Firstname: (string)reader["FirstName"],
                                Lastname: (string)reader["LastName"],
                                Birthday: DateTime.Parse(reader["Birthday"].ToString()),
                                PhoneNumber: (string)reader["PhoneNumber"],
                                PassportId: Convert.ToInt32(reader["PassportId"])
                            );

                            passengers.Add(passenger);
                        }
                    }
                }
            return passengers;
        }
    }
}
