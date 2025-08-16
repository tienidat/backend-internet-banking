using System.ComponentModel.DataAnnotations.Schema;

namespace BPIBankSystem.API.Entities
{
    public class SupportRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? Answer { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }

}
