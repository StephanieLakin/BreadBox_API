using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreadBox_API.Entities
{
    public class TimeEntry
    {
        [Key]
        public int Id { get; set; }


        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double HoursWorked { get; set; }
        public string Description { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
