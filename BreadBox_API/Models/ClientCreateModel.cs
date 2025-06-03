namespace BreadBox_API.Models
{
    public class ClientCreateModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }
    }
}
