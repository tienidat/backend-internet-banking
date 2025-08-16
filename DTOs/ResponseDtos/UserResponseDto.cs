namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public bool IsLocked { get; set; }
    }
}
