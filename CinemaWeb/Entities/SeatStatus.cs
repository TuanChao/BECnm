namespace CinemaWeb.Entities
{
    public class SeatStatus : BaseId
    {
        public string Code { get; set; }
        public string NameStatus { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
    }
}
