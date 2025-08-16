namespace BPIBankSystem.API.DTOs.Requests
{
    public class SupportRequestDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
    }
}
