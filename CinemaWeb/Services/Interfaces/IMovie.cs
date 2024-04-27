using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IMovie
    {
        Task<ResponseObject<DataResponsesMovie>> CreateMovie(Requests_CreateMovie requests);
        Task<ResponseObject<DataResponsesMovie>> UpdateMovie(Requests_UpdateMovie requests);
        string DeleteMovie(int movieId);
        ResponseObject<DataResponsesRate> CreateRate(Requests_CreateRate requests);
        ResponseObject<DataResponsesRate> UpdateRate(Requests_UpdateRate requests);
        string DeleteRate(int rateId);
        ResponseObject<DataResponsesMovieType> CreateMovieType(Requests_CreateMovieType requests);
        ResponseObject<DataResponsesMovieType> UpdateMovieType(Requests_UpdateMovieType requests);
        string DeleteMovieType(int movieTypeId);
        Task<PageResult<DataResponsesMovie>> GetFeaturedMovies(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesMovieType>> GetAllMovieTypes(int pageSize, int pageNumber);
        Task<ResponseObject<DataResponsesMovieType>> GetMovieTypeById(int movieTypeId);
        Task<PageResult<DataResponsesMovie>> GetAllMovie(InputFilter input, int pageSize, int pageNumber);
        Task<ResponseObject<DataResponsesMovie>> GetMovieById(int movieId);
    }
}
