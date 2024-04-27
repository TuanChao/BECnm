namespace CinemaWeb.Entities
{
    public class Promotion : BaseId
    {
        public int Percent { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public int RankCustomerId { get; set; }
        public RankCustomer? RankCustomer { get; set; }
        public IEnumerable<Bill>? Bill { get; set; }
    }
}
