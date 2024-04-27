namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_UpdateSeats
    {
        public int SeatId { get; set; }
        public int Number { get; set; }
        public string Line { get; set; }
        public int SeatTypeId { get; set; }
        public int SeatStatusId { get; set; }
    }
}
