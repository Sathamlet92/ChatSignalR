using BlazingChat.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazingChat.Server.Hubs;
public class ChatHub : Hub
{
    public async Task SendMessageAsync(Messages message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}