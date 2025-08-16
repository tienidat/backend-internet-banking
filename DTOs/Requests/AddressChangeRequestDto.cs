namespace BPIBankSystem.API.DTOs.Requests
{
    public class AddressChangeRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NewAddressId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

    public class CreateAddressChangeRequestDto
    {
        public int UserId { get; set; }
        public int NewAddressId { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
