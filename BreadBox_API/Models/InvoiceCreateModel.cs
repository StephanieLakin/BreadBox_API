namespace BreadBox_API.Models
{
    public class InvoiceCreateModel
    {
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
}
