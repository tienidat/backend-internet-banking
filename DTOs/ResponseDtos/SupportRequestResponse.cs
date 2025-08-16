namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class SupportRequestResponse
    {
        public int Id { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime SentDate { get; set; }
        public string Status { get; set; }
        public DateTime? ReplyDate { get; set; }
    }
}
