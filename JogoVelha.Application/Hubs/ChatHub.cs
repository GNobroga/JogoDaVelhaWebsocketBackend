using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JogoVelha.Application.Hubs;

[Authorize]
public sealed class ChatHub : Hub
{
    private class Action 
    {
        public const string SEND_MESSAGE = "sendMessage";
        public const string RECEIVE_MESSAGE = "receiveMessage";
        
    }

    public async Task SendMessage(string from, string message) 
    {
        await Clients.All.SendAsync(Action.SEND_MESSAGE, from, message);
    }

}