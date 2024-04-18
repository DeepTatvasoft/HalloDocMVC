using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Services.Contracts;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Implementation
{
    public class Authorization : Attribute, IAuthorization, IAuthorizationFilter
    {
        private readonly string _role;

        public Authorization(string role = "")
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var jwtservice = context.HttpContext.RequestServices.GetService<IJwtRepository>();
            if (jwtservice == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "adminlogin" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "adminlogin" }));
                return;
            }

            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "adminlogin" }));
                return;
            }

            if (string.IsNullOrEmpty(_role) || !_role.Contains(roleClaim.Value))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "adminlogin" }));
            }
        }
    }
}
