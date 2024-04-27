namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_CreateSeat
    {
        public int Number { get; set; }
        public int? SeatStatusId { get; set; }
        public string Line { get; set; }
        public int SeatTypeId { get; set; }
    }
}
