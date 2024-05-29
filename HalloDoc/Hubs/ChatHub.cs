using Data.DataModels;
using Microsoft.AspNetCore.SignalR;
using Services.Contracts;

namespace HalloDoc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatFunction chatfunction;

        public ChatHub(IChatFunction chatRepo)
        {
            chatfunction = chatRepo;
        }

        public static Dictionary<string, string> connectedUsers = new();

        public async Task OnConnectedAsync(string senderId)
        {
            if (connectedUsers.ContainsKey(senderId))
            {
                connectedUsers[senderId] = Context.ConnectionId;
            }
            else
            {
                connectedUsers.Add(senderId, Context.ConnectionId);
            }

        }

        public async Task OnDisconnectedAsync(string senderId)
        {
            if (int.Parse(senderId) != 0)
            {
                connectedUsers.Remove(senderId);
            }
        }

        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            string rId = receiverId;
            Chathistory chatMessage = chatfunction.SendMessage(senderId, receiverId, message);
            if (connectedUsers.ContainsKey(rId))
            {
                await Clients.Client(connectedUsers[rId]).SendAsync("ReceiveMessage", chatMessage);
            }
        }
    }
}