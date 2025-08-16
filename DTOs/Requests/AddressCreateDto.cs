namespace BPIBankSystem.API.DTOs.Requests
{
    public class AddressCreateDto
    {
        public int UserId { get; set; }
        public string AddressDetail { get; set; } = string.Empty;
        public string? District { get; set; }
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public bool IsCurrent { get; set; } = false;
    }

}
