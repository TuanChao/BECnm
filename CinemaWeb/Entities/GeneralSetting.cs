namespace CinemaWeb.Entities
{
    public class GeneralSetting : BaseId
    {
        public DateTime BreakTime { get; set; }
        public int BusinesHours { get; set; }
        public DateTime CloseTime { get; set; }
        public double? FixedTicketPrice { get; set; }
        public int PercentDay { get; set; }
        public int PercentWeekend { get; set; }
        public DateTime TimeBeginToChange { get; set; }
    }
}
