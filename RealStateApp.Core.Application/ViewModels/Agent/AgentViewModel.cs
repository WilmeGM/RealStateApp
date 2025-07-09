namespace RealStateApp.Core.Application.ViewModels.Agent
{
    public class AgentViewModel
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
