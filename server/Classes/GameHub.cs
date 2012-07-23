using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalR.Hubs;

namespace Server.Classes
{
    [HubName("GameHub")]
    public class GameHub : Hub, IDisconnect, IConnected
    {
        // Seems like SignalR wouldn't push out to a client unless it calls into a serverside method first.
        // So this is a stub method to get things pumping around.
        // Investigate...
        public void Alive() { }

        // Playing around here....
        public Task Connect()
        {
            //Groups.Add(Context.ConnectionId, "everybody");
            return (Clients.joined(Context.ConnectionId, DateTime.Now.ToString()));
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return (Clients.rejoined(Context.ConnectionId, DateTime.Now.ToString()));
        }

        public Task Disconnect()
        {
            //Groups.Remove(Context.ConnectionId, "everybody");
            return (Clients.leave(Context.ConnectionId, DateTime.Now.ToString()));
        }
    }
}
