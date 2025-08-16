using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.DTOs.Requests
{
    public class TransferRequestDto
    {
        [Required(ErrorMessage = "From Account Number is required.")]
        public string FromAccountNumber { get; set; }

        [Required(ErrorMessage = "To Account Number is required.")]
        public string ToAccountNumber { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(10, double.MaxValue, ErrorMessage = "Amount must be at least 10.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount must be a valid number with up to 2 decimal places.")]
        public decimal Amount { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Transaction Password is required.")]
        [MinLength(6, ErrorMessage = "Transaction Password must be at least 6 characters.")]
        [MaxLength(50, ErrorMessage = "Transaction Password cannot exceed 50 characters.")]
        public string TransactionPassword { get; set; }
    }
}
