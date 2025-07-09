using RealStateApp.Core.Application.Dtos.Error;

namespace RealStateApp.Core.Application.Dtos.Agent
{
    public class UpdateAgentResponse : BaseErrorReportEntity
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
