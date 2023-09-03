using BlazingChat.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazingChat.Server.Hubs;
public class ChatHub : Hub
{
    public async Task SendMessageAsync(Messages message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public Task SendMessageToUserAsync(Messages message)
    {
        var users = new string[]{message.ToUserId!, message.FromUserId! };
        return Clients.Users(users).SendAsync("ReceiveMessage", message);
    }
}