namespace BPIBankSystem.API.DTOs.ResponseDtos
{
    public class CheckbookRequestResponseDto
    {
        public int Id { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public int Leaves { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
