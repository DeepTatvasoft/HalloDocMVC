using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAccess.ServiceRepository
{
    public class JwtRepo : IJwtRepository
    {
        public readonly IConfiguration configuration;

        public JwtRepo(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateJwtToken(LoggedInPersonViewModel loggedInPerson)
        {
            var claims = new List<Claim>
            {
                new Claim("AspNetId" , loggedInPerson.aspuserid.ToString()),
                new Claim("UserName" , loggedInPerson.username),
                new Claim("Role" , loggedInPerson.role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(20);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null) { return false; }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;

                if (jwtSecurityToken != null) { return true; }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}


