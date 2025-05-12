using BCrypt.Net;

namespace RentNest.Core.UtilHelper
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public static bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
