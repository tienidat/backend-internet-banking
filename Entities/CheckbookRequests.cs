using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.Entities
{
    public class CheckbookRequests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "pending";

        public string CheckbookNumber { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
