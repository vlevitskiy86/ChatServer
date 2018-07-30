using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IList<MessageModel> Messages = new List<MessageModel>();

        public ChatController()
        {
            Populate();
        }

        private void Populate()
        {
            Messages.Add(new MessageModel { AuthorName = "John1", Text = "Ldfkj dkcjdsfsf asdlk laksdj", Id = Messages.Count });
            Messages.Add(new MessageModel { AuthorName = "John2", Text = "Ldfkj gdhdfgdkcjasdfglk laksdj", Id = Messages.Count });
            Messages.Add(new MessageModel { AuthorName = "John3", Text = "Ldfkj asd dkcjasdlk ldaksdj", Id = Messages.Count });
            Messages.Add(new MessageModel { AuthorName = "John4", Text = "Ldfkj uiyuidkcjasdlk trlaksdj", Id = Messages.Count });
        }

        [HttpPost]
        public void Post([FromBody] MessageModel message)
        {
            Messages.Add(message);
            //UpdateAllClients();
        }

        // GET api/
        [HttpGet("Messages")]
        public IEnumerable<MessageModel> AllMessages()
        {
            return Messages;
        }

        [HttpGet("NewMessages")]
        public IEnumerable<MessageModel> NewMessages()
        {
            return Messages.Where(x => x.IsNew);
        }

        [HttpGet("NewMessagesRead")]
        public string NewMessagesRead()
        {
            Messages.Where(x => x.IsNew).ToList().ForEach(x => x.IsNew = false);
            return "Ok";
        }

        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
