namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesBillTicket : DataResponsesId
    {
        public int Quantity { get; set; }
        public int SeatNumber { get; set; }
        public string SeatLine { get; set; }
    }
}
