using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatApi
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            Groups.AddToGroupAsync(Context.ConnectionId, name);

            return base.OnConnectedAsync();
        }

        //public async Task SendMessage(string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        //}
    }
}