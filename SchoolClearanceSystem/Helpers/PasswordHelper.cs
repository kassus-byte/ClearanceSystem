using BCryptNet = BCrypt.Net.BCrypt;

namespace SchoolClearanceSystem.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string plainText)
            => BCryptNet.HashPassword(plainText, workFactor: 12);

        public static bool Verify(string plainText, string hashedPassword)
            => BCryptNet.Verify(plainText, hashedPassword);
    }
}