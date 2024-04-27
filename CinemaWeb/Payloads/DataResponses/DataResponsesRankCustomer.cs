namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesRankCustomer : DataResponsesId
    {
        public int Point { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IQueryable<DataResponsesUser> Users { get; set; }
    }
}
