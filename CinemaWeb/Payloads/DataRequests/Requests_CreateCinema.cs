namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_CreateCinema
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string NameOfCinema { get; set; }
        public List<Requests_CreateRoom>? Request_CreateRooms { get; set; }
    }
}
