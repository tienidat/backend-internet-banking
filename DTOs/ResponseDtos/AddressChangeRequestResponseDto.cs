namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class AddressChangeRequestResponseDto
    {
        public int Id { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string NewAddress { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
