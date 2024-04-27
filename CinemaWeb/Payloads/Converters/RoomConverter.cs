using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class RoomConverter
    {
        private readonly SeatConverter _seatConverter;
        private readonly AppDbContext _appDbContext;
        private readonly SchedulesConverter _scheduleConverter;
        public RoomConverter()
        {
            _appDbContext = new AppDbContext();
            _seatConverter = new SeatConverter();
            _scheduleConverter = new SchedulesConverter();
        }
        public DataResponsesRoom ConvertDt(Room room)
        {
            return new DataResponsesRoom
            {
                Id = room.Id,
                Capacity = room.Capacity,
                Description = room.Description,
                Name = room.Name,
                Type = room.Type,
                DataResponseSeats = _appDbContext.Seats.Where(x => x.RoomId == room.Id).Select(x => _seatConverter.ConvertDt(x)).AsQueryable(),
                DataResponseSchedules = _appDbContext.Schedules.Where(x => x.RoomId == room.Id).Select(x => _scheduleConverter.ConvertDt(x)).AsQueryable()
            };
        }
    }
}
