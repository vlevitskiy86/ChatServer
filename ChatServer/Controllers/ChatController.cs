using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Models;
using ChatServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        //private readonly IList<Message> Messages = new List<Message>();
        IMessageService _service;

        public ChatController(IMessageService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Message message)
        {
            _service.AddMessage(message);

            return Ok();
        }

        // GET api/
        [HttpGet("Messages")]
        public List<Message> AllMessages()
        {
            return _service.Messages().GetAwaiter().GetResult();
        }

        [HttpGet("NewMessages")]
        public List<Message> NewMessages()
        {
            return _service.NewMessages().GetAwaiter().GetResult();
        }

        //[HttpGet("NewMessagesRead")]
        //public string MessageReadStatus()
        //{
        //    var items = Messages.Where(x => x.IsNew);
        //    foreach (var i in items)
        //        i.IsNew = false;
        //    return "Ok";
        //}

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
