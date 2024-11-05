using Microsoft.IdentityModel.Tokens;
using momken_backend.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace momken_backend.Services
{
    public class TokenService:ITokenService
    {
        public string GenerateToken(Guid userId,int monthNumper,string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                   new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(CustomClaimTypes.MonthsCount, monthNumper.ToString())
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(1),

            };
            var securtyToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securtyToken);
            return accessToken;
        }
        public ClaimsPrincipal VerifyToken(string token,string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false, // Set to true if you have an issuer to validate
                ValidateAudience = false, // Set to true if you have an audience to validate
                ClockSkew = TimeSpan.Zero // Optional: Reduce the clock skew
            };

            //try
            //{

                return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            //}
            //catch (SecurityTokenExpiredException)
            //{
            //    throw new SecurityTokenExpiredException("Token has expired.");
            //}
            //catch (SecurityTokenException ex)
            //{
            //    throw new SecurityTokenException("Token validation failed: " + ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("An error occurred while validating the token: " + ex.Message);
            //}
        }
    }

    public interface ITokenService
    {
        public string GenerateToken(Guid userId, int monthNumper, string secretKey);
        public ClaimsPrincipal VerifyToken(string token, string secretKey);
    }
        public static class CustomClaimTypes
        {
            public const string MonthsCount = "months_count";
        }
}
