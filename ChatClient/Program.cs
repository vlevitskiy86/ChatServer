using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        private static HubConnection connection;
        private string _name;
        private HttpClient _client = new HttpClient();
        private string _baseAPIUri = "http://localhost:5001/";
        private static string line;

        static void Main(string[] args)
        {
            do
            {
                var instance = new Program();

                connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5001/ChatHub")
                .Build();

                instance.Connect();
                //instance.GetMessages().GetAwaiter().GetResult();
                ////while (true)
                //{
                //    instance.GetNewMessages().GetAwaiter().GetResult();
                //    instance.GetNewMessages().GetAwaiter().GetResult();
                //    instance.GetNewMessages().GetAwaiter().GetResult();
                //}
                line = Console.ReadLine();
                instance.SendMessage(line);
            } while (line != "exit");
        }

        private async void Connect()
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                //this.Dispatcher.Invoke(() =>
                //{
                //    var newMessage = $"{user}: {message}";
                //    messagesList.Items.Add(newMessage);
                //});
                Console.WriteLine("{0} : {1}", user, message);
            });

            try
            {
                await connection.StartAsync();
                //messagesList.Items.Add("Connection started");
                //connectButton.IsEnabled = false;
                //sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void SendMessage(string text)
        {
            try
            {
                await connection.InvokeAsync("SendMessage",
                    "User 1", text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task GetMessages()
        {
            var result = await _client.GetAsync(_baseAPIUri + "api/chat/messages", HttpCompletionOption.ResponseContentRead);
            var text =  await result.Content.ReadAsStringAsync();
            Console.WriteLine(text);
        }

        private async Task GetNewMessages()
        {
            var result = await _client.GetAsync(_baseAPIUri + "api/chat/newmessages", HttpCompletionOption.ResponseContentRead);
            var text = await result.Content.ReadAsStringAsync();
            await _client.GetAsync(_baseAPIUri + "api/chat/newmessagesread", HttpCompletionOption.ResponseContentRead);
            Console.WriteLine(text);
        }
    }
}
