namespace Server.DA
{
    public class BookingDA
    {
        private readonly string _connectionString;
        public BookingDA(string connectionString)
        {
            this._connectionString = connectionString;
        }
    }
}
