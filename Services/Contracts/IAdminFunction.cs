using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminFunction
    {
        (bool,string) loginadmin([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
        NewStateData AdminDashboarddata(int status1,int status2,int status3);
        NewStateData toogletable(string reqtypeid, string status);
        NewStateData1 ViewCase(int id);
        NewStateData regiontable(int regionid, string status);
        int getToCloseRequestCount();
        int getActiveRequestCount();
        int getConcludeRequestCount();
        int getNewRequestCount();
        int getPendingRequestCount();
        int getUnpaidRequestCount();
    }
}