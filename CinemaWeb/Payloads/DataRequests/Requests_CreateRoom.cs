namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_CreateRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<Requests_CreateSeat>? Request_CreateSeats { get; set; }
    }
}
