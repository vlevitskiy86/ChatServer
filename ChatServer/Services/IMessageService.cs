using ChatApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Services
{
    public interface IMessageService
    {
        IEnumerable<Message> GetMessages();
        IEnumerable<Message> GetNewMessages();
        bool AddMessage(Message message);
        bool MarkMessageAsRead(int id);
        bool DeleteMessage(int id);
    }
}
