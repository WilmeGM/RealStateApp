namespace RealStateApp.Core.Application.Helpers
{
    public static class IdGeneratorHelper
    {
        public static string GenerateUniqueCode()
            => new Random().Next(100000, 999999).ToString();
    }
}
