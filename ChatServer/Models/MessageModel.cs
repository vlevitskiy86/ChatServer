using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public DateTime When { get; private set; } = DateTime.Now;
        public bool IsNew { get; set; } = true;
    }
}
