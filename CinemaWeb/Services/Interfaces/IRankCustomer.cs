using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IRankCustomer
    {
        Task<ResponseObject<DataResponsesRankCustomer>> CreateRankCustomer(Requests_CreateRankCustomer request);
        Task<ResponseObject<DataResponsesRankCustomer>> UpdateRankCustomer(Requests_UpdateRankCustomer request);
    }
}
