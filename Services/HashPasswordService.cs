using Microsoft.AspNetCore.Identity;

namespace momken_backend.Services
{
    public class HashPasswordService: IHashPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher;
        public HashPasswordService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }
       public string HashPassword(string Password)
        {
            return _passwordHasher.HashPassword(null, Password);
        }
       public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }

      public interface IHashPasswordService
    {
        string HashPassword(string Password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
