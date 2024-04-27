namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesMovieType : DataResponsesId
    {
        public string MovieTypeName { get; set; }
        public IQueryable<DataResponsesMovie> Movies { get; set; }
    }
}
