namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_UpdateTicket
    {
        public int TicketId { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
    }
}
