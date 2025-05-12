

namespace Dashboard
{
    public class FlightInfo
    {
        public string Number { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }

        public FlightInfo() { } // parameterless constructor needed for deserialization

        public FlightInfo(string number, string status)
        {
            Number = number;
            Status = status;
            DateTime = DateTime.Now;
        }
    }

}
