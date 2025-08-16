using BPIBankSystem.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.DTOs
{
    public class Checks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CheckNumber { get; set; } = string.Empty;

        [Required]
        public int AccountId { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "issued";

        public string CancellationReason { get; set; } = string.Empty;

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}
