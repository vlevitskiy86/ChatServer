using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Data;
using ChatApi.Models;

namespace ChatApi.Services
{
    public class MessageService : IMessageService
    {
        private MessageContext _context;

        public MessageService(MessageContext context)
        {
            _context = context;
            if (_context.Messages.Count() == 0)
                Populate();
        }

        private void Populate()
        {
            _context.Messages.Add(new Message { AuthorName = "John1", Text = "Ldfkj dkcjdsfsf asdlk laksdj", Id = _context.Messages.Count() });
            _context.Messages.Add(new Message { AuthorName = "John2", Text = "Ldfkj gdhdfgdkcjasdfglk laksdj", Id = _context.Messages.Count() });
            _context.Messages.Add(new Message { AuthorName = "John3", Text = "Ldfkj asd dkcjasdlk ldaksdj", Id = _context.Messages.Count() });
            _context.Messages.Add(new Message { AuthorName = "John4", Text = "Ldfkj uiyuidkcjasdlk trlaksdj", Id = _context.Messages.Count() });

            _context.SaveChangesAsync();
        }

        public bool AddMessage(Message message)
        {
            message.Id = _context.Messages.Count() + 1;
            _context.Messages.Add(message);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages;
        }

        public IEnumerable<Message> GetNewMessages()
        {
            return _context.Messages.Where(x => x.IsNew == true).ToList();
        }

        public bool MarkMessageAsRead(int id)
        {
            _context.Messages.Find(id).IsNew = false;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteMessage(int id)
        {
            var obj = _context.Messages.Find(id);
            _context.Messages.Remove(obj);
            return _context.SaveChanges() > 0;
        }
    }
}
