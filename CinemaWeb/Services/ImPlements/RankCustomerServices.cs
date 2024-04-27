using CinemaWeb.Entities;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class RankCustomerServices : BaseServices, IRankCustomer
    {
        private readonly ResponseObject<DataResponsesRankCustomer> _responseObject;
        private readonly RankCustomerConverter _converter;
        public RankCustomerServices(ResponseObject<DataResponsesRankCustomer> responseObject, RankCustomerConverter converter)
        {
            _responseObject = responseObject;
            _converter = converter;
        }

        public async Task<ResponseObject<DataResponsesRankCustomer>> CreateRankCustomer(Requests_CreateRankCustomer request)
        {
            var rankCustomer = new RankCustomer();
            rankCustomer.Name = request.Name;
            rankCustomer.Description = request.Description;
            rankCustomer.Point = request.Point;
            await _appDbContext.RankCustomers.AddAsync(rankCustomer);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Thêm hạng thành công", _converter.ConvertDt(rankCustomer));
        }

        public async Task<ResponseObject<DataResponsesRankCustomer>> UpdateRankCustomer(Requests_UpdateRankCustomer request)
        {
            var rank = await _appDbContext.RankCustomers.SingleOrDefaultAsync(x => x.Id == request.RankCustomerId);
            if (rank == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy id rank", null);
            }
            rank.Name = request.Name;
            rank.Description = request.Description;
            rank.Point = request.Point;
            _appDbContext.RankCustomers.Update(rank);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập nhật thông tin rank thành công", _converter.ConvertDt(rank));
        }
    }
}
