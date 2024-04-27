using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class RankCustomerConverter
    {
        public DataResponsesRankCustomer ConvertDt(RankCustomer rankCustomer)
        {
            return new DataResponsesRankCustomer
            {
                Description = rankCustomer.Description,
                Id = rankCustomer.Id,
                Name = rankCustomer.Name,
                Point = rankCustomer.Point,
            };
        }
    }
}
