using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IHomeFunction
    {
        (Aspnetuser,User) ValidateUser([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
        public (bool,string) changepassword(ResetPasswordVM vm, string id);
        public Aspnetuser getaspuser(PatientReqSubmit model);
        void newaccount(PatientReqSubmit model, string id);
        PatientReqSubmit CreateAccount(string id);
        ChatBoxModal ChatwithProvider(int phyid);
        ChatBoxModal ChatwithAdmin(int sendaerid, int receiverid);
        List<Chathistory> GetAllChat(int sendid,int thisid);
    }
}