namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesTicket : DataResponsesId
    {
        public string Code { get; set; }
        public string ScheduleName { get; set; }
        public int SeatNumber { get; set; }
        public string SeatLine { get; set; }
        public double Price { get; set; }
    }
}
