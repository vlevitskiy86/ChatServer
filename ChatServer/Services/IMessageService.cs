using ChatApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Services
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
