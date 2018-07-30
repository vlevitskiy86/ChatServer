using ChatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public interface IMessageService
    {
        Task<List<Message>> Messages();
        Task<List<Message>> NewMessages();
        Task AddMessage(Message message);
        Task MarkMessageAsRead(int id);
        Task DeleteMessage(int id);
    }
}
