﻿using Data.DataModels;
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
        void AdministratorinfoEdit(AdminProfile Modal);
        void MailinginfoEdit(AdminProfile modal);
        byte[] DownloadExcle(NewStateData model);
    }
}