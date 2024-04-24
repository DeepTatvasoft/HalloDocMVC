using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IFormSubmit
    {
        void AddPatientRequestWiseFile(List<IFormFile> formFile, int reqid);
        (bool, int) BusinessInfo(BusinessSubmit model);
        (bool, int) ConciergeInfo(ConciergeSubmit model);
        (bool, int) familyinfo(FamilyFriendReqSubmit model);
        void patientinfo(PatientReqSubmit model, int adminid);
        void Emailentry(string email, int id);
        bool CheckEmail(string email);
    }
}