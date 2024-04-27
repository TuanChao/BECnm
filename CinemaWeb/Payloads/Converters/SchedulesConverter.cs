using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class SchedulesConverter
    {
        private readonly AppDbContext _appDbContext;
        public SchedulesConverter()
        {
            _appDbContext = new AppDbContext();
        }
        public DataResponsesSchedule ConvertDt(Schedule schedule)
        {
            return new DataResponsesSchedule
            {
                Id = schedule.Id,
                MovieName = _appDbContext.Movies.SingleOrDefault(x => x.Id == schedule.MovieId).Name,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                Name = schedule.Name,
                Price = schedule.Price,
                RoomName = _appDbContext.Rooms.SingleOrDefault(x => x.Id == schedule.RoomId).Name
            };
        }
    }
}
