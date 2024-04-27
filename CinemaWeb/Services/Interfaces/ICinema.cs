using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface ICinema
    {
        ResponseObject<DataResponsesCinema> CreateCinema(Requests_CreateCinema requests);
        ResponseObject<DataResponsesCinema> UpdateCinema(Requests_UpdateCinema requests);
        string DeleteCinema(int cinemaId);
        Task<PageResult<DataResponsesRoom>> GetListRoomInCinema(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesCinema>> GetListCinema(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesCinema>> GetCinemaByMovie(int movieId, int pageSize, int pageNumber);
    }
}
