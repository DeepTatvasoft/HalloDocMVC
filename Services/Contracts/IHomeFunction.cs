using Data.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Services.Contracts
{
    public interface IHomeFunction
    {
        (Aspnetuser,User) ValidateUser([Bind(new[] { "Email,Passwordhash" })] Aspnetuser aspNetUser);
    }
}