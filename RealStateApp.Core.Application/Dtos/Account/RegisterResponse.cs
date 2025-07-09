using RealStateApp.Core.Application.Dtos.Error;

namespace RealStateApp.Core.Application.Dtos.Account
{
    public class RegisterResponse : BaseErrorReportEntity
    {
        public string? Id { get; set; }
    }
}
