namespace BPIBankSystem.API.Entities
{
    public class TransferRequest
    {
        public int Id { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ErrorMessage { get; set; }
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; } 
    }
}
