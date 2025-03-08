using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
namespace Inklusive.Services
{
    public static class UserService
    {

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
        public static bool VerifyPassword(string hashedPassword, string enteredPassword)
        {
            return UserService.HashPassword(enteredPassword) == hashedPassword;
        }
        public static string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString().Substring(0, 8) + "@A1";
        }

        public static void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("dummyx2025x@gmail.com", "post zskl zzwk uzfr"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("dummyx2025x@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }

        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("dummyx2025x@gmail.com", "post zskl zzwk uzfr"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("dummyx2025x@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }

    }
}
