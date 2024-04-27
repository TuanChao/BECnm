namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesRate : DataResponsesId
    {
        public string Description { get; set; }
        public string Code { get; set; }

        public IQueryable<DataResponsesMovie> Movies { get; set; }
    }
}
