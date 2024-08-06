using Microsoft.AspNetCore.SignalR;
using System;

namespace GlobalChat
{
    public class Chathub : Hub
    {
        public static Dictionary<string, string> map { get; set; } = new Dictionary<string, string>();

        // ... Add some keys and values.
        
        public override async Task OnConnectedAsync()
        {
            try
            {
                Console.WriteLine($"User connected: {Context.ConnectionId}");
                var userId = Context.GetHttpContext().Request.Query["userId"].ToString();

                map.Add(userId,Context.ConnectionId);

                foreach ( var pair in map )
                {
                    Console.WriteLine(pair.Key +" " + pair.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during OnConnectedAsync: {ex}");
            }

            await base.OnConnectedAsync();
        }

        public async Task sendMessage(string myId,string userId,string message)
        {
            await Clients.Client(map[userId]).SendAsync("sendMessage",myId,message);
        }

        public async Task deleteMessage(string userId)
        {

        }

        public async Task sendGroupMessage(string groupId,string message)
        {
            Clients.Group(groupId).SendAsync("sendGroupMessage", message);
            Console.WriteLine("Group message sent successfully.");
        }

        public async Task deleteGroupMessage(string groupId)
        {

        }

        public async Task JoinGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
            Console.WriteLine($"User joined group {groupId}.");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                Console.WriteLine($"User disconnected: {Context.ConnectionId}");
                Console.WriteLine($"User disconnected: {map.Count}");
                string r = null;

                foreach (var pair in map)
                {
                    if (pair.Value == Context.ConnectionId.ToString())
                    {
                        r = pair.Key;
                        break;
                    }
                }

                map.Remove(r);
                Console.WriteLine("User with userid " + r + "removed sucessfully");
                Console.WriteLine($"Map size: {map.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during OnConnectedAsync: {ex}");
            }

            await base.OnConnectedAsync();


        }
        

    
    }
}
