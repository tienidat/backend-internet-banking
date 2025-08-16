namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class AccountValidationResponseDto
    {
        public bool IsValid { get; set; }
        public string AccountName { get; set; }
        public bool IsBalanceSufficient { get; set; }
        public string Message { get; set; }
    }
}
