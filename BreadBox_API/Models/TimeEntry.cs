namespace BreadBox_API.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double HoursWorked { get; set; }
        public string? Description { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
    }
}
