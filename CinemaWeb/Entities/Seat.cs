namespace CinemaWeb.Entities
{
    public class Seat : BaseId
    {
        public int Number { get; set; }
        public int SeatStatusId { get; set; }
        public string Line { get; set; }
        public int RoomId { get; set; }
        public bool? IsActive { get; set; } = true;
        public int SeatTypeId { get; set; }

        public IEnumerable<Ticket>? Tickets { get; set; }
        public Room? Room { get; set; }
        public SeatStatus? SeatStatus { get; set; }
        public SeatType? SeatType { get; set; }
    }
}
