namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesBill : DataResponsesId
    {
        public double? TotalMoney { get; set; }
        public string TradingCode { get; set; }
        public DateTime CreateTime { get; set; }
        public string CustomerName { get; set; }

        public string BillStatusName { get; set; }
        public string Name { get; set; }
        public int? PromotionPercent { get; set; }
        public IQueryable<DataResponsesBillFood>? BillFoods { get; set; }
        public IQueryable<DataResponsesBillTicket>? BillTickets { get; set; }
    }
}
