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
    public class PromotionServices : BaseServices, IPromotion
    {
        private readonly ResponseObject<DataResponsesPromotion> _responseObject;
        private readonly PromotionConverter _converter;
        public PromotionServices(ResponseObject<DataResponsesPromotion> responseObject, PromotionConverter converter)
        {
            _responseObject = responseObject;
            _converter = converter;
        }

        public async Task<ResponseObject<DataResponsesPromotion>> CreatePromotion(Requests_CreatePromotion request)
        {
            var rankCustomer = await _appDbContext.RankCustomers.SingleOrDefaultAsync(x => x.Id == request.RankCustomerId);
            if (rankCustomer == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy id hạng", null);
            }
            Promotion promotion = new Promotion
            {
                Description = request.Description,
                EndTime = request.EndTime,
                Name = request.Name,
                Percent = request.Percent,
                RankCustomerId = request.RankCustomerId,
                Quantity = request.Quantity,
                StartTime = request.StartTime,
                Type = request.Type
            };
            await _appDbContext.Promotions.AddAsync(promotion);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Thêm khuyến mãi thành công", _converter.ConvertDt(promotion));
        }

        public async Task<ResponseObject<DataResponsesPromotion>> UpdatePromotion(Requests_UpdatePromotion request)
        {
            var promotion = await _appDbContext.Promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);
            if (promotion == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy khuyến mãi", null);
            }
            promotion.Quantity = request.Quantity;
            promotion.StartTime = request.EndTime;
            promotion.Type = request.Type;
            promotion.EndTime = request.EndTime;
            promotion.Name = request.Name;
            promotion.Description = request.Description;
            promotion.Percent = request.Percent;
            promotion.RankCustomerId = request.RankCustomerId;
            _appDbContext.Promotions.Update(promotion);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập nhật thông tin khuyến mãi thành công", _converter.ConvertDt(promotion));
        }

        public async Task<PageResult<DataResponsesPromotion>> GetAllPromotions(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Promotions.Select(x => _converter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
    }
}
