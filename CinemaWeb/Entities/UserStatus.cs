namespace CinemaWeb.Entities
{
    public class UserStatus : BaseId
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
