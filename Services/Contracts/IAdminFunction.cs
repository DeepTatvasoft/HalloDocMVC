using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminFunction
    {
        (bool, string, int) loginadmin([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
        NewStateData AdminDashboarddata(int status, int currentPage, string searchkey);
        NewStateData toogletable(string reqtypeid, string status, int currentPage, string searchkey);
        NewStateData1 ViewCase(int id);
        NewStateData regiontable(int regionid, string status, int currentPage, string searchkey);
        NewStateData RegionReqtype(int regionid, string reqtypeid, string status, int currentPage, string searchkey);
        NewStateData AdminDashboard();
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
        List<Healthprofessionaltype> getprofession();
        List<Healthprofessional> filterprofession(int professionid);
        Healthprofessional filterbusiness(int vendorid);
        void OrderSubmit(SendOrders sendorder);
        void clearcase(int reqid);
        void CancelAgreement(Agreementmodal modal);
        void AcceptAgreement(int id);
        void CloseCasebtn(int id);
        void Closecaseedit([FromForm] AdminviewDoc formData);
        void AdminResetPassword(AdminProfile modal);
        AdminProfile Profiletab(int adminid);
        bool AdministratorinfoEdit(AdminProfile Modal, List<string> chk);
        void MailinginfoEdit(AdminProfile modal);
        byte[] DownloadExcle(NewStateData model);
        ProviderModal Providertab(int regionid);
        Agreementmodal ReviewAgreement(int id);
        EditPhysicianModal EditPhysician(int id);
        void PhysicianAccInfo(EditPhysicianModal modal, string adminname);
        void PhysicianResetPass(EditPhysicianModal modal, string adminname);
        bool PhysicianInfo(EditPhysicianModal modal, string adminname, List<string> chk);
        void PhysicianMailingInfo(EditPhysicianModal modal, string adminname);
        void ProviderProfile(EditPhysicianModal modal, string adminname);
        void ContactPhysician(int phyid, string chk, string message);
        void EditProviderSign(int physicianid, string base64string);
        void EditProviderPhoto(int physicianid, string base64string);
        void PhyNotification(List<string> chk);
        void DeletePhysician(EditPhysicianModal modal);
        AccessRoleModal AccessTab();
        AccessRoleModal CreateRole();
        void CreateRoleSubmit(AccessRoleModal modal, List<string> chk, string adminname);
        List<Menu> filtermenu(int acctype);
        AccessRoleModal EditRole(int roleid);
        void EditRoleSubmit(AccessRoleModal modal, List<string> chk, string adminname);
        void DeleteRole(int roleid);
        EditPhysicianModal CreateProviderAcc();
        void CreateProviderAccBtn(EditPhysicianModal modal, List<string> chk, string adminname);
        AdminProfile CreateAdminAcc();
        void CreateAdminAccBtn(AdminProfile modal, List<string> chk, string adminname);
        void phyuploadDoc(int physicianid, string doctype);
    }
}