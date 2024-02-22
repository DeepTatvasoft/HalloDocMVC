using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminFunction
    {
        bool loginadmin([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
        NewStateData AdminDashboarddata();
    }
}