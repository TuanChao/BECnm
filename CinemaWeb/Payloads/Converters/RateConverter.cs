using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class RateConverter
    {
        private readonly AppDbContext _appDbContext;
        private readonly MovieConverter _converter;
        public RateConverter()
        {
            _appDbContext = new AppDbContext();
            _converter = new MovieConverter();
        }
        public DataResponsesRate ConvertDt(Rate rate)
        {
            return new DataResponsesRate
            {
                Id = rate.Id,
                Description = rate.Description,
                Movies = _appDbContext.Movies.Where(x => x.RateId == rate.Id).Select(x => _converter.ConvertDt(x)),
            };
        }
    }
}
