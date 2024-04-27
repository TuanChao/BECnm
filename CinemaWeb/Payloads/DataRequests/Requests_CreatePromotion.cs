namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_CreatePromotion
    {
        public int Percent { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int RankCustomerId { get; set; }
    }
}
