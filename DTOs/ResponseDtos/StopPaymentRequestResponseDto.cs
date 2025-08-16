namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class StopPaymentRequestResponseDto
    {
        public int Id { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string ChequeNumber { get; set; } = string.Empty;
        public DateTime? ChequeDate { get; set; }
        public string PayeeName { get; set; } = string.Empty;
        public string ReasonForStopPayment { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
    }
}
