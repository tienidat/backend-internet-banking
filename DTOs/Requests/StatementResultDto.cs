namespace BPIBankSystem.API.DTOs.Requests
{
    public class StatementResultDto
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}
