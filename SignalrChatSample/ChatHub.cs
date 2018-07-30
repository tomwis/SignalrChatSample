using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrChatSample
{
    public class ChatHub : Hub
    {
        private readonly string _loggedInUsersKey = "LoggedInUsers";
        private readonly IDistributedCache _cache;

        public ChatHub(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddLoggedInUser(string user)
        {
            var users = await GetUsersFromCache();

            if (!users.Contains(user))
            {
                users.Add(user);
                users = new List<string>(users.OrderBy(u => u));
                await _cache.SetAsync(_loggedInUsersKey, GetEncodedUsers(users));
                await Clients.All.SendAsync("UpdateLoggedInUsers", users);
            }
        }

        private byte[] GetEncodedUsers(List<string> users)
        {
            var str = string.Join(";;", users.Select(s => s.Replace(";", "/;/")));

            return Encoding.UTF8.GetBytes(str);
        }

        private async Task<List<string>> GetUsersFromCache()
        {
            var value = await _cache.GetAsync(_loggedInUsersKey);

            return Encoding.UTF8.GetString(value ?? new byte[0]).Split(";;", StringSplitOptions.RemoveEmptyEntries).Select(s => s.Replace("/;/", ";")).ToList();
        }

        public async Task RemoveLoggedInUser(string user)
        {
            var users = await GetUsersFromCache();

            if (users.Contains(user))
            {
                users.Remove(user);
                await _cache.SetAsync(_loggedInUsersKey, GetEncodedUsers(users));
                await Clients.All.SendAsync("UpdateLoggedInUsers", users);
            }
        }
    }
}
