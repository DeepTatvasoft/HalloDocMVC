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
        void Transfer(int reqid, string Assignnotes, int phyid);
        void EncounterBtn(NewStateData modal, string[] encounterchk, int phyid);
        void HouseCallBtn(int id);
        EncounterFormViewModel EncounterForm(int id);
        void EncounterFormSubmit(EncounterFormViewModel model);
        void EncounterFormFinalize(EncounterFormViewModel modal);
        bool ConcludeCareBtn(AdminviewDoc modal, int phyid);
        MonthWiseScheduling LoadSchedulingPartial(string date, int regionid, int phyid);
    }
}