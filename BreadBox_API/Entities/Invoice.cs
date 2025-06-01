using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BreadBox_API.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Status { get; set; }
        public DateTime IssuedDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public DateTime? PaidDate { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
