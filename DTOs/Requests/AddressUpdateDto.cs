using BPIBankSystem.API.Entities;

namespace BPIBankSystem.API.DTOs.Requests
{
    public class AddressUpdateDto
    {
        public string AddressDetail { get; set; }
        public string? District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
