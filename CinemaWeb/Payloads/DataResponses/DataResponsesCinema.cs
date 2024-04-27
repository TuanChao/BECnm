namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesCinema : DataResponsesId
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string NameOfCinema { get; set; }
        public IQueryable<DataResponsesRoom> Room { get; set; }
    }
}
