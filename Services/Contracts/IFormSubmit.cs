using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IFormSubmit
    {
        void AddPatientRequestWiseFile(List<IFormFile> formFile, int reqid);
        void BusinessInfo(BusinessSubmit model);
        void ConciergeInfo(ConciergeSubmit model);
        (bool,int) familyinfo(FamilyFriendReqSubmit model);
        void patientinfo(PatientReqSubmit model);
    }
}