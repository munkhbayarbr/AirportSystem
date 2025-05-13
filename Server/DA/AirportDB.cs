using System;
using Microsoft.Data.Sqlite;

namespace Server.DA
{
    public class AirportDB
    {

        public PassengerDA Passenger;
        public FlightDA Flight;
        public BookingDA Booking;


        private readonly string _connectionString;
        public AirportDB(string connectionString)
        {
            this._connectionString = connectionString;
            Passenger = new PassengerDA(connectionString);
            Flight = new FlightDA(connectionString);
            Booking = new BookingDA(connectionString);
        }
    }
}
