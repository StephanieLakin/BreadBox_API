namespace BreadBox_API.Models
{
    public class TimeEntryCreateModel
    {
        public int ClientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double HoursWorked { get; set; }
        public string? Description { get; set; }
        public decimal Rate { get; set; }
        public int UserId { get; set; }
    }
}
