using System.Security.Cryptography;

namespace LibraryManagementSystem.Helpers
{
    public class PasswordGenerator
    {
        public static string GeneratePassword(int length = 8)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!.@#$%^&*";
            
            var passwordChars = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < length; i++)
                {
                    passwordChars[i] = allowedChars[randomBytes[i] % allowedChars.Length];
                }
            }

            return new string(passwordChars);
        }
    }
}
