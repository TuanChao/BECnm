namespace CinemaWeb.Entities
{
    public class RefreshToken : BaseId
    {
        public string Token { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
