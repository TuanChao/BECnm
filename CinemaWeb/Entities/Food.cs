namespace CinemaWeb.Entities
{
    public class Food : BaseId
    {
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string NameOfFood { get; set; }
        public bool? IsActive { get; set; } = true;
        public IEnumerable<BillFood>? BillFoods { get; set; }
    }
}
