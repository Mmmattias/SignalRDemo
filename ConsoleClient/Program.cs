using System;
using System.Configuration;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Skapa uppkopplingen och proxyn
            var hubUrl = ConfigurationManager.AppSettings["SignalRHubUrl"];
            var connection = new HubConnection(hubUrl);
            var hub = connection.CreateHubProxy("chatHub");

            connection.Error += ex => Console.WriteLine("Error: {0}", ex.Message);


            // Vad händer när servern anropar klienten (i det här fallet metoden "Broadcast")?
            hub.On<string>("Broadcast", message => Console.WriteLine(message));
            
            // Starta uppkopplingen
            connection.Start().Wait();

            Console.WriteLine("What's your name?");
            var name = Console.ReadLine();

            while (true)
            {
                var messageToSend = Console.ReadLine();
                var messageModel = new ChatMessageModel(name, messageToSend);

                // Skicka meddelande till servern
                hub.Invoke<ChatMessageModel>("SendMessage", messageModel);
            }
        }
    }

    public class ChatMessageModel
    {
        public ChatMessageModel(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public string Name { get; set; }
        public string Message { get; set; }
    }
}
