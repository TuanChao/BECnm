using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IBanner
    {
        Task<ResponseObject<DataResponsesBanner>> CreateBanner(Requests_CreateBanner request);
        Task<ResponseObject<DataResponsesBanner>> UpdateBanner(Requests_UpdateBanner request);
        Task<string> DeleteBanner(int bannerId);
        Task<PageResult<DataResponsesBanner>> GetAllBanners(int pageSize, int pageNumber);
        Task<ResponseObject<DataResponsesBanner>> GetBannerById(int bannerId);
    }
}
