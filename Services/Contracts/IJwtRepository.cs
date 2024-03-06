using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IJwtRepository
    {
        string GenerateJwtToken(LoggedInPersonViewModel loggedInPerson);

        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}
