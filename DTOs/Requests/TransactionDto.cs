namespace BPIBankSystem.API.DTOs.Requests
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int? TransferRequestId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reference { get; set; }
    }
}
