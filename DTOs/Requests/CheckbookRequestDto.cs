using System.ComponentModel.DataAnnotations;

namespace BPIBankSystem.API.DTOs.Requests
{
    public class CheckbookRequestDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string CheckbookNumber { get; set; }
        public string Notes { get; set; }
    }

    public class CreateCheckbookRequestDto
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string CheckbookNumber { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}

