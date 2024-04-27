using CinemaWeb.Entities;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class TicketServices : BaseServices,ITicket
    {
        private readonly ResponseObject<DataResponsesTicket> _responseObject;
        private readonly TicketConverter _ticketConverter;
        public TicketServices(ResponseObject<DataResponsesTicket> responseObject, TicketConverter ticketConverter)
        {
            _responseObject = responseObject;
            _ticketConverter = ticketConverter;
        }

        public async Task<ResponseObject<DataResponsesTicket>> CreateTicket(int scheduleId, Requests_CreateTicket request)
        {
            var seat = await _appDbContext.Seats.SingleOrDefaultAsync(x => x.Id == request.SeatId);
            if (seat == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy ghế", null);
            }
            var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
            if (schedule == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy thông tin", null);
            }
            Ticket ticket = new Ticket();
            ticket.ScheduleId = scheduleId;
            ticket.SeatId = request.SeatId;
            ticket.Code = "Movie" + DateTime.UtcNow.Ticks.ToString() + new Random().Next(1000, 9999).ToString();
            await _appDbContext.Tickets.AddAsync(ticket);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Tạo vé thành công", _ticketConverter.ConvertDt(ticket));
        }

        public async Task<ResponseObject<DataResponsesTicket>> UpdateTicket(Requests_UpdateTicket request)
        {
            var ticket = await _appDbContext.Tickets.SingleOrDefaultAsync(x => x.Id == request.TicketId);
            if (ticket == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy thông tin", null);
            }
            ticket.Id = request.TicketId;
            ticket.ScheduleId = request.ScheduleId;
            ticket.SeatId = request.SeatId;
            _appDbContext.Tickets.Update(ticket);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập nhật thông tin thành công", _ticketConverter.ConvertDt(ticket));
        }

        public List<Ticket> CreateListTicket(int scheduleId, List<Requests_CreateTicket> requests)
        {
            var schedule = _appDbContext.Seats.SingleOrDefault(x => x.Id == scheduleId);
            if (scheduleId == null)
            {
                throw new ArgumentNullException("Lịch chiếu không tồn tại");
            }
            List<Ticket> list = new List<Ticket>();
            foreach (var request in requests)
            {
                Ticket item = new Ticket
                {
                    IsActive = true,
                    Code = "lstticket",
                    ScheduleId = scheduleId,
                    SeatId = request.SeatId,
                };
                list.Add(item);
            }
            _appDbContext.Tickets.AddRange(list);
            _appDbContext.SaveChanges();
            return list;
        }

    }
}
