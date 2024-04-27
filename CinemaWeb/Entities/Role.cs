namespace CinemaWeb.Entities
{
    public class Role : BaseId
    {
        public string Code { get; set; }
        public string RoleName { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
