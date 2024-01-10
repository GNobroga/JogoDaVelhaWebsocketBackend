namespace JogoVelha.Application.Hubs.Utils;

public class GamingTable
{
    public string Player1 { get; set; } = default!;

    public string Player2 { get; set; } = default!;

    public string Player1Value = default!;
    public string Player2Value = default!;

    private readonly string[,] _table = new string[,]
    {
        {string.Empty, string.Empty, string.Empty},
        {string.Empty, string.Empty, string.Empty},
        {string.Empty, string.Empty, string.Empty}
    };

    public string[] ArrayTable => [.._table];

    private string _currentTurnValue = null!;

    public string CurrentTurnValue => _currentTurnValue;

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

    public static string RandomPlayerTurn() => Random.Shared.Next(2) % 2 == 0 ? "O" : "X";
    
    public void ChangePlayerTurn() => _currentTurnValue = _currentTurnValue == "O" ? "X" : "O";
    
    public bool IsMarked(int row, int col) => _table[row, col] is not {};

    public void SetValue(int row, int col, string value) => _table[row, col] = value;
    
    
    public bool FindWinnerInTable(string value)
    {
        return HasColumnsMarked(_table, value) || HasRowsMarked(_table, value) || HasMarkedInDiagonals(_table, value);
    }

    public bool IsFullMarked() => ArrayTable.All(v => v != string.Empty);
    

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