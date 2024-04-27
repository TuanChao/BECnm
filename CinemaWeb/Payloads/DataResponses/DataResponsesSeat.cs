namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesSeat : DataResponsesId
    {
        public int Number { get; set; }
        public string SeatStatusName { get; set; }
        public string Line { get; set; }
        public string RoomName { get; set; }
        public string SeatTypeName { get; set; }
    }
}
