namespace BPIBankSystem.API.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int? TransferRequestId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reference { get; set; }
        public TransferRequest TransferRequest { get; set; }
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
    }
}
