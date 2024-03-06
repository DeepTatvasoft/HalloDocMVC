using Microsoft.AspNetCore.Mvc.Filters;

namespace Services.Contracts
{
    public interface IAuthorization
    {
        void OnAuthorization(AuthorizationFilterContext context);
    }
}