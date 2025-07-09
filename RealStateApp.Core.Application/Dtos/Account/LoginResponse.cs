using RealStateApp.Core.Application.Dtos.Error;
using System.Text.Json.Serialization;

namespace RealStateApp.Core.Application.Dtos.Account
{
    public class LoginResponse : BaseErrorReportEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }
        public string? JWToken { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }
}
