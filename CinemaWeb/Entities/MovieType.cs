namespace CinemaWeb.Entities
{
    public class MovieType : BaseId
    {
        public string MovieTypeName { get; set; }
        public bool? IsActive { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
    }
}
