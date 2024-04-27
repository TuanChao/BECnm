using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class BillTicketConverter
    {
        private readonly AppDbContext _context;
        public BillTicketConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponsesBillTicket ConvertDt(BillTicket ticket)
        {
            var seat = _context.Seats.FirstOrDefault(s => s.Tickets.Any(t => t.Id == ticket.TicketId));
            if (seat == null)
            {
                return null;
            }

            return new DataResponsesBillTicket
            {
                Id = ticket.Id,
                Quantity = ticket.Quantity,
                SeatLine = seat.Line,
                SeatNumber = seat.Number
            };
        }
    }
}
