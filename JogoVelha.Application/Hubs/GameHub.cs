using JogoVelha.Application.Hubs.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JogoVelha.Application.Hubs;

[Authorize]
public class GameHub : Hub
{


    private static readonly Dictionary<string, string> ConnectedUsers = [];

    private static readonly List<GamingTable> UsersGamingTable = [];

    private const int DELAY = 35;

    public async Task Connect(string username)
    {
        var userId = Context.UserIdentifier!;

        var game = GetGamingTableBySinglePlayerEmail(userId);

        if (!ConnectedUsers.TryAdd(userId, username))
        {
            if (game is null) return;

            await Clients.User(userId).SendAsync("receiveMessage", "RECONNECT", "Voltando para a partida");
            await Task.Delay(DELAY);
            // Enviar o que cada player vai ser
            await Clients.User(userId).SendAsync("receiveRoundKey", game.Player1Value);
            // Enviando o que cada um vai ser
            await Clients.User(userId).SendAsync("receiveRound", game.CurrentTurnValue);
            await Task.Delay(DELAY);
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
            await Task.Delay(DELAY);
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
        await Task.Delay(DELAY);
        await Clients.User(player2).SendAsync("receiveMessage", "RECONNECT", message);

        // Enviar o que cada player vai ser
        await Clients.User(player1).SendAsync("receiveRoundKey", gamingTable.Player1Value);
        await Task.Delay(DELAY);
        await Clients.User(player2).SendAsync("receiveRoundKey", gamingTable.Player2Value);

        // Enviando o que cada um vai ser
        await Clients.User(player1).SendAsync("receiveRound", gamingTable.CurrentTurnValue);

        await Clients.User(player2).SendAsync("receiveRound", gamingTable.CurrentTurnValue);
        await Task.Delay(DELAY);
        await Clients.User(player1).SendAsync("updateTable", gamingTable.ArrayTable);
        await Task.Delay(DELAY);
        await Clients.User(player2).SendAsync("updateTable", gamingTable.ArrayTable);
    }


    public async Task SendRefuseToPlayer(string username, string target)
        => await Clients.User(target).SendAsync("receiveMessage", "WARNING", $"O usuário {username} recusou");


    public async Task MarkTable(string email, int row, int col)
    {

        var gamingTable = GetGamingTableBySinglePlayerEmail(email);

        if (gamingTable is null) return;

        var currentPlayerValue = email == gamingTable.Player1 ? gamingTable.Player1Value :  gamingTable.Player2Value;
        var currentPlayer = email == gamingTable.Player1 ? gamingTable.Player1 :  gamingTable.Player2;
        var otherPlayer = currentPlayer == gamingTable.Player1 ? gamingTable.Player2 : gamingTable.Player1;

        if (currentPlayerValue == gamingTable.CurrentTurnValue)
        {
            if (!gamingTable.IsMarked(row, col)) 
            {
                gamingTable.SetValue(row, col, currentPlayerValue);

                if (gamingTable.FindWinnerInTable(currentPlayerValue))
                {
                    var username =  ConnectedUsers[email];
                    await Clients.User(currentPlayer).SendAsync("receiveMessage", "WINNER", $"Parabéns! Você ganhou ^^");
                    await Clients.User(otherPlayer).SendAsync("receiveMessage", "WINNER", $"O usuário(a) {username} ganhou a partida.");
                    ClearGame(gamingTable);
                    return;
                }
                else if (gamingTable.IsFullMarked()) 
                {
                    await Clients.User(gamingTable.Player1).SendAsync("receiveMessage", "WINNER", "Houve empate!");
                    await Clients.User(otherPlayer).SendAsync("receiveMessage", "WINNER", "Houve empate!");
                    ClearGame(gamingTable);
                    return;
                }

                gamingTable.ChangePlayerTurn();
                await Task.Delay(DELAY);
                await Clients.User(gamingTable.Player1).SendAsync("receiveRound", gamingTable.CurrentTurnValue);
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

        await Clients.User(gamingTable.Player1).SendAsync("updateTable", gamingTable.ArrayTable);
        await Clients.User(gamingTable.Player2).SendAsync("updateTable", gamingTable.ArrayTable);
        
    }

    public Task Disconnect(string email) {
        ConnectedUsers.Remove(email);
        return Task.CompletedTask;
    }

    private static GamingTable? GetGamingTableBySinglePlayerEmail(string email)
    {
        return UsersGamingTable.FirstOrDefault(g => g.Player1 == email || g.Player2 == email);
    }

    private static void ClearGame(GamingTable game)
    {
        UsersGamingTable.Remove(game);
    }
}