using Microsoft.IdentityModel.Tokens;
using momken_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace momken_backend.Services
{
    public class JwtServicePartner:IJwtServicePartner
    {
        private readonly Dtos.JwtOptionPartner _jwtOptionPartner;
        public JwtServicePartner(Dtos.JwtOptionPartner jwtOptionPartner)
        {
            _jwtOptionPartner = jwtOptionPartner;
        }
        public string creatJwtToken(Partner partner)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptionPartner.Issuer,
                Audience = _jwtOptionPartner.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptionPartner.SingningKey)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new(ClaimTypes.NameIdentifier,partner.Id.ToString()),
                        new(ClaimTypes.MobilePhone,partner.PhoneNumber),
                        new(ClaimTypes.Email,partner.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(_jwtOptionPartner.Lifetime),

            };
            var securtyToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securtyToken);
            return accessToken;
        }
    }
    public interface IJwtServicePartner
    {
        string creatJwtToken(Partner partner);
    }
}
