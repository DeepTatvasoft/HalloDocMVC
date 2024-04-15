using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IPhysicianFunction
    {
        NewStateData AdminDashboard(int phyid);
        NewStateData AdminDashboarddata(int status, int currentPage, int phyid, string searchkey = "");
        int getActiveRequestCount(int phyid);
        int getConcludeRequestCount(int phyid);
        int getNewRequestCount(int phyid);
        int getPendingRequestCount(int phyid);
        NewStateData toogletable(string reqtypeid, string status, int currentPage, int phyid, string searchkey = "");
        void PhysicianAccept(int id);
        IActionResult DownloadEncounter(int id);
        void PhysicianNotesSaveChanges(int reqid, string physiciannotes, string physicianname);
    }
}