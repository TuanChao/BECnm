namespace CinemaWeb.Entities
{
    public class SeatType : BaseId
    {
        public string NameType { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
    }
}
