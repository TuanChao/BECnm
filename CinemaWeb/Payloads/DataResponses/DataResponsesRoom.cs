namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesRoom : DataResponsesId
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string CinemaName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IQueryable<DataResponsesSeat> DataResponseSeats { get; set; }
        public IQueryable<DataResponsesSchedule> DataResponseSchedules { get; set; }
    }
}
