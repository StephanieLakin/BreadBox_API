namespace BreadBox_API.Models
{
    public class UserCreateModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; } // Will hash this in the service
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubscriptionPlan { get; set; }
        public string StripeCustomerId { get; set; }
    }
}
