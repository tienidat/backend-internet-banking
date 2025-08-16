namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class CheckResponseDto
    {
        public int Id { get; set; }
        public string CheckNumber { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CancellationReason { get; set; } = string.Empty;
    }
}
