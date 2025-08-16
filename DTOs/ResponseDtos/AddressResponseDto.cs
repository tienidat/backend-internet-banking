namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class AddressResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AddressDetail { get; set; } = string.Empty;
        public string? District { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
    }
}
