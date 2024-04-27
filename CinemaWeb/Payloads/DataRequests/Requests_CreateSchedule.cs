namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_CreateSchedule
    {
        public double Price { get; set; }
        public int MovieId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
    }
}
