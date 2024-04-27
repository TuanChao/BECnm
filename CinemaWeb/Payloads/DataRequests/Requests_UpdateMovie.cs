using System.ComponentModel.DataAnnotations;

namespace CinemaWeb.Payloads.DataRequests
{
    public class Requests_UpdateMovie
    {
        public int Id { get; set; }
        public int MovieDuration { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Caster { get; set; }
        public bool? IsHot { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile HeroImage { get; set; }
        public string Language { get; set; }
        public int MovieTypeId { get; set; }
        public string Name { get; set; }
        public int RateId { get; set; }
        public string Trailer { get; set; }
    }
}
