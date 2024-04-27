using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IFood
    {
        Task<ResponseObject<DataResponsesFood>> CreateFood(Requests_CreateFood requests);
        Task<ResponseObject<DataResponsesFood>> UpdateFood(Requests_UpdateFood requests);
        string DeleteFood(int foodId);
    }
}
