namespace CinemaWeb.Entities
{
    public class ConfirmEmail : BaseId
    {
        public int UserId { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string? ConfirmCode { get; set; }
        public bool? IsConfirm { get; set; }
        public User? User { get; set; }
    }
}
