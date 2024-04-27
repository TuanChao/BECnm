using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class TicketConverter
    {
        private readonly AppDbContext _context;
        public TicketConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponsesTicket ConvertDt(Ticket ticket)
        {
            return new DataResponsesTicket
            {
                Code = ticket.Code,
                Id = ticket.Id,
                ScheduleName = _context.Schedules.SingleOrDefault(x => x.Id == ticket.ScheduleId).Name,
                SeatLine = _context.Seats.SingleOrDefault(x => x.Id == ticket.SeatId).Line,
                SeatNumber = _context.Seats.SingleOrDefault(x => x.Id == ticket.SeatId).Number,
                Price = ticket.PriceTicket
            };
        }
    }
}
