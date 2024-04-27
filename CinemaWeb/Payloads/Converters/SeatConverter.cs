using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class SeatConverter
    {
        private readonly AppDbContext _appDbContext;
        public SeatConverter()
        {
            _appDbContext = new AppDbContext();
        }
        public DataResponsesSeat ConvertDt(Seat seat)
        {

            return new DataResponsesSeat
            {
                Id = seat.Id,
                Line = seat.Line,
                Number = seat.Number,
                RoomName = _appDbContext.Rooms.SingleOrDefault(x => x.Seats.Any(y => y.Id == seat.Id)).Name,
                SeatStatusName = _appDbContext.SeatsStatus.SingleOrDefault(x => x.Seats.Any(y => y.Id == seat.Id)).NameStatus,
                SeatTypeName = _appDbContext.SeatTypes.SingleOrDefault(x => x.Seats.Any(y => y.Id == seat.Id)).NameType
            };
        }
    }
}
