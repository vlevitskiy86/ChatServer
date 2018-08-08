using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Models;
using ChatApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IMessageService _service;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IMessageService service, IHubContext<ChatHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpPost("NewMessage")]
        public IActionResult Post([FromBody] Message message)
        {
            if (_service.AddMessage(message))
            {
                _hubContext.Clients.All.SendAsync("ReceiveMessage", message.AuthorName, message.Text);
                return NoContent();
            }
            else
            {
                return StatusCode(500);
            }
        }

        // GET api/
        [HttpGet("Messages")]
        public IActionResult AllMessages()
        {
            return Ok(_service.GetMessages());
        }

        [HttpGet("NewMessages")]
        public IActionResult NewMessages()
        {
            return Ok(_service.GetNewMessages());
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
