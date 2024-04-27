namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_UpdateRankCustomer
    {
        public int RankCustomerId { get; set; }
        public int Point { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
