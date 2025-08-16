namespace BPIBankSystem.API.DTOs.Requests
{
    public class OtpTransferRequestDto
    {
        public string FromAccountNumber { get; set; }
        public string TransactionPassword { get; set; }
    }
}
