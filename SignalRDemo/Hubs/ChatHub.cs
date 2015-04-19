using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRDemo.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub<IChatClient>
    {
        public override Task OnConnected()
        {
            object address;
            Context.Request.Environment.TryGetValue("server.RemoteIpAddress", out address);
            Clients.Others.Broadcast(string.Format("{0} connected", address as string));
            return base.OnConnected();
        }

        public void SendMessage(ChatMessageModel model)
        {
            Clients.All.Broadcast(string.Format("{0}: {1}", model.Name, model.Message));
        }
    }

    public interface IChatClient
    {
        void Broadcast(string messageModel);
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