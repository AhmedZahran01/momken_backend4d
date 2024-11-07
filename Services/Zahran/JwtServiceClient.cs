using Microsoft.IdentityModel.Tokens;
using momken_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace momken_backend.Services.Zahran
{
    public class JwtServiceClient: IJwtServiceclient
    {

        private readonly Dtos.JwtOptionPartner _jwtOptionPartner;
        public JwtServiceClient(Dtos.JwtOptionPartner jwtOptionPartner)
        {
            _jwtOptionPartner = jwtOptionPartner;
        }
        public string creatJwtToken(Client client)
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
                        new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                        new(ClaimTypes.MobilePhone,client.PhoneNumber),
                        new(ClaimTypes.Email,client.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(_jwtOptionPartner.Lifetime),

            };
            var securtyToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securtyToken);
            return accessToken;
        }

    }

    public interface IJwtServiceclient
    {
        string creatJwtToken(Client client);
    }
}
