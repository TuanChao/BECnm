using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class PromotionConverter
    {
        public DataResponsesPromotion ConvertDt(Promotion promotion)
        {
            return new DataResponsesPromotion
            {
                Description = promotion.Description,
                EndTime = promotion.EndTime,
                Id = promotion.Id,
                Name = promotion.Name,
                Percent = promotion.Percent,
                Quantity = promotion.Quantity,
                StartTime = promotion.StartTime,
                Type = promotion.Type
            };
        }
    }
}
