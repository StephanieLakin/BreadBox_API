namespace BreadBox_API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubscriptionPlan { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public string StripeCustomerId { get; set; }
        public string PasswordHash { get; set; }  // stored hash
    }
}
