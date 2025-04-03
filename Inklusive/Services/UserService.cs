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

       

    }
}
