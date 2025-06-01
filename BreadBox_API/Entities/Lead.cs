using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreadBox_API.Entities
{
    public class Lead
    {
        [Key]
        public int Id {  get; set; }
        
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        public string Phone { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
