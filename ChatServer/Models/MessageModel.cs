using System;

namespace ChatServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public DateTime When { get; private set; } = DateTime.Now;
        public bool IsNew { get; set; } = true;
    }
}
