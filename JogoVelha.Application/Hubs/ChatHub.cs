using System.Collections.Concurrent;
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
        public const string USER_JOINED = "userJoined";
    }

    
    public async Task Connect(string username) 
    {
        await Clients.AllExcept(Context.ConnectionId).SendAsync(Action.SEND_MESSAGE, $"{username} foi conectado ao Chat.");
    }

    public async Task SendMessage(string from, string message) 
    {
        await Clients.All.SendAsync(Action.SEND_MESSAGE, from, message);
    }

}