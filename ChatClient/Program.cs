using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        private string _name;
        private HttpClient _client = new HttpClient();
        private string _baseAPIUri = "http://localhost:5001/";

        static void Main(string[] args)
        {
            var instance = new Program();
            instance.GetMessages().GetAwaiter().GetResult();
            //while (true)
            {
                instance.GetNewMessages().GetAwaiter().GetResult();
                instance.GetNewMessages().GetAwaiter().GetResult();
                instance.GetNewMessages().GetAwaiter().GetResult();
            }
            //Console.WriteLine("Hello World!");
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
