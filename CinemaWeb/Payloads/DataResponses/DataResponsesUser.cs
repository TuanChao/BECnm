namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesUser : DataResponsesId
    {
        public int Point { get; set; } = 0;
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
