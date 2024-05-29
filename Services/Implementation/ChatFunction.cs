using Data.DataContext;
using Data.DataModels;
using NPOI.SS.Formula.Functions;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ChatFunction : IChatFunction
    {
        private readonly ApplicationDbContext _context;

        public ChatFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public Chathistory SendMessage(string senderId, string receiverId, string message)
        {
            Chathistory chathistory = new Chathistory
            {
                Msg = message,
                Sender = senderId,
                Reciever = receiverId,
                Isread = false,
                Senttime = DateTime.Now,
            };
            _context.Chathistories.Add(chathistory);
            _context.SaveChanges();
            return chathistory;
        }
    }
}
