using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class BillConverter
    {
        private readonly AppDbContext _context;
        private readonly BillFoodConverter _billFoodConverter;
        private readonly BillTicketConverter _billTicketConverter;
        public BillConverter()
        {
            _context = new AppDbContext();
            _billFoodConverter = new BillFoodConverter();
            _billTicketConverter = new BillTicketConverter();
        }
        public DataResponsesBill ConvertDt(Bill bill)
        {
            return new DataResponsesBill
            {
                CreateTime = bill.CreateTime,
                CustomerName = _context.Users.SingleOrDefault(x => x.Id == bill.CustomerId).Name,
                Id = bill.Id,
                Name = bill.Name,
                TotalMoney = bill.TotalMoney,
                BillStatusName = _context.BillStatuses.SingleOrDefault(x => x.Id == bill.BillStatusId).Name,
                PromotionPercent = _context.Promotions.SingleOrDefault(x => x.Id == bill.PromotionId)?.Percent,
                TradingCode = bill.TradingCode,
                BillFoods = _context.BillFoods.Where(x => x.BillId == bill.Id)?.Select(x => _billFoodConverter.ConvertDt(x)),
                BillTickets = _context.BillTickets.Where(x => x.BillId == bill.Id).Select(x => _billTicketConverter.ConvertDt(x))
            };
        }
    }
}
