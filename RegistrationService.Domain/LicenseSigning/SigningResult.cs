namespace RegistrationService.Domain.LicenseSigning
{
    public sealed class SigningResult
    {
        public SigningResult(bool success, string code)
        {
            Success = success;
            Code = code;
        }

        public bool Success { get; }
        public string Code { get; }
    }
}
