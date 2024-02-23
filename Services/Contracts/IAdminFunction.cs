using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminFunction
    {
        (bool,string) loginadmin([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
        NewStateData AdminDashboarddata(int status1,int status2,int status3);
    }
}