namespace BPIBankSystem.API.DTOs.Requests
{
    public class ConfirmTransferDto
    {
        public TransferRequestDto TransferRequest { get; set; }
        public string Otp { get; set; }
    }
}
