using Microsoft.AspNetCore.Http;
using Services.ViewModel;

namespace Services.Contracts
{
    public interface IFormSubmit
    {
        void AddPatientRequestWiseFile(List<IFormFile> formFile, int reqid);
        void BusinessInfo(BusinessSubmit model);
        void ConciergeInfo(ConciergeSubmit model);
        void familyinfo(FamilyFriendReqSubmit model);
        void patientinfo(PatientReqSubmit model);
    }
}