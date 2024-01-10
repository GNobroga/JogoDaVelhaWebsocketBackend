using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JogoVelha.Application.Hubs.Utils;
using JogoVelha.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JogoVelha.Application.Hubs;

[Authorize]
public class GameHub : Hub
{


    private static readonly Dictionary<string, string> ConnectedUsers = [];

    private static readonly List<GamingTable> UsersGamingTable = [];


    public async Task Connect(string username)
    {
        var userId = Context.UserIdentifier!;

        var user = UsersGamingTable.FirstOrDefault(game => 
            game.Player1.Equals(userId, StringComparison.InvariantCultureIgnoreCase) || 
            game.Player2.Equals(userId, StringComparison.InvariantCultureIgnoreCase));

        if (user is null)
        {
            ConnectedUsers.Add(userId, username);
        }
        else 
        {
            await Clients.User(userId).SendAsync("reconnect", "Voltando para a partida");
        }
        await base.OnConnectedAsync();
    }

    public async Task SendInvite(string source, string target) 
    {
        if (ConnectedUsers.FirstOrDefault(src => src.Key == target).Key is null)
        {
            await Clients.User(source).SendAsync("receiveErrorMessage", "Usuário não está conectado");
        }
        else if (string.Equals(source, target, StringComparison.InvariantCultureIgnoreCase)) 
        {
            await Clients.User(source).SendAsync("receiveErrorMessage", "Você não pode se convidar");
        }
        else 
        {
            var sender = ConnectedUsers[source];
            await Clients.User(source).SendAsync("receiveMessage", "Convite enviado");
            await Clients.User(target).SendAsync("receiveInvite", $"O usuário {sender} te convidou para um partida.", source);
        }
    }

    public async Task StartGame(string player1, string player2) 
    {   
        var gamingTable = new GamingTable()
        {
            Player1 = player1,
            Player2 = player2,
        };

        UsersGamingTable.Add(gamingTable);

        var message = "A partida já pode começar!";
        await Clients.User(player1).SendAsync("reconnect", message);
        await Clients.User(player2).SendAsync("reconnect", message);
    }

   
}