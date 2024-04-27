using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface ISchedule
    {
        Task<ResponseObject<DataResponsesSchedule>> CreateSchedule(Requests_CreateSchedule request);
        Task<ResponseObject<DataResponsesSchedule>> UpdateSchedule(Requests_UpdateSchedule request);
        Task<PageResult<DataResponsesSchedule>> GetSchedulesByMovie(int movieId, int pageSize, int pageNumber);
        Task<PageResult<DataResponsesSchedule>> GetAlls(InputScheduleData input, int pageSize, int pageNumber);
        Task<string> DeleteSchedule(int scheduleId);
        Task<PageResult<DataResponsesSchedule>> GetSchedulesByDay(DateTime startAt, int pageSize, int pageNumber);
    }
}
