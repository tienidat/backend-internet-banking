namespace BPIBankSystem.API.DTOs.Requests
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressDetail { get; set; } = string.Empty;
        public string? District { get; set; }
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public bool IsCurrent { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
