using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalrChatSample
{
    public class ChatHub : Hub
    {
        static List<string> _users = new List<string>();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddLoggedInUser(string user)
        {
            if (!_users.Contains(user))
            {
                _users.Add(user);
                _users = new List<string>(_users.OrderBy(u => u));
                await Clients.All.SendAsync("UpdateLoggedInUsers", _users);
            }
        }

        public async Task RemoveLoggedInUser(string user)
        {
            if (_users.Contains(user))
            {
                _users.Remove(user);
                await Clients.All.SendAsync("UpdateLoggedInUsers", _users);
            }
        }
    }
}
