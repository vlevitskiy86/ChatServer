using IdentityModel.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

            ConnectToSignalr();

            do
            {
                //GetMessages().GetAwaiter().GetResult();
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

            //var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("test@mail.com", "Pass123#", "api1").GetAwaiter().GetResult();
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            _token = tokenResponse.AccessToken;
            _client.SetBearerToken(_token);
        }

        private static async void ConnectToSignalr()
        {
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5001/chatHub", options =>
               {
                   options.AccessTokenProvider = () => Task.FromResult(_token);
               })
               .Build();

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine("{0} : {1}", user, message);
            });

            try
            {
                await connection.StartAsync();
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
                string message = string.Format("'Id' : '5','AuthorName' : '{0}','Text' : '{1}','When' : '{2}','IsNew' : 'true'", _name, text, DateTime.Now.ToShortTimeString());

                var result = await _client.PostAsync(_baseAPIUri + "api/chat/newmessage", new StringContent("{" + message + "}", Encoding.Unicode, "application/json"));

                Console.WriteLine(result.StatusCode); 
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
