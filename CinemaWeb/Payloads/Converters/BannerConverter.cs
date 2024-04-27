using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class BannerConverter
    {
        public DataResponsesBanner ConvertDt(Banner banner)
        {
            return new DataResponsesBanner()
            {
                Id = banner.Id,
                ImageUrl = banner.ImageUrl,
                Title = banner.Title,
            };
        }
    }
}
