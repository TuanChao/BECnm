namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesMovie : DataResponsesId
    {
        public int MovieDuration { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Caster { get; set; }
        public bool? IsHot { get; set; }
        public string Image { get; set; }
        public string HeroImage { get; set; }
        public string Language { get; set; }
        public string MovieTypeName { get; set; }
        public string Name { get; set; }
        public string RateName { get; set; }
        public string Trailer { get; set; }
        public IQueryable<DataResponsesSchedule> Schedules { get; set; }
    }
}
