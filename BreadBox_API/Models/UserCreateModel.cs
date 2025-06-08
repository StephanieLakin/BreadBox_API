namespace BreadBox_API.Models
{
    public class UserCreateModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; } // Plain-text password to be hashed
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubscriptionPlan { get; set; }
        public string StripeCustomerId { get; set; }
    }
}
