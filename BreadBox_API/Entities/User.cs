using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;


namespace BreadBox_API.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string SubscriptionPlan { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public string StripeCustomerId { get; set; }

        public ICollection<Client> Clients { get; set; }
        public ICollection<Lead> Leads { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<TimeEntry> TimeEntries { get; set; }









    }
}
