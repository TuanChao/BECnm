using CinemaWeb.Entities;
using CinemaWeb.Handle.Generate;
using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class BillServices : BaseServices ,IBill
    {
        private readonly ResponseObject<DataResponsesBillFood> _responseBillFoodObject;
        private readonly ResponseObject<DataResponsesBillTicket> _responseBillTicketObject;
        private readonly ResponseObject<DataResponsesBill> _responseObject;
        private readonly BillConverter _billConverter;
        private readonly BillTicketConverter _billTicketConverter;
        private readonly BillFoodConverter _billFoodConverter;
        public BillServices(ResponseObject<DataResponsesBillFood> responseBillFoodObject, ResponseObject<DataResponsesBillTicket> responseBillTicketObject, ResponseObject<DataResponsesBill> responseObject, BillConverter billConverter, BillTicketConverter billTicketConverter, BillFoodConverter billFoodConverter)
        {
            _responseBillFoodObject = responseBillFoodObject;
            _responseBillTicketObject = responseBillTicketObject;
            _responseObject = responseObject;
            _billConverter = billConverter;
            _billTicketConverter = billTicketConverter;
            _billFoodConverter = billFoodConverter;
        }

        public async Task<ResponseObject<DataResponsesBill>> CreateBill(Requests_CreateBill request)
        {
            var customer = await _appDbContext.Users.SingleOrDefaultAsync(x => x.Id == request.CustomerId);
            if (customer == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy thông tin khách hàng", null);
            }

            var promotion = await _appDbContext.Promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);
            var existingTickets = await _appDbContext.BillTickets.Where(x => request.BillTickets.Select(bt => bt.TicketId).Contains(x.TicketId)).ToListAsync();
            if (existingTickets.Any())
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Vé đã được mua bởi người khác", null);
            }
            Bill bill = new Bill
            {
                CustomerId = request.CustomerId,
                TradingCode = GenerateCode.GenerateCodes(),
                CreateTime = DateTime.UtcNow,
                Name = request.BillName,
                BillStatusId = 1,
                PromotionId = request.PromotionId,
                BillTickets = null,
                BillFoods = null,
                IsActive = true,
                TotalMoney = 0
            };

            await _appDbContext.Bills.AddAsync(bill);
            await _appDbContext.SaveChangesAsync();

            bill.BillTickets = await CreateListBillTicket(bill.Id, request.BillTickets);
            bill.BillFoods = request.BillFoods != null ? await CreateListBillFood(bill.Id, request.BillFoods) : null;

            double priceTicket = bill.BillTickets?.Sum(x => _appDbContext.Tickets.SingleOrDefault(y => y.Id == x.TicketId).PriceTicket * x.Quantity) ?? 0;
            double priceFood = bill.BillFoods?.Sum(x => _appDbContext.Foods.SingleOrDefault(y => y.Id == x.FoodId).Price * x.Quantity) ?? 0;
            double total = priceTicket + priceFood;

            if (promotion != null)
            {
                bill.TotalMoney = total - (total * promotion.Percent / 100.0);
            }
            else
            {
                bill.TotalMoney = total;
            }

            _appDbContext.Bills.Update(bill);
            await _appDbContext.SaveChangesAsync();

            return _responseObject.ResponseSucess("Tạo hóa đơn thành công", _billConverter.ConvertDt(bill));
        }

        public async Task<ResponseObject<DataResponsesBillFood>> CreateBillFood(int billId, Requests_CreateBillFood request)
        {
            var bill = await _appDbContext.Bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return _responseBillFoodObject.ResponseFail(StatusCodes.Status404NotFound, "Hóa đơn không tồn tại", null);
            }
            var billFood = new BillFood
            {
                BillId = billId,
                Quantity = request.Quantity,
                FoodId = request.FoodId,
            };
            await _appDbContext.BillFoods.AddAsync(billFood);
            await _appDbContext.SaveChangesAsync();
            return _responseBillFoodObject.ResponseSucess("Thêm bill food thành công", _billFoodConverter.ConvertDt(billFood));
        }

        public async Task<ResponseObject<DataResponsesBillTicket>> CreateBillTicket(int billId, Requests_CreateBillTicket request)
        {
            var bill = await _appDbContext.Bills.Include(x => x.Promotion).AsNoTracking().SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return _responseBillTicketObject.ResponseFail(StatusCodes.Status404NotFound, "Hóa đơn không tồn tại", null);
            }
            var billTicket = new BillTicket
            {
                BillId = billId,
                Quantity = 1,
                TicketId = request.TicketId,
            };
            await _appDbContext.BillTickets.AddAsync(billTicket);
            await _appDbContext.SaveChangesAsync();
            return _responseBillTicketObject.ResponseSucess("Thêm bill ticket thành công", _billTicketConverter.ConvertDt(billTicket));
        }

        public async Task<List<BillFood>> CreateListBillFood(int billId, List<Requests_CreateBillFood> requests)
        {
            var bill = await _appDbContext.Bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return null;
            }
            List<BillFood> list = new List<BillFood>();
            foreach (Requests_CreateBillFood request in requests)
            {
                BillFood billTicket = new BillFood
                {
                    BillId = billId,
                    Quantity = request.Quantity,
                    FoodId = request.FoodId,
                };
                list.Add(billTicket);
            }
            await _appDbContext.BillFoods.AddRangeAsync(list);
            await _appDbContext.SaveChangesAsync();
            return list;
        }

        public async Task<List<BillTicket>> CreateListBillTicket(int billId, List<Requests_CreateBillTicket> requests)
        {
            var bill = await _appDbContext.Bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return null;
            }
            List<BillTicket> list = new List<BillTicket>();
            foreach (Requests_CreateBillTicket request in requests)
            {
                BillTicket billTicket = new BillTicket
                {
                    BillId = billId,
                    Quantity = 1,
                    TicketId = request.TicketId,
                };
                list.Add(billTicket);
            }
            await _appDbContext.BillTickets.AddRangeAsync(list);
            await _appDbContext.SaveChangesAsync();
            return list;
        }

        public async Task<ResponseObject<DataResponsesBill>> GetPaymentHistoryByBillId(int billId)
        {
            var result = await _appDbContext.Bills.Include(x => x.BillFoods).Include(x => x.BillTickets).AsNoTracking().SingleOrDefaultAsync(x => x.Id == billId);
            if (result.BillStatusId == 1)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Hóa đơn vẫn chưa được thanh toán", null);
            }
            return _responseObject.ResponseSucess("Thông tin thanh toán của hóa đơn", _billConverter.ConvertDt(result));
        }

        public async Task<PageResult<DataResponsesBill>> GetAllBills(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Bills.Include(x => x.BillTickets).Include(x => x.BillFoods).Where(x => x.BillStatusId == 2).Select(x => _billConverter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
        public async Task<IQueryable<DataStaticSales>> SalesStatistics(InputStatistic input)
        {
            var query = _appDbContext.Bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
                                      .Include(x => x.BillTickets).ThenInclude(x => x.Ticket).ThenInclude(x => x.Schedule).ThenInclude(x => x.Room).ThenInclude(x => x.Cinema)
                                      .AsNoTracking()
                                      .Where(x => x.BillStatusId == 2);
            if (input.CinemaId.HasValue)
            {
                query = query.Where(x => x.BillTickets.Any(y => y.Ticket.Schedule.Room.CinemaId == input.CinemaId));
            }

            var billStats = await query
                .GroupBy(x => new
                {
                    CinemaId = input.CinemaId.HasValue ? x.BillTickets.FirstOrDefault().Ticket.Schedule.Room.CinemaId : (int?)null
                })
                .Select(group => new DataStaticSales
                {
                    CinemaId = group.Key.CinemaId,
                    Sales = group.Sum(item => item.TotalMoney),
                })
                .ToListAsync();

            return billStats.AsQueryable();
        }

        public async Task<IQueryable<DataStaticsFood>> SalesStatisticsFood(InputFoodStatistics input)
        {
            var query = _appDbContext.Bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
                                      .AsNoTracking()
                                      .Where(x => x.BillStatusId == 2);

            if (input.FoodId.HasValue)
            {
                query = query.Where(x => x.BillFoods.Any(y => y.FoodId == input.FoodId));
            }

            var billFoodStats = await query
                .SelectMany(x => x.BillFoods)
                .Where(bf => !input.FoodId.HasValue || bf.FoodId == input.FoodId)
                .GroupBy(bf => new
                {
                    FoodId = bf.FoodId
                })
                .Select(group => new DataStaticsFood
                {
                    FoodId = group.Key.FoodId,
                    Sales = group.Sum(x => x.Quantity * x.Food.Price),
                    SellNumber = group.Count()
                }).ToListAsync();

            return billFoodStats.AsQueryable();
        }
    }
}
