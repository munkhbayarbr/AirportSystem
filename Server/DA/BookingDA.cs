using Microsoft.Data.Sqlite;
using Server.DTO;

namespace Server.DA
{
    public class BookingDA
    {
        /// <summary>
        /// Connection string for the database.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Constructor for the BookingDA class.
        /// </summary>
        /// <param name="connectionString"></param>
        public BookingDA(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Get all bookings
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookingReadDTO>> GetBookings()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Booking";
                var command = new SqliteCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                var bookings = new List<BookingReadDTO>();
                while (await reader.ReadAsync())
                {
                    var booking = new BookingReadDTO(
                        Id: reader.GetInt32(0),
                        PassengerId: reader.GetInt32(1),
                        FlightId: reader.GetInt32(2),
                        SeatNumber: reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                        BookingDate: reader.GetDateTime(4)
                    );
                    bookings.Add(booking);
                }
                return bookings;
            }
        }

        /// <summary>
        /// Get a booking by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookingReadDTO> GetBooking(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Booking WHERE Id = $id";
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("$id", id);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new BookingReadDTO(
                        Id: reader.GetInt32(0),
                        PassengerId: reader.GetInt32(1),
                        FlightId: reader.GetInt32(2),
                        SeatNumber: reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                        BookingDate: reader.GetDateTime(4)
                    );
                }
                return null;
            }
        }
        /// <summary>
        /// Add a new booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public async Task AddBooking(BookingCreateDTO booking)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqliteCommand(
                    "INSERT INTO Booking (PassengerId, FlightId, SeatNumber, BookingDate) VALUES ($passengerId, $flightId, $seatNumber, $bookingDate)", connection);
                command.Parameters.AddWithValue("$passengerId", booking.PassengerId);
                command.Parameters.AddWithValue("$flightId", booking.FlightId);
                command.Parameters.AddWithValue("$seatNumber", DBNull.Value);
                command.Parameters.AddWithValue("$bookingDate", booking.BookingDate);
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Update an existing booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public async Task UpdateBooking(BookingUpdateDTO booking)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqliteCommand(
                    "UPDATE Booking SET PassengerId = $passengerId, FlightId = $flightId, SeatNumber = $seatNumber, BookingDate = $bookingDate WHERE Id = $id", connection);
                command.Parameters.AddWithValue("$passengerId", booking.PassengerId);
                command.Parameters.AddWithValue("$flightId", booking.FlightId);
                command.Parameters.AddWithValue("$seatNumber", booking.SeatNumber);
                command.Parameters.AddWithValue("$bookingDate", booking.BookingDate);
                command.Parameters.AddWithValue("$id", booking.Id);
                await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Delete a booking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteBooking(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqliteCommand("DELETE FROM Booking WHERE Id = $id", connection);
                command.Parameters.AddWithValue("$id", id);
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Get bookings by passenger ID
        /// </summary>
        /// <param name="passengerId"></param>
        /// <returns></returns>
        public async Task<List<BookingReadDTO>> GetBookingsByPassengerId(int passengerId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Booking WHERE PassengerId = $passengerId";
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("$passengerId", passengerId);
                var reader = await command.ExecuteReaderAsync();
                var bookings = new List<BookingReadDTO>();
                while (await reader.ReadAsync())
                {
                    var booking = new BookingReadDTO(
                        Id: reader.GetInt32(0),
                        PassengerId: reader.GetInt32(1),
                        FlightId: reader.GetInt32(2),
                        SeatNumber: reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                        BookingDate: reader.GetDateTime(4)
                    );
                    bookings.Add(booking);
                }
                return bookings;
            }
        }

        /// <summary>
        /// Get bookings by passport ID
        /// </summary>
        /// <param name="passportId"></param>
        /// <returns></returns>
        public async Task<List<BookingReadDTO>> GetBookingsByPassportId(int passportId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Booking WHERE PassengerId IN (SELECT Id FROM Passenger WHERE PassportId = $passportId)";
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("$passportId", passportId);
                var reader = await command.ExecuteReaderAsync();
                var bookings = new List<BookingReadDTO>();
                while (await reader.ReadAsync())
                {
                    var booking = new BookingReadDTO(
                        Id: reader.GetInt32(0),
                        PassengerId: reader.GetInt32(1),
                        FlightId: reader.GetInt32(2),
                        SeatNumber: reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                        BookingDate: reader.GetDateTime(4)
                    );
                    bookings.Add(booking);
                }
                return bookings;
            }
        }

        /// <summary>
        /// Get bookings by flight ID
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns></returns>
        public async Task<List<BookingReadDTO>> GetBookingsByFlightId(int flightId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Booking WHERE FlightId = $flightId";
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("$flightId", flightId);
                var reader = await command.ExecuteReaderAsync();
                var bookings = new List<BookingReadDTO>();
                while (await reader.ReadAsync())
                {
                    var booking = new BookingReadDTO(
                        Id: reader.GetInt32(0),
                        PassengerId: reader.GetInt32(1),
                        FlightId: reader.GetInt32(2),
                        SeatNumber: reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                        BookingDate: reader.GetDateTime(4)
                    );
                    bookings.Add(booking);
                }
                return bookings;
            }
        }
    }
}
