using Data.DataModels;

namespace Services.Contracts
{
    public interface IChatFunction
    {
        Chathistory SendMessage(string senderId, string receiverId, string message);
    }
}