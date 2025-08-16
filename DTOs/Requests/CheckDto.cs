namespace BPIBankSystem.API.DTOs.Requests
{
    public class CheckDto
    {
        public int Id { get; set; }
        public string CheckNumber { get; set; }
        public int AccountId { get; set; }
        public DateTime IssueDate { get; set; }
        public string Status { get; set; }
        public string CancellationReason { get; set; }
    }

    public class CreateCheckDto
    {
        public string CheckNumber { get; set; } = string.Empty;
        public int AccountId { get; set; }

        public string CancellationReason { get; set; } = string.Empty;
    }

    public class UpdateCheckDto
    {
        public string Status { get; set; }
        public string CancellationReason { get; set; }
    }
}
