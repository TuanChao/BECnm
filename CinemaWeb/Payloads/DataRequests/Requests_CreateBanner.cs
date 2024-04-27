using System.ComponentModel.DataAnnotations;

namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_CreateBanner
    {
        [DataType(DataType.Upload)]
        public IFormFile ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
