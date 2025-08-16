namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class TransactionResponseDto
    {
        public string Reference { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
