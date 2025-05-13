namespace Server
{
    public class FlightInfo
    {
        public string Number { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }

        public byte[] Airline { get; set; }

        public string Direction { get; set; }

        public string Gate { get; set; }

        public FlightInfo() { }

        public FlightInfo(string direction,byte[] airline, string number,string gate, DateTime datetime, string status)
        {
            Direction = direction;
            Airline = airline;
            Number =number;
            Gate = gate;
            Status = status;
            DateTime = datetime;
        }
    }

}
