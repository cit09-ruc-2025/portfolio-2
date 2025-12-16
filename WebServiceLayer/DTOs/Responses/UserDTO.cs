namespace WebServiceLayer.DTOs.Responses
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}
