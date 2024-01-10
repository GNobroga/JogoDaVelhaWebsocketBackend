namespace JogoVelha.Application.Hubs.Utils;

public class GamingTable
{
    public string Player1 { get; set; } = default!;

    public string Player2 { get; set; } = default!;

    public string Player1Value = default!;
    public string Player2Value = default!;

    private readonly string[,] _table = new string[3, 3];

    private string _currentTurnValue = null!;

    public bool HasWinner { get; set; } = false;

    public GamingTable()
    {
        var value = "X";

        if (Random.Shared.Next(2) % 2 == 0) {
            value = "O";
        }

        Player1Value = value;
        Player2Value = "O";

        if (Player1Value == "O")
        {
            Player2Value = "X";
        }

        _currentTurnValue = RandomPlayerTurn();
    }

    public static string RandomPlayerTurn()
    {
        return Random.Shared.Next(2) % 2 == 0 ? "O" : "X";
    }

    public void Mark(int col, int line, string value)
    {
        _table[col, line] = value;
    }

    public bool FindWinnerInTable(string value)
    {
        return HasColumnsMarked(_table, value) || 
            HasRowsMarked(_table, value) || 
            HasMarkedInDiagonals(_table, value);
    }

    private static bool HasColumnsMarked(string[,] _table, string value)
    {
        for (int col = 0; col < _table.GetLength(1); col++)
        {
            int count = 0;

            for (int line = 0; line < _table.GetLength(0); line++)
            {
                if (_table[line, col].Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    count++;
                }
            }

            if (count >= 3) return true;
        }

        return false;
    }

    private static bool HasRowsMarked(string[,] _table, string value)
    {
        for (int line = 0; line < _table.GetLength(0); line++)
        {
            int count = 0;

            for (int col = 0; col < _table.GetLength(1); col++)
            {
                if (_table[line, col].Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    count++;
                }
            }
            if (count >= 3) return true;
        }
        return false;
    }

    private static bool HasMarkedInDiagonals(string[,] _table, string value)
    {
        int count = 0;

        for (int index = 0; index < _table.GetLength(1); index++)
        {
            if (_table[index, index].Equals(value, StringComparison.InvariantCultureIgnoreCase))
            {
                count++;
            }
        }

        if (count >= 3) return true;
        count = 0;

        int line = _table.GetLength(0) - 1;

        for (int index = 0; index < _table.GetLength(1); index++)
        {
            if (_table[index, line--].Equals(value, StringComparison.InvariantCultureIgnoreCase))
            {
                count++;
            }
        }


        return count >= 3;
    }
}