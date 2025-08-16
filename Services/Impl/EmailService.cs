using System.Net.Mail;
using System.Net;

namespace BPIBankSystem.API.Services.Impl
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress("binhricardo@gmail.com", "BPIBank System");
            var toAddress = new MailAddress(toEmail);

            const string fromPassword = "drjk dzsb iqls qorj"; // App password của Gmail

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            await smtp.SendMailAsync(message);
        }
    }

}
