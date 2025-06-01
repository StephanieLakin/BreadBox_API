namespace BreadBox_API.Models
{
    public class Lead
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
