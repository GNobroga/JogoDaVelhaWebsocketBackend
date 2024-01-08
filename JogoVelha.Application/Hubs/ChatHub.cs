using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace JogoVelha.Application.Hubs;

public sealed class ChatHub : Hub
{
    private class Action 
    {
        public const string SEND_MESSAGE = "sendMessage";
        public const string RECEIVE_MESSAGE = "receiveMessage";
        public const string USER_JOINED = "userJoined";
    }

    readonly ConcurrentDictionary<string, string> _users = new();
    
    public async Task Connect(string username) {
        if (_users.ContainsKey(username))
            return;
        _users[username] = Context.ConnectionId;
        await Clients.All.SendAsync(Action.SEND_MESSAGE, $"O usuário {username} foi conectado.");
    }

}