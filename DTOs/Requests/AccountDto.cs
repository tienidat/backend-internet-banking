using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.DTOs.Requests
{
    public class AccountDto
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account number is required.")]
        [RegularExpression(@"^\d{10,16}$", ErrorMessage = "Account number must be 10 to 16 digits.")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account name is required.")]
        [MaxLength(100, ErrorMessage = "Account name cannot exceed 100 characters.")]
        public string AccountName { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative number.")]
        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Transaction password is required.")]
        [MinLength(6, ErrorMessage = "Transaction password must be at least 6 characters.")]
        [MaxLength(50, ErrorMessage = "Transaction password cannot exceed 50 characters.")]
        public string TransactionPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive integer.")]
        public int UserId { get; set; }
    }

}
