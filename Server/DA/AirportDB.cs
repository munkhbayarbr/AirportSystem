using System;
using Microsoft.Data.Sqlite;

namespace Server.DA
{
    public class AirportDB
    {
        /// <summary>
        /// Data Access Passenger class for the different entities in the database.
        /// </summary>
        public PassengerDA Passenger;

        /// <summary>
        /// Data Access Flight class for the different entities in the database.
        /// </summary>
        public FlightDA Flight;

        /// <summary>
        /// Data Access Booking class for the different entities in the database.
        /// </summary>
        public BookingDA Booking;

        /// <summary>
        /// Data Access Seat class for the different entities in the database.
        /// </summary>
        public SeatDB Seat;

        /// <summary>
        /// Connection string to the database.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Constructor for the AirportDB class.
        /// </summary>
        /// <param name="connectionString"></param>
        public AirportDB(string connectionString)
        {
            this._connectionString = connectionString;
            Passenger = new PassengerDA(connectionString);
            Flight = new FlightDA(connectionString);
            Booking = new BookingDA(connectionString);
            Seat = new SeatDB(connectionString);
        }
    }
}
