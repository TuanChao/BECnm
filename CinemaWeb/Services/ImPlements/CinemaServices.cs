using CinemaWeb.Entities;
using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class CinemaServices : BaseServices, ICinema
    {
        private readonly ResponseObject<DataResponsesCinema> responsescinema;
        private readonly CinemaConverter cinemaConverter;
        private readonly RoomConverter _roomConverter;
        private readonly IRoom _iRoomServices;
        public CinemaServices(IRoom iRoomServices)
        {
            responsescinema = new ResponseObject<DataResponsesCinema>();
            _iRoomServices = iRoomServices;
            _roomConverter = new RoomConverter();
            cinemaConverter = new CinemaConverter();
        }
        public ResponseObject<DataResponsesCinema> CreateCinema(Requests_CreateCinema requests)
        {
            if (string.IsNullOrWhiteSpace(requests.Address) ||
                string.IsNullOrWhiteSpace(requests.Description) ||
                string.IsNullOrWhiteSpace(requests.Code) ||
                string.IsNullOrWhiteSpace(requests.NameOfCinema))
            {
                return responsescinema.ResponseFail(StatusCodes.Status400BadRequest, "Thông Tin Chưa Điền Đầy Đủ!!", null);
            }
            var cinema = new Cinema
            {
                Address = requests.Address,
                Code = requests.Code,
                Description = requests.Description,
                NameOfCinema = requests.NameOfCinema,
                Room = null,
                IsActive = true,
            };
            _appDbContext.Cinemas.Add(cinema);
            _appDbContext.SaveChanges();
            cinema.Room = requests.Request_CreateRooms == null ? null : _iRoomServices.CreateListRoom(cinema.Id, requests.Request_CreateRooms);
            _appDbContext.Cinemas.Update(cinema);
            _appDbContext.SaveChanges();
            return responsescinema.ResponseSucess("Thêm Rạp Thành Công", cinemaConverter.ConvertDt(cinema));
        }
        public ResponseObject<DataResponsesCinema> UpdateCinema(Requests_UpdateCinema requests)
        {
            var updatecinema = _appDbContext.Cinemas.FirstOrDefault(x => x.Id == requests.cinemaId);
            if (requests.cinemaId == null) { return responsescinema.ResponseFail(StatusCodes.Status400BadRequest, "Không tìm thấy cinemaId !!", null); }
            updatecinema.Address = requests.Address;
            updatecinema.Description = requests.Description;
            updatecinema.Code = requests.Code;
            updatecinema.NameOfCinema = requests.NameOfCinema;
            if (updatecinema.Room != null) { _appDbContext.Rooms.RemoveRange(updatecinema.Room); }
            _appDbContext.Cinemas.Update(updatecinema);
            _appDbContext.SaveChanges();
            if (requests.Request_UpdateRooms != null)
            {
                var rooms = _iRoomServices.CreateListRoom(updatecinema.Id, requests.Request_UpdateRooms);
                updatecinema.Room = rooms;
            }
            else { updatecinema.Room = null; }
            _appDbContext.Cinemas.Update(updatecinema);
            _appDbContext.SaveChanges();
            return responsescinema.ResponseSucess("Cập Nhật Thông Tin Phim Thành Công!", cinemaConverter.ConvertDt(updatecinema));
        }

        public string DeleteCinema(int cinemaId)
        {
            var cinemadelete = _appDbContext.Cinemas.SingleOrDefault(x => x.Id == cinemaId);
            if (cinemadelete is null) { return "Không Tìm Thấy Id Cinema!"; }
            cinemadelete.IsActive = false;
            _appDbContext.Cinemas.Update(cinemadelete);
            _appDbContext.SaveChanges();
            return "Xóa rạp thành công";
        }

        public async Task<PageResult<DataResponsesCinema>> GetCinemaByMovie(int movieId, int pageSize, int pageNumber)
        {
            var movie = await _appDbContext.Movies.Include(x => x.Schedules).SingleOrDefaultAsync(x => x.Id == movieId);
            if (movie == null)
            {
                throw new ArgumentNullException("Phim không tồn tại!!");
            }

            var schedules = await _appDbContext.Schedules.Include(x => x.Room)
                                                    .ThenInclude(r => r.Cinema)
                                                    .Where(x => x.MovieId == movieId)
                                                    .ToListAsync();

            var groupedByCinema = schedules.GroupBy(s => s.Room.CinemaId);

            var cinemas = new List<DataResponsesCinema>();

            foreach (var group in groupedByCinema)
            {
                var firstSchedule = group.First();
                var cinema = firstSchedule.Room.Cinema;
                var cinemaDTO = new DataResponsesCinema
                {
                    NameOfCinema = cinema.NameOfCinema,
                    Address = cinema.Address,
                    Description = cinema.Description,
                    Room = group.Select(s => _roomConverter.ConvertDt(s.Room)).AsQueryable(),
                    Id = firstSchedule.Id,
                };

                cinemas.Add(cinemaDTO);
            }

            var result = Pagination.GetPagedData(cinemas.AsQueryable(), pageSize, pageNumber);
            return result;
        }


        public async Task<PageResult<DataResponsesCinema>> GetListCinema(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Cinemas.Include(x => x.Room).Select(x => cinemaConverter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<DataResponsesRoom>> GetListRoomInCinema(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Rooms.Include(x => x.Cinema).Include(x => x.Seats).Include(x => x.Schedules).AsNoTracking().Select(x => _roomConverter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
    }
}
