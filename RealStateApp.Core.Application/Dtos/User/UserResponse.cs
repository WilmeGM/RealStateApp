namespace RealStateApp.Core.Application.Dtos.User
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int PropertyCount { get; set; }
    }
}
