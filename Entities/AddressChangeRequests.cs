using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.Entities
{
    public class AddressChangeRequests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int NewAddressId { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "pending";

        public string Notes { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("NewAddressId")]
        public Address NewAddress { get; set; }
    }
}
