namespace CinemaWeb.Entities
{
    public class BillStatus : BaseId
    {
        public string Name { get; set; }
        public IEnumerable<Bill> Bills { get; set; }
    }
}
