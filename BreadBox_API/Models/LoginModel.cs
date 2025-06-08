namespace BreadBox_API.Models
{
    public class LoginModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }  // Plain-text password from user
    }
}
