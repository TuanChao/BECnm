using CinemaWeb.Entities;
using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IBill
    {
        Task<ResponseObject<DataResponsesBillTicket>> CreateBillTicket(int billId, Requests_CreateBillTicket request);
        Task<List<BillTicket>> CreateListBillTicket(int billId, List<Requests_CreateBillTicket> requests);
        Task<ResponseObject<DataResponsesBillFood>> CreateBillFood(int billId, Requests_CreateBillFood request);
        Task<List<BillFood>> CreateListBillFood(int billId, List<Requests_CreateBillFood> requests);
        Task<ResponseObject<DataResponsesBill>> CreateBill(Requests_CreateBill request);
        Task<ResponseObject<DataResponsesBill>> GetPaymentHistoryByBillId(int billId);
        Task<PageResult<DataResponsesBill>> GetAllBills(int pageSize, int pageNumber);
        Task<IQueryable<DataStaticSales>> SalesStatistics(InputStatistic input);
        Task<IQueryable<DataStaticsFood>> SalesStatisticsFood(InputFoodStatistics input);
    }
}
