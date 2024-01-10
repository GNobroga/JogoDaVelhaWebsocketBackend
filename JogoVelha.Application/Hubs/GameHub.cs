using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using JogoVelha.Application.Hubs.Utils;
using JogoVelha.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NetTopologySuite.IO.GML3;
using Npgsql.Replication;

namespace JogoVelha.Application.Hubs;

[Authorize]
public class GameHub : Hub
{


    private static readonly Dictionary<string, string> ConnectedUsers = [];

    private static readonly List<GamingTable> UsersGamingTable = [];

    public async Task Connect(string username)
    {
        var userId = Context.UserIdentifier!;

        var game = GetGamingTableBySinglePlayerEmail(userId);

        if (game is null)
        {
            ConnectedUsers.Add(userId, username);
        }
        else 
        {

             // Enviar o que cada player vai ser
            await Clients.User(game.Player1).SendAsync("receiveRoundKey", game.Player1Value);
            await Task.Delay(50);
            await Clients.User(game.Player2).SendAsync("receiveRoundKey", game.Player2Value);

            // Enviando o que cada um vai ser
            await Clients.User(game.Player1).SendAsync("receiveRound", game.CurrentTurnValue);
            await Task.Delay(50);
            await Clients.User(game.Player2).SendAsync("receiveRound", game.CurrentTurnValue);
            await Clients.User(userId).SendAsync("receiveMessage", "RECONNECT", "Voltando para a partida");
            await Task.Delay(100);
            await Clients.User(userId).SendAsync("updateTable", game.ArrayTable);
        }
        await base.OnConnectedAsync();
    }

    public async Task SendInvite(string source, string target) 
    {
        if (ConnectedUsers.FirstOrDefault(src => src.Key == target).Key is null)
        {
            await Clients.User(source).SendAsync("receiveMessage", "ERROR", "Usuário não está conectado");
        }
        else if (string.Equals(source, target, StringComparison.InvariantCultureIgnoreCase)) 
        {
            await Clients.User(source).SendAsync("receiveMessage", "ERROR", "Você não pode se convidar");
        }
        else 
        {
            var sender = ConnectedUsers[source];
            await Clients.User(source).SendAsync("receiveMessage", "", "Convite enviado");
            await Task.Delay(50);
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
        await Clients.User(player1).SendAsync("receiveMessage", "RECONNECT", message);
        await Task.Delay(50);
        await Clients.User(player2).SendAsync("receiveMessage", "RECONNECT", message);

        // Enviar o que cada player vai ser
        await Clients.User(player1).SendAsync("receiveRoundKey", gamingTable.Player1Value);
        await Task.Delay(50);
        await Clients.User(player2).SendAsync("receiveRoundKey", gamingTable.Player2Value);

        // Enviando o que cada um vai ser
        await Clients.User(player1).SendAsync("receiveRound", gamingTable.CurrentTurnValue);
        await Task.Delay(50);
        await Clients.User(player2).SendAsync("receiveRound", gamingTable.CurrentTurnValue);

        await Clients.User(player1).SendAsync("updateTable", gamingTable.ArrayTable);
        await Task.Delay(50);
        await Clients.User(player2).SendAsync("updateTable", gamingTable.ArrayTable);
    }


    public async Task SendRefuseToPlayer(string username, string target)
        => await Clients.User(target).SendAsync("receiveMessage", "WARNING", $"O usuário {username} recusou");


    public async Task MarkTable(string email, int row, int col)
    {

        var gamingTable = GetGamingTableBySinglePlayerEmail(email);

        if (gamingTable is null) return;

        var currentPlayerValue = email == gamingTable.Player1 ? gamingTable.Player1Value :  gamingTable.Player2Value;

        if (currentPlayerValue == gamingTable.CurrentTurnValue)
        {
            if (!gamingTable.IsMarked(row, col)) 
            {
                gamingTable.SetValue(row, col, currentPlayerValue);
                gamingTable.ChangePlayerTurn();
                await Clients.User(gamingTable.Player1).SendAsync("receiveRound", gamingTable.CurrentTurnValue);
                await Task.Delay(50);
                await Clients.User(gamingTable.Player2).SendAsync("receiveRound", gamingTable.CurrentTurnValue);
            }
            else 
            {
                await Clients.User(email).SendAsync("receiveMessage", "ERROR", "Já está marcado");
            }
        }
        else 
        {
            await Clients.User(email).SendAsync("receiveMessage", "WARNING", "Não é sua vez de jogar");
        }

        await Task.Delay(50);
        await Clients.User(gamingTable.Player1).SendAsync("updateTable", gamingTable.ArrayTable);
        await Task.Delay(50);
        await Clients.User(gamingTable.Player2).SendAsync("updateTable", gamingTable.ArrayTable);
        
    }

    private static GamingTable? GetGamingTableBySinglePlayerEmail(string email)
    {
        return UsersGamingTable.FirstOrDefault(g => g.Player1 == email || g.Player2 == email);
    }
}