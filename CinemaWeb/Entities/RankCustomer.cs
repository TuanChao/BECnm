namespace CinemaWeb.Entities
{
    public class RankCustomer : BaseId
    {
        public int Point { get; set; } = 0;
        public string Description { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public IEnumerable<Promotion> Promotions { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
