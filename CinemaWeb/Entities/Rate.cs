namespace CinemaWeb.Entities
{
    public class Rate : BaseId
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
    }
}
