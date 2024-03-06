using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminFunction
    {
        (bool, string, int) loginadmin([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
        NewStateData AdminDashboarddata(int status1, int status2, int status3);
        NewStateData toogletable(string reqtypeid, string status);
        NewStateData1 ViewCase(int id);
        NewStateData regiontable(int regionid, string status);
        int getToCloseRequestCount();
        int getActiveRequestCount();
        int getConcludeRequestCount();
        int getNewRequestCount();
        int getPendingRequestCount();
        int getUnpaidRequestCount();
        void cancelcase(int reqid, int casetagid, string cancelnotes, string adminname, int id);
        List<Physician> filterregion(string regionid);
        void assigncase(int reqid, int regid, int phyid, string Assignnotes, string adminname, int id);
        void blockcase(int reqid, string Blocknotes);
        ViewNotesModel ViewNotes(int reqid);
        void AdminNotesSaveChanges(int reqid, string adminnotes, string adminname);
        AdminviewDoc AdminuploadDoc(int reqid);
        int SingleDelete(int reqfileid);
        List<string> SendMail(List<int> reqwiseid, int reqid);
    }
}