namespace BPIBankSystem.API.Entities
{
    public class UserLoginAttempt
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; } = false;
        public DateTime AttemptTime { get; set; } = DateTime.UtcNow;
    }

}
