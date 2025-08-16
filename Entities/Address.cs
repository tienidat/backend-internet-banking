namespace BPIBankSystem.API.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressDetail { get; set; } = null!;
        public string? District { get; set; }
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public bool IsCurrent { get; set; } = true;
        public DateTime UpdatedAt { get; set; }
        public User? User { get; set; }
    }

}
