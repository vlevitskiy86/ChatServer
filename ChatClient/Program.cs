using IdentityModel.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        private static HubConnection connection;
        private static string _name;
        private static string _password;
        private static string _token;
        private static HttpClient _client = new HttpClient();
        private static string _baseAPIUri = "http://localhost:5001/";
        private static string line;

        static void Main(string[] args)
        {
            Console.WriteLine("What is your name?");
            _name = Console.ReadLine();

            Console.WriteLine("What is your password?");
            _password = Console.ReadLine();

            CallToTokenServer();

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5001/chatHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token);
                })
                //.WithUrl("http://localhost:5001/chatHub")
                .Build();

            Connect();

            do
            {

                //GetMessages().GetAwaiter().GetResult();
                ////while (true)
                //{
                //    instance.GetNewMessages().GetAwaiter().GetResult();
                //    instance.GetNewMessages().GetAwaiter().GetResult();
                //    instance.GetNewMessages().GetAwaiter().GetResult();
                //}
                line = Console.ReadLine();
                SendMessage(line);
            } while (line.ToLower() != "exit");
        }

        public static void CallToTokenServer()
        {
            var disco = DiscoveryClient.GetAsync("http://localhost:5000").GetAwaiter().GetResult();
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            //var tokenResponse = tokenClient.RequestClientCredentialsAsync("api1").GetAwaiter().GetResult();

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(_name, _password, "api1").GetAwaiter().GetResult();

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            _token = tokenResponse.AccessToken;
            //_client.SetBearerToken(_token);

            //var response = _client.GetAsync("http://localhost:5001/identity").GetAwaiter().GetResult();
            //if (!response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(response.StatusCode);
            //}
            //else
            //{
            //    var content = response.Content.ReadAsStringAsync();
            //    Console.WriteLine(JArray.Parse(content.Result));
            //}
        }

        private static async void Connect()
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

        private static async void SendMessage(string text)
        {
            try
            {
                await connection.InvokeAsync("SendMessage",
                    _name, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async Task GetMessages()
        {
            var result = await _client.GetAsync(_baseAPIUri + "api/chat/messages", HttpCompletionOption.ResponseContentRead);
            var text = await result.Content.ReadAsStringAsync();
            Console.WriteLine(text);
        }

        //private async Task GetNewMessages()
        //{
        //    var result = await _client.GetAsync(_baseAPIUri + "api/chat/newmessages", HttpCompletionOption.ResponseContentRead);
        //    var text = await result.Content.ReadAsStringAsync();
        //    await _client.GetAsync(_baseAPIUri + "api/chat/newmessagesread", HttpCompletionOption.ResponseContentRead);
        //    Console.WriteLine(text);
        //}
    }
}
