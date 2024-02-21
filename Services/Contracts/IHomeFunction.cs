using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IHomeFunction
    {
        (Aspnetuser,User) ValidateUser([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
        public (bool,string) changepassword(ResetPasswordVM vm, string id);
    }
}