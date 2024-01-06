
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public sealed class ChatHub : Hub
{
    public async Task SendMessage(string username, string message)
    {
        await Clients.All.SendAsync(HubMethods.SendMessage, message);
    }
}