using CinemaWeb.Entities;
using CinemaWeb.Handle.HandleImage;
using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class BannerServices : BaseServices, IBanner
    {
        private readonly ResponseObject<DataResponsesBanner> _responseObject;
        private readonly BannerConverter _converter;
        public BannerServices(ResponseObject<DataResponsesBanner> _responseObject, BannerConverter _converter)
        {
            this._responseObject = _responseObject;
            this._converter = _converter;
        }
        public async Task<ResponseObject<DataResponsesBanner>> CreateBanner(Requests_CreateBanner request)
        {

            Banner banner = new Banner
            {
                ImageUrl = await HandleUploadFileImages.UploadLoadFile(request.ImageUrl),
                Title = request.Title
            };
            await _appDbContext.Banners.AddAsync(banner);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Thêm banner thành công", _converter.ConvertDt(banner));
        }

        public async Task<string> DeleteBanner(int bannerId)
        {
            var banner = await _appDbContext.Banners.SingleOrDefaultAsync(x => x.Id == bannerId);
            if (banner == null)
            {
                return "Không tồn tại";
            }
            _appDbContext.Banners.Remove(banner);
            await _appDbContext.SaveChangesAsync();
            return "Xóa banner thành công";
        }

        public async Task<PageResult<DataResponsesBanner>> GetAllBanners(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Banners.Select(x => _converter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponsesBanner>> GetBannerById(int bannerId)
        {
            var result = await _appDbContext.Banners.SingleOrDefaultAsync(x => x.Id == bannerId);
            if (result == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy banner Id", null);
            }
            return _responseObject.ResponseSucess("Lấy dữ liệu thành công", _converter.ConvertDt(result));
        }

        public async Task<ResponseObject<DataResponsesBanner>> UpdateBanner(Requests_UpdateBanner request)
        {
            var banner = await _appDbContext.Banners.SingleOrDefaultAsync(x => x.Id == request.BannerId);
            if (banner == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy", null);
            }
            banner.Title = request.Title;
            banner.ImageUrl = await HandleUploadFileImages.UpdateFile(banner.ImageUrl, request.ImageUrl);
            _appDbContext.Banners.Update(banner);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập nhật banner thành công", _converter.ConvertDt(banner));
        }
    }
}
