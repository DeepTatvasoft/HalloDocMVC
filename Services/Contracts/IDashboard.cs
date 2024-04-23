using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboard
    {
        PatientDashboardedit PatientDashboard(int temp);
        string editUser(PatientDashboardedit dashedit, int id);
        PatientDashboardedit ViewDocument(int temp, int uid, string tempname);
        public (byte[], string, string) FileDownload(int id);
        public (byte[], string, string) DownloadFile(PatientDashboardedit dashedit, List<string> chk);
        public void AddPatientRequestWiseFile(List<IFormFile> formFile, int reqid);
        PatientReqSubmit SubmitForMe(int id);
        bool checkmail(string email);
        bool checkmailAdmin(string email);
    }
}