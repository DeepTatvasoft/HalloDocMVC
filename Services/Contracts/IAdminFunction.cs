using Data.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Services.Contracts
{
    public interface IAdminFunction
    {
        bool loginadmin([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
    }
}