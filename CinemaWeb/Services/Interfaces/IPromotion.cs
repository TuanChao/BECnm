using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IPromotion
    {
        Task<ResponseObject<DataResponsesPromotion>> CreatePromotion(Requests_CreatePromotion request);
        Task<ResponseObject<DataResponsesPromotion>> UpdatePromotion(Requests_UpdatePromotion request);
        Task<PageResult<DataResponsesPromotion>> GetAllPromotions(int pageSize, int pageNumber);
    }
}
