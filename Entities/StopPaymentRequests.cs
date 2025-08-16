using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.Entities
{
    public class StopPaymentRequests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(20)]
        public string CheckNumber { get; set; } = string.Empty;

        public DateTime? ChequeDate { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string PayeeName { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "pending";

        [Required]
        [StringLength(255)]
        public string Reason { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
