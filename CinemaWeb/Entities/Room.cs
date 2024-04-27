namespace CinemaWeb.Entities
{
    public class Room : BaseId
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int CinemaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public IEnumerable<Schedule> Schedules { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
        public Cinema? Cinema { get; set; }
    }
}
