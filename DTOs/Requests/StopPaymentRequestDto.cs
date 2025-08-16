using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.DTOs.Requests
{
    public class StopPaymentRequestDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string CheckNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public string PayeeName { get; set; }

    }

    public class CreateStopPaymentRequestDto
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string CheckNumber { get; set; } = string.Empty;
        public DateTime? ChequeDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        [StringLength(100)]
        public string PayeeName { get; set; }
    }
}
