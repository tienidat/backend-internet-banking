namespace BPIBankSystem.API.Data
{
    public class CommonResponse<T>
    {
        public string status { get; set; }
        public string message { get; set; }
        public T data { get; set; }

        public CommonResponse() { }

        public CommonResponse(string status, string message, T? data = default)
        {
            this.status = status;
            this.message = message;
            this.data = data;
        }
    }
}
