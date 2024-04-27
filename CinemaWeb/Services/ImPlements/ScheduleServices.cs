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
    public class ScheduleServices : BaseServices, ISchedule
    {
        private readonly ResponseObject<DataResponsesSchedule> _responseObject;
        private readonly SchedulesConverter _converter;
        public ScheduleServices(SchedulesConverter converter, ResponseObject<DataResponsesSchedule> _responseObject)
        {
            _converter = converter;
            this._responseObject = _responseObject;
        }

        public async Task<ResponseObject<DataResponsesSchedule>> CreateSchedule(Requests_CreateSchedule request)
        {
            var room = await _appDbContext.Rooms.SingleOrDefaultAsync(x => x.Id == request.RoomId);
            if (room == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }
            var movie = await _appDbContext.Movies.SingleOrDefaultAsync(x => x.Id == request.MovieId);
            if (movie == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy thông tin", null);
            }
            var schedule = new Schedule();
            schedule.RoomId = room.Id;
            schedule.MovieId = movie.Id;
            schedule.Code = DateTime.UtcNow.Ticks.ToString() + "abc" + new Random().Next(1000, 9999);
            schedule.StartAt = request.StartAt;
            schedule.EndAt = request.EndAt;
            schedule.Price = request.Price;
            schedule.Name = request.Name;
            if (_appDbContext.Schedules.Any(x => !((request.StartAt < x.StartAt && request.EndAt < x.StartAt) || (request.StartAt > x.EndAt && request.EndAt > x.EndAt)) && x.RoomId == request.RoomId))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Lịch chiếu bị trùng", null);
            }
            await _appDbContext.Schedules.AddAsync(schedule);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Thêm lịch trình thành công", _converter.ConvertDt(schedule));
        }

        public async Task<ResponseObject<DataResponsesSchedule>> UpdateSchedule(Requests_UpdateSchedule request)
        {
            var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == request.ScheduleId);
            if (schedule == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Lịch trình không tồn tại", null);
            }
            if (!_appDbContext.Rooms.Any(x => x.Id == request.RoomId))
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }
            if (!_appDbContext.Movies.Any(x => x.Id == request.MovieId))
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phim", null);
            }
            schedule.StartAt = request.StartAt;
            schedule.Price = request.Price;
            schedule.Price = request.Price;
            schedule.Name = request.Name;
            schedule.Code = DateTime.UtcNow.Ticks.ToString() + "abc" + new Random().Next(100, 999);
            schedule.MovieId = request.MovieId;
            schedule.RoomId = request.RoomId;
            _appDbContext.Schedules.Update(schedule);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập nhật thông tin lịch trình thành công", _converter.ConvertDt(schedule));
        }

        public async Task<PageResult<DataResponsesSchedule>> GetSchedulesByMovie(int movieId, int pageSize, int pageNumber)
        {
            var query = _appDbContext.Schedules.Where(x => x.MovieId == movieId).Select(x => _converter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<DataResponsesSchedule>> GetAlls(InputScheduleData input, int pageSize, int pageNumber)
        {
            var query = await _appDbContext.Schedules.Include(x => x.Room).ToListAsync();
            if (input.RoomId.HasValue)
            {
                query = query.Where(x => x.RoomId == input.RoomId).ToList();
            }
            var result = Pagination.GetPagedData(query.Select(x => _converter.ConvertDt(x)).AsQueryable(), pageSize, pageNumber);
            return result;
        }

        public async Task<string> DeleteSchedule(int scheduleId)
        {
            var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
            if (schedule.EndAt < DateTime.Now && schedule != null)
            {
                schedule.IsActive = false;
                _appDbContext.Schedules.Update(schedule);
                await _appDbContext.SaveChangesAsync();
                return "Xóa bản ghi thành công";
            }
            return "Lịch trình không tồn tại";

        }

        public async Task<PageResult<DataResponsesSchedule>> GetSchedulesByDay(DateTime startAt, int pageSize, int pageNumber)
        {
            var query = _appDbContext.Schedules
                .Include(x => x.Movie)
                .AsNoTracking()
                .Where(x => x.StartAt.Date == startAt.Date)
                .Select(x => _converter.ConvertDt(x));

            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
    }
}
