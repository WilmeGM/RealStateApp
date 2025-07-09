namespace RealStateApp.Core.Application.Dtos.Error
{
    public class BaseErrorReportEntity
    {
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
